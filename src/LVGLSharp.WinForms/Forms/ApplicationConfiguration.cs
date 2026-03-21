using System;

namespace LVGLSharp.Forms
{
    public static class ApplicationConfiguration
    {
        private static Action? s_windowsRuntimeInitializer;
        private static Action? s_linuxRuntimeInitializer;

        public static void Initialize()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);

            if (OperatingSystem.IsWindows())
            {
                s_windowsRuntimeInitializer?.Invoke();
            }
            if (OperatingSystem.IsLinux())
            {
                s_linuxRuntimeInitializer?.Invoke();
            }
        }

        /// <summary>
        /// Registers the Windows runtime initializer that runs as part of <see cref="Initialize"/> on Windows.
        /// </summary>
        /// <param name="runtimeInitializer">The Windows runtime initializer to execute.</param>
        public static void RegisterWindowsRuntimeInitializer(Action runtimeInitializer)
        {
            ArgumentNullException.ThrowIfNull(runtimeInitializer);

            if (s_windowsRuntimeInitializer is not null)
            {
                throw new InvalidOperationException("A Windows runtime initializer has already been registered.");
            }

            s_windowsRuntimeInitializer = runtimeInitializer;
        }

        /// <summary>
        /// Registers the Linux runtime initializer that runs as part of <see cref="Initialize"/> on Linux.
        /// </summary>
        /// <param name="runtimeInitializer">The Linux runtime initializer to execute.</param>
        public static void RegisterLinuxRuntimeInitializer(Action runtimeInitializer)
        {
            ArgumentNullException.ThrowIfNull(runtimeInitializer);

            if (s_linuxRuntimeInitializer is not null)
            {
                throw new InvalidOperationException("A Linux runtime initializer has already been registered.");
            }

            s_linuxRuntimeInitializer = runtimeInitializer;
        }
    }
}