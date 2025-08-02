using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Movies.Application.Models
{
  [Table(("MovieGenres"))]
  public class MovieGenre
  {
    [Key, Column(Order = 0)]
    [ForeignKey("Movie")]
    public Guid MovieId { get; set; }

    [Key, Column(Order = 1)]
    [ForeignKey("Genre")]
    public int GenreId { get; set; }

    public virtual MovieBase Movie { get; set; }
    public virtual Genre Genre { get; set; }
  }
}