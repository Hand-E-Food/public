namespace ColourBlind
{
    [Flags]
    public enum PlayerId
    {
        None = 0,
        A = 1,
        B = 2,
        AB = A | B,
    }
}
