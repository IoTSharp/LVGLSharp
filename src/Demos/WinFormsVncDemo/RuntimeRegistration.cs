using System.Runtime.CompilerServices;
using LVGLSharp.Drawing;
using LVGLSharp.Forms;
using LVGLSharp.Runtime.Remote;

namespace WinFormsVncDemo;

internal static class RuntimeRegistration
{
    [ModuleInitializer]
    internal static void Register()
    {
        ApplicationConfiguration.RegisterWindowsRuntimeInitializer(RegisterRuntime);
        ApplicationConfiguration.RegisterLinuxRuntimeInitializer(RegisterRuntime);
        ApplicationConfiguration.RegisterMacOsRuntimeInitializer(RegisterRuntime);
    }

    private static void RegisterRuntime()
    {
        Application.UseRuntime(static _ => new VncView());
        Image.RegisterFactory(static _ => throw new NotSupportedException("WinFormsVncDemo 尚未配置图片加载运行时。"));
    }
}
