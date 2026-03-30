---
title: NuGet Guide
description: Install guidance for the official LVGLSharp NuGet packages, repository-side supporting libraries, and the smallest practical setup.
lang: en
template: structured
hero:
  eyebrow: "NuGet"
  title: "Install help, package choices, and a minimal example"
  lead: "This page summarizes the current 9 official `LVGLSharp` NuGet packages and 3 repository-side supporting libraries so you can decide which combination fits your scenario."
  actions:
    - label: "Open repository"
      url: "https://github.com/IoTSharp/LVGLSharp"
      style: secondary
    - label: "Read the docs map"
      url: "/en/navigation.html"
      style: primary
  tags:
    - "LVGLSharp.Forms"
    - "Runtime.Windows"
    - "Runtime.Linux"
    - "Runtime.Headless"
  code_title: "Install Example"
  code: |
    dotnet add package LVGLSharp.Forms
    dotnet add package LVGLSharp.Runtime.Windows
    dotnet add package LVGLSharp.Runtime.Linux
  note_title: "How to choose packages"
  note_text: "For a first integration, start with `LVGLSharp.Forms` plus the runtime package that matches your target host. Add `LVGLSharp.Runtime.Headless` for snapshots and automation. Add Remote or macOS packages only when you are intentionally exploring those paths."
stats:
  - label: "Current baseline"
    value: "9.5.0.5"
  - label: "Official NuGet packages"
    value: "9"
  - label: "Supporting libraries"
    value: "3"
  - label: "Version and downloads"
    value: "Live badges"
  - label: "Hosting org"
    value: "IoTSharp"
sections:
  - title: "Official NuGet packages"
    description: "These packages are part of the current public NuGet publishing line."
    variant: cards
    columns: 3
    items:
      - title: "LVGLSharp.Forms"
        description: "The main WinForms-style compatibility layer and the normal entry point for application code."
      - title: "LVGLSharp.Core"
        description: "Shared runtime abstractions, fonts, diagnostics, and helper infrastructure."
      - title: "LVGLSharp.Interop"
        description: "Low-level LVGL P/Invoke bindings for advanced integrations."
      - title: "LVGLSharp.Native"
        description: "Platform-native LVGL assets and publish-time targets."
      - title: "LVGLSharp.Runtime.Windows"
        description: "Windows desktop runtime for validation and application hosting."
      - title: "LVGLSharp.Runtime.Linux"
        description: "Linux runtime covering WSLg, X11, Wayland, SDL, and FrameBuffer paths."
      - title: "LVGLSharp.Runtime.Headless"
        description: "Headless rendering support for snapshots, regression checks, and automation."
      - title: "LVGLSharp.Runtime.MacOs"
        description: "macOS runtime boundary package with diagnostics and early host scaffolding."
      - title: "LVGLSharp.Runtime.Remote"
        description: "Remote runtime abstractions for sessions, frames, input, and VNC/RDP-oriented work."
  - title: "Repository-side supporting libraries"
    description: "These libraries are part of the repository layering but are not in the main public publishing workflow."
    variant: cards
    columns: 3
    items:
      - title: "LVGLSharp.Drawing"
        description: "Cross-platform drawing primitives without a direct `System.Drawing` dependency."
      - title: "LVGLSharp.WPF"
        description: "Experimental WPF-like bootstrap and XAML runtime loader."
      - title: "LVGLSharp.Analyzers"
        description: "Roslyn analyzers that are normally brought in transitively by `LVGLSharp.Forms`."
  - title: "Choose by scenario"
    description: "If you do not want to learn the full package structure first, start from the scenario that matches your goal."
    variant: quick-links
    columns: 4
    items:
      - title: "Windows-only validation"
        description: "Start with `LVGLSharp.Forms` + `LVGLSharp.Runtime.Windows`."
        url: "https://www.nuget.org/packages/LVGLSharp.Runtime.Windows/"
      - title: "Cross-platform validation"
        description: "Add both Windows and Linux runtimes if you plan to build or verify across multiple hosts."
        url: "/en/navigation.html"
      - title: "Snapshots and automation"
        description: "Add `LVGLSharp.Runtime.Headless` when you need screenshots, headless rendering, or regression checks."
        url: "/en/preview-local.html"
      - title: "Remote display work"
        description: "Add `LVGLSharp.Runtime.Remote` only when you are intentionally building remote-session scenarios."
        url: "https://www.nuget.org/packages/LVGLSharp.Runtime.Remote/"
  - title: "Recommended install path"
    description: "If you are evaluating the project for the first time, this is the simplest order."
    variant: list
    ordered: true
    surface: true
    items:
      - label: "Add `LVGLSharp.Forms` first"
      - label: "Add `LVGLSharp.Runtime.Windows` or `LVGLSharp.Runtime.Linux` based on your target"
      - label: "Add `LVGLSharp.Runtime.Headless` only when you need snapshots or automation"
      - label: "Add `LVGLSharp.Runtime.Remote` or `LVGLSharp.Runtime.MacOs` only for deliberate advanced exploration"
      - label: "Keep `ApplicationConfiguration.Initialize()` as the unified startup entry"
