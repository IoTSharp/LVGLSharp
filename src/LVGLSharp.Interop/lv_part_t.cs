namespace LVGLSharp.Interop
{
    public enum lv_part_t : uint
    {
        LV_PART_MAIN = 0x000000,
        LV_PART_SCROLLBAR = 0x010000,
        LV_PART_INDICATOR = 0x020000,
        LV_PART_KNOB = 0x030000,
        LV_PART_SELECTED = 0x040000,
        LV_PART_ITEMS = 0x050000,
        LV_PART_CURSOR = 0x060000,
        LV_PART_CUSTOM_FIRST = 0x080000,
        LV_PART_ANY = 0x0F0000,
    }
}
