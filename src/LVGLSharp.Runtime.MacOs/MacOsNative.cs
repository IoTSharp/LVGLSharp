using System.Runtime.InteropServices;

namespace LVGLSharp.Runtime.MacOs;

internal static partial class MacOsNative
{
    internal const string LibObjC = "/usr/lib/libobjc.A.dylib";
    internal const string CoreGraphics = "/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics";

    internal const nuint NSWindowStyleMaskTitled = 1;
    internal const nuint NSWindowStyleMaskClosable = 1 << 1;
    internal const nuint NSWindowStyleMaskMiniaturizable = 1 << 2;
    internal const nuint NSWindowStyleMaskResizable = 1 << 3;
    internal const nuint NSBackingStoreBuffered = 2;

    internal const nuint NSApplicationActivationPolicyRegular = 0;

    internal const nuint NSViewWidthSizable = 1 << 1;
    internal const nuint NSViewHeightSizable = 1 << 4;

    internal static readonly nuint NSEventMaskAny = nuint.MaxValue;

    internal const nuint NSEventTypeLeftMouseDown = 1;
    internal const nuint NSEventTypeLeftMouseUp = 2;
    internal const nuint NSEventTypeRightMouseDown = 3;
    internal const nuint NSEventTypeRightMouseUp = 4;
    internal const nuint NSEventTypeMouseMoved = 5;
    internal const nuint NSEventTypeLeftMouseDragged = 6;
    internal const nuint NSEventTypeRightMouseDragged = 7;
    internal const nuint NSEventTypeKeyDown = 10;
    internal const nuint NSEventTypeKeyUp = 11;
    internal const nuint NSEventTypeFlagsChanged = 12;
    internal const nuint NSEventTypeScrollWheel = 22;
    internal const nuint NSEventTypeOtherMouseDown = 25;
    internal const nuint NSEventTypeOtherMouseUp = 26;
    internal const nuint NSEventTypeOtherMouseDragged = 27;

    internal const nuint MacVirtualKeyReturn = 36;
    internal const nuint MacVirtualKeyTab = 48;
    internal const nuint MacVirtualKeyDelete = 51;
    internal const nuint MacVirtualKeyEscape = 53;
    internal const nuint MacVirtualKeyForwardDelete = 117;
    internal const nuint MacVirtualKeyHome = 115;
    internal const nuint MacVirtualKeyEnd = 119;
    internal const nuint MacVirtualKeyPageUp = 116;
    internal const nuint MacVirtualKeyPageDown = 121;
    internal const nuint MacVirtualKeyLeftArrow = 123;
    internal const nuint MacVirtualKeyRightArrow = 124;
    internal const nuint MacVirtualKeyDownArrow = 125;
    internal const nuint MacVirtualKeyUpArrow = 126;

    internal const uint kCGImageAlphaPremultipliedFirst = 2;
    internal const uint kCGBitmapByteOrder32Little = 2u << 12;
    internal const int kCGRenderingIntentDefault = 0;

