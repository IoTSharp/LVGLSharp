using LVGLSharp.Interop;
using System;
using System.Runtime.InteropServices;

namespace LVGLSharp.Runtime.Linux;

internal unsafe sealed class SdlBufferPresenter : IDisposable
{
    private readonly object _renderLock = new();
    private readonly float _dpi;

    private IntPtr _window;
    private IntPtr _renderer;
    private IntPtr _texture;
    private uint* _frameBuffer;
    private byte* _drawBuffer;
    private uint _drawBufferByteSize;

    public SdlBufferPresenter(int width, int height, float dpi)
    {
        PixelWidth = width;
        PixelHeight = height;
        _dpi = dpi;
    }

    public int PixelWidth { get; private set; }

    public int PixelHeight { get; private set; }

    public float Dpi => _dpi;

    public IntPtr Window => _window;

    public IntPtr Renderer => _renderer;

    public IntPtr Texture => _texture;

    public byte* DrawBuffer => _drawBuffer;

    public uint DrawBufferByteSize => _drawBufferByteSize;

    public bool IsReady => _renderer != IntPtr.Zero && _texture != IntPtr.Zero && _frameBuffer != null;

    public void InitializeWindow(string title, bool borderless)
    {
        if (SdlNative.SDL_Init(SdlNative.SDL_INIT_VIDEO | SdlNative.SDL_INIT_EVENTS) != 0)
        {
            throw new InvalidOperationException($"³õÊ¼»¯ SDL2 Ê§°Ü£º{SdlNative.GetSdlError()}¡£");
        }

        var windowFlags = SdlNative.SDL_WINDOW_SHOWN | SdlNative.SDL_WINDOW_ALLOW_HIGHDPI;
        if (borderless)
        {
            windowFlags |= SdlNative.SDL_WINDOW_BORDERLESS;
        }

        _window = SdlNative.SDL_CreateWindow(title, SdlNative.SDL_WINDOWPOS_CENTERED, SdlNative.SDL_WINDOWPOS_CENTERED, PixelWidth, PixelHeight, windowFlags);
        if (_window == IntPtr.Zero)
        {
            throw new InvalidOperationException($"SDL ´°¿Ú´´½¨Ê§°Ü£º{SdlNative.GetSdlError()}¡£");
        }

        _renderer = SdlNative.SDL_CreateRenderer(_window, -1, SdlNative.SDL_RENDERER_ACCELERATED | SdlNative.SDL_RENDERER_PRESENTVSYNC | SdlNative.SDL_RENDERER_TARGETTEXTURE);
        if (_renderer == IntPtr.Zero)
        {
            _renderer = SdlNative.SDL_CreateRenderer(_window, -1, SdlNative.SDL_RENDERER_ACCELERATED | SdlNative.SDL_RENDERER_TARGETTEXTURE);
        }

        if (_renderer == IntPtr.Zero)
        {
            throw new InvalidOperationException($"SDL äÖÈ¾Æ÷´´½¨Ê§°Ü£º{SdlNative.GetSdlError()}¡£");
        }

        AllocateBuffers(PixelWidth, PixelHeight);
    }

    public bool ResizeIfNeeded(int width, int height, lv_display_t* display, lv_obj_t* root)
    {
        if (width <= 0 || height <= 0)
        {
            return false;
        }

        if (width == PixelWidth && height == PixelHeight)
        {
            return false;
        }

        AllocateBuffers(width, height);
        PixelWidth = width;
        PixelHeight = height;

        if (_window != IntPtr.Zero)
        {
            SdlNative.SDL_SetWindowSize(_window, width, height);
        }

        if (display != null)
        {
            lv_display_set_resolution(display, width, height);
            lv_display_set_buffers(display, _drawBuffer, null, _drawBufferByteSize, LV_DISPLAY_RENDER_MODE_FULL);
        }

        if (root != null)
        {
            lv_obj_invalidate(root);
        }

        return true;
    }

