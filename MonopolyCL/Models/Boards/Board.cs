namespace MonopolyCL.Models.Boards;

public class Board
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<IBoardSpace> Spaces { get; set; }
}