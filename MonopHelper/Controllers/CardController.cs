using Microsoft.AspNetCore.Mvc;
using MonopHelper.Authentication;
using MonopHelper.Models.GameDb.Cards;
using MonopHelper.Services.Cards;

namespace MonopHelper.Controllers;

public class CardController : Controller
{
    private readonly CardService _cardService;
    private readonly UserInfo _userInfo;

    public CardController(CardService cardService, UserInfo userInfo)
    {
        _cardService = cardService;
        _userInfo = userInfo;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    public async Task<IActionResult> CardsTablePartial(int id)
    {
        var model = await _cardService.GetCardsFromDeck(id, true);
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

    public async Task<IActionResult> AddCardsPartial()
    {
        var model = await _cardService.GetCardTypes();
        return PartialView("Cards/_AddCardTypes", model);
    }
    
    public async Task<IActionResult> CardTypesPartial()
    {
        var model = await _cardService.GetCardTypes();
        return PartialView("Cards/_CardTypesTable", model);
    }

    [HttpPost]
    public async Task<bool> AddCardType(string name)
    {
        var valid = await _cardService.ValidateCardType(name);
        if (!valid) return valid;

        var cardType = new CardType
        {
            TenantId = _userInfo.TenantId,
            Name = name,
            IsDeleted = false
        };

        await _cardService.AddCardType(cardType);
        return true;
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
        var cardType = await _cardService.FindCardType(id);
        if (cardType == null) return false;

        //Sets cards to undefined that have the type being deleted:
        var cardsWithType = await _cardService.GetCardsFromType(id);
        foreach (var card in cardsWithType)
        {
            card.CardTypeId = await _cardService.GetUndefinedTypeId();
        }
        await _cardService.UpdateCard(cardsWithType);

        cardType.IsDeleted = true;
        await _cardService.UpdateCardType(cardType);
        return true;
    }
    
    
    public async Task<IActionResult> CardDecksPartial()
    {
        var model = await _cardService.GetCardDecks();
        return PartialView("Cards/_CardDecksTable", model);
    }

    [HttpPost]
    public async Task<bool> RemoveCardDeck(int id)
    {
        var cardDeck = await _cardService.FindCardDeck(id);
        if (cardDeck == null) return false;

        //Sets cards to undefined that have the type being deleted:
        var cardsInDeck = await _cardService.GetCardsFromDeck(id);
        foreach (var card in cardsInDeck)
        {
            card.CardTypeId = await _cardService.GetUndefinedTypeId();
        }
        await _cardService.UpdateCard(cardsInDeck);

        cardDeck.IsDeleted = true;
        await _cardService.UpdateCardDeck(cardDeck);
        return true;
    }
}