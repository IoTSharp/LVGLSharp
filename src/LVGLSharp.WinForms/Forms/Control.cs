using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using LVGLSharp.Drawing;
using LVGLSharp.Interop;
namespace LVGLSharp.Forms
{
    public class Control : Component, IComponent, IDisposable
    {
        static Control()
        {
            DefaultFont = new Font("Segoe UI", 9F);
        }

        private int _layoutSuspendCount;
        private bool _layoutPending;
        private Control? _layoutAffectedControl;
        private string? _layoutAffectedProperty;
        private Size _anchorReferenceParentSize;
        private Rectangle _anchorReferenceBounds;

        //
        // 摘要:
        //     Initializes a new instance of the System.Windows.Forms.Control class with default
        //     settings.
        public Control() : base()
        {
            Controls = CreateControlsInstance();
            Visible = true;
            Enabled = true;
            TabStop = true;
            Text = string.Empty;
            Name = string.Empty;
            Margin = new Padding(3);
            Padding = Padding.Empty;
            Anchor = AnchorStyles.Top | AnchorStyles.Left;
            WindowTarget = NullViewTarget.Instance;
            ProductName = string.Empty;
            ProductVersion = string.Empty;
            LayoutEngine = new LayoutEngine();
            CompanyName = string.Empty;
            DataBindings = new ControlBindingsCollection();
            DefaultCursor = new Cursor();
            CreateParams = new CreateParams();
        }
        //
        // 摘要:
        //     Initializes a new instance of the System.Windows.Forms.Control class with specific
        //     text.
        //
        // 参数:
        //   text:
        //     The text displayed by the control.
        public Control(string? text) : this()
        {

        }
        //
        // 摘要:
        //     Initializes a new instance of the System.Windows.Forms.Control class as a child
        //     control, with specific text.
        //
        // 参数:
        //   parent:
        //     The System.Windows.Forms.Control to be the parent of the control.
        //
        //   text:
        //     The text displayed by the control.
        public Control(Control? parent, string? text) : this(text)
        {

        }
        //
        // 摘要:
        //     Initializes a new instance of the System.Windows.Forms.Control class with specific
        //     text, size, and location.
        //
        // 参数:
        //   text:
        //     The text displayed by the control.
        //
        //   left:
        //     The System.Drawing.Point.X position of the control, in pixels, from the left
        //     edge of the control's container. The value is assigned to the System.Windows.Forms.Control.Left
        //     property.
        //
        //   top:
        //     The System.Drawing.Point.Y position of the control, in pixels, from the top edge
        //     of the control's container. The value is assigned to the System.Windows.Forms.Control.Top
        //     property.
        //
        //   width:
        //     The width of the control, in pixels. The value is assigned to the System.Windows.Forms.Control.Width
        //     property.
        //
        //   height:
        //     The height of the control, in pixels. The value is assigned to the System.Windows.Forms.Control.Height
        //     property.
        public Control(string? text, int left, int top, int width, int height) : this(text)
        {

        }
        //
        // 摘要:
        //     Initializes a new instance of the System.Windows.Forms.Control class as a child
        //     control, with specific text, size, and location.
        //
        // 参数:
        //   parent:
        //     The System.Windows.Forms.Control to be the parent of the control.
        //
        //   text:
        //     The text displayed by the control.
        //
        //   left:
        //     The System.Drawing.Point.X position of the control, in pixels, from the left
        //     edge of the control's container. The value is assigned to the System.Windows.Forms.Control.Left
        //     property.
        //
        //   top:
        //     The System.Drawing.Point.Y position of the control, in pixels, from the top edge
        //     of the control's container. The value is assigned to the System.Windows.Forms.Control.Top
        //     property.
        //
        //   width:
        //     The width of the control, in pixels. The value is assigned to the System.Windows.Forms.Control.Width
        //     property.
        //
        //   height:
        //     The height of the control, in pixels. The value is assigned to the System.Windows.Forms.Control.Height
        //     property.
        public Control(Control? parent, string? text, int left, int top, int width, int height) : this(parent, text)
        {

        }

        //
        // 摘要:
        //     Gets the default font of the control.
        //
        // 返回结果:
        //     The default System.Drawing.Font of the control. The value returned will vary
        //     depending on the user's operating system the local culture setting of their system.
        //
        //
        // 异常:
        //   T:System.ArgumentException:
        //     The default font or the regional alternative fonts are not installed on the client
        //     computer.
        public static Font DefaultFont { get; private set; }
        //
        // 摘要:
        //     Gets the default foreground color of the control.
        //
        // 返回结果:
        //     The default foreground System.Drawing.Color of the control. The default is System.Drawing.SystemColors.ControlText.
        public static Color DefaultForeColor { get; }
        //
        // 摘要:
        //     Gets or sets a value indicating whether to catch calls on the wrong thread that
        //     access a control's System.Windows.Forms.Control.Handle property when an application
        //     is being debugged.
        //
        // 返回结果:
        //     true if calls on the wrong thread are caught; otherwise, false.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static bool CheckForIllegalCrossThreadCalls { get; set; }
        //
        // 摘要:
        //     Gets the default background color of the control.
        //
        // 返回结果:
        //     The default background System.Drawing.Color of the control. The default is System.Drawing.SystemColors.Control.
        public static Color DefaultBackColor { get; }
        //
        // 摘要:
        //     Gets a value indicating which of the mouse buttons is in a pressed state.
        //
        // 返回结果:
        //     A bitwise combination of the System.Windows.Forms.MouseButtons enumeration values.
        //     The default is System.Windows.Forms.MouseButtons.None.
        public static MouseButtons MouseButtons { get; }
        //
        // 摘要:
        //     Gets a value indicating which of the modifier keys (SHIFT, CTRL, and ALT) is
        //     in a pressed state.
        //
        // 返回结果:
        //     A bitwise combination of the System.Windows.Forms.Keys values. The default is
        //     System.Windows.Forms.Keys.None.
        public static Keys ModifierKeys { get; }
        //
        // 摘要:
        //     Gets the position of the mouse cursor in screen coordinates.
        //
        // 返回结果:
        //     A System.Drawing.Point that contains the coordinates of the mouse cursor relative
        //     to the upper-left corner of the screen.
        public static Point MousePosition { get; }
        //
        // 摘要:
        //     Gets an object that represents a propagating IME mode.
        //
        // 返回结果:
        //     An object that represents a propagating IME mode.
        protected static ImeMode PropagatingImeMode { get; }
#nullable disable

