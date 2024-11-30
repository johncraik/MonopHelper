using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopHelper.Helpers.GameDefaults;
using MonopHelper.Models.GameDb.Cards;

namespace MonopHelper.Services.Cards;

public class CardService
{
    private readonly GameDbSet<Card> _cardSet;
    private readonly GameDbSet<CardType> _typeSet;
    private readonly GameDbSet<CardDeck> _deckSet;
    private readonly UserInfo _userInfo;

    public CardService(GameDbSet<Card> cardSet,
        GameDbSet<CardType> typeSet,
        GameDbSet<CardDeck> deckSet,
        UserInfo userInfo)
    {
        _cardSet = cardSet;
        _typeSet = typeSet;
        _deckSet = deckSet;
        _userInfo = userInfo;
    }

    public async Task<List<Card>> GetCardsFromDeck(int deckId, bool undefined = false)
    {
        if (!_cardSet.Qry.Any()) return new List<Card>();
        return await _cardSet.Qry.Include(c => c.CardType)
            .Include(c => c.CardDeck)
            .Where(c => !c.IsDeleted && c.DeckId == deckId 
                         && (undefined ? c.CardType.TenantId >= 0 : c.CardType.TenantId != 0)
                         && (undefined ? c.CardDeck.TenantId >= 0 : c.CardDeck.TenantId != 0))
            .OrderBy(c => c.CardType.Name).ThenBy(c => c.Text).ToListAsync();
    }

    public async Task<List<Card>> GetCardsFromType(int typeId, bool undefined = false)
    {
        if (!_cardSet.Qry.Any()) return new List<Card>();
        return await _cardSet.Qry.Include(c => c.CardType)
            .Include(c => c.CardDeck)
            .Where(c => !c.IsDeleted && c.CardTypeId == typeId 
                        && (undefined ? c.CardType.TenantId >= 0 : c.CardType.TenantId != 0)
                        && (undefined ? c.CardDeck.TenantId >= 0 : c.CardDeck.TenantId != 0))
            .OrderBy(c => c.CardType.Name).ThenBy(c => c.Text).ToListAsync();
    }

    public async Task<List<CardDeck>> GetCardDecks(bool undefined = false)
    {
        if (!_deckSet.Qry.Any()) return new List<CardDeck>();
        return await _deckSet.Qry.Where(d => !d.IsDeleted && (undefined ? d.TenantId >= 0 : d.TenantId != 0))
            .OrderByDescending(t => t.TenantId).ThenBy(d => d.DiffRating).ToListAsync();
    }

    public async Task<List<CardType>> GetCardTypes(bool undefined = false)
    {
        if (!_typeSet.Qry.Any()) return new List<CardType>();
        return await _typeSet.Qry.Where(t => !t.IsDeleted && (undefined ? t.TenantId >= 0 : t.TenantId != 0))
            .OrderByDescending(t => t.TenantId).ThenBy(t => t.Name).ToListAsync();
    }

    
    public async Task<Card?> FindCard(int id) => await _cardSet.Qry.FirstOrDefaultAsync(t => t.Id == id);
    public async Task<bool> ValidateCard(string txt) => !await _cardSet.Qry.AnyAsync(c => c.Text == txt);
    
    public async Task<CardType?> FindCardType(int id) => 
        await _typeSet.Qry.FirstOrDefaultAsync(t => t.Id == id);
    public async Task<bool> ValidateCardType(string txt) => !await _typeSet.Qry.AnyAsync(t => t.Name == txt);
    public async Task<int> GetUndefinedTypeId() =>
        (await _typeSet.Qry.FirstOrDefaultAsync(t => t.TenantId == CardDefaults.TenantId))?.Id ?? 0;
    
    
    public async Task<CardDeck?> FindCardDeck(int id) =>
        await _deckSet.Qry.FirstOrDefaultAsync(t => t.Id == id);
    public async Task<double> ValidateCardDeck(double dr)
    {
        var existingRating = await _deckSet.Qry.FirstOrDefaultAsync(d => Math.Abs(d.DiffRating - dr) < 0.1);
        if (existingRating == null) return dr;

        var newDr = dr - 0.1;
        var diff = existingRating.DiffRating - dr;
        if (diff < 0) newDr = dr + 0.1;

        return newDr;
    }
    public async Task<int> GetUndefinedDeckId() =>
        (await _deckSet.Qry.FirstOrDefaultAsync(d => d.TenantId == CardDefaults.TenantId))?.Id ?? 0;

    
    public async Task AddCard(Card card) => await _cardSet.AddAsync(card);
    public async Task UpdateCard(Card card) => await _cardSet.UpdateAsync(card);
    public async Task UpdateCard(List<Card> cards) => await _cardSet.UpdateAsync(cards);

    public async Task AddCardType(CardType type) => await _typeSet.AddAsync(type);
    public async Task UpdateCardType(CardType type) => await _typeSet.UpdateAsync(type);
    
    public async Task AddCardDeck(CardDeck deck) => await _deckSet.AddAsync(deck);
    public async Task UpdateCardDeck(CardDeck deck) => await _deckSet.UpdateAsync(deck);

}