using LVGLSharp.Interop;

namespace LVGLSharp;

/// <summary>
/// Represents the managed font state applied to an LVGL root object.
/// </summary>
public readonly unsafe struct LvglManagedFontState
{
    internal LvglManagedFontState(
        lv_font_t* fallbackFont,
        lv_font_t* managedFont,
        SixLaborsFontManager? fontManager,
        LvglFontDiagnostics diagnostics,
        lv_style_t* defaultFontStyle)
    {
        FallbackFont = fallbackFont;
        ManagedFont = managedFont;
        FontManager = fontManager;
        Diagnostics = diagnostics;
        DefaultFontStyle = defaultFontStyle;
    }

    public static LvglManagedFontState Empty => default;

    public lv_font_t* FallbackFont { get; }

    public lv_font_t* ManagedFont { get; }

    public SixLaborsFontManager? FontManager { get; }

    public LvglFontDiagnostics Diagnostics { get; }

    public lv_style_t* DefaultFontStyle { get; }

    public void ApplyTo(
        ref lv_font_t* fallbackFont,
        ref SixLaborsFontManager? fontManager,
        ref lv_style_t* defaultFontStyle)
    {
        fallbackFont = FallbackFont;
        fontManager = FontManager;
        defaultFontStyle = DefaultFontStyle;
    }

    public void ApplyTo(
        ref lv_font_t* fallbackFont,
        ref SixLaborsFontManager? fontManager,
        ref LvglFontDiagnostics diagnostics,
        ref lv_style_t* defaultFontStyle)
    {
        ApplyTo(ref fallbackFont, ref fontManager, ref defaultFontStyle);
        diagnostics = Diagnostics;
    }

    public void ApplyTo(
        ref lv_font_t* fallbackFont,
        ref lv_font_t* managedFont,
        ref SixLaborsFontManager? fontManager,
        ref LvglFontDiagnostics diagnostics,
        ref lv_style_t* defaultFontStyle)
    {
        ApplyTo(ref fallbackFont, ref fontManager, ref diagnostics, ref defaultFontStyle);
        managedFont = ManagedFont;
    }
}
