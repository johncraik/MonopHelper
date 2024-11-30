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

    [HttpPost]
    public async Task<bool> RemoveCard(int id)
    {
        var card = await _cardService.FindCard(id);
        if (card == null) return false;

        card.IsDeleted = true;
        await _cardService.UpdateCard(card);
        return true;
    }

    public async Task<IActionResult> CardTypesPartial()
    {
        var model = await _cardService.GetCardTypes();
        return PartialView("Cards/_CardTypesTable", model);
    }

    [HttpPost]
    public async Task<bool> EditCardType(int id, string name)
    {
        var cardType = await _cardService.FindCardType(id);
        if (cardType == null) return false;

        cardType.Name = name;
        await _cardService.UpdateCardType(cardType);
        return true;
    }

    [HttpPost]
    public async Task<bool> RemoveCardType(int id)
    {
        await _cardService.Test();
        
        var cardType = await _cardService.FindCardType(id);
        if (cardType == null) return false;

        cardType.IsDeleted = true;
        await _cardService.UpdateCardType(cardType);
        return true;
    }
}