namespace PsiFi.Skills
{
    public abstract class Weapon : Skill
    {
        public override School School => School.Weapon;

        public Weapon(Protagonist protagonist) : base(protagonist) { }
    }
}
