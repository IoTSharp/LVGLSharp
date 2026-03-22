namespace LVGLSharp.Interop
{
    public enum lv_state_t : uint
    {
        LV_STATE_DEFAULT = 0,
        LV_STATE_ALT = 1 << 0,
        LV_STATE_CHECKED = 1 << 2,
        LV_STATE_FOCUSED = 1 << 3,
        LV_STATE_FOCUS_KEY = 1 << 4,
        LV_STATE_EDITED = 1 << 5,
        LV_STATE_HOVERED = 1 << 6,
        LV_STATE_PRESSED = 1 << 7,
        LV_STATE_SCROLLED = 1 << 8,
        LV_STATE_DISABLED = 1 << 9,
        LV_STATE_USER_1 = 1 << 12,
        LV_STATE_USER_2 = 1 << 13,
        LV_STATE_USER_3 = 1 << 14,
        LV_STATE_USER_4 = 1 << 15,
        LV_STATE_ANY = 0xFFFF,
    }
}
