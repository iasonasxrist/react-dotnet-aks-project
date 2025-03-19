using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api;

public static class ContractMapping
{
    public static Movie MapToMovie(this CreateMovieRequest request)
    {
        return new Movie
        {
            Title = request.Title,
            Id = Guid.NewGuid(),
            Genres = request.Genres.ToList(),
            YearOfRelease = request.YearOfRelease
        };
    }
    
    public static Movie MapToMovie(this UpdateMovieRequest request, Guid id)
    {
        return new Movie
        {
            Title = request.Title,
            Id = id,
            Genres = request.Genres.ToList(),
            YearOfRelease = request.YearOfRelease
        };
    }

    public static MovieResponse MapToResponse(this Movie movie)
    {
        return new MovieResponse()
        {
            Id = movie.Id,
            Title = movie.Title,
            Slug = movie.Slug,
            Genres = movie.Genres,
            YearOfRelease = movie.YearOfRelease
        };
    }

    public static MoviesResponse MapToResponse(this IEnumerable<Movie> movies)
    {
        return new MoviesResponse()
        {
            Items = movies.Select(MapToResponse)
        };
    }
}