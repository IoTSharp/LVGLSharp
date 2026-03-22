using LVGLSharp;
using LVGLSharp.Interop;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace LVGLSharp.Runtime.Linux;

public unsafe sealed partial class SdlView : IView
{
    private const string SdlLib = "libSDL2-2.0.so.0";

    private const uint SDL_INIT_VIDEO = 0x00000020;
    private const uint SDL_INIT_EVENTS = 0x00004000;

    private const uint SDL_WINDOW_SHOWN = 0x00000004;
    private const uint SDL_WINDOW_BORDERLESS = 0x00000010;
    private const uint SDL_WINDOW_ALLOW_HIGHDPI = 0x00002000;

    private const uint SDL_RENDERER_ACCELERATED = 0x00000002;
    private const uint SDL_RENDERER_PRESENTVSYNC = 0x00000004;
    private const uint SDL_RENDERER_TARGETTEXTURE = 0x00000008;

    private const uint SDL_TEXTUREACCESS_STREAMING = 1;
    private const uint SDL_PIXELFORMAT_ARGB8888 = 372645892;

    private const uint SDL_QUIT = 0x100;
    private const uint SDL_MOUSEMOTION = 0x400;
    private const uint SDL_MOUSEBUTTONDOWN = 0x401;
    private const uint SDL_MOUSEBUTTONUP = 0x402;
    private const uint SDL_MOUSEWHEEL = 0x403;
    private const uint SDL_KEYDOWN = 0x300;
    private const uint SDL_KEYUP = 0x301;

    private const byte SDL_BUTTON_LEFT = 1;
    private const byte SDL_BUTTON_MIDDLE = 2;
    private const byte SDL_BUTTON_RIGHT = 3;

    private const int SDL_WINDOWPOS_CENTERED = 0x2FFF0000;

    private const int SDLK_RETURN = 13;
    private const int SDLK_ESCAPE = 27;
    private const int SDLK_BACKSPACE = 8;
    private const int SDLK_TAB = 9;
    private const int SDLK_DELETE = 127;
    private const int SDLK_LEFT = 1073741904;
    private const int SDLK_RIGHT = 1073741903;
    private const int SDLK_UP = 1073741906;
    private const int SDLK_DOWN = 1073741905;
    private const int SDLK_HOME = 1073741898;
    private const int SDLK_END = 1073741901;
    private const int SDLK_PAGEUP = 1073741899;
    private const int SDLK_PAGEDOWN = 1073741902;

    private static readonly object s_renderLock = new();
    private static SdlView? s_activeView;

    private readonly string _title;
    private readonly int _width;
    private readonly int _height;
    private readonly float _dpi;
    private readonly bool _borderless;

    private IntPtr _window;
    private IntPtr _renderer;
    private IntPtr _texture;
    private uint* _frameBuffer;
    private byte* _drawBuffer;
    private uint _drawBufferByteSize;
    private lv_display_t* _lvDisplay;
    private lv_indev_t* _mouseIndev;
    private lv_indev_t* _keyboardIndev;
    private lv_indev_t* _wheelIndev;
    private bool _running;
    private bool _initialized;
    private int _mouseX;
    private int _mouseY;
    private bool _mousePressed;
    private uint _mouseButton;
    private uint _lastKey;
    private bool _keyPressed;
    private int _wheelDiff;
    private ulong _lastPresentTick;

    private lv_font_t* _fallbackFont;
    private lv_style_t* _defaultFontStyle;
    private SixLaborsFontManager? _fontManager;
    private string? _resolvedSystemFontPath;
    private string? _fontDiagnosticSummary;
    private string? _fontGlyphDiagnosticSummary;
    private static lv_obj_t* s_root;
    private static lv_group_t* s_keyInputGroup;

