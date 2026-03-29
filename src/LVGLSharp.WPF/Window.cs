using LVGLSharp.Forms;

namespace LVGLSharp.WPF;

public class Window
{
    private Control? _content;

    public string Title { get; set; } = "LVGLSharp.WPF";

    public double Width { get; set; } = 800;

    public double Height { get; set; } = 450;

    public Control? Content
    {
        get => _content;
        set => _content = value;
    }

    protected virtual Form CreateHostFormCore()
    {
        return new Form();
    }

    protected virtual void BuildContent(Form form)
    {
        if (_content is not null)
        {
            // Match WPF root content behavior: stretch to fill the window by default.
            if (_content.Dock == DockStyle.None)
            {
                _content.Dock = DockStyle.Fill;
            }

            form.Controls.Add(_content);
        }
    }

    internal Form CreateHostForm()
    {
        var form = CreateHostFormCore();
        if (form is null)
        {
            throw new InvalidOperationException("CreateHostFormCore() must return a non-null form instance.");
        }

        form.Text = Title;
        form.Width = (int)Math.Round(Width);
        form.Height = (int)Math.Round(Height);

        BuildContent(form);
        return form;
    }
}
