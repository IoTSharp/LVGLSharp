using System;

namespace LVGLSharp.Forms
{
    public static class ApplicationConfiguration
    {
        private static Action? s_windowsRuntimeInitializer;
        private static Action? s_linuxRuntimeInitializer;
        private static Action? s_macOsRuntimeInitializer;

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
            if (OperatingSystem.IsMacOS())
            {
                s_macOsRuntimeInitializer?.Invoke();
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

        /// <summary>
        /// Registers the macOS runtime initializer that runs as part of <see cref="Initialize"/> on macOS.
        /// </summary>
        /// <param name="runtimeInitializer">The macOS runtime initializer to execute.</param>
        public static void RegisterMacOsRuntimeInitializer(Action runtimeInitializer)
        {
            ArgumentNullException.ThrowIfNull(runtimeInitializer);

            if (s_macOsRuntimeInitializer is not null)
            {
                throw new InvalidOperationException("A macOS runtime initializer has already been registered.");
            }

            s_macOsRuntimeInitializer = runtimeInitializer;
        }
    }
}
