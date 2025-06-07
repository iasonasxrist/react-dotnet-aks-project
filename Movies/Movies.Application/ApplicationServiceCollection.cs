using System.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Database;
using Movies.Application.Repositories;
using Movies.Application.Services;

namespace Movies.Application;

public static class ApplicationServiceCollection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IMovieService,MovieService>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Scoped);
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<MovieDbContext>(options => options.UseSqlServer(connectionString));
        services.AddSingleton<IDbConnectionFactory>(_=>new SqlConnectionFactory(connectionString));
        services.AddScoped<DbInitializer>();
        return services;
    }
}
