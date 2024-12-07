using Microsoft.AspNetCore.Mvc;
using MonopHelper.Authentication;
using MonopHelper.Helpers.GameDefaults;
using MonopHelper.Models.GameDb.Cards;
using MonopHelper.Services.Cards;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Cards.Defaults;

namespace MonopHelper.Controllers;

public class CardController : Controller
{
    private readonly CardService _cardService;
    private readonly UserInfo _userInfo;
    private readonly CardGameService _cardGameService;

    public CardController(CardService cardService, UserInfo userInfo, CardGameService cardGameService)
    {
        _cardService = cardService;
        _userInfo = userInfo;
        _cardGameService = cardGameService;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    public async Task<IActionResult> CardsTablePartial(int id)
    {
        var model = await _cardService.GetCardsFromDeck(id, true);
        return PartialView("Cards/_CardsTable", model);
    }

    [HttpPost]
    public async Task<bool> RemoveCard(int id)
    {
        var card = await _cardService.FindCard(id, monop: false);
        if (card == null) return false;

        card.IsDeleted = true;
        await _cardService.UpdateCard(card);
        return true;
    }

    public async Task<IActionResult> AddCardsPartial(int deckId)
    {
        var model = await _cardService.GetCardTypes();
        var deck = await _cardService.FindCardDeck(deckId);
        
        return PartialView("Cards/_AddCardTypes", (model, deck != null));
    }
    
    public async Task<IActionResult> CardTypesPartial()
    {
        var model = await _cardService.GetCardTypes(monop: false);
        return PartialView("Cards/_CardTypesTable", model);
    }

    [HttpPost]
    public async Task<bool> AddCardType(string name)
    {
        var valid = await _cardService.ValidateCardType(name);
        if (!valid) return valid;

        var cardType = new CardType
        {
            TenantId = _userInfo.TenantId,
            Name = name,
            IsDeleted = false
        };

        await _cardService.AddCardType(cardType);
        return true;
    }

    [HttpPost]
    public async Task<bool> EditCardType(int id, string name)
    {
        var cardType = await _cardService.FindCardType(id);
        if (cardType == null) return false;

        cardType.Name = name;
        await _cardService.UpdateCardType(cardType);
        return true;
    }

    [HttpPost]
    public async Task<bool> RemoveCardType(int id)
    {
        var cardType = await _cardService.FindCardType(id);
        if (cardType == null) return false;

        //Sets cards to undefined that have the type being deleted:
        var cardsWithType = await _cardService.GetCardsFromType(id);
        foreach (var card in cardsWithType)
        {
            card.CardTypeId = await _cardService.GetUndefinedTypeId();
        }
        await _cardService.UpdateCard(cardsWithType);

        cardType.IsDeleted = true;
        await _cardService.UpdateCardType(cardType);
        return true;
    }
    
    
    public async Task<IActionResult> CardDecksPartial()
    {
        var model = await _cardService.GetCardDecks(monop: false);
        return PartialView("Cards/_CardDecksTable", model);
    }

    [HttpPost]
    public async Task<bool> RemoveCardDeck(int id)
    {
        var cardDeck = await _cardService.FindCardDeck(id);
        if (cardDeck == null) return false;

        //Sets cards to undefined that have the type being deleted:
        var cardsInDeck = await _cardService.GetCardsFromDeck(id);
        foreach (var card in cardsInDeck)
        {
            card.DeckId = await _cardService.GetUndefinedDeckId();
        }
        await _cardService.UpdateCard(cardsInDeck);
        
        //Deletes games that have that deck:
        

        cardDeck.IsDeleted = true;
        await _cardService.UpdateCardDeck(cardDeck);
        return true;
    }


    public async Task<IActionResult> CardGamesPartial()
    {
        var model = await _cardGameService.GetCardGames();
        return PartialView("Cards/_CardGameTable", model);
    }
    
    [HttpPost]
    public async Task<bool> RemoveCardGame(int id)
    {
        return await _cardGameService.DeleteCardGame(id);
    }
}