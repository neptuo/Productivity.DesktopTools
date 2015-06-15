using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.UI.Hotkeys
{
    public interface IHotkeyCollection
    {
        IHotkeyCollection Add(Hotkey hotkey, Action<Hotkey> handler);

        IHotkeyCollection Remove(Hotkey hotkey);
        IHotkeyCollection Remove(Action<Hotkey> handler);
    }
}
