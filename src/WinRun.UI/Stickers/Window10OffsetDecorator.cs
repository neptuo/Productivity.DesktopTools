using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.Stickers
{
    public class Window10OffsetDecorator : IStickPointDecorator
    {
        public const int verticalOffset = 4;
        public const int horizontalOffset = 4;

        public StickPoint DecorateTop(StickPoint point)
        {
            return StickPoint.Create(point.Handle, point.Value - verticalOffset, point.Priority);
        }

        public StickPoint DecorateBottom(StickPoint point)
        {
            return StickPoint.Create(point.Handle, point.Value + verticalOffset, point.Priority);
        }

        public StickPoint DecorateLeft(StickPoint point)
        {
            int value = point.Handle == null 
                ? point.Value - horizontalOffset 
                : point.Value - 2 * horizontalOffset;

            return StickPoint.Create(point.Handle, value, point.Priority);
        }

        public StickPoint DecorateRight(StickPoint point)
        {
            int value = point.Handle == null
                ? point.Value - horizontalOffset
                : point.Value + horizontalOffset;

            return StickPoint.Create(point.Handle, value, point.Priority);
        }
    }
}
