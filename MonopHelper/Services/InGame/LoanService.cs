using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopHelper.Models;
using SQLitePCL;

namespace MonopHelper.Services.InGame;

public class LoanService
{
    private readonly ApplicationDbContext _context;

    public LoanService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Loan>> GetPlayerLoans(int playerId, bool repaid = true)
    {
        var loans = _context.Loans.Where(l => l.PlayerId == playerId)
            .OrderBy(l => l.Repaid);

        if (!repaid) return await loans.Where(l => !l.Repaid).ToListAsync();
        return await loans.ToListAsync();
    }

    public async Task<Loan?> FindLoan(int id)
    {
        return await _context.Loans.FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task CreateLoan(Loan loan)
    {
        loan.Repaid = false;
        loan.RepaidAmount = 0;
        await _context.Loans.AddAsync(loan);
        await _context.SaveChangesAsync();
    }

    public async Task Repay(int loanId, int amount)
    {
        var loan = await FindLoan(loanId);
        if (loan != null)
        {
            loan.RepaidAmount += amount;
            if (loan.RepaidAmount >= loan.Amount)
            {
                loan.RepaidAmount = loan.Amount;
                loan.Repaid = true;
            }

            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RepayAll(int playerId, int amount, byte type)
    {
        var loans = await GetPlayerLoans(playerId, false);

        var splitAmount = amount;
        if (loans.Count > 0) splitAmount = (int)Math.Round((double)amount / loans.Count);
        
        foreach (var loan in loans)
        {
            var repay = type > 0 ? loan.Pass(type) : splitAmount;

            loan.RepaidAmount += repay;
            if (loan.RepaidAmount < loan.Amount) continue;
            
            loan.RepaidAmount = loan.Amount;
            loan.Repaid = true;
        }

        if (loans.Count > 0)
        {
            _context.Loans.UpdateRange(loans);
            await _context.SaveChangesAsync();
        }
    }
}