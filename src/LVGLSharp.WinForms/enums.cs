using System;
using System.Collections.Generic;
using System.Text;

namespace LVGLSharp.Forms
{
    public enum FlatStyle
    {
        Flat,
        Popup,
        Standard,
        System
    }
    public enum ContentAlignment
    {
        TopLeft = 0x001,
        TopCenter = 0x002,
        TopRight = 0x004,
        MiddleLeft = 0x010,
        MiddleCenter = 0x020,
        MiddleRight = 0x040,
        BottomLeft = 0x100,
        BottomCenter = 0x200,
        BottomRight = 0x400
    }
    //
    // 摘要:
    //     Specifies how rows or columns of user interface (UI) elements should be sized
    //     relative to their container.
    public enum SizeType
    {
        //
        // 摘要:
        //     The row or column should be automatically sized to share space with its peers.
        AutoSize = 0,
        //
        // 摘要:
        //     The row or column should be sized to an exact number of pixels.
        Absolute = 1,
        //
        // 摘要:
        //     The row or column should be sized as a percentage of the parent container.
        Percent = 2
    }
    public enum HighDpiMode
    {
        SystemAware
    }

    public enum AutoScaleMode
    {
        Font,
        Dpi,
        Inherit,
        None
    }
    public enum ControlStyles
    {

    }
    public enum AutoSizeMode
    {
        None = -1,
        GrowAndShrink = 0,
        GrowOnly = 1
    }
    [Flags]
    public enum MouseButtons
    {
        None = 0,
        Left = 0x100000,
        Right = 0x200000,
        Middle = 0x400000,
        XButton1 = 0x800000,
        XButton2 = 0x1000000
    }
    [Flags]
    public enum Keys
    {
        None = 0,
        
        // 修饰键掩码
        Modifiers = unchecked((int)0xFFFF0000),
        
        // 修饰键
        Shift = 0x00010000,
        Control = 0x00020000,
        Alt = 0x00040000,
        
        // 字母键
        A = 0x41,
        C = 0x43,
        V = 0x56,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,
        
        // 功能键
        Back = 8,
        Tab = 9,
        Enter = 13,
        Escape = 27,
        Space = 32,
        Delete = 46,
        
        // 方向键
        Left = 0x25,
        Up = 0x26,
        Right = 0x27,
        Down = 0x28,
        
        // 数字键
        D0 = 0x30,
        D1 = 0x31,
        D2 = 0x32,
        D3 = 0x33,
        D4 = 0x34,
        D5 = 0x35,
        D6 = 0x36,
        D7 = 0x37,
        D8 = 0x38,
        D9 = 0x39,
    }
    public enum ImeMode
    {
        Inherit = -1,
        Off = 0,
        On = 1,
        Disable = 3
    }
    public enum ImageLayout
    {
        None,
        Tile,
        Center,
        Stretch,
        Zoom
    }
    [Flags]
    public enum AnchorStyles
    {
        None = 0,
        Top = 1,
        Bottom = 2,
        Left = 4,
        Right = 8
    }
    public enum DockStyle
    {
        None,
        Top,
        Bottom,
        Left,
        Right,
        Fill
    }
    public enum RightToLeft
    {
        No = 0,
        Yes = 1,
        Inherit = 2
    }

    public enum BorderStyle
    {
        None,
        FixedSingle,
        Fixed3D
    }

    public enum HorizontalAlignment
    {
        Left,
        Right,
        Center
    }

    public enum Orientation
    {
        Horizontal,
        Vertical
    }

    public enum TickStyle
    {
        None,
        TopLeft,
        BottomRight,
        Both
    }

    public enum DrawMode
    {
        Normal,
        OwnerDrawFixed,
        OwnerDrawVariable
    }

    public enum SelectionMode
    {
        None,
        One,
        MultiSimple,
        MultiExtended
    }

    public enum PictureBoxSizeMode
    {
        Normal,
        StretchImage,
        AutoSize,
        CenterImage,
        Zoom
    }

    public enum FormWindowState
    {
        Normal,
        Minimized,
        Maximized
    }

    public enum FormBorderStyle
    {
        None,
        FixedSingle,
        Fixed3D,
        FixedDialog,
        Sizable,
        FixedToolWindow,
        SizableToolWindow
    }

    public enum FormStartPosition
    {
        Manual,
        CenterScreen,
        WindowsDefaultLocation,
        WindowsDefaultBounds,
        CenterParent
    }

    public enum ScrollBars
    {
        None,
        Horizontal,
        Vertical,
        Both,
        ForcedHorizontal,
        ForcedVertical,
        ForcedBoth
    }
}

    public enum ComboBoxStyle
    {
        Simple = 0,
        DropDown = 1,
        DropDownList = 2
    }

    public enum CheckState
    {
        Unchecked = 0,
        Checked = 1,
        Indeterminate = 2
    }

