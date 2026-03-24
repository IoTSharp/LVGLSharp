namespace LVGLSharp.Runtime.MacOs;

public interface IMacOsSurface : IDisposable
{
    string Title { get; }

    int Width { get; }

    int Height { get; }

    float Dpi { get; }

    bool IsCreated { get; }

    bool IsCloseRequested { get; }

    (int X, int Y) CurrentMousePosition { get; }

    uint CurrentMouseButton { get; }

    bool IsMousePressed { get; }

    uint CurrentKey { get; }

    bool IsKeyPressed { get; }

    void Create();

    void PumpEvents();

    int ConsumeWheelDiff();

    string? ConsumePendingText();

    uint ConsumeEditingKeyPress();

    void Present(byte[] bgra8888Bytes, int width, int height, int stride);

    void StartTextInput();

    void StopTextInput();
}
