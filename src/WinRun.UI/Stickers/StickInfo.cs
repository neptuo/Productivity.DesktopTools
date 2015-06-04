using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.UI.Stickers
{
    public class StickInfo
    {
        public IntPtr? Handle;
        public int Value;

        public StickInfo(int value)
        {
            Value = value;
        }

        public StickInfo(IntPtr handle, int value)
            : this(value)
        {
            Handle = handle;
        }
    }
}