    [StructLayout(LayoutKind.Sequential)]
    internal struct CGPoint
    {
        public double X;
        public double Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CGSize
    {
        public double Width;
        public double Height;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CGRect
    {
        public CGPoint Origin;
        public CGSize Size;

        public CGRect(double x, double y, double width, double height)
        {
            Origin = new CGPoint { X = x, Y = y };
            Size = new CGSize { Width = width, Height = height };
        }
    }

    [LibraryImport(LibObjC, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial IntPtr objc_getClass(string name);

    [LibraryImport(LibObjC, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial IntPtr sel_registerName(string name);

    [LibraryImport(LibObjC, EntryPoint = "objc_msgSend")]
    internal static partial IntPtr IntPtr_objc_msgSend(IntPtr receiver, IntPtr selector);

    [LibraryImport(LibObjC, EntryPoint = "objc_msgSend")]
    internal static partial void void_objc_msgSend(IntPtr receiver, IntPtr selector);

    [LibraryImport(LibObjC, EntryPoint = "objc_msgSend")]
    internal static partial void void_bool_objc_msgSend(IntPtr receiver, IntPtr selector, [MarshalAs(UnmanagedType.I1)] bool value);

    [LibraryImport(LibObjC, EntryPoint = "objc_msgSend")]
    internal static partial void void_nuint_objc_msgSend(IntPtr receiver, IntPtr selector, nuint value);

    [LibraryImport(LibObjC, EntryPoint = "objc_msgSend")]
    internal static partial void void_double_objc_msgSend(IntPtr receiver, IntPtr selector, double value);

    [LibraryImport(LibObjC, EntryPoint = "objc_msgSend")]
    internal static partial void void_IntPtr_objc_msgSend(IntPtr receiver, IntPtr selector, IntPtr value);

    [LibraryImport(LibObjC, EntryPoint = "objc_msgSend")]
    internal static partial IntPtr IntPtr_CGRect_objc_msgSend(IntPtr receiver, IntPtr selector, CGRect rect);

    [LibraryImport(LibObjC, EntryPoint = "objc_msgSend")]
    internal static partial IntPtr IntPtr_CGRect_nuint_nuint_bool_objc_msgSend(IntPtr receiver, IntPtr selector, CGRect rect, nuint styleMask, nuint backing, [MarshalAs(UnmanagedType.I1)] bool deferCreation);

    [LibraryImport(LibObjC, EntryPoint = "objc_msgSend", StringMarshalling = StringMarshalling.Utf8)]
    internal static partial IntPtr IntPtr_string_objc_msgSend(IntPtr receiver, IntPtr selector, string value);

    [LibraryImport(LibObjC, EntryPoint = "objc_msgSend")]
    internal static partial IntPtr IntPtr_nuint_IntPtr_IntPtr_bool_objc_msgSend(IntPtr receiver, IntPtr selector, nuint mask, IntPtr untilDate, IntPtr inMode, [MarshalAs(UnmanagedType.I1)] bool dequeue);

    [LibraryImport(LibObjC, EntryPoint = "objc_msgSend")]
    internal static partial CGRect CGRect_objc_msgSend(IntPtr receiver, IntPtr selector);

    [LibraryImport(LibObjC, EntryPoint = "objc_msgSend")]
    internal static partial CGPoint CGPoint_objc_msgSend(IntPtr receiver, IntPtr selector);

    [LibraryImport(LibObjC, EntryPoint = "objc_msgSend")]
    internal static partial double double_objc_msgSend(IntPtr receiver, IntPtr selector);

    [LibraryImport(LibObjC, EntryPoint = "objc_msgSend")]
    internal static partial nuint nuint_objc_msgSend(IntPtr receiver, IntPtr selector);

    [LibraryImport(LibObjC, EntryPoint = "objc_msgSend")]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool bool_objc_msgSend(IntPtr receiver, IntPtr selector);

    [LibraryImport(CoreGraphics)]
    internal static partial IntPtr CGColorSpaceCreateDeviceRGB();

    [LibraryImport(CoreGraphics)]
    internal static partial void CGColorSpaceRelease(IntPtr colorSpace);

    [LibraryImport(CoreGraphics)]
    internal static partial IntPtr CGDataProviderCreateWithData(IntPtr info, IntPtr data, nuint size, IntPtr releaseData);

    [LibraryImport(CoreGraphics)]
    internal static partial void CGDataProviderRelease(IntPtr provider);

    [LibraryImport(CoreGraphics)]
    internal static partial IntPtr CGImageCreate(
        nuint width,
        nuint height,
        nuint bitsPerComponent,
        nuint bitsPerPixel,
        nuint bytesPerRow,
        IntPtr space,
        uint bitmapInfo,
        IntPtr provider,
        IntPtr decode,
        [MarshalAs(UnmanagedType.I1)] bool shouldInterpolate,
        int intent);

    [LibraryImport(CoreGraphics)]
    internal static partial void CGImageRelease(IntPtr image);

    internal static IntPtr GetClass(string name) => objc_getClass(name);

    internal static IntPtr GetSelector(string name) => sel_registerName(name);
}
