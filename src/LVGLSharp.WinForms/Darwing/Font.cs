using System;
using System.Collections.Generic;
using System.Text;
namespace LVGLSharp.Darwing
{
    public class Font
    {
        public Font(string v1, float v2)
        {
            Name = v1;
            Size = v2;
        }

        public string Name { get; }
        public float Size { get; }
    }
}
