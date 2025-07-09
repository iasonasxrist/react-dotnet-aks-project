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

    /*   public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
       {
           using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
           using var transaction = connection.BeginTransaction();

           var result = await connection.ExecuteAsync(new CommandDefinition("""
               insert into movies (id, slug, title, yearofrelease) 
               values (@Id, @Slug, @Title, @YearOfRelease)
               """, movie, cancellationToken: token));

           if (result > 0)
           {
               foreach (var genre in movie.Genres)
               {
                   await connection.ExecuteAsync(new CommandDefinition("""
                       insert into genres (movieId, name) 
                       values (@MovieId, @Name)
                       """, new { MovieId = movie.Id, Name = genre }, cancellationToken: token));
               }
           }
           transaction.Commit();

           return result > 0;
       }
    */
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
       
            //return await _movieDbContext.Movies.Include(x => x.Genres).ToListAsync();
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
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();
        
        await connection.ExecuteAsync(new CommandDefinition("""
            delete from genres where movieid = @id
            """, new { id }, cancellationToken: token));
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
            delete from movies where id = @id
            """, new { id }, cancellationToken: token));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
            select count(1) from movies where id = @id
            """, new { id }, cancellationToken: token));
    }
}
