using LVGLSharp.Interop;
using SixLabors.Fonts;

namespace LVGLSharp;

public static unsafe class LvglManagedFontHelper
{
    private const string EnableManagedFontEnvironmentVariable = "LVGLSHARP_ENABLE_MANAGED_FONT";
    private const string DisableManagedFontEnvironmentVariable = "LVGLSHARP_DISABLE_CUSTOM_FONT";

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
}
