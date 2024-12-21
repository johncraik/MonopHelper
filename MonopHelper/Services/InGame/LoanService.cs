using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopHelper.Models;
using SQLitePCL;

namespace MonopHelper.Services.InGame;

public class LoanService
{
    private readonly ApplicationDbContext _context;
    private readonly UserInfo _userInfo;

    public LoanService(ApplicationDbContext context, UserInfo userInfo)
    {
        _context = context;
        _userInfo = userInfo;
    }
    
    public async Task<List<Loan>> GetPlayerLoans(int playerId, bool repaid = true)
    {
        var loans = _context.Loans.Where(l => l.TenantId == _userInfo.TenantId 
                                              && l.PlayerId == playerId)
            .OrderBy(l => l.Repaid);

        if (!repaid) return await loans.Where(l => !l.Repaid).ToListAsync();
        return await loans.ToListAsync();
    }

    public async Task<Loan?> FindLoan(int id)
    {
        return await _context.Loans.FirstOrDefaultAsync(l => l.TenantId == _userInfo.TenantId && l.Id == id);
    }

    public async Task<bool> CreateLoan(Loan loan)
    {
        var numLoans = NumberOfPlayerLoans(loan.PlayerId);
        if (numLoans >= 3) return false;
        
        loan.TenantId = _userInfo.TenantId;
        loan.Repaid = false;
        loan.RepaidAmount = 0;
        
        await _context.Loans.AddAsync(loan);
        await _context.SaveChangesAsync();

        return true;
    }

    private int NumberOfPlayerLoans(int playerId) => _context.Loans.Count(l => l.PlayerId == playerId && !l.Repaid);

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