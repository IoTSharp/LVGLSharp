using LVGLSharp.Interop;
using System.Runtime.InteropServices;

namespace LVGLSharp.Runtime.MacOs;

internal sealed class MacOsNativeSurface : IMacOsSurface
{
    private readonly MacOsInputState _inputState = new();
    private readonly bool _borderless;

    private IntPtr _autoreleasePool;
    private IntPtr _app;
    private IntPtr _window;
    private IntPtr _contentView;
    private IntPtr _layer;
    private IntPtr _runLoopMode;
    private IntPtr _colorSpace;
    private bool _disposed;
    private bool _textInputEnabled;

    private static readonly IntPtr s_nsAutoreleasePoolClass = MacOsNative.GetClass("NSAutoreleasePool");
    private static readonly IntPtr s_nsApplicationClass = MacOsNative.GetClass("NSApplication");
    private static readonly IntPtr s_nsWindowClass = MacOsNative.GetClass("NSWindow");
    private static readonly IntPtr s_nsViewClass = MacOsNative.GetClass("NSView");
    private static readonly IntPtr s_nsDateClass = MacOsNative.GetClass("NSDate");
    private static readonly IntPtr s_nsStringClass = MacOsNative.GetClass("NSString");

    private static readonly IntPtr s_selAlloc = MacOsNative.GetSelector("alloc");
    private static readonly IntPtr s_selInit = MacOsNative.GetSelector("init");
    private static readonly IntPtr s_selDrain = MacOsNative.GetSelector("drain");
    private static readonly IntPtr s_selRelease = MacOsNative.GetSelector("release");
    private static readonly IntPtr s_selSharedApplication = MacOsNative.GetSelector("sharedApplication");
    private static readonly IntPtr s_selSetActivationPolicy = MacOsNative.GetSelector("setActivationPolicy:");
    private static readonly IntPtr s_selFinishLaunching = MacOsNative.GetSelector("finishLaunching");
    private static readonly IntPtr s_selActivateIgnoringOtherApps = MacOsNative.GetSelector("activateIgnoringOtherApps:");
    private static readonly IntPtr s_selInitWithContentRect = MacOsNative.GetSelector("initWithContentRect:styleMask:backing:defer:");
    private static readonly IntPtr s_selInitWithFrame = MacOsNative.GetSelector("initWithFrame:");
    private static readonly IntPtr s_selSetTitle = MacOsNative.GetSelector("setTitle:");
    private static readonly IntPtr s_selCenter = MacOsNative.GetSelector("center");
    private static readonly IntPtr s_selSetContentView = MacOsNative.GetSelector("setContentView:");
    private static readonly IntPtr s_selMakeKeyAndOrderFront = MacOsNative.GetSelector("makeKeyAndOrderFront:");
    private static readonly IntPtr s_selSetAcceptsMouseMovedEvents = MacOsNative.GetSelector("setAcceptsMouseMovedEvents:");
    private static readonly IntPtr s_selSetReleasedWhenClosed = MacOsNative.GetSelector("setReleasedWhenClosed:");
    private static readonly IntPtr s_selClose = MacOsNative.GetSelector("close");
    private static readonly IntPtr s_selIsVisible = MacOsNative.GetSelector("isVisible");
    private static readonly IntPtr s_selBackingScaleFactor = MacOsNative.GetSelector("backingScaleFactor");
    private static readonly IntPtr s_selSetAutoresizingMask = MacOsNative.GetSelector("setAutoresizingMask:");
    private static readonly IntPtr s_selSetWantsLayer = MacOsNative.GetSelector("setWantsLayer:");
    private static readonly IntPtr s_selLayer = MacOsNative.GetSelector("layer");
    private static readonly IntPtr s_selSetContents = MacOsNative.GetSelector("setContents:");
    private static readonly IntPtr s_selSetContentsScale = MacOsNative.GetSelector("setContentsScale:");
    private static readonly IntPtr s_selBounds = MacOsNative.GetSelector("bounds");
    private static readonly IntPtr s_selNextEventMatchingMask = MacOsNative.GetSelector("nextEventMatchingMask:untilDate:inMode:dequeue:");
    private static readonly IntPtr s_selSendEvent = MacOsNative.GetSelector("sendEvent:");
    private static readonly IntPtr s_selUpdateWindows = MacOsNative.GetSelector("updateWindows");
    private static readonly IntPtr s_selDistantPast = MacOsNative.GetSelector("distantPast");
    private static readonly IntPtr s_selInitWithUTF8String = MacOsNative.GetSelector("initWithUTF8String:");
    private static readonly IntPtr s_selType = MacOsNative.GetSelector("type");
    private static readonly IntPtr s_selLocationInWindow = MacOsNative.GetSelector("locationInWindow");
    private static readonly IntPtr s_selScrollingDeltaY = MacOsNative.GetSelector("scrollingDeltaY");
    private static readonly IntPtr s_selKeyCode = MacOsNative.GetSelector("keyCode");
    private static readonly IntPtr s_selCharacters = MacOsNative.GetSelector("characters");
    private static readonly IntPtr s_selUTF8String = MacOsNative.GetSelector("UTF8String");

