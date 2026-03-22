using System.Runtime.CompilerServices;

namespace LVGLSharp.Interop
{
    public partial struct lv_fs_path_ex_t
    {
        [NativeTypeName("char[64]")]
        public _path_e__FixedBuffer path;

        [InlineArray(64)]
        public partial struct _path_e__FixedBuffer
        {
            public sbyte e0;
        }
    }
}
