using Microsoft.Extensions.Logging;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Cards.CardActions;

namespace MonopolyCL.Data;

public class CardContext
{
    public GameDbSet<Card> CardSet { get; init; }
    public GameDbSet<CardType> CardTypeSet { get; init; }
    public GameDbSet<CardDeck> CardDeckSet { get; init; }
    public GameDbSet<CardGame> CardGameSet { get; init; }
    public GameDbSet<CardToGame> CardToGameSet { get; init; }
    public GameDbSet<TypeToGame> TypeToGameSet { get; init; }
    public GameDbSet<ActionConfig> CardActionSet { get; init; }
    
    public CardContext(GameDbContext context, UserInfo userInfo)
    {
        CardSet = new GameDbSet<Card>(context, userInfo);
        CardTypeSet = new GameDbSet<CardType>(context, userInfo);
        CardDeckSet = new GameDbSet<CardDeck>(context, userInfo);
        CardGameSet = new GameDbSet<CardGame>(context, userInfo);
        CardToGameSet = new GameDbSet<CardToGame>(context, userInfo);
        TypeToGameSet = new GameDbSet<TypeToGame>(context, userInfo);
        CardActionSet = new GameDbSet<ActionConfig>(context, userInfo);
    }
}