    public MacOsNativeSurface(MacOsViewOptions options, bool borderless = false)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        Title = options.Title;
        Width = options.Width;
        Height = options.Height;
        Dpi = options.Dpi;
        _borderless = borderless;
    }

    public string Title { get; }

    public int Width { get; private set; }

    public int Height { get; private set; }

    public float Dpi { get; private set; }

    public bool IsCreated => _window != IntPtr.Zero;

    public bool IsCloseRequested { get; private set; }

    public (int X, int Y) CurrentMousePosition => _inputState.CurrentMousePosition;

    public uint CurrentMouseButton => _inputState.CurrentMouseButton;

    public bool IsMousePressed => _inputState.IsMousePressed;

    public uint CurrentKey => _inputState.CurrentKey;

    public bool IsKeyPressed => _inputState.IsKeyPressed;

    public void Create()
    {
        ThrowIfDisposed();

        if (!OperatingSystem.IsMacOS())
        {
            throw new PlatformNotSupportedException("MacOsView 仅支持在 macOS 上运行。");
        }

        if (IsCreated)
        {
            return;
        }

        _autoreleasePool = CreateAutoreleasePool();
        _colorSpace = MacOsNative.CGColorSpaceCreateDeviceRGB();
        _runLoopMode = CreateNSString("kCFRunLoopDefaultMode");

        _app = MacOsNative.IntPtr_objc_msgSend(s_nsApplicationClass, s_selSharedApplication);
        MacOsNative.void_nuint_objc_msgSend(_app, s_selSetActivationPolicy, MacOsNative.NSApplicationActivationPolicyRegular);
        MacOsNative.void_objc_msgSend(_app, s_selFinishLaunching);

        var frame = new MacOsNative.CGRect(0, 0, Width, Height);
        var styleMask = _borderless
            ? 0u
            : MacOsNative.NSWindowStyleMaskTitled |
              MacOsNative.NSWindowStyleMaskClosable |
              MacOsNative.NSWindowStyleMaskMiniaturizable |
              MacOsNative.NSWindowStyleMaskResizable;

        _window = MacOsNative.IntPtr_CGRect_nuint_nuint_bool_objc_msgSend(
            MacOsNative.IntPtr_objc_msgSend(s_nsWindowClass, s_selAlloc),
            s_selInitWithContentRect,
            frame,
            styleMask,
            MacOsNative.NSBackingStoreBuffered,
            false);

        _contentView = MacOsNative.IntPtr_CGRect_objc_msgSend(
            MacOsNative.IntPtr_objc_msgSend(s_nsViewClass, s_selAlloc),
            s_selInitWithFrame,
            frame);

        MacOsNative.void_nuint_objc_msgSend(
            _contentView,
            s_selSetAutoresizingMask,
            MacOsNative.NSViewWidthSizable | MacOsNative.NSViewHeightSizable);
        MacOsNative.void_bool_objc_msgSend(_contentView, s_selSetWantsLayer, true);
        _layer = MacOsNative.IntPtr_objc_msgSend(_contentView, s_selLayer);

        using var titleString = CreateTemporaryNSString(Title);
        MacOsNative.void_IntPtr_objc_msgSend(_window, s_selSetTitle, titleString.Handle);
        MacOsNative.void_IntPtr_objc_msgSend(_window, s_selSetContentView, _contentView);
        MacOsNative.void_bool_objc_msgSend(_window, s_selSetAcceptsMouseMovedEvents, true);
        MacOsNative.void_bool_objc_msgSend(_window, s_selSetReleasedWhenClosed, false);
        MacOsNative.void_objc_msgSend(_window, s_selCenter);
        MacOsNative.void_IntPtr_objc_msgSend(_window, s_selMakeKeyAndOrderFront, IntPtr.Zero);
        MacOsNative.void_bool_objc_msgSend(_app, s_selActivateIgnoringOtherApps, true);

        UpdateMetrics();
        _inputState.Reset();
        IsCloseRequested = false;
    }

    public void PumpEvents()
    {
        ThrowIfDisposed();

        if (!IsCreated)
        {
            throw new InvalidOperationException("MacOs surface 尚未创建。");
        }

        using var pool = CreateTemporaryAutoreleasePool();
        var distantPast = MacOsNative.IntPtr_objc_msgSend(s_nsDateClass, s_selDistantPast);
        while (true)
        {
            var evt = MacOsNative.IntPtr_nuint_IntPtr_IntPtr_bool_objc_msgSend(
                _app,
                s_selNextEventMatchingMask,
                MacOsNative.NSEventMaskAny,
                distantPast,
                _runLoopMode,
                true);

            if (evt == IntPtr.Zero)
            {
                break;
            }

            HandleEvent(evt);

            var eventType = MacOsNative.nuint_objc_msgSend(evt, s_selType);
            if (eventType != MacOsNative.NSEventTypeKeyDown &&
                eventType != MacOsNative.NSEventTypeKeyUp &&
                eventType != MacOsNative.NSEventTypeFlagsChanged)
            {
                MacOsNative.void_IntPtr_objc_msgSend(_app, s_selSendEvent, evt);
            }
        }

        MacOsNative.void_objc_msgSend(_app, s_selUpdateWindows);
        UpdateMetrics();

        if (!MacOsNative.bool_objc_msgSend(_window, s_selIsVisible))
        {
            IsCloseRequested = true;
        }
    }

    public int ConsumeWheelDiff() => _inputState.ConsumeWheelDiff();

    public string? ConsumePendingText() => _inputState.ConsumePendingText();

    public uint ConsumeEditingKeyPress() => _inputState.ConsumeEditingKeyPress();

    public void Present(byte[] bgra8888Bytes, int width, int height, int stride)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(bgra8888Bytes);

        if (!IsCreated || _layer == IntPtr.Zero || width <= 0 || height <= 0)
        {
            return;
        }

        unsafe
        {
            fixed (byte* pixels = bgra8888Bytes)
            {
                var provider = MacOsNative.CGDataProviderCreateWithData(IntPtr.Zero, (IntPtr)pixels, (nuint)bgra8888Bytes.Length, IntPtr.Zero);
                if (provider == IntPtr.Zero)
                {
                    return;
                }

                try
                {
                    var image = MacOsNative.CGImageCreate(
                        (nuint)width,
                        (nuint)height,
                        8,
                        32,
                        (nuint)stride,
                        _colorSpace,
                        MacOsNative.kCGImageAlphaPremultipliedFirst | MacOsNative.kCGBitmapByteOrder32Little,
                        provider,
                        IntPtr.Zero,
                        false,
                        MacOsNative.kCGRenderingIntentDefault);

                    if (image == IntPtr.Zero)
                    {
                        return;
                    }

                    try
                    {
                        MacOsNative.void_double_objc_msgSend(_layer, s_selSetContentsScale, Math.Max(1.0, Dpi / 96.0));
                        MacOsNative.void_IntPtr_objc_msgSend(_layer, s_selSetContents, image);
                    }
                    finally
                    {
                        MacOsNative.CGImageRelease(image);
                    }
                }
                finally
                {
                    MacOsNative.CGDataProviderRelease(provider);
                }
            }
        }
    }

    public void StartTextInput()
    {
        _textInputEnabled = true;
    }

    public void StopTextInput()
    {
        _textInputEnabled = false;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        IsCloseRequested = true;

        if (_window != IntPtr.Zero)
        {
            MacOsNative.void_objc_msgSend(_window, s_selClose);
            MacOsNative.void_objc_msgSend(_window, s_selRelease);
            _window = IntPtr.Zero;
        }

        if (_contentView != IntPtr.Zero)
        {
            MacOsNative.void_objc_msgSend(_contentView, s_selRelease);
            _contentView = IntPtr.Zero;
        }

        if (_runLoopMode != IntPtr.Zero)
        {
            MacOsNative.void_objc_msgSend(_runLoopMode, s_selRelease);
            _runLoopMode = IntPtr.Zero;
        }

        if (_colorSpace != IntPtr.Zero)
        {
            MacOsNative.CGColorSpaceRelease(_colorSpace);
            _colorSpace = IntPtr.Zero;
        }

        if (_autoreleasePool != IntPtr.Zero)
        {
            MacOsNative.void_objc_msgSend(_autoreleasePool, s_selDrain);
            _autoreleasePool = IntPtr.Zero;
        }

        _layer = IntPtr.Zero;
        _app = IntPtr.Zero;
        _inputState.Reset();
    }

    private void HandleEvent(IntPtr evt)
    {
        var eventType = MacOsNative.nuint_objc_msgSend(evt, s_selType);
        switch (eventType)
        {
            case MacOsNative.NSEventTypeMouseMoved:
            case MacOsNative.NSEventTypeLeftMouseDragged:
            case MacOsNative.NSEventTypeRightMouseDragged:
            case MacOsNative.NSEventTypeOtherMouseDragged:
                UpdateMousePosition(evt);
                break;
            case MacOsNative.NSEventTypeLeftMouseDown:
                UpdateMouseButton(evt, 1U, true);
                break;
            case MacOsNative.NSEventTypeLeftMouseUp:
                UpdateMouseButton(evt, 1U, false);
                break;
            case MacOsNative.NSEventTypeRightMouseDown:
                UpdateMouseButton(evt, 2U, true);
                break;
            case MacOsNative.NSEventTypeRightMouseUp:
                UpdateMouseButton(evt, 2U, false);
                break;
            case MacOsNative.NSEventTypeOtherMouseDown:
                UpdateMouseButton(evt, 4U, true);
                break;
            case MacOsNative.NSEventTypeOtherMouseUp:
                UpdateMouseButton(evt, 4U, false);
                break;
            case MacOsNative.NSEventTypeScrollWheel:
                _inputState.AddWheelDiff((int)Math.Round(MacOsNative.double_objc_msgSend(evt, s_selScrollingDeltaY)));
                break;
            case MacOsNative.NSEventTypeKeyDown:
                HandleKeyDown(evt);
                break;
            case MacOsNative.NSEventTypeKeyUp:
                _inputState.SetKey(_inputState.CurrentKey, false);
                break;
            case MacOsNative.NSEventTypeFlagsChanged:
                _inputState.SetKey(0, false);
                break;
        }
    }

    private void UpdateMousePosition(IntPtr evt)
    {
        var point = ConvertPointToPixels(MacOsNative.CGPoint_objc_msgSend(evt, s_selLocationInWindow));
        _inputState.UpdateMousePosition(point.X, point.Y);
    }

    private void UpdateMouseButton(IntPtr evt, uint button, bool pressed)
    {
        var point = ConvertPointToPixels(MacOsNative.CGPoint_objc_msgSend(evt, s_selLocationInWindow));
        _inputState.SetMouseButton(button, pressed, point.X, point.Y);
    }

    private void HandleKeyDown(IntPtr evt)
    {
        var keyCode = MacOsNative.nuint_objc_msgSend(evt, s_selKeyCode);
        var mappedKey = MapVirtualKey(keyCode);
        var pendingText = _textInputEnabled ? ReadCharacters(evt) : null;

        if (mappedKey == 0 && !string.IsNullOrEmpty(pendingText))
        {
            mappedKey = pendingText[0];
        }

        _inputState.SetKey(mappedKey, true, SanitizePendingText(mappedKey, pendingText));
    }

    private void UpdateMetrics()
    {
        if (_contentView == IntPtr.Zero)
        {
            return;
        }

        var bounds = MacOsNative.CGRect_objc_msgSend(_contentView, s_selBounds);
        var scale = _window != IntPtr.Zero
            ? Math.Max(1.0, MacOsNative.double_objc_msgSend(_window, s_selBackingScaleFactor))
            : 1.0;

        Width = Math.Max(1, (int)Math.Round(bounds.Size.Width * scale));
        Height = Math.Max(1, (int)Math.Round(bounds.Size.Height * scale));
        Dpi = (float)(96.0 * scale);
    }

    private (int X, int Y) ConvertPointToPixels(MacOsNative.CGPoint point)
    {
        var bounds = MacOsNative.CGRect_objc_msgSend(_contentView, s_selBounds);
        var scale = Math.Max(1.0, _window != IntPtr.Zero ? MacOsNative.double_objc_msgSend(_window, s_selBackingScaleFactor) : 1.0);
        var x = (int)Math.Round(point.X * scale);
        var y = (int)Math.Round((bounds.Size.Height - point.Y) * scale);
        return (Math.Max(0, x), Math.Max(0, y));
    }

    private static uint MapVirtualKey(nuint keyCode)
    {
        return keyCode switch
        {
            MacOsNative.MacVirtualKeyReturn => (uint)lv_key_t.LV_KEY_ENTER,
            MacOsNative.MacVirtualKeyEscape => (uint)lv_key_t.LV_KEY_ESC,
            MacOsNative.MacVirtualKeyDelete => (uint)lv_key_t.LV_KEY_BACKSPACE,
            MacOsNative.MacVirtualKeyForwardDelete => (uint)lv_key_t.LV_KEY_DEL,
            MacOsNative.MacVirtualKeyTab => (uint)lv_key_t.LV_KEY_NEXT,
            MacOsNative.MacVirtualKeyLeftArrow => (uint)lv_key_t.LV_KEY_LEFT,
            MacOsNative.MacVirtualKeyRightArrow => (uint)lv_key_t.LV_KEY_RIGHT,
            MacOsNative.MacVirtualKeyUpArrow => (uint)lv_key_t.LV_KEY_UP,
            MacOsNative.MacVirtualKeyDownArrow => (uint)lv_key_t.LV_KEY_DOWN,
            MacOsNative.MacVirtualKeyHome => (uint)lv_key_t.LV_KEY_HOME,
            MacOsNative.MacVirtualKeyEnd => (uint)lv_key_t.LV_KEY_END,
            MacOsNative.MacVirtualKeyPageUp => (uint)lv_key_t.LV_KEY_PREV,
            MacOsNative.MacVirtualKeyPageDown => (uint)lv_key_t.LV_KEY_NEXT,
            _ => 0,
        };
    }

    private static string? SanitizePendingText(uint mappedKey, string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        return mappedKey switch
        {
            (uint)lv_key_t.LV_KEY_ENTER => null,
            (uint)lv_key_t.LV_KEY_ESC => null,
            (uint)lv_key_t.LV_KEY_BACKSPACE => null,
            (uint)lv_key_t.LV_KEY_DEL => null,
            (uint)lv_key_t.LV_KEY_LEFT => null,
            (uint)lv_key_t.LV_KEY_RIGHT => null,
            (uint)lv_key_t.LV_KEY_UP => null,
            (uint)lv_key_t.LV_KEY_DOWN => null,
            _ => text,
        };
    }

    private static string? ReadCharacters(IntPtr evt)
    {
        var nsString = MacOsNative.IntPtr_objc_msgSend(evt, s_selCharacters);
        if (nsString == IntPtr.Zero)
        {
            return null;
        }

        var utf8 = MacOsNative.IntPtr_objc_msgSend(nsString, s_selUTF8String);
        if (utf8 == IntPtr.Zero)
        {
            return null;
        }

        var text = Marshal.PtrToStringUTF8(utf8);
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        return text[0] < ' ' ? null : text;
    }

    private IntPtr CreateAutoreleasePool()
    {
        var pool = MacOsNative.IntPtr_objc_msgSend(s_nsAutoreleasePoolClass, s_selAlloc);
        return MacOsNative.IntPtr_objc_msgSend(pool, s_selInit);
    }

    private TemporaryAutoreleasePool CreateTemporaryAutoreleasePool()
    {
        return new TemporaryAutoreleasePool(CreateAutoreleasePool());
    }

    private IntPtr CreateNSString(string value)
    {
        var nsString = MacOsNative.IntPtr_objc_msgSend(s_nsStringClass, s_selAlloc);
        return MacOsNative.IntPtr_string_objc_msgSend(nsString, s_selInitWithUTF8String, value);
    }

    private TemporaryNSString CreateTemporaryNSString(string value)
    {
        return new TemporaryNSString(CreateNSString(value));
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(MacOsNativeSurface));
        }
    }

    private readonly struct TemporaryAutoreleasePool : IDisposable
    {
        public TemporaryAutoreleasePool(IntPtr handle)
        {
            Handle = handle;
        }

        public IntPtr Handle { get; }

        public void Dispose()
        {
            if (Handle != IntPtr.Zero)
            {
                MacOsNative.void_objc_msgSend(Handle, s_selDrain);
            }
        }
    }

    private readonly struct TemporaryNSString : IDisposable
    {
        public TemporaryNSString(IntPtr handle)
        {
            Handle = handle;
        }

        public IntPtr Handle { get; }

        public void Dispose()
        {
            if (Handle != IntPtr.Zero)
            {
                MacOsNative.void_objc_msgSend(Handle, s_selRelease);
            }
        }
    }
}
