using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Data;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Cards.Actions;
using MonopolyCL.Models.Cards.Enums;
using MonopolyCL.Models.Properties.Enums;

namespace MonopolyCL.Services.Cards;

public class CardActionsService
{
    private readonly UserInfo _userInfo;
    private readonly CardActionContext _actionContext;
    private readonly CardContext _cardContext;

    public CardActionsService(UserInfo userInfo,
        CardActionContext actionContext,
        CardContext cardContext)
    {
        _userInfo = userInfo;
        _actionContext = actionContext;
        _cardContext = cardContext;
    }

    private async Task CreateAction(int cardId, int actionId, CARD_ACTION action)
    {
        var cardAction = new CardAction
        {
            TenantId = _userInfo.TenantId,
            CardId = cardId,
            ActionId = actionId,
            Action = action
        };
        await _actionContext.CardActions.AddAsync(cardAction);
    }

    public async Task<bool> AddAdvanceAction(int cardId, int index, PROP_SET set, bool claimGo)
    {
        var card = await _cardContext.CardSet.Query().FirstOrDefaultAsync(c => c.Id == cardId);
        if (card == null) return false;

        var advance = await _actionContext.AdvanceActions.Query().FirstOrDefaultAsync(a => a.AdvanceIndex == index && a.ClaimGo == claimGo);
        if (advance == null)
        {
            advance = new AdvanceAction
            {
                AdvanceIndex = index,
                ClaimGo = claimGo,
                Set = set
            };
            advance.SetColour();
            await _actionContext.AdvanceActions.AddAsync(advance);
        }

        await CreateAction(card.Id, advance.Id, CARD_ACTION.ADVANCE);
        return true;
    }

    public async Task<bool> AddMoveAction(int cardId, int amount, bool isForward)
    {
        var card = await _cardContext.CardSet.Query().FirstOrDefaultAsync(c => c.Id == cardId);
        if (card == null) return false;

        var move = await _actionContext.MoveActions.Query().FirstOrDefaultAsync(m => m.MoveAmount == amount && m.IsForward == isForward);
        if (move == null)
        {
            move = new MoveAction
            {
                MoveAmount = amount,
                IsForward = isForward
            };
            await _actionContext.MoveActions.AddAsync(move);
        }

        await CreateAction(card.Id, move.Id, CARD_ACTION.MOVE);
        return true;
    }

    public async Task<bool> AddKeepAction(int cardId)
    {
        var card = await _cardContext.CardSet.Query().FirstOrDefaultAsync(c => c.Id == cardId);
        if (card == null) return false;

        var keep = new KeepAction();
        await _actionContext.KeepActions.AddAsync(keep);
        await CreateAction(card.Id, keep.Id, CARD_ACTION.KEEP_CARD);
        return true;
    }

    public async Task<bool> AddChoiceAction(int cardId, int typeId)
    {
        var card = await _cardContext.CardSet.Query().FirstOrDefaultAsync(c => c.Id == cardId);
        if (card == null) return false;

        var type = await _cardContext.CardTypeSet.Query().FirstOrDefaultAsync(t => t.Id == typeId);
        if (type == null) return false;

        var choice = await _actionContext.ChoiceActions.Query().FirstOrDefaultAsync(c => c.CardTypeId == type.Id);
        if (choice == null)
        {
            choice = new ChoiceAction
            {
                CardTypeId = type.Id
            };
            await _actionContext.ChoiceActions.AddAsync(choice);
        }

        await CreateAction(card.Id, choice.Id, CARD_ACTION.CHOICE);
        return true;
    }

    public async Task<bool> AddPayPlayerAction(int cardId, PAY_PLAYER payType)
    {
        var card = await _cardContext.CardSet.Query().FirstOrDefaultAsync(c => c.Id == cardId);
        if (card == null) return false;

        var payPlayer = await _actionContext.PayPlayerActions.Query().FirstOrDefaultAsync(p => p.PayToType == payType
                                                                      && p.AmountToPlayer == card.Cost);
        if (payPlayer == null)
        {
            payPlayer = new PayPlayerAction
            {
                PayToType = payType,
                AmountToPlayer = card.Cost ?? 0
            };
            await _actionContext.PayPlayerActions.AddAsync(payPlayer);
        }

        await CreateAction(card.Id, payPlayer.Id, CARD_ACTION.PAY_PLAYER);
        return true;
    }

    public async Task<bool> AddStreetRepairsAction(int cardId, int house, int hotel)
    {
        var card = await _cardContext.CardSet.Query().FirstOrDefaultAsync(c => c.Id == cardId);
        if (card == null) return false;

        var sr = await _actionContext.StreetRepairsActions.Query()
            .FirstOrDefaultAsync(s => s.HouseCost == house && s.HotelCost == hotel);
        if (sr == null)
        {
            sr = new StreetRepairsAction
            {
                HouseCost = house,
                HotelCost = hotel
            };
            await _actionContext.StreetRepairsActions.AddAsync(sr);
        }

        await CreateAction(card.Id, sr.Id, CARD_ACTION.STREET_REPAIRS);
        return true;
    }

    public async Task<(CardAction?, ICardAction?)> GetCardAction(int cardId)
    {
        var card = await _cardContext.CardSet.Query().FirstOrDefaultAsync(c => c.Id == cardId);
        if (card == null) return (null, null);

        var cardAction = await _actionContext.CardActions.Query().FirstOrDefaultAsync(a => a.CardId == card.Id);
        if (cardAction == null) return (null, null);

        ICardAction? action = cardAction.Action switch
        {
            CARD_ACTION.ADVANCE =>
                await _actionContext.AdvanceActions.Query().FirstOrDefaultAsync(a => a.Id == cardAction.ActionId),
            CARD_ACTION.KEEP_CARD =>
                await _actionContext.KeepActions.Query().FirstOrDefaultAsync(a => a.Id == cardAction.ActionId),
            CARD_ACTION.PAY_PLAYER =>
                await _actionContext.PayPlayerActions.Query().FirstOrDefaultAsync(a => a.Id == cardAction.ActionId),
            CARD_ACTION.STREET_REPAIRS =>
                await _actionContext.StreetRepairsActions.Query().FirstOrDefaultAsync(a => a.Id == cardAction.ActionId),
            CARD_ACTION.MOVE => 
                await _actionContext.MoveActions.Query().FirstOrDefaultAsync(m => m.Id == cardAction.ActionId),
            CARD_ACTION.CHOICE =>
                await _actionContext.ChoiceActions.Query().FirstOrDefaultAsync(c => c.Id == cardAction.ActionId),
            _ => null
        };

        return (cardAction, action);
    }
}