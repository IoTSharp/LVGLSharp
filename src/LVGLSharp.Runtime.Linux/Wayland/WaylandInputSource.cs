using System;

namespace LVGLSharp.Runtime.Linux;

internal sealed class WaylandInputSource : IDisposable
{
    public bool SupportsPointer => true;

    public bool SupportsKeyboard => true;

    public bool SupportsTextInput => false;

    public (int X, int Y) CurrentMousePosition { get; private set; }

    public uint CurrentMouseButton { get; private set; }

    public bool IsDisposed { get; private set; }

    public void UpdateMouseState(int x, int y, uint button)
    {
        ThrowIfDisposed();

        CurrentMousePosition = (x, y);
        CurrentMouseButton = button;
    }

    public void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
    }

    public void Dispose()
    {
        CurrentMousePosition = (0, 0);
        CurrentMouseButton = 0;
        IsDisposed = true;
    }
}
