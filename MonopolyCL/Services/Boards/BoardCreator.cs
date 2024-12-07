using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopolyCL.Models.Boards;
using MonopolyCL.Models.Boards.DataModel;
using MonopolyCL.Models.Properties;

namespace MonopolyCL.Services.Boards;

public class BoardCreator
{
    private readonly GameDbSet<BoardDM> _boardSet;
    private readonly GeneralBoardSpaces _generalBoardSpaces;

    public BoardCreator(GameDbSet<BoardDM> boardSet, GeneralBoardSpaces generalBoardSpaces)
    {
        _boardSet = boardSet;
        _generalBoardSpaces = generalBoardSpaces;
    }

    public async Task<Board?> BuildBoard(int boardId, List<IProperty> properties)
    {
        var boardDataModel = await _boardSet.Query().FirstOrDefaultAsync(b => b.Id == boardId);
        if (boardDataModel == null) return null;

        var board = new Board
        {
             Id = boardId,
             Name = boardDataModel.Name
        };

        board.Spaces = [];
        board.Spaces.AddRange(properties);
        board.Spaces.AddRange(_generalBoardSpaces.GetGenericSpaces());
        board.Spaces.AddRange(_generalBoardSpaces.GetTaxSpaces());
        board.Spaces.AddRange(await _generalBoardSpaces.GetCardSpaces());

        board.Spaces = board.Spaces.OrderBy(s => s.BoardIndex).ToList();

        return board;
    }
}