    public void Present()
    {
        if (!IsReady)
        {
            return;
        }

        lock (_renderLock)
        {
            _ = SdlNative.SDL_UpdateTexture(_texture, IntPtr.Zero, (IntPtr)_frameBuffer, PixelWidth * sizeof(uint));
            _ = SdlNative.SDL_RenderClear(_renderer);
            _ = SdlNative.SDL_RenderCopy(_renderer, _texture, IntPtr.Zero, IntPtr.Zero);
            SdlNative.SDL_RenderPresent(_renderer);
        }
    }

    public void Flush(lv_display_t* display, lv_area_t* area, byte* pxMap)
    {
        if (_frameBuffer == null)
        {
            lv_display_flush_ready(display);
            return;
        }

        lock (_renderLock)
        {
            var width = lv_area_get_width(area);
            var height = lv_area_get_height(area);

            for (var y = 0; y < height; y++)
            {
                var dst = _frameBuffer + (area->y1 + y) * PixelWidth + area->x1;
                var src = ((ushort*)pxMap) + y * width;
                for (var x = 0; x < width; x++)
                {
                    dst[x] = ConvertRgb565ToArgb8888(src[x]);
                }
            }
        }

        lv_display_flush_ready(display);
    }

    public void Dispose()
    {
        if (_texture != IntPtr.Zero)
        {
            SdlNative.SDL_DestroyTexture(_texture);
            _texture = IntPtr.Zero;
        }

        if (_renderer != IntPtr.Zero)
        {
            SdlNative.SDL_DestroyRenderer(_renderer);
            _renderer = IntPtr.Zero;
        }

        if (_window != IntPtr.Zero)
        {
            SdlNative.SDL_DestroyWindow(_window);
            _window = IntPtr.Zero;
        }

        ReleaseBuffers();
        SdlNative.SDL_Quit();
    }

    private void AllocateBuffers(int width, int height)
    {
        var newTexture = SdlNative.SDL_CreateTexture(_renderer, SdlNative.SDL_PIXELFORMAT_ARGB8888, (int)SdlNative.SDL_TEXTUREACCESS_STREAMING, width, height);
        if (newTexture == IntPtr.Zero)
        {
            throw new InvalidOperationException($"SDL ÎÆÀí´´½¨Ê§°Ü£º{SdlNative.GetSdlError()}¡£");
        }

        var newFrameBuffer = (uint*)NativeMemory.AllocZeroed((nuint)(width * height), (nuint)sizeof(uint));
        if (newFrameBuffer == null)
        {
            SdlNative.SDL_DestroyTexture(newTexture);
            throw new OutOfMemoryException("SDL framebuffer ·ÖÅäÊ§°Ü¡£");
        }

        var newDrawBufferByteSize = checked((uint)(width * height * sizeof(ushort)));
        var newDrawBuffer = (byte*)NativeMemory.AllocZeroed((nuint)newDrawBufferByteSize);
        if (newDrawBuffer == null)
        {
            SdlNative.SDL_DestroyTexture(newTexture);
            NativeMemory.Free(newFrameBuffer);
            throw new OutOfMemoryException("SDL draw buffer ·ÖÅäÊ§°Ü¡£");
        }

        ReleaseBuffers();

        _texture = newTexture;
        _frameBuffer = newFrameBuffer;
        _drawBuffer = newDrawBuffer;
        _drawBufferByteSize = newDrawBufferByteSize;
    }

    private void ReleaseBuffers()
    {
        if (_texture != IntPtr.Zero)
        {
            SdlNative.SDL_DestroyTexture(_texture);
            _texture = IntPtr.Zero;
        }

        if (_frameBuffer != null)
        {
            NativeMemory.Free(_frameBuffer);
            _frameBuffer = null;
        }

        if (_drawBuffer != null)
        {
            NativeMemory.Free(_drawBuffer);
            _drawBuffer = null;
            _drawBufferByteSize = 0;
        }
    }

    private static uint ConvertRgb565ToArgb8888(ushort pixel)
    {
        uint r = (uint)((pixel >> 11) & 0x1F);
        uint g = (uint)((pixel >> 5) & 0x3F);
        uint b = (uint)(pixel & 0x1F);

        r = (r << 3) | (r >> 2);
        g = (g << 2) | (g >> 4);
        b = (b << 3) | (b >> 2);

        return 0xFF000000u | (r << 16) | (g << 8) | b;
    }
}