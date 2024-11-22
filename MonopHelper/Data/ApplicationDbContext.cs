using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonopHelper.Models;

namespace MonopHelper.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Game> Games { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<PlayerToProperty> PlayerToProperties { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<PlayerToLoan> PlayerToLoans { get; set; }
    public DbSet<Loan> Loans { get; set; }
}