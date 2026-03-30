namespace LVGLSharp.Drawing;

public readonly struct RectangleF : IEquatable<RectangleF>
{
    public RectangleF(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public RectangleF(PointF location, SizeF size)
    {
        X = location.X;
        Y = location.Y;
        Width = size.Width;
        Height = size.Height;
    }

    public float X { get; }

    public float Y { get; }

    public float Width { get; }

    public float Height { get; }

    public static RectangleF Empty => new(0, 0, 0, 0);

    public bool IsEmpty => Width <= 0f || Height <= 0f;

    public PointF Location => new(X, Y);

    public SizeF Size => new(Width, Height);

    public float Left => X;

    public float Top => Y;

    public float Right => X + Width;

    public float Bottom => Y + Height;

    public void Deconstruct(out float x, out float y, out float width, out float height)
    {
        x = X;
        y = Y;
        width = Width;
        height = Height;
    }

    public static RectangleF FromLTRB(float left, float top, float right, float bottom) => new(left, top, right - left, bottom - top);

    public bool Contains(float x, float y) => x >= Left && x < Right && y >= Top && y < Bottom;

    public bool Contains(PointF point) => Contains(point.X, point.Y);

    public bool Contains(RectangleF rectangle) => rectangle.Left >= Left && rectangle.Right <= Right && rectangle.Top >= Top && rectangle.Bottom <= Bottom;

    public RectangleF Inflate(float width, float height) => FromLTRB(Left - width, Top - height, Right + width, Bottom + height);

    public RectangleF Offset(float x, float y) => new(X + x, Y + y, Width, Height);

    public RectangleF Offset(PointF point) => Offset(point.X, point.Y);

    public bool IntersectsWith(RectangleF rectangle) => rectangle.Left < Right && Left < rectangle.Right && rectangle.Top < Bottom && Top < rectangle.Bottom;

    public static RectangleF Intersect(RectangleF a, RectangleF b)
    {
        float left = Math.Max(a.Left, b.Left);
        float top = Math.Max(a.Top, b.Top);
        float right = Math.Min(a.Right, b.Right);
        float bottom = Math.Min(a.Bottom, b.Bottom);

        return right > left && bottom > top
            ? FromLTRB(left, top, right, bottom)
            : Empty;
    }

    public static RectangleF Union(RectangleF a, RectangleF b)
    {
        if (a.IsEmpty)
        {
            return b;
        }

        if (b.IsEmpty)
        {
            return a;
        }

        return FromLTRB(
            Math.Min(a.Left, b.Left),
            Math.Min(a.Top, b.Top),
            Math.Max(a.Right, b.Right),
            Math.Max(a.Bottom, b.Bottom));
    }

    public bool Equals(RectangleF other) => X.Equals(other.X) && Y.Equals(other.Y) && Width.Equals(other.Width) && Height.Equals(other.Height);

    public override bool Equals(object? obj) => obj is RectangleF other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

    public override string ToString() => $"{{X={X},Y={Y},Width={Width},Height={Height}}}";

    public static implicit operator RectangleF(Rectangle rectangle) => new(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

    public static bool operator ==(RectangleF left, RectangleF right) => left.Equals(right);

    public static bool operator !=(RectangleF left, RectangleF right) => !left.Equals(right);
}
