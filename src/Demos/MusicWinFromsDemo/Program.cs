using System;

namespace MusicWinFromsDemo
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

#if !WINDOWS
            DemoRuntimeConfiguration.Configure();
#endif

            Application.Run(new frmMusicWinFromsDemo());
        }
    }
}
