using System;

namespace Generator.HotKeys
{
    [Flags]
    public enum KeyModifier
    {
        NotSet = 0,

        Alt = 0x0001,

        Control = 0x0002,

        NoRepeat = 0x4000,

        Shift = 0x0004,

        Win = 0x0008,
    }
}
