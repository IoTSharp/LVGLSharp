using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LVGLSharp.Runtime.Linux;

internal enum LinuxHostEnvironment
{
    Wslg,
    X11,
    FrameBuffer,
}

internal static class LinuxEnvironmentDetector
{
    internal static string FormatWslgWindowTitle(string title, string? detectedDisplay)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        var preferredDisplay = GetDiagnosticValue(GetPreferredWslgDisplay(detectedDisplay));
        return $"{title} [WSLg:{preferredDisplay}]";
    }

    internal static string GetWslgDiagnosticSummary(string? detectedDisplay)
    {
        var processDisplay = GetDiagnosticValue(Environment.GetEnvironmentVariable("DISPLAY"));
        var preferredDisplay = GetDiagnosticValue(GetPreferredWslgDisplay(detectedDisplay));
        var waylandDisplay = GetDiagnosticValue(Environment.GetEnvironmentVariable("WAYLAND_DISPLAY"));
        var distroName = GetDiagnosticValue(Environment.GetEnvironmentVariable("WSL_DISTRO_NAME"));

        return $"WSLg DISPLAY={processDisplay}, PreferredX11={preferredDisplay}, WAYLAND={waylandDisplay}, DISTRO={distroName}";
    }

    internal static IReadOnlyList<string?> GetX11DisplayCandidates(string? preferredDisplay)
    {
        List<string?> candidates = [];
        HashSet<string> seen = new(StringComparer.Ordinal);

        if (!string.IsNullOrWhiteSpace(preferredDisplay) && seen.Add(preferredDisplay))
        {
            candidates.Add(preferredDisplay);
        }

        foreach (var detectedDisplay in EnumerateSocketDisplays())
        {
            if (seen.Add(detectedDisplay))
            {
                candidates.Add(detectedDisplay);
            }
        }

        return candidates;
    }

    internal static LinuxHostEnvironment ResolveHostEnvironment(string? detectedDisplay, string fbdev)
    {
        if (IsWslg(detectedDisplay))
        {
            return LinuxHostEnvironment.Wslg;
        }

        if (detectedDisplay is not null)
        {
            return LinuxHostEnvironment.X11;
        }

        if (File.Exists(fbdev))
        {
            return LinuxHostEnvironment.FrameBuffer;
        }

        return LinuxHostEnvironment.X11;
    }

    internal static string? DetectX11Display()
    {
        const string x11SocketDir = "/tmp/.X11-unix";
        if (Directory.Exists(x11SocketDir))
        {
            var displayEntry = Directory.EnumerateFiles(x11SocketDir, "X*")
                .Select(Path.GetFileName)
                .Select(static name => name is { Length: > 1 } value ? value[1..] : string.Empty)
                .Where(static value => value.Length > 0)
                .OrderBy(static value => value, StringComparer.Ordinal)
                .FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(displayEntry))
            {
                return $":{displayEntry}";
            }
        }

        return null;
    }

    private static IEnumerable<string> EnumerateSocketDisplays()
    {
        const string x11SocketDir = "/tmp/.X11-unix";
        if (!Directory.Exists(x11SocketDir))
        {
            yield break;
        }

        foreach (var displayEntry in Directory.EnumerateFiles(x11SocketDir, "X*")
            .Select(Path.GetFileName)
            .Select(static name => name is { Length: > 1 } value ? value[1..] : string.Empty)
            .Where(static value => value.Length > 0)
            .OrderBy(static value => value, StringComparer.Ordinal))
        {
            yield return $":{displayEntry}";
        }
    }

    internal static string? GetPreferredWslgDisplay(string? detectedDisplay)
    {
        var processDisplay = Environment.GetEnvironmentVariable("DISPLAY");
        if (!string.IsNullOrWhiteSpace(processDisplay))
        {
            return processDisplay;
        }

        if (!string.IsNullOrWhiteSpace(detectedDisplay))
        {
            return detectedDisplay;
        }

        return Directory.Exists("/mnt/wslg") ? ":0" : null;
    }

    private static bool IsWsl()
    {
        if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("WSL_DISTRO_NAME")) ||
            !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("WSL_INTEROP")))
        {
            return true;
        }

        const string procVersionPath = "/proc/version";
        if (!File.Exists(procVersionPath))
        {
            return false;
        }

        var procVersion = File.ReadAllText(procVersionPath);
        return procVersion.Contains("Microsoft", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsWslg(string? detectedDisplay)
    {
        if (!IsWsl())
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("WAYLAND_DISPLAY")) ||
            !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("WSLG_RUNTIME_DIR")) ||
            Directory.Exists("/mnt/wslg"))
        {
            return true;
        }

        return detectedDisplay is not null &&
            !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DISPLAY"));
    }

    private static string GetDiagnosticValue(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? "<unset>" : value;
    }
}
