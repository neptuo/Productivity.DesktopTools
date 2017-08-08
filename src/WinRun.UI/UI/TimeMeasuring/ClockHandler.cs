using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinRun.Hotkeys;
using WinRun.Properties;

namespace WinRun.UI.TimeMeasuring
{
    public class ClockHandler
    {
        private ClockWindow clockWindow;

        public Hotkey LargeHotkey { get; protected set; }
        public Hotkey MediumHotkey { get; protected set; }
        protected ClockSize Size { get; set; }

        public ClockHandler()
        {
            Size = ClockSize.Large;
            LargeHotkey = new Hotkey(ModifierKeys.Windows, Key.F6);
            MediumHotkey = new Hotkey(ModifierKeys.Windows | ModifierKeys.Shift, Key.F6);

            if (Settings.Default.IsClockOpen)
                Handle(LargeHotkey);
        }

        public virtual void Handle(Hotkey hotkey)
        {
            if (clockWindow == null)
            {
                clockWindow = new ClockWindow();
                clockWindow.Closed += (sender, args) => { clockWindow = null; };
            }

            clockWindow.SetClockSize(MapHotkeyToSize(hotkey));
            clockWindow.Show();
            clockWindow.Activate();
        }

        protected virtual ClockSize MapHotkeyToSize(Hotkey hotkey)
        {
            if (hotkey == LargeHotkey)
                return ClockSize.Large;
            else if (hotkey == MediumHotkey)
                return ClockSize.Medium;
            else
                throw new NotSupportedException(hotkey.ToString());
        }
    }
}