---

## Official Package Overview

The table below uses live NuGet badges for versions and download counts. The repository development baseline remains `9.5.0.5`.

| NuGet package | Version | Downloads | Description |
|---|---|---|---|
| `LVGLSharp.Forms` | [![LVGLSharp.Forms](https://img.shields.io/nuget/v/LVGLSharp.Forms.svg)](https://www.nuget.org/packages/LVGLSharp.Forms/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Forms) | Main application-facing package with WinForms-style APIs and runtime registration entry points. |
| `LVGLSharp.Core` | [![LVGLSharp.Core](https://img.shields.io/nuget/v/LVGLSharp.Core.svg)](https://www.nuget.org/packages/LVGLSharp.Core/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Core) | Shared runtime abstractions, fonts, diagnostics, and host helpers. |
| `LVGLSharp.Interop` | [![LVGLSharp.Interop](https://img.shields.io/nuget/v/LVGLSharp.Interop.svg)](https://www.nuget.org/packages/LVGLSharp.Interop/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Interop) | Auto-generated low-level LVGL P/Invoke bindings. |
| `LVGLSharp.Native` | [![LVGLSharp.Native](https://img.shields.io/nuget/v/LVGLSharp.Native.svg)](https://www.nuget.org/packages/LVGLSharp.Native/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Native) | RID-specific native LVGL assets and publish-time targets. |
| `LVGLSharp.Runtime.Windows` | [![LVGLSharp.Runtime.Windows](https://img.shields.io/nuget/v/LVGLSharp.Runtime.Windows.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.Windows/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.Windows) | Windows desktop runtime with Win32 hosting support. |
| `LVGLSharp.Runtime.Linux` | [![LVGLSharp.Runtime.Linux](https://img.shields.io/nuget/v/LVGLSharp.Runtime.Linux.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.Linux/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.Linux) | Linux runtime covering WSLg, X11, Wayland, SDL, and FrameBuffer paths. |
| `LVGLSharp.Runtime.Headless` | [![LVGLSharp.Runtime.Headless](https://img.shields.io/nuget/v/LVGLSharp.Runtime.Headless.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.Headless/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.Headless) | Headless runtime for offscreen rendering, snapshots, and automation. |
| `LVGLSharp.Runtime.MacOs` | [![LVGLSharp.Runtime.MacOs](https://img.shields.io/nuget/v/LVGLSharp.Runtime.MacOs.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.MacOs/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.MacOs) | Early macOS runtime package with diagnostics and host scaffolding. |
| `LVGLSharp.Runtime.Remote` | [![LVGLSharp.Runtime.Remote](https://img.shields.io/nuget/v/LVGLSharp.Runtime.Remote.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.Remote/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.Remote) | Remote-session abstractions, frame transport, and VNC/RDP-oriented runtime pieces. |

## Repository-side Supporting Libraries

| Library | Publishing model | Description |
|---|---|---|
| `LVGLSharp.Drawing` | Repository-side support library | Cross-platform drawing primitives shared by the runtime and UI layers. |
| `LVGLSharp.WPF` | Experimental repository library | WPF-like bootstrap and XAML runtime loader built on top of `LVGLSharp.Forms` and `LVGLSharp.Runtime.Windows`. |
| `LVGLSharp.Analyzers` | Bundled with `LVGLSharp.Forms` | Roslyn analyzers for runtime-package combinations and other repository constraints. |

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
```

```csharp
ApplicationConfiguration.Initialize();
Application.Run(new MainForm());
```
