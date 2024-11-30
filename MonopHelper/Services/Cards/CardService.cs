using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
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

    public async Task Test() => await _cardSet.Test();

    public async Task<List<Card>> GetCardsFromDeck(int deckId)
    {
        if (!_cardSet.Qry.Any()) return new List<Card>();
        return await _cardSet.Qry.Include(c => c.CardType)
            .Where(c => !c.IsDeleted && c.DeckId == deckId)
            .OrderBy(c => c.CardType.Name).ThenBy(c => c.Text).ToListAsync();
    }

    public async Task<List<CardDeck>> GetCardDecks()
    {
        if (!_deckSet.Qry.Any()) return new List<CardDeck>();
        return await _deckSet.Qry.Where(d => !d.IsDeleted)
            .OrderBy(d => d.DiffRating).ToListAsync();
    }

    public async Task<List<CardType>> GetCardTypes()
    {
        if (!_typeSet.Qry.Any()) return new List<CardType>();
        return await _typeSet.Qry.Where(t => !t.IsDeleted)
            .OrderBy(t => t.Name).ToListAsync();
    }

    
    public async Task<Card?> FindCard(int id) => await _cardSet.Qry.FirstOrDefaultAsync(t => t.Id == id);
    public async Task<CardType?> FindCardType(int id) => 
        await _typeSet.Qry.FirstOrDefaultAsync(t => t.Id == id);
    public async Task<CardDeck?> FindCardDeck(int id) =>
        await _deckSet.Qry.FirstOrDefaultAsync(t => t.Id == id);

    public async Task AddCard(Card card) => await _cardSet.AddAsync(card);
    public async Task UpdateCard(Card card) => await _cardSet.UpdateAsync(card);

    public async Task AddCardType(CardType type) => await _typeSet.AddAsync(type);
    public async Task UpdateCardType(CardType type) => await _typeSet.UpdateAsync(type);
    
    public async Task AddCardDeck(CardDeck deck) => await _deckSet.AddAsync(deck);
    public async Task UpdateCardDeck(CardDeck deck) => await _deckSet.UpdateAsync(deck);

}