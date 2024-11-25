using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopHelper.Models.GameDb.Cards;

namespace MonopHelper.Services.Cards;

public class CardService
{
    private readonly GameDbContext _context;

    public CardService(GameDbContext context)
    {
        _context = context;
    }

    public async Task<List<Card>> GetCardsFromDeck(int deckId)
    {
        return await _context.Cards.Include(c => c.CardType)
            .Where(c => !c.IsDeleted && c.DeckId == deckId).OrderBy(c => c.CardType.Name).ThenBy(c => c.Text)
            .ToListAsync();
    }

    public async Task<List<CardDeck>> GetCardDecks()
    {
        return await _context.CardDecks.Where(d => !d.IsDeleted)
            .OrderBy(d => d.DiffRating).ToListAsync();
    }
}