    [StructLayout(LayoutKind.Sequential)]
    private struct SDL_Keysym
    {
        public uint scancode;
        public int sym;
        public ushort mod;
        public uint unused;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SDL_KeyboardEvent
    {
        public uint type;
        public uint timestamp;
        public uint windowID;
        public byte state;
        public byte repeat;
        public byte padding2;
        public byte padding3;
        public SDL_Keysym keysym;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SDL_MouseMotionEvent
    {
        public uint type;
        public uint timestamp;
        public uint windowID;
        public uint which;
        public uint state;
        public int x;
        public int y;
        public int xrel;
        public int yrel;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SDL_MouseButtonEvent
    {
        public uint type;
        public uint timestamp;
        public uint windowID;
        public uint which;
        public byte button;
        public byte state;
        public byte clicks;
        public byte padding1;
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SDL_MouseWheelEvent
    {
        public uint type;
        public uint timestamp;
        public uint windowID;
        public uint which;
        public int x;
        public int y;
        public uint direction;
        public float preciseX;
        public float preciseY;
        public int mouseX;
        public int mouseY;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SDL_QuitEvent
    {
        public uint type;
        public uint timestamp;
    }

    [StructLayout(LayoutKind.Explicit, Size = 56)]
    private struct SDL_Event
    {
        [FieldOffset(0)] public uint type;
        [FieldOffset(0)] public SDL_KeyboardEvent key;
        [FieldOffset(0)] public SDL_MouseMotionEvent motion;
        [FieldOffset(0)] public SDL_MouseButtonEvent button;
        [FieldOffset(0)] public SDL_MouseWheelEvent wheel;
        [FieldOffset(0)] public SDL_QuitEvent quit;
    }

    [LibraryImport(SdlLib, EntryPoint = "SDL_Init")]
    private static partial int SDL_Init(uint flags);

    [LibraryImport(SdlLib, EntryPoint = "SDL_Quit")]
    private static partial void SDL_Quit();

    [LibraryImport(SdlLib, EntryPoint = "SDL_CreateWindow", StringMarshalling = StringMarshalling.Utf8)]
    private static partial IntPtr SDL_CreateWindow(string title, int x, int y, int w, int h, uint flags);

    [LibraryImport(SdlLib, EntryPoint = "SDL_DestroyWindow")]
    private static partial void SDL_DestroyWindow(IntPtr window);

    [LibraryImport(SdlLib, EntryPoint = "SDL_CreateRenderer")]
    private static partial IntPtr SDL_CreateRenderer(IntPtr window, int index, uint flags);

    [LibraryImport(SdlLib, EntryPoint = "SDL_DestroyRenderer")]
    private static partial void SDL_DestroyRenderer(IntPtr renderer);

    [LibraryImport(SdlLib, EntryPoint = "SDL_CreateTexture")]
    private static partial IntPtr SDL_CreateTexture(IntPtr renderer, uint format, int access, int w, int h);

    [LibraryImport(SdlLib, EntryPoint = "SDL_DestroyTexture")]
    private static partial void SDL_DestroyTexture(IntPtr texture);

    [LibraryImport(SdlLib, EntryPoint = "SDL_UpdateTexture")]
    private static partial int SDL_UpdateTexture(IntPtr texture, IntPtr rect, IntPtr pixels, int pitch);

    [LibraryImport(SdlLib, EntryPoint = "SDL_RenderClear")]
    private static partial int SDL_RenderClear(IntPtr renderer);

    [LibraryImport(SdlLib, EntryPoint = "SDL_RenderCopy")]
    private static partial int SDL_RenderCopy(IntPtr renderer, IntPtr texture, IntPtr srcRect, IntPtr dstRect);

    [LibraryImport(SdlLib, EntryPoint = "SDL_RenderPresent")]
    private static partial void SDL_RenderPresent(IntPtr renderer);

    [LibraryImport(SdlLib, EntryPoint = "SDL_PollEvent")]
    private static partial int SDL_PollEvent(out SDL_Event sdlEvent);

    [LibraryImport(SdlLib, EntryPoint = "SDL_GetError")]
    private static partial IntPtr SDL_GetError();

    public SdlView(string title = "LVGLSharp SDL", int width = 800, int height = 600, float dpi = 96f, bool borderless = false)
    {
        _title = title;
        _width = width;
        _height = height;
        _dpi = dpi;
        _borderless = borderless;
    }

    public static (int X, int Y) CurrentMousePosition => s_activeView is null ? (0, 0) : (s_activeView._mouseX, s_activeView._mouseY);

    public static uint CurrentMouseButton => s_activeView?._mouseButton ?? 0U;

    public lv_obj_t* Root => s_root;

    public lv_group_t* KeyInputGroup => s_keyInputGroup;

    public delegate* unmanaged[Cdecl]<lv_event_t*, void> SendTextAreaFocusCallback => null;

    public void Init()
    {
        if (_initialized)
        {
            return;
        }

        LvglNativeLibraryResolver.EnsureRegistered();

        if (!lv_is_initialized())
        {
            lv_init();
        }

        try
        {
            InitializeSdl();
            InitializeLvgl();

            s_root = lv_scr_act();
            s_keyInputGroup = lv_group_create();
            lv_indev_set_group(_keyboardIndev, s_keyInputGroup);
            _fallbackFont = lv_obj_get_style_text_font(s_root, LV_PART_MAIN);
            _fontDiagnosticSummary = LinuxSystemFontResolver.GetFontPathDiagnosticSummary();
            _fontGlyphDiagnosticSummary = LinuxSystemFontResolver.GetGlyphDiagnosticSummary();

            _resolvedSystemFontPath = LinuxSystemFontResolver.TryResolveFontPath();
            if (!string.IsNullOrWhiteSpace(_resolvedSystemFontPath))
            {
                _fontManager = new SixLaborsFontManager(
                    _resolvedSystemFontPath,
                    12,
                    _dpi,
                    _fallbackFont,
                    LvglHostDefaults.CreateDefaultFontFallbackGlyphs());

                _defaultFontStyle = LvglHostDefaults.ApplyDefaultFontStyle(s_root, _fontManager.GetLvFontPtr());
            }

            s_activeView = this;
            _running = true;
            _lastPresentTick = (ulong)Environment.TickCount64;
            _initialized = true;
        }
        catch
        {
            Stop();
            throw;
        }
    }

    public void ProcessEvents()
    {
        if (!_initialized)
        {
            return;
        }

        PollEvents();
        PresentFrame();
    }

    public void StartLoop(Action handle)
    {
        try
        {
            while (_running)
            {
                ProcessEvents();
                handle?.Invoke();
            }
        }
        finally
        {
            Stop();
        }
    }

    public void Stop()
    {
        if (s_activeView == this)
        {
            s_activeView = null;
        }

        _running = false;

        if (_mouseIndev != null)
        {
            lv_indev_delete(_mouseIndev);
            _mouseIndev = null;
        }

        if (_keyboardIndev != null)
        {
            lv_indev_delete(_keyboardIndev);
            _keyboardIndev = null;
        }

        if (_wheelIndev != null)
        {
            lv_indev_delete(_wheelIndev);
            _wheelIndev = null;
        }

        if (s_keyInputGroup != null)
        {
            lv_group_delete(s_keyInputGroup);
            s_keyInputGroup = null;
        }

        if (_lvDisplay != null)
        {
            lv_display_delete(_lvDisplay);
            _lvDisplay = null;
        }

        if (_texture != IntPtr.Zero)
        {
            SDL_DestroyTexture(_texture);
            _texture = IntPtr.Zero;
        }

        if (_renderer != IntPtr.Zero)
        {
            SDL_DestroyRenderer(_renderer);
            _renderer = IntPtr.Zero;
        }

        if (_window != IntPtr.Zero)
        {
            SDL_DestroyWindow(_window);
            _window = IntPtr.Zero;
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

        _fontManager?.Dispose();
        _fontManager = null;
        _resolvedSystemFontPath = null;
        _fontDiagnosticSummary = null;
        _fontGlyphDiagnosticSummary = null;

        s_root = null;
        _lastKey = 0;
        _keyPressed = false;
        _wheelDiff = 0;

        if (_initialized)
        {
            SDL_Quit();
        }

        _initialized = false;
    }

    public void AttachTextInput(lv_obj_t* textArea)
    {
        // SDL 第一版先只走键盘输入路径。
    }

    public override string ToString()
    {
        return $"Title={_title}, Window={_window != IntPtr.Zero}:{_width}x{_height}, Renderer={_renderer != IntPtr.Zero}, Texture={_texture != IntPtr.Zero}, Running={_running}, Initialized={_initialized}, LvDisplay={_lvDisplay != null}, Root={s_root != null}, KeyGroup={s_keyInputGroup != null}, FontPath={_resolvedSystemFontPath ?? "<none>"}, FontDiag={_fontDiagnosticSummary ?? "<unresolved>"}, GlyphDiag={_fontGlyphDiagnosticSummary ?? "<unresolved>"}";
    }

    private void InitializeSdl()
    {
        if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_EVENTS) != 0)
        {
            throw new InvalidOperationException($"初始化 SDL2 失败：{GetSdlError()}。");
        }

        var windowFlags = SDL_WINDOW_SHOWN | SDL_WINDOW_ALLOW_HIGHDPI;
        if (_borderless)
        {
            windowFlags |= SDL_WINDOW_BORDERLESS;
        }

        _window = SDL_CreateWindow(_title, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, _width, _height, windowFlags);
        if (_window == IntPtr.Zero)
        {
            throw new InvalidOperationException($"SDL 窗口创建失败：{GetSdlError()}。");
        }

        _renderer = SDL_CreateRenderer(_window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC | SDL_RENDERER_TARGETTEXTURE);
        if (_renderer == IntPtr.Zero)
        {
            _renderer = SDL_CreateRenderer(_window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_TARGETTEXTURE);
        }

        if (_renderer == IntPtr.Zero)
        {
            throw new InvalidOperationException($"SDL 渲染器创建失败：{GetSdlError()}。");
        }

        _texture = SDL_CreateTexture(_renderer, SDL_PIXELFORMAT_ARGB8888, (int)SDL_TEXTUREACCESS_STREAMING, _width, _height);
        if (_texture == IntPtr.Zero)
        {
            throw new InvalidOperationException($"SDL 纹理创建失败：{GetSdlError()}。");
        }

        _frameBuffer = (uint*)NativeMemory.AllocZeroed((nuint)(_width * _height), (nuint)sizeof(uint));
        if (_frameBuffer == null)
        {
            throw new OutOfMemoryException("SDL framebuffer 分配失败。");
        }

        _drawBufferByteSize = checked((uint)(_width * _height * sizeof(ushort)));
        _drawBuffer = (byte*)NativeMemory.AllocZeroed((nuint)_drawBufferByteSize);
        if (_drawBuffer == null)
        {
            throw new OutOfMemoryException("SDL draw buffer 分配失败。");
        }
    }

    private void InitializeLvgl()
    {
        _lvDisplay = lv_display_create(_width, _height);
        if (_lvDisplay == null)
        {
            throw new InvalidOperationException("LVGL display 创建失败。");
        }

        lv_display_set_buffers(_lvDisplay, _drawBuffer, null, _drawBufferByteSize, LV_DISPLAY_RENDER_MODE_FULL);
        lv_display_set_flush_cb(_lvDisplay, &FlushCb);

        _mouseIndev = lv_indev_create();
        lv_indev_set_type(_mouseIndev, LV_INDEV_TYPE_POINTER);
        lv_indev_set_read_cb(_mouseIndev, &MouseReadCb);
        lv_indev_set_display(_mouseIndev, _lvDisplay);

        _keyboardIndev = lv_indev_create();
        lv_indev_set_type(_keyboardIndev, LV_INDEV_TYPE_KEYPAD);
        lv_indev_set_read_cb(_keyboardIndev, &KeyboardReadCb);
        lv_indev_set_display(_keyboardIndev, _lvDisplay);

        _wheelIndev = lv_indev_create();
        lv_indev_set_type(_wheelIndev, LV_INDEV_TYPE_ENCODER);
        lv_indev_set_read_cb(_wheelIndev, &WheelReadCb);
        lv_indev_set_display(_wheelIndev, _lvDisplay);
    }

    private void PollEvents()
    {
        while (SDL_PollEvent(out var sdlEvent) != 0)
        {
            switch (sdlEvent.type)
            {
                case SDL_QUIT:
                    _running = false;
                    break;
                case SDL_MOUSEMOTION:
                    _mouseX = sdlEvent.motion.x;
                    _mouseY = sdlEvent.motion.y;
                    break;
                case SDL_MOUSEBUTTONDOWN:
                    _mousePressed = true;
                    _mouseButton = MapMouseButton(sdlEvent.button.button);
                    _mouseX = sdlEvent.button.x;
                    _mouseY = sdlEvent.button.y;
                    break;
                case SDL_MOUSEBUTTONUP:
                    _mousePressed = false;
                    _mouseButton = 0;
                    _mouseX = sdlEvent.button.x;
                    _mouseY = sdlEvent.button.y;
                    break;
                case SDL_MOUSEWHEEL:
                    _wheelDiff += sdlEvent.wheel.y;
                    break;
                case SDL_KEYDOWN:
                    _lastKey = MapSdlKey(sdlEvent.key.keysym.sym);
                    _keyPressed = _lastKey != 0;
                    break;
                case SDL_KEYUP:
                    _keyPressed = false;
                    break;
            }
        }
    }

    private void PresentFrame()
    {
        var now = (ulong)Environment.TickCount64;
        var diff = _lastPresentTick == 0 ? 0U : (uint)(now - _lastPresentTick);
        _lastPresentTick = now;

        lv_tick_inc(diff);
        lv_timer_handler();

        if (_renderer == IntPtr.Zero || _texture == IntPtr.Zero || _frameBuffer == null)
        {
            Thread.Sleep(5);
            return;
        }

        lock (s_renderLock)
        {
            _ = SDL_UpdateTexture(_texture, IntPtr.Zero, (IntPtr)_frameBuffer, _width * sizeof(uint));
            _ = SDL_RenderClear(_renderer);
            _ = SDL_RenderCopy(_renderer, _texture, IntPtr.Zero, IntPtr.Zero);
            SDL_RenderPresent(_renderer);
        }

        Thread.Sleep(5);
    }

    private static uint MapMouseButton(byte button)
    {
        return button switch
        {
            SDL_BUTTON_LEFT => 1U,
            SDL_BUTTON_RIGHT => 2U,
            SDL_BUTTON_MIDDLE => 4U,
            _ => 0U,
        };
    }

    private static uint MapSdlKey(int key)
    {
        return key switch
        {
            SDLK_RETURN => (uint)LV_KEY_ENTER,
            SDLK_ESCAPE => (uint)LV_KEY_ESC,
            SDLK_BACKSPACE => (uint)LV_KEY_BACKSPACE,
            SDLK_TAB => (uint)LV_KEY_NEXT,
            SDLK_HOME => (uint)LV_KEY_HOME,
            SDLK_END => (uint)LV_KEY_END,
            SDLK_DELETE => (uint)LV_KEY_DEL,
            SDLK_LEFT => (uint)LV_KEY_LEFT,
            SDLK_RIGHT => (uint)LV_KEY_RIGHT,
            SDLK_UP => (uint)LV_KEY_UP,
            SDLK_DOWN => (uint)LV_KEY_DOWN,
            SDLK_PAGEUP => (uint)LV_KEY_PREV,
            SDLK_PAGEDOWN => (uint)LV_KEY_NEXT,
            >= 32 and <= 126 => (uint)key,
            _ => 0,
        };
    }

    private static string GetSdlError()
    {
        var errorPtr = SDL_GetError();
        return errorPtr == IntPtr.Zero ? "未知错误" : Marshal.PtrToStringUTF8(errorPtr) ?? "未知错误";
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

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void FlushCb(lv_display_t* display, lv_area_t* area, byte* pxMap)
    {
        var view = s_activeView;
        if (view is null || view._frameBuffer == null)
        {
            lv_display_flush_ready(display);
            return;
        }

        lock (s_renderLock)
        {
            var width = lv_area_get_width(area);
            var height = lv_area_get_height(area);

            for (var y = 0; y < height; y++)
            {
                var dst = view._frameBuffer + (area->y1 + y) * view._width + area->x1;
                var src = ((ushort*)pxMap) + y * width;
                for (var x = 0; x < width; x++)
                {
                    dst[x] = ConvertRgb565ToArgb8888(src[x]);
                }
            }
        }

        lv_display_flush_ready(display);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void MouseReadCb(lv_indev_t* indev, lv_indev_data_t* data)
    {
        var view = s_activeView;
        if (view is null)
        {
            data->point.x = 0;
            data->point.y = 0;
            data->state = LV_INDEV_STATE_REL;
            data->enc_diff = 0;
            return;
        }

        data->point.x = view._mouseX;
        data->point.y = view._mouseY;
        data->state = view._mousePressed ? LV_INDEV_STATE_PR : LV_INDEV_STATE_REL;
        data->enc_diff = 0;
        data->btn_id = view._mouseButton;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void KeyboardReadCb(lv_indev_t* indev, lv_indev_data_t* data)
    {
        var view = s_activeView;
        if (view is null)
        {
            data->key = 0;
            data->state = LV_INDEV_STATE_REL;
            return;
        }

        data->key = view._lastKey;
        data->state = view._keyPressed ? LV_INDEV_STATE_PR : LV_INDEV_STATE_REL;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void WheelReadCb(lv_indev_t* indev, lv_indev_data_t* data)
    {
        var view = s_activeView;
        if (view is null)
        {
            data->enc_diff = 0;
            data->state = LV_INDEV_STATE_REL;
            return;
        }

        data->enc_diff = (short)view._wheelDiff;
        view._wheelDiff = 0;
        data->state = LV_INDEV_STATE_REL;
    }
}