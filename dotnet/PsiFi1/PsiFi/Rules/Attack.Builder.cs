using PsiFi.Geometry;
using PsiFi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static PsiFi.Geometry.Math;

namespace PsiFi.Rules
{
    partial class Attack
    {
        /// <summary>
        /// Starts an attack from the specified <paramref name="origin"/>.
        /// </summary>
        /// <param name="origin">The mob launching the attack.</param>
        /// <returns>An attack builder for method chaining.</returns>
        public static Builder2 From(Mob origin) => Builder.Create().From(origin);

        /// <summary>
        /// Helps build an attack using method chaining.
        /// </summary>
        public class Builder : Builder1, Builder2, Builder3, Builder4, Builder5
        {
            /// <summary>
            /// Creates a new attack builder.
            /// </summary>
            /// <returns>The attack builder for method chaining.</returns>
            public static Builder1 Create() => new Builder();

            private Attack attack = new Attack();

            private Builder() { }

            /// <inheritdoc/>
            public Builder2 From(Mob origin)
            {
                attack.Origin = origin;
                return this;
            }

            /// <inheritdoc/>
            public Builder3 MeleeTo(Cell target)
            {
                attack.Target = target;
                return this;
            }

            /// <inheritdoc/>
            public Builder3 RangedTo(Cell target, Appearance appearance)
            {
                if (attack.Origin.Cell == null) throw new InvalidOperationException("Cannot include a path because the origin does not have a location.");
                attack.Target = target;
                attack.Appearance = appearance;
                attack.Path = GetStraightPath(attack.Origin.Cell.Location, target.Location).ToList();
                return this;
            }

            /// <inheritdoc/>
            public Builder3 RangedTo(Cell target, Appearance appearance, IList<Point> path)
            {
                attack.Target = target;
                attack.Appearance = appearance;
                attack.Path = path;
                return this;
            }

            /// <inheritdoc/>
            public Builder5 DirectTo(Cell target)
            {
                attack.Target = target;
                attack.Accuracy = null;
                return this;
            }

            /// <inheritdoc/>
            public Builder3 AdjustAccuracy(int delta)
            {
                attack.Accuracy += delta;
                return this;
            }

            /// <inheritdoc/>
            public Builder4 IfMisses(params IEffect[] effects)
            {
                attack.MissEffects = effects ?? EmptyEffects;
                return this;
            }

            /// <inheritdoc/>
            public Attack IfHits(params IEffect[] effects)
            {
                attack.HitEffects = effects ?? EmptyEffects;
                return attack;
            }

            /// <inheritdoc/>
            public Attack WithEffect(params IEffect[] effects)
            {
                attack.MissEffects = effects ?? EmptyEffects;
                attack.HitEffects = effects ?? EmptyEffects;
                return attack;
            }
        }

        public interface Builder1
        {
            /// <summary>
            /// Sets this attack's origin.
            /// </summary>
            /// <param name="origin">This attack's origin.</param>
            /// <returns>This object for method chaining.</returns>
            Builder2 From(Mob origin);
        }

        public interface Builder2
        {

            /// <summary>
            /// Makes a melee attack against the target.
            /// </summary>
            /// <param name="target">This attack's target.</param>
            /// <returns>This object for method chaining.</returns>
            Builder3 MeleeTo(Cell target);

            /// <summary>
            /// Makes a ranged attack against the target.
            /// </summary>
            /// <param name="target">This attack's target.</param>
            /// <param name="appearance">This attack's appearance.</param>
            /// <returns>This object for method chaining.</returns>
            Builder3 RangedTo(Cell target, Appearance appearance);

            /// <summary>
            /// Makes a ranged attack against the target along a specified path.
            /// </summary>
            /// <param name="target">This attack's target.</param>
            /// <param name="appearance">This attack's appearance.</param>
            /// <param name="path">This attack's path.</param>
            /// <returns>This object for method chaining.</returns>
            Builder3 RangedTo(Cell target, Appearance appearance, IList<Point> path);

            /// <summary>
            /// Makes a direct attack against the target.
            /// </summary>
            /// <param name="target">This attack's target.</param>
            /// <returns>This object for method chaining.</returns>
            Builder5 DirectTo(Cell target);
        }

        public interface Builder3 : Builder4
        {
            /// <summary>
            /// Modify the accuracy of this attack.
            /// </summary>
            /// <param name="delta">The ammount to change the accuracy by, in percentage points.</param>
            /// <returns>This object for method chaining.</returns>
            Builder3 AdjustAccuracy(int delta);

            /// <summary>
            /// Sets the effects that occur when this attack misses the target.
            /// </summary>
            /// <param name="effects">The effects.</param>
            /// <returns>This object for method chaining.</returns>
            Builder4 IfMisses(params IEffect[] effects);
        }

        public interface Builder4
        {
            /// <summary>
            /// Sets the effects that occur when this attack hits the target.
            /// </summary>
            /// <param name="effects">The effects.</param>
            /// <returns>The finalised attack.</returns>
            Attack IfHits(params IEffect[] effects);
        }

        public interface Builder5
        {
            /// <summary>
            /// Sets the effects this attack has on the target.
            /// </summary>
            /// <param name="effects">The effects.</param>
            /// <returns>The finalised attack.</returns>
            Attack WithEffect(params IEffect[] effects);
        }
    }
}
