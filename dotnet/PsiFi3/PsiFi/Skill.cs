using System.Diagnostics;

namespace PsiFi
{
    /// <summary>
    /// A protagonist's skill.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public abstract class Skill
    {
        internal string DebuggerDisplay => $"{Name} ({School})";

        /// <summary>
        /// Initialises a new <see cref="Skill"/>.
        /// </summary>
        /// <param name="protagonist">The protagonist that has this skill.</param>
        public Skill(Protagonist protagonist)
        {
            Protagonist = protagonist;
        }

        /// <summary>
        /// This skill's abilities.
        /// </summary>
        public Ability? Ability { get; protected set; } = null;

        /// <summary>
        /// This skill's name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The protagonist that has this skill.
        /// </summary>
        public Protagonist Protagonist { get; }

        /// <summary>
        /// This skill's school.
        /// </summary>
        public abstract School School { get; }
    }
}
