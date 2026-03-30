using LVGLSharp;
using LVGLSharp.Interop;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using static LVGLSharp.Interop.LVGL;

namespace LVGLSharp.Runtime.MacOs;

public unsafe sealed class MacOsView : ViewLifetimeBase
{
    private readonly MacOsViewOptions _options;
    private readonly IMacOsSurface _surface;
    private MacOsFrameBuffer _frameBuffer;

    private static MacOsView? s_activeView;

    private lv_display_t* _lvDisplay;
    private lv_indev_t* _mouseIndev;
    private lv_indev_t* _keyboardIndev;
    private lv_indev_t* _wheelIndev;
    private lv_obj_t* _root;
    private lv_group_t* _keyInputGroup;
    private GCHandle _selfHandle;
    private lv_obj_t* _focusedTextArea;
    private byte* _drawBuffer;
    private uint _drawBufferByteSize;
    private bool _running;
    private bool _initialized;
    private ulong _lastPresentTick;

    public MacOsView()
        : this(new MacOsViewOptions())
    {
    }

    public MacOsView(MacOsViewOptions options)
        : this(options, new MacOsNativeSurface(options))
    {
    }

    internal MacOsView(MacOsViewOptions options, IMacOsSurface surface)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _options.Validate();
        _surface = surface ?? throw new ArgumentNullException(nameof(surface));
        _frameBuffer = new MacOsFrameBuffer(_options.Width, _options.Height);
    }

    public MacOsView(string title = "LVGLSharp MacOs", int width = 800, int height = 600, float dpi = 96f)
        : this(new MacOsViewOptions
        {
            Title = title,
            Width = width,
            Height = height,
            Dpi = dpi,
        })
    {
    }

    public MacOsViewOptions Options => _options;

    public IMacOsSurface Surface => _surface;

    public MacOsFrameBuffer FrameBuffer => _frameBuffer;

    public MacOsHostDiagnostics Diagnostics => new(
        _options.Title,
        _frameBuffer.Width,
        _frameBuffer.Height,
        _surface.IsCreated ? _surface.Dpi : _options.Dpi,
        _surface.IsCreated,
        _initialized,
        _running,
        _frameBuffer.Argb8888Bytes.Length > 0);

    public MacOsHostContext HostContext => new(_options, _surface, _frameBuffer, Diagnostics);

    public static (int X, int Y) CurrentMousePosition => s_activeView?._surface.CurrentMousePosition ?? (0, 0);

    public static uint CurrentMouseButton => s_activeView?._surface.CurrentMouseButton ?? 0;

    public override lv_obj_t* Root => _root;

    public override lv_group_t* KeyInputGroup => _keyInputGroup;

    public override delegate* unmanaged[Cdecl]<lv_event_t*, void> SendTextAreaFocusCallback => &HandleTextAreaFocused;

    protected override void OnOpenCore()
    {
        LvglNativeLibraryResolver.EnsureRegistered();

        if (!lv_is_initialized())
        {
            lv_init();
        }

        _surface.Create();
        InitializeDisplay();

        _root = lv_scr_act();
        _keyInputGroup = lv_group_create();
        lv_indev_set_group(_keyboardIndev, _keyInputGroup);

        s_activeView = this;
        _initialized = true;
        _running = true;
        _lastPresentTick = (ulong)Environment.TickCount64;
    }

    public override void HandleEvents()
    {
        if (!_initialized)
        {
            return;
        }

        _surface.PumpEvents();
        EnsureSurfaceSize();
        CommitPendingTextInput();
        PresentFrame();

        if (_surface.IsCloseRequested)
        {
            _running = false;
        }
    }

    protected override void RunLoopCore(Action iteration)
    {
        while (_running)
        {
            HandleEvents();
            iteration?.Invoke();
        }
    }

    protected override void OnCloseCore()
    {
        if (s_activeView == this)
        {
            s_activeView = null;
        }

        _running = false;
        _initialized = false;
        _focusedTextArea = null;

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

        if (_keyInputGroup != null)
        {
            lv_group_delete(_keyInputGroup);
            _keyInputGroup = null;
        }

        if (_lvDisplay != null)
        {
            lv_display_delete(_lvDisplay);
            _lvDisplay = null;
        }

        if (_drawBuffer != null)
        {
            NativeMemory.Free(_drawBuffer);
            _drawBuffer = null;
            _drawBufferByteSize = 0;
        }

        if (_selfHandle.IsAllocated)
        {
            _selfHandle.Free();
        }

        _root = null;
        _surface.Dispose();
        _lastPresentTick = 0;
    }

    public override void RegisterTextInput(lv_obj_t* textArea)
    {
        UpdateFocusedTextArea(textArea);
    }

    protected override bool CanSkipClose() =>
        !_initialized &&
        _lvDisplay == null &&
        _mouseIndev == null &&
        _keyboardIndev == null &&
        _wheelIndev == null &&
        _root == null &&
        _keyInputGroup == null;

    public override string ToString() => HostContext.ToString();

    private void InitializeDisplay()
    {
        AllocateBuffers(_surface.Width, _surface.Height);

        _lvDisplay = lv_display_create(_frameBuffer.Width, _frameBuffer.Height);
        if (_lvDisplay == null)
        {
            throw new InvalidOperationException("LVGL display 创建失败。");
        }

        if (!_selfHandle.IsAllocated)
        {
            _selfHandle = GCHandle.Alloc(this);
        }

        var selfPtr = (void*)GCHandle.ToIntPtr(_selfHandle);
        lv_display_set_user_data(_lvDisplay, selfPtr);
        lv_display_set_buffers(_lvDisplay, _drawBuffer, null, _drawBufferByteSize, lv_display_render_mode_t.LV_DISPLAY_RENDER_MODE_FULL);
        lv_display_set_flush_cb(_lvDisplay, &FlushCb);

        _mouseIndev = lv_indev_create();
        lv_indev_set_type(_mouseIndev, lv_indev_type_t.LV_INDEV_TYPE_POINTER);
        lv_indev_set_read_cb(_mouseIndev, &MouseReadCb);
        lv_indev_set_display(_mouseIndev, _lvDisplay);
        lv_indev_set_user_data(_mouseIndev, selfPtr);

        _keyboardIndev = lv_indev_create();
        lv_indev_set_type(_keyboardIndev, lv_indev_type_t.LV_INDEV_TYPE_KEYPAD);
        lv_indev_set_read_cb(_keyboardIndev, &KeyboardReadCb);
        lv_indev_set_display(_keyboardIndev, _lvDisplay);
        lv_indev_set_user_data(_keyboardIndev, selfPtr);

        _wheelIndev = lv_indev_create();
        lv_indev_set_type(_wheelIndev, lv_indev_type_t.LV_INDEV_TYPE_ENCODER);
        lv_indev_set_read_cb(_wheelIndev, &WheelReadCb);
        lv_indev_set_display(_wheelIndev, _lvDisplay);
        lv_indev_set_user_data(_wheelIndev, selfPtr);
    }

    private void AllocateBuffers(int width, int height)
    {
        if (_drawBuffer != null)
        {
            NativeMemory.Free(_drawBuffer);
            _drawBuffer = null;
            _drawBufferByteSize = 0;
        }

        _frameBuffer = new MacOsFrameBuffer(width, height);
        _drawBufferByteSize = DisplayBufferSizeHelper.GetRgb565DrawBufferByteSize(width, height);
        _drawBuffer = (byte*)NativeMemory.AllocZeroed((nuint)_drawBufferByteSize);
        if (_drawBuffer == null)
        {
            throw new OutOfMemoryException("macOS draw buffer 分配失败。");
        }
    }

    private void EnsureSurfaceSize()
    {
        if (_surface.Width <= 0 || _surface.Height <= 0)
        {
            return;
        }

        if (_surface.Width == _frameBuffer.Width && _surface.Height == _frameBuffer.Height)
        {
            return;
        }

        AllocateBuffers(_surface.Width, _surface.Height);

        if (_lvDisplay != null)
        {
            lv_display_set_resolution(_lvDisplay, _frameBuffer.Width, _frameBuffer.Height);
            lv_display_set_buffers(_lvDisplay, _drawBuffer, null, _drawBufferByteSize, lv_display_render_mode_t.LV_DISPLAY_RENDER_MODE_FULL);
        }

        if (_root != null)
        {
            lv_obj_invalidate(_root);
        }
    }

    private void PresentFrame()
    {
        var now = (ulong)Environment.TickCount64;
        var diff = _lastPresentTick == 0 ? 0U : (uint)(now - _lastPresentTick);
        _lastPresentTick = now;

        lv_tick_inc(diff);
        lv_timer_handler();
        _surface.Present(_frameBuffer.Argb8888Bytes, _frameBuffer.Width, _frameBuffer.Height, _frameBuffer.Stride);
    }

    private void CommitPendingTextInput()
    {
        CommitEditingKeys();

        var pendingText = _surface.ConsumePendingText();
        if (string.IsNullOrEmpty(pendingText) || _focusedTextArea == null)
        {
            return;
        }

        var utf8Bytes = System.Text.Encoding.UTF8.GetBytes(pendingText + "\0");
        fixed (byte* utf8Ptr = utf8Bytes)
        {
            lv_textarea_add_text(_focusedTextArea, utf8Ptr);
        }
    }

    private void CommitEditingKeys()
    {
        if (_focusedTextArea == null)
        {
            return;
        }

        switch (_surface.ConsumeEditingKeyPress())
        {
            case (uint)lv_key_t.LV_KEY_BACKSPACE:
                lv_textarea_delete_char(_focusedTextArea);
                break;
            case (uint)lv_key_t.LV_KEY_DEL:
                lv_textarea_delete_char_forward(_focusedTextArea);
                break;
            case (uint)lv_key_t.LV_KEY_LEFT:
                lv_textarea_cursor_left(_focusedTextArea);
                break;
            case (uint)lv_key_t.LV_KEY_RIGHT:
                lv_textarea_cursor_right(_focusedTextArea);
                break;
            case (uint)lv_key_t.LV_KEY_UP:
                lv_textarea_cursor_up(_focusedTextArea);
                break;
            case (uint)lv_key_t.LV_KEY_DOWN:
                lv_textarea_cursor_down(_focusedTextArea);
                break;
        }
    }

    private void UpdateFocusedTextArea(lv_obj_t* textArea)
    {
        _focusedTextArea = textArea;
        if (_focusedTextArea != null)
        {
            _surface.StartTextInput();
        }
        else
        {
            _surface.StopTextInput();
        }
    }

    private static MacOsView? GetViewFromDisplay(lv_display_t* display)
    {
        if (display == null)
        {
            return null;
        }

        var userData = lv_display_get_user_data(display);
        return userData == null ? null : (MacOsView?)GCHandle.FromIntPtr((IntPtr)userData).Target;
    }

    private static MacOsView? GetViewFromIndev(lv_indev_t* indev)
    {
        if (indev == null)
        {
            return null;
        }

        var userData = lv_indev_get_user_data(indev);
        return userData == null ? null : (MacOsView?)GCHandle.FromIntPtr((IntPtr)userData).Target;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void HandleTextAreaFocused(lv_event_t* e)
    {
        var view = s_activeView;
        if (view is null)
        {
            return;
        }

        view.UpdateFocusedTextArea(lv_event_get_target_obj(e));
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void FlushCb(lv_display_t* display, lv_area_t* area, byte* pxMap)
    {
        var view = GetViewFromDisplay(display) ?? s_activeView;
        if (view is null || area == null || pxMap == null)
        {
            lv_display_flush_ready(display);
            return;
        }

        var width = lv_area_get_width(area);
        var height = lv_area_get_height(area);
        var buffer = view._frameBuffer.Argb8888Bytes;
        var stride = view._frameBuffer.Stride;

        for (var y = 0; y < height; y++)
        {
            var src = ((ushort*)pxMap) + y * width;
            var rowOffset = (area->y1 + y) * stride;
            for (var x = 0; x < width; x++)
            {
                var pixel = src[x];
                var r = (byte)((((pixel >> 11) & 0x1F) << 3) | (((pixel >> 11) & 0x1F) >> 2));
                var g = (byte)((((pixel >> 5) & 0x3F) << 2) | (((pixel >> 5) & 0x3F) >> 4));
                var b = (byte)(((pixel & 0x1F) << 3) | ((pixel & 0x1F) >> 2));
                var offset = rowOffset + ((area->x1 + x) * 4);
                buffer[offset] = b;
                buffer[offset + 1] = g;
                buffer[offset + 2] = r;
                buffer[offset + 3] = 0xFF;
            }
        }

        lv_display_flush_ready(display);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void MouseReadCb(lv_indev_t* indev, lv_indev_data_t* data)
    {
        var view = GetViewFromIndev(indev) ?? s_activeView;
        if (view is null)
        {
            data->point.x = 0;
            data->point.y = 0;
            data->state = LV_INDEV_STATE_REL;
            data->enc_diff = 0;
            data->btn_id = 0;
            return;
        }

        data->point.x = view._surface.CurrentMousePosition.X;
        data->point.y = view._surface.CurrentMousePosition.Y;
        data->state = view._surface.IsMousePressed ? LV_INDEV_STATE_PR : LV_INDEV_STATE_REL;
        data->enc_diff = 0;
        data->btn_id = view._surface.CurrentMouseButton;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void KeyboardReadCb(lv_indev_t* indev, lv_indev_data_t* data)
    {
        var view = GetViewFromIndev(indev) ?? s_activeView;
        if (view is null)
        {
            data->key = 0;
            data->state = LV_INDEV_STATE_REL;
            return;
        }

        data->key = view._surface.CurrentKey;
        data->state = view._surface.IsKeyPressed ? LV_INDEV_STATE_PR : LV_INDEV_STATE_REL;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void WheelReadCb(lv_indev_t* indev, lv_indev_data_t* data)
    {
        var view = GetViewFromIndev(indev) ?? s_activeView;
        if (view is null)
        {
            data->enc_diff = 0;
            data->state = LV_INDEV_STATE_REL;
            return;
        }

        data->enc_diff = (short)view._surface.ConsumeWheelDiff();
        data->state = LV_INDEV_STATE_REL;
    }
}
