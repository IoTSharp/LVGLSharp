namespace LVGLSharp.Interop
{
    public partial struct lv_calendar_date_t
    {
        [NativeTypeName("uint16_t")]
        public ushort year;

        [NativeTypeName("uint8_t")]
        public byte month;

        [NativeTypeName("uint8_t")]
        public byte day;
    }
}
