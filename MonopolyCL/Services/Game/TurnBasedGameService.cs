using MonopolyCL.Services.Cards;

namespace MonopolyCL.Services.Game;

public class TurnBasedGameService
{
    private readonly MonopolyGameService _gameService;
    private readonly CardGameService _cardGameService;

    public TurnBasedGameService(MonopolyGameService gameService, CardGameService cardGameService)
    {
        _gameService = gameService;
        _cardGameService = cardGameService;
    }
}