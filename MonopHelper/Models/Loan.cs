using System.ComponentModel.DataAnnotations.Schema;
using MonopolyCL.Extensions;

namespace MonopHelper.Models;

public class Loan
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int Amount { get; set; }
    public int RepaidAmount { get; set; }
    public bool Repaid { get; set; }
    
    public int PlayerId { get; set; }
    
    [ForeignKey(nameof(PlayerId))]
    public virtual Player Player { get; set; }

    public int Outstanding()
    {
        var balance = Amount - RepaidAmount;
        return balance > 0 ? balance : 0;
    }

    public int Pass(int numPasses)
    {
        var percent = (int)Math.Round(Amount * (0.05*numPasses), 0);
        return percent.RoundToTen();
    }
}