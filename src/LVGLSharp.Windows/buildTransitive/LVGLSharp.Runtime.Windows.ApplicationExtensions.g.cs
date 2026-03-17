namespace LVGLSharp.Forms;

public static class WindowsRuntimeApplicationExtensions
{
    [global::System.Runtime.CompilerServices.ModuleInitializer]
    internal static void Initialize()
    {
        if (global::System.OperatingSystem.IsWindows())
        {
            global::LVGLSharp.Forms.Application.UseRuntime(static (title, width, height) => new global::LVGLSharp.Runtime.Windows.Win32Window(title, (uint)width, (uint)height), static () => global::LVGLSharp.Runtime.Windows.Win32Window.CurrentMouseButton);
        }
    }

    extension(global::LVGLSharp.Forms.Application)
    {
        /// <summary>
        /// Registers the Windows runtime for <see cref="global::LVGLSharp.Forms.Application.Run(global::LVGLSharp.Forms.Form)"/>.
        /// </summary>
        public static void UseWindowsRuntime()
        {
            global::LVGLSharp.Forms.Application.UseRuntime(static (title, width, height) => new global::LVGLSharp.Runtime.Windows.Win32Window(title, (uint)width, (uint)height), static () => global::LVGLSharp.Runtime.Windows.Win32Window.CurrentMouseButton);
        }
    }
}
