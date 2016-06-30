using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.Stickers
{
    public interface IStickPointDecorator
    {
        StickPoint DecorateTop(StickPoint point);
        StickPoint DecorateBottom(StickPoint point);
        StickPoint DecorateLeft(StickPoint point);
        StickPoint DecorateRight(StickPoint point);
    }
}
