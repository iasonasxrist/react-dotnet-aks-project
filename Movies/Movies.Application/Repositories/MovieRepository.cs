using Dapper;
using Microsoft.EntityFrameworkCore;
using Movies.Application.Database;
using Movies.Application.Models;
namespace Movies.Application.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly MovieDbContext _movieDbContext;

    public MovieRepository (MovieDbContext movieDbContext)
    {
        _movieDbContext = movieDbContext;
    }
  
  public async Task<Movie?> CreateAsync(Movie movie, CancellationToken token = default)
    {
        // Remove the outer 'using' for _dbContext (let DI handle disposal)
        try
        {
            // Start transaction
            using (var transaction = await _movieDbContext.Database.BeginTransactionAsync(token))
            {
                try
                {
                    var newMovie = _movieDbContext.Movies.Add(new Movie()
                    {
                        Slug = Movie.GenerateSlug(movie.Title, movie.YearOfRelease.ToString()),
                        Title = movie.Title,
                        YearOfRelease = movie.YearOfRelease,
                    });

                    await _movieDbContext.SaveChangesAsync(token);
                    await transaction.CommitAsync(token); // Use CommitAsync
                    Console.WriteLine("Transaction committed successfully.");
                    return newMovie.Entity;
                }
                catch
                {
                    await transaction.RollbackAsync(token); // Explicit rollback
                    Console.WriteLine("Transaction rolled back due to an error.");
                    throw; // Re-throw to preserve the original exception
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }

    }       

    public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
            try
            {
                var fetchMovie = await _movieDbContext.Movies.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: token);
                return fetchMovie;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
    }

    public async Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default)
    {
        try
        {
            var fetchMovie = await _movieDbContext.Movies.FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken: token);
            return fetchMovie;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default)
    {
       
            return await _movieDbContext.Movies.Include(x => x.MovieGenres)
                         .ThenInclude(x => x.Genre)
                         .ToListAsync();
     }

    public async Task<bool> UpdateAsync(Movie movie, CancellationToken token = default)
    {
        var fetchMovie = await _movieDbContext.Movies.FirstOrDefaultAsync(x => x.Id ==movie.Id, cancellationToken: token);
        if (fetchMovie is null)
        {
            return false;
        }

        // Start transaction
        using (var transaction = await _movieDbContext.Database.BeginTransactionAsync(token))
        {
            try
            {
                var newMovie = _movieDbContext.Movies.Update(movie);

                await _movieDbContext.SaveChangesAsync(token);
                transaction.Commit();
                Console.WriteLine("Transaction committed successfully.");
                return true;
            }
            catch
            {
                await transaction.RollbackAsync(token); // Explicit rollback
                Console.WriteLine("Transaction rolled back due to an error.");
                return false; 
            }
        }
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {      
      var fetchMovie = await _movieDbContext.Movies.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: token);
        if (fetchMovie is not null) {
          _movieDbContext.Movies.Remove(fetchMovie);
          await _movieDbContext.SaveChangesAsync();
          return true;
        }
        return false;
    }

  public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
  {
    var fetchMovie = await _movieDbContext.Movies.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: token);
    if(fetchMovie is not null) {
      return true;
    }
    return false;
  }
}