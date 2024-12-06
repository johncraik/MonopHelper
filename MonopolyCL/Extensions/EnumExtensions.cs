using System.Globalization;

namespace MonopolyCL.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum e) =>
        CultureInfo.CurrentCulture.TextInfo.ToTitleCase(e.ToString().Replace('_', ' ').ToLower());
}