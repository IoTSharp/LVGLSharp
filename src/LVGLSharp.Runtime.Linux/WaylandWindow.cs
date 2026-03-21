using System;

namespace LVGLSharp.Runtime.Linux;

internal sealed class WaylandWindow : IDisposable
{
    public WaylandWindow(string title, int width, int height, bool borderless)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        Title = title;
        Width = width;
        Height = height;
        Borderless = borderless;
    }

    public string Title { get; }

    public int Width { get; }

    public int Height { get; }

    public bool Borderless { get; }

    public bool IsDisposed { get; private set; }

    public void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
    }

    public void Dispose()
    {
        IsDisposed = true;
    }
}
