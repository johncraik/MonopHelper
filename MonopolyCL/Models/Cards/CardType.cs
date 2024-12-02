using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Cards;

public class CardType : TenantedModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsDeleted { get; set; }
}