using System;
using System.Windows.Forms;

namespace Generator.HotKeys
{
    public class HotKey
    {
        public HotKey(int id, KeyModifier modifier, Keys key, Action handlerAction)
        {
            Id = id;
            Modifier = modifier;
            Key = key;
            HandlerAction = handlerAction;
        }

        public KeyModifier Modifier { get; private set; }

        public Keys Key { get; private set; }
        
        public Action HandlerAction { get; private set; }

        public int Id { get; private set; }
    }
}
