using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinRun.Stickers
{
    public class WindowStickHook : IBackgroundService
    {
        private readonly IntPtr handle;
        private readonly IStickPointProvider pointProvider;
        private readonly Win32.WinEventDelegate hookDelegate;
        private IntPtr hookPointer;
        private Position initialPosition;

        public WindowStickHook(IntPtr handle, IStickPointProvider pointProvider)
        {
            this.handle = handle;
            this.pointProvider = pointProvider;
            this.hookDelegate = new Win32.WinEventDelegate(WndProc2);
        }

        public void Install()
        {
            hookPointer = Win32.SetWinEventHook(
                (uint)Win32.EventContants.EVENT_SYSTEM_MOVESIZESTART,
                (uint)Win32.EventContants.EVENT_SYSTEM_MOVESIZEEND,
                IntPtr.Zero,
                hookDelegate,
                0,
                0,
                (uint)Win32.SetWinEventHookParameter.WINEVENT_OUTOFCONTEXT
            );

            //Log("SetWinEventHook on '{0}' returned '{1}'.", handle, hookPointer);
        }

        public void UnInstall()
        {
            Win32.UnhookWinEvent(hookPointer);
        }

        private void WndProc2(IntPtr hWinEventHook, uint eventType, IntPtr handle, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (this.handle != handle)
                return;

            if (eventType == (uint)Win32.EventContants.EVENT_SYSTEM_MOVESIZESTART)
            {
                //Log("Starting to move or resize window '{0}' ('{1}', '{2}', '{3}', '{4}').", handle, idObject, idChild, dwEventThread, dwmsEventTime);
                initialPosition = GetWindowPositionOrDefault(handle);
            }
            else if (eventType == (uint)Win32.EventContants.EVENT_SYSTEM_MOVESIZEEND)
            {
                //Log("Resized or moved window '{0}' (from '{5}') ('{1}', '{2}', '{3}', '{4}').", handle, idObject, idChild, dwEventThread, dwmsEventTime, handle);

                Position currentPosition = GetWindowPositionOrDefault(handle);
                if (currentPosition == null)
                    return;

                Win32.RECT offset = GetWindowFrameOffset(handle, currentPosition);

                ResizeDirection resize = ResizeDirection.Empty;
                MoveDirection move = MoveDirection.Empty;

                if (initialPosition != null)
                {
                    if (initialPosition.Left < currentPosition.Left)
                        move |= MoveDirection.LeftToRight;
                    else if (initialPosition.Left > currentPosition.Left)
                        move |= MoveDirection.RightToLeft;

                    if (initialPosition.Top < currentPosition.Top)
                        move |= MoveDirection.TopToBottom;
                    else if (initialPosition.Top > currentPosition.Top)
                        move |= MoveDirection.BottomToTop;

                    if (initialPosition.Width != currentPosition.Width)
                        resize |= ResizeDirection.Width;

                    if (initialPosition.Height != currentPosition.Height)
                        resize |= ResizeDirection.Height;
                }

                //Log("User state: {0}x{1} at {2}x{3}.", currentPosition.Left, currentPosition.Top, currentPosition.Width, currentPosition.Height);

                StickContext left;
                if (resize.HasFlag(ResizeDirection.Width))
                    left = new StickContext(currentPosition.Left, currentPosition.Width);
                else
                    left = new StickContext(currentPosition.Left);

                StickContext top;
                if (resize.HasFlag(ResizeDirection.Height))
                    top = new StickContext(currentPosition.Top, currentPosition.Height);
                else
                    top = new StickContext(currentPosition.Top);

                int leftPriority = Int32.MaxValue;
                int topPriority = Int32.MaxValue;

                if (move.HasFlag(MoveDirection.RightToLeft) || move.HasFlag(MoveDirection.LeftToRight))
                {
                    TryStickLeft(left, ref leftPriority);
                    TryStickRight(left, currentPosition, ref leftPriority);
                }

                if (move.HasFlag(MoveDirection.BottomToTop) || move.HasFlag(MoveDirection.TopToBottom))
                {
                    TryStickTop(top, ref topPriority);
                    TryStickBottom(top, currentPosition, ref topPriority);
                }

                if (move == MoveDirection.Empty && resize.HasFlag(ResizeDirection.Width))
                    TryStickRight(left, currentPosition, ref leftPriority);

                if (move == MoveDirection.Empty && resize.HasFlag(ResizeDirection.Height))
                    TryStickBottom(top, currentPosition, ref topPriority);

                Position targetPosition = new Position(
                    left.NewPosition - offset.Left,
                    top.NewPosition - offset.Top,
                    (left.IsResize ? left.NewSize : currentPosition.Width) - offset.Right + offset.Left,
                    (top.IsResize ? top.NewSize : currentPosition.Height) - offset.Bottom + offset.Top
                );

                //Log("Stick state: {0}x{1} at {2}x{3}.", targetPosition.Left, targetPosition.Top, targetPosition.Width, targetPosition.Height);
                
                Win32.SetWindowPos(
                    handle,
                    IntPtr.Zero,
                    targetPosition.Left,
                    targetPosition.Top,
                    targetPosition.Width,
                    targetPosition.Height,
                    0
                );
            }
        }

        private Win32.RECT GetWindowFrameOffset(IntPtr handle, Position currentPosition)
        {
            Win32.RECT realSize;
            if (Win32.GetWindowRect(handle, out realSize))
            {
                return new Win32.RECT()
                {
                    Left = currentPosition.Left - realSize.Left,
                    Top = currentPosition.Top - realSize.Top,
                    Right = currentPosition.Left + currentPosition.Width - realSize.Right,
                    Bottom = currentPosition.Top + currentPosition.Height - realSize.Bottom
                };
            }

            return new Win32.RECT();
        }

        private void TryStickLeft(StickContext left, ref int leftPriority)
        {
            foreach (StickPoint other in pointProvider.ForLeft())
            {
                if (other.Handle == handle)
                    continue;

                if (leftPriority < other.Priority)
                    break;

                if (left.TryStickTo(other.Value))
                    leftPriority = other.Priority;
            }
        }

        private void TryStickRight(StickContext left, Position currentPosition, ref int leftPriority)
        {
            foreach (StickPoint other in pointProvider.ForRight())
            {
                if (other.Handle == handle)
                    continue;

                if (leftPriority < other.Priority)
                    break;

                if (left.TryStickTo(other.Value, currentPosition.Width))
                    leftPriority = other.Priority;
            }
        }

        private void TryStickTop(StickContext top, ref int topPriority)
        {
            foreach (StickPoint other in pointProvider.ForTop())
            {
                if (other.Handle == handle)
                    continue;

                if (topPriority < other.Priority)
                    break;

                if (top.TryStickTo(other.Value))
                    topPriority = other.Priority;
            }
        }

        private void TryStickBottom(StickContext top, Position currentPosition, ref int topPriority)
        {
            foreach (StickPoint other in pointProvider.ForBottom())
            {
                if (other.Handle == handle)
                    continue;

                if (topPriority < other.Priority)
                    break;

                if (top.TryStickTo(other.Value, currentPosition.Height))
                    topPriority = other.Priority;
            }
        }

        private string GetWindowText(IntPtr? handle)
        {
            if (handle == null)
                return "NO HANDLE";

            StringBuilder content = new StringBuilder(1024);
            Win32.GetWindowText(handle.Value, content, content.Capacity);
            return content.ToString();
        }

        private Position GetWindowPositionOrDefault(IntPtr handle)
        {
            Win32.RECT info;
            Win32.DwmGetWindowAttribute(handle, (int)Win32.DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out info, Marshal.SizeOf(typeof(Win32.RECT)));

            return new Position(
                info.Left,
                info.Top,
                info.Right - info.Left + Window10OffsetDecorator.WindowWidthOverlap,
                info.Bottom - info.Top + Window10OffsetDecorator.WindowHeightOverlap
            );
        }

        private void Log(string messageFormat, params object[] parameters)
        {
            Console.WriteLine(messageFormat, parameters);
        }
    }
}
