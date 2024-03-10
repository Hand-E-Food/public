using PsiFi.Mapping.Actions;
using Rogue;
using System;

namespace PsiFi.Mapping.Mobs
{
    /// <summary>
    /// 
    /// </summary>
    public class RentACop : Mob
    {
        public override char Character => 'p';

        public override Color ForeColor => Color.Blue;

        public override int MaximumHealth => 10;

        public override string Name => "Rent-A-Cop";

        public override IAction Act()
        {
            throw new NotImplementedException();
        }
    }
}
