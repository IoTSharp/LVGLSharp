using LVGLSharp.Interop;

namespace LVGLSharp;

public abstract unsafe class ViewLifetimeBase : IView
{
    private bool _isOpen;
    private bool _isDisposed;

    public void Open()
    {
        if (!TryBeginOpen())
        {
            return;
        }

        try
        {
            OnOpenCore();
        }
        catch
        {
            MarkOpenFailed();
            throw;
        }
    }

    public void RunLoop(Action iteration)
    {
        try
        {
            RunLoopCore(iteration);
        }
        finally
        {
            Close();
        }
    }

    public void Close()
    {
        if (CanSkipClose())
        {
            TryBeginClose();
            return;
        }

        if (!TryBeginClose())
        {
            return;
        }

        OnCloseCore();
    }

    public void Dispose()
    {
        Close();
    }

    public abstract lv_obj_t* Root { get; }
    public abstract lv_group_t* KeyInputGroup { get; }
    public abstract delegate* unmanaged[Cdecl]<lv_event_t*, void> SendTextAreaFocusCallback { get; }
    public abstract void HandleEvents();
    public abstract void RegisterTextInput(lv_obj_t* textArea);

    protected abstract void OnOpenCore();
    protected abstract void RunLoopCore(Action iteration);
    protected abstract void OnCloseCore();

    protected virtual bool CanSkipClose() => false;

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
