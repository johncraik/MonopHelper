namespace MonopHelper.Models;

public class Loan
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public int RepaidAmount { get; set; }
    public bool Repaid { get; set; }
}