namespace LVGLSharp.Runtime.MacOs;

public sealed class MacOsSurfaceSkeleton : IMacOsSurface
{
    private bool _disposed;
    private readonly MacOsInputState _inputState = new();

    public MacOsSurfaceSkeleton(MacOsViewOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        Title = options.Title;
        Width = options.Width;
        Height = options.Height;
        Dpi = options.Dpi;
    }

    public string Title { get; }

    public int Width { get; }

    public int Height { get; }

    public float Dpi { get; }

    public bool IsCreated { get; private set; }

    public bool IsCloseRequested => false;

    public (int X, int Y) CurrentMousePosition => _inputState.CurrentMousePosition;

    public uint CurrentMouseButton => _inputState.CurrentMouseButton;

    public bool IsMousePressed => _inputState.IsMousePressed;

    public uint CurrentKey => _inputState.CurrentKey;

    public bool IsKeyPressed => _inputState.IsKeyPressed;

    public void Create()
    {
        ThrowIfDisposed();
        IsCreated = true;
    }

    public void PumpEvents()
    {
        ThrowIfDisposed();
        if (!IsCreated)
        {
            throw new InvalidOperationException("MacOs surface 尚未创建。");
        }
    }

    public int ConsumeWheelDiff() => 0;

    public string? ConsumePendingText() => null;

    public uint ConsumeEditingKeyPress() => 0;

    public void Present(byte[] bgra8888Bytes, int width, int height, int stride)
    {
        ThrowIfDisposed();
    }

    public void StartTextInput()
    {
        ThrowIfDisposed();
    }

    public void StopTextInput()
    {
        ThrowIfDisposed();
    }

    public void Dispose()
    {
        _disposed = true;
        IsCreated = false;
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(MacOsSurfaceSkeleton));
        }
    }
}
