namespace MonopHelper.Models.ViewModels;

public class NewGamePlayers
{
    public const string NoPlayers = "No players added!";
    private const string PlayerDelimiter = "/#/";
    public List<string> Players { get; init; }

    public NewGamePlayers(string? p)
    {
        Players = p?.Split(PlayerDelimiter, StringSplitOptions.RemoveEmptyEntries).OrderBy(ply => ply).ToList() ?? [NoPlayers];
        
        //Remove no players:
        if (Players.Count > 1)
        {
            var noPlayers = Players.FirstOrDefault(ply => ply == NoPlayers);
            if (noPlayers != null) Players.Remove(noPlayers);
        }
    }
}