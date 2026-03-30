using LVGLSharp.Interop;
using SixLabors.Fonts;
using System.Collections.Generic;

namespace LVGLSharp;

public static unsafe class LvglFontHelper
{
    private static readonly HashSet<uint> s_defaultFontFallbackGlyphs =
    [
        61441, 61448, 61451, 61452, 61453, 61457, 61459, 61461, 61465, 61468,
        61473, 61478, 61479, 61480, 61502, 61507, 61512, 61515, 61516, 61517,
        61521, 61522, 61523, 61524, 61543, 61544, 61550, 61552, 61553, 61556,
        61559, 61560, 61561, 61563, 61587, 61589, 61636, 61637, 61639, 61641,
        61664, 61671, 61674, 61683, 61724, 61732, 61787, 61931, 62016, 62017,
        62018, 62019, 62020, 62087, 62099, 62189, 62212, 62810, 63426, 63650
    ];

    /// <summary>
    /// Creates a copy of the default glyph set that should fall back to the native LVGL font.
    /// </summary>
    public static HashSet<uint> CreateDefaultFontFallbackGlyphs()
    {
        return new HashSet<uint>(s_defaultFontFallbackGlyphs);
    }

    /// <summary>
    /// Applies the specified font to the root object and records it as the active runtime text font.
    /// </summary>
    public static lv_style_t* ApplyDefaultFontStyle(lv_obj_t* root, lv_font_t* font)
    {
        if (root == null || font == null)
        {
            return null;
        }

        LvglRuntimeFontRegistry.SetActiveTextFont(font);
        lv_obj_set_style_text_font(root, font, 0);
        return null;
    }

    /// <summary>
    /// Creates a managed font from a font file path, applies it to the root object, and records it as the active runtime font.
    /// </summary>
    public static SixLaborsFontManager ApplyManagedFont(
        lv_obj_t* root,
        string fontPath,
        float size,
        float dpi,
        lv_font_t* fallback,
        out lv_font_t* font,
        out lv_style_t* style)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fontPath);

        var manager = new SixLaborsFontManager(fontPath, size, dpi, fallback, CreateDefaultFontFallbackGlyphs());
        font = manager.GetLvFontPtr();
        style = ApplyDefaultFontStyle(root, font);
        return manager;
    }

    /// <summary>
    /// Creates a managed font from a font family, applies it to the root object, and records it as the active runtime font.
    /// </summary>
    public static SixLaborsFontManager ApplyManagedFont(
        lv_obj_t* root,
        FontFamily fontFamily,
        float size,
        float dpi,
        lv_font_t* fallback,
        out lv_font_t* font,
        out lv_style_t* style)
    {
        var manager = new SixLaborsFontManager(fontFamily, size, dpi, fallback, CreateDefaultFontFallbackGlyphs());
        font = manager.GetLvFontPtr();
        style = ApplyDefaultFontStyle(root, font);
        return manager;
    }

    /// <summary>
    /// Resolves the effective text font for the object, falling back to the active runtime font and LVGL default font.
    /// </summary>
    public static lv_font_t* GetEffectiveTextFont(lv_obj_t* obj, lv_part_t part, lv_font_t* fallback = null)
    {
        if (obj != null)
        {
            try
            {
                var font = lv_obj_get_style_text_font(obj, part);
                if (font != null)
                {
                    return font;
                }
            }
            catch (NullReferenceException)
            {
                // Native style resolution can surface as NullReferenceException while the
                // LVGL style chain is still being initialized. Fall back to the active font.
            }
        }

        if (fallback != null)
        {
            return fallback;
        }

        fallback = LvglRuntimeFontRegistry.GetActiveTextFont();
        if (fallback != null)
        {
            return fallback;
        }

        return (lv_font_t*)lv_font_get_default();
    }
}
