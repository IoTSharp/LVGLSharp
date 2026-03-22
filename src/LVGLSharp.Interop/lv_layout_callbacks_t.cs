namespace LVGLSharp.Interop
{
    public unsafe partial struct lv_layout_callbacks_t
    {
        [NativeTypeName("lv_layout_update_cb_t")]
        public delegate* unmanaged[Cdecl]<lv_obj_t*, void*, void> layout_update_cb;

        [NativeTypeName("lv_layout_get_min_size_cb_t")]
        public delegate* unmanaged[Cdecl]<lv_obj_t*, int*, c_bool1, void*, c_bool1> get_min_size_cb;
    }
}
