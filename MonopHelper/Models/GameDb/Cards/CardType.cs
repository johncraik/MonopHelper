namespace MonopHelper.Models.GameDb.Cards;

public class CardType
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public required string Name { get; set; }
    public bool IsDeleted { get; set; }
}