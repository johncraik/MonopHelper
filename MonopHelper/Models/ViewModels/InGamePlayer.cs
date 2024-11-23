namespace MonopHelper.Models.ViewModels;

public class InGamePlayer
{
    public Player Player { get; set; }
    public List<Property> Properties { get; set; }
    public List<Loan> Loans { get; set; }
}