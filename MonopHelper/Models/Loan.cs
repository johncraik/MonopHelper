namespace MonopHelper.Models;

public class Loan
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public int RepaidAmount { get; set; }
    public bool Repaid { get; set; }

    public int Outstanding()
    {
        var balance = Amount - RepaidAmount;
        return balance > 0 ? balance : 0;
    }

    public int Pass(int numPasses)
    {
        var percent = (int)Math.Round(Amount * (0.05*numPasses), 0);
        return ((int)Math.Round(percent / 10d)) * 10;
    }
}