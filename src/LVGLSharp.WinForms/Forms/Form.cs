using LVGLSharp.Runtime.Windows;
using System;
using System.Drawing;

namespace LVGLSharp.Forms
{
    public class Form: Control
    {
        unsafe  IWindow window;

        public Form()
        {
#if LINUX
        window = new LinuxView(dpi: 96f);
#else
            window = new Win32Window("LVGLSharp", 710, 470);
#endif
            window.Init();


        }
        protected virtual void  Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }
        public SizeF AutoScaleDimensions { get; set; }

        public AutoScaleMode AutoScaleMode { get; set; }
        public void SuspendLayout()
        {
        }
    }
}