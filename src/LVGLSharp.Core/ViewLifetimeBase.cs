namespace LVGLSharp;

internal abstract class ViewLifetimeBase
{
    private bool _isOpen;
    private bool _isDisposed;

    protected void ThrowIfDisposed()
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }

    protected bool TryBeginOpen()
    {
        ThrowIfDisposed();
        if (_isOpen)
        {
            return false;
        }

        _isOpen = true;
        return true;
    }

    protected bool TryBeginClose()
    {
        if (_isDisposed)
        {
            return false;
        }

        _isDisposed = true;
        _isOpen = false;
        return true;
    }

    protected void MarkOpenFailed()
    {
        _isOpen = false;
    }

    protected bool IsDisposed => _isDisposed;
    protected bool IsOpen => _isOpen;
}
