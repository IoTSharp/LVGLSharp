using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace LVGLSharp.Runtime.Remote.Rdp;

public sealed class RdpTransportSkeleton : RemoteTransportBase, IRemoteHostedTransport, IDisposable
{
    private TcpListener? _listener;
    private CancellationTokenSource? _cts;
    private Task? _acceptTask;
    private RemoteFrame? _latestFrame;
    private int _latestFrameRevision;
    private int _connectionAttempts;
    private int _handshakeWarningLogged;
    private IRemoteInputSink? _inputSink;

    public RdpTransportSkeleton(RdpSessionOptions options)
        : base("rdp", new RemoteTransportCapabilities(
            SupportsClipboardSync: true,
            SupportsPointerInput: true,
            SupportsKeyboardInput: true,
            SupportsFrameStreaming: true))
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
        Options.Validate();
    }

    public RdpSessionOptions Options { get; }

    public int LatestFrameRevision => Volatile.Read(ref _latestFrameRevision);

    public int ConnectionAttempts => Volatile.Read(ref _connectionAttempts);

    public void Start()
    {
        if (_cts != null)
        {
            return;
        }

        _cts = new CancellationTokenSource();
        _listener = new TcpListener(IPAddress.Parse(Options.Host), Options.Port);
        _listener.Start();
        _acceptTask = Task.Run(() => AcceptLoop(_cts.Token));
    }

    public void Stop()
    {
        var cts = _cts;
        if (cts == null)
        {
            return;
        }

        _cts = null;

        try
        {
            cts.Cancel();
            _listener?.Stop();

            if (_acceptTask is not null)
            {
                _acceptTask.Wait(TimeSpan.FromSeconds(1));
            }
        }
        catch
        {
        }
        finally
        {
            _listener = null;
            _acceptTask = null;
            cts.Dispose();
        }
    }

    void IRemoteHostedTransport.AttachInputSink(IRemoteInputSink inputSink)
    {
        _inputSink = inputSink ?? throw new ArgumentNullException(nameof(inputSink));
    }

    public override Task SendFrameAsync(RemoteFrame frame, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(frame);
        _latestFrame = frame;
        Interlocked.Increment(ref _latestFrameRevision);
        return Task.CompletedTask;
    }

    public override Task SendInputAsync(RemoteInputEvent inputEvent, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(inputEvent);

        _inputSink?.PostInput(inputEvent);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Stop();
    }

    private async Task AcceptLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                var client = await _listener!.AcceptTcpClientAsync(token);
                Interlocked.Increment(ref _connectionAttempts);
                _ = Task.Run(() => RejectClientAsync(client, token), token);
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                break;
            }
            catch (ObjectDisposedException) when (token.IsCancellationRequested)
            {
                break;
            }
            catch (SocketException) when (token.IsCancellationRequested)
            {
                break;
            }
            catch
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }
            }
        }
    }

    private Task RejectClientAsync(TcpClient client, CancellationToken token)
    {
        using (client)
        {
            if (Interlocked.Exchange(ref _handshakeWarningLogged, 1) == 0)
            {
                Console.WriteLine("RDP transport accepted a TCP connection, but protocol handshake is still under implementation.");
            }

            if (!token.IsCancellationRequested)
            {
                try
                {
                    client.Client.Shutdown(SocketShutdown.Both);
                }
                catch
                {
                }
            }
        }

        return Task.CompletedTask;
    }
}
