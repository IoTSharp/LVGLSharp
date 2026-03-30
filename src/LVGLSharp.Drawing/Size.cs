namespace LVGLSharp.Drawing;

public readonly struct Size : IEquatable<Size>
{
    public Size(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public Size(Point point)
    {
        Width = point.X;
        Height = point.Y;
    }

    public int Width { get; }

    public int Height { get; }

    public static Size Empty => new(0, 0);

    public bool IsEmpty => Width == 0 && Height == 0;

    public void Deconstruct(out int width, out int height)
    {
        width = Width;
        height = Height;
    }

    public static Size Add(Size size1, Size size2) => new(size1.Width + size2.Width, size1.Height + size2.Height);

    public static Size Subtract(Size size1, Size size2) => new(size1.Width - size2.Width, size1.Height - size2.Height);

    public static Size Ceiling(SizeF value) => new((int)Math.Ceiling(value.Width), (int)Math.Ceiling(value.Height));

    public static Size Round(SizeF value) => new((int)Math.Round(value.Width), (int)Math.Round(value.Height));

    public static Size Truncate(SizeF value) => new((int)value.Width, (int)value.Height);

    public Point ToPoint() => new(Width, Height);

    public bool Equals(Size other) => Width == other.Width && Height == other.Height;

    public override bool Equals(object? obj) => obj is Size other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Width, Height);

    public override string ToString() => $"{{Width={Width}, Height={Height}}}";

    public static explicit operator Point(Size size) => new(size);

    public static implicit operator SizeF(Size size) => new(size.Width, size.Height);

    public static Size operator +(Size left, Size right) => Add(left, right);

    public static Size operator -(Size left, Size right) => Subtract(left, right);

    public static bool operator ==(Size left, Size right) => left.Equals(right);

    public static bool operator !=(Size left, Size right) => !left.Equals(right);
}
