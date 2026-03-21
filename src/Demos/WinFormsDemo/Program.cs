using System;
 
namespace WinFormsDemo
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

#if LVGLSHARP_FORMS
            DemoRuntimeConfiguration.Configure();
#endif

            Application.Run(new frmMain());
        }
    }
}