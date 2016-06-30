using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.Stickers
{
    public class StickPoint
    {
        public IntPtr? Handle;
        public int Value;
        public int Priority { get; private set; }

        public StickPoint(int value, int priority)
        {
            Value = value;
            Priority = priority;
        }

        public StickPoint(IntPtr handle, int value, int priority)
            : this(value, priority)
        {
            Handle = handle;
        }

        public static StickPoint Create(IntPtr? handle, int value, int priority)
        {
            if (handle == null)
                return new StickPoint(value, priority);

            return new StickPoint(handle.Value, value, priority);
        }
    }
}
