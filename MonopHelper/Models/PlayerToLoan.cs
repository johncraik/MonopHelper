using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonopHelper.Models;

[PrimaryKey(nameof(PlayerId), nameof(LoanId))]
public class PlayerToLoan
{
    public int PlayerId { get; set; }
    public int LoanId { get; set; }
    
    [ForeignKey(nameof(PlayerId))]
    public virtual Player Player { get; set; }
    
    [ForeignKey(nameof(LoanId))]
    public virtual Loan Loan { get; set; }
}