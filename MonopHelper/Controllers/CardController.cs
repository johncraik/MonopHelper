using Microsoft.AspNetCore.Mvc;
using MonopHelper.Services.Cards;

namespace MonopHelper.Controllers;

public class CardController : Controller
{
    private readonly CardService _cardService;

    public CardController(CardService cardService)
    {
        _cardService = cardService;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    public async Task<IActionResult> CardsTablePartial(int id)
    {
        var model = await _cardService.GetCardsFromDeck(id);
        return PartialView("Cards/_CardsTable", model);
    }

    public async Task<bool> RemoveCard(int id)
    {
        var card = await _cardService.FindCard(id);
        if (card == null) return false;

        await _cardService.RemoveCard(card);
        return true;
    }
}