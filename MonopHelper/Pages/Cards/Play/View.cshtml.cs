using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopHelper.Models.GameDb.Cards;
using MonopHelper.Services.Cards;
using MonopolyCL.Models.Cards;

namespace MonopHelper.Pages.Cards.Play;

[Authorize]
public class View : PageModel
{
    private readonly CardService _cardService;
    private readonly CardGameService _cardGameService;

    public View(CardService cardService, CardGameService cardGameService)
    {
        _cardService = cardService;
        _cardGameService = cardGameService;
    }
    
    public Card Card { get; set; }
    public int GameId { get; set; }
    
    public async Task<IActionResult> OnGet(int id, int typeId)
    {
        //Find Game:
        var game = await _cardGameService.FetchGame(id);
        if(game == null) return new UnauthorizedResult();
        
        //Find Type:
        var type = await _cardService.FindCardType(typeId);
        if (type == null) return new UnauthorizedResult();

        //Get card:
        var card = await _cardGameService.GetCard(id, typeId);
        if(card == null) return new UnauthorizedResult();
        
        Card = card;
        GameId = game.Game.Id;
        return Page();
    }
}