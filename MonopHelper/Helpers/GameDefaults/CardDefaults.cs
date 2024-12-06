using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopHelper.Models.GameDb.Cards;
using MonopolyCL.Data;
using MonopolyCL.Models.Cards;

namespace MonopHelper.Helpers.GameDefaults;

public class CardDefaults
{
    private readonly GameDbContext _context;

    public CardDefaults(GameDbContext context)
    {
        _context = context;
    }

    public static readonly int TenantId = 0;
    public static readonly string DefaultName = "Undefined";

    public async Task EnsureDefaults()
    {
        /*
         * Type Default:
         */
        var typeExists = await _context.CardTypes.Where(t => t.TenantId == TenantId).ToListAsync();
        if (typeExists.Count > 0)
        {
            //Update name and deleted:
            var unDefinedTypes = typeExists.Where(t => t.Name == DefaultName).ToList();
            if (unDefinedTypes.Count > 0)
            {
                var type = unDefinedTypes.FirstOrDefault();
                if (type != null)
                {
                    type.Name = DefaultName;
                    type.IsDeleted = false;

                    _context.CardTypes.Update(type);
                    _context.CardTypes.RemoveRange(unDefinedTypes.Where(t => t.Id != type.Id).ToList());
                    await _context.SaveChangesAsync();
                }
                else
                {
                    await CreateTypeDefault(DefaultName);
                }
            }
            else
            {
                await CreateTypeDefault(DefaultName);
            }

            var chance = typeExists.FirstOrDefault(t => t.Name == "Chance");
            if (chance == null) await CreateTypeDefault("Chance");

            var comChest = typeExists.FirstOrDefault(t => t.Name == "Community Chest");
            if (comChest == null) await CreateTypeDefault("Community Chest");
        }
        else
        {
            await CreateTypeDefault(DefaultName);
            await CreateTypeDefault("Chance");
            await CreateTypeDefault("Community Chest");
        }
        
        /*
         * Deck Default:
         */
        var deckExists = await _context.CardDecks.Where(t => t.TenantId == TenantId).ToListAsync();
        if (deckExists.Count > 0)
        {
            //Update name and deleted:
            var deck = deckExists.FirstOrDefault();
            if (deck != null)
            {
                deck.Name = DefaultName;
                deck.DiffRating = 0;
                deck.IsDeleted = false;

                _context.CardDecks.Update(deck);
                _context.CardDecks.RemoveRange(deckExists.Where(d => d.Id != deck.Id).ToList());
                await _context.SaveChangesAsync();
            }
            else
            {
                await CreateDeckDefault();
            }
        }
        else
        {
            await CreateDeckDefault();
        }
    }

    private async Task CreateTypeDefault(string name)
    {
        var type = new CardType
        {
            TenantId = TenantId,
            Name = name,
            IsDeleted = false
        };
        await _context.CardTypes.AddAsync(type);
        await _context.SaveChangesAsync();
    }
    private async Task CreateDeckDefault()
    {
        var deck = new CardDeck
        {
            TenantId = TenantId,
            Name = DefaultName,
            DiffRating = 0,
            IsDeleted = false
        };
        await _context.CardDecks.AddAsync(deck);
        await _context.SaveChangesAsync();
    }


    public async Task<(FileStream? file, CardType? type, CardDeck? deck)> OpenCardsFile(string name)
    {
        var type = await _context.CardTypes.FirstOrDefaultAsync(t => t.Name == name && t.TenantId == TenantId);
        if (type == null) return (null, null, null);

        var deck = await _context.CardDecks.FirstOrDefaultAsync(d => d.TenantId == TenantId && d.Name == DefaultName);
        if (deck == null) return (null, null, null);

        try
        {
            var file = File.OpenRead($"{Environment.CurrentDirectory}/Helpers/GameDefaults/CSVs/{name}.csv");
            return (file, type, deck);;
        }
        catch (Exception ex)
        {
            return (null, null, null);
        }
    }
}