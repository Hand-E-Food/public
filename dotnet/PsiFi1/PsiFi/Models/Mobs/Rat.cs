using PsiFi.Brains;
using PsiFi.Rules.Effects;
using System;

namespace PsiFi.Models.Mobs
{
    class Rat : Mob
    {
        public override IBrain? Brain { get; }

        public Rat() : base(new Appearance('r', ConsoleColor.DarkYellow))
        {
            Brain = new AnimalBrain(this);
            MeleeEffects = new[] { new DamageEffect(new DamageRange("1d3")) };
            WalkDuration = 13;
        }
    }
}
