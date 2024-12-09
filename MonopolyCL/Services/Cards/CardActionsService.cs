using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Cards.Actions;
using MonopolyCL.Models.Cards.Enums;

namespace MonopolyCL.Services.Cards;

public class CardActionsService
{
    private readonly UserInfo _userInfo;
    private readonly GameDbSet<Card> _cardSet;
    private readonly GameDbSet<CardAction> _actionSet;
    private readonly GameDbSet<AdvanceAction> _advSet;
    private readonly GameDbSet<MoveAction> _moveSet;
    private readonly GameDbSet<KeepAction> _keepSet;
    private readonly GameDbSet<PayPlayerAction> _ppSet;
    private readonly GameDbSet<StreetRepairsAction> _srSet;

    public CardActionsService(UserInfo userInfo,
        GameDbSet<Card> cardSet,
        GameDbSet<CardAction> actionSet,
        GameDbSet<AdvanceAction> advSet,
        GameDbSet<MoveAction> moveSet,
        GameDbSet<KeepAction> keepSet,
        GameDbSet<PayPlayerAction> ppSet,
        GameDbSet<StreetRepairsAction> srSet)
    {
        _userInfo = userInfo;
        _cardSet = cardSet;
        _actionSet = actionSet;
        _advSet = advSet;
        _moveSet = moveSet;
        _keepSet = keepSet;
        _ppSet = ppSet;
        _srSet = srSet;
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
        await _actionSet.AddAsync(cardAction);
    }

    public async Task<bool> AddAdvanceAction(int cardId, int index)
    {
        var card = await _cardSet.Query().FirstOrDefaultAsync(c => c.Id == cardId);
        if (card == null) return false;

        var advance = await _advSet.Query().FirstOrDefaultAsync(a => a.AdvanceIndex == index);
        if (advance == null)
        {
            advance = new AdvanceAction
            {
                AdvanceIndex = index
            };
            await _advSet.AddAsync(advance);
        }

        await CreateAction(card.Id, advance.AdvanceIndex, CARD_ACTION.ADVANCE);
        return true;
    }

    public async Task<bool> AddMoveAction(int cardId, int amount, bool isForward)
    {
        var card = await _cardSet.Query().FirstOrDefaultAsync(c => c.Id == cardId);
        if (card == null) return false;

        var move = await _moveSet.Query().FirstOrDefaultAsync(m => m.MoveAmount == amount && m.IsForward == isForward);
        if (move == null)
        {
            move = new MoveAction
            {
                MoveAmount = amount,
                IsForward = isForward
            };
            await _moveSet.AddAsync(move);
        }

        await CreateAction(card.Id, move.Id, CARD_ACTION.MOVE);
        return true;
    }

    public async Task<bool> AddKeepAction(int cardId)
    {
        var card = await _cardSet.Query().FirstOrDefaultAsync(c => c.Id == cardId);
        if (card == null) return false;

        var keep = new KeepAction();
        await _keepSet.AddAsync(keep);
        await CreateAction(card.Id, keep.Id, CARD_ACTION.KEEP_CARD);
        return true;
    }

    public async Task<bool> AddPayPlayerAction(int cardId, PAY_PLAYER payType)
    {
        var card = await _cardSet.Query().FirstOrDefaultAsync(c => c.Id == cardId);
        if (card == null) return false;

        var payPlayer = await _ppSet.Query().FirstOrDefaultAsync(p => p.PayToType == payType
                                                                      && p.AmountToPlayer == card.Cost);
        if (payPlayer == null)
        {
            payPlayer = new PayPlayerAction
            {
                PayToType = payType,
                AmountToPlayer = card.Cost ?? 0
            };
            await _ppSet.AddAsync(payPlayer);
        }

        await CreateAction(card.Id, payPlayer.Id, CARD_ACTION.PAY_PLAYER);
        return true;
    }

    public async Task<bool> AddStreetRepairsAction(int cardId, int house, int hotel)
    {
        var card = await _cardSet.Query().FirstOrDefaultAsync(c => c.Id == cardId);
        if (card == null) return false;

        var sr = await _srSet.Query().FirstOrDefaultAsync(s => s.HouseCost == house
                                                                    && s.HotelCost == hotel);
        if (sr == null)
        {
            sr = new StreetRepairsAction
            {
                HouseCost = house,
                HotelCost = hotel
            };
            await _srSet.AddAsync(sr);
        }

        await CreateAction(card.Id, sr.Id, CARD_ACTION.STREET_REPAIRS);
        return true;
    }

    public async Task<(CardAction?, ICardAction?)> GetCardAction(int cardId)
    {
        var card = await _cardSet.Query().FirstOrDefaultAsync(c => c.Id == cardId);
        if (card == null) return (null, null);

        var cardAction = await _actionSet.Query().FirstOrDefaultAsync(a => a.CardId == card.Id);
        if (cardAction == null) return (null, null);

        ICardAction? action = cardAction.Action switch
        {
            CARD_ACTION.ADVANCE =>
                await _advSet.Query().FirstOrDefaultAsync(a => a.AdvanceIndex == cardAction.ActionId),
            CARD_ACTION.KEEP_CARD =>
                await _keepSet.Query().FirstOrDefaultAsync(a => a.Id == cardAction.ActionId),
            CARD_ACTION.PAY_PLAYER =>
                await _ppSet.Query().FirstOrDefaultAsync(a => a.Id == cardAction.ActionId),
            CARD_ACTION.STREET_REPAIRS =>
                await _srSet.Query().FirstOrDefaultAsync(a => a.Id == cardAction.ActionId),
            CARD_ACTION.MOVE => 
                await _moveSet.Query().FirstOrDefaultAsync(m => m.Id == cardAction.ActionId),
            _ => null
        };

        return (cardAction, action);
    }
}