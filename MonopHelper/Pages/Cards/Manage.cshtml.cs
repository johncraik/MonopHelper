using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using MonopHelper.Models.GameDb.Cards;
using MonopHelper.Services.Cards;

namespace MonopHelper.Pages.Cards;

[Authorize]
public class Manage : PageModel
{
    private readonly CardService _cardService;
    private readonly UploadCardsService _uploadCardsService;

    public Manage(CardService cardService, UploadCardsService uploadCardsService)
    {
        _cardService = cardService;
        _uploadCardsService = uploadCardsService;
    }
    
    //Cards Tab:
    public List<SelectListItem> CardDecks { get; set; }
    public List<Card> Cards { get; set; }
    
    //Card Types Tab:
    public List<CardType> CardTypes { get; set; }

    
    //Front End Models:
    public class UploadCardsModel
    {
        public int TypeId { get; set; }
        public int DeckId { get; set; }
        [Required]
        [DisplayName("CSV File")]
        public IFormFile CsvFile { get; set; }
    }
    
    [BindProperty]
    public UploadCardsModel Upload { get; set; }
    
    
    private async Task PopulateData(int id)
    {
        var deckId = 0;
        var idIsDeck = await _cardService.FindCardDeck(id);
        if (idIsDeck != null) deckId = idIsDeck.Id;
        
        var decks = await _cardService.GetCardDecks();
        CardDecks = decks.Select(d => new SelectListItem
        {
            Text = d.Name,
            Value = d.Id.ToString(),
            Selected = d.Id == deckId
        }).ToList();

        CardTypes = await _cardService.GetCardTypes();
        
        Cards = await _cardService.GetCardsFromDeck(deckId == 0 ? decks.FirstOrDefault()?.Id ?? 0 : deckId);
    }
    
    public async Task<IActionResult> OnGet(int id)
    {
        await PopulateData(id);
        
        return Page();
    }

    public async Task<IActionResult> OnPostUpload()
    {
        if (ModelState.IsValid)
        {
            var success = await _uploadCardsService.UploadFile(Upload.CsvFile, Upload.TypeId, Upload.DeckId);
            if (success)
            {
                return RedirectToPage($"/Cards/{nameof(Manage)}", new { id = Upload.DeckId });
            }
        }

        await PopulateData(Upload.DeckId);
        return Page();
    }
}