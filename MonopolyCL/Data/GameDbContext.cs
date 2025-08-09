using Microsoft.EntityFrameworkCore;
using MonopolyCL.Models.Boards.DataModel;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Cards.CardActions;
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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            fk.DeleteBehavior = DeleteBehavior.Restrict;
        
        base.OnModelCreating(modelBuilder);
    }

    #region Cards

    public DbSet<Card> Cards { get; set; }
    public DbSet<CardType> CardTypes { get; set; }
    public DbSet<CardDeck> CardDecks { get; set; }
    
    public DbSet<CardGame> CardGames { get; set; }
    public DbSet<CardToGame> CardsToGames { get; set; }
    public DbSet<TypeToGame> TypesToGames { get; set; }
    
    public DbSet<ActionConfig> CardActionConfigs { get; set; }
    
    #endregion

    #region Properties

    public DbSet<PropertyDM> Properties { get; set; }
    public DbSet<GameProperty> GameProps { get; set; }
    public DbSet<PlayerToProperty> PlayersToProperties { get; set; }
    
    public DbSet<BoardDM> Boards { get; set; }
    public DbSet<BoardToProperty> BoardsToProperties { get; set; }

    #endregion

    #region Game

    public DbSet<GameDM> Games { get; set; }
    public DbSet<TurnOrder> TurnOrders { get; set; }
    public DbSet<PlayerDM> Players { get; set; }
    public DbSet<GamePlayer> GamePlayers { get; set; }
    public DbSet<Loan> PlayerLoans { get; set; }
    public DbSet<PlayerToCard> PlayersToCards { get; set; }
    public DbSet<DiceNumbers> PlayerDiceNumbers { get; set; }

    #endregion
}