using Neptuo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WinRun.ShellHooks
{
    public class NewWindowListener : System.IDisposable
    {
        private readonly IntPtr handle;
        private readonly IntPtr hook;
        private readonly Win32.CBTProc callback;

        public NewWindowListener(IntPtr handle)
        {
            Ensure.NotNull(handle, "handle");
            this.handle = handle;

            callback = CallBack;

            hook = Win32.SetWindowsHookExA(
                Win32.HookType.WH_SHELL,
                callback,
                instancePtr: IntPtr.Zero,
                threadID: Win32.GetCurrentThreadId()
            );
        }

        private IntPtr CallBack(int code, IntPtr wParam, IntPtr lParam)
        {
            Win32.CBTHookCodes codes = (Win32.CBTHookCodes)code;

            if (codes == Win32.CBTHookCodes.HCBT_CREATEWND)
            {
                MessageBox.Show("Window created");
            }
            //else if (codes == Win32.CBTHookCodes.HCBT_DESTROYWND)
            //{
            //    MessageBox.Show("Window destroyed");
            //}

            return Win32.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
