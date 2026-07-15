using System.Runtime.InteropServices;
using LVGLSharp.Runtime.Headless;
using Xunit;

namespace LVGLSharp.Headless.Tests;

internal static class HeadlessTestEnvironment
{
    public static OffscreenView CreateOpenView(OffscreenOptions options)
    {
        AssertNativeAssetsCopiedToOutput();

        var view = new OffscreenView(options);
        try
        {
            view.Open();
            return view;
        }
        catch (Exception ex) when (IsNativeLoadFailure(ex))
        {
            view.Dispose();
            throw new InvalidOperationException(
                $"[NativeLoadFailure] LVGL native library could not be loaded before LVGL initialization. {CreateDiagnosticSummary()}",
                ex);
        }
        catch (InvalidOperationException ex)
        {
            view.Dispose();
            throw new InvalidOperationException(
                $"[LvglInitializationFailure] OffscreenView failed during LVGL initialization or display creation. {CreateDiagnosticSummary()}",
                ex);
        }
    }

    public static void AssertNativeAssetsCopiedToOutput()
    {
        var expectedPaths = GetExpectedNativeAssetPaths().ToArray();
        var foundExpectedAsset = expectedPaths.Any(File.Exists);

        Assert.True(
            foundExpectedAsset,
            $"[NativeMissing] Expected LVGL native asset was not copied to the headless test output. ExpectedAny={string.Join(", ", expectedPaths)}; {CreateDiagnosticSummary()}");
    }

    public static string CreateDiagnosticSummary()
    {
        var nativeFiles = EnumerateOutputNativeFiles()
            .Select(path =>
            {
                var fileInfo = new FileInfo(path);
                return $"{Path.GetRelativePath(AppContext.BaseDirectory, path)} ({fileInfo.Length} bytes)";
            })
            .DefaultIfEmpty("none");

        return $"BaseDirectory={AppContext.BaseDirectory}; " +
               $"OS={RuntimeInformation.OSDescription}; " +
               $"Architecture={RuntimeInformation.ProcessArchitecture}; " +
               $"ExpectedRid={GetExpectedRuntimeIdentifier()}; " +
               $"OutputNativeFiles={string.Join(", ", nativeFiles)}";
    }

    private static IEnumerable<string> GetExpectedNativeAssetPaths()
    {
        var nativeDirectory = Path.Combine(AppContext.BaseDirectory, "runtimes", GetExpectedRuntimeIdentifier(), "native");

        foreach (var fileName in GetExpectedLibraryFileNames())
        {
            yield return Path.Combine(nativeDirectory, fileName);
        }
    }

    private static IEnumerable<string> EnumerateOutputNativeFiles()
    {
        var nativeRoot = Path.Combine(AppContext.BaseDirectory, "runtimes");
        if (!Directory.Exists(nativeRoot))
        {
            yield break;
        }

        foreach (var file in Directory.EnumerateFiles(nativeRoot, "*", SearchOption.AllDirectories).OrderBy(path => path, StringComparer.Ordinal))
        {
            yield return file;
        }
    }

    private static string GetExpectedRuntimeIdentifier()
    {
        if (OperatingSystem.IsWindows())
        {
            return RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X86 => "win-x86",
                Architecture.Arm64 => "win-arm64",
                _ => "win-x64"
            };
        }

        if (OperatingSystem.IsLinux())
        {
            return RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.Arm => "linux-arm",
                Architecture.Arm64 => "linux-arm64",
                _ => "linux-x64"
            };
        }

        if (OperatingSystem.IsMacOS())
        {
            return RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.Arm64 => "osx-arm64",
                _ => "osx-x64"
            };
        }

        return RuntimeInformation.RuntimeIdentifier;
    }

    private static IEnumerable<string> GetExpectedLibraryFileNames()
    {
        if (OperatingSystem.IsWindows())
        {
            yield return "lvgl.dll";
            yield break;
        }

        if (OperatingSystem.IsLinux())
        {
            yield return "liblvgl.so.9";
            yield return "liblvgl.so";
            yield break;
        }

        if (OperatingSystem.IsMacOS())
        {
            yield return "liblvgl.dylib";
            yield break;
        }

        yield return "lvgl";
    }

    private static bool IsNativeLoadFailure(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            if (current is DllNotFoundException or EntryPointNotFoundException or BadImageFormatException)
            {
                return true;
            }
        }

        return false;
    }
}
