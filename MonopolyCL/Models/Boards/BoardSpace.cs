using MonopolyCL.Models.Boards.Enums;

namespace MonopolyCL.Models.Boards;

public interface IBoardSpace
{
    public byte BoardIndex { get; internal set; }
}

public class GenericSpace : IBoardSpace
{
    public string Name { get; set; }
    public byte BoardIndex { get; set; }
    public SPACE_ACTIONS Action { get; set; }
}

public class TaxSpace : IBoardSpace
{
    public string Name { get; set; }
    public byte BoardIndex { get; set; }
    public int TaxAmount { get; set; }
}

public class CardSpace : IBoardSpace
{
    public string Name { get; set; }
    public byte BoardIndex { get; set; }
    public int CardTypeId { get; set; }
}