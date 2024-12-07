namespace MonopolyCL.Models.Game;

/// <summary>
/// Optional Game Rules:
/// <list type="bullet">
///     <item>
///         <term>BASE (0): </term>
///         <description>Base monopoly rules</description>
///     </item>
/// <item>
///         <term>DOUBLE_RULES (1): </term>
///         <description>Adds rules for rolling doubles. Includes paying/receiving double (excluding rent).</description>
///     </item>
/// <item>
///         <term>DIRECTIONAL (2): </term>
///         <description>Adds the ability to change direction when rolling a double.</description>
///     </item>
/// <item>
///         <term>NUMBER_RULE (4): </term>
///         <description>Adds 'your number' to the game. Defined when starting a game. Includes rules relating to 'your number'.</description>
///     </item>
/// <item>
///         <term>FREE_PARKING (8): </term>
///         <description>Includes all the rules relating to free parking (max 1,000, hand in property, pay when no money, purging, etc)</description>
///     </item>
/// <item>
///         <term>STREETS (16): </term>
///         <description>Adds the street rule to the game. When owning all properties in the street, building is half price.</description>
///     </item>
/// <item>
///         <term>RESERVATIONS (32): </term>
///         <description>Adds the reservation rule to the game. No player can have a set until all players have a set. Includes all sub-rules of reservations.</description>
///     </item>
/// <item>
///         <term>TRIPLE_RULE (64): </term>
///         <description>Adds a third dice to the game. All other players move on the third dice. Rolling a triple gives you £1,000, increasing by £500 each time.</description>
///     </item>
/// <item>
///         <term>LOANS (128): </term>
///         <description>Can get loans from the bank. Maximum of three loans.</description>
///     </item>
/// <item>
///         <term>JAIL_RULE (256): </term>
///         <description>Cost to leave jail increases by 50% each time.</description>
///     </item>
/// </list>
/// </summary>
[Flags]
public enum GAME_RULES
{
    BASE = 0,
    DOUBLE_RULES = 1,
    DIRECTIONAL = 2,
    NUMBER_RULE = 4,
    FREE_PARKING = 8,
    STREETS = 16,
    RESERVATIONS = 32,
    TRIPLE_RULE = 64,
    LOANS = 128,
    JAIL_RULE = 256
}