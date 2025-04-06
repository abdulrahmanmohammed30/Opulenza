using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Opulenza.Application.Helpers;

public static class SlugHelper
{
    public static string GenerateSlug(string phrase)
    {
        string str = phrase.ToLowerInvariant();

        // Remove diacritics (e.g. converting é to e)
        str = RemoveDiacritics(str);

        // Remove invalid characters
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

        // Replace multiple spaces with a single space
        str = Regex.Replace(str, @"\s+", " ").Trim();

        // Replace spaces with hyphens
        str = Regex.Replace(str, @"\s", "-");

        return str;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();
        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }
        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}