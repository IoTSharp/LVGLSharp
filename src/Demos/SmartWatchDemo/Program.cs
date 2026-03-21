using System;

namespace SmartWatchDemo;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        Application.Run(new frmSmartWatchDemo());
    }
}
