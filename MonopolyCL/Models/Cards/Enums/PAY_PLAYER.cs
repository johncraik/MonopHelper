namespace MonopolyCL.Models.Cards.Enums;

public enum PAY_PLAYER
{
    ALL,
    LEFT,
    RIGHT
}

public static class PayPlayerExtensions
{
    public static List<PAY_PLAYER> GetPayTypeList() => [PAY_PLAYER.ALL, PAY_PLAYER.LEFT, PAY_PLAYER.RIGHT];
}