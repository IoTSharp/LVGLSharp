using FormsApplication = LVGLSharp.Forms.Application;

namespace LVGLSharp.WPF;

public class Application
{
    private static bool _runtimeInitialized;

    public static void Run()
    {
        Run(new Window());
    }

    public static void Run(Func<Window> startupFactory)
    {
        ArgumentNullException.ThrowIfNull(startupFactory);
        Run(startupFactory());
    }

    public static void Run<TWindow>()
        where TWindow : Window, new()
    {
        Run(new TWindow());
    }

    public static void Run(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        EnsureRuntimeInitialized();
        FormsApplication.Run(window.CreateHostForm());
    }

    private static void EnsureRuntimeInitialized()
    {
        if (_runtimeInitialized)
        {
            return;
        }

        LVGLSharp.Forms.ApplicationConfiguration.Initialize();

        _runtimeInitialized = true;
    }
}
