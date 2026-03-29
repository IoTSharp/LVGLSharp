using System.Globalization;
using System.Xml.Linq;
using LVGLSharp.Forms;
using Controls = LVGLSharp.WPF.Controls;

namespace LVGLSharp.WPF;

public static class XamlRuntimeLoader
{
    public static void LoadIntoWindow(Window window, string xamlRelativePath)
    {
        ArgumentNullException.ThrowIfNull(window);
        ArgumentException.ThrowIfNullOrWhiteSpace(xamlRelativePath);

        var doc = LoadXamlDocument(window.GetType().Assembly, EmbeddedResourceFileSystem.NormalizePath(xamlRelativePath));
        var root = doc.Root ?? throw new InvalidOperationException("XAML root element is missing.");

        if (!string.Equals(root.Name.LocalName, "Window", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Root element must be <Window>.");
        }

        ApplyWindowAttributes(window, root);

        var contentElement = root.Elements().FirstOrDefault();
        if (contentElement is null)
        {
            return;
        }

        window.Content = CreateControlTree(contentElement, window.GetType().Assembly);
    }

    public static ApplicationDefinition? TryLoadApplicationDefinition(System.Reflection.Assembly assembly, string appXamlRelativePath)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentException.ThrowIfNullOrWhiteSpace(appXamlRelativePath);

        var stream = EmbeddedResourceFileSystem.TryOpenRead(assembly, EmbeddedResourceFileSystem.NormalizePath(appXamlRelativePath));
        if (stream is null)
        {
            return null;
        }

        using (stream)
        {
            var doc = XDocument.Load(stream, LoadOptions.None);
            var root = doc.Root;
            if (root is null || !string.Equals(root.Name.LocalName, "Application", StringComparison.Ordinal))
            {
                return null;
            }

            return new ApplicationDefinition
            {
                StartupUri = EmbeddedResourceFileSystem.NormalizePath(GetAttribute(root, "StartupUri"))
            };
        }
    }

    private static XDocument LoadXamlDocument(System.Reflection.Assembly assembly, string xamlRelativePath)
    {
        using var stream = EmbeddedResourceFileSystem.OpenRead(assembly, xamlRelativePath);

        return XDocument.Load(stream, LoadOptions.None);
    }

    public static string NormalizeResourcePath(string? path) => EmbeddedResourceFileSystem.NormalizePath(path);

    private static void ApplyWindowAttributes(Window window, XElement element)
    {
        var title = GetAttribute(element, "Title");
        if (!string.IsNullOrWhiteSpace(title))
        {
            window.Title = title;
        }

        if (TryGetDoubleAttribute(element, "Width", out var width))
        {
            window.Width = width;
        }

        if (TryGetDoubleAttribute(element, "Height", out var height))
        {
            window.Height = height;
        }
    }

    private static Control CreateControlTree(XElement element, System.Reflection.Assembly assembly)
    {
        Control control = CreateControl(element.Name.LocalName);
        ApplyCommonProperties(control, element);
        ApplySpecificProperties(control, element, assembly);

        if (control is Controls.Grid grid)
        {
            foreach (var childElement in element.Elements())
            {
                grid.Children.Add(CreateControlTree(childElement, assembly));
            }
        }

        return control;
    }

    private static Control CreateControl(string elementName)
    {
        return elementName switch
        {
            "Grid" => new Controls.Grid(),
            "Button" => new Controls.Button(),
            "CheckBox" => new Controls.CheckBox(),
            "ComboBox" => new Controls.ComboBox(),
            "Label" => new Controls.Label(),
            "RadioButton" => new Controls.RadioButton(),
            "TextBlock" => new Controls.TextBlock(),
            "TextBox" => new Controls.TextBox(),
            "Image" => new Controls.Image(),
            _ => throw new NotSupportedException($"Unsupported XAML element: <{elementName}>.")
        };
    }

    private static void ApplyCommonProperties(Control control, XElement element)
    {
        if (TryGetIntAttribute(element, "Width", out var width))
        {
            control.Width = width;
        }

        if (TryGetIntAttribute(element, "Height", out var height))
        {
            control.Height = height;
        }

        if (TryGetThicknessAttribute(element, "Margin", out var margin))
        {
            ApplyMargin(control, margin);
        }

        if (TryGetEnumAttribute<HorizontalAlignment>(element, "HorizontalAlignment", out var horizontalAlignment))
        {
            ApplyHorizontalAlignment(control, horizontalAlignment);
        }

        if (TryGetEnumAttribute<VerticalAlignment>(element, "VerticalAlignment", out var verticalAlignment))
        {
            ApplyVerticalAlignment(control, verticalAlignment);
        }
    }

