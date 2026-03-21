using LVGLSharp;
using LVGLSharp.Interop;
using System;

namespace LVGLSharp.Runtime.Linux;

public unsafe class LinuxView : IView
{
    private readonly IView _inner;
    private readonly LinuxHostEnvironment _environment;
    private static LinuxView? s_activeView;

    public LinuxView(string title = "LVGLSharp Linux", int width = 800, int height = 600, float dpi = 96f,
        string fbdev = "/dev/fb0", string indev = "/dev/input/event0", bool borderless = false)
    {
        var detectedDisplay = LinuxEnvironmentDetector.DetectX11Display();
        _environment = LinuxEnvironmentDetector.ResolveHostEnvironment(detectedDisplay, fbdev);

        _inner = _environment switch
        {
            LinuxHostEnvironment.Wslg => new WslgView(title, width, height, dpi, detectedDisplay, borderless),
            LinuxHostEnvironment.FrameBuffer => new FrameBufferView(fbdev, indev, dpi),
            LinuxHostEnvironment.X11 => new X11View(title, width, height, dpi, detectedDisplay, borderless),
            _ => throw new InvalidOperationException($"Unsupported Linux view mode: {_environment}"),
        };

        s_activeView = this;
    }

    public lv_obj_t* Root => _inner.Root;
    public lv_group_t* KeyInputGroup => _inner.KeyInputGroup;
    public delegate* unmanaged[Cdecl]<lv_event_t*, void> SendTextAreaFocusCallback => _inner.SendTextAreaFocusCallback;
    public static (int X, int Y) CurrentMousePosition => s_activeView?._inner is X11View ? X11View.CurrentMousePosition : (0, 0);
    public static uint CurrentMouseButton => s_activeView?._inner is X11View ? X11View.CurrentMouseButton : 0U;

    public void Init()
    {
        _inner.Init();
    }

    public void ProcessEvents()
    {
        _inner.ProcessEvents();
    }

    public void StartLoop(Action handle)
    {
        _inner.StartLoop(handle);
    }

    public void Stop()
    {
        _inner.Stop();
    }

    public void AttachTextInput(lv_obj_t* textArea)
    {
        _inner.AttachTextInput(textArea);
    }
}
