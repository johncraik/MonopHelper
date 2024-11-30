using System.ComponentModel;
using System.Drawing.Drawing2D;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using MonopHelper.Models.GameDb.Cards;
using MonopHelper.Services.Cards;

namespace MonopHelper.Pages.Cards;

public class Edit : PageModel
{
    private readonly CardService _cardService;

    public Edit(CardService cardService)
    {
        _cardService = cardService;
    }

    public List<SelectListItem> CardDecks { get; set; }
    public List<SelectListItem> CardTypes { get; set; }
    public bool Adding = false;
    [BindProperty]
    public CardViewModel Input { get; set; }

    public class CardViewModel
    {
        [Required]
        [DisplayName("Card Text")]
        public string CardText { get; set; }
        [DisplayName("Cost to Player")]
        public int? Cost { get; set; }
        [DisplayName("Card Type")]
        public int CardTypeId { get; set; }
        [DisplayName("Card Deck")]
        public int DeckId { get; set; }

        public CardViewModel()
        {
        }
        
        public CardViewModel(Card card)
        {
            CardText = card.Text;
            Cost = card.Cost;
            CardTypeId = card.CardTypeId;
            DeckId = card.DeckId;
        }
        
        public void Fill(Card card)
        {
            card.Text = CardText;
            card.Cost = Cost;
            card.DeckId = DeckId;
            card.CardTypeId = CardTypeId;
        }
    }

    public async Task SetupPage()
    {
        CardDecks = (await _cardService.GetCardDecks())
            .Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString()
            }).ToList();
        CardTypes = (await _cardService.GetCardTypes())
            .Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.Id.ToString()
            }).ToList();
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
        await SetupPage();

        if (Adding) return Page();
        var card = await _cardService.FindCard(id);
        if (card == null)
        {
            return RedirectToPage(nameof(Manage));
        }

        if (!Adding)
        {
            Input = new CardViewModel(card);
        }
        
        return Page();
    }

    public async Task<IActionResult> OnPost(int id)
    {
        SetupAdding(id);
        await SetupPage();
        
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var card = await _cardService.FindCard(id);
        if (Adding || card == null)
        {
            card = new Card { Text = "" };
            Input.Fill(card);
            await _cardService.AddCard(card);
        }
        else
        {
            Input.Fill(card);
            await _cardService.UpdateCard(card);
        }
        
        return RedirectToPage(nameof(Manage), new {id = card.DeckId});
    }
}