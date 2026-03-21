namespace LVGLSharp
{
    internal static class WindowHostFactory
    {
        private static Func<WindowCreateOptions, IView>? s_factory;

        internal static bool IsRegistered => s_factory is not null;

        internal static void Register(Func<string, int, int, IView> factory)
        {
            ArgumentNullException.ThrowIfNull(factory);

            Register(options => factory(options.Title, options.Width, options.Height));
        }

        internal static void Register(Func<WindowCreateOptions, IView> factory)
        {
            ArgumentNullException.ThrowIfNull(factory);

            s_factory = factory;
        }

        internal static IView Create(string title, int width, int height)
        {
            return Create(new WindowCreateOptions(title, width, height));
        }

        internal static IView Create(WindowCreateOptions options)
        {
            if (s_factory is null)
            {
                PlatformRuntimeRegistration.EnsureCurrentPlatformRegistered();
            }

            return s_factory?.Invoke(options)
                ?? throw new InvalidOperationException("No LVGLSharp runtime has been configured. Reference a matching runtime package so it is registered during `ApplicationConfiguration.Initialize()`, or call `Application.UseRuntime(...)` before `Application.Run(...)`.");
        }
    }

    internal static class RuntimeInputState
    {
        private static Func<uint>? s_currentMouseButtonProvider;
        private static Func<(int X, int Y)>? s_currentMousePositionProvider;
        private static bool s_registered;

        internal static bool IsRegistered => s_registered;

        internal static void RegisterCurrentMouseButtonProvider(Func<uint>? currentMouseButtonProvider)
        {
            s_currentMouseButtonProvider = currentMouseButtonProvider;
            s_registered = true;
        }

        internal static void RegisterCurrentMousePositionProvider(Func<(int X, int Y)>? currentMousePositionProvider)
        {
            s_currentMousePositionProvider = currentMousePositionProvider;
            s_registered = true;
        }

        internal static uint GetCurrentMouseButton()
        {
            if (!s_registered)
            {
                PlatformRuntimeRegistration.EnsureCurrentPlatformRegistered();
            }

            return s_currentMouseButtonProvider?.Invoke() ?? 0;
        }

        internal static (int X, int Y) GetCurrentMousePosition()
        {
            if (!s_registered)
            {
                PlatformRuntimeRegistration.EnsureCurrentPlatformRegistered();
            }

            return s_currentMousePositionProvider?.Invoke() ?? (0, 0);
        }
    }
}
