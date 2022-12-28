namespace PsiFi
{
    /// <summary>
    /// A classificaion of skill.
    /// </summary>
    public enum School
    {
        Default = 0,
        Athlete = 1,
        Psionic = 2,
        Technology = 4,
        Weapon = 8,
        Evolution = Athlete | Psionic,
        Cyborg = Athlete | Technology,
        Assassin = Athlete | Weapon,
        Psiborg = Psionic | Technology,
        Psiconaut = Psionic | Weapon,
        Mechanaut = Technology | Weapon,
    }
}
