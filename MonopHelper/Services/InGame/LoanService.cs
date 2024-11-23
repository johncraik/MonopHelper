using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopHelper.Models;

namespace MonopHelper.Services.InGame;

public class LoanService
{
    private readonly ApplicationDbContext _context;

    public LoanService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Loan>> GetPlayerLoans(int playerId)
    {
        return await _context.Loans.Where(l => l.PlayerId == playerId)
            .OrderBy(l => l.Repaid).ToListAsync();
    }
}