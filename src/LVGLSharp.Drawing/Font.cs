namespace LVGLSharp.Drawing;

public class Font
{
    public Font(string name, float size)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        Size = size;
    }

    public string Name { get; }

    public float Size { get; }
}
