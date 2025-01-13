using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Data;
using MonopolyCL.Dictionaries;
using MonopolyCL.Models.Cards;
using MonopolyCL.Services.Cards;

namespace MonopHelper.Services.Cards;

public class CardService
{
    private readonly CardContext _cardContext;
    private readonly UserInfo _userInfo;
    private readonly CardActionsService _cardActionsService;

    public CardService(CardContext cardContext,
        UserInfo userInfo,
        CardActionsService cardActionsService)
    {
        _cardContext = cardContext;
        _userInfo = userInfo;
        _cardActionsService = cardActionsService;
    }

    public async Task<List<Card>> GetCardsFromDeck(int deckId, bool undefined = false)
    {
        if (!_cardContext.CardSet.Query().Any()) return new List<Card>();
        return await _cardContext.CardSet.Query().Include(c => c.CardType)
            .Include(c => c.CardDeck)
            .Where(c => c.DeckId == deckId 
                        && (undefined || c.CardType.TenantId != DefaultsDictionary.DefaultTenant)
                        && (undefined || c.CardDeck.TenantId != DefaultsDictionary.DefaultTenant))
            .OrderBy(c => c.CardType.Id).ThenBy(c => c.Text).ToListAsync();
    }

    public async Task<List<(Card, bool)>> GetCardsAndActionFromDeck(int deckId, bool undefined = false)
    {
        var cards = await GetCardsFromDeck(deckId, undefined);
        var rtn = new List<(Card, bool)>();
        foreach (var card in cards)
        {
            var action = await _cardActionsService.GetCardAction(card.Id);
            rtn.Add((card, action is { Item1: not null, Item2: not null }));
        }

        return rtn;
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
        if (!_cardContext.CardSet.Query().Any()) return new List<Card>();
        return await _cardContext.CardSet.Query().Include(c => c.CardType)
            .Include(c => c.CardDeck)
            .Where(c => c.CardTypeId == typeId 
                        && (undefined || c.CardType.TenantId != DefaultsDictionary.DefaultTenant)
                        && (undefined || c.CardDeck.TenantId != DefaultsDictionary.DefaultTenant))
            .OrderBy(c => c.CardType.Name).ThenBy(c => c.Text).ToListAsync();
    }

    public async Task<List<CardDeck>> GetCardDecks(bool undefined = false, bool monop = true)
    {
        if (!_cardContext.CardDeckSet.Query().Any()) return new List<CardDeck>();
        return await _cardContext.CardDeckSet.Query().Where(d => (undefined || d.TenantId != DefaultsDictionary.DefaultTenant) 
                                                                 && (monop || d.TenantId != DefaultsDictionary.MonopolyTenant))
            .OrderByDescending(t => t.TenantId).ThenBy(d => d.DiffRating).ToListAsync();
    }

    public async Task<List<CardType>> GetCardTypes(bool undefined = false, bool monop = true)
    {
        if (!_cardContext.CardTypeSet.Query().Any()) return new List<CardType>();
        return await _cardContext.CardTypeSet.Query().Where(t => (undefined || t.TenantId != DefaultsDictionary.DefaultTenant) 
                                                                 && (monop || t.TenantId != DefaultsDictionary.MonopolyTenant))
            .OrderByDescending(t => t.TenantId).ThenBy(t => t.Name).ToListAsync();
    }

    
    public async Task<Card?> FindCard(int id, bool undefined = true, bool monop = true) => 
        await _cardContext.CardSet.Query().Include(c => c.CardType).Include(c => c.CardDeck)
            .FirstOrDefaultAsync(c => c.Id == id && (undefined || c.CardType.TenantId != DefaultsDictionary.DefaultTenant)
                                                 && (undefined || c.CardDeck.TenantId != DefaultsDictionary.DefaultTenant)
                                                 && (monop || c.CardDeck.TenantId != DefaultsDictionary.MonopolyTenant));
    public async Task<bool> ValidateCard(Card card) => !await _cardContext.CardSet.Query().AnyAsync(c => 
        c.DeckId == card.DeckId && c.Text == card.Text);
    
    public async Task<CardType?> FindCardType(int id, bool monop = false) => await _cardContext.CardTypeSet.Query()
        .Where(t => monop || t.TenantId != DefaultsDictionary.MonopolyTenant).FirstOrDefaultAsync(t => t.Id == id);
    public async Task<bool> ValidateCardType(string txt) => !await _cardContext.CardTypeSet.Query().AnyAsync(t => t.Name == txt);
    public async Task<int> GetUndefinedTypeId() =>
        (await _cardContext.CardTypeSet.Query().FirstOrDefaultAsync(t => t.TenantId == DefaultsDictionary.DefaultTenant))?.Id ?? 0;
    
    
    public async Task<CardDeck?> FindCardDeck(int id, bool monop = false) => await _cardContext.CardDeckSet.Query()
        .Where(d => monop || d.TenantId != DefaultsDictionary.MonopolyTenant).FirstOrDefaultAsync(t => t.Id == id);
    public async Task<double> ValidateCardDeck(double dr)
    {
        var existingRating = await _cardContext.CardDeckSet.Query().FirstOrDefaultAsync(d => Math.Abs(d.DiffRating - dr) < 0.1);
        if (existingRating == null) return dr;

        var newDr = dr - 0.1;
        var diff = existingRating.DiffRating - dr;
        if (diff < 0) newDr = dr + 0.1;

        return newDr;
    }
    public async Task<int> GetUndefinedDeckId() =>
        (await _cardContext.CardDeckSet.Query().FirstOrDefaultAsync(d => d.TenantId == DefaultsDictionary.DefaultTenant))?.Id ?? 0;
    
    
    public async Task AddCard(Card card) => await _cardContext.CardSet.AddAsync(card);
    public async Task AddCard(List<Card> cards) => await _cardContext.CardSet.AddAsync(cards);
    public async Task UpdateCard(Card card) => await _cardContext.CardSet.UpdateAsync(card);
    public async Task UpdateCard(List<Card> cards) => await _cardContext.CardSet.UpdateAsync(cards);

    public async Task AddCardType(CardType type) => await _cardContext.CardTypeSet.AddAsync(type);
    public async Task UpdateCardType(CardType type) => await _cardContext.CardTypeSet.UpdateAsync(type);
    
    public async Task AddCardDeck(CardDeck deck) => await _cardContext.CardDeckSet.AddAsync(deck);
    public async Task UpdateCardDeck(CardDeck deck) => await _cardContext.CardDeckSet.UpdateAsync(deck);

}