using LVGLSharp.Drawing;

namespace LVGLSharp.Forms
{
    public class MouseEventArgs : EventArgs
    {
        public MouseEventArgs()
            : this(MouseButtons.None, 0, 0, 0, 0)
        {
        }

        public MouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta = 0)
        {
            Button = button;
            Clicks = clicks;
            X = x;
            Y = y;
            Delta = delta;
        }

        public MouseButtons Button { get; }

        public int Clicks { get; }

        public int X { get; }

        public int Y { get; }

        public int Delta { get; }

        public Point Location => new(X, Y);
    }
}
