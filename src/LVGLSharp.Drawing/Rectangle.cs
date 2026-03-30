namespace LVGLSharp.Drawing;

public readonly struct Rectangle : IEquatable<Rectangle>
{
    public Rectangle(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public Rectangle(Point location, Size size)
    {
        X = location.X;
        Y = location.Y;
        Width = size.Width;
        Height = size.Height;
    }

    public int X { get; }

    public int Y { get; }

    public int Width { get; }

    public int Height { get; }

    public static Rectangle Empty => new(0, 0, 0, 0);

    public bool IsEmpty => Width <= 0 || Height <= 0;

    public Point Location => new(X, Y);

    public Size Size => new(Width, Height);

    public int Left => X;

    public int Top => Y;

    public int Right => X + Width;

    public int Bottom => Y + Height;

    public void Deconstruct(out int x, out int y, out int width, out int height)
    {
        x = X;
        y = Y;
        width = Width;
        height = Height;
    }

    public static Rectangle FromLTRB(int left, int top, int right, int bottom) => new(left, top, right - left, bottom - top);

    public static Rectangle Ceiling(RectangleF value) => new(Point.Ceiling(value.Location), Size.Ceiling(value.Size));

    public static Rectangle Round(RectangleF value) => new(Point.Round(value.Location), Size.Round(value.Size));

    public static Rectangle Truncate(RectangleF value) => new(Point.Truncate(value.Location), Size.Truncate(value.Size));

    public bool Contains(int x, int y) => x >= Left && x < Right && y >= Top && y < Bottom;

    public bool Contains(Point point) => Contains(point.X, point.Y);

    public bool Contains(Rectangle rectangle) => rectangle.Left >= Left && rectangle.Right <= Right && rectangle.Top >= Top && rectangle.Bottom <= Bottom;

    public Rectangle Inflate(int width, int height) => FromLTRB(Left - width, Top - height, Right + width, Bottom + height);

    public Rectangle Inflate(Size size) => Inflate(size.Width, size.Height);

    public Rectangle Offset(int x, int y) => new(X + x, Y + y, Width, Height);

    public Rectangle Offset(Point point) => Offset(point.X, point.Y);

    public bool IntersectsWith(Rectangle rectangle) => rectangle.Left < Right && Left < rectangle.Right && rectangle.Top < Bottom && Top < rectangle.Bottom;

    public static Rectangle Intersect(Rectangle a, Rectangle b)
    {
        int left = Math.Max(a.Left, b.Left);
        int top = Math.Max(a.Top, b.Top);
        int right = Math.Min(a.Right, b.Right);
        int bottom = Math.Min(a.Bottom, b.Bottom);

        return right > left && bottom > top
            ? FromLTRB(left, top, right, bottom)
            : Empty;
    }

    public static Rectangle Union(Rectangle a, Rectangle b)
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

    public bool Equals(Rectangle other) => X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;

    public override bool Equals(object? obj) => obj is Rectangle other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

    public override string ToString() => $"{{X={X},Y={Y},Width={Width},Height={Height}}}";

    public static implicit operator RectangleF(Rectangle rectangle) => new(rectangle.Location, rectangle.Size);

    public static bool operator ==(Rectangle left, Rectangle right) => left.Equals(right);

    public static bool operator !=(Rectangle left, Rectangle right) => !left.Equals(right);
}
