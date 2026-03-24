

namespace WinFormsVncDemo
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
      