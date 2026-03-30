namespace LVGLSharp.Drawing;

public readonly struct Point : IEquatable<Point>
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point(Size size)
    {
        X = size.Width;
        Y = size.Height;
    }

    public int X { get; }

    public int Y { get; }

    public static Point Empty => new(0, 0);

    public bool IsEmpty => X == 0 && Y == 0;

    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }

    public static Point Add(Point point, Size size) => new(point.X + size.Width, point.Y + size.Height);

    public static Point Subtract(Point point, Size size) => new(point.X - size.Width, point.Y - size.Height);

    public static Point Ceiling(PointF value) => new((int)Math.Ceiling(value.X), (int)Math.Ceiling(value.Y));

    public static Point Round(PointF value) => new((int)Math.Round(value.X), (int)Math.Round(value.Y));

    public static Point Truncate(PointF value) => new((int)value.X, (int)value.Y);

    public Point Offset(int dx, int dy) => new(X + dx, Y + dy);

    public Point Offset(Point point) => Offset(point.X, point.Y);

    public bool Equals(Point other) => X == other.X && Y == other.Y;

    public override bool Equals(object? obj) => obj is Point other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public override string ToString() => $"{{X={X},Y={Y}}}";

    public static explicit operator Size(Point point) => new(point);

    public static Point operator +(Point point, Size size) => Add(point, size);

    public static Point operator -(Point point, Size size) => Subtract(point, size);

    public static bool operator ==(Point left, Point right) => left.Equals(right);

    public static bool operator !=(Point left, Point right) => !left.Equals(right);
}
