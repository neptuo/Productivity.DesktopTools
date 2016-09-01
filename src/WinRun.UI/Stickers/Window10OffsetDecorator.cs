using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.Stickers
{
    // http://stackoverflow.com/questions/34139450/getwindowrect-returns-a-size-including-invisible-borders
    public class Window10OffsetDecorator : IStickPointDecorator
    {
        public const int DesktopLeftOffset = -3;

        public const int WindowWidthOverlap = -16;
        public const int WindowHeightOverlap = -3;

        public const int WindowOtherLeftOverlap = -7;

        public const int VerticalOffset = 4;
        public const int HorizontalOffset = 4;

        public StickPoint DecorateTop(StickPoint point)
        {
            return StickPoint.Create(point.Handle, point.Value - VerticalOffset, point.Priority);
        }

        public StickPoint DecorateBottom(StickPoint point)
        {
            return StickPoint.Create(point.Handle, point.Value + VerticalOffset, point.Priority);
        }

        public StickPoint DecorateLeft(StickPoint point)
        {
            int value = point.Handle == null 
                ? point.Value - HorizontalOffset 
                : point.Value - 2 * HorizontalOffset;

            return StickPoint.Create(point.Handle, value, point.Priority);
        }

        public StickPoint DecorateRight(StickPoint point)
        {
            int value = point.Handle == null
                ? point.Value - HorizontalOffset
                : point.Value + HorizontalOffset;

            return StickPoint.Create(point.Handle, value, point.Priority);
        }
    }
}
