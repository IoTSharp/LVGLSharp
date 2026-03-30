# LVGLSharp

[中文](./README.md) | English

**LVGLSharp** is a cross-platform WinForms-style UI stack built on top of [LVGL](https://github.com/lvgl/lvgl). The main application-facing package is `LVGLSharp.Forms`, while the rest of the package lineup separates runtime hosting, native assets, low-level bindings, and supporting infrastructure.

Repository development baseline: `9.5.0.5`

- Documentation: <https://lvglsharp.net/>
- Roadmap: <https://github.com/IoTSharp/LVGLSharp/blob/main/ROADMAP.md>
- Changelog: <https://github.com/IoTSharp/LVGLSharp/blob/main/CHANGELOG.md>

> The project is still evolving quickly and should be evaluated carefully before production use.

## Features

- WinForms-style API compatibility with `Control`, `Form`, `Button`, `Label`, `TextBox`, `CheckBox`, `RadioButton`, `ComboBox`, `ListBox`, `PictureBox`, `Panel`, `GroupBox`, `FlowLayoutPanel`, `TableLayoutPanel`, `ProgressBar`, `TrackBar`, `NumericUpDown`, `RichTextBox`, and related control infrastructure.
- Full LVGL interop through `LVGLSharp.Interop`, generated with ClangSharpPInvokeGenerator so the complete LVGL C API remains reachable from .NET.
- Layered runtime packages for Windows, Linux, Headless, macOS, and Remote paths, with current stable emphasis on Windows, Linux, and Headless flows.
- NativeAOT-friendly publishing and multi-RID native asset distribution through `LVGLSharp.Native`.
- Automatic runtime registration through `buildTransitive` when runtime packages are referenced.
- Cross-platform drawing primitives via `LVGLSharp.Drawing` without a dependency on `System.Drawing`.

## Preview

The screenshots below come from the captured demo output under `docs/images`.

<p align="center">
  <img src="./docs/images/x11-pictureboxdemo.png" alt="LVGLSharp X11 PictureBoxDemo" width="48%" />
  <img src="./docs/images/x11-musicdemo.png" alt="LVGLSharp X11 MusicDemo" width="48%" />
</p>

<p align="center">
  <img src="./docs/images/x11-smartwatchdemo.png" alt="LVGLSharp X11 SmartWatchDemo" width="48%" />
  <img src="./docs/images/wslg-pictureboxdemo-wayland-embedded-font-check.png" alt="LVGLSharp WSLg Wayland Embedded Font Check" width="48%" />
</p>

<p align="center">
  <img src="./docs/images/winformsvncdemo-vnc-case.png" alt="LVGLSharp WinFormsVncDemo over VNC" width="48%" />
</p>

## Official NuGet Packages

The version and download columns below use live NuGet badges so the README stays aligned with what is actually published.

| Package | Version | Downloads | Description |
|---|---|---|---|
| `LVGLSharp.Forms` | [![LVGLSharp.Forms](https://img.shields.io/nuget/v/LVGLSharp.Forms.svg)](https://www.nuget.org/packages/LVGLSharp.Forms/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Forms) | Main WinForms-style application package and runtime-registration entry point. |
| `LVGLSharp.Core` | [![LVGLSharp.Core](https://img.shields.io/nuget/v/LVGLSharp.Core.svg)](https://www.nuget.org/packages/LVGLSharp.Core/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Core) | Shared runtime abstractions, fonts, diagnostics, and host helpers. |
| `LVGLSharp.Interop` | [![LVGLSharp.Interop](https://img.shields.io/nuget/v/LVGLSharp.Interop.svg)](https://www.nuget.org/packages/LVGLSharp.Interop/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Interop) | Auto-generated low-level LVGL P/Invoke bindings. |
| `LVGLSharp.Native` | [![LVGLSharp.Native](https://img.shields.io/nuget/v/LVGLSharp.Native.svg)](https://www.nuget.org/packages/LVGLSharp.Native/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Native) | RID-specific native LVGL assets and publish-time targets. |
| `LVGLSharp.Runtime.Windows` | [![LVGLSharp.Runtime.Windows](https://img.shields.io/nuget/v/LVGLSharp.Runtime.Windows.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.Windows/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.Windows) | Windows desktop runtime with Win32 hosting support. |
| `LVGLSharp.Runtime.Linux` | [![LVGLSharp.Runtime.Linux](https://img.shields.io/nuget/v/LVGLSharp.Runtime.Linux.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.Linux/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.Linux) | Linux runtime covering WSLg, X11, Wayland, SDL, and FrameBuffer paths. |
| `LVGLSharp.Runtime.Headless` | [![LVGLSharp.Runtime.Headless](https://img.shields.io/nuget/v/LVGLSharp.Runtime.Headless.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.Headless/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.Headless) | Headless runtime for offscreen rendering, snapshots, and automation. |
| `LVGLSharp.Runtime.MacOs` | [![LVGLSharp.Runtime.MacOs](https://img.shields.io/nuget/v/LVGLSharp.Runtime.MacOs.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.MacOs/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.MacOs) | Early macOS runtime package with diagnostics and host scaffolding. |
| `LVGLSharp.Runtime.Remote` | [![LVGLSharp.Runtime.Remote](https://img.shields.io/nuget/v/LVGLSharp.Runtime.Remote.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.Remote/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.Remote) | Remote-session abstractions, frame transport, and VNC/RDP-oriented runtime pieces. |

All 9 packages above are currently published on NuGet. The repository also contains several supporting libraries that are documented below but are not part of the main public publishing workflow.

## Repository-side Supporting Libraries

| Library | Status | Description |
|---|---|---|
| `LVGLSharp.Drawing` | Repository library | Cross-platform drawing primitives used by runtimes and UI layers. |
| `LVGLSharp.WPF` | Experimental repository library | WPF-like bootstrap and XAML runtime loader built on top of `LVGLSharp.Forms` and `LVGLSharp.Runtime.Windows`. |
| `LVGLSharp.Analyzers` | Bundled with `LVGLSharp.Forms` | Roslyn analyzers that validate runtime-package usage and related build-time patterns. |

`LVGLSharp.Forms` already carries the analyzers transitively. In most applications you start with `LVGLSharp.Forms` plus the runtime package that matches the host you want.

## Quick Start

### 1. Project file

The recommended starting point is the same multi-target pattern used by the demos in this repository:

```xml
<PropertyGroup>
  <TargetFrameworks>net10.0-windows;net10.0</TargetFrameworks>
</PropertyGroup>

<PropertyGroup Condition="'$(TargetFramework)' == 'net10.0-windows'">
  <UseWindowsForms>true</UseWindowsForms>
</PropertyGroup>

<PropertyGroup Condition="'$(TargetFramework)' == 'net10.0'">
  <UseLVGLSharpForms>true</UseLVGLSharpForms>
  <PublishAot>true</PublishAot>
</PropertyGroup>

<ItemGroup Condition="'$(TargetFramework)' == 'net10.0'">
  <PackageReference Include="LVGLSharp.Forms" Version="*" />
  <PackageReference Include="LVGLSharp.Runtime.Windows" Version="*" />
  <PackageReference Include="LVGLSharp.Runtime.Linux" Version="*" />
</ItemGroup>
```

Add `LVGLSharp.Runtime.Headless` when you need snapshots or automation. Add `LVGLSharp.Runtime.Remote` or `LVGLSharp.Runtime.MacOs` only when you are intentionally exploring those paths.

### 2. Entry point

The `UseLVGLSharpForms=true` target uses the normal startup pattern:

```csharp
ApplicationConfiguration.Initialize();
Application.Run(new MainForm());
```

### 3. Publish examples

```bash
dotnet publish -f net10.0 -r linux-arm64 -c Release
dotnet publish -f net10.0 -r linux-x64 -c Release
dotnet publish -f net10.0-windows -r win-x64 -c Release
```

## Current Runtime Status

| Area | Status | Notes |
|---|---|---|
| WinForms compatibility layer | Usable | Core controls, form lifecycle, and basic layout patterns are already available. |
| Windows runtime | Usable | One of the current stable host paths. |
| Linux `WSLg` / `X11` | Usable | Main desktop-side Linux validation path. |
| Linux `FrameBuffer` | Usable | Main device-side Linux path. |
| Linux `Wayland` / `SDL` | Experimental | Implemented, but still needs more validation and release discipline. |
| Headless `Offscreen` | Usable | Supports PNG snapshots, screenshots, and regression-entry scenarios. |
| Linux `DRM/KMS` | Scaffolded | `DrmView` exists, but the native backend still needs to be completed. |
| macOS runtime | Early package boundary | Diagnostics, context, and surface/frame-buffer scaffolding are in the repository and package. |
| Remote runtime | Early package boundary | VNC / RDP abstractions, sessions, and transport skeletons are present. |

See the roadmap for the fuller engineering status and priority order.

## Community

The documentation site, Issues, and the WeChat group are all valid ways to discuss usage, cross-platform adaptation, and troubleshooting.

![LVGLSharp WeChat Group](./preview/wechat-group.png)

## License

This project is licensed under the [MIT License](./LICENSE.txt).
