using System;

namespace MusicWinFromsDemo
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            Application.Run(new frmMusicDemo());
        }
    }
}
