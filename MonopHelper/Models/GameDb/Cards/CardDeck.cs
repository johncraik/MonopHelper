namespace MonopHelper.Models.GameDb.Cards;

public class CardDeck
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public required string Name { get; set; }
    public int DiffRating { get; set; }
    public bool IsDeleted { get; set; }
}