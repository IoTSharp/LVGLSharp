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

            throw new InvalidOperationException("No LVGLSharp runtime has been configured. Reference the matching runtime assembly and call its registration method before running the application or loading images.");
        }
    }
}