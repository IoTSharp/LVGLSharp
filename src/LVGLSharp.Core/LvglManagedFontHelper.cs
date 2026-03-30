using LVGLSharp.Interop;
using SixLabors.Fonts;
using SixLabors.Fonts.Unicode;
using System.Numerics;
using System.Reflection;
using System.Text;

namespace LVGLSharp;

public static unsafe class LvglManagedFontHelper
{
    private const string EnableManagedFontEnvironmentVariable = "LVGLSHARP_ENABLE_MANAGED_FONT";
    private const string DisableManagedFontEnvironmentVariable = "LVGLSHARP_DISABLE_CUSTOM_FONT";
    private const string EmbeddedFallbackFontResourceName = "LVGLSharp.Core.Assets.Fonts.NotoSansSC-Regular.otf";
    private const string EmbeddedFallbackFontFileName = "NotoSansSC-Regular.otf";

    /// <summary>
    /// Determines whether managed font rendering should be enabled for the current process.
    /// </summary>
    public static bool IsManagedFontEnabled(bool defaultEnabled = false)
    {
        if (string.Equals(Environment.GetEnvironmentVariable(EnableManagedFontEnvironmentVariable), "1", StringComparison.Ordinal))
        {
            return true;
        }

        if (string.Equals(Environment.GetEnvironmentVariable(DisableManagedFontEnvironmentVariable), "1", StringComparison.Ordinal))
        {
            return false;
        }

        return defaultEnabled;
    }

    /// <summary>
    /// Resolves the first available system font family from the provided candidates.
    /// </summary>
    public static bool TryResolveFontFamily(IEnumerable<string> fontFamilyNames, out FontFamily fontFamily)
    {
        ArgumentNullException.ThrowIfNull(fontFamilyNames);

        foreach (var fontFamilyName in fontFamilyNames)
        {
            if (string.IsNullOrWhiteSpace(fontFamilyName))
            {
                continue;
            }

            if (SystemFonts.TryGet(fontFamilyName, out fontFamily))
            {
                return true;
            }
        }

        fontFamily = default;
        return false;
    }

    /// <summary>
    /// Initializes managed font state from a resolved font file path.
    /// </summary>
    public static LvglManagedFontState InitializeManagedFont(
        lv_obj_t* root,
        string? fontPath,
        float size,
        float dpi,
        LvglFontDiagnostics diagnostics,
        bool enabled)
    {
        var fallbackFont = LvglFontHelper.GetEffectiveTextFont(root, lv_part_t.LV_PART_MAIN);
        if (!enabled)
        {
            return new LvglManagedFontState(
                fallbackFont,
                null,
                null,
                LvglFontDiagnostics.FromPath(null, "Source=LvglNativeFont; ManagedFontDisabled", null),
                null);
        }

        var fontManager = TryApplyManagedFont(root, fontPath, size, dpi, fallbackFont, out var font, out var style, enabled: true);
        if (fontManager is not null)
        {
            return new LvglManagedFontState(fallbackFont, font, fontManager, diagnostics, style);
        }

        if (TryResolveEmbeddedFallbackFontPath(out var embeddedFallbackFontPath))
        {
            var embeddedFallbackDiagnostics = CreateEmbeddedFallbackDiagnostics(embeddedFallbackFontPath, diagnostics.DiagnosticSummary);
            var embeddedFallbackFontManager = TryApplyManagedFont(root, embeddedFallbackFontPath, size, dpi, fallbackFont, out font, out style, enabled: true);
            if (embeddedFallbackFontManager is not null)
            {
                return new LvglManagedFontState(fallbackFont, font, embeddedFallbackFontManager, embeddedFallbackDiagnostics, style);
            }
        }

        return new LvglManagedFontState(
            fallbackFont,
            null,
            null,
            LvglFontDiagnostics.FromPath(null, $"{diagnostics.DisplaySummary}; Source=LvglNativeFont; EmbeddedFallback=<none>", diagnostics.GlyphDiagnosticSummary),
            null);
    }

