using SixLabors.Fonts;

namespace LVGLSharp;

public readonly record struct LvglFontDiagnostics(
    string? ResolvedFontPath,
    string? DiagnosticSummary,
    string? GlyphDiagnosticSummary)
{
    public string DisplayResolvedFontPath => string.IsNullOrWhiteSpace(ResolvedFontPath) ? "<none>" : ResolvedFontPath;

    public string DisplaySummary => string.IsNullOrWhiteSpace(DiagnosticSummary) ? "<unresolved>" : DiagnosticSummary;

    public string DisplayGlyphSummary => string.IsNullOrWhiteSpace(GlyphDiagnosticSummary) ? "<unresolved>" : GlyphDiagnosticSummary;

    public static LvglFontDiagnostics Empty => default;

    public static LvglFontDiagnostics FromPath(string? resolvedFontPath, string? diagnosticSummary, string? glyphDiagnosticSummary)
    {
        return new LvglFontDiagnostics(resolvedFontPath, diagnosticSummary, glyphDiagnosticSummary);
    }

    public static LvglFontDiagnostics FromFontFamily(FontFamily? fontFamily, IEnumerable<string> candidates, bool enabled)
    {
        ArgumentNullException.ThrowIfNull(candidates);

        if (!enabled)
        {
            return new LvglFontDiagnostics(null, "Source=LvglNativeFont; ManagedFontDisabled", null);
        }

        var candidateList = string.Join(", ", candidates.Where(static candidate => !string.IsNullOrWhiteSpace(candidate)));
        if (fontFamily is null)
        {
            return new LvglFontDiagnostics(null, $"Source=PlatformSystemFont; ManagedFontEnabled; Family=<none>; Candidates={candidateList}", null);
        }

        return new LvglFontDiagnostics(
            fontFamily.Value.Name,
            $"Source=PlatformSystemFont; ManagedFontEnabled; Family={fontFamily.Value.Name}; Candidates={candidateList}",
            null);
    }
}
