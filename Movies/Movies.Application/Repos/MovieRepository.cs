using Movies.Application.Models;

namespace Movies.Application;

public class MovieRepository : IMovieRepository
{       
    List<Movie> _movies = new();
    public Task<bool> CreateAsync(Movie movie)
    {
        _movies.Add(movie);
        return Task.FromResult(_movies.Count > 0);
    }
    
    public Task<Movie?> GetByIdAsync(Guid id)
    {
        var movie = _movies.SingleOrDefault(x => x.Id == id);
        return Task.FromResult(movie);
    }

    public Task<Movie> GetBySlugAsync(string slug)
    {
        var movie = _movies.SingleOrDefault(x => x.Slug == slug);
        return Task.FromResult(movie);
    }

    public Task<IEnumerable<Movie>> GetAllAsync()
    
    {
            return Task.FromResult(_movies.AsEnumerable());
            
    }

    public Task<bool> UpdateAsync(Movie movie)
    {
        var moviesIndex = _movies.FindIndex(x => x.Id == movie.Id);
        if (moviesIndex != -1)
        {
            return Task.FromResult(false);
            
        }
        _movies[moviesIndex] = movie;
        return Task.FromResult(true);

    }

    public Task<bool> DeleteAsync(Guid id)
    {
       var movies = _movies.RemoveAll(x => x.Id == id);
       var moviesDeleted = movies > 0;
       return Task.FromResult(moviesDeleted);
    }
}