    /// <summary>
    /// Initializes managed font state from a resolved font family.
    /// </summary>
    public static LvglManagedFontState InitializeManagedFont(
        lv_obj_t* root,
        FontFamily? fontFamily,
        IEnumerable<string> fontFamilyNames,
        float size,
        float dpi,
        bool enabled)
    {
        var diagnostics = CreateFontDiagnostics(fontFamily, fontFamilyNames, enabled);
        var fallbackFont = LvglFontHelper.GetEffectiveTextFont(root, lv_part_t.LV_PART_MAIN);
        if (!enabled)
        {
            return new LvglManagedFontState(fallbackFont, null, null, diagnostics, null);
        }

        var fontManager = TryApplyManagedFont(root, fontFamily, size, dpi, fallbackFont, out var font, out var style, enabled: true);
        if (fontManager is not null)
        {
            return new LvglManagedFontState(fallbackFont, font, fontManager, diagnostics, style);
        }

        if (TryResolveEmbeddedFallbackFontPath(out var embeddedFallbackFontPath))
        {
            var embeddedFallbackDiagnostics = CreateEmbeddedFallbackDiagnostics(embeddedFallbackFontPath, diagnostics.DiagnosticSummary);
            var embeddedFallbackFontManager = TryApplyManagedFont(root, embeddedFallbackFontPath, size, dpi, fallbackFont, out font, out style, enabled: true);
            if (embeddedFallbackFontManager is not null)
            {
                return new LvglManagedFontState(fallbackFont, font, embeddedFallbackFontManager, embeddedFallbackDiagnostics, style);
            }
        }

        return new LvglManagedFontState(
            fallbackFont,
            null,
            null,
            LvglFontDiagnostics.FromPath(null, $"{diagnostics.DisplaySummary}; Source=LvglNativeFont; EmbeddedFallback=<none>", diagnostics.GlyphDiagnosticSummary),
            null);
    }

    /// <summary>
    /// Creates a shared diagnostics model for managed font family resolution.
    /// </summary>
    public static LvglFontDiagnostics CreateFontDiagnostics(FontFamily? fontFamily, IEnumerable<string> fontFamilyNames, bool enabled)
    {
        return LvglFontDiagnostics.FromFontFamily(fontFamily, fontFamilyNames, enabled);
    }

    /// <summary>
    /// Resolves the shared embedded fallback font path extracted from the Core assembly resources.
    /// </summary>
    public static bool TryResolveEmbeddedFallbackFontPath(out string resolvedFontPath)
    {
        Assembly assembly = typeof(LvglManagedFontHelper).Assembly;
        using Stream? resourceStream = assembly.GetManifestResourceStream(EmbeddedFallbackFontResourceName);
        if (resourceStream is null)
        {
            resolvedFontPath = string.Empty;
            return false;
        }

        string targetDirectory = Path.Combine(Path.GetTempPath(), "LVGLSharp", "fonts");
        Directory.CreateDirectory(targetDirectory);

        resolvedFontPath = Path.Combine(targetDirectory, EmbeddedFallbackFontFileName);
        if (File.Exists(resolvedFontPath) && new FileInfo(resolvedFontPath).Length == resourceStream.Length)
        {
            return true;
        }

        using FileStream outputStream = File.Create(resolvedFontPath);
        resourceStream.CopyTo(outputStream);
        return true;
    }

    /// <summary>
    /// Creates diagnostics for the shared embedded fallback font.
    /// </summary>
    public static LvglFontDiagnostics CreateEmbeddedFallbackDiagnostics(string fontPath, string? previousSummary = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fontPath);

        string summary = string.IsNullOrWhiteSpace(previousSummary)
            ? $"Source=EmbeddedFallbackFont; Path={fontPath}"
            : $"{previousSummary}; Source=EmbeddedFallbackFont; Path={fontPath}";

