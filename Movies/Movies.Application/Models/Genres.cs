using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Movies.Application.Models
{
  [Table(("Genres"))]
  public class Genre
  {
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [Required]
    public required string Name { get; init; }

    public virtual ICollection<MovieGenre> MovieGenres { get; set; } = [];
  }
}