    private static void ApplySpecificProperties(Control control, XElement element, System.Reflection.Assembly assembly)
    {
        switch (control)
        {
            case Controls.Button button:
                button.Content = GetAttribute(element, "Content") ?? button.Content;
                break;
            case Controls.CheckBox checkBox:
                checkBox.Content = GetAttribute(element, "Content") ?? checkBox.Content;
                if (TryGetBoolAttribute(element, "IsChecked", out var checkBoxIsChecked))
                {
                    checkBox.IsChecked = checkBoxIsChecked;
                }
                break;
            case Controls.Label label:
                label.Content = GetAttribute(element, "Content") ?? label.Content;
                break;
            case Controls.RadioButton radioButton:
                radioButton.Content = GetAttribute(element, "Content") ?? radioButton.Content;
                if (TryGetBoolAttribute(element, "IsChecked", out var radioButtonIsChecked))
                {
                    radioButton.Checked = radioButtonIsChecked;
                }
                break;
            case Controls.ComboBox comboBox:
                ApplyComboBoxItems(comboBox, element);
                if (TryGetIntAttribute(element, "SelectedIndex", out var selectedIndex))
                {
                    comboBox.SelectedIndex = selectedIndex;
                }
                break;
            case Controls.TextBlock textBlock:
                textBlock.Text = GetAttribute(element, "Text") ?? textBlock.Text;
                if (TryGetEnumAttribute<TextWrapping>(element, "TextWrapping", out var textWrapping))
                {
                    textBlock.TextWrapping = textWrapping;
                }
                break;
            case Controls.TextBox textBox:
                textBox.Text = GetAttribute(element, "Text") ?? textBox.Text;
                if (TryGetEnumAttribute<TextWrapping>(element, "TextWrapping", out var textBoxWrapping))
                {
                    textBox.TextWrapping = textBoxWrapping;
                }
                break;
            case Controls.Image image:
                var source = GetAttribute(element, "Source");
                if (!string.IsNullOrWhiteSpace(source))
                {
                    image.Source = ResolveImageSourcePath(assembly, source);
                }
                break;
        }
    }

    private static string ResolveImageSourcePath(System.Reflection.Assembly assembly, string source)
    {
        var normalized = NormalizeImageSource(source);
        if (string.IsNullOrWhiteSpace(normalized))
        {
            return normalized;
        }

        if (File.Exists(normalized))
        {
            return normalized;
        }

        if (!EmbeddedResourceFileSystem.Exists(assembly, normalized))
        {
            return normalized;
        }

        string cacheDir = Path.Combine(AppContext.BaseDirectory, ".lvglsharp-wpf-assets");
        return EmbeddedResourceFileSystem.MaterializeToCache(assembly, normalized, cacheDir);
    }

    private static string NormalizeImageSource(string source)
    {
        var normalized = source.Trim();
        if (normalized.StartsWith("/", StringComparison.Ordinal))
        {
            normalized = normalized.TrimStart('/');
        }

        return normalized;
    }

    private static void ApplyMargin(Control control, Thickness margin)
    {
        switch (control)
        {
            case Controls.Button button:
                button.Margin = margin;
                break;
            case Controls.CheckBox checkBox:
                checkBox.Margin = margin;
                break;
            case Controls.ComboBox comboBox:
                comboBox.Margin = margin;
                break;
            case Controls.Label label:
                label.Margin = margin;
                break;
            case Controls.RadioButton radioButton:
                radioButton.Margin = margin;
                break;
            case Controls.TextBlock textBlock:
                textBlock.Margin = margin;
                break;
            case Controls.TextBox textBox:
                textBox.Margin = margin;
                break;
            case Controls.Image image:
                image.Margin = margin;
                break;
        }
    }

