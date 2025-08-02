using Movies.Application.Models;
using Movies.Contracts.Requests;

public interface IGenreRepository
{
  Task<bool> CreateRequestGenresAsync(CreateMovieRequest request, CancellationToken cancellationToken);
  bool GenreExists(string name);
  Task MapGenresToMoviesAsync(Movie newMovie, CreateMovieRequest request, CancellationToken cancellationToken);
}