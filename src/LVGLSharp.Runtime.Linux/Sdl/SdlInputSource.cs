using System;

using System.Runtime.InteropServices;

namespace LVGLSharp.Runtime.Linux;

internal sealed class SdlInputSource
{
    private string? _pendingText;

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

    public bool TryHandleEvent(SdlNative.SDL_Event sdlEvent, out bool closeRequested, out (int Width, int Height)? resize)
    {
        closeRequested = false;
        resize = null;

        switch (sdlEvent.type)
        {
            case SdlNative.SDL_QUIT:
                closeRequested = true;
                return true;
            case SdlNative.SDL_MOUSEMOTION:
                CurrentMousePosition = (sdlEvent.motion.x, sdlEvent.motion.y);
                return true;
            case SdlNative.SDL_MOUSEBUTTONDOWN:
                IsMousePressed = true;
                CurrentMouseButton = MapMouseButton(sdlEvent.button.button);
                CurrentMousePosition = (sdlEvent.button.x, sdlEvent.button.y);
                return true;
            case SdlNative.SDL_MOUSEBUTTONUP:
                IsMousePressed = false;
                CurrentMouseButton = 0;
                CurrentMousePosition = (sdlEvent.button.x, sdlEvent.button.y);
                return true;
            case SdlNative.SDL_MOUSEWHEEL:
                WheelDiff += sdlEvent.wheel.y;
                return true;
            case SdlNative.SDL_KEYDOWN:
                CurrentKey = MapSdlKey(sdlEvent.key.keysym.sym);
                IsKeyPressed = CurrentKey != 0;
                return true;
            case SdlNative.SDL_KEYUP:
                IsKeyPressed = false;
                return true;
            case SdlNative.SDL_TEXTINPUT:
                _pendingText = ReadTextInput(ref sdlEvent.text);
                return true;
            case SdlNative.SDL_WINDOWEVENT:
                if (sdlEvent.window.@event == SdlNative.SDL_WINDOWEVENT_SIZE_CHANGED)
                {
                    resize = (sdlEvent.window.data1, sdlEvent.window.data2);
                    return true;
                }

                break;
        }

        return false;
    }

    public int ConsumeWheelDiff()
    {
        var value = WheelDiff;
        WheelDiff = 0;
        return value;
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
    }

    private static unsafe string? ReadTextInput(ref SdlNative.SDL_TextInputEvent textEvent)
    {
        fixed (byte* textPtr = textEvent.text)
        {
            if (textPtr[0] == 0)
            {
                return null;
            }

            return Marshal.PtrToStringUTF8((IntPtr)textPtr);
        }
    }

    private static uint MapMouseButton(byte button)
    {
        return button switch
        {
            SdlNative.SDL_BUTTON_LEFT => 1U,
            SdlNative.SDL_BUTTON_RIGHT => 2U,
            SdlNative.SDL_BUTTON_MIDDLE => 4U,
            _ => 0U,
        };
    }

    private static uint MapSdlKey(int key)
    {
        return key switch
        {
            SdlNative.SDLK_RETURN => (uint)LV_KEY_ENTER,
            SdlNative.SDLK_ESCAPE => (uint)LV_KEY_ESC,
            SdlNative.SDLK_BACKSPACE => (uint)LV_KEY_BACKSPACE,
            SdlNative.SDLK_TAB => (uint)LV_KEY_NEXT,
            SdlNative.SDLK_HOME => (uint)LV_KEY_HOME,
            SdlNative.SDLK_END => (uint)LV_KEY_END,
            SdlNative.SDLK_DELETE => (uint)LV_KEY_DEL,
            SdlNative.SDLK_LEFT => (uint)LV_KEY_LEFT,
            SdlNative.SDLK_RIGHT => (uint)LV_KEY_RIGHT,
            SdlNative.SDLK_UP => (uint)LV_KEY_UP,
            SdlNative.SDLK_DOWN => (uint)LV_KEY_DOWN,
            SdlNative.SDLK_PAGEUP => (uint)LV_KEY_PREV,
            SdlNative.SDLK_PAGEDOWN => (uint)LV_KEY_NEXT,
            >= 32 and <= 126 => (uint)key,
            _ => 0,
        };
    }
}