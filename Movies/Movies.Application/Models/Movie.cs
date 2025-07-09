using System.Reflection;
using System.Text.RegularExpressions;

namespace Movies.Application.Models;

public partial class Movie :MovieBase
{
    public static string GenerateSlug(string Title, string YearOfRelease)
    {
        var sluggedTitle = SlugRegex().Replace(Title, string.Empty)
            .ToLower().Replace(" ", "-");
        return $"{sluggedTitle}-{YearOfRelease}";
    }

    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
    private static partial Regex SlugRegex();
}