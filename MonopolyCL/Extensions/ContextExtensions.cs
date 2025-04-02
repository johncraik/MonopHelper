using Microsoft.Extensions.DependencyInjection;
using MonopolyCL.Data;
namespace MonopolyCL.Extensions;

public static class ContextExtensions
{
    public static IServiceCollection CreateContextServices(this IServiceCollection services)
    {
        //Cards:
        services.AddScoped<CardContext>();
        
        //Games:
        services.AddScoped<BoardContext>();
        services.AddScoped<PlayerContext>();
        services.AddScoped<GameContext>();
        
        return services;
    }
}