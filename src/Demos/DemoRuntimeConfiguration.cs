using System;
using LVGLSharp.Drawing;
using LVGLSharp.Forms;
using LVGLSharp.Runtime.Linux;
using LVGLSharp.Runtime.Windows;

internal static class DemoRuntimeConfiguration
{
    internal static void Configure()
    {
        if (OperatingSystem.IsWindows())
        {
            Application.UseRuntime(static (title, width, height) => new Win32Window(title, (uint)width, (uint)height), static () => Win32Window.CurrentMouseButton);
            Image.RegisterFactory(static path => new WindowsImageSource(path));
            return;
        }

        if (OperatingSystem.IsLinux())
        {
            Application.UseRuntime(static (title, width, height) => new LinuxView(title, width, height));
            Image.RegisterFactory(static path => new LinuxImageSource(path));
            return;
        }

        throw new PlatformNotSupportedException();
    }
}
