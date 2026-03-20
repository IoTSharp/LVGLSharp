using LVGLSharp;
using LVGLSharp.Interop;
using System;
using System.Runtime.InteropServices;

namespace LVGLSharp.Runtime.Linux;

public unsafe partial class X11View : IWindow
{
    [StructLayout(LayoutKind.Sequential)]
    private struct LvglHostX11
    {
        public int width;
        public int height;
        public int running;
        public nint user_data;
    }

    private const string HostLib = "lvgl_host_x11";
    private const string DefaultDisplay = ":1";

    [LibraryImport(HostLib, EntryPoint = "lvgl_host_x11_init", StringMarshalling = StringMarshalling.Utf8)]
    private static partial int lvgl_host_x11_init(ref LvglHostX11 host, int width, int height, string title);

    [LibraryImport(HostLib, EntryPoint = "lvgl_host_x11_poll")]
    private static partial void lvgl_host_x11_poll(ref LvglHostX11 host);

    [LibraryImport(HostLib, EntryPoint = "lvgl_host_x11_present")]
    private static partial void lvgl_host_x11_present(ref LvglHostX11 host);

    [LibraryImport(HostLib, EntryPoint = "lvgl_host_x11_shutdown")]
    private static partial void lvgl_host_x11_shutdown(ref LvglHostX11 host);

    [LibraryImport(HostLib, EntryPoint = "lvgl_host_x11_is_running")]
    private static partial int lvgl_host_x11_is_running(ref LvglHostX11 host);

    [LibraryImport(HostLib, EntryPoint = "lvgl_host_x11_set_keyboard_group")]
    private static partial void lvgl_host_x11_set_keyboard_group(ref LvglHostX11 host, lv_group_t* group);

    [LibraryImport("libc", EntryPoint = "setenv", StringMarshalling = StringMarshalling.Utf8)]
    private static partial int setenv(string name, string value, int overwrite);

    private readonly string _title;
    private readonly int _width;
    private readonly int _height;
    private readonly float _dpi;
    private LvglHostX11 _host;
    private bool _initialized;
    private lv_font_t* _fallbackFont;
    private lv_style_t* _defaultFontStyle;
    private SixLaborsFontManager? _fontManager;

    public X11View(string title = "LVGLSharp X11", int width = 800, int height = 600, float dpi = 96f)
    {
        _title = title;
        _width = width;
        _height = height;
        _dpi = dpi;
    }

    public static lv_obj_t* root { get; private set; }
    public static lv_group_t* key_inputGroup { get; private set; }
    public static delegate* unmanaged[Cdecl]<lv_event_t*, void> SendTextAreaFocusCb { get; private set; }

    public lv_obj_t* Root => root;
    public lv_group_t* KeyInputGroup => key_inputGroup;
    public delegate* unmanaged[Cdecl]<lv_event_t*, void> SendTextAreaFocusCallback => SendTextAreaFocusCb;

    private static void EnsureDisplayEnvironment()
    {
        if (!OperatingSystem.IsLinux())
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DISPLAY")))
        {
            return;
        }

        Environment.SetEnvironmentVariable("DISPLAY", DefaultDisplay);
        setenv("DISPLAY", DefaultDisplay, 1);
    }

    public void Init()
    {
        if (_initialized)
        {
            return;
        }

        EnsureDisplayEnvironment();
        LvglNativeLibraryResolver.EnsureRegistered();

        var rc = lvgl_host_x11_init(ref _host, _width, _height, _title);
        if (rc != 0)
        {
            throw new InvalidOperationException($"初始化 X11 host 失败，请检查 DISPLAY/X11 环境是否可用。当前 DISPLAY={Environment.GetEnvironmentVariable("DISPLAY") ?? "<null>"}");
        }

        root = lv_scr_act();
        key_inputGroup = lv_group_create();
        lvgl_host_x11_set_keyboard_group(ref _host, key_inputGroup);
        _fallbackFont = lv_obj_get_style_text_font(root, LV_PART_MAIN);

        var systemFontPath = LinuxSystemFontResolver.TryResolveFontPath();
        if (!string.IsNullOrWhiteSpace(systemFontPath))
        {
            _fontManager = new SixLaborsFontManager(
                systemFontPath,
                12,
                _dpi,
                _fallbackFont,
                LvglHostDefaults.CreateDefaultFontFallbackGlyphs());

            _defaultFontStyle = LvglHostDefaults.ApplyDefaultFontStyle(root, _fontManager.GetLvFontPtr());
        }
        SendTextAreaFocusCb = null;
        _initialized = true;
    }

    public void AttachTextInput(lv_obj_t* textArea)
    {
        // X11 版本先不接软键盘；后续如需输入法/虚拟键盘再补。
    }

    public void StartLoop(Action handle)
    {
        while (lvgl_host_x11_is_running(ref _host) != 0)
        {
            ProcessEvents();
            handle?.Invoke();
        }
    }

    public void ProcessEvents()
    {
        lvgl_host_x11_poll(ref _host);
        lvgl_host_x11_present(ref _host);
    }

    public void Stop()
    {
        if (!_initialized)
        {
            return;
        }

        lvgl_host_x11_shutdown(ref _host);
        _initialized = false;
    }
}
