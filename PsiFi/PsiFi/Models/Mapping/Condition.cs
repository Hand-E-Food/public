using System;

namespace PsiFi.Models.Mapping
{
    static class Condition
    {
        /// <summary>
        /// Checks whether a mob is dead.
        /// </summary>
        /// <param name="mob">The mob to check.</param>
        /// <param name="signal">the signal to return when this condition is met.</param>
        /// <returns>A function that returns the <paramref name="signal"/> when the <paramref name="mob"/> has zero health.</returns>
        public static Func<object> IsDead(Mob mob, object signal) => () => mob.Health.Value <= 0 ? signal : null;
    }
}
