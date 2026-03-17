namespace LVGLSharp
{
    internal static class WindowHostFactory
    {
        private static Func<string, int, int, IWindow>? s_factory;

        internal static void Register(Func<string, int, int, IWindow> factory)
        {
            ArgumentNullException.ThrowIfNull(factory);

            s_factory = factory;
        }

        internal static IWindow Create(string title, int width, int height)
        {
            return s_factory?.Invoke(title, width, height)
                ?? throw new InvalidOperationException("No LVGLSharp runtime has been configured. Call `Application.UseRuntime(...)` before `Application.Run(...)`.");
        }
    }

    internal static class RuntimeInputState
    {
        private static Func<uint>? s_currentMouseButtonProvider;

        internal static void RegisterCurrentMouseButtonProvider(Func<uint>? currentMouseButtonProvider)
        {
            s_currentMouseButtonProvider = currentMouseButtonProvider;
        }

        internal static uint GetCurrentMouseButton()
        {
            return s_currentMouseButtonProvider?.Invoke() ?? 0;
        }
    }
}
