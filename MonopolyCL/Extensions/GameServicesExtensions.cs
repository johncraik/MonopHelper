using Microsoft.Extensions.DependencyInjection;
using MonopHelper.Data;
using MonopolyCL.Models.Boards.DataModel;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Game;
using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Models.Properties.DataModel;
using MonopolyCL.Services.Boards;
using MonopolyCL.Services.Players;
using MonopolyCL.Services.Properties;

namespace MonopolyCL.Extensions;

public static class GameServicesExtensions
{
    public static IServiceCollection GetGameServices(this IServiceCollection services)
    {
        services.AddScoped<GameDbSet<Card>>();
        services.AddScoped<GameDbSet<CardType>>();
        services.AddScoped<GameDbSet<CardDeck>>();
        services.AddScoped<GameDbSet<CardGame>>();
        services.AddScoped<GameDbSet<CardToGame>>();
        services.AddScoped<GameDbSet<TypeToGame>>();

        services.AddScoped<GameDbSet<BoardDM>>();
        services.AddScoped<GameDbSet<BoardToProperty>>();
        services.AddScoped<GameDbSet<PropertyDM>>();
        services.AddScoped<GameDbSet<GameProperty>>();
        services.AddScoped<GameDbSet<PlayerDM>>();
        services.AddScoped<GameDbSet<DiceNumbers>>();
        services.AddScoped<GameDbSet<GamePlayer>>();
        services.AddScoped<GameDbSet<GameDM>>();

        services.AddTransient<GeneralBoardSpaces>();

        services.AddScoped<ColPropCreator>();
        services.AddScoped<StationPropCreator>();
        services.AddScoped<UtilPropCreator>();
        
        services.AddScoped<PlayerCreator>();
        services.AddScoped<BoardCreator>();
        
        services.AddScoped<CsvReader<CardUpload>>();
        
        return services;
    }
}