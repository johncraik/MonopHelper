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
using MonopolyCL.Services.Cards;

namespace MonopolyCL.Extensions;

public static class ContextExtensions
{
    public static IServiceCollection CreateContextServices(this IServiceCollection services)
    {
        //Cards:
        services.AddScoped<CardContext>();
        services.AddScoped<CardActionContext>();
        
        //services.AddScoped<GameDbSet<DiceNumbers>>();
        //services.AddScoped<GameDbSet<PlayerToCard>>();
        
        //Games:
        services.AddScoped<BoardContext>();
        services.AddScoped<PlayerContext>();
        services.AddScoped<GameContext>();
        
        return services;
    }
}