using System;
using System.Diagnostics.CodeAnalysis;

namespace PsiFi.Views.ColorConsole
{
    struct KeyInfo : IEquatable<KeyInfo>, IEquatable<ConsoleKeyInfo>
    {
        public readonly ConsoleKey Key;
        public readonly ConsoleModifiers Modifiers;

        public KeyInfo(ConsoleKey key)
        {
            Modifiers = 0;
            Key = key;
        }

        public KeyInfo(ConsoleModifiers modifiers, ConsoleKey key)
        {
            Modifiers = modifiers;
            Key = key;
        }

        public override bool Equals(object obj)
        {
            if (obj is KeyInfo keyInfo)
                return Equals(keyInfo);
            if (obj is ConsoleKeyInfo consoleKeyInfo)
                return Equals(consoleKeyInfo);
            return false;
        }
        public bool Equals([AllowNull] ConsoleKeyInfo other) => this.Key == other.Key && this.Modifiers == other.Modifiers;
        public bool Equals([AllowNull] KeyInfo other) => this.Key == other.Key && this.Modifiers == other.Modifiers;

        public override int GetHashCode() => (int)Key + ((int)Modifiers << 8);

        public static implicit operator KeyInfo(ConsoleKey key) => new KeyInfo(key);
        public static implicit operator KeyInfo(ConsoleKeyInfo info) => new KeyInfo(info.Modifiers, info.Key);

        public static bool operator ==(KeyInfo a, KeyInfo b) => a.Equals(b);

        public static bool operator !=(KeyInfo a, KeyInfo b) => !a.Equals(b);
    }
}
