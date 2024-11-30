using MonopHelper.Data;

namespace MonopHelper.Models.GameDb.Cards;

public class CardDeck : TenantedModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public double DiffRating { get; set; }
    public bool IsDeleted { get; set; }
}