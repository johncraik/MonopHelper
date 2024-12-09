using Microsoft.EntityFrameworkCore;
using MonopolyCL.Models.Properties.Enums;

namespace MonopolyCL.Models.Cards.Actions;

[PrimaryKey(nameof(AdvanceIndex))]
public class AdvanceAction : ICardAction
{
    public int AdvanceIndex { get; set; }
    public PROP_SET Set { get; set; }
    public string Colour { get; private set; }

    public void SetColour()
    {
        Colour = Set switch
        {
            PROP_SET.BROWN => "prop-0",
            PROP_SET.BLUE => "prop-1",
            PROP_SET.PINK => "prop-2",
            PROP_SET.ORANGE => "prop-3",
            PROP_SET.RED => "prop-4",
            PROP_SET.YELLOW => "prop-5",
            PROP_SET.GREEN => "prop-6",
            PROP_SET.DARK_BLUE => "prop-7",
            PROP_SET.STATION => "prop-8",
            PROP_SET.UTILITY => "prop-9",
            _ => "bg-secondary"
        };
    }
}