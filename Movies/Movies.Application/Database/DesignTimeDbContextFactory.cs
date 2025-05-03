using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Movies.Application.Database;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MovieDbContext>
{
    public MovieDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configurationRoot = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var builder = new DbContextOptionsBuilder<MovieDbContext>();
        var connectionString = configurationRoot.GetConnectionString("DefaultConnection");
        
        builder.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(180),
                errorNumbersToAdd: null);
        });
        return new MovieDbContext(builder.Options);
    }
}