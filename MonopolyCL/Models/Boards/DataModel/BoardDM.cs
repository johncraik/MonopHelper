using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Boards.DataModel;

public class BoardDM : TenantedModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}