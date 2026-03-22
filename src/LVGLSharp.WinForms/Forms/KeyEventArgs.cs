namespace LVGLSharp.Forms
{
    public class KeyEventArgs : EventArgs
    {
        public KeyEventArgs()
            : this(Keys.None)
        {
        }

        public KeyEventArgs(Keys keyData)
        {
            KeyData = keyData;
        }

        public Keys KeyData { get; }

        public Keys KeyCode => KeyData & ~Keys.Modifiers;

        public bool Alt => (KeyData & Keys.Alt) == Keys.Alt;

        public bool Control => (KeyData & Keys.Control) == Keys.Control;

        public bool Shift => (KeyData & Keys.Shift) == Keys.Shift;

        public bool Handled { get; set; }

        public bool SuppressKeyPress { get; set; }
    }
}