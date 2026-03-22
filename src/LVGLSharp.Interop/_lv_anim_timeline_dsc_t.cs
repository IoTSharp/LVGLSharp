using System.Runtime.CompilerServices;

namespace LVGLSharp.Interop
{
    public partial struct _lv_anim_timeline_dsc_t
    {
        public lv_anim_t anim;

        [NativeTypeName("uint32_t")]
        public uint start_time;

        [NativeBitfield("is_started", offset: 0, length: 1)]
        [NativeBitfield("is_completed", offset: 1, length: 1)]
        public byte _bitfield;

        [NativeTypeName("uint8_t : 1")]
        public byte is_started
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (byte)(_bitfield & 0x1u);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (byte)((_bitfield & ~0x1u) | (value & 0x1u));
            }
        }

        [NativeTypeName("uint8_t : 1")]
        public byte is_completed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (byte)((_bitfield >> 1) & 0x1u);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (byte)((_bitfield & ~(0x1u << 1)) | ((value & 0x1u) << 1));
            }
        }
    }
}
