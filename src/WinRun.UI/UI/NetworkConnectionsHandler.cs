using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinRun.Hotkeys;

namespace WinRun.UI
{
    public class NetworkConnectionsHandler : IHotkeyHandler
    {
        public Hotkey Hotkey { get; private set; }

        public NetworkConnectionsHandler()
        {
            Hotkey = new Hotkey(ModifierKeys.Windows, Key.F5);
        }

        public void Handle(Hotkey hotkey)
        {
            Process.Start(@"C:\Windows\explorer.exe", "::{7007ACC7-3202-11D1-AAD2-00805FC1270E}");
        }
    }
}
