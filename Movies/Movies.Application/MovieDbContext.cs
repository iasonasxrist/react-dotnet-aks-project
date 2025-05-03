using Microsoft.EntityFrameworkCore;
using Movies.Application.Models;

namespace Movies.Application;

public class MovieDbContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }
    public MovieDbContext(DbContextOptions<MovieDbContext> options)
        : base(options)
    {
    }

    public MovieDbContext()
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasIndex(m => m.Slug).IsUnique().HasDatabaseName("movies_slug_idx");
        });
        SeedMovies(modelBuilder);
    }

    protected static void SeedMovies(ModelBuilder builder)
    {
        var guid1 = new Guid("573fd5ec-5a81-4c68-ac27-d4725f38d634");
        var guid2 = new Guid("9c9565fd-4de5-418f-84fd-d76a19df58f4");
        var guid3 = new Guid("0f72d19d-2c8b-49fc-8f9b-061138daeb6d");
        builder.Entity<Movie>().HasData(
            new MovieBase()
            {
                Id = guid1,
                Title = "The Dark Knight",
                Genres = ["Scifi", "Adventures"],
                YearOfRelease = 2024,
                Slug = "the_dark_knight",
            },
            new MovieBase()
            {
                Id = guid2,
                Title = "Harry Potter and the Philosopher",
                Genres = ["Scifi", "Adventures"],
                YearOfRelease = 1999,
                Slug = "harry_potter_and_the_philosopher",
            },
            new MovieBase()
            {
                Id = guid3,
                Title = "Mission impossing and the philosopher",
                Genres = ["Horror", "Adventures"],
                YearOfRelease = 2010,
                Slug = "mission_impossing_and_the_philosopher",
            }
        );
    }
}
