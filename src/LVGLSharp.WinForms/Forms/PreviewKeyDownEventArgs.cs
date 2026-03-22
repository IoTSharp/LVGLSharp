namespace LVGLSharp.Forms
{
    public class PreviewKeyDownEventArgs : EventArgs
    {
        public PreviewKeyDownEventArgs()
            : this(Keys.None)
        {
        }

        public PreviewKeyDownEventArgs(Keys keyData)
        {
            KeyData = keyData;
        }

        public Keys KeyData { get; }

        public bool IsInputKey { get; set; }
    }
}