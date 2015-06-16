using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.Stickers
{
    public class StickContext
    {
        public bool IsModified { get; private set; }
        public int OriginalPosition { get; private set; }
        public int NewPosition { get; private set; }

        public bool IsResize { get; private set; }
        public int OriginalSize { get; private set; }
        public int NewSize { get; private set; }

        public StickContext(int originalPosition)
        {
            NewPosition = OriginalPosition = originalPosition;
        }

        public StickContext(int originalPosition, int originalSize)
            :this(originalPosition)
        {
            NewSize = OriginalSize = originalSize;
            IsResize = true;
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
                    int newTargetPosition = Math.Max(newPosition - originalOffset, 0);

                    if(!IsResize || originalOffset == 0)
                        NewPosition = newTargetPosition;

                    if (IsResize)
                    {
                        if (originalOffset == 0)
                            NewSize = OriginalSize + Math.Abs(newTargetPosition - OriginalPosition);
                        else
                            NewSize = OriginalSize + (newTargetPosition - OriginalPosition);
                    }
                    
                    isModified = true;
                }
            }

            if (isModified)
                IsModified = true;

            return isModified;
        }
    }
}
