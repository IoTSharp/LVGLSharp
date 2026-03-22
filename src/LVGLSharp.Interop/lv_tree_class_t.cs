namespace LVGLSharp.Interop
{
    public unsafe partial struct lv_tree_class_t
    {
        [NativeTypeName("const lv_tree_class_t *")]
        public lv_tree_class_t* base_class;

        [NativeTypeName("uint32_t")]
        public uint instance_size;

        [NativeTypeName("lv_tree_constructor_cb_t")]
        public delegate* unmanaged[Cdecl]<lv_tree_class_t*, lv_tree_node_t*, void> constructor_cb;

        [NativeTypeName("lv_tree_destructor_cb_t")]
        public delegate* unmanaged[Cdecl]<lv_tree_class_t*, lv_tree_node_t*, void> destructor_cb;
    }
}
