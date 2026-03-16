namespace LVGLSharp.Forms
{
    /// <summary>
    /// 键盘修饰键状态管理
    /// </summary>
    public static class ModifierKeys
    {
        private static Keys _currentModifiers = Keys.None;

        public static Keys Current
        {
            get => _currentModifiers;
            internal set => _currentModifiers = value;
        }

        public static bool IsControlPressed => (_currentModifiers & Keys.Control) == Keys.Control;
        public static bool IsShiftPressed => (_currentModifiers & Keys.Shift) == Keys.Shift;
        public static bool IsAltPressed => (_currentModifiers & Keys.Alt) == Keys.Alt;
    }
}
