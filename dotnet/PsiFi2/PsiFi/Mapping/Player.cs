using PsiFi.Mapping.Actions;
using Rogue;
using System;

namespace PsiFi.Mapping
{
    /// <summary>
    /// The player character.
    /// </summary>
    public class Player : Mob
    {
        /// <inheritdoc/>
        public override char Character => '@';

        /// <inheritdoc/>
        public override Color ForeColor => Color.White;

        /// <inheritdoc/>
        public override int Health
        {
            get => health;
            set
            {
                value = Math.Min(value, MaximumHealth);
                if (health == value) return;
                health = value;
                HealthChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private int health;
        /// <summary>
        /// Raised after this player's <see cref="Health"/> has changed.
        /// </summary>
        public event EventHandler? HealthChanged;

        /// <inheritdoc/>
        public override int MaximumHealth => maximumHealth;
        /// <summary>
        /// Sets this player's maximum health.
        /// </summary>
        /// <param name="value">The new maximum health.</param>
        public void SetMaximumHealth(int value)
        {
            if (maximumHealth == value) return;
            maximumHealth = value;
            if (Health > maximumHealth) Health = maximumHealth;
            MaximumHealthChanged?.Invoke(this, EventArgs.Empty);
        }
        private int maximumHealth;
        /// <summary>
        /// Raised after this player's <see cref="MaximumHealth"/> has changed.
        /// </summary>
        public event EventHandler? MaximumHealthChanged;

        /// <inheritdoc/>
        public override string Name => name;
        /// <summary>
        /// Sets this player's name.
        /// </summary>
        /// <param name="value">The new name.</param>
        public void SetName(string value)
        {
            if (name == value) return;
            name = value;
            NameChanged?.Invoke(this, EventArgs.Empty);
        }
        private string name = "Hero Protagonist";
        /// <summary>
        /// Raised after this player's <see cref="Name"/> has changed.
        /// </summary>
        public event EventHandler? NameChanged;

        /// <summary>
        /// This player's <see cref="PlayerEngine"/> that manages the user interaction.
        /// </summary>
        public PlayerEngine PlayerEngine { get; set; } = null!;

        /// <inheritdoc/>
        public override IAction Act() => PlayerEngine.Act();

        public void UseTime(int milliseconds) => TimeUntilNextTurn += milliseconds;
    }
}
