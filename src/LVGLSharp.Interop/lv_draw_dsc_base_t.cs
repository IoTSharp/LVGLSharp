using System.Runtime.CompilerServices;

namespace LVGLSharp.Interop
{
    public unsafe partial struct lv_draw_dsc_base_t
    {
        public lv_obj_t* obj;

        [NativeTypeName("uint32_t")]
        public uint part;

        [NativeTypeName("uint32_t")]
        public uint id1;

        [NativeTypeName("uint32_t")]
        public uint id2;

        public lv_layer_t* layer;

        [NativeTypeName("int16_t")]
        public short drop_shadow_ofs_x;

        [NativeTypeName("int16_t")]
        public short drop_shadow_ofs_y;

        public lv_color_t drop_shadow_color;

        [NativeTypeName("lv_opa_t")]
        public byte drop_shadow_opa;

        [NativeBitfield("drop_shadow_blur_radius", offset: 0, length: 20)]
        [NativeBitfield("drop_shadow_quality", offset: 20, length: 3)]
        public int _bitfield;

        [NativeTypeName("int32_t : 20")]
        public int drop_shadow_blur_radius
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (_bitfield << 12) >> 12;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (_bitfield & ~0xFFFFF) | (value & 0xFFFFF);
            }
        }

        [NativeTypeName("lv_blur_quality_t : 3")]
        public lv_blur_quality_t drop_shadow_quality
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (lv_blur_quality_t)((_bitfield << 9) >> 29);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (_bitfield & ~(0x7 << 20)) | (((int)(value) & 0x7) << 20);
            }
        }

        [NativeTypeName("size_t")]
        public nuint dsc_size;

        public void* user_data;
    }
}
