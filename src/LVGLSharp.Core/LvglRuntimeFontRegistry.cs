using System.Threading;
using LVGLSharp.Interop;

namespace LVGLSharp
{
    public static class LvglRuntimeFontRegistry
    {
        private static nint s_activeTextFont;

        public static unsafe void SetActiveTextFont(lv_font_t* font)
        {
            Volatile.Write(ref s_activeTextFont, (nint)font);
        }

        public static void ClearActiveTextFont()
        {
            Volatile.Write(ref s_activeTextFont, nint.Zero);
        }

        public static unsafe lv_font_t* GetActiveTextFont()
        {
            return (lv_font_t*)Volatile.Read(ref s_activeTextFont);
        }
    }
}
