using LVGLSharp.Interop;
using LVGLSharp.Darwing;
using System.ComponentModel;

namespace LVGLSharp.Forms
{
    /// <summary>Displays an image using the LVGL image widget.</summary>
    public class PictureBox : Control, ISupportInitialize
    {
        private bool _initializing;
        private Image? _image;
        private string? _imageLocation;
        private PictureBoxSizeMode _sizeMode = PictureBoxSizeMode.Normal;
        private nint _lvglImagePtr = nint.Zero;

        /// <summary>
        /// Gets or sets the image that is displayed in the PictureBox.
        /// </summary>
        [Localizable(true)]
        [Bindable(true)]
        public Image? Image
        {
            get => _image;
            set
            {
                if (_image != value)
                {
                    _image = value;
                    if (!_initializing)
                    {
                        UpdateImage();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the path or URL of the image to display in the PictureBox.
        /// </summary>
        [Localizable(true)]
        [DefaultValue(null)]
        public string? ImageLocation
        {
            get => _imageLocation;
            set
            {
                if (_imageLocation != value)
                {
                    _imageLocation = value;
                    if (!_initializing && !string.IsNullOrEmpty(value))
                    {
                        LoadImage(value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets how the image is displayed.
        /// </summary>
        [Localizable(true)]
        [DefaultValue(PictureBoxSizeMode.Normal)]
        public PictureBoxSizeMode SizeMode
        {
            get => _sizeMode;
            set
            {
                if (_sizeMode != value)
                {
                    _sizeMode = value;
                    if (!_initializing)
                    {
                        UpdateSizeMode();
                    }
                }
            }
        }

        /// <summary>Signals the object that initialization is starting.</summary>
        public void BeginInit()
        {
            _initializing = true;
        }

        /// <summary>Signals the object that initialization is complete.</summary>
        public void EndInit()
        {
            _initializing = false;
            if (_image != null)
            {
                UpdateImage();
            }
            else if (!string.IsNullOrEmpty(_imageLocation))
            {
                LoadImage(_imageLocation);
            }
            UpdateSizeMode();
        }

        /// <summary>
        /// Loads an image from the specified location.
        /// </summary>
        /// <param name="url">The path or URL of the image to load.</param>
        public void Load(string url)
        {
            ArgumentNullException.ThrowIfNull(url);
            LoadImage(url);
        }

        /// <summary>
        /// Loads an image from the specified location asynchronously.
        /// </summary>
        /// <param name="url">The path or URL of the image to load.</param>
        public async Task LoadAsync(string url)
        {
            ArgumentNullException.ThrowIfNull(url);
            await Task.Run(() => LoadImage(url));
        }

        internal override unsafe void CreateLvglObject(nint parentHandle)
        {
            _lvglObjectHandle = (nint)lv_image_create((lv_obj_t*)parentHandle);
            ApplyLvglProperties();
            
            // Apply initial image if set
            if (_image != null)
            {
                UpdateImage();
            }
            else if (!string.IsNullOrEmpty(_imageLocation))
            {
                LoadImage(_imageLocation);
            }
            
            UpdateSizeMode();
            CreateChildrenLvglObjects();
        }

        private unsafe void UpdateImage()
        {
            if (_lvglObjectHandle == nint.Zero)
                return;

            // TODO: Convert Image to LVGL image format
            // This is a placeholder implementation
            // In a real implementation, you would need to:
            // 1. Convert the Image to a format LVGL understands
            // 2. Store the image data in memory
            // 3. Pass the pointer to lv_image_set_src
            
            // For now, just clear if image is null
            if (_image == null && _lvglImagePtr != nint.Zero)
            {
                lv_image_set_src((lv_obj_t*)_lvglObjectHandle, (void*)nint.Zero);
                _lvglImagePtr = nint.Zero;
            }
        }

        private unsafe void LoadImage(string path)
        {
            if (_lvglObjectHandle == nint.Zero)
                return;

            ArgumentNullException.ThrowIfNull(path);

            // TODO: Implement actual image loading from file path
            // This requires:
            // 1. Reading the image file
            // 2. Converting it to LVGL format (lv_image_dsc_t)
            // 3. Storing the image data
            // 4. Passing it to LVGL
            
            // For now, try to use LVGL's file system if path looks like an LVGL path
            if (path.StartsWith("A:", StringComparison.Ordinal) || 
                path.StartsWith("S:", StringComparison.Ordinal))
            {
                // LVGL internal file system path
                var pathBytes = System.Text.Encoding.UTF8.GetBytes(path);
                fixed (byte* pathPtr = pathBytes)
                {
                    lv_image_set_src((lv_obj_t*)_lvglObjectHandle, pathPtr);
                }
            }
            else
            {
                // Regular file path - needs implementation
                throw new NotImplementedException(
                    "Loading images from regular file paths is not yet implemented. " +
                    "Please use LVGL file system paths (A:, S:) or set the Image property directly.");
            }
        }

        private unsafe void UpdateSizeMode()
        {
            if (_lvglObjectHandle == nint.Zero)
                return;

            lv_obj_t* obj = (lv_obj_t*)_lvglObjectHandle;

            switch (_sizeMode)
            {
                case PictureBoxSizeMode.Normal:
                    // Normal mode - image at original size, top-left aligned
                    lv_image_set_inner_align(obj, LV_IMAGE_ALIGN_TOP_LEFT);
                    lv_image_set_scale(obj, 256); // 256 = 100% in LVGL
                    break;

                case PictureBoxSizeMode.StretchImage:
                    // Stretch to fill the control
                    lv_image_set_inner_align(obj, LV_IMAGE_ALIGN_STRETCH);
                    break;

                case PictureBoxSizeMode.AutoSize:
                    // Resize control to fit image
                    // TODO: Get image dimensions and resize control
                    lv_image_set_inner_align(obj, LV_IMAGE_ALIGN_TOP_LEFT);
                    lv_image_set_scale(obj, 256);
                    break;

                case PictureBoxSizeMode.CenterImage:
                    // Center the image in the control
                    lv_image_set_inner_align(obj, LV_IMAGE_ALIGN_CENTER);
                    lv_image_set_scale(obj, 256);
                    break;

                case PictureBoxSizeMode.Zoom:
                    // Zoom to fit while maintaining aspect ratio
                    lv_image_set_inner_align(obj, LV_IMAGE_ALIGN_CENTER);
                    // TODO: Calculate scale to fit while maintaining aspect ratio
                    break;
            }
        }

        /// <summary>
        /// Sets the rotation angle of the image.
        /// </summary>
        /// <param name="angle">The rotation angle in degrees (0-360).</param>
        public unsafe void SetRotation(int angle)
        {
            if (_lvglObjectHandle != nint.Zero)
            {
                // LVGL uses 0.1 degree units
                lv_image_set_rotation((lv_obj_t*)_lvglObjectHandle, angle * 10);
            }
        }

        /// <summary>
        /// Sets the zoom/scale of the image.
        /// </summary>
        /// <param name="zoom">The zoom factor (256 = 100%, 512 = 200%, 128 = 50%).</param>
        public unsafe void SetZoom(uint zoom)
        {
            if (_lvglObjectHandle != nint.Zero)
            {
                lv_image_set_scale((lv_obj_t*)_lvglObjectHandle, zoom);
            }
        }

        /// <summary>
        /// Enables or disables anti-aliasing for the image.
        /// </summary>
        /// <param name="enabled">True to enable anti-aliasing, false to disable.</param>
        public unsafe void SetAntiAlias(bool enabled)
        {
            if (_lvglObjectHandle != nint.Zero)
            {
                lv_image_set_antialias((lv_obj_t*)_lvglObjectHandle, enabled);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _image = null;
                if (_lvglImagePtr != nint.Zero)
                {
                    // TODO: Free LVGL image data if allocated
                    _lvglImagePtr = nint.Zero;
                }
            }
            base.Dispose(disposing);
        }
    }
}
