namespace LVGLSharp.WPF;

public enum ResourceDictionaryLocation
{
    None,
    SourceAssembly,
}

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class ThemeInfoAttribute : Attribute
{
    public ThemeInfoAttribute(ResourceDictionaryLocation themeDictionaryLocation, ResourceDictionaryLocation genericDictionaryLocation)
    {
        ThemeDictionaryLocation = themeDictionaryLocation;
        GenericDictionaryLocation = genericDictionaryLocation;
    }

    public ResourceDictionaryLocation ThemeDictionaryLocation { get; }

    public ResourceDictionaryLocation GenericDictionaryLocation { get; }
}

public enum HorizontalAlignment
{
    Left,
    Center,
    Right,
    Stretch,
}

public enum VerticalAlignment
{
    Top,
    Center,
    Bottom,
    Stretch,
}

public enum TextWrapping
{
    NoWrap,
    Wrap,
}

public readonly struct Thickness
{
    public Thickness(double uniform)
    {
        Left = uniform;
        Top = uniform;
        Right = uniform;
        Bottom = uniform;
    }

    public Thickness(double left, double top, double right, double bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public double Left { get; }

    public double Top { get; }

    public double Right { get; }

    public double Bottom { get; }
}
