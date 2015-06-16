using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using WinRun.Hotkeys;

namespace WinRun.UI
{
    public class SystemSuspendHandler
    {
        public Hotkey SleepHotkey { get; private set; }
        public Hotkey HibernateHotkey { get; private set; }

        public SystemSuspendHandler()
        {
            SleepHotkey = new Hotkey(ModifierKeys.Windows, Key.F4);
            HibernateHotkey = new Hotkey(ModifierKeys.Windows, Key.F12);
        }

        public virtual void Handle(Hotkey hotkey)
        {
            Application.SetSuspendState(MapHotkeyToPowerState(hotkey), true, true);
        }

        protected PowerState MapHotkeyToPowerState(Hotkey hotkey)
        {
            if (hotkey == SleepHotkey)
                return PowerState.Suspend;
            else if (hotkey == HibernateHotkey)
                return PowerState.Hibernate;
            else
                throw new NotSupportedException(hotkey.ToString());
        }
    }
}
