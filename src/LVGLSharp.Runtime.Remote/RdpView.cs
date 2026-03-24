namespace LVGLSharp.Runtime.Remote;

public unsafe sealed class RdpView : RemoteViewBase
{
    public RdpView()
        : this(new Rdp.RdpTransportSkeleton(new Rdp.RdpSessionOptions()))
    {
    }

    public RdpView(Rdp.RdpSessionOptions options)
        : this(new Rdp.RdpTransportSkeleton(options))
    {
    }

    public RdpView(Rdp.RdpTransportSkeleton transport)
        : base(transport, transport.Options, transport.Options.Width, transport.Options.Height)
    {
    }

    public new Rdp.RdpTransportSkeleton Transport => (Rdp.RdpTransportSkeleton)base.Transport;
}
