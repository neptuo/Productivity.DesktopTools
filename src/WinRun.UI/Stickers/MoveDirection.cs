using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.UI.Stickers
{
    [Flags]
    public enum MoveDirection
    {
        Empty,
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop
    }
}
