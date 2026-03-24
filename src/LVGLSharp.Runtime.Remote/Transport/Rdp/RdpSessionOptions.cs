namespace LVGLSharp.Runtime.Remote.Rdp;

public sealed record RdpSessionOptions : RemoteSessionOptions
{
    public string Host { get; init; } = "127.0.0.1";

    public int Port { get; init; } = 3389;

    public int Width { get; init; } = 800;

    public int Height { get; init; } = 480;

    public string? Username { get; init; }

    public bool EnableNetworkAutoDetect { get; init; } = true;
}
