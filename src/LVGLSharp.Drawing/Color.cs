namespace LVGLSharp.Drawing;

public readonly struct Color : IEquatable<Color>
{
    private readonly string? _name;

    private static readonly Dictionary<string, Color> s_namedColors = new(StringComparer.OrdinalIgnoreCase)
    {
        [nameof(Transparent)] = new Color(0, 255, 255, 255, nameof(Transparent)),
        [nameof(Black)] = new Color(255, 0, 0, 0, nameof(Black)),
        [nameof(White)] = new Color(255, 255, 255, 255, nameof(White)),
        [nameof(Red)] = new Color(255, 255, 0, 0, nameof(Red)),
        [nameof(Green)] = new Color(255, 0, 128, 0, nameof(Green)),
        [nameof(Blue)] = new Color(255, 0, 0, 255, nameof(Blue)),
        [nameof(Yellow)] = new Color(255, 255, 255, 0, nameof(Yellow)),
        [nameof(Gray)] = new Color(255, 128, 128, 128, nameof(Gray)),
        [nameof(Grey)] = new Color(255, 128, 128, 128, nameof(Grey)),
        [nameof(Orange)] = new Color(255, 255, 165, 0, nameof(Orange)),
        [nameof(Purple)] = new Color(255, 128, 0, 128, nameof(Purple)),
        [nameof(Brown)] = new Color(255, 165, 42, 42, nameof(Brown)),
        [nameof(Pink)] = new Color(255, 255, 192, 203, nameof(Pink)),
        [nameof(Cyan)] = new Color(255, 0, 255, 255, nameof(Cyan)),
        [nameof(Magenta)] = new Color(255, 255, 0, 255, nameof(Magenta)),
        [nameof(Silver)] = new Color(255, 192, 192, 192, nameof(Silver)),
        [nameof(LightGray)] = new Color(255, 211, 211, 211, nameof(LightGray)),
        [nameof(LightGrey)] = new Color(255, 211, 211, 211, nameof(LightGrey)),
        [nameof(DarkGray)] = new Color(255, 169, 169, 169, nameof(DarkGray)),
        [nameof(DarkGrey)] = new Color(255, 169, 169, 169, nameof(DarkGrey)),
    };

    public Color(byte r, byte g, byte b, byte a = 255)
        : this(a, r, g, b, null)
    {
    }

    private Color(byte a, byte r, byte g, byte b, string? name)
    {
        A = a;
        R = r;
        G = g;
        B = b;
        _name = name;
    }

    public byte A { get; }

    public byte R { get; }

    public byte G { get; }

    public byte B { get; }

    public bool IsEmpty => A == 0 && R == 0 && G == 0 && B == 0 && _name is null;

    public bool IsNamedColor => _name is not null;

    public bool IsKnownColor => _name is not null && s_namedColors.ContainsKey(_name);

    public bool IsSystemColor => false;

    public string Name => _name ?? (IsEmpty ? nameof(Empty) : ToArgb().ToString("X8"));

    public static Color Empty => default;

    public static Color Transparent => new(0, 255, 255, 255, nameof(Transparent));

    public static Color Black => new(255, 0, 0, 0, nameof(Black));

    public static Color White => new(255, 255, 255, 255, nameof(White));

    public static Color Red => new(255, 255, 0, 0, nameof(Red));

    public static Color Green => new(255, 0, 128, 0, nameof(Green));

    public static Color Blue => new(255, 0, 0, 255, nameof(Blue));

    public static Color Yellow => new(255, 255, 255, 0, nameof(Yellow));

    public static Color Gray => new(255, 128, 128, 128, nameof(Gray));

    public static Color Grey => new(255, 128, 128, 128, nameof(Grey));

    public static Color Orange => new(255, 255, 165, 0, nameof(Orange));

    public static Color Purple => new(255, 128, 0, 128, nameof(Purple));

    public static Color Brown => new(255, 165, 42, 42, nameof(Brown));

    public static Color Pink => new(255, 255, 192, 203, nameof(Pink));

    public static Color Cyan => new(255, 0, 255, 255, nameof(Cyan));

    public static Color Magenta => new(255, 255, 0, 255, nameof(Magenta));

    public static Color Silver => new(255, 192, 192, 192, nameof(Silver));

    public static Color LightGray => new(255, 211, 211, 211, nameof(LightGray));

    public static Color LightGrey => new(255, 211, 211, 211, nameof(LightGrey));

    public static Color DarkGray => new(255, 169, 169, 169, nameof(DarkGray));

    public static Color DarkGrey => new(255, 169, 169, 169, nameof(DarkGrey));

    public static Color FromKnownColor(KnownColor color)
    {
        return FromName(color.ToString());
    }

    public static Color FromArgb(int argb)
    {
        return new Color(
            (byte)((argb >> 24) & 0xFF),
            (byte)((argb >> 16) & 0xFF),
            (byte)((argb >> 8) & 0xFF),
            (byte)(argb & 0xFF),
            null);
    }

    public static Color FromArgb(int red, int green, int blue)
    {
        return FromArgb(255, red, green, blue);
    }

    public static Color FromArgb(int alpha, int red, int green, int blue)
    {
        return new Color(ToByte(alpha), ToByte(red), ToByte(green), ToByte(blue), null);
    }

    public static Color FromArgb(int alpha, Color baseColor)
    {
        return new Color(ToByte(alpha), baseColor.R, baseColor.G, baseColor.B, baseColor._name);
    }

    public static Color FromName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return s_namedColors.TryGetValue(name, out var color)
            ? color
            : new Color(0, 0, 0, 0, name);
    }

    public int ToArgb()
    {
        return (A << 24) | (R << 16) | (G << 8) | B;
    }

    public bool Equals(Color other) => A == other.A && R == other.R && G == other.G && B == other.B && string.Equals(_name, other._name, StringComparison.Ordinal);

    public override bool Equals(object? obj) => obj is Color other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(A, R, G, B, _name);

    public override string ToString() => Name;

    public static bool operator ==(Color left, Color right) => left.Equals(right);

    public static bool operator !=(Color left, Color right) => !left.Equals(right);

    private static byte ToByte(int value)
    {
        return value < byte.MinValue || value > byte.MaxValue
            ? throw new ArgumentOutOfRangeException(nameof(value))
            : (byte)value;
    }
}
