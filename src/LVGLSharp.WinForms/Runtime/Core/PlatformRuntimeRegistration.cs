using LVGLSharp.Drawing;

namespace LVGLSharp
{
    internal static class PlatformRuntimeRegistration
    {
        internal static void EnsureCurrentPlatformRegistered()
        {
            if (WindowHostFactory.IsRegistered && Image.IsFactoryRegistered && RuntimeInputState.IsRegistered)
            {
                return;
            }

            throw new InvalidOperationException("No LVGLSharp runtime has been configured. Reference `LVGLSharp.Runtime.Windows`, `LVGLSharp.Runtime.Linux`, and/or `LVGLSharp.Runtime.MacOs` so the runtime can be registered automatically during `ApplicationConfiguration.Initialize()`, or call `Application.UseRuntime(...)` and `Image.RegisterFactory(...)` manually before running the application or loading images.");
        }
    }
}