        return LvglFontDiagnostics.FromPath(fontPath, summary, CreateGlyphDiagnosticSummary(fontPath));
    }

    /// <summary>
    /// Creates a glyph diagnostics summary for a font file path.
    /// </summary>
    public static string CreateGlyphDiagnosticSummary(string fontPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fontPath);

        try
        {
            FontCollection collection = new();
            FontFamily family = collection.Add(fontPath);
            Font font = family.CreateFont(12);
            Rune[] sampleRunes = [new Rune('图'), new Rune('像'), new Rune('路'), new Rune('径')];
            Dictionary<string, int> signatureCounts = new(StringComparer.Ordinal);
            List<string> parts = new(sampleRunes.Length);

            foreach (var rune in sampleRunes)
            {
                if (!TryGetGlyphSignature(font, rune, out var signature))
                {
                    parts.Add($"{rune}:Missing");
                    continue;
                }

                parts.Add($"{rune}:{signature}");
                signatureCounts[signature] = signatureCounts.TryGetValue(signature, out var count) ? count + 1 : 1;
            }

            bool uniformSignature = signatureCounts.Count == 1 && signatureCounts.Values.First() == sampleRunes.Length;
            return $"Uniform={uniformSignature}; Distinct={signatureCounts.Count}; Samples={string.Join(", ", parts)}";
        }
        catch (IOException ex)
        {
            return $"GlyphDiagError={ex.GetType().Name}:{ex.Message}";
        }
        catch (UnauthorizedAccessException ex)
        {
            return $"GlyphDiagError={ex.GetType().Name}:{ex.Message}";
        }
        catch (InvalidOperationException ex)
        {
            return $"GlyphDiagError={ex.GetType().Name}:{ex.Message}";
        }
    }

    /// <summary>
    /// Applies a managed font from the specified font file path when managed fonts are enabled.
    /// </summary>
    public static SixLaborsFontManager? TryApplyManagedFont(
        lv_obj_t* root,
        string? fontPath,
        float size,
        float dpi,
        lv_font_t* fallback,
        out lv_font_t* font,
        out lv_style_t* style,
        bool enabled)
    {
        font = null;
        style = null;
        if (!enabled || string.IsNullOrWhiteSpace(fontPath))
        {
            return null;
        }

        return LvglFontHelper.ApplyManagedFont(root, fontPath, size, dpi, fallback, out font, out style);
    }

    /// <summary>
    /// Applies a managed font from the specified font family when managed fonts are enabled.
    /// </summary>
    public static SixLaborsFontManager? TryApplyManagedFont(
        lv_obj_t* root,
        FontFamily? fontFamily,
        float size,
        float dpi,
        lv_font_t* fallback,
        out lv_font_t* font,
        out lv_style_t* style,
        bool enabled)
    {
        font = null;
        style = null;
        if (!enabled || fontFamily is null)
        {
            return null;
        }

        var manager = new SixLaborsFontManager(new Font(fontFamily.Value, size), dpi, fallback, LvglFontHelper.CreateDefaultFontFallbackGlyphs());
        font = manager.GetLvFontPtr();
        style = LvglFontHelper.ApplyDefaultFontStyle(root, font);
        return manager;
    }

    /// <summary>
    /// Releases managed font state and clears the active runtime font registry.
    /// </summary>
    public static void ReleaseManagedFont(
        ref lv_font_t* fallbackFont,
        ref lv_font_t* font,
        ref SixLaborsFontManager? fontManager,
        ref lv_style_t* defaultFontStyle)
    {
        LvglRuntimeFontRegistry.ClearActiveTextFont();
        fontManager?.Dispose();
        fontManager = null;
        fallbackFont = null;
        font = null;
        defaultFontStyle = null;
    }

    /// <summary>
    /// Releases managed font state and clears the active runtime font registry.
    /// </summary>
    public static void ReleaseManagedFont(
        ref lv_font_t* fallbackFont,
        ref SixLaborsFontManager? fontManager,
        ref lv_style_t* defaultFontStyle)
    {
        lv_font_t* font = null;
        ReleaseManagedFont(ref fallbackFont, ref font, ref fontManager, ref defaultFontStyle);
    }

    /// <summary>
    /// Releases managed font state and diagnostics.
    /// </summary>
    public static void ReleaseManagedFont(
        ref lv_font_t* fallbackFont,
        ref lv_font_t* font,
        ref SixLaborsFontManager? fontManager,
        ref LvglFontDiagnostics diagnostics,
        ref lv_style_t* defaultFontStyle)
    {
        ReleaseManagedFont(ref fallbackFont, ref font, ref fontManager, ref defaultFontStyle);
        diagnostics = LvglFontDiagnostics.Empty;
    }

    /// <summary>
    /// Releases managed font state and diagnostics.
    /// </summary>
    public static void ReleaseManagedFont(
        ref lv_font_t* fallbackFont,
        ref SixLaborsFontManager? fontManager,
        ref LvglFontDiagnostics diagnostics,
        ref lv_style_t* defaultFontStyle)
    {
        lv_font_t* font = null;
        ReleaseManagedFont(ref fallbackFont, ref font, ref fontManager, ref diagnostics, ref defaultFontStyle);
    }

    private static bool TryGetGlyphSignature(Font font, Rune rune, out string signature)
    {
        signature = string.Empty;

        if (!font.TryGetGlyphs(new CodePoint(rune.Value), out var glyphs) || glyphs.Count == 0)
        {
            return false;
        }

        FontRectangle bbox = glyphs[0].BoundingBox(GlyphLayoutMode.Horizontal, Vector2.Zero, 72f);
        signature = $"Count={glyphs.Count};Adv={Math.Round((double)glyphs[0].GlyphMetrics.AdvanceWidth, 2)};BBox={Math.Round((double)bbox.Width, 2)}x{Math.Round((double)bbox.Height, 2)}@{Math.Round((double)bbox.Left, 2)},{Math.Round((double)bbox.Top, 2)}";
        return true;
    }
}
