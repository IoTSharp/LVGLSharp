using System.Runtime.CompilerServices;

namespace LVGLSharp.Interop
{
    public partial struct lv_draw_triangle_dsc_t
    {
        public lv_draw_dsc_base_t @base;

        [NativeTypeName("lv_point_precise_t[3]")]
        public _p_e__FixedBuffer p;

        public lv_color_t color;

        [NativeTypeName("lv_opa_t")]
        public byte opa;

        public lv_grad_dsc_t grad;

        [InlineArray(3)]
        public partial struct _p_e__FixedBuffer
        {
            public lv_point_precise_t e0;
        }
    }
}
