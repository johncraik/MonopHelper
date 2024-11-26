using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Models;

namespace MonopHelper.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Tenant> Tenants { get; set; }
    
    
    public DbSet<Game> Games { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Loan> Loans { get; set; }
}