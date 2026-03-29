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

    public static void RunFromAppXaml(
        System.Reflection.Assembly assembly,
        string appXamlPath,
        string fallbackStartupXaml,
        Func<Window> fallbackFactory,
        Func<string, Window?>? startupResolver = null)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentException.ThrowIfNullOrWhiteSpace(appXamlPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(fallbackStartupXaml);
        ArgumentNullException.ThrowIfNull(fallbackFactory);

        EnsureRuntimeInitialized();

        var appDefinition = XamlRuntimeLoader.TryLoadApplicationDefinition(assembly, appXamlPath);

        string startupXaml = appDefinition?.StartupUri ?? fallbackStartupXaml;
        var window = startupResolver?.Invoke(startupXaml) ?? fallbackFactory();

        // If custom startup window ctor did not initialize content, load from StartupUri XAML.
        if (window.Content is null)
        {
            XamlRuntimeLoader.LoadIntoWindow(window, startupXaml);
        }

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
