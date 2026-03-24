---
title: NuGet Guide
description: Install help for the main LVGLSharp packages, with clearer scenario-based guidance and a minimal example.
lang: en
template: structured
hero:
  eyebrow: "NuGet"
  title: "Install help, package choices, and a minimal example"
  lead: "This page summarizes the main `LVGLSharp` NuGet packages, the recommended package combinations, and the smallest practical setup to get you moving."
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
    - "NativeAOT"
  code_title: "Install Example"
  code: |
    dotnet add package LVGLSharp.Forms
    dotnet add package LVGLSharp.Runtime.Windows
    dotnet add package LVGLSharp.Runtime.Linux
  note_title: "How to choose packages"
  note_text: "For a first integration, start with `LVGLSharp.Forms` and then add the runtime package that matches your target platform. Add both Windows and Linux runtimes only if your project is intentionally multi-platform."
stats:
  - label: "Current baseline"
    value: "9.5.0.5"
  - label: "Published packages"
    value: "6"
  - label: "Pending packages"
    value: "3"
  - label: "Startup sample"
    value: "2 snippets"
  - label: "Hosting org"
    value: "IoTSharp"
sections:
  - title: "Main packages"
    description: "These packages cover most first-time integration paths."
    variant: cards
    columns: 3
    items:
      - title: "LVGLSharp.Forms"
        description: "The main WinForms-style compatibility layer and the normal entry point for application code."
      - title: "LVGLSharp.Runtime.Windows"
        description: "Windows host runtime for desktop development and validation."
      - title: "LVGLSharp.Runtime.Linux"
        description: "Linux host runtime for WSLg, X11, Wayland, and related runtime paths."
      - title: "LVGLSharp.Runtime.Headless"
        description: "Headless rendering support for snapshots, regression checks, and automation scenarios."
      - title: "LVGLSharp.Interop"
        description: "Low-level LVGL P/Invoke bindings, usually brought in transitively by higher-level packages."
      - title: "LVGLSharp.Native"
        description: "Platform-native LVGL libraries and packaging support."
  - title: "Choose by scenario"
    description: "If you do not want to learn the full package structure first, start from the scenario that matches your goal."
    variant: quick-links
    columns: 3
    items:
      - title: "Windows-only validation"
        description: "Start with `LVGLSharp.Forms` + `LVGLSharp.Runtime.Windows` for the most direct desktop setup."
        url: "https://github.com/IoTSharp/LVGLSharp"
      - title: "Cross-platform validation"
        description: "Add both Windows and Linux runtimes if you plan to build or verify across multiple hosts."
        url: "/en/navigation.html"
      - title: "Snapshots and automation"
        description: "Add `LVGLSharp.Runtime.Headless` when you need screenshots, headless rendering, or regression checks."
        url: "/en/preview-local.html"
  - title: "Recommended install path"
    description: "If you are evaluating the project for the first time, this is the simplest order."
    variant: list
    ordered: true
    surface: true
    items:
      - label: "Add `LVGLSharp.Forms` first"
      - label: "Add `LVGLSharp.Runtime.Windows` or `LVGLSharp.Runtime.Linux` based on your target"
      - label: "Add `LVGLSharp.Runtime.Headless` only when you need screenshots or automation"
      - label: "Keep `ApplicationConfiguration.Initialize()` as the unified startup entry"
---

## Package Overview

This table follows the same presentation used on the repository homepage. Version and download counts are shown through live NuGet badges.

| NuGet package | Version | Downloads | Description |
|---|---|---|---|
| `LVGLSharp.Forms` | [![LVGLSharp.Forms](https://img.shields.io/nuget/v/LVGLSharp.Forms.svg)](https://www.nuget.org/packages/LVGLSharp.Forms/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Forms) | WinForms-style compatibility layer and `buildTransitive` integration entry point. |
| `LVGLSharp.Core` | [![LVGLSharp.Core](https://img.shields.io/nuget/v/LVGLSharp.Core.svg)](https://www.nuget.org/packages/LVGLSharp.Core/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Core) | Shared runtime abstractions, fonts, and common helpers. |
| `LVGLSharp.Interop` | [![LVGLSharp.Interop](https://img.shields.io/nuget/v/LVGLSharp.Interop.svg)](https://www.nuget.org/packages/LVGLSharp.Interop/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Interop) | LVGL P/Invoke bindings. |
| `LVGLSharp.Native` | [![LVGLSharp.Native](https://img.shields.io/nuget/v/LVGLSharp.Native.svg)](https://www.nuget.org/packages/LVGLSharp.Native/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Native) | Multi-RID native library distribution package. |
| `LVGLSharp.Runtime.Windows` | [![LVGLSharp.Runtime.Windows](https://img.shields.io/nuget/v/LVGLSharp.Runtime.Windows.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.Windows/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.Windows) | Windows runtime. |
| `LVGLSharp.Runtime.Linux` | [![LVGLSharp.Runtime.Linux](https://img.shields.io/nuget/v/LVGLSharp.Runtime.Linux.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.Linux/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.Linux) | Linux runtime. |
| `LVGLSharp.Runtime.Headless` | Pending release | - | Headless rendering, snapshots, and automation runtime. |
| `LVGLSharp.Runtime.MacOs` | Pending release | - | macOS runtime skeleton and diagnostics structure. |
| `LVGLSharp.Runtime.Remote` | Pending release | - | Remote abstraction layer with `VNC` / `RDP` skeletons. |

The first 6 packages are currently published on NuGet. `LVGLSharp.Runtime.Headless`, `LVGLSharp.Runtime.MacOs`, and `LVGLSharp.Runtime.Remote` are still pending standalone publication.

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
