using LVGLSharp.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVGLSharp
{
    public unsafe interface IWindow
    {
        public static lv_obj_t* root { get; set; }
        public static lv_group_t* key_inputGroup { get; set; }
        public static delegate* unmanaged[Cdecl]<lv_event_t*, void> SendTextAreaFocusCb { get; set; }

        public void Init();
        public void StartLoop(Action handle);
    }
}
