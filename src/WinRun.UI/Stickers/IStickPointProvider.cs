using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.UI.Stickers
{
    public interface IStickPointProvider
    {
        IEnumerable<StickInfo> ForTop();
        IEnumerable<StickInfo> ForBottom();
        IEnumerable<StickInfo> ForLeft();
        IEnumerable<StickInfo> ForRight();
    }
}
