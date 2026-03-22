namespace LVGLSharp.Forms
{
    [Flags]
    public enum PreProcessControlState
    {
        MessageProcessed = 0x0000,
        MessageNeeded = 0x0001,
        MessageNotNeeded = 0x0002,
    }
}