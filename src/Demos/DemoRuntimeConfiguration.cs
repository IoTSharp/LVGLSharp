using System;
using LVGLSharp.Forms;

internal static class DemoRuntimeConfiguration
{
    internal static void Configure()
    {
        if (OperatingSystem.IsWindows())
        {
            Application.UseWindowsRuntime();
            return;
        }

        if (OperatingSystem.IsLinux())
        {
            Application.UseLinuxRuntime();
            return;
        }

        throw new PlatformNotSupportedException();
    }
}
