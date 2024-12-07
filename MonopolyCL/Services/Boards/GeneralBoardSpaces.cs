using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopolyCL.Models.Boards;
using MonopolyCL.Models.Boards.Enums;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Cards.Defaults;


namespace MonopolyCL.Services.Boards;

public class GeneralBoardSpaces
{
    private readonly GameDbSet<CardType> _cardSet;
    private readonly byte[] _cardIndexes = [2, 7, 17, 22, 33, 36]; 

    public GeneralBoardSpaces(GameDbSet<CardType> cardSet)
    {
        _cardSet = cardSet;
    }
    
    public List<GenericSpace> GetGenericSpaces()
    {
        return
        [
            new GenericSpace
            {
                Action = SPACE_ACTIONS.GO,
                BoardIndex = 0
            },

            new GenericSpace
            {
                Action = SPACE_ACTIONS.JAIL,
                BoardIndex = 10
            },

            new GenericSpace
            {
                Action = SPACE_ACTIONS.FREE_PARKING,
                BoardIndex = 20
            },

            new GenericSpace
            {
                Action = SPACE_ACTIONS.GO_TO_JAIL,
                BoardIndex = 30
            }
        ];
    }

    public List<TaxSpace> GetTaxSpaces()
    {
        return
        [
            new TaxSpace
            {
                Name = "Income Tax",
                TaxAmount = 200,
                BoardIndex = 4
            },
            new TaxSpace
            {
                Name = "Super Tax",
                TaxAmount = 100,
                BoardIndex = 38
            }
        ];
    }

    public async Task<List<CardSpace>> GetCardSpaces()
    {
        var types = await _cardSet.Query().Where(t => t.TenantId == DefaultsDictionary.MonopolyTenant)
            .Select(t => new
            {
                t.Name,
                t.Id
            }).ToListAsync();

        if (types.Count < 2) return [];

        var chance = types.FirstOrDefault(t => t.Name == CardDefaultsDictionary.Chance);
        var comChest = types.FirstOrDefault(t => t.Name == CardDefaultsDictionary.ComChest);
        if (chance == null || comChest == null) return [];

        var cardSpaces = new List<CardSpace>();
        for (var i = 0; i < 6; i++)
        {
            if (i % 2 == 0)
            {
                cardSpaces.Add(new CardSpace
                {
                    Name = comChest.Name,
                    CardTypeId = comChest.Id,
                    BoardIndex = _cardIndexes[i]
                });
            }
            else
            {
                cardSpaces.Add(new CardSpace
                {
                    Name = chance.Name,
                    CardTypeId = chance.Id,
                    BoardIndex = _cardIndexes[i]
                });
            }
        }

        return cardSpaces;
    }
}