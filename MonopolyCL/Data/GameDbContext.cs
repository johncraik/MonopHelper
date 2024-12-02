using Microsoft.EntityFrameworkCore;
using MonopolyCL.Models.Cards;

namespace MonopHelper.Data;

public class GameDbContext : DbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> options)
        : base(options)
    {
    }

    public DbSet<Card> Cards { get; set; }
    public DbSet<CardType> CardTypes { get; set; }
    public DbSet<CardDeck> CardDecks { get; set; }
    
    public DbSet<CardGame> CardGames { get; set; }
    public DbSet<CardToGame> CardsToGames { get; set; }
    public DbSet<TypeToGame> TypesToGames { get; set; }
}