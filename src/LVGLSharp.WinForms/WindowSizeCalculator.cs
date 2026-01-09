using System.Runtime.InteropServices;
namespace LVGLSharp.Forms
{
    public class WindowSizeCalculator
    {
        /// <summary>
        /// 根据客户区大小计算窗口大小
        /// </summary>
        /// <param name="clientWidth">客户区宽度</param>
        /// <param name="clientHeight">客户区高度</param>
        /// <param name="style">窗口样式</param>
        /// <param name="exStyle">窗口扩展样式</param>
        /// <param name="hasMenu">是否有菜单</param>
        /// <param name="dpi">DPI值</param>
        /// <returns>计算后的窗口矩形</returns>
        public static NativeMethods.RECT CalculateWindowRect(
            int clientWidth,
            int clientHeight,
            uint style = NativeMethods.WS_OVERLAPPEDWINDOW,
            uint exStyle = 0,
            bool hasMenu = false,
            uint dpi = 96)
        {
            var rect = new NativeMethods.RECT(0, 0, clientWidth, clientHeight);

            bool success = NativeMethods.AdjustWindowRectExForDpi(
                ref rect,
                style,
                hasMenu,
                exStyle,
                dpi);

            if (!success)
            {
                int error = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(error);
            }

            return rect;
        }

        /// <summary>
        /// 计算窗口大小（使用系统DPI）
        /// </summary>
        public static (int width, int height) GetWindowSize(
            int clientWidth,
            int clientHeight,
            uint style = NativeMethods.WS_OVERLAPPEDWINDOW,
            bool hasMenu = false)
        {
            // 获取系统DPI
            uint systemDpi = (uint)NativeMethods.GetDpiForSystem();

            var rect = CalculateWindowRect(
                clientWidth, clientHeight,
                style, 0, hasMenu, systemDpi);

            return (rect.Width, rect.Height);
        }
    }
}
