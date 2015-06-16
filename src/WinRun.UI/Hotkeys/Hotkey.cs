using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WinRun.Hotkeys
{
    public class Hotkey : IEquatable<Hotkey>
    {
        public ModifierKeys Modifier { get; private set; }
        public Key Key { get; private set; }

        public Hotkey(ModifierKeys modifier, Key key)
        {
            Key = key;
            Modifier = modifier;
        }

        public bool Equals(Hotkey other)
        {
            if (ReferenceEquals(null, other)) 
                return false;

            if (ReferenceEquals(this, other)) 
                return true;

            return Modifier.Equals(other.Modifier) && Key.Equals(other.Key);
        }

        public override bool Equals(object obj)
        {
            Hotkey other = obj as Hotkey;
            if (other == null)
                return false;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Modifier.GetHashCode() * 397) ^ Key.GetHashCode();
            }
        }

        public override string ToString()
        {
            return String.Format("({0} + {1})", Modifier, Key);
        }
    }
}
