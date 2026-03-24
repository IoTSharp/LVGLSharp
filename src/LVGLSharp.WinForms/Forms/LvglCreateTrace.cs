using System;
using System.Threading;

namespace LVGLSharp.Forms
{
    internal static class LvglCreateTrace
    {
        private static int s_depth;

        internal static void Before(Control child)
        {
            if (!IsEnabled() || child is null)
            {
                return;
            }

            int depth = Interlocked.Increment(ref s_depth) - 1;
            string indent = new(' ', Math.Max(0, depth) * 2);
            string text = child.Text ?? string.Empty;
            if (text.Length > 48)
            {
                text = text[..48] + "...";
            }

            Console.Error.WriteLine($"{indent}create {child.GetType().Name} name='{child.Name}' text='{text.Replace('\n', ' ')}'");
        }

        internal static void After()
        {
            if (!IsEnabled())
            {
                return;
            }

            Interlocked.Decrement(ref s_depth);
        }

        private static bool IsEnabled()
        {
            string? value = Environment.GetEnvironmentVariable("LVGLSHARP_TRACE_CREATE");
            return string.Equals(value, "1", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
        }
    }
}
