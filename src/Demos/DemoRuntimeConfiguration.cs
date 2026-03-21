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
            Application.UseRuntime(
                static options => new Win32Window(options.Title, (uint)options.Width, (uint)options.Height, options.Borderless),
                static () => Win32Window.CurrentMouseButton,
                static () => Win32Window.CurrentMousePosition);
            Image.RegisterFactory(static path => new WindowsImageSource(path));
            return;
        }

        if (OperatingSystem.IsLinux())
        {
            Application.UseRuntime(
                static options => new LinuxView(options.Title, options.Width, options.Height, borderless: options.Borderless),
                static () => LinuxView.CurrentMouseButton,
                static () => LinuxView.CurrentMousePosition);
            Image.RegisterFactory(static path => new LinuxImageSource(path));
            return;
        }

        throw new PlatformNotSupportedException();
    }


}
