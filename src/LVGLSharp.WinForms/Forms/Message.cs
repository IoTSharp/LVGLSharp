namespace LVGLSharp.Forms
{
    public struct Message
    {
        public Message(nint hWnd, uint msg, nuint wParam, nint lParam)
        {
            this.hWnd = hWnd;
            this.msg = msg;
            this.wParam = wParam;
            this.lParam = lParam;
        }

        public IntPtr hWnd { get; set; }
        public uint msg { get; set; }
        public UIntPtr wParam { get; set; }
        public IntPtr lParam { get; set; }
    }
}