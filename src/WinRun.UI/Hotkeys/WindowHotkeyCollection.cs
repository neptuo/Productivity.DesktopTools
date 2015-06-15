using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace WinRun.UI.Hotkeys
{
    public class WindowHotkeyCollection : IHotkeyCollection, IDisposable
    {
        private readonly Dictionary<Hotkey, HandlerList> storage = new Dictionary<Hotkey, HandlerList>();

        public Window Window { get; protected set; }
        public HwndSource HWndSource { get; protected set; }
        public WindowInteropHelper WindowInteropHelper { get; protected set; }

        public WindowHotkeyCollection(Window window)
        {
            Window = window;
            WindowInteropHelper = new WindowInteropHelper(Window);
            HWndSource = HwndSource.FromHwnd(WindowInteropHelper.Handle);

            HWndSource.AddHook(MainWindowProc);
        }

        public IntPtr MainWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32.WM_HOTKEY:
                    handled = OnHotKey((ModifierKeys)((int)lParam & 0xFFFF), KeyInterop.KeyFromVirtualKey(((int)lParam >> 16) & 0xFFFF));
                    break;
            }
            return IntPtr.Zero;
        }

        private bool OnHotKey(ModifierKeys modifiers, Key key)
        {
            Hotkey hotkey = new Hotkey(modifiers, key);
            HandlerList handlers;
            if (storage.TryGetValue(hotkey, out handlers))
            {
                foreach (Action<Hotkey> handler in handlers)
                    handler(hotkey);

                return true;
            }

            return false;
        }


        public IHotkeyCollection Add(Hotkey hotkey, Action<Hotkey> handler)
        {
            HandlerList handlers;
            if (!storage.TryGetValue(hotkey, out handlers))
            {
                handlers = new HandlerList((short)hotkey.GetHashCode());
                if (Win32.RegisterHotKey(WindowInteropHelper.Handle, handlers.Atom, (uint)hotkey.Modifier, (uint)KeyInterop.VirtualKeyFromKey(hotkey.Key)))
                    storage[hotkey] = handlers;
                else
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            handlers.Handlers.Add(handler);
            return this;
        }

        public IHotkeyCollection Remove(Hotkey hotkey)
        {
            HandlerList handlers;
            if (storage.TryGetValue(hotkey, out handlers))
            {
                if(storage.Remove(hotkey))
                    Win32.UnregisterHotKey(WindowInteropHelper.Handle, handlers.Atom);
            }

            return this;
        }

        public IHotkeyCollection Remove(Action<Hotkey> handler)
        {
            foreach (KeyValuePair<Hotkey, HandlerList> handlers in storage)
            {
                if (handlers.Value.Handlers.Contains(handler))
                {
                    handlers.Value.Handlers.Remove(handler);
                    if (handlers.Value.Handlers.Count == 0)
                        return Remove(handlers.Key);
                }
            }

            return this;
        }

        public void Dispose()
        {
            foreach (HandlerList handlers in storage.Values)
            {
                Win32.UnregisterHotKey(WindowInteropHelper.Handle, handlers.Atom);
                Win32.GlobalDeleteAtom(handlers.Atom);
            }

            storage.Clear();
        }

        private class HandlerList : IEnumerable<Action<Hotkey>>
        {
            public short Atom { get; private set; }
            public List<Action<Hotkey>> Handlers { get; private set; }

            public HandlerList(short atom)
            {
                Atom = atom;
                Handlers = new List<Action<Hotkey>>();
            }

            public IEnumerator<Action<Hotkey>> GetEnumerator()
            {
                return Handlers.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
