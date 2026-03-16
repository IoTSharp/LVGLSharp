using System.Runtime.InteropServices;

namespace LVGLSharp.Forms
{
    /// <summary>
    /// 提供跨平台的剪贴板操作功能
    /// </summary>
    public static class Clipboard
    {
        private static string? _clipboardText;

        public static void SetText(string? text)
        {
            _clipboardText = text;
#if WINDOWS
            SetSystemClipboardText(text);
#elif LINUX
            SetLinuxClipboardText(text);
#endif
        }

        public static string? GetText()
        {
#if WINDOWS
            var systemText = GetSystemClipboardText();
            if (systemText != null)
            {
                _clipboardText = systemText;
            }
#elif LINUX
            var systemText = GetLinuxClipboardText();
            if (systemText != null)
            {
                _clipboardText = systemText;
            }
#endif
            return _clipboardText;
        }

        public static void Clear()
        {
            _clipboardText = null;
#if WINDOWS
            ClearSystemClipboard();
#elif LINUX
            ClearLinuxClipboard();
#endif
        }

        public static bool ContainsText()
        {
#if WINDOWS
            return ContainsSystemText() || !string.IsNullOrEmpty(_clipboardText);
#elif LINUX
            return ContainsLinuxText() || !string.IsNullOrEmpty(_clipboardText);
#else
            return !string.IsNullOrEmpty(_clipboardText);
#endif
        }

#if WINDOWS
        private const uint CF_UNICODETEXT = 13;
        private const uint GMEM_MOVEABLE = 0x0002;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool CloseClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool EmptyClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool IsClipboardFormatAvailable(uint format);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GlobalUnlock(IntPtr hMem);

        private static void SetSystemClipboardText(string? text)
        {
            if (string.IsNullOrEmpty(text)) return;

            try
            {
                if (!OpenClipboard(IntPtr.Zero)) return;
                EmptyClipboard();

                var bytes = System.Text.Encoding.Unicode.GetBytes(text + "\0");
                var hGlobal = GlobalAlloc(GMEM_MOVEABLE, (UIntPtr)bytes.Length);
                if (hGlobal == IntPtr.Zero)
                {
                    CloseClipboard();
                    return;
                }

                var target = GlobalLock(hGlobal);
                if (target != IntPtr.Zero)
                {
                    Marshal.Copy(bytes, 0, target, bytes.Length);
                    GlobalUnlock(hGlobal);
                    SetClipboardData(CF_UNICODETEXT, hGlobal);
                }

                CloseClipboard();
            }
            catch { }
        }

        private static string? GetSystemClipboardText()
        {
            try
            {
                if (!OpenClipboard(IntPtr.Zero)) return null;

                var handle = GetClipboardData(CF_UNICODETEXT);
                if (handle == IntPtr.Zero)
                {
                    CloseClipboard();
                    return null;
                }

                var pointer = GlobalLock(handle);
                if (pointer == IntPtr.Zero)
                {
                    CloseClipboard();
                    return null;
                }

                var text = Marshal.PtrToStringUni(pointer);
                GlobalUnlock(handle);
                CloseClipboard();

                return text;
            }
            catch
            {
                return null;
            }
        }

        private static void ClearSystemClipboard()
        {
            try
            {
                if (OpenClipboard(IntPtr.Zero))
                {
                    EmptyClipboard();
                    CloseClipboard();
                }
            }
            catch { }
        }

        private static bool ContainsSystemText()
        {
            return IsClipboardFormatAvailable(CF_UNICODETEXT);
        }
#endif

#if LINUX
        // Linux X11 剪贴板支持
        private const string LibX11 = "libX11.so.6";
        
        [DllImport(LibX11)]
        private static extern IntPtr XOpenDisplay(string? display);
        
        [DllImport(LibX11)]
        private static extern int XCloseDisplay(IntPtr display);
        
        [DllImport(LibX11)]
        private static extern IntPtr XInternAtom(IntPtr display, string atom_name, bool only_if_exists);
        
        [DllImport(LibX11)]
        private static extern int XSetSelectionOwner(IntPtr display, IntPtr selection, IntPtr owner, IntPtr time);
        
        [DllImport(LibX11)]
        private static extern IntPtr XGetSelectionOwner(IntPtr display, IntPtr selection);
        
        [DllImport(LibX11)]
        private static extern IntPtr XDefaultRootWindow(IntPtr display);

        private static IntPtr _display = IntPtr.Zero;
        private static IntPtr _clipboardAtom = IntPtr.Zero;
        private static IntPtr _utf8Atom = IntPtr.Zero;

        private static void EnsureX11Display()
        {
            if (_display == IntPtr.Zero)
            {
                try
                {
                    _display = XOpenDisplay(null);
                    if (_display != IntPtr.Zero)
                    {
                        _clipboardAtom = XInternAtom(_display, "CLIPBOARD", false);
                        _utf8Atom = XInternAtom(_display, "UTF8_STRING", false);
                    }
                }
                catch { }
            }
        }

        private static void SetLinuxClipboardText(string? text)
        {
            if (string.IsNullOrEmpty(text)) return;

            try
            {
                EnsureX11Display();
                if (_display == IntPtr.Zero) return;

                // 简化实现：通过 xclip 命令设置剪贴板
                try
                {
                    var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "xclip",
                        Arguments = "-selection clipboard",
                        RedirectStandardInput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    });

                    if (process != null)
                    {
                        process.StandardInput.Write(text);
                        process.StandardInput.Close();
                        process.WaitForExit();
                    }
                }
                catch { }
            }
            catch { }
        }

        private static string? GetLinuxClipboardText()
        {
            try
            {
                // 使用 xclip 命令获取剪贴板内容
                var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "xclip",
                    Arguments = "-selection clipboard -o",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    var text = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    return string.IsNullOrEmpty(text) ? null : text;
                }
            }
            catch { }

            return null;
        }

        private static void ClearLinuxClipboard()
        {
            try
            {
                // 设置空字符串到剪贴板
                var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "xclip",
                    Arguments = "-selection clipboard",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    process.StandardInput.Write("");
                    process.StandardInput.Close();
                    process.WaitForExit();
                }
            }
            catch { }
        }

        private static bool ContainsLinuxText()
        {
            var text = GetLinuxClipboardText();
            return !string.IsNullOrEmpty(text);
        }
#endif
    }
}
