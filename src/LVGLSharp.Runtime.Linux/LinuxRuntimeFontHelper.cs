using LVGLSharp.Interop;

namespace LVGLSharp.Runtime.Linux;

internal static unsafe class LinuxRuntimeFontHelper
{
    internal static LvglManagedFontState InitializeRuntimeFont(
        lv_obj_t* root,
        float dpi,
        bool disableManagedFont = false,
        float size = 12)
    {
        bool managedFontEnabled = !disableManagedFont && LvglManagedFontHelper.IsManagedFontEnabled(defaultEnabled: true);
        var resolvedSystemFontPath = managedFontEnabled ? LinuxSystemFontResolver.TryResolveFontPath() : null;
        var fontDiagnostics = LvglFontDiagnostics.FromPath(
            resolvedSystemFontPath,
            LinuxSystemFontResolver.GetFontPathDiagnosticSummary(),
            LinuxSystemFontResolver.GetGlyphDiagnosticSummary());

        return LvglManagedFontHelper.InitializeManagedFont(
            root,
            resolvedSystemFontPath,
            size,
            dpi,
            fontDiagnostics,
            managedFontEnabled);
    }
}
