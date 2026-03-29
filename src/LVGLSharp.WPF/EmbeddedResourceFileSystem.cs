using System.Reflection;

namespace LVGLSharp.WPF;

public static class EmbeddedResourceFileSystem
{
    public static string NormalizePath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return string.Empty;
        }

        return path.Replace('\\', '/').TrimStart('/');
    }

    public static bool Exists(Assembly assembly, string path)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        return ResolveResourceName(assembly, path) is not null;
    }

    public static Stream OpenRead(Assembly assembly, string path)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var stream = TryOpenRead(assembly, path);
        if (stream is null)
        {
            throw new FileNotFoundException($"Embedded resource not found: {NormalizePath(path)}", NormalizePath(path));
        }

        return stream;
    }

    public static Stream? TryOpenRead(Assembly assembly, string path)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var resourceName = ResolveResourceName(assembly, path);
        return resourceName is null ? null : assembly.GetManifestResourceStream(resourceName);
    }

    public static string MaterializeToCache(Assembly assembly, string path, string cacheRoot)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentException.ThrowIfNullOrWhiteSpace(cacheRoot);

        string normalized = NormalizePath(path);
        Directory.CreateDirectory(cacheRoot);

        string safeName = normalized.Replace('/', '_').Replace('\\', '_');
        if (string.IsNullOrWhiteSpace(Path.GetExtension(safeName)))
        {
            safeName += ".bin";
        }

        string outputPath = Path.Combine(cacheRoot, safeName);

        using var input = OpenRead(assembly, normalized);
        using var output = File.Create(outputPath);
        input.CopyTo(output);

        return outputPath;
    }

    private static string? ResolveResourceName(Assembly assembly, string path)
    {
        string normalized = NormalizePath(path);

        if (assembly.GetManifestResourceNames().Contains(normalized, StringComparer.Ordinal))
        {
            return normalized;
        }

        return assembly
            .GetManifestResourceNames()
            .FirstOrDefault(name => NormalizePath(name).EndsWith(normalized, StringComparison.OrdinalIgnoreCase));
    }
}