        //
        // 摘要:
        //     Gets or sets the width of the control.
        //
        // 返回结果:
        //     The width of the control in pixels.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlWidthDescr")]
        public int Width
        {
            get => _size.Width;
            set => Size = new Size(value, _size.Height);
        }
#nullable enable
        //
        // 摘要:
        //     This property is not relevant for this class.
        //
        // 返回结果:
        //     The NativeWindow contained within the control.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [SRCategoryAttribute("CatBehavior")]
        [SRDescriptionAttribute("ControlWindowTargetDescr")]
        public IViewTarget WindowTarget { get; set; }
        //
        // 摘要:
        //     Gets the size of a rectangular area into which the control can fit.
        //
        // 返回结果:
        //     A System.Drawing.Size containing the height and width, in pixels.
        [Browsable(false)]
        public Size PreferredSize { get; }
        //
        // 摘要:
        //     Gets or sets padding within the control.
        //
        // 返回结果:
        //     A System.Windows.Forms.Padding representing the control's internal spacing characteristics.
        [Localizable(true)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlPaddingDescr")]
        public Padding Padding
        {
            get => _padding;
            set
            {
                _padding = value;
                OnPaddingChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets or sets a value indicating whether to use the wait cursor for the current
        //     control and all child controls.
        //
        // 返回结果:
        //     true to use the wait cursor for the current control and all child controls; otherwise,
        //     false. The default is false.
        [Browsable(true)]
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [SRCategoryAttribute("CatAppearance")]
        [SRDescriptionAttribute("ControlUseWaitCursorDescr")]
        public bool UseWaitCursor { get; set; }
        //
        // 摘要:
        //     Gets or sets a value indicating whether the control and all its child controls
        //     are displayed.
        //
        // 返回结果:
        //     true to display the control and its child controls; otherwise, false. The default
        //     is true. When getting the value, true is returned only if the control is visible
        //     and the parent control, if it exists, is visible.
        [Localizable(true)]
        [SRCategoryAttribute("CatBehavior")]
        [SRDescriptionAttribute("ControlVisibleDescr")]
        public bool Visible
        {
            get => _visible;
            set
            {
                if (_visible == value) return;
                _visible = value;
                unsafe
                {
                    var obj = (Interop.lv_obj_t*)_lvglObjectHandle;
                    if (obj != null)
                    {
                        if (_visible)
                            lv_obj_remove_flag(obj, LV_OBJ_FLAG_HIDDEN);
                        else
                            lv_obj_add_flag(obj, LV_OBJ_FLAG_HIDDEN);
                    }
                }
                OnVisibleChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets or sets the Input Method Editor (IME) mode of the control.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.ImeMode values. The default is System.Windows.Forms.ImeMode.Inherit.
        //
        //
        // 异常:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     The assigned value is not one of the System.Windows.Forms.ImeMode enumeration
        //     values.
        [AmbientValue(ImeMode.Inherit)]
        [Localizable(true)]
        [SRCategoryAttribute("CatBehavior")]
        [SRDescriptionAttribute("ControlIMEModeDescr")]
        public ImeMode ImeMode { get; set; }
        //
        // 摘要:
        //     Gets a value indicating whether the control has a handle associated with it.
        //
        //
        // 返回结果:
        //     true if a handle has been assigned to the control; otherwise, false.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRDescriptionAttribute("ControlHandleCreatedDescr")]
        public bool IsHandleCreated => Handle!= nint.Zero;
        //
        // 摘要:
        //     Gets or sets the height of the control.
        //
        // 返回结果:
        //     The height of the control in pixels.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlHeightDescr")]
        public int Height
        {
            get => _size.Height;
            set => Size = new Size(_size.Width, value);
        }
        //
        // 摘要:
        //     Gets a value indicating whether the control contains one or more child controls.
        //
        //
        // 返回结果:
        //     true if the control contains one or more child controls; otherwise, false.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRDescriptionAttribute("ControlHasChildrenDescr")]
        public bool HasChildren { get; }
        //
        // 摘要:
        //     Gets the window handle that the control is bound to.
        //
        // 返回结果:
        //     An System.IntPtr that contains the window handle (HWND) of the control.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DispId(-515)]
        [SRDescriptionAttribute("ControlHandleDescr")]
        public nint Handle { get; internal set; }
        //
        // 摘要:
        //     Gets or sets the foreground color of the control.
        //
        // 返回结果:
        //     The foreground System.Drawing.Color of the control. The default is the value
        //     of the System.Windows.Forms.Control.DefaultForeColor property.
        [DispId(-513)]
        [SRCategoryAttribute("CatAppearance")]
        [SRDescriptionAttribute("ControlForeColorDescr")]
        public virtual Color ForeColor
        {
            get => _foreColor;
            set
            {
                if (_foreColor == value) return;
                _foreColor = value;
                unsafe
                {
                    var obj = (Interop.lv_obj_t*)_lvglObjectHandle;
                    if (obj != null && value != Color.Empty)
                        lv_obj_set_style_text_color(obj, lv_color_make(value.R, value.G, value.B), 0);
                }
                OnForeColorChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets or sets the font of the text displayed by the control.
        //
        // 返回结果:
        //     The System.Drawing.Font to apply to the text displayed by the control. The default
        //     is the value of the System.Windows.Forms.Control.DefaultFont property.
        [AmbientValue(null)]
        [DispId(-512)]
        [Localizable(true)]
        [SRCategoryAttribute("CatAppearance")]
        [SRDescriptionAttribute("ControlFontDescr")]
        public virtual Font Font
        {
            get => _font!;
            set
            {
                if (ReferenceEquals(_font, value)) return;
                _font = value;
                OnFontChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets a value indicating whether the caller must call an invoke method when making
        //     method calls to the control because the caller is on a different thread than
        //     the one the control was created on.
        //
        // 返回结果:
        //     true if the control's System.Windows.Forms.Control.Handle was created on a different
        //     thread than the calling thread (indicating that you must make calls to the control
        //     through an invoke method); otherwise, false.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRDescriptionAttribute("ControlInvokeRequiredDescr")]
        public bool InvokeRequired { get; }
        //
        // 摘要:
        //     Gets or sets a value indicating whether the control is visible to accessibility
        //     applications.
        //
        // 返回结果:
        //     true if the control is visible to accessibility applications; otherwise, false.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatBehavior")]
        [SRDescriptionAttribute("ControlIsAccessibleDescr")]
        public bool IsAccessible { get; set; }
        //
        // 摘要:
        //     Gets or sets the text associated with this control.
        //
        // 返回结果:
        //     The text associated with this control.
        [Bindable(true)]
        [DispId(-517)]
        [Localizable(true)]
        [SRCategoryAttribute("CatAppearance")]
        [SRDescriptionAttribute("ControlTextDescr")]
        public virtual string Text
        {
            get => _text ?? string.Empty;
            set
            {
                if (_text == value) return;
                _text = value;
                OnTextChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets the parent control that is not parented by another Windows Forms control.
        //     Typically, this is the outermost System.Windows.Forms.Form that the control is
        //     contained in.
        //
        // 返回结果:
        //     The System.Windows.Forms.Control that represents the top-level control that contains
        //     the current control.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatBehavior")]
        [SRDescriptionAttribute("ControlTopLevelControlDescr")]
        public Control? TopLevelControl { get; }
        //
        // 摘要:
        //     Gets or sets the size that is the lower limit that System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size)
        //     can specify.
        //
        // 返回结果:
        //     An ordered pair of type System.Drawing.Size representing the width and height
        //     of a rectangle.
        [Localizable(true)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlMinimumSizeDescr")]
        public virtual Size MinimumSize { get; set; }
        //
        // 摘要:
        //     Gets or sets the name of the control.
        //
        // 返回结果:
        //     The name of the control. The default is an empty string ("").
        [Browsable(false)]
        public string Name { get; set; }
        //
        // 摘要:
        //     Gets or sets the parent container of the control.
        //
        // 返回结果:
        //     A System.Windows.Forms.Control that represents the parent or container control
        //     of the control.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [SRCategoryAttribute("CatBehavior")]
        [SRDescriptionAttribute("ControlParentDescr")]
        public Control? Parent
        {
            get => _parent;
            set
            {
                if (ReferenceEquals(_parent, value)) return;
                _parent = value;
                OnParentChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets the product name of the assembly containing the control.
        //
        // 返回结果:
        //     The product name of the assembly containing the control.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRDescriptionAttribute("ControlProductNameDescr")]
        public string ProductName { get; private set; }
        //
        // 摘要:
        //     Gets the version of the assembly containing the control.
        //
        // 返回结果:
        //     The file version of the assembly containing the control.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRDescriptionAttribute("ControlProductVersionDescr")]
        public string ProductVersion { get; private set; }
        //
        // 摘要:
        //     Gets a value indicating whether the control is currently re-creating its handle.
        //
        //
        // 返回结果:
        //     true if the control is currently re-creating its handle; otherwise, false.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatBehavior")]
        [SRDescriptionAttribute("ControlRecreatingHandleDescr")]
        public bool RecreatingHandle { get; }
        //
        // 摘要:
        //     Gets or sets the window region associated with the control.
        //
        // 返回结果:
        //     The window System.Drawing.Region associated with the control.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlRegionDescr")]
        public Region? Region
        {
            get => _region;
            set
            {
                if (ReferenceEquals(_region, value)) return;
                _region = value;
                OnRegionChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets or sets the coordinates of the upper-left corner of the control relative
        //     to the upper-left corner of its container.
        //
        // 返回结果:
        //     The System.Drawing.Point that represents the upper-left corner of the control
        //     relative to the upper-left corner of its container.
        [Localizable(true)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlLocationDescr")]
        public Point Location
        {
            get => _location;
            set
            {
                if (_location == value) return;
                _location = value;
                unsafe
                {
                    var obj = (Interop.lv_obj_t*)_lvglObjectHandle;
                    if (obj != null && _dock != DockStyle.Fill)
                        lv_obj_set_pos(obj, value.X, value.Y);
                }
                OnLocationChanged(EventArgs.Empty);
                OnMove(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets or sets the distance, in pixels, between the left edge of the control and
        //     the left edge of its container's client area.
        //
        // 返回结果:
        //     An System.Int32 representing the distance, in pixels, between the left edge of
        //     the control and the left edge of its container's client area.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlLeftDescr")]
        public int Left
        {
            get => _location.X;
            set => Location = new Point(value, _location.Y);
        }
        //
        // 摘要:
        //     Gets the distance, in pixels, between the right edge of the control and the left
        //     edge of its container's client area.
        //
        // 返回结果:
        //     An System.Int32 representing the distance, in pixels, between the right edge
        //     of the control and the left edge of its container's client area.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlRightDescr")]
        public int Right => _location.X + _size.Width;
        //
        // 摘要:
        //     Gets or sets a value indicating whether control's elements are aligned to support
        //     locales using right-to-left fonts.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.RightToLeft values. The default is System.Windows.Forms.RightToLeft.Inherit.
        //
        //
        // 异常:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     The assigned value is not one of the System.Windows.Forms.RightToLeft values.
        [AmbientValue(RightToLeft.Inherit)]
        [Localizable(true)]
        [SRCategoryAttribute("CatAppearance")]
        [SRDescriptionAttribute("ControlRightToLeftDescr")]
        public virtual RightToLeft RightToLeft
        {
            get => _rightToLeft;
            set
            {
                if (_rightToLeft == value) return;
                _rightToLeft = value;
                OnRightToLeftChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets a value indicating whether the control is mirrored.
        //
        // 返回结果:
        //     true if the control is mirrored; otherwise, false.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("IsMirroredDescr")]
        public bool IsMirrored { get; }
        //
        // 摘要:
        //     Gets or sets the site of the control.
        //
        // 返回结果:
        //     The System.ComponentModel.ISite associated with the System.Windows.Forms.Control,
        //     if any.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override ISite? Site { get; set; }
        //
        // 摘要:
        //     Gets or sets the height and width of the control.
        //
        // 返回结果:
        //     The System.Drawing.Size that represents the height and width of the control in
        //     pixels.
        [Localizable(true)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlSizeDescr")]
        public Size Size
        {
            get => _size;
            set
            {
                if (_size == value) return;
                _size = value;
                unsafe
                {
                    var obj = (Interop.lv_obj_t*)_lvglObjectHandle;
                    if (obj != null && _dock != DockStyle.Fill)
                    {
                        int w = value.Width > 0 ? value.Width : LV_SIZE_CONTENT;
                        int h = value.Height > 0 ? value.Height : LV_SIZE_CONTENT;
                        lv_obj_set_size(obj, w, h);
                    }
                }
                OnSizeChanged(EventArgs.Empty);
                OnResize(EventArgs.Empty);
                OnClientSizeChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets or sets the tab order of the control within its container.
        //
        // 返回结果:
        //     The index value of the control within the set of controls within its container.
        //     The controls in the container are included in the tab order.
        [Localizable(true)]
        [MergableProperty(false)]
        [SRCategoryAttribute("CatBehavior")]
        [SRDescriptionAttribute("ControlTabIndexDescr")]
        public int TabIndex
        {
            get => _tabIndex;
            set
            {
                if (_tabIndex == value) return;
                _tabIndex = value;
                OnTabIndexChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets or sets a value indicating whether the user can give the focus to this control
        //     using the TAB key.
        //
        // 返回结果:
        //     true if the user can give the focus to the control using the TAB key; otherwise,
        //     false. The default is true. Note: This property will always return true for an
        //     instance of the System.Windows.Forms.Form class.
        [DefaultValue(true)]
        [DispId(-516)]
        [SRCategoryAttribute("CatBehavior")]
        [SRDescriptionAttribute("ControlTabStopDescr")]
        public bool TabStop
        {
            get => _tabStop;
            set
            {
                if (_tabStop == value) return;
                _tabStop = value;
                OnTabStopChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets or sets the object that contains data about the control.
        //
        // 返回结果:
        //     An System.Object that contains data about the control. The default is null.
        [Bindable(true)]
        [DefaultValue(null)]
        [Localizable(false)]
        [SRCategoryAttribute("CatData")]
        [SRDescriptionAttribute("ControlTagDescr")]
        [TypeConverter(typeof(StringConverter))]
        public object? Tag { get; set; }
        //
        // 摘要:
        //     Gets a value indicating whether the control has input focus.
        //
        // 返回结果:
        //     true if the control has focus; otherwise, false.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRDescriptionAttribute("ControlFocusedDescr")]
        public virtual bool Focused { get; }
        //
        // 摘要:
        //     Gets or sets the distance, in pixels, between the top edge of the control and
        //     the top edge of its container's client area.
        //
        // 返回结果:
        //     An System.Int32 representing the distance, in pixels, between the top edge of
        //     the control and the top edge of its container's client area.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlTopDescr")]
        public int Top
        {
            get => _location.Y;
            set => Location = new Point(_location.X, value);
        }
        //
        // 摘要:
        //     Indicates if one of the Ancestors of this control is sited and that site in DesignMode.
        //     This property is read-only.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool IsAncestorSiteInDesignMode { get; }
        //
        // 摘要:
        //     Gets or sets a value indicating whether the control can respond to user interaction.
        //
        //
        // 返回结果:
        //     true if the control can respond to user interaction; otherwise, false. The default
        //     is true.
        [DispId(-514)]
        [Localizable(true)]
        [SRCategoryAttribute("CatBehavior")]
        [SRDescriptionAttribute("ControlEnabledDescr")]
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled == value) return;
                _enabled = value;
                unsafe
                {
                    var obj = (Interop.lv_obj_t*)_lvglObjectHandle;
                    if (obj != null)
                    {
                        if (_enabled)
                            lv_obj_remove_state(obj, lv_state_t.LV_STATE_DISABLED);
                        else
                            lv_obj_add_state(obj, lv_state_t.LV_STATE_DISABLED);
                    }
                }
                OnEnabledChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets a value indicating whether the control has been disposed of.
        //
        // 返回结果:
        //     true if the control has been disposed of; otherwise, false.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRDescriptionAttribute("ControlDisposedDescr")]
        public bool IsDisposed { get; }
        //
        // 摘要:
        //     Gets or sets which control borders are docked to its parent control and determines
        //     how a control is resized with its parent.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DockStyle values. The default is System.Windows.Forms.DockStyle.None.
        //
        //
        // 异常:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     The value assigned is not one of the System.Windows.Forms.DockStyle values.
        [DefaultValue(DockStyle.None)]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlDockDescr")]
        public virtual DockStyle Dock
        {
            get => _dock;
            set
            {
                if (_dock == value) return;
                _dock = value;
                unsafe
                {
                    var obj = (Interop.lv_obj_t*)_lvglObjectHandle;
                    if (obj != null)
                    {
                        if (_dock == DockStyle.Fill)
                            lv_obj_set_size(obj, LvPct(100), LvPct(100));
                        else if (_size.Width > 0 || _size.Height > 0)
                        {
                            int w = _size.Width > 0 ? _size.Width : LV_SIZE_CONTENT;
                            int h = _size.Height > 0 ? _size.Height : LV_SIZE_CONTENT;
                            lv_obj_set_size(obj, w, h);
                        }
                    }
                }
                OnDockChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets a value indicating whether the control can receive focus.
        //
        // 返回结果:
        //     true if the control can receive focus; otherwise, false.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatFocus")]
        [SRDescriptionAttribute("ControlCanFocusDescr")]
        public bool CanFocus { get; }
        //
        // 摘要:
        //     Gets or sets the size and location of the control including its nonclient elements,
        //     in pixels, relative to the parent control.
        //
        // 返回结果:
        //     A System.Drawing.Rectangle in pixels relative to the parent control that represents
        //     the size and location of the control including its nonclient elements.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlBoundsDescr")]
        public Rectangle Bounds { get; set; }
        //
        // 摘要:
        //     Gets the distance, in pixels, between the bottom edge of the control and the
        //     top edge of its container's client area.
        //
        // 返回结果:
        //     An System.Int32 representing the distance, in pixels, between the bottom edge
        //     of the control and the top edge of its container's client area.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlBottomDescr")]
        public int Bottom => _location.Y + _size.Height;
        //
        // 摘要:
        //     Gets or sets the System.Windows.Forms.BindingContext for the control.
        //
        // 返回结果:
        //     A System.Windows.Forms.BindingContext for the control.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRDescriptionAttribute("ControlBindingContextDescr")]
        public virtual BindingContext? BindingContext
        {
            get => _bindingContext;
            set
            {
                if (ReferenceEquals(_bindingContext, value)) return;
                _bindingContext = value;
                OnBindingContextChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets or sets the background image layout as defined in the System.Windows.Forms.ImageLayout
        //     enumeration.
        //
        // 返回结果:
        //     One of the values of System.Windows.Forms.ImageLayout (System.Windows.Forms.ImageLayout.Center
        //     , System.Windows.Forms.ImageLayout.None, System.Windows.Forms.ImageLayout.Stretch,
        //     System.Windows.Forms.ImageLayout.Tile, or System.Windows.Forms.ImageLayout.Zoom).
        //     System.Windows.Forms.ImageLayout.Tile is the default value.
        //
        // 异常:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     The specified enumeration value does not exist.
        [DefaultValue(ImageLayout.Tile)]
        [Localizable(true)]
        [SRCategoryAttribute("CatAppearance")]
        [SRDescriptionAttribute("ControlBackgroundImageLayoutDescr")]
        public virtual ImageLayout BackgroundImageLayout { get; set; }
        //
        // 摘要:
        //     Gets or sets the background image displayed in the control.
        //
        // 返回结果:
        //     An System.Drawing.Image that represents the image to display in the background
        //     of the control.
        [DefaultValue(null)]
        [Localizable(true)]
        [SRCategoryAttribute("CatAppearance")]
        [SRDescriptionAttribute("ControlBackgroundImageDescr")]
        public virtual Image? BackgroundImage { get; set; }
        //
        // 摘要:
        //     Gets or sets the background color for the control.
        //
        // 返回结果:
        //     A System.Drawing.Color that represents the background color of the control. The
        //     default is the value of the System.Windows.Forms.Control.DefaultBackColor property.
        [DispId(-501)]
        [SRCategoryAttribute("CatAppearance")]
        [SRDescriptionAttribute("ControlBackColorDescr")]
        public virtual Color BackColor
        {
            get => _backColor;
            set
            {
                if (_backColor == value) return;
                _backColor = value;
                unsafe
                {
                    var obj = (Interop.lv_obj_t*)_lvglObjectHandle;
                    if (obj != null && value != Color.Empty)
                        lv_obj_set_style_bg_color(obj, lv_color_make(value.R, value.G, value.B), 0);
                }
                OnBackColorChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets or sets the data context for the purpose of data binding. This is an ambient
        //     property.
        [Bindable(true)]
        [Browsable(false)]
        [SRCategoryAttribute("CatData")]
        public virtual object? DataContext
        {
            get => _dataContext;
            set
            {
                if (ReferenceEquals(_dataContext, value)) return;
                _dataContext = value;
                OnDataContextChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets a value indicating whether the control can be selected.
        //
        // 返回结果:
        //     true if the control can be selected; otherwise, false.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatFocus")]
        [SRDescriptionAttribute("ControlCanSelectDescr")]
        public bool CanSelect { get; }
        //
        // 摘要:
        //     Gets a cached instance of the control's layout engine.
        //
        // 返回结果:
        //     The System.Windows.Forms.Layout.LayoutEngine for the control's contents.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual LayoutEngine LayoutEngine { get; protected set; }
        //
        // 摘要:
        //     This property is not relevant for this class.
        //
        // 返回结果:
        //     true if enabled; otherwise, false.
        [Browsable(false)]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlAutoSizeDescr")]
        public virtual bool AutoSize
        {
            get => _autoSize;
            set
            {
                if (_autoSize == value) return;
                _autoSize = value;
                OnAutoSizeChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets or sets the edges of the container to which a control is bound and determines
        //     how a control is resized with its parent.
        //
        // 返回结果:
        //     A bitwise combination of the System.Windows.Forms.AnchorStyles values. The default
        //     is Top and Left.
        [DefaultValue(AnchorStyles.Top | AnchorStyles.Left)]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlAnchorDescr")]
        public virtual AnchorStyles Anchor
        {
            get => _anchor;
            set
            {
                if (_anchor == value) return;
                _anchor = value;
                // Anchor doesn't have its own event in WinForms
            }
        }
        //
        // 摘要:
        //     Gets or sets a value indicating whether the control can accept data that the
        //     user drags onto it.
        //
        // 返回结果:
        //     true if drag-and-drop operations are allowed in the control; otherwise, false.
        //     The default is false.
        [DefaultValue(false)]
        [SRCategoryAttribute("CatBehavior")]
        [SRDescriptionAttribute("ControlAllowDropDescr")]
        public virtual bool AllowDrop { get; set; }

        //
        // 摘要:
        //     Gets or sets the name of the control used by accessibility client applications.
        //
        //
        // 返回结果:
        //     The name of the control used by accessibility client applications. The default
        //     is null.
        [DefaultValue(null)]
        [Localizable(true)]
        [SRCategoryAttribute("CatAccessibility")]
        [SRDescriptionAttribute("ControlAccessibleNameDescr")]
        public string? AccessibleName { get; set; }
        //
        // 摘要:
        //     Gets or sets the description of the control used by accessibility client applications.
        //
        //
        // 返回结果:
        //     The description of the control used by accessibility client applications. The
        //     default is null.
        [DefaultValue(null)]
        [Localizable(true)]
        [SRCategoryAttribute("CatAccessibility")]
        [SRDescriptionAttribute("ControlAccessibleDescriptionDescr")]
        public string? AccessibleDescription { get; set; }
        //
        // 摘要:
        //     Gets or sets the default action description of the control for use by accessibility
        //     client applications.
        //
        // 返回结果:
        //     The default action description of the control for use by accessibility client
        //     applications.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatAccessibility")]
        [SRDescriptionAttribute("ControlAccessibleDefaultActionDescr")]
        public string? AccessibleDefaultActionDescription { get; set; }

        //
        // 摘要:
        //     Gets or sets where this control is scrolled to in System.Windows.Forms.ScrollableControl.ScrollControlIntoView(System.Windows.Forms.Control).
        //
        //
        // 返回结果:
        //     A System.Drawing.Point specifying the scroll location. The default is the upper-left
        //     corner of the control.
        [Browsable(false)]
        [DefaultValue(typeof(Point), "0, 0")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual Point AutoScrollOffset { get; set; }
        //
        // 摘要:
        //     Gets or sets a value indicating whether the control has captured the mouse.
        //
        // 返回结果:
        //     true if the control has captured the mouse; otherwise, false.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatFocus")]
        [SRDescriptionAttribute("ControlCaptureDescr")]
        public bool Capture { get; set; }
        //
        // 摘要:
        //     Gets or sets a value indicating whether the control causes validation to be performed
        //     on any controls that require validation when it receives focus.
        //
        // 返回结果:
        //     true if the control causes validation to be performed on any controls requiring
        //     validation when it receives focus; otherwise, false. The default is true.
        [DefaultValue(true)]
        [SRCategoryAttribute("CatFocus")]
        [SRDescriptionAttribute("ControlCausesValidationDescr")]
        public bool CausesValidation
        {
            get => _causesValidation;
            set
            {
                if (_causesValidation == value) return;
                _causesValidation = value;
                OnCausesValidationChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets or sets the space between controls.
        //
        // 返回结果:
        //     A System.Windows.Forms.Padding representing the space between controls.
        [Localizable(true)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlMarginDescr")]
        public Padding Margin
        {
            get => _margin;
            set
            {
                _margin = value;
                OnMarginChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets or sets the height and width of the client area of the control.
        //
        // 返回结果:
        //     A System.Drawing.Size that represents the dimensions of the client area of the
        //     control.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlClientSizeDescr")]
        public Size ClientSize { get; set; }
        //
        // 摘要:
        //     Gets the name of the company or creator of the application containing the control.
        //
        //
        // 返回结果:
        //     The company name or creator of the application containing the control.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRDescriptionAttribute("ControlCompanyNameDescr")]
        public string CompanyName { get; private set; }
        //
        // 摘要:
        //     Gets a value indicating whether the base System.Windows.Forms.Control class is
        //     in the process of disposing.
        //
        // 返回结果:
        //     true if the base System.Windows.Forms.Control class is in the process of disposing;
        //     otherwise, false.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRDescriptionAttribute("ControlDisposingDescr")]
        public bool Disposing { get; }
        //
        // 摘要:
        //     Gets a value indicating whether the control, or one of its child controls, currently
        //     has the input focus.
        //
        // 返回结果:
        //     true if the control or one of its child controls currently has the input focus;
        //     otherwise, false.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRDescriptionAttribute("ControlContainsFocusDescr")]
        public bool ContainsFocus { get; }
        //
        // 摘要:
        //     Gets or sets the System.Windows.Forms.ContextMenuStrip associated with this control.
        //
        //
        // 返回结果:
        //     The System.Windows.Forms.ContextMenuStrip for this control, or null if there
        //     is no System.Windows.Forms.ContextMenuStrip. The default is null.
        [DefaultValue(null)]
        [SRCategoryAttribute("CatBehavior")]
        [SRDescriptionAttribute("ControlContextMenuDescr")]
        public virtual ContextMenuStrip? ContextMenuStrip
        {
            get => _contextMenuStrip;
            set
            {
                if (ReferenceEquals(_contextMenuStrip, value)) return;
                _contextMenuStrip = value;
                OnContextMenuStripChanged(EventArgs.Empty);
            }
        }

        
        //
        // 摘要:
        //     Gets the collection of controls contained within the control.
        //
        // 返回结果:
        //     A System.Windows.Forms.Control.ControlCollection representing the collection
        //     of controls contained within the control.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [SRDescriptionAttribute("ControlControlsDescr")]
        public ControlCollection Controls { get; }
        //
        // 摘要:
        //     Gets a value indicating whether the control has been created.
        //
        // 返回结果:
        //     true if the control has been created; otherwise, false.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRDescriptionAttribute("ControlCreatedDescr")]
        public bool Created { get; }
        //
        // 摘要:
        //     Gets or sets the size that is the upper limit that System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size)
        //     can specify.
        //
        // 返回结果:
        //     An ordered pair of type System.Drawing.Size representing the width and height
        //     of a rectangle.
        [AmbientValue(typeof(Size), "0, 0")]
        [Localizable(true)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlMaximumSizeDescr")]
        public virtual Size MaximumSize { get; set; }
        //
        // 摘要:
        //     Gets or sets the cursor that is displayed when the mouse pointer is over the
        //     control.
        //
        // 返回结果:
        //     A System.Windows.Forms.Cursor that represents the cursor to display when the
        //     mouse pointer is over the control.
        [AmbientValue(null)]
        [SRCategoryAttribute("CatAppearance")]
        [SRDescriptionAttribute("ControlCursorDescr")]
        public virtual Cursor Cursor
        {
            get => _cursor!;
            set
            {
                if (ReferenceEquals(_cursor, value)) return;
                _cursor = value;
                OnCursorChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Gets the data bindings for the control.
        //
        // 返回结果:
        //     A System.Windows.Forms.ControlBindingsCollection that contains the System.Windows.Forms.Binding
        //     objects for the control.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [ParenthesizePropertyName(true)]
        [RefreshProperties(RefreshProperties.All)]
        [SRCategoryAttribute("CatData")]
        [SRDescriptionAttribute("ControlBindingsDescr")]
        public ControlBindingsCollection DataBindings { get; private set; }
        //
        // 摘要:
        //     Gets the rectangle that represents the client area of the control.
        //
        // 返回结果:
        //     A System.Drawing.Rectangle that represents the client area of the control.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlClientRectangleDescr")]
        public Rectangle ClientRectangle { get; }
        //
        // 摘要:
        //     Gets the rectangle that represents the display area of the control.
        //
        // 返回结果:
        //     A System.Drawing.Rectangle that represents the display area of the control.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRDescriptionAttribute("ControlDisplayRectangleDescr")]
        public virtual Rectangle DisplayRectangle { get; }
        //
        // 摘要:
        //     Gets the DPI value for the display device where the control is currently being
        //     displayed.
        //
        // 返回结果:
        //     The DPI value of the display device.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public int DeviceDpi { get; }
        //
        // 摘要:
        //     Gets or sets the height of the font of the control.
        //
        // 返回结果:
        //     The height of the System.Drawing.Font of the control in pixels.
        protected int FontHeight { get; set; }
        //
        // 摘要:
        //     Gets the default size of the control.
        //
        // 返回结果:
        //     The default System.Drawing.Size of the control.
        protected virtual Size DefaultSize { get; }
        //
        // 摘要:
        //     Gets or sets the default cursor for the control.
        //
        // 返回结果:
        //     An object of type System.Windows.Forms.Cursor representing the current default
        //     cursor.
        protected virtual Cursor DefaultCursor { get; private set; }
        //
        // 摘要:
        //     Gets the length and height, in pixels, that is specified as the default minimum
        //     size of a control.
        //
        // 返回结果:
        //     A System.Drawing.Size representing the size of the control.
        protected virtual Size DefaultMinimumSize { get; }
        //
        // 摘要:
        //     Gets the length and height, in pixels, that is specified as the default maximum
        //     size of a control.
        //
        // 返回结果:
        //     A System.Drawing.Point.#ctor(System.Drawing.Size) representing the size of the
        //     control.
        protected virtual Size DefaultMaximumSize { get; }
        //
        // 摘要:
        //     Gets the space, in pixels, that is specified by default between controls.
        //
        // 返回结果:
        //     A System.Windows.Forms.Padding that represents the default space between controls.
        protected virtual Padding DefaultMargin { get; }
        //
        // 摘要:
        //     Gets the required creation parameters when the control handle is created.
        //
        // 返回结果:
        //     A System.Windows.Forms.CreateParams that contains the required creation parameters
        //     when the handle to the control is created.
        protected virtual CreateParams CreateParams { get; private set; }
        //
        // 摘要:
        //     Determines if events can be raised on the control.
        //
        // 返回结果:
        //     true if the control is hosted as an ActiveX control whose events are not frozen;
        //     otherwise, false.
        protected override bool CanRaiseEvents { get; }
        //
        // 摘要:
        //     Gets the default internal spacing, in pixels, of the contents of a control.
        //
        // 返回结果:
        //     The default internal spacing of the contents of a control.
        protected virtual Padding DefaultPadding { get; }
        //
        // 摘要:
        //     Gets or sets a value indicating whether this control should redraw its surface
        //     using a secondary buffer to reduce or prevent flicker.
        //
        // 返回结果:
        //     true if the surface of the control should be drawn using double buffering; otherwise,
        //     false.
        [SRCategoryAttribute("CatBehavior")]
        [SRDescriptionAttribute("ControlDoubleBufferedDescr")]
        protected virtual bool DoubleBuffered { get; set; }
        //
        // 摘要:
        //     Gets or sets a value indicating whether the control redraws itself when resized.
        //
        //
        // 返回结果:
        //     true if the control redraws itself when resized; otherwise, false.
        [SRDescriptionAttribute("ControlResizeRedrawDescr")]
        protected bool ResizeRedraw { get; set; }
        //
        // 摘要:
        //     Gets a value that determines the scaling of child controls.
        //
        // 返回结果:
        //     true if child controls will be scaled when the System.Windows.Forms.Control.Scale(System.Single)
        //     method on this control is called; otherwise, false. The default is true.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual bool ScaleChildren { get; }
        //
        // 摘要:
        //     Gets a value indicating whether the System.Windows.Forms.Control.ImeMode property
        //     can be set to an active value, to enable IME support.
        //
        // 返回结果:
        //     true in all cases.
        protected virtual bool CanEnableIme { get; }

        //
        // 摘要:
        //     Gets a value indicating whether the user interface is in the appropriate state
        //     to show or hide keyboard accelerators.
        //
        // 返回结果:
        //     true if the keyboard accelerators are visible; otherwise, false.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected internal virtual bool ShowKeyboardCues { get; }
        //
        // 摘要:
        //     Gets a value indicating whether the control should display focus rectangles.
        //
        //
        // 返回结果:
        //     true if the control should display focus rectangles; otherwise, false.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected internal virtual bool ShowFocusCues { get; }
        //
        // 摘要:
        //     This property is now obsolete.
        //
        // 返回结果:
        //     true if the control is rendered from right to left; otherwise, false. The default
        //     is false.
        [Obsolete("This property has been deprecated. Please use RightToLeft instead. https://go.microsoft.com/fwlink/?linkid=14202")]
        protected internal bool RenderRightToLeft { get; }

        //
        // 摘要:
        //     Occurs when the system colors change.
        [SRCategoryAttribute("CatBehavior")]
        [SRDescriptionAttribute("ControlOnSystemColorsChangedDescr")]
        public event EventHandler? SystemColorsChanged;
        //
        // 摘要:
        //     Occurs when a control should reposition its child controls.
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlOnLayoutDescr")]
        public event LayoutEventHandler? Layout;
        //
        // 摘要:
        //     Occurs when a key is released while the control has focus.
        [SRCategoryAttribute("CatKey")]
        [SRDescriptionAttribute("ControlOnKeyUpDescr")]
        public event KeyEventHandler? KeyUp;
        //
        // 摘要:
        //     Occurs when a character, space, or backspace key is pressed while the control
        //     has focus.
        [SRCategoryAttribute("CatKey")]
        [SRDescriptionAttribute("ControlOnKeyPressDescr")]
        public event KeyPressEventHandler? KeyPress;
        //
        // 摘要:
        //     Occurs when a key is pressed while the control has focus.
        [SRCategoryAttribute("CatKey")]
        [SRDescriptionAttribute("ControlOnKeyDownDescr")]
        public event KeyEventHandler? KeyDown;
        //
        // 摘要:
        //     Occurs before a key event when a key is pressed while the control has focus.
        [SRCategoryAttribute("CatKey")]
        [SRDescriptionAttribute("ControlOnPreviewKeyDownDescr")]
        public event EventHandler<PreviewKeyDownEventArgs>? PreviewKeyDown;
        //
        // 摘要:
        //     Occurs when the control receives focus.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatFocus")]
        [SRDescriptionAttribute("ControlOnGotFocusDescr")]
        public event EventHandler? GotFocus;
        //
        // 摘要:
        //     Occurs when the control is entered.
        [SRCategoryAttribute("CatFocus")]
        [SRDescriptionAttribute("ControlOnEnterDescr")]
        public event EventHandler? Enter;
        //
        // 摘要:
        //     Occurs when the control is redrawn.
        [SRCategoryAttribute("CatAppearance")]
        [SRDescriptionAttribute("ControlOnPaintDescr")]
#pragma warning disable CS0067
        public event PaintEventHandler? Paint;
#pragma warning restore CS0067

        //
        // 摘要:
        //     Occurs when the input focus leaves the control.
        [SRCategoryAttribute("CatFocus")]
        [SRDescriptionAttribute("ControlOnLeaveDescr")]
        public event EventHandler? Leave;
        //
        // 摘要:
        //     Occurs when the control's padding changes.
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlOnPaddingChangedDescr")]
        public event EventHandler? PaddingChanged;
        //
        // 摘要:
        //     Occurs when a control's display requires redrawing.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatAppearance")]
        [SRDescriptionAttribute("ControlOnInvalidateDescr")]
#pragma warning disable CS0067
        public event InvalidateEventHandler? Invalidated;
#pragma warning restore CS0067
        //
        // 摘要:
        //     Occurs when the control is double-clicked.
        [SRCategoryAttribute("CatAction")]
        [SRDescriptionAttribute("ControlOnDoubleClickDescr")]
        public event EventHandler? DoubleClick;
        //
        // 摘要:
        //     Occurs when the control loses focus.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatFocus")]
        [SRDescriptionAttribute("ControlOnLostFocusDescr")]
        public event EventHandler? LostFocus;
        //
        // 摘要:
        //     Occurs when the control loses mouse capture.
        [SRCategoryAttribute("CatAction")]
        [SRDescriptionAttribute("ControlOnMouseCaptureChangedDescr")]
        public event EventHandler? MouseCaptureChanged;
        //
        // 摘要:
        //     Occurs when the control is double clicked by the mouse.
        [SRCategoryAttribute("CatAction")]
        [SRDescriptionAttribute("ControlOnMouseDoubleClickDescr")]
        public event MouseEventHandler? MouseDoubleClick;

        //
        // 摘要:
        //     Occurs when the mouse pointer is over the control and a mouse button is pressed.
        [SRCategoryAttribute("CatMouse")]
        [SRDescriptionAttribute("ControlOnMouseDownDescr")]
        public event MouseEventHandler? MouseDown;
        //
        // 摘要:
        //     Occurs when the mouse pointer enters the control.
        [SRCategoryAttribute("CatMouse")]
        [SRDescriptionAttribute("ControlOnMouseEnterDescr")]
        public event EventHandler? MouseEnter;
        //
        // 摘要:
        //     Occurs when the mouse pointer leaves the control.
        [SRCategoryAttribute("CatMouse")]
        [SRDescriptionAttribute("ControlOnMouseLeaveDescr")]
        public event EventHandler? MouseLeave;
        //
        // 摘要:
        //     Occurs when the DPI setting for a control is changed programmatically before
        //     a DPI change event for its parent control or form has occurred.
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlOnDpiChangedBeforeParentDescr")]
#pragma warning disable CS0067
        public event EventHandler? DpiChangedBeforeParent;
#pragma warning restore CS0067
        //
        // 摘要:
        //     Occurs when the mouse pointer is moved over the control.
        [SRCategoryAttribute("CatMouse")]
        [SRDescriptionAttribute("ControlOnMouseMoveDescr")]
        public event MouseEventHandler? MouseMove;
        //
        // 摘要:
        //     Occurs when the mouse pointer is over the control and a mouse button is released.
        [SRCategoryAttribute("CatMouse")]
        [SRDescriptionAttribute("ControlOnMouseUpDescr")]
        public event MouseEventHandler? MouseUp;
        //
        // 摘要:
        //     Occurs when the mouse wheel moves while the control has focus.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatMouse")]
        [SRDescriptionAttribute("ControlOnMouseWheelDescr")]
        public event MouseEventHandler? MouseWheel;
        //
        // 摘要:
        //     Occurs when the control is moved.
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlOnMoveDescr")]
        public event EventHandler? Move;

        //
        // 摘要:
        //     Occurs when the control is resized.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlOnResizeDescr")]
        public event EventHandler? Resize;

        //
        // 摘要:
        //     Occurs when the control style changes.
        [SRCategoryAttribute("CatBehavior")]
        [SRDescriptionAttribute("ControlOnStyleChangedDescr")]
        public event EventHandler? StyleChanged;
        //
        // 摘要:
        //     Occurs when the control is clicked by the mouse.
        [SRCategoryAttribute("CatAction")]
        [SRDescriptionAttribute("ControlOnMouseClickDescr")]
        public event MouseEventHandler? MouseClick;
        //
        // 摘要:
        //     Occurs when the control's handle is in the process of being destroyed.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatPrivate")]
        [SRDescriptionAttribute("ControlOnDestroyHandleDescr")]
        public event EventHandler? HandleDestroyed;

        //
        // 摘要:
        //     This event is not relevant for this class.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnAutoSizeChangedDescr")]
        public event EventHandler? AutoSizeChanged;
        //
        // 摘要:
        //     Occurs when the value of the System.Windows.Forms.Control.BackColor property
        //     changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnBackColorChangedDescr")]
        public event EventHandler? BackColorChanged;
        //
        // 摘要:
        //     Occurs when the value of the System.Windows.Forms.Control.BackgroundImage property
        //     changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnBackgroundImageChangedDescr")]
        public event EventHandler? BackgroundImageChanged;
        //
        // 摘要:
        //     Occurs when the System.Windows.Forms.Control.BackgroundImageLayout property changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnBackgroundImageLayoutChangedDescr")]
        public event EventHandler? BackgroundImageLayoutChanged;
        //
        // 摘要:
        //     Occurs when the value of the System.Windows.Forms.BindingContext property changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnBindingContextChangedDescr")]
        public event EventHandler? BindingContextChanged;
        //
        // 摘要:
        //     Occurs when the value of the System.Windows.Forms.Control.CausesValidation property
        //     changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnCausesValidationChangedDescr")]
        public event EventHandler? CausesValidationChanged;
        //
        // 摘要:
        //     Occurs when the value of the System.Windows.Forms.Control.ClientSize property
        //     changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnClientSizeChangedDescr")]
        public event EventHandler? ClientSizeChanged;
        //
        // 摘要:
        //     Occurs when the value of the System.Windows.Forms.Control.ContextMenuStrip property
        //     changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlContextMenuStripChangedDescr")]
        public event EventHandler? ContextMenuStripChanged;
        //
        // 摘要:
        //     Occurs when the value of the System.Windows.Forms.Control.Cursor property changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnCursorChangedDescr")]
        public event EventHandler? CursorChanged;
        //
        // 摘要:
        //     Occurs when the value of the System.Windows.Forms.Control.Dock property changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnDockChangedDescr")]
        public event EventHandler? DockChanged;
        //
        // 摘要:
        //     Occurs when the System.Windows.Forms.Control.Enabled property value has changed.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnEnabledChangedDescr")]
        public event EventHandler? EnabledChanged;
        //
        // 摘要:
        //     Occurs when the System.Windows.Forms.Control.Font property value changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnFontChangedDescr")]
        public event EventHandler? FontChanged;
        //
        // 摘要:
        //     Occurs when the System.Windows.Forms.Control.ForeColor property value changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnForeColorChangedDescr")]
        public event EventHandler? ForeColorChanged;
        //
        // 摘要:
        //     Occurs when the value of the System.Windows.Forms.Control.Location property value has changed.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnLocationChangedDescr")]
        public event EventHandler? LocationChanged;
        //
        // 摘要:
        //     Occurs when the control's margin changes.
        [SRCategoryAttribute("CatLayout")]
        [SRDescriptionAttribute("ControlOnMarginChangedDescr")]
        public event EventHandler? MarginChanged;
        //
        // 摘要:
        //     Occurs when the value of the System.Windows.Forms.Control.Region property changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlRegionChangedDescr")]
        public event EventHandler? RegionChanged;
        //
        // 摘要:
        //     Occurs when the System.Windows.Forms.Control.RightToLeft property value changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnRightToLeftChangedDescr")]
        public event EventHandler? RightToLeftChanged;
        //
        // 摘要:
        //     Occurs when the System.Windows.Forms.Control.Size property value changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnSizeChangedDescr")]
        public event EventHandler? SizeChanged;
        //
        // 摘要:
        //     Occurs when the System.Windows.Forms.Control.TabIndex property value changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnTabIndexChangedDescr")]
        public event EventHandler? TabIndexChanged;
        //
        // 摘要:
        //     Occurs when the System.Windows.Forms.Control.TabStop property value changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnTabStopChangedDescr")]
        public event EventHandler? TabStopChanged;
        //
        // 摘要:
        //     Occurs when the System.Windows.Forms.Control.Text property value changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnTextChangedDescr")]
        public event EventHandler? TextChanged;
        //
        // 摘要:
        //     Occurs when the System.Windows.Forms.Control.Visible property value changes.
        [SRCategoryAttribute("CatPropertyChanged")]
        [SRDescriptionAttribute("ControlOnVisibleChangedDescr")]
        public event EventHandler? VisibleChanged;
        //
        // 摘要:
        //     Occurs when the control is clicked.
        [SRCategoryAttribute("CatAction")]
        [SRDescriptionAttribute("ControlOnClickDescr")]
        public event EventHandler? Click;
        //
        // 摘要:
        //     Occurs when a new control is added to the System.Windows.Forms.Control.ControlCollection.
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatBehavior")]
        [SRDescriptionAttribute("ControlOnControlAddedDescr")]
        public event ControlEventHandler? ControlAdded;
        //
        // 摘要:
        //     Occurs when a control is removed from the System.Windows.Forms.Control.ControlCollection.
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatBehavior")]
        [SRDescriptionAttribute("ControlOnControlRemovedDescr")]
        public event ControlEventHandler? ControlRemoved;
        //
        // 摘要:
        //     Occurs when the value of the System.Windows.Forms.Control.DataContext property
        //     changes.
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SRCategoryAttribute("CatData")]
        [SRDescriptionAttribute("ControlDataContextChangedDescr")]
        public event EventHandler? DataContextChanged;

        //
        // 摘要:
        //     Occurs when an object is dragged out of the control's bounds.
        [SRCategoryAttribute("CatDragDrop")]
        [SRDescriptionAttribute("ControlOnDragLeaveDescr")]
#pragma warning disable CS0067
        public event EventHandler? DragLeave;
#pragma warning restore CS0067
        //
        // 摘要:
        //     Occurs when a handle is created for the control.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event EventHandler? HandleCreated;
        //
        // 摘要:
        //     Occurs when the DPI setting for a control is changed programmatically after the
        //     DPI of its parent control or form has changed.
#pragma warning disable CS0067
        public event EventHandler? DpiChangedAfterParent;
#pragma warning restore CS0067
        //
        // 摘要:
        //     Occurs when the mouse pointer rests on the control.
        public event EventHandler? MouseHover;
        //
        // 摘要:
        //     Occurs when the System.Windows.Forms.Control.Parent property value changes.
        public event EventHandler? ParentChanged;
        //
        // 摘要:
        //     Occurs when the control is finished validating.
        public event EventHandler? Validated;
        //
        // 摘要:
        //     Occurs when the control is validating.
        public event CancelEventHandler? Validating;
#nullable disable
        //
        // 摘要:
        //     Occurs when the value of the System.Windows.Forms.Control.ContextMenu property
        //     changes.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("`ContextMenu` is provided for binary compatibility with .NET Framework and is not intended to be used directly from your code. Use `ContextMenuStrip` instead.", false, DiagnosticId = "WFDEV006", UrlFormat = "https://aka.ms/winforms-warnings/{0}")]
#pragma warning disable CS0067
        public event EventHandler ContextMenuChanged;
#pragma warning restore CS0067
#nullable enable
        //
        // 摘要:
        //     Occurs when the System.Windows.Forms.Control.ImeMode property has changed.
#pragma warning disable CS0067
        public event EventHandler? ImeModeChanged;

        private sealed class NullViewTarget : IViewTarget
        {
            public static readonly NullViewTarget Instance = new();

            public unsafe lv_obj_t* Root => null;

            public unsafe lv_group_t* KeyInputGroup => null;

            public unsafe delegate* unmanaged[Cdecl]<lv_event_t*, void> SendTextAreaFocusCallback => null;

            public void Open() { }

            public void HandleEvents() { }

            public void RunLoop(Action iteration) { }

            public void Close() { }

            public unsafe void RegisterTextInput(lv_obj_t* textArea) { }

            public void Dispose() { }
        }
#pragma warning restore CS0067



        //
        // 摘要:
        //     Brings the control to the front of the z-order.
        public void BringToFront()
        {

        }
        //
        // 摘要:
        //     Retrieves a value indicating whether the specified control is a child of the
        //     control.
        //
        // 参数:
        //   ctl:
        //     The System.Windows.Forms.Control to evaluate.
        //
        // 返回结果:
        //     true if the specified control is a child of the control; otherwise, false.
        public bool Contains([NotNullWhen(true)] Control? ctl)
        {
            return ctl is not null && Controls.Contains(ctl);
        }
        //
        // 摘要:
        //     Forces the creation of the visible control, including the creation of the handle
        //     and any visible child controls.
        public void CreateControl()
        {
            if (_lvglObjectHandle != 0)
            {
                return;
            }

            if (Parent is not null && Parent._lvglObjectHandle != 0)
            {
                CreateLvglObject(Parent._lvglObjectHandle);
            }

            foreach (var child in Controls)
            {
                if (child.Visible)
                {
                    child.CreateControl();
                }
            }

            OnCreateControl();
        }

        //
        // 摘要:
        //     Retrieves the form that the control is on.
        //
        // 返回结果:
        //     The System.Windows.Forms.Form that the control is on.
        public Form? FindForm()
        {
            for (Control? current = this; current is not null; current = current.Parent)
            {
                if (current is Form form)
                {
                    return form;
                }
            }

            return this as Form;
        }
        //
        // 摘要:
        //     Sets input focus to the control.
        //
        // 返回结果:
        //     true if the input focus request was successful; otherwise, false.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool Focus()
        {
            return default(bool);
        }


        //
        // 摘要:
        //     Forces the control to apply layout logic to all its child controls.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void PerformLayout()
        {
            PerformLayout(null, null);
        }
        //
        // 摘要:
        //     Forces the control to apply layout logic to all its child controls.
        //
        // 参数:
        //   affectedControl:
        //     A System.Windows.Forms.Control that represents the most recently changed control.
        //
        //
        //   affectedProperty:
        //     The name of the most recently changed property on the control.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void PerformLayout(Control? affectedControl, string? affectedProperty)
        {
            if (_layoutSuspendCount > 0)
            {
                _layoutPending = true;
                _layoutAffectedControl = affectedControl;
                _layoutAffectedProperty = affectedProperty;
                return;
            }

            if (this is TableLayoutPanel tableLayoutPanel)
            {
                tableLayoutPanel.PerformTableLayout();
            }
            else if (this is FlowLayoutPanel flowLayoutPanel)
            {
                flowLayoutPanel.PerformFlowLayout();
            }
            else
            {
                PerformDockAndAnchorLayout();
            }

            OnLayout(new LayoutEventArgs(affectedControl, affectedProperty));
        }
        //
        // 摘要:
        //     Computes the location of the specified screen point into client coordinates.
        //
        //
        // 参数:
        //   p:
        //     The screen coordinate System.Drawing.Point to convert.
        //
        // 返回结果:
        //     A System.Drawing.Point that represents the converted System.Drawing.Point, p,
        //     in client coordinates.
        public Point PointToClient(Point p)
        {
            return p;
        }
        //
        // 摘要:
        //     Computes the location of the specified client point into screen coordinates.
        //
        //
        // 参数:
        //   p:
        //     The client coordinate System.Drawing.Point to convert.
        //
        // 返回结果:
        //     A System.Drawing.Point that represents the converted System.Drawing.Point, p,
        //     in screen coordinates.
        public Point PointToScreen(Point p)
        {
            return p;
        }
        //
        // 摘要:
        //     Preprocesses keyboard or input messages within the message loop before they are
        //     dispatched.
        //
        // 参数:
        //   msg:
        //     A System.Windows.Forms.Message that represents the message to process.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.PreProcessControlState values, depending on whether
        //     System.Windows.Forms.Control.PreProcessMessage(System.Windows.Forms.Message@)
        //     is true or false and whether System.Windows.Forms.Control.IsInputKey(System.Windows.Forms.Keys)
        //     or System.Windows.Forms.Control.IsInputChar(System.Char) are true or false.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public PreProcessControlState PreProcessControlMessage(ref Message msg)
        {
            if (_lvglObjectHandle == 0)
            {
                return PreProcessControlState.MessageNotNeeded;
            }

            if (PreProcessMessage(ref msg))
            {
                return PreProcessControlState.MessageProcessed;
            }

            return PreProcessControlState.MessageNotNeeded;
        }

        protected internal static Keys TranslateLvglKey(uint lvglKey)
        {
            Keys key = lvglKey switch
            {
                (uint)lv_key_t.LV_KEY_LEFT => Keys.Left,
                (uint)lv_key_t.LV_KEY_RIGHT => Keys.Right,
                (uint)lv_key_t.LV_KEY_UP => Keys.Up,
                (uint)lv_key_t.LV_KEY_DOWN => Keys.Down,
                (uint)lv_key_t.LV_KEY_BACKSPACE => Keys.Back,
                (uint)lv_key_t.LV_KEY_DEL => Keys.Delete,
                (uint)lv_key_t.LV_KEY_ENTER => Keys.Enter,
                (uint)lv_key_t.LV_KEY_ESC => Keys.Escape,
                (uint)lv_key_t.LV_KEY_NEXT => Keys.Tab,
                _ => lvglKey <= 0xFFFF ? (Keys)lvglKey : Keys.None,
            };

            return key | LVGLSharp.Forms.ModifierKeys.Current;
        }

        protected internal static char TranslateLvglKeyChar(uint lvglKey)
        {
            return lvglKey switch
            {
                (uint)lv_key_t.LV_KEY_ENTER => '\n',
                (uint)lv_key_t.LV_KEY_BACKSPACE => '\b',
                (uint)lv_key_t.LV_KEY_NEXT => '\t',
                _ when lvglKey >= 32 && lvglKey <= 126 => (char)lvglKey,
                _ => '\0',
            };
        }
        //
        // 摘要:
        //     Preprocesses keyboard or input messages within the message loop before they are
        //     dispatched.
        //
        // 参数:
        //   msg:
        //     A System.Windows.Forms.Message, passed by reference, that represents the message
        //     to process. The possible values are WM_KEYDOWN, WM_SYSKEYDOWN, WM_CHAR, and WM_SYSCHAR.
        //
        //
        // 返回结果:
        //     true if the message was processed by the control; otherwise, false.
        public virtual bool PreProcessMessage(ref Message msg)
        {
            return ProcessKeyMessage(ref msg);
        }
        //
        // 摘要:
        //     Computes the size and location of the specified screen rectangle in client coordinates.
        //
        //
        // 参数:
        //   r:
        //     The screen coordinate System.Drawing.Rectangle to convert.
        //
        // 返回结果:
        //     A System.Drawing.Rectangle that represents the converted System.Drawing.Rectangle,
        //     r, in client coordinates.
        public Rectangle RectangleToClient(Rectangle r)
        {
            return default(Rectangle);
        }
        //
        // 摘要:
        //     Computes the size and location of the specified client rectangle in screen coordinates.
        //
        //
        // 参数:
        //   r:
        //     The client coordinate System.Drawing.Rectangle to convert.
        //
        // 返回结果:
        //     A System.Drawing.Rectangle that represents the converted System.Drawing.Rectangle,
        //     p, in screen coordinates.
        public Rectangle RectangleToScreen(Rectangle r)
        {
            return default(Rectangle);
        }
        //
        // 摘要:
        //     Forces the control to invalidate its client area and immediately redraw itself
        //     and any child controls.
        public virtual void Refresh()
        {

        }
        //
        // 摘要:
        //     Resets the System.Windows.Forms.Control.BackColor property to its default value.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void ResetBackColor()
        {

        }
        //
        // 摘要:
        //     Causes a control bound to the System.Windows.Forms.BindingSource to reread all
        //     the items in the list and refresh their displayed values.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBindings()
        {

        }
        //
        // 摘要:
        //     Resets the System.Windows.Forms.Control.Cursor property to its default value.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void ResetCursor()
        {

        }
        //
        // 摘要:
        //     Resets the System.Windows.Forms.Control.Font property to its default value.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void ResetFont()
        {

        }
        //
        // 摘要:
        //     Resets the System.Windows.Forms.Control.ForeColor property to its default value.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void ResetForeColor()
        {

        }
        //
        // 摘要:
        //     Resets the System.Windows.Forms.Control.ImeMode property to its default value.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetImeMode()
        {

        }
        //
        // 摘要:
        //     Resets the System.Windows.Forms.Control.RightToLeft property to its default value.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void ResetRightToLeft()
        {

        }
        //
        // 摘要:
        //     Resets the System.Windows.Forms.Control.Text property to its default value (System.String.Empty).
        public virtual void ResetText()
        {
            Text = string.Empty;
            OnTextChanged(EventArgs.Empty);
        }
        //
        // 摘要:
        //     Resumes usual layout logic, optionally forcing an immediate layout of pending
        //     layout requests.
        //
        // 参数:
        //   performLayout:
        //     true to execute pending layout requests; otherwise, false.
        public void ResumeLayout(bool performLayout)
        {
            if (_layoutSuspendCount > 0)
            {
                _layoutSuspendCount--;
            }

            if (_layoutSuspendCount == 0 && (performLayout || _layoutPending))
            {
                var affectedControl = _layoutAffectedControl;
                var affectedProperty = _layoutAffectedProperty;
                _layoutPending = false;
                _layoutAffectedControl = null;
                _layoutAffectedProperty = null;
                PerformLayout(affectedControl, affectedProperty);
            }
        }
        //
        // 摘要:
        //     Resumes usual layout logic.
        public void ResumeLayout()
        {
            ResumeLayout(true);
        }

        //
        // 摘要:
        //     Scales the control and all child controls by the specified scaling factor.
        //
        // 参数:
        //   factor:
        //     A System.Drawing.SizeF containing the horizontal and vertical scaling factors.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void Scale(SizeF factor)
        {

        }

        //
        // 摘要:
        //     Activates the control.
        public void Select()
        {

        }
        //
        // 摘要:
        //     Activates the next control.
        //
        // 参数:
        //   ctl:
        //     The System.Windows.Forms.Control at which to start the search.
        //
        //   forward:
        //     true to move forward in the tab order; false to move backward in the tab order.
        //
        //
        //   tabStopOnly:
        //     true to ignore the controls with the System.Windows.Forms.Control.TabStop property
        //     set to false; otherwise, false.
        //
        //   nested:
        //     true to include nested (children of child controls) child controls; otherwise,
        //     false.
        //
        //   wrap:
        //     true to continue searching from the first control in the tab order after the
        //     last control has been reached; otherwise, false.
        //
        // 返回结果:
        //     true if a control was activated; otherwise, false.
        public bool SelectNextControl(Control? ctl, bool forward, bool tabStopOnly, bool nested, bool wrap)
        {
            return default(bool);
        }
        //
        // 摘要:
        //     Sends the control to the back of the z-order.
        public void SendToBack()
        {

        }
        //
        // 摘要:
        //     Sets the bounds of the control to the specified location and size.
        //
        // 参数:
        //   x:
        //     The new System.Windows.Forms.Control.Left property value of the control.
        //
        //   y:
        //     The new System.Windows.Forms.Control.Top property value of the control.
        //
        //   width:
        //     The new System.Windows.Forms.Control.Width property value of the control.
        //
        //   height:
        //     The new System.Windows.Forms.Control.Height property value of the control.
        public void SetBounds(int x, int y, int width, int height)
        {
            SetBounds(x, y, width, height, BoundsSpecified.All);
        }
        //
        // 摘要:
        //     Sets the specified bounds of the control to the specified location and size.
        //
        //
        // 参数:
        //   x:
        //     The new System.Windows.Forms.Control.Left property value of the control.
        //
        //   y:
        //     The new System.Windows.Forms.Control.Top property value of the control.
        //
        //   width:
        //     The new System.Windows.Forms.Control.Width property value of the control.
        //
        //   height:
        //     The new System.Windows.Forms.Control.Height property value of the control.
        //
        //   specified:
        //     A bitwise combination of the System.Windows.Forms.BoundsSpecified values. For
        //     any parameter not specified, the current value will be used.
        public void SetBounds(int x, int y, int width, int height, BoundsSpecified specified)
        {
            var currentLocation = Location;
            var currentSize = Size;

            if ((specified & BoundsSpecified.X) == 0)
                x = currentLocation.X;

            if ((specified & BoundsSpecified.Y) == 0)
                y = currentLocation.Y;

            if ((specified & BoundsSpecified.Width) == 0)
                width = currentSize.Width;

            if ((specified & BoundsSpecified.Height) == 0)
                height = currentSize.Height;

            SetBoundsCore(x, y, width, height, specified);
        }
        //
        // 摘要:
        //     Displays the control to the user.
        public void Show()
        {
            SetVisibleCore(true);
        }
        //
        // 摘要:
        //     Temporarily suspends the layout logic for the control.
        public void SuspendLayout()
        {
            _layoutSuspendCount++;
        }
        //
        // 摘要:
        //     Causes the control to redraw the invalidated regions within its client area.
        public void Update()
        {

        }

        //
        // 摘要:
        //     Creates a new instance of the control collection for the control.
        //
        // 返回结果:
        //     A new instance of System.Windows.Forms.Control.ControlCollection assigned to
        //     the control.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual ControlCollection CreateControlsInstance()
        {
            return new ControlCollection(this);
        }
        //
        // 摘要:
        //     Creates a handle for the control.
        //
        // 异常:
        //   T:System.ObjectDisposedException:
        //     The object is in a disposed state.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void CreateHandle()
        {

        }
        //
        // 摘要:
        //     Sends the specified message to the default window procedure.
        //
        // 参数:
        //   m:
        //     The Windows System.Windows.Forms.Message to process.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void DefWndProc(ref Message m)
        {

        }
        //
        // 摘要:
        //     Destroys the handle associated with the control.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void DestroyHandle()
        {
            if (Handle == 0)
            {
                return;
            }

            Handle = 0;
            OnHandleDestroyed(EventArgs.Empty);
        }
        //
        // 摘要:
        //     Releases the unmanaged resources used by the System.Windows.Forms.Control and
        //     its child controls and optionally releases the managed resources.
        //
        // 参数:
        //   disposing:
        //     true to release both managed and unmanaged resources; false to release only unmanaged
        //     resources.
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                OnHandleDestroyed(EventArgs.Empty);
                if (_lvglEventGcHandle.IsAllocated)
                    _lvglEventGcHandle.Free();
            }
            base.Dispose(disposing);
        }

        //
        // 摘要:
        //     Retrieves a value indicating how a control will behave when its System.Windows.Forms.Control.AutoSize
        //     property is enabled.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.AutoSizeMode values.
        protected AutoSizeMode GetAutoSizeMode()
        {
            return AutoSizeMode.None;
        }
        //
        // 摘要:
        //     Retrieves the bounds within which the control is scaled.
        //
        // 参数:
        //   bounds:
        //     A System.Drawing.Rectangle that specifies the area for which to retrieve the
        //     display bounds.
        //
        //   factor:
        //     The height and width of the control's bounds.
        //
        //   specified:
        //     One of the values of System.Windows.Forms.BoundsSpecified that specifies the
        //     bounds of the control to use when defining its size and position.
        //
        // 返回结果:
        //     A System.Drawing.Rectangle representing the bounds within which the control is
        //     scaled.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual Rectangle GetScaledBounds(Rectangle bounds, SizeF factor, BoundsSpecified specified)
        {
            return bounds;
        }
        //
        // 摘要:
        //     Retrieves the value of the specified control style bit for the control.
        //
        // 参数:
        //   flag:
        //     The System.Windows.Forms.ControlStyles bit to return the value from.
        //
        // 返回结果:
        //     true if the specified control style bit is set to true; otherwise, false.
        protected bool GetStyle(ControlStyles flag)
        {
            return default;
        }
        //
        // 摘要:
        //     Determines if the control is a top-level control.
        //
        // 返回结果:
        //     true if the control is a top-level control; otherwise, false.
        protected bool GetTopLevel()
        {
            return default;
        }
        //
        // 摘要:
        //     Called after the control has been added to another container.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void InitLayout()
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.GotFocus event for the specified control.
        //
        //
        // 参数:
        //   toInvoke:
        //     The System.Windows.Forms.Control to assign the event to.
        //
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected void InvokeGotFocus(Control? toInvoke, EventArgs e)
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.LostFocus event for the specified control.
        //
        //
        // 参数:
        //   toInvoke:
        //     The System.Windows.Forms.Control to assign the event to.
        //
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected void InvokeLostFocus(Control? toInvoke, EventArgs e)
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.Click event for the specified control.
        //
        //
        // 参数:
        //   toInvoke:
        //     The System.Windows.Forms.Control to assign the System.Windows.Forms.Control.Click
        //     event to.
        //
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected void InvokeOnClick(Control? toInvoke, EventArgs e)
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.Paint event for the specified control.
        //
        //
        // 参数:
        //   c:
        //     The System.Windows.Forms.Control to assign the System.Windows.Forms.Control.Paint
        //     event to.
        //
        //   e:
        //     An System.Windows.Forms.PaintEventArgs that contains the event data.
        protected void InvokePaint(Control c, PaintEventArgs e)
        {

        }
        //
        // 摘要:
        //     Raises the PaintBackground event for the specified control.
        //
        // 参数:
        //   c:
        //     The System.Windows.Forms.Control to assign the System.Windows.Forms.Control.Paint
        //     event to.
        //
        //   e:
        //     An System.Windows.Forms.PaintEventArgs that contains the event data.
        protected void InvokePaintBackground(Control c, PaintEventArgs e)
        {

        }
        //
        // 摘要:
        //     Determines if a character is an input character that the control recognizes.
        //
        //
        // 参数:
        //   charCode:
        //     The character to test.
        //
        // 返回结果:
        //     true if the character should be sent directly to the control and not preprocessed;
        //     otherwise, false.
        protected virtual bool IsInputChar(char charCode)
        {
            return true;
        }
        //
        // 摘要:
        //     Determines whether the specified key is a regular input key or a special key
        //     that requires preprocessing.
        //
        // 参数:
        //   keyData:
        //     One of the System.Windows.Forms.Keys values.
        //
        // 返回结果:
        //     true if the specified key is a regular input key; otherwise, false.
        protected virtual bool IsInputKey(Keys keyData)
        {
            return false;
        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.Invalidated event with a specified region
        //     of the control to invalidate.
        //
        // 参数:
        //   invalidatedArea:
        //     A System.Drawing.Rectangle representing the area to invalidate.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void NotifyInvalidate(Rectangle invalidatedArea)
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.AutoSizeChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        protected virtual void OnAutoSizeChanged(EventArgs e) { AutoSizeChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.BackColorChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnBackColorChanged(EventArgs e) { BackColorChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.BackgroundImageChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnBackgroundImageChanged(EventArgs e) { BackgroundImageChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.BackgroundImageLayoutChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnBackgroundImageLayoutChanged(EventArgs e) { BackgroundImageLayoutChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.BindingContextChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnBindingContextChanged(EventArgs e) { BindingContextChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.CausesValidationChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnCausesValidationChanged(EventArgs e) { CausesValidationChanged?.Invoke(this, e); }

        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.Click event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnClick(EventArgs e) { Click?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.ClientSizeChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnClientSizeChanged(EventArgs e)
        {
            ClientSizeChanged?.Invoke(this, e);

            if (Controls.Count > 0)
            {
                PerformLayout(this, nameof(ClientSize));
            }
        }

        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.ContextMenuStripChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnContextMenuStripChanged(EventArgs e) { ContextMenuStripChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.ControlAdded event.
        //
        // 参数:
        //   e:
        //     A System.Windows.Forms.ControlEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnControlAdded(ControlEventArgs e)
        {
            ControlAdded?.Invoke(this, e);
        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.ControlRemoved event.
        //
        // 参数:
        //   e:
        //     A System.Windows.Forms.ControlEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnControlRemoved(ControlEventArgs e)
        {
            ControlRemoved?.Invoke(this, e);
        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.CreateControl method.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnCreateControl()
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.CursorChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnCursorChanged(EventArgs e) { CursorChanged?.Invoke(this, e); }
        //
        // 参数:
        //   e:
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDataContextChanged(EventArgs e) { DataContextChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.DockChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDockChanged(EventArgs e) { DockChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.DoubleClick event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDoubleClick(EventArgs e) { DoubleClick?.Invoke(this, e); }

        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.EnabledChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnEnabledChanged(EventArgs e) { EnabledChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.FontChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnFontChanged(EventArgs e) { FontChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.ForeColorChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnForeColorChanged(EventArgs e) { ForeColorChanged?.Invoke(this, e); }

        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.GotFocus event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnGotFocus(EventArgs e) { GotFocus?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.HandleCreated event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHandleCreated(EventArgs e)
        {
            HandleCreated?.Invoke(this, e);
        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.HandleDestroyed event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHandleDestroyed(EventArgs e)
        {
            HandleDestroyed?.Invoke(this, e);
        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.HelpRequested event.
        //
        // 参数:
        //   hevent:
        //     A System.Windows.Forms.HelpEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnHelpRequested(HelpEventArgs hevent)
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.ImeModeChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        protected virtual void OnImeModeChanged(EventArgs e) { ImeModeChanged?.Invoke(this, e); }

        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.KeyDown event.
        //
        // 参数:
        //   e:
        //     A System.Windows.Forms.KeyEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnKeyDown(KeyEventArgs e) { KeyDown?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.KeyPress event.
        //
        // 参数:
        //   e:
        //     A System.Windows.Forms.KeyPressEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnKeyPress(KeyPressEventArgs e) { KeyPress?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.KeyUp event.
        //
        // 参数:
        //   e:
        //     A System.Windows.Forms.KeyEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnKeyUp(KeyEventArgs e) { KeyUp?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.Layout event.
        //
        // 参数:
        //   levent:
        //     A System.Windows.Forms.LayoutEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnLayout(LayoutEventArgs levent)
        {
            Layout?.Invoke(this, levent);
        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.LocationChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnLocationChanged(EventArgs e)
        {
            LocationChanged?.Invoke(this, e);
        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.LostFocus event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnLostFocus(EventArgs e) { LostFocus?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.MarginChanged event.
        //
        // 参数:
        //   e:
        //     A System.EventArgs that contains the event data.
        protected virtual void OnMarginChanged(EventArgs e) { MarginChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.MouseCaptureChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseCaptureChanged(EventArgs e) { MouseCaptureChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.MouseClick event.
        //
        // 参数:
        //   e:
        //     An System.Windows.Forms.MouseEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseClick(MouseEventArgs e) { MouseClick?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.MouseDoubleClick event.
        //
        // 参数:
        //   e:
        //     An System.Windows.Forms.MouseEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseDoubleClick(MouseEventArgs e) { MouseDoubleClick?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.MouseDown event.
        //
        // 参数:
        //   e:
        //     A System.Windows.Forms.MouseEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseDown(MouseEventArgs e) { MouseDown?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.MouseEnter event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseEnter(EventArgs e) { MouseEnter?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.MouseHover event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseHover(EventArgs e) { MouseHover?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.MouseLeave event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseLeave(EventArgs e) { MouseLeave?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.MouseMove event.
        //
        // 参数:
        //   e:
        //     A System.Windows.Forms.MouseEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseMove(MouseEventArgs e) { MouseMove?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.MouseUp event.
        //
        // 参数:
        //   e:
        //     A System.Windows.Forms.MouseEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseUp(MouseEventArgs e) { MouseUp?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.MouseWheel event.
        //
        // 参数:
        //   e:
        //     A System.Windows.Forms.MouseEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMouseWheel(MouseEventArgs e) { MouseWheel?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.Move event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMove(EventArgs e)
        {
            Move?.Invoke(this, e);
        }
        //
        // 摘要:
        //     Notifies the control of Windows messages.
        //
        // 参数:
        //   m:
        //     A System.Windows.Forms.Message that represents the Windows message.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnNotifyMessage(Message m)
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.PaddingChanged event.
        //
        // 参数:
        //   e:
        //     A System.EventArgs that contains the event data.
        protected virtual void OnPaddingChanged(EventArgs e) { PaddingChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.Paint event.
        //
        // 参数:
        //   e:
        //     A System.Windows.Forms.PaintEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnPaint(PaintEventArgs e)
        {

        }
        //
        // 摘要:
        //     Paints the background of the control.
        //
        // 参数:
        //   pevent:
        //     A System.Windows.Forms.PaintEventArgs that contains information about the control
        //     to paint.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnPaintBackground(PaintEventArgs pevent)
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.BackColorChanged event when the System.Windows.Forms.Control.BackColor
        //     property value of the control's container changes.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnParentBackColorChanged(EventArgs e)
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.BackgroundImageChanged event when the
        //     System.Windows.Forms.Control.BackgroundImage property value of the control's
        //     container changes.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnParentBackgroundImageChanged(EventArgs e)
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.BindingContextChanged event when the
        //     System.Windows.Forms.Control.BindingContext property value of the control's container
        //     changes.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnParentBindingContextChanged(EventArgs e)
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.ParentChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnParentChanged(EventArgs e)
        {
            ParentChanged?.Invoke(this, e);
        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.CursorChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnParentCursorChanged(EventArgs e)
        {

        }
        //
        // 参数:
        //   e:
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnParentDataContextChanged(EventArgs e)
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.EnabledChanged event when the System.Windows.Forms.Control.Enabled
        //     property value of the control's container changes.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnParentEnabledChanged(EventArgs e)
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.FontChanged event when the System.Windows.Forms.Control.Font
        //     property value of the control's container changes.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnParentFontChanged(EventArgs e)
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.ForeColorChanged event when the System.Windows.Forms.Control.ForeColor
        //     property value of the control's container changes.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnParentForeColorChanged(EventArgs e)
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.RightToLeftChanged event when the System.Windows.Forms.Control.RightToLeft
        //     property value of the control's container changes.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnParentRightToLeftChanged(EventArgs e)
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.VisibleChanged event when the System.Windows.Forms.Control.Visible
        //     property value of the control's container changes.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnParentVisibleChanged(EventArgs e)
        {

        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.PreviewKeyDown event.
        //
        // 参数:
        //   e:
        //     A System.Windows.Forms.PreviewKeyDownEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            PreviewKeyDown?.Invoke(this, e);
        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.Paint event.
        //
        // 参数:
        //   e:
        //     A System.Windows.Forms.PaintEventArgs that contains the event data.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     The e parameter is null.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnPrint(PaintEventArgs e)
        {

        }

        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.RegionChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnRegionChanged(EventArgs e) { RegionChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.Resize event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnResize(EventArgs e)
        {
            Resize?.Invoke(this, e);
        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.RightToLeftChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnRightToLeftChanged(EventArgs e) { RightToLeftChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.SizeChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnSizeChanged(EventArgs e)
        {
            SizeChanged?.Invoke(this, e);

            if (Controls.Count > 0)
            {
                PerformLayout(this, nameof(Size));
            }
        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.StyleChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnStyleChanged(EventArgs e) { StyleChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.SystemColorsChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnSystemColorsChanged(EventArgs e) { SystemColorsChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.TabIndexChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnTabIndexChanged(EventArgs e) { TabIndexChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.TabStopChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnTabStopChanged(EventArgs e) { TabStopChanged?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.TextChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnTextChanged(EventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.Validated event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnValidated(EventArgs e) { Validated?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.Validating event.
        //
        // 参数:
        //   e:
        //     A System.ComponentModel.CancelEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnValidating(CancelEventArgs e) { Validating?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.VisibleChanged event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnVisibleChanged(EventArgs e)
        {
            VisibleChanged?.Invoke(this, e);
        }
        //
        // 摘要:
        //     Processes a command key.
        //
        // 参数:
        //   msg:
        //     A System.Windows.Forms.Message, passed by reference, that represents the window
        //     message to process.
        //
        //   keyData:
        //     One of the System.Windows.Forms.Keys values that represents the key to process.
        //
        //
        // 返回结果:
        //     true if the character was processed by the control; otherwise, false.
        protected virtual bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return false;
        }
        //
        // 摘要:
        //     Processes a dialog character.
        //
        // 参数:
        //   charCode:
        //     The character to process.
        //
        // 返回结果:
        //     true if the character was processed by the control; otherwise, false.
        protected virtual bool ProcessDialogChar(char charCode)
        {
            return true;
        }
        //
        // 摘要:
        //     Processes a dialog key.
        //
        // 参数:
        //   keyData:
        //     One of the System.Windows.Forms.Keys values that represents the key to process.
        //
        //
        // 返回结果:
        //     true if the key was processed by the control; otherwise, false.
        protected virtual bool ProcessDialogKey(Keys keyData)
        {
            return true;
        }
        //
        // 摘要:
        //     Processes a key message and generates the appropriate control events.
        //
        // 参数:
        //   m:
        //     A System.Windows.Forms.Message, passed by reference, that represents the window
        //     message to process.
        //
        // 返回结果:
        //     true if the message was processed by the control; otherwise, false.
        protected virtual bool ProcessKeyEventArgs(ref Message m)
        {
            Keys keyData = TranslateLvglKey((uint)m.wParam);
            var keyEventArgs = new KeyEventArgs(keyData);
            OnKeyDown(keyEventArgs);

            if (!keyEventArgs.SuppressKeyPress)
            {
                char keyChar = TranslateLvglKeyChar((uint)m.wParam);
                if (keyChar != '\0')
                {
                    var keyPressEventArgs = new KeyPressEventArgs(keyChar);
                    OnKeyPress(keyPressEventArgs);

                    if (keyPressEventArgs.Handled)
                    {
                        keyEventArgs.Handled = true;
                    }
                }
            }

            return keyEventArgs.Handled || keyEventArgs.SuppressKeyPress;
        }
        //
        // 摘要:
        //     Previews a keyboard message.
        //
        // 参数:
        //   m:
        //     A System.Windows.Forms.Message, passed by reference, that represents the window
        //     message to process.
        //
        // 返回结果:
        //     true if the message was processed by the control; otherwise, false.
        protected virtual bool ProcessKeyPreview(ref Message m)
        {
            Keys keyData = TranslateLvglKey((uint)m.wParam);
            var previewArgs = new PreviewKeyDownEventArgs(keyData)
            {
                IsInputKey = IsInputKey(keyData)
            };

            OnPreviewKeyDown(previewArgs);
            return previewArgs.IsInputKey;
        }

        //
        // 摘要:
        //     Raises the appropriate key event.
        //
        // 参数:
        //   key:
        //     The event to raise.
        //
        //   e:
        //     A System.Windows.Forms.KeyEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected void RaiseKeyEvent(object key, KeyEventArgs e)
        {

        }
        //
        // 摘要:
        //     Raises the appropriate mouse event.
        //
        // 参数:
        //   key:
        //     The event to raise.
        //
        //   e:
        //     A System.Windows.Forms.MouseEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected void RaiseMouseEvent(object key, MouseEventArgs e)
        {

        }
        //
        // 摘要:
        //     Raises the appropriate paint event.
        //
        // 参数:
        //   key:
        //     The event to raise.
        //
        //   e:
        //     A System.Windows.Forms.PaintEventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected void RaisePaintEvent(object key, PaintEventArgs e)
        {

        }
        //
        // 摘要:
        //     Forces the re-creation of the handle for the control.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected void RecreateHandle()
        {

        }
        //
        // 摘要:
        //     Provides constants for rescaling the control when a DPI change occurs.
        //
        // 参数:
        //   deviceDpiOld:
        //     The DPI value prior to the change.
        //
        //   deviceDpiNew:
        //     The DPI value after the change.
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void RescaleConstantsForDpi(int deviceDpiOld, int deviceDpiNew)
        {

        }
        //
        // 摘要:
        //     Resets the control to handle the System.Windows.Forms.Control.MouseLeave event.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected void ResetMouseEventArgs()
        {

        }

        //
        // 摘要:
        //     Scales a control's location, size, padding and margin.
        //
        // 参数:
        //   factor:
        //     The factor by which the height and width of the control will be scaled.
        //
        //   specified:
        //     A System.Windows.Forms.BoundsSpecified value that specifies the bounds of the
        //     control to use when defining its size and position.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void ScaleControl(SizeF factor, BoundsSpecified specified)
        {

        }
        //
        // 摘要:
        //     This method is not relevant for this class.
        //
        // 参数:
        //   dx:
        //     The horizontal scaling factor.
        //
        //   dy:
        //     The vertical scaling factor.
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual void ScaleCore(float dx, float dy)
        {

        }
        //
        // 摘要:
        //     Activates a child control. Optionally specifies the direction in the tab order
        //     to select the control from.
        //
        // 参数:
        //   directed:
        //     true to specify the direction of the control to select; otherwise, false.
        //
        //   forward:
        //     true to move forward in the tab order; false to move backward in the tab order.
        protected virtual void Select(bool directed, bool forward)
        {

        }
        //
        // 摘要:
        //     Sets a value indicating how a control will behave when its System.Windows.Forms.Control.AutoSize
        //     property is enabled.
        //
        // 参数:
        //   mode:
        //     One of the System.Windows.Forms.AutoSizeMode values.
        protected void SetAutoSizeMode(AutoSizeMode mode)
        {

        }
        //
        // 摘要:
        //     Performs the work of setting the specified bounds of this control.
        //
        // 参数:
        //   x:
        //     The new System.Windows.Forms.Control.Left property value of the control.
        //
        //   y:
        //     The new System.Windows.Forms.Control.Top property value of the control.
        //
        //   width:
        //     The new System.Windows.Forms.Control.Width property value of the control.
        //
        //   height:
        //     The new System.Windows.Forms.Control.Height property value of the control.
        //
        //   specified:
        //     A bitwise combination of the System.Windows.Forms.BoundsSpecified values.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            UpdateBounds(x, y, width, height);

            if (_lvglObjectHandle != 0)
            {
                unsafe
                {
                    ApplyLvglProperties();
                }
            }
        }
        //
        // 摘要:
        //     Sets the size of the client area of the control.
        //
        // 参数:
        //   x:
        //     The client area width, in pixels.
        //
        //   y:
        //     The client area height, in pixels.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void SetClientSizeCore(int x, int y)
        {

        }
        //
        // 摘要:
        //     Sets a specified System.Windows.Forms.ControlStyles flag to either true or false.
        //
        // 参数:
        //   flag:
        //     The System.Windows.Forms.ControlStyles bit to set.
        //
        //   value:
        //     true to apply the specified style to the control; otherwise, false.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected void SetStyle(ControlStyles flag, bool value)
        {

        }
        //
        // 摘要:
        //     Sets the control as the top-level control.
        //
        // 参数:
        //   value:
        //     true to set the control as the top-level control; otherwise, false.
        //
        // 异常:
        //   T:System.InvalidOperationException:
        //     The value parameter is set to true and the control is an ActiveX control.
        //
        //   T:System.Exception:
        //     The System.Windows.Forms.Control.GetTopLevel return value is not equal to the
        //     value parameter and the System.Windows.Forms.Control.Parent property is not null.
        protected void SetTopLevel(bool value)
        {

        }
        //
        // 摘要:
        //     Sets the control to the specified visible state.
        //
        // 参数:
        //   value:
        //     true to make the control visible; otherwise, false.
        protected virtual unsafe void SetVisibleCore(bool value)
        {
            if (Visible == value)
            {
                return;
            }

            Visible = value;

            var obj = (Interop.lv_obj_t*)_lvglObjectHandle;
            if (obj != null)
            {
                if (value)
                {
                    lv_obj_remove_flag(obj, LV_OBJ_FLAG_HIDDEN);
                }
                else
                {
                    lv_obj_add_flag(obj, LV_OBJ_FLAG_HIDDEN);
                }
            }

            OnVisibleChanged(EventArgs.Empty);
        }
       
        //
        // 摘要:
        //     Updates the bounds of the control with the specified size, location, and client
        //     size.
        //
        // 参数:
        //   x:
        //     The System.Drawing.Point.X coordinate of the control.
        //
        //   y:
        //     The System.Drawing.Point.Y coordinate of the control.
        //
        //   width:
        //     The System.Drawing.Size.Width of the control.
        //
        //   height:
        //     The System.Drawing.Size.Height of the control.
        //
        //   clientWidth:
        //     The client System.Drawing.Size.Width of the control.
        //
        //   clientHeight:
        //     The client System.Drawing.Size.Height of the control.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected void UpdateBounds(int x, int y, int width, int height, int clientWidth, int clientHeight)
        {
            var oldLocation = Location;
            var oldSize = Size;
            var oldClientSize = ClientSize;

            Left = x;
            Top = y;
            Width = width;
            Height = height;
            Location = new Point(x, y);
            Size = new Size(width, height);
            Bounds = new Rectangle(x, y, width, height);
            ClientSize = new Size(clientWidth, clientHeight);

            if (oldLocation != Location)
            {
                OnLocationChanged(EventArgs.Empty);
                OnMove(EventArgs.Empty);
            }

            if (oldSize != Size)
            {
                OnSizeChanged(EventArgs.Empty);
                OnResize(EventArgs.Empty);
            }

            if (oldClientSize != ClientSize)
            {
                OnClientSizeChanged(EventArgs.Empty);
            }
        }
        //
        // 摘要:
        //     Updates the bounds of the control with the specified size and location.
        //
        // 参数:
        //   x:
        //     The System.Drawing.Point.X coordinate of the control.
        //
        //   y:
        //     The System.Drawing.Point.Y coordinate of the control.
        //
        //   width:
        //     The System.Drawing.Size.Width of the control.
        //
        //   height:
        //     The System.Drawing.Size.Height of the control.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected void UpdateBounds(int x, int y, int width, int height)
        {
            UpdateBounds(x, y, width, height, width, height);
        }
        //
        // 摘要:
        //     Forces the assigned styles to be reapplied to the control.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected void UpdateStyles()
        {

        }
        //
        // 摘要:
        //     Updates the control in its parent's z-order.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected void UpdateZOrder()
        {

        }
        //
        // 摘要:
        //     Processes Windows messages.
        //
        // 参数:
        //   m:
        //     The Windows System.Windows.Forms.Message to process.
        protected virtual void WndProc(ref Message m)
        {

        }

        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.Enter event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected internal virtual void OnEnter(EventArgs e) { Enter?.Invoke(this, e); }
        //
        // 摘要:
        //     Raises the System.Windows.Forms.Control.Leave event.
        //
        // 参数:
        //   e:
        //     An System.EventArgs that contains the event data.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected internal virtual void OnLeave(EventArgs e) { Leave?.Invoke(this, e); }
        //
        // 摘要:
        //     Processes a keyboard message.
        //
        // 参数:
        //   m:
        //     A System.Windows.Forms.Message, passed by reference, that represents the window
        //     message to process.
        //
        // 返回结果:
        //     true if the message was processed by the control; otherwise, false.
        protected internal virtual bool ProcessKeyMessage(ref Message m)
        {
            Keys keyData = TranslateLvglKey((uint)m.wParam);

            if (ProcessCmdKey(ref m, keyData))
            {
                return true;
            }

            bool treatAsInputKey = ProcessKeyPreview(ref m);

            if (treatAsInputKey)
            {
                return ProcessKeyEventArgs(ref m);
            }

            if (IsInputKey(keyData))
            {
                return ProcessKeyEventArgs(ref m);
            }

            return ProcessDialogKey(keyData);
        }
        //
        // 摘要:
        //     Processes a mnemonic character.
        //
        // 参数:
        //   charCode:
        //     The character to process.
        //
        // 返回结果:
        //     true if the character was processed as a mnemonic by the control; otherwise,
        //     false.
        protected internal virtual bool ProcessMnemonic(char charCode)
        {
            return true;
        }

        //
        // 摘要:
        //     Updates the bounds of the control with the current size and location.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected internal void UpdateBounds()
        {
            UpdateBounds(Location.X, Location.Y, Size.Width, Size.Height, ClientSize.Width, ClientSize.Height);
        }

        internal void HandleChildAdded(Control child)
        {
            ArgumentNullException.ThrowIfNull(child);

            child.Parent = this;
            child.CaptureAnchorLayout();
            child.InitLayout();
            OnControlAdded(new ControlEventArgs(child));
            PerformLayout(child, nameof(Controls));
        }

        internal void HandleChildRemoved(Control child)
        {
            ArgumentNullException.ThrowIfNull(child);

            child.Parent = null;
            OnControlRemoved(new ControlEventArgs(child));
            PerformLayout(child, nameof(Controls));
        }

        // Backing fields for tracked properties
        private bool _visible = true;
        private bool _enabled = true;
        private string? _text;
        private Size _size;
        private Point _location;
        private Color _backColor;
        private Color _foreColor;
        private Font? _font;
        private Padding _padding = new Padding();
        private Padding _margin = new Padding();
        private DockStyle _dock;
        private Control? _parent;
        private int _tabIndex;
        private bool _tabStop = true;
        private bool _causesValidation = true;
        private Cursor? _cursor;
        private ContextMenuStrip? _contextMenuStrip;
        private Region? _region;
        private RightToLeft _rightToLeft = RightToLeft.Inherit;
        private BindingContext? _bindingContext;
        private object? _dataContext;
        private bool _autoSize;
        private AnchorStyles _anchor = AnchorStyles.Top | AnchorStyles.Left;

        // --- LVGL Integration ---

        /// <summary>LVGL object handle (lv_obj_t*) stored as nint.</summary>
        internal nint _lvglObjectHandle;

        /// <summary>GCHandle used to bridge this managed Control to the unmanaged LVGL event callback.</summary>
        private GCHandle _lvglEventGcHandle;

        /// <summary>
        /// Registers a single LVGL event callback on the current LVGL object that routes
        /// all LVGL events to the managed On* virtual methods.
        /// </summary>
        protected unsafe void RegisterLvglEvents()
        {
            if (_lvglObjectHandle == 0) return;
            if (_lvglEventGcHandle.IsAllocated) _lvglEventGcHandle.Free();
            _lvglEventGcHandle = GCHandle.Alloc(this);
            lv_obj_add_event_cb(
                (Interop.lv_obj_t*)_lvglObjectHandle,
                &LvglEventCallback,
                LV_EVENT_ALL,
                (void*)GCHandle.ToIntPtr(_lvglEventGcHandle));
            OnHandleCreated(EventArgs.Empty);
        }

        /// <summary>
        /// Static LVGL event callback — routes events to the owning <see cref="Control"/>'s
        /// <see cref="DispatchLvglEvent"/> method via a stored <see cref="GCHandle"/>.
        /// </summary>
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        private static unsafe void LvglEventCallback(Interop.lv_event_t* e)
        {
            void* userData = lv_event_get_user_data(e);
            if (userData == null) return;
            var gcHandle = GCHandle.FromIntPtr(new IntPtr(userData));
            if (gcHandle.IsAllocated && gcHandle.Target is Control ctrl)
                ctrl.DispatchLvglEvent(lv_event_get_code(e), e);
        }

        /// <summary>Dispatches an LVGL event code to the appropriate On* method(s).</summary>
        protected virtual unsafe void DispatchLvglEvent(Interop.lv_event_code_t code, Interop.lv_event_t* lvglEvent)
        {
            MouseEventArgs mouseEventArgs = CreateMouseEventArgs(code);

            switch (code)
            {
                case LV_EVENT_CLICKED:
                    OnClick(EventArgs.Empty);
                    OnMouseClick(mouseEventArgs);
                    break;
                case LV_EVENT_DOUBLE_CLICKED:
                    OnDoubleClick(EventArgs.Empty);
                    OnMouseDoubleClick(mouseEventArgs);
                    break;
                case LV_EVENT_PRESSED:
                    OnMouseDown(mouseEventArgs);
                    break;
                case LV_EVENT_RELEASED:
                    OnMouseUp(mouseEventArgs);
                    break;
                case LV_EVENT_PRESSING:
                    OnMouseMove(mouseEventArgs);
                    break;
                case LV_EVENT_FOCUSED:
                    OnEnter(EventArgs.Empty);
                    OnGotFocus(EventArgs.Empty);
                    break;
                case LV_EVENT_DEFOCUSED:
                    // Object has lost keyboard focus 锟斤拷 fire LostFocus
                    OnLostFocus(EventArgs.Empty);
                    break;
                case LV_EVENT_LEAVE:
                    // Group navigation moved away from this object 锟斤拷 fire Leave
                    OnLeave(EventArgs.Empty);
                    break;
                case LV_EVENT_KEY:
                    uint lvglKey = lv_event_get_key(lvglEvent);
                    Keys translatedKey = TranslateLvglKey(lvglKey);
                    var keyMessage = new Message(
                        Handle,
                        0x0100,
                        (nuint)lvglKey,
                        0);

                    var preprocessState = PreProcessControlMessage(ref keyMessage);
                    if (preprocessState == PreProcessControlState.MessageNotNeeded)
                    {
                        var keyEventArgs = new KeyEventArgs(translatedKey);
                        OnKeyDown(keyEventArgs);

                        if (!keyEventArgs.SuppressKeyPress)
                        {
                            char keyChar = TranslateLvglKeyChar(lvglKey);
                            if (keyChar != '\0')
                            {
                                OnKeyPress(new KeyPressEventArgs(keyChar));
                            }
                        }
                    }
                    break;
                case LV_EVENT_SIZE_CHANGED:
                    OnResize(EventArgs.Empty);
                    OnSizeChanged(EventArgs.Empty);
                    OnClientSizeChanged(EventArgs.Empty);
                    break;
                case LV_EVENT_LAYOUT_CHANGED:
                    OnLayout(new LayoutEventArgs(null, null));
                    break;
                case LV_EVENT_STYLE_CHANGED:
                    OnStyleChanged(EventArgs.Empty);
                    break;
                case LV_EVENT_HOVER_OVER:
                    OnMouseEnter(EventArgs.Empty);
                    OnMouseHover(EventArgs.Empty);
                    break;
                case LV_EVENT_HOVER_LEAVE:
                    OnMouseLeave(EventArgs.Empty);
                    break;
                case LV_EVENT_SCROLL:
                    OnMouseWheel(mouseEventArgs);
                    break;
                case LV_EVENT_DELETE:
                    OnHandleDestroyed(EventArgs.Empty);
                    break;
                case LV_EVENT_VALUE_CHANGED:
                    OnTextChanged(EventArgs.Empty);
                    break;
            }
        }

        private static MouseEventArgs CreateMouseEventArgs(Interop.lv_event_code_t code)
        {
            var position = RuntimeInputState.GetCurrentMousePosition();
            MouseButtons button = MapMouseButton(RuntimeInputState.GetCurrentMouseButton());
            int clicks = code == LV_EVENT_DOUBLE_CLICKED ? 2 : 1;

            return new MouseEventArgs(button, clicks, position.X, position.Y);
        }

        private static MouseButtons MapMouseButton(uint button)
        {
            return button switch
            {
                1 => MouseButtons.Left,
                2 => MouseButtons.Right,
                4 => MouseButtons.Middle,
                8 => MouseButtons.XButton1,
                16 => MouseButtons.XButton2,
                _ => MouseButtons.None,
            };
        }

        /// <summary>Converts a percentage value to an LVGL LV_PCT coordinate.
        /// LVGL identifies percentage coordinates by setting bit 29 (LV_COORD_TYPE_SPEC flag).
        /// </summary>
        protected static int LvPct(int percent) => percent | (1 << 29); // bit 29 = LV_COORD_TYPE_SPEC

        /// <summary>Converts a C# string to a null-terminated UTF-8 byte array for LVGL.</summary>
        internal static byte[] ToUtf8(string? text)
        {
            if (string.IsNullOrEmpty(text)) return new byte[] { 0 };
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            var result = new byte[bytes.Length + 1];
            bytes.CopyTo(result, 0);
            return result;
        }

        /// <summary>
        /// Creates the LVGL widget for this control under the given parent LVGL object.
        /// Overrides should call <see cref="ApplyLvglProperties"/> and
        /// <see cref="CreateChildrenLvglObjects"/> after creating the widget.
        /// </summary>
        internal virtual unsafe void CreateLvglObject(nint parentHandle)
        {
            var parent = (Interop.lv_obj_t*)parentHandle;
            _lvglObjectHandle = (nint)lv_obj_create(parent);
            // Remove default padding so child positioning is exact
            lv_obj_set_style_pad_all((Interop.lv_obj_t*)_lvglObjectHandle, 0, 0);
            ApplyLvglProperties();
            CreateChildrenLvglObjects();
        }

        /// <summary>Applies WinForms layout properties (size, position, visibility) to the LVGL object.</summary>
        protected virtual unsafe void ApplyLvglProperties()
        {
            var obj = (Interop.lv_obj_t*)_lvglObjectHandle;
            if (obj == null) return;

            int w = Size.Width > 0 ? Size.Width : LV_SIZE_CONTENT;
            int h = Size.Height > 0 ? Size.Height : LV_SIZE_CONTENT;
            lv_obj_set_size(obj, w, h);
            lv_obj_set_pos(obj, Location.X, Location.Y);

            if (BackColor != Color.Empty)
            {
                lv_obj_set_style_bg_opa(obj, (byte)_lv_opacity_level_t.LV_OPA_COVER, 0);
                lv_obj_set_style_bg_color(obj, lv_color_make(BackColor.R, BackColor.G, BackColor.B), 0);
            }

            if (ForeColor != Color.Empty)
            {
                lv_obj_set_style_text_color(obj, lv_color_make(ForeColor.R, ForeColor.G, ForeColor.B), 0);
            }

            if (!AllowsNativeScrolling)
            {
                lv_obj_remove_flag(obj, LV_OBJ_FLAG_SCROLLABLE | LV_OBJ_FLAG_SCROLL_ELASTIC | LV_OBJ_FLAG_SCROLL_MOMENTUM | LV_OBJ_FLAG_SCROLL_CHAIN);
                lv_obj_set_scrollbar_mode(obj, LV_SCROLLBAR_MODE_OFF);
            }

            if (!Visible)
                lv_obj_add_flag(obj, LV_OBJ_FLAG_HIDDEN);

            RegisterLvglEvents();
        }

        protected virtual bool AllowsNativeScrolling => false;

        /// <summary>Recursively creates LVGL objects for all WinForms child controls.</summary>
        protected unsafe void CreateChildrenLvglObjects()
        {
            foreach (var child in Controls)
            {
                child.CreateLvglObject(_lvglObjectHandle);
            }
        }

        private void PerformDockAndAnchorLayout()
        {
            var availableWidth = Math.Max(0, (ClientSize.Width > 0 ? ClientSize.Width : Size.Width) - Padding.Horizontal);
            var availableHeight = Math.Max(0, (ClientSize.Height > 0 ? ClientSize.Height : Size.Height) - Padding.Vertical);
            var left = Padding.Left;
            var top = Padding.Top;
            var right = Padding.Left + availableWidth;
            var bottom = Padding.Top + availableHeight;

            foreach (var child in Controls)
            {
                var margin = child.Margin;
                switch (child.Dock)
                {
                    case DockStyle.Top:
                    {
                        var height = child.Size.Height;
                        child.SetBounds(
                            left + margin.Left,
                            top + margin.Top,
                            Math.Max(0, right - left - margin.Horizontal),
                            height,
                            BoundsSpecified.All);
                        top += height + margin.Vertical;
                        child.CaptureAnchorLayout();
                        break;
                    }
                    case DockStyle.Bottom:
                    {
                        var height = child.Size.Height;
                        child.SetBounds(
                            left + margin.Left,
                            bottom - height + margin.Top - margin.Bottom,
                            Math.Max(0, right - left - margin.Horizontal),
                            height,
                            BoundsSpecified.All);
                        bottom -= height + margin.Vertical;
                        child.CaptureAnchorLayout();
                        break;
                    }
                    case DockStyle.Left:
                    {
                        var width = child.Size.Width;
                        child.SetBounds(
                            left + margin.Left,
                            top + margin.Top,
                            width,
                            Math.Max(0, bottom - top - margin.Vertical),
                            BoundsSpecified.All);
                        left += width + margin.Horizontal;
                        child.CaptureAnchorLayout();
                        break;
                    }
                    case DockStyle.Right:
                    {
                        var width = child.Size.Width;
                        child.SetBounds(
                            right - width + margin.Left - margin.Right,
                            top + margin.Top,
                            width,
                            Math.Max(0, bottom - top - margin.Vertical),
                            BoundsSpecified.All);
                        right -= width + margin.Horizontal;
                        child.CaptureAnchorLayout();
                        break;
                    }
                    case DockStyle.Fill:
                    {
                        child.SetBounds(
                            left + margin.Left,
                            top + margin.Top,
                            Math.Max(0, right - left - margin.Horizontal),
                            Math.Max(0, bottom - top - margin.Vertical),
                            BoundsSpecified.All);
                        child.CaptureAnchorLayout();
                        break;
                    }
                }
            }

            foreach (var child in Controls)
            {
                if (child.Dock != DockStyle.None)
                {
                    continue;
                }

                child.ApplyAnchorLayout(availableWidth, availableHeight);
            }
        }

        private void ApplyAnchorLayout(int parentWidth, int parentHeight)
        {
            if (_anchorReferenceParentSize.Width == 0 && _anchorReferenceParentSize.Height == 0)
            {
                CaptureAnchorLayout();
                return;
            }

            var deltaWidth = parentWidth - _anchorReferenceParentSize.Width;
            var deltaHeight = parentHeight - _anchorReferenceParentSize.Height;
            var bounds = _anchorReferenceBounds;
            var x = bounds.X;
            var y = bounds.Y;
            var width = bounds.Width;
            var height = bounds.Height;
            var anchorLeft = (Anchor & AnchorStyles.Left) == AnchorStyles.Left;
            var anchorRight = (Anchor & AnchorStyles.Right) == AnchorStyles.Right;
            var anchorTop = (Anchor & AnchorStyles.Top) == AnchorStyles.Top;
            var anchorBottom = (Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom;

            if (anchorLeft && anchorRight)
            {
                width = Math.Max(0, width + deltaWidth);
            }
            else if (!anchorLeft && anchorRight)
            {
                x += deltaWidth;
            }

            if (anchorTop && anchorBottom)
            {
                height = Math.Max(0, height + deltaHeight);
            }
            else if (!anchorTop && anchorBottom)
            {
                y += deltaHeight;
            }

            SetBounds(x, y, width, height, BoundsSpecified.All);
        }

        internal void CaptureAnchorLayout()
        {
            var parentSize = Parent is null
                ? new Size(0, 0)
                : (Parent.ClientSize.Width > 0 || Parent.ClientSize.Height > 0 ? Parent.ClientSize : Parent.Size);

            _anchorReferenceParentSize = parentSize;
            _anchorReferenceBounds = Bounds;
        }

    }
}