    private static void ApplyHorizontalAlignment(Control control, HorizontalAlignment value)
    {
        switch (control)
        {
            case Controls.Button button:
                button.HorizontalAlignment = value;
                break;
            case Controls.CheckBox checkBox:
                checkBox.HorizontalAlignment = value;
                break;
            case Controls.ComboBox comboBox:
                comboBox.HorizontalAlignment = value;
                break;
            case Controls.Label label:
                label.HorizontalAlignment = value;
                break;
            case Controls.RadioButton radioButton:
                radioButton.HorizontalAlignment = value;
                break;
            case Controls.TextBlock textBlock:
                textBlock.HorizontalAlignment = value;
                break;
            case Controls.TextBox textBox:
                textBox.HorizontalAlignment = value;
                break;
            case Controls.Image image:
                image.HorizontalAlignment = value;
                break;
        }
    }

    private static void ApplyVerticalAlignment(Control control, VerticalAlignment value)
    {
        switch (control)
        {
            case Controls.Button button:
                button.VerticalAlignment = value;
                break;
            case Controls.CheckBox checkBox:
                checkBox.VerticalAlignment = value;
                break;
            case Controls.ComboBox comboBox:
                comboBox.VerticalAlignment = value;
                break;
            case Controls.Label label:
                label.VerticalAlignment = value;
                break;
            case Controls.RadioButton radioButton:
                radioButton.VerticalAlignment = value;
                break;
            case Controls.TextBlock textBlock:
                textBlock.VerticalAlignment = value;
                break;
            case Controls.TextBox textBox:
                textBox.VerticalAlignment = value;
                break;
            case Controls.Image image:
                image.VerticalAlignment = value;
                break;
        }
    }

    private static bool TryGetIntAttribute(XElement element, string name, out int value)
    {
        value = default;
        var text = GetAttribute(element, name);
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var doubleValue))
        {
            return false;
        }

        value = (int)Math.Round(doubleValue);
        return true;
    }

    private static bool TryGetBoolAttribute(XElement element, string name, out bool value)
    {
        value = default;
        var text = GetAttribute(element, name);
        return !string.IsNullOrWhiteSpace(text) && bool.TryParse(text, out value);
    }

    private static bool TryGetDoubleAttribute(XElement element, string name, out double value)
    {
        value = default;
        var text = GetAttribute(element, name);
        return !string.IsNullOrWhiteSpace(text) &&
               double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }

    private static bool TryGetThicknessAttribute(XElement element, string name, out Thickness thickness)
    {
        thickness = default;
        var text = GetAttribute(element, name);
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        var parts = text.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1 && TryParseDouble(parts[0], out var all))
        {
            thickness = new Thickness(all);
            return true;
        }

        if (parts.Length == 2 && TryParseDouble(parts[0], out var horizontal) && TryParseDouble(parts[1], out var vertical))
        {
            thickness = new Thickness(horizontal, vertical, horizontal, vertical);
            return true;
        }

        if (parts.Length == 4 &&
            TryParseDouble(parts[0], out var left) &&
            TryParseDouble(parts[1], out var top) &&
            TryParseDouble(parts[2], out var right) &&
            TryParseDouble(parts[3], out var bottom))
        {
            thickness = new Thickness(left, top, right, bottom);
            return true;
        }

        return false;
    }

    private static bool TryGetEnumAttribute<TEnum>(XElement element, string name, out TEnum value)
        where TEnum : struct, Enum
    {
        value = default;
        var text = GetAttribute(element, name);
        return !string.IsNullOrWhiteSpace(text) && Enum.TryParse(text, ignoreCase: true, out value);
    }

    private static string? GetAttribute(XElement element, string name)
    {
        return element.Attribute(name)?.Value;
    }

    private static bool TryParseDouble(string text, out double value)
    {
        return double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }

    private static void ApplyComboBoxItems(Controls.ComboBox comboBox, XElement element)
    {
        foreach (var child in element.Elements())
        {
            if (string.Equals(child.Name.LocalName, "ComboBoxItem", StringComparison.Ordinal))
            {
                var content = GetAttribute(child, "Content");
                if (!string.IsNullOrWhiteSpace(content))
                {
                    comboBox.Items.Add(content);
                    continue;
                }

                var text = child.Value?.Trim();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    comboBox.Items.Add(text);
                }
            }
        }
    }

    public sealed class ApplicationDefinition
    {
        public string? StartupUri { get; init; }
    }
}