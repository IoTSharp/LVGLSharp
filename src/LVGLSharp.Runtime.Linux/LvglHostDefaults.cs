using LVGLSharp.Interop;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LVGLSharp.Runtime.Linux
{
    internal static unsafe class LvglHostDefaults
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

        internal static HashSet<uint> CreateDefaultFontFallbackGlyphs()
        {
            return new HashSet<uint>(s_defaultFontFallbackGlyphs);
        }

        internal static lv_style_t* ApplyDefaultFontStyle(lv_obj_t* root, lv_font_t* font)
        {
            if (root == null || font == null)
            {
                return null;
            }

            // Apply the font directly to the root object so we don't depend on
            // manually allocating lv_style_t from managed code.
            LvglRuntimeFontRegistry.SetActiveTextFont(font);
            lv_obj_set_style_text_font(root, font, 0);
            return null;
        }
    }
}
