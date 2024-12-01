using System.Globalization;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopHelper.Models.GameDb.Cards;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using MonopHelper.Models.GameDb.Cards.ViewModels;

namespace MonopHelper.Services.Cards;

public class UploadCardsService
{
    private readonly GameDbSet<Card> _cardSet;
    private readonly CardService _cardService;
    private readonly ILogger<UploadCardsService> _logger;
    private readonly UserInfo _userInfo;

    public UploadCardsService(GameDbSet<Card> cardSet, CardService cardService, 
        ILogger<UploadCardsService> logger, UserInfo userInfo)
    {
        _cardSet = cardSet;
        _cardService = cardService;
        _logger = logger;
        _userInfo = userInfo;
    }

    private async Task<bool> ValidateFile(IFormFile file)
    {
        return file.ContentType == "text/csv";
    }

    public async Task<bool> UploadFile(IFormFile file, int typeId, int deckId)
    {
        //Validate file:
        var valid = await ValidateFile(file);
        if (!valid) return valid;

        //Get card type:
        var cardType = await _cardService.FindCardType(typeId);
        if (cardType == null) return false;
        
        //Get card deck:
        var deck = await _cardService.FindCardDeck(deckId);
        if (deck == null) return false;
        
        //Convert to cards:
        try
        {
            //Read file:
            using var reader = new StreamReader(file.OpenReadStream());
            using var cvsReader = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = cvsReader.GetRecords<CardUpload>();
            
            var cards = new List<Card>();
            foreach (var record in records)
            {
                //Parse cost:
                int? cost = null;
                var success = int.TryParse(record.Cost, out var parsedCost);
                if (success) cost = parsedCost;
                
                //Create card:
                cards.Add(new Card
                {
                    TenantId = _userInfo.TenantId,
                    CardTypeId = cardType.Id,
                    DeckId = deck.Id,
                    Text = record.Text,
                    Cost = cost
                });
            }

            //Add cards to db:
            await _cardSet.AddAsync(cards);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Unable to convert CSV file to cards!");
            return false;
        }
    }
    
    public async Task<bool> UploadFile(FileStream file, CardType? cardType, CardDeck? deck)
    {
        if (cardType == null || deck == null) return false;
        
        //Convert to cards:
        try
        {
            //Read file:
            using var reader = new StreamReader(file);
            using var cvsReader = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = cvsReader.GetRecords<CardUpload>();
            
            var cards = new List<Card>();
            foreach (var record in records)
            {
                //Parse cost:
                int? cost = null;
                var success = int.TryParse(record.Cost, out var parsedCost);
                if (success) cost = parsedCost;
                
                //Create card:
                var card = new Card
                {
                    TenantId = 0,
                    CardTypeId = cardType.Id,
                    DeckId = deck.Id,
                    Text = record.Text,
                    Cost = cost
                };

                if (await _cardSet.Query().FirstOrDefaultAsync(c => 
                        c.Text == card.Text && c.TenantId == card.TenantId && c.CardTypeId == card.CardTypeId) == null)
                {
                    cards.Add(card);
                }
            }

            //Add cards to db:
            await _cardSet.AddAsync(cards);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Unable to convert CSV file to cards!");
            return false;
        }
    }
}