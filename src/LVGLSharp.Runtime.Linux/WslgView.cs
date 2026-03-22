namespace LVGLSharp.Runtime.Linux;

public unsafe class WslgView : X11View
{
    private readonly string _diagnosticSummary;

    public WslgView(string title = "LVGLSharp WSLg", int width = 800, int height = 600, float dpi = 96f, string? displayName = null, bool borderless = false)
        : base(
            LinuxEnvironmentDetector.FormatWslgWindowTitle(title, displayName),
            width,
            height,
            dpi,
            LinuxEnvironmentDetector.GetPreferredWslgDisplay(displayName),
            borderless)
    {
        _diagnosticSummary = LinuxEnvironmentDetector.GetWslgDiagnosticSummary(displayName);
    }

    public override string ToString() => $"{_diagnosticSummary}, {base.ToString()}";
}
