using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonopHelper.Authentication;
using MonopHelper.Models.GameDb.Cards;
using MonopHelper.Services.Cards;

namespace MonopHelper.Pages.Cards;

[Authorize]
public class Deck : PageModel
{
    private readonly CardService _cardService;
    private readonly UserInfo _userInfo;

    public Deck(CardService cardService, UserInfo userInfo)
    {
        _cardService = cardService;
        _userInfo = userInfo;
    }
    
    public bool Adding = false;
    [BindProperty]
    public DeckViewModel Input { get; set; }

    public class DeckViewModel
    {
        [Required]
        [DisplayName("Deck Name")]
        public string Name { get; set; }
        [DisplayName("Difficulty Rating")]
        public double DiffRating { get; set; }

        public DeckViewModel()
        {
        }
        
        public DeckViewModel(CardDeck deck)
        {
            Name = deck.Name;
            DiffRating = deck.DiffRating;
        }
        
        public void Fill(CardDeck deck)
        {
            deck.Name = Name;
            deck.DiffRating = DiffRating;
        }
    }
    
    public void SetupAdding(int id)
    {
        if (id == default)
        {
            Adding = true;
        }
    }
    
    public async Task<IActionResult> OnGet(int id)
    {
        SetupAdding(id);

        if (Adding) return Page();
        var deck = await _cardService.FindCardDeck(id);
        if (deck == null)
        {
            return RedirectToPage(nameof(Manage));
        }

        if (!Adding)
        {
            Input = new DeckViewModel(deck);
        }
        
        return Page();
    }

    public async Task<IActionResult> OnPost(int id)
    {
        SetupAdding(id);
        
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var deck = await _cardService.FindCardDeck(id);
        if (Adding || deck == null)
        {
            deck = new CardDeck { Name = "" };
            Input.Fill(deck);
            deck.TenantId = _userInfo.TenantId;

            var validatedRating = await _cardService.ValidateCardDeck(deck.DiffRating);
            deck.DiffRating = validatedRating;
            await _cardService.AddCardDeck(deck);
        }
        else
        {
            Input.Fill(deck);
            
            var validatedRating = await _cardService.ValidateCardDeck(deck.DiffRating);
            deck.DiffRating = validatedRating;
            await _cardService.UpdateCardDeck(deck);
        }
        
        return RedirectToPage(nameof(Manage), new {id = deck.Id});
    }
}