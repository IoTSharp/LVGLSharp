namespace LVGLSharp.Forms
{
    public class KeyPressEventArgs : EventArgs
    {
        public KeyPressEventArgs()
            : this('\0')
        {
        }

        public KeyPressEventArgs(char keyChar)
        {
            KeyChar = keyChar;
        }

        public char KeyChar { get; set; }

        public bool Handled { get; set; }
    }
}