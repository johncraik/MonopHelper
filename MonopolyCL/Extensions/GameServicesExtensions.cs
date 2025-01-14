using Microsoft.Extensions.DependencyInjection;
using MonopolyCL.Helpers;
using MonopolyCL.Models.Cards;
using MonopolyCL.Services.Boards;
using MonopolyCL.Services.Cards;
using MonopolyCL.Services.Game;
using MonopolyCL.Services.Players;
using MonopolyCL.Services.Properties;

namespace MonopolyCL.Extensions;

public static class GameServicesExtensions
{
    public static IServiceCollection GetGameServices(this IServiceCollection services)
    {
        services.AddTransient<ShuffleList<Card>>();
        services.AddTransient<CardService>();
        services.AddTransient<CardGameService>();
        
        services.AddTransient<GeneralBoardSpaces>();

        services.AddScoped<ColPropCreator>();
        services.AddScoped<StationPropCreator>();
        services.AddScoped<UtilPropCreator>();
        
        services.AddScoped<PlayerCreator>();
        services.AddScoped<PlayerService>();
        services.AddScoped<BoardCreator>();
        services.AddScoped<BoardService>();

        services.AddTransient<MonopolyGameService>();
        services.AddTransient<CardActionsService>();
        
        services.AddScoped<CsvReader<CardUpload>>();
        
        return services;
    }
}