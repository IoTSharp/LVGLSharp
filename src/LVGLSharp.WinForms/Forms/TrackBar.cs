using LVGLSharp.Interop;

namespace LVGLSharp.Forms
{
    public class TrackBar : Control
    {
        private int _minimum = 0;
        private int _maximum = 10;
        private int _value = 0;

        public event EventHandler? ValueChanged;

        public int Minimum
        {
            get => _minimum;
            set
            {
                if (_minimum == value)
                {
                    return;
                }

                _minimum = value;
                if (_maximum < _minimum)
                {
                    _maximum = _minimum;
                }

                if (_value < _minimum)
                {
                    _value = _minimum;
                }

                UpdateLvglValue();
            }
        }

        public int Maximum
        {
            get => _maximum;
            set
            {
                if (_maximum == value)
                {
                    return;
                }

                _maximum = value;
                if (_minimum > _maximum)
                {
                    _minimum = _maximum;
                }

                if (_value > _maximum)
                {
                    _value = _maximum;
                }

                UpdateLvglValue();
            }
        }

        public int Value
        {
            get => _value;
            set
            {
                int clampedValue = Math.Clamp(value, _minimum, _maximum);
                if (_value == clampedValue)
                {
                    return;
                }

                _value = clampedValue;
                UpdateLvglValue();
                OnValueChanged(EventArgs.Empty);
            }
        }

        public Orientation Orientation { get; set; } = Orientation.Horizontal;
        /// <remarks>Not currently applied to the LVGL slider widget.</remarks>
        public int TickFrequency { get; set; } = 1;
        /// <remarks>Not currently applied to the LVGL slider widget.</remarks>
        public TickStyle TickStyle { get; set; } = TickStyle.BottomRight;
        /// <remarks>Not currently applied to the LVGL slider widget.</remarks>
        public int SmallChange { get; set; } = 1;
        /// <remarks>Not currently applied to the LVGL slider widget.</remarks>
        public int LargeChange { get; set; } = 5;

        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        private unsafe void UpdateLvglValue()
        {
            if (_lvglObjectHandle == 0) return;
            var obj = (lv_obj_t*)_lvglObjectHandle;
            lv_slider_set_range(obj, _minimum, _maximum);
            lv_slider_set_value(obj, _value, LV_ANIM_OFF);
        }

        internal override unsafe void CreateLvglObject(nint parentHandle)
        {
            _lvglObjectHandle = (nint)lv_slider_create((lv_obj_t*)parentHandle);
            var obj = (lv_obj_t*)_lvglObjectHandle;
            lv_slider_set_range(obj, _minimum, _maximum);
            lv_slider_set_value(obj, _value, LV_ANIM_OFF);
            // Apply orientation
            var orient = Orientation == Orientation.Vertical
                ? lv_slider_orientation_t.LV_SLIDER_ORIENTATION_VERTICAL
                : lv_slider_orientation_t.LV_SLIDER_ORIENTATION_HORIZONTAL;
            lv_slider_set_orientation(obj, orient);
            Application.CurrentStyleSet.TrackBar.Apply(obj);
            ApplyLvglProperties();
            CreateChildrenLvglObjects();
        }

        protected override void DispatchLvglEvent(lv_event_code_t code)
        {
            if (code == LV_EVENT_VALUE_CHANGED)
            {
                SyncValueFromLvgl();
                return;
            }

            base.DispatchLvglEvent(code);
        }

        private unsafe void SyncValueFromLvgl()
        {
            if (_lvglObjectHandle == nint.Zero)
            {
                return;
            }

            int currentValue = lv_slider_get_value((lv_obj_t*)_lvglObjectHandle);
            currentValue = Math.Clamp(currentValue, _minimum, _maximum);
            if (_value == currentValue)
            {
                return;
            }

            _value = currentValue;
            OnValueChanged(EventArgs.Empty);
        }
    }
}
