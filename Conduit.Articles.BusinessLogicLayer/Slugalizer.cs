using System.Text;
using System.Text.RegularExpressions;
using Conduit.Articles.DomainLayer;
using Conduit.Articles.DomainLayer.Utilities;

namespace Conduit.Articles.BusinessLogicLayer;

public class Slugilizator : ISlugilizator
{
    private static readonly Dictionary<string, string> charReplacements = new()
    {
        ["а"] = "а",
        ["б"] = "b",
        ["в"] = "v",
        ["г"] = "g",
        ["д"] = "d",
        ["е"] = "ye",
        ["ё"] = "yo",
        ["ж"] = "zh",
        ["з"] = "z",
        ["и"] = "ee",
        ["й"] = "i",
        ["к"] = "k",
        ["л"] = "l",
        ["м"] = "m",
        ["н"] = "n",
        ["о"] = "o",
        ["п"] = "p",
        ["р"] = "r",
        ["с"] = "s",
        ["т"] = "t",
        ["у"] = "u",
        ["ф"] = "f",
        ["х"] = "h",
        ["ц"] = "ts",
        ["ч"] = "ch",
        ["ш"] = "sh",
        ["щ"] = "sh",
        ["ы"] = "i",
        ["э"] = "e",
        ["ю"] = "yu",
        ["я"] = "ya"
    };

    public string GetSlug(
        string title)
    {
        return GenerateSlug(title);
    }

    private static string ReplaceCharacters(
        string phrase)
    {
        var builder = new StringBuilder(phrase);
        for (var i = 0; i < builder.Length; i++)
        {
            var oldString = char.ToLower(builder[i]).ToString();
            var contains =
                charReplacements.TryGetValue(oldString, out var replacement);
            if (contains)
            {
                Console.WriteLine($"Replace {oldString} to {replacement}");
                builder.Replace(oldString, replacement, i, replacement.Length);
                i += replacement.Length - 1;
            }
        }

        return builder.ToString();
    }

    private static string GenerateSlug(
        string phrase)
    {
        var str = ReplaceCharacters(phrase.ToLower());

        str = Regex.Replace(str, @"[^a-z0-9\s-]",
            ""); // invalid chars          
        str = Regex.Replace(str, @"\s+", " ").Trim();

        if (str.Length >= 45)
        {
            str = str[..45].Trim(); // cut and trim it  
        }

        str = Regex.Replace(str, @"\s", "-"); // hyphens  
        str = str.Insert(str.Length,
            "-" + DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        return str;
    }
}
