using System;

namespace LVGLSharp.Forms
{
    public class ApplicationConfiguration
    {
        public static void Initialize()
        {
          Application.EnableVisualStyles();
           Application.SetCompatibleTextRenderingDefault(false);
           Application.SetHighDpiMode(HighDpiMode.SystemAware);
        }
    }
}