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

    public static LvglFontDiagnostics FromNativeFont(string details)
    {
        return new LvglFontDiagnostics(null, CreateSummary("LvglNativeFont", "Fallback", null, null, null, details), null);
    }

    public static LvglFontDiagnostics FromPath(string? resolvedFontPath, string? diagnosticSummary, string? glyphDiagnosticSummary)
    {
        return new LvglFontDiagnostics(resolvedFontPath, diagnosticSummary, glyphDiagnosticSummary);
    }

    public static LvglFontDiagnostics FromPlatformFontPath(
        string? resolvedFontPath,
        string details,
        string? glyphDiagnosticSummary,
        string? familyName = null,
        IEnumerable<string>? candidates = null)
    {
        return new LvglFontDiagnostics(
            resolvedFontPath,
            CreateSummary("PlatformSystemFont", resolvedFontPath is null ? "Unavailable" : "Resolved", familyName, resolvedFontPath, candidates, details),
            glyphDiagnosticSummary);
    }

    public static LvglFontDiagnostics FromEmbeddedFallbackFont(string resolvedFontPath, string details, string? glyphDiagnosticSummary)
    {
        return new LvglFontDiagnostics(
            resolvedFontPath,
            CreateSummary("EmbeddedFallbackFont", "Resolved", null, resolvedFontPath, null, details),
            glyphDiagnosticSummary);
    }

    public static LvglFontDiagnostics FromFontFamily(FontFamily? fontFamily, IEnumerable<string> candidates, bool enabled, string? details = null)
    {
        ArgumentNullException.ThrowIfNull(candidates);

        var candidateList = candidates.Where(static candidate => !string.IsNullOrWhiteSpace(candidate)).ToArray();

        if (!enabled)
        {
            string disabledDetails = string.IsNullOrWhiteSpace(details)
                ? "ManagedFontDisabled"
                : $"ManagedFontDisabled; {details}";
            return new LvglFontDiagnostics(null, CreateSummary("LvglNativeFont", "Disabled", null, null, candidateList, disabledDetails), null);
        }

        if (fontFamily is null)
        {
            string unavailableDetails = string.IsNullOrWhiteSpace(details)
                ? "Resolver=SystemFonts"
                : details;
            return new LvglFontDiagnostics(null, CreateSummary("PlatformSystemFont", "Unavailable", null, null, candidateList, unavailableDetails), null);
        }

        string resolvedDetails = string.IsNullOrWhiteSpace(details)
            ? "Resolver=SystemFonts"
            : details;
        return new LvglFontDiagnostics(
            fontFamily.Value.Name,
            CreateSummary("PlatformSystemFont", "Resolved", fontFamily.Value.Name, fontFamily.Value.Name, candidateList, resolvedDetails),
            null);
    }

    private static string CreateSummary(
        string source,
        string outcome,
        string? family,
        string? path,
        IEnumerable<string>? candidates,
        string? details)
    {
        string candidateText = candidates is null
            ? "<none>"
            : string.Join(", ", candidates.Where(static candidate => !string.IsNullOrWhiteSpace(candidate)));

        if (string.IsNullOrWhiteSpace(candidateText))
        {
            candidateText = "<none>";
        }

        return $"Source={source}; Outcome={outcome}; Family={family ?? "<none>"}; Path={path ?? "<none>"}; Candidates={candidateText}; Details={details ?? "<none>"}";
    }
}
