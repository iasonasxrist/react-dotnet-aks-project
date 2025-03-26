using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Database;

namespace Movies.Application;

public static class ApplicationServiceCollection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // services.AddSingleton<ShardingService>();
        services.AddSingleton<IMovieRepository, MovieRepository>();
        return services;

    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_=> new NpgsqlConnectionFactory(connectionString));
        return services;
    }
}