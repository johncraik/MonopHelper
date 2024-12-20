using Microsoft.EntityFrameworkCore;
using MonopolyCL.Models.Boards.DataModel;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Cards.Actions;
using MonopolyCL.Models.Cards.Game;
using MonopolyCL.Models.Game;
using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Models.Properties.DataModel;

namespace MonopolyCL.Data;

public class GameDbContext : DbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> options)
        : base(options)
    {
    }

    #region Cards

    public DbSet<Card> Cards { get; set; }
    public DbSet<CardType> CardTypes { get; set; }
    public DbSet<CardDeck> CardDecks { get; set; }
    
    public DbSet<CardGame> CardGames { get; set; }
    public DbSet<CardToGame> CardsToGames { get; set; }
    public DbSet<TypeToGame> TypesToGames { get; set; }
    
    #endregion

    #region CardActions

    public DbSet<CardAction> CardActions { get; set; }
    public DbSet<AdvanceAction> AdvanceActions { get; set; }
    public DbSet<MoveAction> MoveActions { get; set; }
    public DbSet<KeepAction> KeepActions { get; set; }
    public DbSet<ChoiceAction> ChoiceActions { get; set; }
    public DbSet<PayPlayerAction> PayPlayerActions { get; set; }
    public DbSet<StreetRepairsAction> StreetRepairsActions { get; set; }

    #endregion

    #region Properties

    public DbSet<PropertyDM> Properties { get; set; }
    public DbSet<GameProperty> GameProps { get; set; }
    
    public DbSet<BoardDM> Boards { get; set; }
    public DbSet<BoardToProperty> BoardsToProperties { get; set; }

    #endregion

    #region Game

    public DbSet<GameDM> Games { get; set; }
    public DbSet<PlayerDM> Players { get; set; }
    public DbSet<GamePlayer> GamePlayers { get; set; }
    public DbSet<PlayerToCard> PlayersToCards { get; set; }
    public DbSet<DiceNumbers> PlayerDiceNumbers { get; set; }
    public DbSet<GameCard> GameCards { get; set; }
    public DbSet<GameType> GameCardTypes { get; set; }

    #endregion
}