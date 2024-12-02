namespace MonopolyCL.Models.Enums;

[Flags]
public enum GameRules
{
    Base = 1,
    DoubleRule = 2,
    Directional = 4,
    NumberRule = 8,
    FreeParking = 16,
    Streets = 32,
    Reservations = 64,
    TripleRule = 128,
    Loans = 256,
    JailRule = 512
}