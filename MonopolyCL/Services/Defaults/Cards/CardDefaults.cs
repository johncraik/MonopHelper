using Microsoft.EntityFrameworkCore;
using MonopolyCL.Data;
using MonopolyCL.Dictionaries;
using MonopolyCL.Models.Cards;

namespace MonopolyCL.Services.Defaults.Cards;

public enum CardDefaultsCheck{
    All,
    Types,
    Decks
}

public class CardDefaults
{
    private readonly GameDbContext _context;
    private readonly CsvReader<CardUpload> _csvReader;

    public CardDefaults(GameDbContext context, CsvReader<CardUpload> csvReader)
    {
        _context = context;
        _csvReader = csvReader;
    }

    public async Task EnsureDefaults()
    {
        //Create undefined type and deck:
        await EnsureDefaults(DefaultsDictionary.DefaultTenant, DefaultsDictionary.Undefined, 
            DefaultsDictionary.UndefinedColour, CardDefaultsCheck.All);
        
        //Create monopoly chance and com chest:
        await EnsureDefaults(DefaultsDictionary.MonopolyTenant, CardDefaultsDictionary.Chance, 
            CardDefaultsDictionary.ChanceColour, CardDefaultsCheck.Types);
        await EnsureDefaults(DefaultsDictionary.MonopolyTenant, CardDefaultsDictionary.ComChest, 
            CardDefaultsDictionary.ComChestColour, CardDefaultsCheck.Types);
        
        //Create monopoly deck:
        await EnsureDefaults(DefaultsDictionary.MonopolyTenant, CardDefaultsDictionary.StandardDeck, "",
            CardDefaultsCheck.Decks);
        
        //Check chance and com chest cards exist:
        await EnsureCards(CardDefaultsDictionary.Chance);
        await EnsureCards(CardDefaultsDictionary.ComChest);
    }

    private async Task EnsureDefaults(int id, string name, string colour, CardDefaultsCheck check)
    {
        switch (check)
        {
            case CardDefaultsCheck.Types:
                await CheckTypes(id, name, colour);
                break;
            case CardDefaultsCheck.Decks:
                await CheckDecks(id, name);
                break;
            case CardDefaultsCheck.All:
            default:
                await CheckTypes(id, name, colour);
                await CheckDecks(id, name);
                break;
        }
    }

    private async Task CheckTypes(int id, string name, string colour)
    {
        var defTypes = await _context.CardTypes.Where(t => t.TenantId == id && t.Name == name).ToListAsync();
        if (defTypes.Count != 1)
        {
            switch (defTypes.Count)
            {
                case 0:
                    await CreateType(id, name, colour);
                    break;
                default:
                    _context.CardTypes.RemoveRange(defTypes.Where(t =>
                        t.Id != (defTypes.FirstOrDefault()?.Id)));
                    await _context.SaveChangesAsync();
                    break;
            }
        }
    }

    private async Task CheckDecks(int id, string name)
    {
        var defDecks = await _context.CardDecks.Where(d => d.TenantId == id && d.Name == name).ToListAsync();
        if (defDecks.Count != 1)
        {
            switch (defDecks.Count)
            {
                case 0:
                    await CreateDeck(id, name);
                    break;
                default:
                    _context.CardDecks.RemoveRange(defDecks.Where(t =>
                        t.Id != (defDecks.FirstOrDefault()?.Id)));
                    await _context.SaveChangesAsync();
                    break;
            }
        }
    }

    private async Task EnsureCards(string name)
    {
        var type = await _context.CardTypes.FirstOrDefaultAsync(t => t.TenantId == DefaultsDictionary.MonopolyTenant
                                                                     && t.Name == name);
        var deck = await _context.CardDecks.FirstOrDefaultAsync(d => d.TenantId == DefaultsDictionary.MonopolyTenant
                                                                     && d.Name == CardDefaultsDictionary.StandardDeck);
        if(type == null || deck == null) return;

        //Get existing cards:
        var existingCards = await _context.Cards.Where(c => c.TenantId == DefaultsDictionary.MonopolyTenant
                                                            && c.CardTypeId == type.Id
                                                            && c.DeckId == deck.Id).ToListAsync();
        if (existingCards.Count == 0)
        {
            var file = File.OpenRead($"{Environment.CurrentDirectory}/../MonopolyCL/Services/Defaults/Cards/{name}.csv");
            var records = _csvReader.UploadFile(file);
            var cards = new List<Card>();
            foreach (var r in records!.ToList())
            {
                //Parse cost:
                int? cost = null;
                var success = int.TryParse(r.Cost, out var parsedCost);
                if (success) cost = parsedCost;
                    
                //Create card:
                cards.Add(new Card
                {
                    TenantId = DefaultsDictionary.MonopolyTenant,
                    CardTypeId = type.Id,
                    DeckId = deck.Id,
                    Text = r.Text,
                    Cost = cost
                });
            }
            
            await _context.Cards.AddRangeAsync(cards);
            await _context.SaveChangesAsync();
        }
        
    }

    private async Task CreateType(int id, string name, string colour)
    {
        var type = new CardType
        {
            TenantId = id,
            Name = name,
            Colour = colour,
            IsDeleted = false
        };
        await _context.CardTypes.AddAsync(type);
        await _context.SaveChangesAsync();
    }
    private async Task CreateDeck(int id, string name)
    {
        var deck = new CardDeck
        {
            TenantId = id,
            Name = name,
            IsDeleted = false
        };
        await _context.CardDecks.AddAsync(deck);
        await _context.SaveChangesAsync();
    }
}