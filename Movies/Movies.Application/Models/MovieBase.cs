using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Application.Models;


[Table(("Movies"))]
public class MovieBase
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public Guid Id { get; init; }

  [Required]
  public required string Title { get; set; }

  [Required]
  public required string Slug { get; set; }

  [Required]
  public required int YearOfRelease { get; set; }

  [Required]
  public virtual ICollection<MovieGenre> MovieGenres { get; set; } = [];
}