namespace LVGLSharp.Runtime.MacOs;

internal sealed class MacOsInputState
{
    private string? _pendingText;
    private uint _lastDispatchedEditingKey;

    public (int X, int Y) CurrentMousePosition { get; private set; }

    public uint CurrentMouseButton { get; private set; }

    public bool IsMousePressed { get; private set; }

    public uint CurrentKey { get; private set; }

    public bool IsKeyPressed { get; private set; }

    public int WheelDiff { get; private set; }

    public string? ConsumePendingText()
    {
        var value = _pendingText;
        _pendingText = null;
        return value;
    }

    public uint ConsumeEditingKeyPress()
    {
        if (!IsKeyPressed)
        {
            _lastDispatchedEditingKey = 0;
            return 0;
        }

        if (CurrentKey == _lastDispatchedEditingKey)
        {
            return 0;
        }

        _lastDispatchedEditingKey = CurrentKey;
        return CurrentKey;
    }

    public int ConsumeWheelDiff()
    {
        var value = WheelDiff;
        WheelDiff = 0;
        return value;
    }

    public void UpdateMousePosition(int x, int y)
    {
        CurrentMousePosition = (x, y);
    }

    public void SetMouseButton(uint button, bool pressed, int x, int y)
    {
        CurrentMousePosition = (x, y);
        CurrentMouseButton = pressed ? button : 0;
        IsMousePressed = pressed;
    }

    public void AddWheelDiff(int diff)
    {
        WheelDiff += diff;
    }

    public void SetKey(uint key, bool pressed, string? pendingText = null)
    {
        CurrentKey = key;
        IsKeyPressed = pressed && key != 0;

        if (!string.IsNullOrEmpty(pendingText))
        {
            _pendingText = pendingText;
        }

        if (!pressed)
        {
            _lastDispatchedEditingKey = 0;
        }
    }

    public void Reset()
    {
        CurrentMousePosition = (0, 0);
        CurrentMouseButton = 0;
        IsMousePressed = false;
        CurrentKey = 0;
        IsKeyPressed = false;
        WheelDiff = 0;
        _pendingText = null;
        _lastDispatchedEditingKey = 0;
    }
}
