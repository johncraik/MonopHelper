using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopHelper.Helpers;
using MonopHelper.Helpers.GameDefaults;
using MonopHelper.Models.GameDb.Cards;
using MonopolyCL.Models.Cards;

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
        if (!_cardSet.Query().Any()) return new List<Card>();
        return await _cardSet.Query().Include(c => c.CardType)
            .Include(c => c.CardDeck)
            .Where(c => c.DeckId == deckId 
                         && (undefined ? c.CardType.TenantId >= 0 : c.CardType.TenantId != 0)
                         && (undefined ? c.CardDeck.TenantId >= 0 : c.CardDeck.TenantId != 0))
            .OrderBy(c => c.CardType.Name).ThenBy(c => c.Text).ToListAsync();
    }

    public async Task<bool> MoveCardsInDeck(int deckId, int newDeckId)
    {
        var deck = await FindCardDeck(newDeckId);
        if (deck == null) return false;
        
        var cards = await GetCardsFromDeck(deckId, true);
        foreach (var card in cards)
        {
            card.DeckId = newDeckId;
        }

        await UpdateCard(cards);
        return true;
    }
    
    public async Task<bool> CopyCardsInDeck(int deckId, int newDeckId)
    {
        var deck = await FindCardDeck(newDeckId);
        if (deck == null) return false;
        
        var cards = await GetCardsFromDeck(deckId, true);
        var newCards = new List<Card>();
        foreach (var newCard in cards.Select(card => new Card
                 {
                     TenantId = card.TenantId,
                     CardTypeId = card.CardTypeId,
                     Text = card.Text,
                     Cost = card.Cost,
                     IsDeleted = card.IsDeleted,
                     DeckId = newDeckId
                 }))
        {
            var valid = await ValidateCard(newCard);
            if(valid) newCards.Add(newCard);
        }

        await AddCard(newCards);
        return true;
    }

    public async Task<List<Card>> GetCardsFromType(int typeId, bool undefined = false)
    {
        if (!_cardSet.Query().Any()) return new List<Card>();
        return await _cardSet.Query().Include(c => c.CardType)
            .Include(c => c.CardDeck)
            .Where(c => c.CardTypeId == typeId 
                        && (undefined ? c.CardType.TenantId >= 0 : c.CardType.TenantId != 0)
                        && (undefined ? c.CardDeck.TenantId >= 0 : c.CardDeck.TenantId != 0))
            .OrderBy(c => c.CardType.Name).ThenBy(c => c.Text).ToListAsync();
    }

    public async Task<List<CardDeck>> GetCardDecks(bool undefined = false)
    {
        if (!_deckSet.Query().Any()) return new List<CardDeck>();
        return await _deckSet.Query().Where(d => (undefined ? d.TenantId >= 0 : d.TenantId != 0))
            .OrderByDescending(t => t.TenantId).ThenBy(d => d.DiffRating).ToListAsync();
    }

    public async Task<List<CardType>> GetCardTypes(bool undefined = false)
    {
        if (!_typeSet.Query().Any()) return new List<CardType>();
        return await _typeSet.Query().Where(t => (undefined ? t.TenantId >= 0 : t.TenantId != 0))
            .OrderByDescending(t => t.TenantId).ThenBy(t => t.Name).ToListAsync();
    }

    
    public async Task<Card?> FindCard(int id, bool undefined = true) => 
        await _cardSet.Query().Include(c => c.CardType).Include(c => c.CardDeck)
            .FirstOrDefaultAsync(c => c.Id == id && (undefined ? c.CardType.TenantId >= 0 : c.CardType.TenantId != 0) 
                                                 && (undefined ? c.CardDeck.TenantId >= 0 : c.CardDeck.TenantId != 0));
    public async Task<bool> ValidateCard(Card card) => !await _cardSet.Query().AnyAsync(c => 
        c.DeckId == card.DeckId && c.Text == card.Text);
    
    public async Task<CardType?> FindCardType(int id) => 
        await _typeSet.Query().FirstOrDefaultAsync(t => t.Id == id);
    public async Task<bool> ValidateCardType(string txt) => !await _typeSet.Query().AnyAsync(t => t.Name == txt);
    public async Task<int> GetUndefinedTypeId() =>
        (await _typeSet.Query().FirstOrDefaultAsync(t => t.TenantId == CardDefaults.TenantId))?.Id ?? 0;
    
    
    public async Task<CardDeck?> FindCardDeck(int id) =>
        await _deckSet.Query().FirstOrDefaultAsync(t => t.Id == id);
    public async Task<double> ValidateCardDeck(double dr)
    {
        var existingRating = await _deckSet.Query().FirstOrDefaultAsync(d => Math.Abs(d.DiffRating - dr) < 0.1);
        if (existingRating == null) return dr;

        var newDr = dr - 0.1;
        var diff = existingRating.DiffRating - dr;
        if (diff < 0) newDr = dr + 0.1;

        return newDr;
    }
    public async Task<int> GetUndefinedDeckId() =>
        (await _deckSet.Query().FirstOrDefaultAsync(d => d.TenantId == CardDefaults.TenantId))?.Id ?? 0;
    
    
    public async Task AddCard(Card card) => await _cardSet.AddAsync(card);
    public async Task AddCard(List<Card> cards) => await _cardSet.AddAsync(cards);
    public async Task UpdateCard(Card card) => await _cardSet.UpdateAsync(card);
    public async Task UpdateCard(List<Card> cards) => await _cardSet.UpdateAsync(cards);

    public async Task AddCardType(CardType type) => await _typeSet.AddAsync(type);
    public async Task UpdateCardType(CardType type) => await _typeSet.UpdateAsync(type);
    
    public async Task AddCardDeck(CardDeck deck) => await _deckSet.AddAsync(deck);
    public async Task UpdateCardDeck(CardDeck deck) => await _deckSet.UpdateAsync(deck);

}