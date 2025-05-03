using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Application.Models;

[Table(("Movies"))]
public class MovieBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required Guid Id { get; init; }
    
    [Required]
    public required string Title { get; set; }

    [Required]
    public string Slug { get; set; }

    [Required]
    public required int YearOfRelease { get; set; }
    
    [Required]
    public required List<string> Genres { get; init; } = new();
}