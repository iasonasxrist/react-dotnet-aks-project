using Microsoft.EntityFrameworkCore;
using Movies.Application.Models;

namespace Movies.Api;

public class MovieDbContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }
    public MoviesDbContext(DbContextOptions<MoviesDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("movies");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Slug).IsRequired().HasMaxLength(255);
            entity.Property(m => m.Title).IsRequired().HasMaxLength(255);
            entity.Property(m => m.YearOfRelease).IsRequired();
            entity.HasIndex(m => m.Slug).IsUnique().HasDatabaseName("movies_slug_idx");
        });
    }
}
