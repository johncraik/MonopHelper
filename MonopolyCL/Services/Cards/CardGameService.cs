using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopolyCL.Data;
using MonopolyCL.Helpers;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Cards.ViewModels;

namespace MonopolyCL.Services.Cards;

public class CardGameService
{
    private readonly CardService _cardService;
    private readonly UserInfo _userInfo;
    private readonly CardContext _cardContext;

    public CardGameService(CardService cardService,
        UserInfo userInfo,
        CardContext cardContext)
    {
        _cardService = cardService;
        _userInfo = userInfo;
        _cardContext = cardContext;
    }

    public async Task<CardGameViewModel?> CreateGame(int deckId)
    {
        //Check Deck Exists:
        var deck = await _cardService.FindCardDeck(deckId, true);
        if (deck == null) return null;
        
        //Create game:
        var game = new CardGame
        {
            TenantId = _userInfo.TenantId,
            UserId = _userInfo.UserId,
            DateCreated = DateTime.Now,
            LastPlayed = DateTime.Now,
            DeckId = deck.Id,
            IsDeleted = false
        };
        await _cardContext.CardGameSet.AddAsync(game);
        
        //Get cards and shuffle:
        var cardsInDeck = await _cardService.GetCardsFromDeck(deckId);
        var shuffle = new ShuffleList<Card>();
        var shuffledCards = shuffle.Shuffle(cardsInDeck);
        
        //Create view model:
        var cgvm = new CardGameViewModel
        {
            Game = game
        };

        cgvm.Cards = [];
        cgvm.Types = [];
        var addCardToGame = new List<CardToGame>();
        var addTypeToGame = new List<TypeToGame>();
        uint index = 1;
        foreach (var card in shuffledCards)
        {
            //Create card to game reference:
            var cardToGame = new CardToGame
            {
                TenantId = _userInfo.TenantId,
                CardId = card.Id,
                GameId = game.Id,
                Index = index
            };
            cgvm.Cards.Add((card, index));
            addCardToGame.Add(cardToGame);
            
            if (!addTypeToGame.Select(ttg => ttg.TypeId).Contains(card.CardTypeId))
            {
                //Create type to game reference IF type not already referenced:
                var typeToGame = new TypeToGame
                {
                    TenantId = _userInfo.TenantId,
                    TypeId = card.CardTypeId,
                    GameId = game.Id,
                    CurrentIndex = 1
                };
                cgvm.Types.Add((card.CardType, 0));
                addTypeToGame.Add(typeToGame);
            }

            //Increase index:
            index++;
        }

        //Add referenced to databed:
        await _cardContext.CardToGameSet.AddAsync(addCardToGame);
        await _cardContext.TypeToGameSet.AddAsync(addTypeToGame);
        
        //Return view model:
        return cgvm;
    }

    
    public async Task<CardGameViewModel?> FetchGame(int id)
    {
        //Get game:
        var game = await _cardContext.CardGameSet.Query().FirstOrDefaultAsync(g => g.UserId == _userInfo.UserId
                                                                   && g.Id == id);
        //Return null if no game found:
        if (game == null) return null;
        
        //Get card IDs in game:
        var cardIds = _cardContext.CardToGameSet.Query(true).Where(ctg => ctg.GameId == game.Id)
            .Select(ctg => new
            {
                ctg.CardId,
                ctg.Index
            });
    
        /*
         * Build view model:
         */
        var cgvm = new CardGameViewModel
        {
            Game = game,
            Cards = [],
            Types = []
        };
        //Get cards for view model:
        foreach (var cardIndex in cardIds)
        {
            //Find card:
            var card = await _cardService.FindCard(cardIndex.CardId, false);
            if (card != null)
            {
                //Add card and index to view model:
                cgvm.Cards.Add((card, cardIndex.Index));
            }
        }

        //Get types for view model:
        var typeIds = _cardContext.TypeToGameSet.Query(true).Where(ttg => ttg.GameId == game.Id)
            .Select(ttg => new
            {
                ttg.TypeId,
                ttg.CurrentIndex
            });
        foreach (var typeCurIndex in typeIds)
        {
            //Find type:
            var type = await _cardService.FindCardType(typeCurIndex.TypeId, true);
            if (type != null)
            {
                //Add type and current index to view model:
                cgvm.Types.Add((type, typeCurIndex.CurrentIndex));
            }
        }

        //Set last played:
        game.LastPlayed = DateTime.Now;
        await _cardContext.CardGameSet.UpdateAsync(game);

        return cgvm;
    }

    public async Task<Card?> GetCard(int gameId, int typeId)
    {
        //Fetch Game Data:
        var cgvm = await FetchGame(gameId);
        if (cgvm == null) return null;
        
        //Get Type selected in game:
        var typeVm = cgvm.Types.FirstOrDefault(t => t.Type.Id == typeId);
        if (typeVm.Type == null) return null;
        
        //Get cards in type:
        var cards = cgvm.Cards.Where(c => c.Card.CardTypeId == typeVm.Type.Id)
            .OrderBy(c => c.Index).ToList();
        if (cards.Count == 0) return null;
        
        //Get card indexes:
        var indexes = cards.Select(c => c.Index).ToList();
        
        //Calculate card index:
        var cardIndex = indexes.FirstOrDefault(ci => ci >= typeVm.CurrentIndex);
        var newIndex = cardIndex == default ? indexes.Min() : cardIndex;

        //Get Card:
        var cardId = cards.FirstOrDefault(c => c.Index == newIndex).Card?.Id ?? 0;
        var card = await _cardService.FindCard(cardId, false);
        if (card == null) return null;
        
        
        //Update current index:
        var type = await _cardContext.TypeToGameSet.Query(true)
            .FirstOrDefaultAsync(t => t.GameId == gameId && t.TypeId == typeVm.Type.Id);
        if (type == null) return null;

        type.CurrentIndex = newIndex + 1;
        await _cardContext.TypeToGameSet.UpdateAsync(type);
        
        return card;
    }

    public async Task<bool> DeleteCardGame(int id)
    {
        var cgvm = await FetchGame(id);
        if (cgvm == null) return false;

        //Remove many-many records:
        var cardsInGame = _cardContext.CardToGameSet.Query(true).Where(c => c.GameId == id).ToList();
        var typesInGame = _cardContext.TypeToGameSet.Query(true).Where(t => t.GameId == id).ToList();

        await _cardContext.CardToGameSet.RemoveAsync(cardsInGame);
        await _cardContext.TypeToGameSet.RemoveAsync(typesInGame);
        
        //Update game to is deleted:
        cgvm.Game.IsDeleted = true;
        await _cardContext.CardGameSet.UpdateAsync(cgvm.Game);

        return true;
    }

    public async Task<List<CardGame>> GetCardGames() => await _cardContext.CardGameSet.Query().Include(g => g.Deck)
            .Where(g => g.UserId == _userInfo.UserId).ToListAsync();

    public async Task RemoveGamesWithDeck(int deckId)
    {
        var games = await _cardContext.CardGameSet.Query().Where(g => g.DeckId == deckId).ToListAsync();
        foreach (var g in games)
        {
            await DeleteCardGame(g.Id);
        }
    }
}