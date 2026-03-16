namespace LVGLSharp.Forms
{
    /// <summary>
    /// 表示右键菜单项
    /// </summary>
    public class ToolStripMenuItem
    {
        /// <summary>
        /// 菜单项文本
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// 菜单项是否启用
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 菜单项标签（用于标识）
        /// </summary>
        public object? Tag { get; set; }

        /// <summary>
        /// 快捷键
        /// </summary>
        public Keys ShortcutKeys { get; set; } = Keys.None;

        /// <summary>
        /// 是否显示快捷键
        /// </summary>
        public bool ShowShortcutKeys { get; set; } = true;

        /// <summary>
        /// 菜单项图标（LVGL 图像路径）
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// 是否是分隔符
        /// </summary>
        public bool IsSeparator { get; set; }

        /// <summary>
        /// 点击事件
        /// </summary>
        public event EventHandler? Click;

        /// <summary>
        /// 触发点击事件
        /// </summary>
        internal void PerformClick()
        {
            if (Enabled && !IsSeparator)
            {
                Click?.Invoke(this, EventArgs.Empty);
            }
        }

        public ToolStripMenuItem()
        {
        }

        public ToolStripMenuItem(string text)
        {
            Text = text;
        }

        public ToolStripMenuItem(string text, EventHandler? onClick)
        {
            Text = text;
            if (onClick != null)
            {
                Click += onClick;
            }
        }

        public ToolStripMenuItem(string text, EventHandler? onClick, Keys shortcutKeys)
        {
            Text = text;
            ShortcutKeys = shortcutKeys;
            if (onClick != null)
            {
                Click += onClick;
            }
        }

        /// <summary>
        /// 创建分隔符
        /// </summary>
        public static ToolStripMenuItem CreateSeparator()
        {
            return new ToolStripMenuItem { IsSeparator = true, Text = "-" };
        }

        /// <summary>
        /// 获取快捷键文本
        /// </summary>
        public string GetShortcutKeyText()
        {
            if (!ShowShortcutKeys || ShortcutKeys == Keys.None)
                return string.Empty;

            var parts = new List<string>();

            if ((ShortcutKeys & Keys.Control) == Keys.Control)
                parts.Add("Ctrl");
            if ((ShortcutKeys & Keys.Shift) == Keys.Shift)
                parts.Add("Shift");
            if ((ShortcutKeys & Keys.Alt) == Keys.Alt)
                parts.Add("Alt");

            var key = ShortcutKeys & ~Keys.Modifiers;
            if (key != Keys.None)
                parts.Add(key.ToString());

            return string.Join("+", parts);
        }
    }
}
