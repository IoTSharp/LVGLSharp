using LVGLSharp.Interop;
using LVGLSharp.Runtime.Windows;
using System;

namespace LVGLSharp.Forms
{
    unsafe public static class Application
    {
  
        unsafe static lv_obj_t* root;
        unsafe static lv_group_t* key_inputGroup = null;
        unsafe static delegate* unmanaged[Cdecl]<lv_event_t*, void> SendTextAreaFocusCb = null;
        public static void Run(Form form1)
        {

#if LINUX
        root = LinuxView.root;
        key_inputGroup = LinuxView.key_inputGroup;
        SendTextAreaFocusCb = LinuxView.SendTextAreaFocusCb;
#else
            root = Win32Window.root;
            key_inputGroup = Win32Window.key_inputGroup;
            SendTextAreaFocusCb = Win32Window.SendTextAreaFocusCb;
#endif

        }

        internal static void EnableVisualStyles()
        {
        }

        internal static void SetCompatibleTextRenderingDefault(bool v)
        {
        }

        public static void SetHighDpiMode(HighDpiMode systemAware)
        {
        }
    }
}