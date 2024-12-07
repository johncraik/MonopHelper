using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using MonopHelper.Authentication;
using MonopHelper.Services.Cards;
using MonopolyCL.Dictionaries;
using MonopolyCL.Models.Cards;

namespace MonopHelper.Pages.Cards;

[Authorize(Roles = $"{GameRoles.ManageCards}, {GameRoles.Admin}")]
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
    public List<SelectListItem> CardDeckDropdown { get; set; }
    public List<Card> Cards { get; set; }
    
    //Card Types Tab:
    public List<CardType> CardTypes { get; set; }
    public List<CardType> EditCardTypes { get; set; }
    
    //Card Decks Tab:
    public List<CardDeck> CardDecks { get; set; }
    public List<CardDeck> EditCardDecks { get; set; }

    
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
        
        var decks = await _cardService.GetCardDecks(true);
        CardDecks = decks;
        EditCardDecks = decks.Where(d =>
            d.TenantId != DefaultsDictionary.DefaultTenant && d.TenantId != DefaultsDictionary.MonopolyTenant).ToList();
        CardDeckDropdown = decks.Select(d => new SelectListItem
        {
            Text = d.Name,
            Value = d.Id.ToString(),
            Selected = d.Id == deckId
        }).ToList();
        
        if(CardDeckDropdown.Count == 0) CardDeckDropdown.Add(new SelectListItem
        {
            Text = "No Decks Found!",
            Value = "-1"
        });

        CardTypes = await _cardService.GetCardTypes();
        EditCardTypes = CardTypes.Where(t =>
            t.TenantId != DefaultsDictionary.DefaultTenant && t.TenantId != DefaultsDictionary.MonopolyTenant).ToList();

        var selectedDeckId = 0;
        if (decks.Count > 0 && deckId == 0)
        {
            selectedDeckId = decks.FirstOrDefault()?.Id ?? 0;
        }
        else if(deckId > 0)
        {
            selectedDeckId = deckId;
        }
        
        Cards = await _cardService.GetCardsFromDeck(selectedDeckId, true);
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