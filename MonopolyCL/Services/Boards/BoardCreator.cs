using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopolyCL.Data;
using MonopolyCL.Models.Boards;
using MonopolyCL.Models.Boards.DataModel;
using MonopolyCL.Models.Properties;

namespace MonopolyCL.Services.Boards;

public class BoardCreator
{
    private readonly BoardContext _boardContext;
    private readonly GeneralBoardSpaces _generalBoardSpaces;

    public BoardCreator(BoardContext boardContext, GeneralBoardSpaces generalBoardSpaces)
    {
        _boardContext = boardContext;
        _generalBoardSpaces = generalBoardSpaces;
    }

    public async Task<Board?> BuildBoard(int boardId, List<IProperty> properties)
    {
        var boardDataModel = await _boardContext.Boards.Query().FirstOrDefaultAsync(b => b.Id == boardId);
        if (boardDataModel == null) return null;

        var board = new Board
        {
             Id = boardId,
             Name = boardDataModel.Name,
             Spaces = []
        };

        board.Spaces.AddRange(properties);
        board.Spaces.AddRange(_generalBoardSpaces.GetGenericSpaces());
        board.Spaces.AddRange(_generalBoardSpaces.GetTaxSpaces());
        board.Spaces.AddRange(await _generalBoardSpaces.GetCardSpaces());

        board.Spaces = board.Spaces.OrderBy(s => s.BoardIndex).ToList();

        return board;
    }
}