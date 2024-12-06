namespace MonopolyCL.Models.Game;

[Flags]
public enum GameRules
{
    Base = 0,
    DoubleRule = 1,
    Directional = 2,
    NumberRule = 4,
    FreeParking = 8,
    Streets = 16,
    Reservations = 32,
    TripleRule = 64,
    Loans = 128,
    JailRule = 256
}