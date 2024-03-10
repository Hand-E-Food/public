using System;

namespace PsiFi.View
{
    class KeyboardInput
    {
        public ConsoleKeyInfo GetKeyPress() => Console.ReadKey(intercept: true);
    }
}
