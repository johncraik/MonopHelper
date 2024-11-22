using Microsoft.EntityFrameworkCore;

namespace MonopHelper.Models;

[PrimaryKey(nameof(PlayerId), nameof(LoanId))]
public class PlayerToLoan
{
    public int PlayerId { get; set; }
    public int LoanId { get; set; }
}