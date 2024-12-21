using Microsoft.Extensions.Logging;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Models.Cards;

namespace MonopolyCL.Data;

public class CardContext
{
    public GameDbSet<Card> CardSet { get; init; }
    public GameDbSet<CardType> CardTypeSet { get; init; }
    public GameDbSet<CardDeck> CardDeckSet { get; init; }
    public GameDbSet<CardGame> CardGameSet { get; init; }
    public GameDbSet<CardToGame> CardToGameSet { get; init; }
    public GameDbSet<TypeToGame> TypeToGameSet { get; init; }
    
    public CardContext(GameDbSet<Card> cardSet,
        GameDbSet<CardType> cardTypeSet,
        GameDbSet<CardDeck> cardDeckSet,
        GameDbSet<CardGame> cardGameSet,
        GameDbSet<CardToGame> cardToGameSet,
        GameDbSet<TypeToGame> typeToGameSet)
    {
        CardSet = cardSet;
        CardTypeSet = cardTypeSet;
        CardDeckSet = cardDeckSet;
        CardGameSet = cardGameSet;
        CardToGameSet = cardToGameSet;
        TypeToGameSet = typeToGameSet;
    }
}