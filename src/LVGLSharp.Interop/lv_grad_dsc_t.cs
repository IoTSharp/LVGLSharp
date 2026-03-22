using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LVGLSharp.Interop
{
    public unsafe partial struct lv_grad_dsc_t
    {
        [NativeTypeName("lv_grad_stop_t[2]")]
        public _stops_e__FixedBuffer stops;

        [NativeTypeName("uint8_t")]
        public byte stops_count;

        [NativeBitfield("dir", offset: 0, length: 4)]
        [NativeBitfield("extend", offset: 4, length: 3)]
        public int _bitfield;

        [NativeTypeName("lv_grad_dir_t : 4")]
        public lv_grad_dir_t dir
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (lv_grad_dir_t)((_bitfield << 28) >> 28);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (_bitfield & ~0xF) | ((int)(value) & 0xF);
            }
        }

        [NativeTypeName("lv_grad_extend_t : 3")]
        public lv_grad_extend_t extend
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (lv_grad_extend_t)((_bitfield << 25) >> 29);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (_bitfield & ~(0x7 << 4)) | (((int)(value) & 0x7) << 4);
            }
        }

        [NativeTypeName("__AnonymousRecord_lv_grad_L67_C5")]
        public _params_e__Union @params;

        public void* state;

        [StructLayout(LayoutKind.Explicit)]
        public partial struct _params_e__Union
        {
            [FieldOffset(0)]
            [NativeTypeName("__AnonymousRecord_lv_grad_L69_C9")]
            public _linear_e__Struct linear;

            [FieldOffset(0)]
            [NativeTypeName("__AnonymousRecord_lv_grad_L74_C9")]
            public _radial_e__Struct radial;

            [FieldOffset(0)]
            [NativeTypeName("__AnonymousRecord_lv_grad_L82_C9")]
            public _conical_e__Struct conical;

            public partial struct _linear_e__Struct
            {
                public lv_point_t start;

                public lv_point_t end;
            }

            public partial struct _radial_e__Struct
            {
                public lv_point_t focal;

                public lv_point_t focal_extent;

                public lv_point_t end;

                public lv_point_t end_extent;
            }

            public partial struct _conical_e__Struct
            {
                public lv_point_t center;

                [NativeTypeName("int16_t")]
                public short start_angle;

                [NativeTypeName("int16_t")]
                public short end_angle;
            }
        }

        [InlineArray(2)]
        public partial struct _stops_e__FixedBuffer
        {
            public lv_grad_stop_t e0;
        }
    }
}
