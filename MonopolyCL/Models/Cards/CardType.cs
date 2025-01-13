using System.Text.RegularExpressions;
using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Cards;

public class CardType : TenantedModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsDeleted { get; set; }

    private string? _Colour = "#7afaba";
    public string Colour
    {
        get => _Colour ?? "#7afaba";
        set
        {
            var val = value;
            if (val[0] != '#')
            {
                val = $"#{val}";
            }

            if (Regex.IsMatch(val, @"^#([0-9A-Fa-f]{6}|[0-9A-Fa-f]{3})$"))
            {
                _Colour = val;
            }
        }
    }

    public string HoverColour()
    {
        var rgb = ExtractRGB();
        
        rgb.R = (int)(rgb.R + (255 - rgb.R) * 0.4);
        rgb.R = Math.Min(255, Math.Max(0, rgb.R));
        
        rgb.G = (int)(rgb.G + (255 - rgb.G) * 0.4);
        rgb.G = Math.Min(255, Math.Max(0, rgb.G));
        
        rgb.B = (int)(rgb.B + (255 - rgb.B) * 0.4);
        rgb.B = Math.Min(255, Math.Max(0, rgb.B));

        return $"#{rgb.R:X2}{rgb.G:X2}{rgb.B:X2}";
    }

    public string FontColour()
    {
        var rgb = ExtractRGB();

        var rNormalised = rgb.R / 255.0;
        var gNormalised = rgb.G / 255.0;
        var bNormalised = rgb.B / 255.0;

        var luminance = 0.2126 * rNormalised + 0.7152 * gNormalised + 0.0722 * bNormalised;
        return luminance > 0.5 ? "#000000" : "#ffffff";
    }

    private (int R, int G, int B) ExtractRGB()
    {
        var hex = Colour[1..];
        
        var r = Convert.ToInt32(hex[..2], 16);
        var g = Convert.ToInt32(hex[2..4], 16);
        var b = Convert.ToInt32(hex[4..6], 16);

        return (r, g, b);
    }
}