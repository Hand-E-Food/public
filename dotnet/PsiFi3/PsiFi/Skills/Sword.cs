using PsiFi.Abilities;

namespace PsiFi.Skills
{
    /// <summary>
    /// The default sword weapon.
    /// </summary>
    public class Sword : Weapon
    {
        public override string Name => "Sword";
        public override School School => School.Default;
     
        public Sword(Protagonist protagonist) : base(protagonist)
        {
            Ability = new AttackAbility(protagonist, Name, new Damage(2));
        }
    }
}
