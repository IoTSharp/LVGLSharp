namespace LVGLSharp.Drawing;

public sealed class Region : ICloneable
{
    private static readonly Rectangle s_infiniteBounds = new(int.MinValue / 4, int.MinValue / 4, int.MaxValue / 2, int.MaxValue / 2);

    private Rectangle _bounds;
    private bool _isInfinite;

    public Region()
    {
        _isInfinite = false;
        _bounds = Rectangle.Empty;
    }

    public Region(Rectangle rect)
    {
        _isInfinite = false;
        _bounds = rect;
    }

    public Region(RectangleF rect)
        : this(Rectangle.Ceiling(rect))
    {
    }

    private Region(Rectangle bounds, bool isInfinite)
    {
        _bounds = bounds;
        _isInfinite = isInfinite;
    }

    public Rectangle Bounds => _isInfinite ? s_infiniteBounds : _bounds;

    public bool IsInfinite => _isInfinite;

    public bool IsEmpty => !_isInfinite && _bounds.IsEmpty;

    public object Clone() => new Region(_bounds, _isInfinite);

    public Rectangle GetBounds() => Bounds;

    public void MakeEmpty()
    {
        _isInfinite = false;
        _bounds = Rectangle.Empty;
    }

    public void MakeInfinite()
    {
        _isInfinite = true;
        _bounds = s_infiniteBounds;
    }

    public void Intersect(Rectangle rect)
    {
        if (_isInfinite)
        {
            _isInfinite = false;
            _bounds = rect;
            return;
        }

        _bounds = Rectangle.Intersect(_bounds, rect);
    }

    public void Intersect(Region region)
    {
        ArgumentNullException.ThrowIfNull(region);

        if (region._isInfinite)
        {
            return;
        }

        Intersect(region._bounds);
    }

    public void Union(Rectangle rect)
    {
        if (_isInfinite)
        {
            return;
        }

        _bounds = Rectangle.Union(_bounds, rect);
    }

    public void Union(Region region)
    {
        ArgumentNullException.ThrowIfNull(region);

        if (region._isInfinite)
        {
            MakeInfinite();
            return;
        }

        Union(region._bounds);
    }

    public void Exclude(Rectangle rect)
    {
        if (_isInfinite)
        {
            return;
        }

        _bounds = Subtract(_bounds, rect);
    }

    public void Exclude(Region region)
    {
        ArgumentNullException.ThrowIfNull(region);

        if (region._isInfinite)
        {
            MakeEmpty();
            return;
        }

        Exclude(region._bounds);
    }

    public void Xor(Rectangle rect)
    {
        if (_isInfinite)
        {
            return;
        }

        Rectangle left = Subtract(_bounds, rect);
        Rectangle right = Subtract(rect, _bounds);
        _bounds = Rectangle.Union(left, right);
    }

    public void Xor(Region region)
    {
        ArgumentNullException.ThrowIfNull(region);

        if (_isInfinite || region._isInfinite)
        {
            if (_isInfinite && region._isInfinite)
            {
                MakeEmpty();
                return;
            }

            MakeInfinite();
            return;
        }

        Xor(region._bounds);
    }

    public void Complement(Rectangle rect)
    {
        if (_isInfinite)
        {
            MakeEmpty();
            return;
        }

        _bounds = Subtract(rect, _bounds);
    }

    public void Complement(Region region)
    {
        ArgumentNullException.ThrowIfNull(region);

        if (region._isInfinite)
        {
            if (_isInfinite)
            {
                MakeEmpty();
                return;
            }

            MakeInfinite();
            return;
        }

        Complement(region._bounds);
    }

    public void Translate(int dx, int dy)
    {
        if (_isInfinite)
        {
            return;
        }

        _bounds = _bounds.Offset(dx, dy);
    }

    public bool IsVisible(int x, int y) => _isInfinite || _bounds.Contains(x, y);

    public bool IsVisible(Point point) => IsVisible(point.X, point.Y);

    public bool IsVisible(Rectangle rectangle) => _isInfinite || _bounds.IntersectsWith(rectangle) || _bounds.Contains(rectangle);

    public override string ToString() => _isInfinite ? "Infinite" : _bounds.ToString();

    private static Rectangle Subtract(Rectangle source, Rectangle excluded)
    {
        if (source.IsEmpty || excluded.IsEmpty || !source.IntersectsWith(excluded))
        {
            return source;
        }

        if (excluded.Contains(source))
        {
            return Rectangle.Empty;
        }

        Rectangle intersection = Rectangle.Intersect(source, excluded);
        Rectangle[] candidates =
        [
            Rectangle.FromLTRB(source.Left, source.Top, intersection.Left, source.Bottom),
            Rectangle.FromLTRB(intersection.Right, source.Top, source.Right, source.Bottom),
            Rectangle.FromLTRB(source.Left, source.Top, source.Right, intersection.Top),
            Rectangle.FromLTRB(source.Left, intersection.Bottom, source.Right, source.Bottom),
        ];

        Rectangle best = Rectangle.Empty;
        int bestArea = 0;
        foreach (var candidate in candidates)
        {
            if (candidate.IsEmpty)
            {
                continue;
            }

            int area = candidate.Width * candidate.Height;
            if (area > bestArea)
            {
                best = candidate;
                bestArea = area;
            }
        }

        return best;
    }
}
