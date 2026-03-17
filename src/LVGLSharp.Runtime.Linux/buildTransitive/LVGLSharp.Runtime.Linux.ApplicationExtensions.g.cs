namespace LVGLSharp.Forms;

public static class LinuxRuntimeApplicationExtensions
{
    [global::System.Runtime.CompilerServices.ModuleInitializer]
    internal static void Initialize()
    {
        if (global::System.OperatingSystem.IsLinux())
        {
            global::LVGLSharp.Forms.Application.UseRuntime(static (_, _, _) => new global::LVGLSharp.Runtime.Linux.LinuxView());
        }
    }

    extension(global::LVGLSharp.Forms.Application)
    {
        /// <summary>
        /// Registers the Linux runtime for <see cref="global::LVGLSharp.Forms.Application.Run(global::LVGLSharp.Forms.Form)"/>.
        /// </summary>
        public static void UseLinuxRuntime()
        {
            global::LVGLSharp.Forms.Application.UseRuntime(static (_, _, _) => new global::LVGLSharp.Runtime.Linux.LinuxView());
        }
    }
}
