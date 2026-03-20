using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LVGLSharp.Runtime.Linux;

internal static class LinuxSystemFontResolver
{
    private static readonly Lazy<string?> s_cachedFontPath = new(TryResolveFontPathCore);

    private static readonly string[] s_fontDirectories =
    [
        "/usr/share/fonts",
        "/usr/local/share/fonts",
        "/system/fonts",
    ];

    private static readonly string[] s_preferredFontFileNames =
    [
        "NotoSansCJK-Regular.ttc",
        "NotoSansCJKSC-Regular.otf",
        "NotoSansSC-Regular.otf",
        "NotoSansSC-Regular.ttf",
        "SourceHanSansSC-Regular.otf",
        "SourceHanSansCN-Regular.otf",
        "WenQuanYiZenHei.ttf",
        "wqy-zenhei.ttc",
        "msyh.ttc",
        "msyh.ttf",
        "simhei.ttf",
        "SimHei.ttf",
        "DroidSansFallbackFull.ttf",
        "ARPLUKaitiMGB.ttf",
        "uming.ttc",
        "ukai.ttc",
        "DejaVuSans.ttf",
        "LiberationSans-Regular.ttf",
        "FreeSans.ttf",
    ];

    private static readonly string[] s_preferredFamilyTokens =
    [
        "notosanscjksc",
        "notosanscjk",
        "notosanssc",
        "sourcehansanssc",
        "sourcehansanscn",
        "wenquanyi",
        "zenhei",
        "yahei",
        "simhei",
        "droidsansfallback",
        "ukai",
        "uming",
        "dejavusans",
        "liberationsans",
        "freesans",
    ];

    internal static string? TryResolveFontPath()
    {
        return s_cachedFontPath.Value;
    }

    private static string? TryResolveFontPathCore()
    {
        foreach (var candidate in EnumerateCandidateDirectories())
        {
            var resolved = TryResolveFromDirectory(candidate);
            if (!string.IsNullOrWhiteSpace(resolved))
            {
                return resolved;
            }
        }

        return null;
    }

    private static IEnumerable<string> EnumerateCandidateDirectories()
    {
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (!string.IsNullOrWhiteSpace(home))
        {
            yield return Path.Combine(home, ".local", "share", "fonts");
            yield return Path.Combine(home, ".fonts");
        }

        foreach (var directory in s_fontDirectories)
        {
            yield return directory;
        }
    }

    private static string? TryResolveFromDirectory(string directory)
    {
        if (!Directory.Exists(directory))
        {
            return null;
        }

        IReadOnlyList<string> fontFiles;
        try
        {
            fontFiles = Directory.EnumerateFiles(directory, "*.*", SearchOption.AllDirectories)
                .Where(static path =>
                    path.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase) ||
                    path.EndsWith(".ttc", StringComparison.OrdinalIgnoreCase) ||
                    path.EndsWith(".otf", StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }
        catch
        {
            return null;
        }

        foreach (var preferredFileName in s_preferredFontFileNames)
        {
            var exactMatch = fontFiles.FirstOrDefault(path =>
                string.Equals(Path.GetFileName(path), preferredFileName, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(exactMatch))
            {
                return exactMatch;
            }
        }

        foreach (var familyToken in s_preferredFamilyTokens)
        {
            var tokenMatch = fontFiles.FirstOrDefault(path =>
                Path.GetFileNameWithoutExtension(path).Contains(familyToken, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(tokenMatch))
            {
                return tokenMatch;
            }
        }

        return fontFiles.FirstOrDefault();
    }
}
