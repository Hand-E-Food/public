using System;

namespace PsiFi.Models.Mobs
{
    class Player : Mob
    {
        /// <inheritdoc/>
        public override IBrain? Brain => null;

        /// <inheritdoc/>
        public override int Health
        {
            get => base.Health;
            set
            {
                if (value > MaximumHealth) value = MaximumHealth;
                if (base.Health == value) return;
                base.Health = value;
                OnHealthChanged();
            }
        }
        protected void OnHealthChanged() => HealthChanged?.Invoke();
        /// <summary>
        /// Raised after this player's <see cref="Health"/> or <see cref="MaximumHealth"/> has changed.
        /// </summary>
        public event SimpleEventHandler? HealthChanged;

        /// <summary>
        /// This occupant's maximum health.
        /// </summary>
        public int MaximumHealth 
        {
            get => maximumHealth;
            set
            {
                if (maximumHealth == value) return;
                maximumHealth = value;
                if (Health > maximumHealth)
                    Health = maximumHealth;
                else
                    OnHealthChanged();
            }
        }
        private int maximumHealth;

        /// <summary>
        /// Initialises a new instance of the <see cref="Player"/> class.
        /// </summary>
        public Player() : base(new Appearance('@', ConsoleColor.White))
        { }
    }
}
