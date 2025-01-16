using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonopolyCL.Models.Game;
using MonopolyCL.Services.Boards;
using MonopolyCL.Services.Cards;
using MonopolyCL.Services.Game;
using MonopolyCL.Services.Players;

namespace MonopHelper.Pages.TurnBased;

public class Index : PageModel
{
    private readonly MonopolyGameService _gameService;
    private readonly CardService _cardService;
    private readonly PlayerService _playerService;
    private readonly BoardService _boardService;
    public List<SelectListItem> Boards { get; set; }
    public List<SelectListItem> Decks { get; set; }
    public List<SelectListItem> Players { get; set; }
    [BindProperty]
    public CreateGameViewModel CreateGame { get; set; }
    
    public Index(MonopolyGameService gameService, 
        CardService cardService,
        PlayerService playerService,
        BoardService boardService)
    {
        _gameService = gameService;
        _cardService = cardService;
        _playerService = playerService;
        _boardService = boardService;
    }
    
    public class CreateGameViewModel
    {
        [DisplayName("Board")]
        public int BoardId { get; set; }
        [DisplayName("Card Deck")]
        public int DeckId { get; set; }
        [DisplayName("Players")]
        public List<int> PlayerIds { get; set; }
        [DisplayName("Game Name")]
        public string GameName { get; set; }
    }
    
    public class TurnBasedViewModel
    {
        public int GameId { get; set; }
        public int CardGameId { get; set; }
        [DisplayName("Name")]
        public string GameName { get; set; }
        [DisplayName("Board")]
        public string BoardName { get; set; }
        public string Players { get; set; }
        [DisplayName("Last Played")]
        public DateTime LastPlayed { get; set; }
    }

    public IEnumerable<TurnBasedViewModel> Games;

    public async Task SetupPage()
    {
        var games = await _gameService.GetGames();
        Games = games.Select(g => new TurnBasedViewModel
        {
            GameId = g.Game.Id,
            CardGameId = g.Cards?.Game.Id ?? 0,
            GameName = g.Game.Name,
            BoardName = g.Board.Name,
            Players = (g.Players.Select(p => p.Name).Aggregate("", (current, name) => current += $"{name}, "))[..^2],
            LastPlayed = g.Game.LastPlayed
        });

        Boards = (await _boardService.GetBoards()).Select(b => new SelectListItem
        {
            Text = b.Name,
            Value = b.Id.ToString()
        }).ToList();
        Decks = (await _cardService.GetCardDecks()).Select(d => new SelectListItem
        {
            Text = d.Name,
            Value = d.Id.ToString()
        }).ToList();
        Players = (await _playerService.GetPlayers()).Select(p => new SelectListItem
        {
            Text = p.Name,
            Value = p.Id.ToString()
        }).ToList();
    }

    public async Task OnGet() => await SetupPage();

    public async Task<IActionResult> OnPost()
    {
        await SetupPage();
        if (!ModelState.IsValid) return Page();

        var res = await _gameService.CreateGame(CreateGame.GameName,
            CreateGame.BoardId, 
            CreateGame.DeckId, 
            CreateGame.PlayerIds,
            (GAME_RULES)511);
        var game = res.ReturnObj;
        if (game != null && res.Response.IsValid) return RedirectToPage(nameof(Setup), new { id = game.Game.Id });
        
        ModelState.AddModelError(res.Response.ErrorKey, res.Response.ErrorMsg);
        return Page();
    }
}