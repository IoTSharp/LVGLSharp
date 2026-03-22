using System;
using System.Runtime.InteropServices;

namespace LVGLSharp.Runtime.Linux;

internal static unsafe partial class SdlNative
{
    internal const string SdlLib = "libSDL2-2.0.so.0";

    internal const uint SDL_INIT_VIDEO = 0x00000020;
    internal const uint SDL_INIT_EVENTS = 0x00004000;

    internal const uint SDL_WINDOW_SHOWN = 0x00000004;
    internal const uint SDL_WINDOW_BORDERLESS = 0x00000010;
    internal const uint SDL_WINDOW_ALLOW_HIGHDPI = 0x00002000;

    internal const uint SDL_RENDERER_ACCELERATED = 0x00000002;
    internal const uint SDL_RENDERER_PRESENTVSYNC = 0x00000004;
    internal const uint SDL_RENDERER_TARGETTEXTURE = 0x00000008;

    internal const uint SDL_TEXTUREACCESS_STREAMING = 1;
    internal const uint SDL_PIXELFORMAT_ARGB8888 = 372645892;

    internal const uint SDL_QUIT = 0x100;
    internal const uint SDL_WINDOWEVENT = 0x200;
    internal const uint SDL_KEYDOWN = 0x300;
    internal const uint SDL_KEYUP = 0x301;
    internal const uint SDL_TEXTINPUT = 0x303;
    internal const uint SDL_MOUSEMOTION = 0x400;
    internal const uint SDL_MOUSEBUTTONDOWN = 0x401;
    internal const uint SDL_MOUSEBUTTONUP = 0x402;
    internal const uint SDL_MOUSEWHEEL = 0x403;

    internal const byte SDL_WINDOWEVENT_SIZE_CHANGED = 6;

    internal const byte SDL_BUTTON_LEFT = 1;
    internal const byte SDL_BUTTON_MIDDLE = 2;
    internal const byte SDL_BUTTON_RIGHT = 3;

    internal const int SDL_WINDOWPOS_CENTERED = 0x2FFF0000;

    internal const int SDLK_RETURN = 13;
    internal const int SDLK_ESCAPE = 27;
    internal const int SDLK_BACKSPACE = 8;
    internal const int SDLK_TAB = 9;
    internal const int SDLK_DELETE = 127;
    internal const int SDLK_LEFT = 1073741904;
    internal const int SDLK_RIGHT = 1073741903;
    internal const int SDLK_UP = 1073741906;
    internal const int SDLK_DOWN = 1073741905;
    internal const int SDLK_HOME = 1073741898;
    internal const int SDLK_END = 1073741901;
    internal const int SDLK_PAGEUP = 1073741899;
    internal const int SDLK_PAGEDOWN = 1073741902;

    [StructLayout(LayoutKind.Sequential)]
    internal struct SDL_Keysym
    {
        public uint scancode;
        public int sym;
        public ushort mod;
        public uint unused;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SDL_KeyboardEvent
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
    internal unsafe struct SDL_TextInputEvent
    {
        public uint type;
        public uint timestamp;
        public uint windowID;
        public fixed byte text[32];
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SDL_MouseMotionEvent
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
    internal struct SDL_MouseButtonEvent
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
    internal struct SDL_MouseWheelEvent
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
    internal struct SDL_QuitEvent
    {
        public uint type;
        public uint timestamp;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SDL_WindowEvent
    {
        public uint type;
        public uint timestamp;
        public uint windowID;
        public byte @event;
        public byte padding1;
        public byte padding2;
        public byte padding3;
        public int data1;
        public int data2;
    }

    [StructLayout(LayoutKind.Explicit, Size = 56)]
    internal struct SDL_Event
    {
        [FieldOffset(0)] public uint type;
        [FieldOffset(0)] public SDL_KeyboardEvent key;
        [FieldOffset(0)] public SDL_TextInputEvent text;
        [FieldOffset(0)] public SDL_MouseMotionEvent motion;
        [FieldOffset(0)] public SDL_MouseButtonEvent button;
        [FieldOffset(0)] public SDL_MouseWheelEvent wheel;
        [FieldOffset(0)] public SDL_WindowEvent window;
        [FieldOffset(0)] public SDL_QuitEvent quit;
    }

    [LibraryImport(SdlLib, EntryPoint = "SDL_Init")]
    internal static partial int SDL_Init(uint flags);

    [LibraryImport(SdlLib, EntryPoint = "SDL_Quit")]
    internal static partial void SDL_Quit();

    [LibraryImport(SdlLib, EntryPoint = "SDL_CreateWindow", StringMarshalling = StringMarshalling.Utf8)]
    internal static partial IntPtr SDL_CreateWindow(string title, int x, int y, int w, int h, uint flags);

    [LibraryImport(SdlLib, EntryPoint = "SDL_DestroyWindow")]
    internal static partial void SDL_DestroyWindow(IntPtr window);

    [LibraryImport(SdlLib, EntryPoint = "SDL_CreateRenderer")]
    internal static partial IntPtr SDL_CreateRenderer(IntPtr window, int index, uint flags);

    [LibraryImport(SdlLib, EntryPoint = "SDL_DestroyRenderer")]
    internal static partial void SDL_DestroyRenderer(IntPtr renderer);

    [LibraryImport(SdlLib, EntryPoint = "SDL_CreateTexture")]
    internal static partial IntPtr SDL_CreateTexture(IntPtr renderer, uint format, int access, int w, int h);

    [LibraryImport(SdlLib, EntryPoint = "SDL_DestroyTexture")]
    internal static partial void SDL_DestroyTexture(IntPtr texture);

    [LibraryImport(SdlLib, EntryPoint = "SDL_UpdateTexture")]
    internal static partial int SDL_UpdateTexture(IntPtr texture, IntPtr rect, IntPtr pixels, int pitch);

    [LibraryImport(SdlLib, EntryPoint = "SDL_RenderClear")]
    internal static partial int SDL_RenderClear(IntPtr renderer);

    [LibraryImport(SdlLib, EntryPoint = "SDL_RenderCopy")]
    internal static partial int SDL_RenderCopy(IntPtr renderer, IntPtr texture, IntPtr srcRect, IntPtr dstRect);

    [LibraryImport(SdlLib, EntryPoint = "SDL_RenderPresent")]
    internal static partial void SDL_RenderPresent(IntPtr renderer);

    [LibraryImport(SdlLib, EntryPoint = "SDL_PollEvent")]
    internal static partial int SDL_PollEvent(out SDL_Event sdlEvent);

    [LibraryImport(SdlLib, EntryPoint = "SDL_GetError")]
    internal static partial IntPtr SDL_GetError();

    [LibraryImport(SdlLib, EntryPoint = "SDL_SetWindowSize")]
    internal static partial void SDL_SetWindowSize(IntPtr window, int w, int h);

    [LibraryImport(SdlLib, EntryPoint = "SDL_StartTextInput")]
    internal static partial void SDL_StartTextInput();

    [LibraryImport(SdlLib, EntryPoint = "SDL_StopTextInput")]
    internal static partial void SDL_StopTextInput();

    internal static string GetSdlError()
    {
        var errorPtr = SDL_GetError();
        return errorPtr == IntPtr.Zero ? "Î´Öª´íÎó" : Marshal.PtrToStringUTF8(errorPtr) ?? "Î´Öª´íÎó";
    }
}