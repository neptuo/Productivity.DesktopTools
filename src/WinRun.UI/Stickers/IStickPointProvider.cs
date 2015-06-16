using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.Stickers
{
    public interface IStickPointProvider
    {
        IEnumerable<StickPoint> ForTop();
        IEnumerable<StickPoint> ForBottom();
        IEnumerable<StickPoint> ForLeft();
        IEnumerable<StickPoint> ForRight();
    }
}
