using System.Collections.Generic;
using System.Text;
using System;
namespace LVGLSharp.Forms
{
    public class DpiHelper
    {
        // 设置DPI感知级别
        public static bool SetPerMonitorDpiAwareness()
        {
            try
            {
                // Windows 10 Creators Update (1703) 及以上版本
                if (Environment.OSVersion.Version >= new Version(10, 0, 15063))
                {
                    return NativeMethods.SetProcessDpiAwarenessContext(
                        NativeMethods.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2);
                }
                else if (Environment.OSVersion.Version >= new Version(10, 0, 14393))
                {
                    return NativeMethods.SetProcessDpiAwarenessContext(
                        NativeMethods.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE);
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // 获取窗口的DPI
        public static uint GetWindowDpi(IntPtr hwnd)
        {
            if (hwnd != IntPtr.Zero)
            {
                return NativeMethods.GetDpiForWindow(hwnd);
            }
            return (uint)NativeMethods.GetDpiForSystem();
        }
    }
}
