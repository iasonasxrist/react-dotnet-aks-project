using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Movies.Application;
using Movies.Application.Models;
using Movies.Contracts.Requests;
namespace Movies.Api.Controller;

[ApiController]
public class MoviesContoller : ControllerBase
{
   private readonly IMovieRepository _movieRepository;
   private readonly ShardingService _shardingService;

   public MoviesContoller(IMovieRepository movieRepository)
   {
       _movieRepository = movieRepository;
   }
    [HttpPost(ApiEndpoints.Movies.Create)]
   public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
   {
       var movie = request.MapToMovie();
       await _movieRepository.CreateAsync(movie);
       
       return CreatedAtAction(nameof(Get), new {idOrSlug = movie.Id}, movie);
   }
   
   [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug)
    {
        var movie = Guid.TryParse(idOrSlug, out var id) ? await _movieRepository.GetByIdAsync(id) : await _movieRepository.GetBySlugAsync(idOrSlug);
        if (movie is null)
        {
            return NotFound();
        }

        var response = movie.MapToResponse();
        return Ok(response);
        // using (var connection = _shardingService.GetShardConnection(id))
        // {
        //     await connection.OpenAsync();
        //     var command = new SqlCommand("SELECT * FROM Movie WHERE Id = @Id", connection);
        //     command.Parameters.AddWithValue("@MovieId", id);
        //     var reader = await command.ExecuteReaderAsync();
        //     if (await reader.ReadAsync())
        //     {
        //         var movie = new
        //         {
        //             Id = reader.GetGuid(0),
        //             Title = reader["Title"],
        //             YearOfRelease = reader["yearOfRelease"],
        //             Genre = reader["Genres"],
        //             Rating = reader["Rating"]
        //         };
        //         return Ok(movie);
        //     }
        //     return NotFound();
        // }
    }

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _movieRepository.GetAllAsync();
        var response = movies.MapToResponse();
        return Ok(response);
    }

    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request)
    {
        var movie =  request.MapToMovie(id);
        var updated = await _movieRepository.UpdateAsync(movie);
        if (!updated)
        {
            return NotFound();
        }
        var response = movie.MapToResponse();
        return Ok(response);
    }
    
    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Update([FromRoute] Guid id)
    {
        var deleted = await _movieRepository.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return Ok();
    }

    
}