using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopHelper.Models.GameDb.Cards;

namespace MonopHelper.Services.Cards;

public class CardService
{
    private readonly GameDbContext _context;
    private readonly UserInfo _userInfo;

    public CardService(GameDbContext context, UserInfo userInfo)
    {
        _context = context;
        _userInfo = userInfo;
    }

    public async Task<List<Card>> GetCardsFromDeck(int deckId)
    {
        return await _context.Cards.Include(c => c.CardType)
            .Where(c => c.CardType.TenantId == _userInfo.TenantId && !c.IsDeleted && c.DeckId == deckId)
            .OrderBy(c => c.CardType.Name).ThenBy(c => c.Text).ToListAsync();
    }

    public async Task<List<CardDeck>> GetCardDecks()
    {
        return await _context.CardDecks.Where(d => d.TenantId == _userInfo.TenantId && !d.IsDeleted)
            .OrderBy(d => d.DiffRating).ToListAsync();
    }

    public async Task<List<CardType>> GetCardTypes()
    {
        return await _context.CardTypes.Where(t => t.TenantId == _userInfo.TenantId && !t.IsDeleted)
            .OrderBy(t => t.Name).ToListAsync();
    }

    
    public async Task<Card?> FindCard(int id) => await _context.Cards.FirstOrDefaultAsync(t => t.Id == id);
    public async Task<CardType?> FindCardType(int id) => 
        await _context.CardTypes.FirstOrDefaultAsync(t => t.TenantId == _userInfo.TenantId && t.Id == id);
    public async Task<CardDeck?> FindCardDeck(int id) =>
        await _context.CardDecks.FirstOrDefaultAsync(t => t.TenantId == _userInfo.TenantId && t.Id == id);

    public async Task AddCard(Card card)
    {
        await _context.Cards.AddAsync(card);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCard(Card card)
    {
        _context.Cards.Update(card);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveCard(Card card)
    {
        _context.Cards.Remove(card);
        await _context.SaveChangesAsync();
    }
}