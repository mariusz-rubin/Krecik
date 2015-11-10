using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Krecik.HotKeys
{
    public class HotKeyManager : NativeWindow, IDisposable
    {
        private const int WM_HOTKEY = 0x0312;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private int _lastHotKeyId = 0;

        private List<HotKey> _hotKeys;

        public HotKeyManager()
        {
            this.CreateHandle(new CreateParams());
            _hotKeys = new List<HotKey>();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if(m.Msg == WM_HOTKEY)
            {
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);

                var registeredHotKey = _hotKeys.SingleOrDefault(x => x.Modifier == modifier && x.Key == key);

                if (registeredHotKey != null)
                {
                    registeredHotKey.HandlerAction();
                }
            }
        }

        public bool RegisterHotKey(KeyModifier keyModifier, Keys key, Action handlerAction)
        {
            _lastHotKeyId++;

            if (RegisterHotKey(this.Handle, _lastHotKeyId, (uint)keyModifier, (uint)key))
            {
                _hotKeys.Add(new HotKey(_lastHotKeyId, keyModifier, key, handlerAction));

                return true;
            }

            return false;
        }

        public void Dispose()
        {
            foreach(HotKey hotKey in _hotKeys)
            {
                UnregisterHotKey(this.Handle, hotKey.Id);
            }

            this.DestroyHandle();
        }
    }
}
