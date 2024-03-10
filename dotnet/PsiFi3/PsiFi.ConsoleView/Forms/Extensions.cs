using PsiFi.Abilities;
using PsiFi.Skills;

namespace PsiFi.ConsoleView.Forms
{
    internal static class Extensions
    {
        private static readonly Dictionary<Type, char> ButtonKeyByType = new() {
            { typeof(QuitAbility), 'Q' },
            { typeof(TargetAbility), 't' },
            { typeof(Sword), 'f' },
        };

        /// <summary>
        /// Gets the key to use on a button that activates this ability.
        /// </summary>
        /// <param name="ability">The ability.</param>
        /// <returns>The key to use on a button to activate this ability.</returns>
        public static char GetButtonKey(this Ability ability) =>
            ButtonKeyByType[ability.GetType()];

        /// <summary>
        /// Gets the key to use on a button that activates this skill.
        /// </summary>
        /// <param name="skill">The skill.</param>
        /// <returns>The key to use on a button to activate this skill.</returns>
        public static char GetButtonKey(this Skill skill) =>
            ButtonKeyByType[skill.GetType()];

        /// <summary>
        /// Gets this school's color.
        /// </summary>
        /// <param name="school">The school.</param>
        /// <returns>This school's color.</returns>
        public static ConsoleColor GetColor(this School school)
        {
            return school switch
            {
                School.Default => ConsoleColor.White,
                School.Athlete => ConsoleColor.Red,
                School.Psionic => ConsoleColor.Blue,
                School.Technology => ConsoleColor.Green,
                School.Weapon => ConsoleColor.Gray,
                School.Evolution => ConsoleColor.Magenta,
                School.Cyborg => ConsoleColor.Yellow,
                School.Assassin => ConsoleColor.Magenta,
                School.Psiborg => ConsoleColor.Cyan,
                School.Psiconaut => ConsoleColor.Cyan,
                School.Mechanaut => ConsoleColor.Yellow,
                _ => ConsoleColor.White,
            };
        }
    }
}
