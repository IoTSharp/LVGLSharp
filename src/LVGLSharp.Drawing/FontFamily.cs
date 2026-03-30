namespace LVGLSharp.Drawing;

public sealed class FontFamily : IEquatable<FontFamily>
{
    public static FontFamily GenericSansSerif { get; } = new("Microsoft Sans Serif");

    public static FontFamily GenericSerif { get; } = new("Times New Roman");

    public static FontFamily GenericMonospace { get; } = new("Courier New");

    public FontFamily(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
    }

    public string Name { get; }

    public string GetName(int language) => Name;

    public bool IsStyleAvailable(FontStyle style) => style == FontStyle.Regular;

    public bool Equals(FontFamily? other) => other is not null && string.Equals(Name, other.Name, StringComparison.Ordinal);

    public override bool Equals(object? obj) => obj is FontFamily other && Equals(other);

    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Name);

    public override string ToString() => Name;
}
