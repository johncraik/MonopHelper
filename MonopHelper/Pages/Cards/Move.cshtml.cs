using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Editing;
using MonopHelper.Models.GameDb.Cards;
using MonopHelper.Services.Cards;
using MonopolyCL.Models.Cards;

namespace MonopHelper.Pages.Cards;

public class Move : PageModel
{
    private readonly CardService _cardService;

    public Move(CardService cardService)
    {
        _cardService = cardService;
    }
    
    public string DeckName { get; set; }
    public List<SelectListItem> CardDecks { get; set; }
    public List<Card> Cards { get; set; } 
    
    [BindProperty]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid deck")]
    [DisplayName("Card Deck")]
    public int SelectedDeck { get; set; }
    [BindProperty]
    public int CurrentDeck { get; set; }
    [BindProperty]
    public bool Copy { get; set; }

    public async Task<bool> SetupPage(int deckId, bool copy)
    {
        var deck = await _cardService.FindCardDeck(deckId, copy);
        if (deck == null) return false;

        CurrentDeck = deck.Id;
        DeckName = deck.Name; 
            
        Cards = await _cardService.GetCardsFromDeck(deckId, true);
        CardDecks = (await _cardService.GetCardDecks(monop: false)).Where(d => d.Id != deck.Id)
            .Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString()
            }).ToList();

        return true;
    }
    
    public async Task<IActionResult> OnGet(int id, bool copy = false)
    {
        var success = await SetupPage(id, copy);
        if (!success) return new UnauthorizedResult();

        Copy = copy;
        
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var setup = await SetupPage(CurrentDeck, Copy);
        if (!setup) return new UnauthorizedResult();
        
        if (!ModelState.IsValid) return Page();

        var success = false;
        if (Copy)
        {
            success = await _cardService.CopyCardsInDeck(CurrentDeck, SelectedDeck);
        }
        else
        {
            success = await _cardService.MoveCardsInDeck(CurrentDeck, SelectedDeck);
        }
        
        if (success) return RedirectToPage(nameof(Manage), new {id = SelectedDeck});
        
        ModelState["SelectedDeck"]?.Errors.Add("Cannot find card deck!");
        return Page();
    }
}