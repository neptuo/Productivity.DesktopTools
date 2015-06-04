using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.UI.Stickers
{
    public class StickContext
    {
        public bool IsModified { get; private set; }
        public int OriginalPosition { get; private set; }
        public int NewPosition { get; private set; }

        public StickContext(int originalPosition)
        {
            NewPosition = OriginalPosition = originalPosition;
        }

        public bool TryStickTo(int newPosition)
        {
            return TryStickTo(newPosition, 0);
        }

        public bool TryStickTo(int newPosition, int originalOffset)
        {
            bool isModified = false;
            int originalPosition = OriginalPosition + originalOffset;

            if (newPosition - StickService.StickOffset < originalPosition && originalPosition < newPosition + StickService.StickOffset)
            {
                if (!IsModified || Math.Abs(NewPosition - OriginalPosition) > Math.Abs(newPosition - originalPosition))
                {
                    NewPosition = Math.Max(newPosition - originalOffset, 0);
                    isModified = true;
                }
            }

            if (isModified)
                IsModified = true;

            return isModified;
        }
    }
}
