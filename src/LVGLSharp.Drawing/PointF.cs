namespace LVGLSharp.Drawing;

public readonly struct PointF : IEquatable<PointF>
{
    public PointF(float x, float y)
    {
        X = x;
        Y = y;
    }

    public PointF(Point point)
    {
        X = point.X;
        Y = point.Y;
    }

    public float X { get; }

    public float Y { get; }

    public static PointF Empty => new(0, 0);

    public bool IsEmpty => X == 0f && Y == 0f;

    public void Deconstruct(out float x, out float y)
    {
        x = X;
        y = Y;
    }

    public static PointF Add(PointF point, SizeF size) => new(point.X + size.Width, point.Y + size.Height);

    public static PointF Subtract(PointF point, SizeF size) => new(point.X - size.Width, point.Y - size.Height);

    public PointF Offset(float dx, float dy) => new(X + dx, Y + dy);

    public bool Equals(PointF other) => X.Equals(other.X) && Y.Equals(other.Y);

    public override bool Equals(object? obj) => obj is PointF other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public override string ToString() => $"{{X={X},Y={Y}}}";

    public static implicit operator PointF(Point point) => new(point);

    public static PointF operator +(PointF point, SizeF size) => Add(point, size);

    public static PointF operator -(PointF point, SizeF size) => Subtract(point, size);

    public static bool operator ==(PointF left, PointF right) => left.Equals(right);

    public static bool operator !=(PointF left, PointF right) => !left.Equals(right);
}
