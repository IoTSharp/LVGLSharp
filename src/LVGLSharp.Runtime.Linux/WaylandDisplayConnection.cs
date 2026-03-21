using System;
using System.Runtime.InteropServices;

namespace LVGLSharp.Runtime.Linux;

internal sealed partial class WaylandDisplayConnection : IDisposable
{
    private const string WaylandClientLib = "libwayland-client.so.0";

    private IntPtr _display;

    [LibraryImport(WaylandClientLib, EntryPoint = "wl_display_connect", StringMarshalling = StringMarshalling.Utf8)]
    private static partial IntPtr WlDisplayConnect(string? name);

    [LibraryImport(WaylandClientLib, EntryPoint = "wl_display_disconnect")]
    private static partial void WlDisplayDisconnect(IntPtr display);

    public WaylandDisplayConnection(string? requestedDisplayName, string diagnosticSummary)
    {
        DiagnosticSummary = string.IsNullOrWhiteSpace(diagnosticSummary)
            ? throw new ArgumentException("Value cannot be null or whitespace.", nameof(diagnosticSummary))
            : diagnosticSummary;
        RequestedDisplayName = requestedDisplayName;
    }

    public string? RequestedDisplayName { get; }

    public string DiagnosticSummary { get; }

    public string? ConnectedDisplayName { get; private set; }

    public bool IsConnected => _display != IntPtr.Zero;

    public bool IsDisposed { get; private set; }

    public void Connect()
    {
        ThrowIfDisposed();

        if (IsConnected)
        {
            return;
        }

        _display = WlDisplayConnect(RequestedDisplayName);
        if (_display == IntPtr.Zero)
        {
            throw new InvalidOperationException($"Unable to connect to Wayland display. {DiagnosticSummary}");
        }

        ConnectedDisplayName = RequestedDisplayName;
    }

    public void Disconnect()
    {
        if (!IsConnected)
        {
            ConnectedDisplayName = null;
            return;
        }

        WlDisplayDisconnect(_display);
        _display = IntPtr.Zero;
        ConnectedDisplayName = null;
    }

    public void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
    }

    public void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        Disconnect();
        IsDisposed = true;
    }
}
