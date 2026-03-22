namespace LVGLSharp.Interop
{
    public unsafe partial struct lv_layer_t
    {
        public lv_draw_buf_t* draw_buf;

        public lv_draw_task_t* draw_task_head;

        public lv_layer_t* parent;

        public lv_layer_t* next;

        public void* user_data;

        public lv_area_t buf_area;

        public lv_area_t phy_clip_area;

        public lv_area_t _clip_area;

        [NativeTypeName("int32_t")]
        public int partial_y_offset;

        public lv_color32_t recolor;

        public lv_color_format_t color_format;

        [NativeTypeName("bool")]
        public c_bool1 all_tasks_added;

        [NativeTypeName("lv_opa_t")]
        public byte opa;
    }
}
