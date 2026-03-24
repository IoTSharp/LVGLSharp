# WinFormsRdpDemo

This demo hosts a `LVGLSharp.Forms` window through `RdpView` so we have a dedicated WinForms-facing entry point for the RDP runtime path.

## Current status

- The demo defaults to `RdpView`
- The placeholder listener binds `0.0.0.0:3389`
- Startup prints suggested local endpoints to the console
- The RDP protocol handshake is still under implementation

## Why it exists

- Keep a clean WinForms demo surface for the RDP path
- Validate runtime registration and view hosting without polluting `Program`
- Provide a stable place to continue the real RDP transport work
