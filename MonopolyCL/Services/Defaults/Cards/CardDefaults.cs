using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Cards.Defaults;

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
            CardDefaultsCheck.All);
        
        //Create monopoly chance and com chest:
        await EnsureDefaults(DefaultsDictionary.MonopolyTenant, CardDefaultsDictionary.Chance, 
            CardDefaultsCheck.Types);
        await EnsureDefaults(DefaultsDictionary.MonopolyTenant, CardDefaultsDictionary.ComChest, 
            CardDefaultsCheck.Types);
        
        //Create monopoly deck:
        await EnsureDefaults(DefaultsDictionary.MonopolyTenant, CardDefaultsDictionary.StandardDeck,
            CardDefaultsCheck.Decks);
        
        //Check chance and com chest cards exist:
        await EnsureCards(CardDefaultsDictionary.Chance);
        await EnsureCards(CardDefaultsDictionary.ComChest);
    }

    private async Task EnsureDefaults(int id, string name, CardDefaultsCheck check)
    {
        switch (check)
        {
            case CardDefaultsCheck.Types:
                await CheckTypes(id, name);
                break;
            case CardDefaultsCheck.Decks:
                await CheckDecks(id, name);
                break;
            case CardDefaultsCheck.All:
            default:
                await CheckTypes(id, name);
                await CheckDecks(id, name);
                break;
        }
    }

    private async Task CheckTypes(int id, string name)
    {
        var defTypes = await _context.CardTypes.Where(t => t.TenantId == id && t.Name == name).ToListAsync();
        if (defTypes.Count != 1)
        {
            switch (defTypes.Count)
            {
                case 0:
                    await CreateType(id, name);
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
        var type = await _context.CardTypes.FirstOrDefaultAsync(t => t.Id == DefaultsDictionary.MonopolyTenant
                                                                     && t.Name == name);
        var deck = await _context.CardDecks.FirstOrDefaultAsync(d => d.Id == DefaultsDictionary.MonopolyTenant
                                                                     && d.Name == CardDefaultsDictionary.StandardDeck);
        if(type == null || deck == null) return;

        var file = File.OpenRead($"{Environment.CurrentDirectory}/Services/Defaults/Cards/{name}.csv");
        var records = _csvReader.UploadFile(file);
        var cards = new List<Card>();
        foreach (var r in records!)
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

        //Get existing cards:
        var existingCards = await _context.Cards.Where(c => c.TenantId == DefaultsDictionary.DefaultTenant
                                                            && c.CardTypeId == type.Id
                                                            && c.DeckId == deck.Id).ToListAsync();
        foreach (var ec in existingCards.Where(c => c.IsDeleted))
        {
            //Ensure none are deleted:
            ec.IsDeleted = false;
        }

        //Get list of cards to add:
        var addCards = existingCards.Any(c => !cards.Select(ac => new
            {
                ac.TenantId,
                ac.CardTypeId,
                ac.DeckId,
                ac.Text,
                ac.Cost,
                ac.IsDeleted
            }).Contains(new
                {
                    c.TenantId,
                    c.CardTypeId,
                    c.DeckId,
                    c.Text,
                    c.Cost,
                    c.IsDeleted
                }));

        if (addCards)
        {
            _context.Cards.RemoveRange(existingCards);
            await _context.Cards.AddRangeAsync(cards);
            await _context.SaveChangesAsync();
        }
        else
        {
            _context.Update(existingCards);
            await _context.SaveChangesAsync();
        }
    }

    private async Task CreateType(int id, string name)
    {
        var type = new CardType
        {
            TenantId = id,
            Name = name,
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