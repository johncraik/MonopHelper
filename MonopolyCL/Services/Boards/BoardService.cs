using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopolyCL.Data;
using MonopolyCL.Models.Boards.DataModel;

namespace MonopolyCL.Services.Boards;

public class BoardService
{
    private readonly BoardContext _boardContext;

    public BoardService(BoardContext boardContext)
    {
        _boardContext = boardContext;
    }

    public async Task<List<BoardDM>> GetBoards() => await _boardContext.Boards.Query().ToListAsync();
}