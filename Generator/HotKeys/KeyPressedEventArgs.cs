using System;
using System.Windows.Forms;

namespace Generator.HotKeys
{
    public class KeyPressedEventArgs : EventArgs
    {
        private KeyModifier _modifier;
        private Keys _key;

        internal KeyPressedEventArgs(KeyModifier modifier, Keys key)
        {
            _modifier = modifier;
            _key = key;
        }

        public KeyModifier Modifier
        {
            get { return _modifier; }
        }

        public Keys Key
        {
            get { return _key; }
        }
    }
}
