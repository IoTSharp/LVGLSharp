namespace LVGLSharp.Drawing;

public readonly struct SizeF : IEquatable<SizeF>
{
    public SizeF(float width, float height)
    {
        Width = width;
        Height = height;
    }

    public float Width { get; }

    public float Height { get; }

    public static SizeF Empty => new(0, 0);

    public bool IsEmpty => Width == 0f && Height == 0f;

    public void Deconstruct(out float width, out float height)
    {
        width = Width;
        height = Height;
    }

    public static SizeF Add(SizeF size1, SizeF size2) => new(size1.Width + size2.Width, size1.Height + size2.Height);

    public static SizeF Subtract(SizeF size1, SizeF size2) => new(size1.Width - size2.Width, size1.Height - size2.Height);

    public Size ToSize() => new((int)Width, (int)Height);

    public bool Equals(SizeF other) => Width.Equals(other.Width) && Height.Equals(other.Height);

    public override bool Equals(object? obj) => obj is SizeF other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Width, Height);

    public override string ToString() => $"{{Width={Width}, Height={Height}}}";

    public static explicit operator PointF(SizeF size) => new(size.Width, size.Height);

    public static SizeF operator +(SizeF left, SizeF right) => Add(left, right);

    public static SizeF operator -(SizeF left, SizeF right) => Subtract(left, right);

    public static bool operator ==(SizeF left, SizeF right) => left.Equals(right);

    public static bool operator !=(SizeF left, SizeF right) => !left.Equals(right);
}
