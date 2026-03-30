namespace LVGLSharp.Drawing;

public sealed class Font : IEquatable<Font>
{
    public Font(FontFamily family, float emSize)
        : this(family, emSize, FontStyle.Regular, GraphicsUnit.Point)
    {
    }

    public Font(FontFamily family, float emSize, FontStyle style)
        : this(family, emSize, style, GraphicsUnit.Point)
    {
    }

    public Font(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit)
        : this((family ?? throw new ArgumentNullException(nameof(family))).Name, emSize, style, unit)
    {
        FontFamily = family;
    }

    public Font(string familyName, float emSize)
        : this(familyName, emSize, FontStyle.Regular, GraphicsUnit.Point)
    {
    }

    public Font(string familyName, float emSize, FontStyle style)
        : this(familyName, emSize, style, GraphicsUnit.Point)
    {
    }

    public Font(string familyName, float emSize, FontStyle style, GraphicsUnit unit)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(familyName);
        if (emSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(emSize));
        }

        Name = familyName;
        Size = emSize;
        Style = style;
        Unit = unit;
        FontFamily = new FontFamily(familyName);
    }

    public string Name { get; }

    public float Size { get; }

    public float SizeInPoints => Unit == GraphicsUnit.Point ? Size : Size;

    public FontStyle Style { get; }

    public GraphicsUnit Unit { get; }

    public bool Bold => (Style & FontStyle.Bold) != 0;

    public bool Italic => (Style & FontStyle.Italic) != 0;

    public bool Underline => (Style & FontStyle.Underline) != 0;

    public bool Strikeout => (Style & FontStyle.Strikeout) != 0;

    public FontFamily FontFamily { get; }

    public string OriginalFontName => Name;

    public byte GdiCharSet => 1;

    public bool GdiVerticalFont => false;

    public Font Clone()
    {
        return new Font(FontFamily, Size, Style, Unit);
    }

    public Font WithSize(float emSize)
    {
        return new Font(FontFamily, emSize, Style, Unit);
    }

    public bool Equals(Font? other)
    {
        return other is not null &&
            string.Equals(Name, other.Name, StringComparison.Ordinal) &&
            Size.Equals(other.Size) &&
            Style == other.Style &&
            Unit == other.Unit;
    }

    public override bool Equals(object? obj) => obj is Font other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Name, Size, Style, Unit);

    public override string ToString() => $"[{Name}, {SizeInPoints}pt]";
}
