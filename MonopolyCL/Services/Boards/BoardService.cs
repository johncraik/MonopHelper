using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopolyCL.Models.Boards.DataModel;

namespace MonopolyCL.Services.Boards;

public class BoardService
{
    private readonly GameDbSet<BoardDM> _boardSet;

    public BoardService(GameDbSet<BoardDM> boardSet)
    {
        _boardSet = boardSet;
    }

    public async Task<List<BoardDM>> GetBoards() => await _boardSet.Query().ToListAsync();
}