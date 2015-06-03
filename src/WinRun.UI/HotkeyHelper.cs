using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.ComponentModel;

namespace WinRun.UI
{
    public delegate void OnHotkeyPress();

    public class HotkeyHelper
    {
        public Window Window { get; protected set; }
        public HwndSource HWndSource { get; protected set; }
        public WindowInteropHelper WindowInteropHelper { get; protected set; }

        private List<Mapping> Mappings { get; set; }

        public HotkeyHelper(Window window)
        {
            Mappings = new List<Mapping>();

            Window = window;
            WindowInteropHelper = new WindowInteropHelper(Window);
            HWndSource = HwndSource.FromHwnd(WindowInteropHelper.Handle);

            Window.Closed += new EventHandler(Window_Closed);
            HWndSource.AddHook(MainWindowProc);
        }

        public void Register(Key key, ModifierKeys modifiers, OnHotkeyPress deleg, string identifier = null)
        {
            Mapping mapp = new Mapping(key, modifiers, deleg, identifier);
            Mappings.Add(mapp);

            if (!Win32.RegisterHotKey(WindowInteropHelper.Handle, mapp.Atom, (uint)modifiers, (uint)KeyInterop.VirtualKeyFromKey(key)))
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public void UnRegister(Key key, ModifierKeys modifiers)
        {
            Mapping mapp = null;

            foreach (Mapping mapping in Mappings)
            {
                if (mapping.Modifiers == modifiers && mapping.Key == key)
                    mapp = mapping;
            }

            if (mapp != null)
            {
                Mappings.Remove(mapp);
                Win32.UnregisterHotKey(WindowInteropHelper.Handle, mapp.Atom);
                Win32.GlobalDeleteAtom(mapp.Atom);
            }
        }

        public void UnRegister(string identifier)
        {
            Mapping mapp = null;

            foreach (Mapping mapping in Mappings)
            {
                if (mapping.Identifier == identifier)
                    mapp = mapping;
            }

            if (mapp != null)
            {
                Mappings.Remove(mapp);
                Win32.UnregisterHotKey(WindowInteropHelper.Handle, mapp.Atom);
                Win32.GlobalDeleteAtom(mapp.Atom);
            }
        }

        public void UnRegisterAll()
        {
            foreach (Mapping mapp in Mappings)
            {
                Win32.UnregisterHotKey(WindowInteropHelper.Handle, mapp.Atom);
                Win32.GlobalDeleteAtom(mapp.Atom);
            }

            Mappings.Clear();
        }

        public void Window_Closed(object sender, EventArgs e)
        {
            UnRegisterAll();
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
            foreach (Mapping mapping in Mappings)
            {
                if (mapping.Modifiers == modifiers && mapping.Key == key)
                {
                    mapping.Delegate();
                    return true;
                }
            }
            return false;
        }
    }

    internal sealed class Mapping
    {
        public Key Key { get; private set; }

        public ModifierKeys Modifiers { get; private set; }

        public OnHotkeyPress Delegate { get; private set; }

        public string Identifier { get; private set; }

        public short Atom { get; private set; }

        public Mapping(Key key, ModifierKeys modifiers, OnHotkeyPress deleg, string identifier = null)
        {
            Key = key;
            Modifiers = modifiers;
            Delegate = deleg;
            Identifier = identifier;
            Atom = Win32.GlobalAddAtom(identifier ?? deleg.ToString());
        }
    }

    internal static class Win32
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("kernel32", SetLastError = true)]
        public static extern short GlobalAddAtom(string lpString);

        [DllImport("kernel32", SetLastError = true)]
        public static extern short GlobalDeleteAtom(short nAtom);

        public const int MOD_ALT = 1;
        public const int MOD_CONTROL = 2;
        public const int MOD_SHIFT = 4;
        public const int MOD_WIN = 8;

        public const uint VK_KEY_C = 0x43;

        public const int WM_HOTKEY = 0x312;
    }
}
