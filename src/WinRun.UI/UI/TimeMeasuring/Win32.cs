using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.UI.TimeMeasuring
{
    public static class Win32
    {
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [Flags]
        public enum DWMWINDOWATTRIBUTE
        {
            DWMWA_NCRENDERING_ENABLED = 1,
            DWMWA_NCRENDERING_POLICY,
            DWMWA_TRANSITIONS_FORCEDISABLED,
            DWMWA_ALLOW_NCPAINT,
            DWMWA_CAPTION_BUTTON_BOUNDS,
            DWMWA_NONCLIENT_RTL_LAYOUT,
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            DWMWA_FLIP3D_POLICY,
            DWMWA_EXTENDED_FRAME_BOUNDS,
            DWMWA_HAS_ICONIC_BITMAP,
            DWMWA_DISALLOW_PEEK,
            DWMWA_EXCLUDED_FROM_PEEK,
            DWMWA_LAST
        }

        [Flags]
        public enum DWMNCRenderingPolicy
        {
            UseWindowStyle,
            Disabled,
            Enabled,
            Last
        }

        public static void RemoveFromAeroPeek(IntPtr hwnd) //Hwnd is the handle to your window
        {
            int renderPolicy = (int)DWMNCRenderingPolicy.Enabled;
            DwmSetWindowAttribute(hwnd, (int)DWMWINDOWATTRIBUTE.DWMWA_EXCLUDED_FROM_PEEK, ref renderPolicy, sizeof(int));
        }




        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("a5cd92ff-29be-454c-8d04-d82879fb3f1b")]
        [SuppressUnmanagedCodeSecurity]
        public interface IVirtualDesktopManager
        {
            [PreserveSig]
            int IsWindowOnCurrentVirtualDesktop(
                [In] IntPtr TopLevelWindow,
                [Out] out int OnCurrentDesktop
                );
            [PreserveSig]
            int GetWindowDesktopId(
                [In] IntPtr TopLevelWindow,
                [Out] out Guid CurrentDesktop
                );

            [PreserveSig]
            int MoveWindowToDesktop(
                [In] IntPtr TopLevelWindow,
                [MarshalAs(UnmanagedType.LPStruct)]
            [In]Guid CurrentDesktop
                );
        }

        [ComImport, Guid("aa509086-5ca9-4c25-8f95-589d3c07b48a")]
        public class CVirtualDesktopManager
        {

        }
    }

    public class VirtualDesktopManager
    {
        public VirtualDesktopManager()
        {
            cmanager = new Win32.CVirtualDesktopManager();
            manager = (Win32.IVirtualDesktopManager)cmanager;
        }

        ~VirtualDesktopManager()
        {
            manager = null;
            cmanager = null;
        }

        private Win32.CVirtualDesktopManager cmanager = null;
        private Win32.IVirtualDesktopManager manager;

        public bool IsWindowOnCurrentVirtualDesktop(IntPtr TopLevelWindow)
        {
            int result;
            int hr;
            if ((hr = manager.IsWindowOnCurrentVirtualDesktop(TopLevelWindow, out result)) != 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
            return result != 0;
        }

        public Guid GetWindowDesktopId(IntPtr TopLevelWindow)
        {
            Guid result;
            int hr;
            if ((hr = manager.GetWindowDesktopId(TopLevelWindow, out result)) != 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
            return result;
        }

        public void MoveWindowToDesktop(IntPtr TopLevelWindow, Guid CurrentDesktop)
        {
            int hr;
            if ((hr = manager.MoveWindowToDesktop(TopLevelWindow, CurrentDesktop)) != 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
        }
    }
}
