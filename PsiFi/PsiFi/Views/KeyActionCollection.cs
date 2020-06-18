using System;
using System.Collections.Generic;
using System.Text;

namespace PsiFi.Views
{
    class KeyActionCollection : Dictionary<ConsoleKeyInfo, Action>
    {
        /// <summary>
        /// Reads keys until a valid key combination is pressed and performs its action.
        /// </summary>
        /// <param name="keyActions">The valid key combinations and their actions.</param>
        /// <returns>The retunr value of the action.</returns>
        public void ConsoleActOnKey()
        {
            Action action;
            while (!TryGetValue(Console.ReadKey(true), out action)) ;
            action();
        }

    }

    class KeyActionCollection<T> : Dictionary<ConsoleKeyInfo, Func<T>>
    {
        /// <summary>
        /// Reads keys until a valid key combination is pressed and performs its action.
        /// </summary>
        /// <param name="keyActions">The valid key combinations and their actions.</param>
        /// <returns>The retunr value of the action.</returns>
        public T ReadKey()
        {
            Func<T> action;
            while (!TryGetValue(Console.ReadKey(true), out action)) ;
            return action();
        }
    }
}
