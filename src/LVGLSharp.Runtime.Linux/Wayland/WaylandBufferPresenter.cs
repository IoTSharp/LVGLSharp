using LVGLSharp.Interop;
using System;
using System.Runtime.InteropServices;

namespace LVGLSharp.Runtime.Linux;

internal unsafe sealed class WaylandBufferPresenter : IDisposable
{
    private byte* _drawBuffer;
    private uint _drawBufferByteSize;

    public WaylandBufferPresenter(int pixelWidth, int pixelHeight, float dpi)
    {
        PixelWidth = pixelWidth;
        PixelHeight = pixelHeight;
        Dpi = dpi;
    }

    public int PixelWidth { get; }

    public int PixelHeight { get; }

    public float Dpi { get; }

    public byte* DrawBuffer => _drawBuffer;

    public uint DrawBufferByteSize => _drawBufferByteSize;

    public uint FlushCount { get; private set; }

    public int LastFlushWidth { get; private set; }

    public int LastFlushHeight { get; private set; }

    public bool HasAllocatedBuffer => _drawBuffer != null;

    public bool IsDisposed { get; private set; }

    public void Initialize()
    {
        ThrowIfDisposed();

        if (_drawBuffer != null)
        {
            return;
        }

        _drawBufferByteSize = checked((uint)(PixelWidth * PixelHeight * sizeof(ushort)));
        _drawBuffer = (byte*)NativeMemory.AllocZeroed((nuint)_drawBufferByteSize);
        if (_drawBuffer == null)
        {
            throw new OutOfMemoryException("Wayland draw buffer allocation failed.");
        }
    }

    public void Flush(lv_display_t* display, lv_area_t* area, IntPtr surfaceProxy)
    {
        ThrowIfDisposed();

        if (area != null)
        {
            LastFlushWidth = lv_area_get_width(area);
            LastFlushHeight = lv_area_get_height(area);
        }

        FlushCount++;

        if (surfaceProxy != IntPtr.Zero)
        {
            WaylandNative.CommitSurface(surfaceProxy);
        }

        lv_display_flush_ready(display);
    }

    public void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
    }

    public void Dispose()
    {
        if (_drawBuffer != null)
        {
            NativeMemory.Free(_drawBuffer);
            _drawBuffer = null;
        }

        _drawBufferByteSize = 0;
        FlushCount = 0;
        LastFlushWidth = 0;
        LastFlushHeight = 0;
        IsDisposed = true;
    }
}
