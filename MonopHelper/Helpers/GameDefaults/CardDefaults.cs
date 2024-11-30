using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopHelper.Models.GameDb.Cards;

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
            var type = typeExists.FirstOrDefault();
            if (type != null)
            {
                type.Name = DefaultName;
                type.IsDeleted = false;

                _context.CardTypes.Update(type);
                _context.CardTypes.RemoveRange(typeExists.Where(t => t.Id != type.Id).ToList());
                await _context.SaveChangesAsync();
            }
            else
            {
                await CreateTypeDefault();
            }
        }
        else
        {
            await CreateTypeDefault();
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

    private async Task CreateTypeDefault()
    {
        var type = new CardType
        {
            TenantId = TenantId,
            Name = DefaultName,
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
}