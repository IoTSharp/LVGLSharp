using LVGLSharp.Interop;

namespace LVGLSharp
{
    public unsafe interface IView : IDisposable
    {
        lv_obj_t* Root { get; }

        lv_group_t* KeyInputGroup { get; }

        delegate* unmanaged[Cdecl]<lv_event_t*, void> SendTextAreaFocusCallback { get; }

        void Open();

        void HandleEvents();

        void RunLoop(Action iteration);

        void Close();

        void RegisterTextInput(lv_obj_t* textArea);
    }
}
