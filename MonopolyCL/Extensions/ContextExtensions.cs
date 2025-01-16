using Microsoft.Extensions.DependencyInjection;
using MonopHelper.Data;
using MonopolyCL.Data;
using MonopolyCL.Models.Boards.DataModel;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Cards.Actions;
using MonopolyCL.Models.Cards.Game;
using MonopolyCL.Models.Game;
using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Models.Properties.DataModel;

namespace MonopolyCL.Extensions;

public static class ContextExtensions
{
    public static IServiceCollection CreateContextServices(this IServiceCollection services)
    {
        //Cards:
        services.AddScoped<GameDbSet<Card>>();
        services.AddScoped<GameDbSet<CardType>>();
        services.AddScoped<GameDbSet<CardDeck>>();
        services.AddScoped<GameDbSet<CardGame>>();
        services.AddScoped<GameDbSet<CardToGame>>();
        services.AddScoped<GameDbSet<TypeToGame>>();
        services.AddScoped<CardContext>();
        
        //Card Actions:
        services.AddScoped<GameDbSet<CardAction>>();
        services.AddScoped<GameDbSet<AdvanceAction>>();
        services.AddScoped<GameDbSet<MoveAction>>();
        services.AddScoped<GameDbSet<KeepAction>>();
        services.AddScoped<GameDbSet<ChoiceAction>>();
        services.AddScoped<GameDbSet<PayPlayerAction>>();
        services.AddScoped<GameDbSet<StreetRepairsAction>>();

        //Boards & Properties:
        services.AddScoped<GameDbSet<BoardDM>>();
        services.AddScoped<GameDbSet<BoardToProperty>>();
        services.AddScoped<GameDbSet<PropertyDM>>();
        services.AddScoped<GameDbSet<GameProperty>>();
        
        //Players:
        services.AddScoped<GameDbSet<PlayerDM>>();
        services.AddScoped<GameDbSet<DiceNumbers>>();
        services.AddScoped<GameDbSet<GamePlayer>>();
        services.AddScoped<GameDbSet<PlayerToCard>>();
        
        //Games:
        services.AddScoped<GameDbSet<GameDM>>();
        services.AddScoped<GameDbSet<TurnOrder>>();
        services.AddScoped<GameDbSet<GameCard>>();
        services.AddScoped<GameDbSet<GameType>>();
        
        return services;
    }
}