using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WinRun.Hotkeys
{
    public class HotkeyService : IBackgroundService, IHotkeyCollection
    {
        private readonly Window window;
        private WindowHotkeyCollection collection;

        public HotkeyService(Window window)
        {
            this.window = window;
        }

        public void Install()
        {
            collection = new WindowHotkeyCollection(window);
        }

        public void UnInstall()
        {
            collection.Dispose();
        }

        public IHotkeyCollection Add(Hotkey hotkey, Action<Hotkey> handler)
        {
            collection.Add(hotkey, handler);
            return this;
        }

        public IHotkeyCollection Remove(Hotkey hotkey)
        {
            collection.Remove(hotkey);
            return this;
        }

        public IHotkeyCollection Remove(Action<Hotkey> handler)
        {
            collection.Remove(handler);
            return this;
        }
    }
}
