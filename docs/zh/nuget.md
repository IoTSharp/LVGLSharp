---
title: NuGet 安装
description: 说明 LVGLSharp 的正式 NuGet 包、仓库内附带库、推荐组合以及最小接入方式。
lang: zh-CN
template: structured
hero:
  eyebrow: "NuGet"
  title: "安装指导、包选择与最小示例"
  lead: "这一页集中说明 `LVGLSharp` 现有 9 个正式 NuGet 包与 3 个仓库内附带库的职责、推荐组合和最小入口，方便你快速判断该引用哪些库。"
  actions:
    - label: "查看源码仓库"
      url: "https://github.com/IoTSharp/LVGLSharp"
      style: secondary
    - label: "阅读文档导航"
      url: "/zh/navigation.html"
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
  note_title: "怎么选包"
  note_text: "如果只是先跑通整体开发体验，先从 `LVGLSharp.Forms` + 对应运行时包开始。快照与自动化再加 `LVGLSharp.Runtime.Headless`；Remote 和 macOS 路径只在你明确需要时再单独加入。"
stats:
  - label: "当前基线版本"
    value: "9.5.0.5"
  - label: "正式 NuGet 包"
    value: "9"
  - label: "仓库附带库"
    value: "3"
  - label: "版本与下载量"
    value: "实时徽章"
  - label: "发布组织"
    value: "IoTSharp"
sections:
  - title: "正式 NuGet 包"
    description: "这些包已经进入当前正式的 NuGet 发布口径。"
    variant: cards
    columns: 3
    items:
      - title: "LVGLSharp.Forms"
        description: "主要的 WinForms 风格 API 兼容层，也是大多数应用接入时的入口包。"
      - title: "LVGLSharp.Core"
        description: "共享运行时抽象、字体、诊断与公共辅助能力。"
      - title: "LVGLSharp.Interop"
        description: "底层 LVGL P/Invoke 绑定，适合高级集成和低层控制。"
      - title: "LVGLSharp.Native"
        description: "各平台对应的原生 LVGL 库与发布时目标文件。"
      - title: "LVGLSharp.Runtime.Windows"
        description: "Windows 运行时宿主实现，适合桌面开发与验证。"
      - title: "LVGLSharp.Runtime.Linux"
        description: "Linux 运行时宿主实现，覆盖 WSLg、X11、Wayland、SDL 与 FrameBuffer 路径。"
      - title: "LVGLSharp.Runtime.Headless"
        description: "用于无头渲染、截图回归、自动化验证和远程帧源场景。"
      - title: "LVGLSharp.Runtime.MacOs"
        description: "macOS 运行时包边界，当前提供诊断、上下文与宿主骨架。"
      - title: "LVGLSharp.Runtime.Remote"
        description: "远程运行时抽象，覆盖会话、帧编码、输入事件与 VNC/RDP 方向。"
  - title: "仓库内附带库"
    description: "这些库同样是仓库分层的一部分，但不属于当前正式 NuGet 发布主线。"
    variant: cards
    columns: 3
    items:
      - title: "LVGLSharp.Drawing"
        description: "跨平台绘图基础类型，避免直接依赖 `System.Drawing`。"
      - title: "LVGLSharp.WPF"
        description: "实验性的 WPF 风格启动层与 XAML 运行时加载能力。"
      - title: "LVGLSharp.Analyzers"
        description: "Roslyn 分析器，由 `LVGLSharp.Forms` 传递给应用项目。"
  - title: "按场景安装"
    description: "如果你不想先理解全部包关系，可以直接按使用场景选择。"
    variant: quick-links
    columns: 4
    items:
      - title: "只跑 Windows"
        description: "先装 `LVGLSharp.Forms` + `LVGLSharp.Runtime.Windows`。"
        url: "https://www.nuget.org/packages/LVGLSharp.Runtime.Windows/"
      - title: "做跨平台验证"
        description: "同时加上 Windows 和 Linux 运行时，方便多目标构建和宿主切换。"
        url: "/zh/navigation.html"
      - title: "做截图与自动化"
        description: "加入 `LVGLSharp.Runtime.Headless`，用于 PNG 快照、测试与 CI 验证。"
        url: "/zh/preview-local.html"
      - title: "做远程显示"
        description: "需要远程会话、帧传输或 VNC/RDP 骨架时再加 `LVGLSharp.Runtime.Remote`。"
        url: "https://www.nuget.org/packages/LVGLSharp.Runtime.Remote/"
  - title: "推荐安装路径"
    description: "如果你是第一次接入，建议按这个顺序走。"
    variant: list
    ordered: true
    surface: true
    items:
      - label: "先添加 `LVGLSharp.Forms`"
      - label: "按目标平台添加 `LVGLSharp.Runtime.Windows` 或 `LVGLSharp.Runtime.Linux`"
      - label: "如果需要截图与自动化，再增加 `LVGLSharp.Runtime.Headless`"
      - label: "只有在明确探索远程或 macOS 路径时，再增加 `LVGLSharp.Runtime.Remote` 或 `LVGLSharp.Runtime.MacOs`"
      - label: "保持 `ApplicationConfiguration.Initialize()` 作为统一入口"
---

## 正式 NuGet 包概览

下表的版本与下载量均由 NuGet 实时徽章显示；仓库当前开发基线仍以 `9.5.0.5` 为准。

| NuGet 名称 | 版本 | 下载量 | 说明 |
|---|---|---|---|
| `LVGLSharp.Forms` | [![LVGLSharp.Forms](https://img.shields.io/nuget/v/LVGLSharp.Forms.svg)](https://www.nuget.org/packages/LVGLSharp.Forms/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Forms) | 主应用入口包，提供 WinForms 风格 API 与运行时注册入口。 |
| `LVGLSharp.Core` | [![LVGLSharp.Core](https://img.shields.io/nuget/v/LVGLSharp.Core.svg)](https://www.nuget.org/packages/LVGLSharp.Core/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Core) | 共享运行时抽象、字体、诊断与宿主辅助能力。 |
| `LVGLSharp.Interop` | [![LVGLSharp.Interop](https://img.shields.io/nuget/v/LVGLSharp.Interop.svg)](https://www.nuget.org/packages/LVGLSharp.Interop/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Interop) | 自动生成的 LVGL 底层 P/Invoke 绑定。 |
| `LVGLSharp.Native` | [![LVGLSharp.Native](https://img.shields.io/nuget/v/LVGLSharp.Native.svg)](https://www.nuget.org/packages/LVGLSharp.Native/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Native) | 多 RID 原生 LVGL 资产与发布时目标文件。 |
| `LVGLSharp.Runtime.Windows` | [![LVGLSharp.Runtime.Windows](https://img.shields.io/nuget/v/LVGLSharp.Runtime.Windows.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.Windows/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.Windows) | Windows 桌面运行时与 Win32 宿主支持。 |
| `LVGLSharp.Runtime.Linux` | [![LVGLSharp.Runtime.Linux](https://img.shields.io/nuget/v/LVGLSharp.Runtime.Linux.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.Linux/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.Linux) | Linux 运行时，覆盖 WSLg、X11、Wayland、SDL 与 FrameBuffer 路径。 |
| `LVGLSharp.Runtime.Headless` | [![LVGLSharp.Runtime.Headless](https://img.shields.io/nuget/v/LVGLSharp.Runtime.Headless.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.Headless/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.Headless) | 无头渲染、PNG 快照、回归验证与自动化运行时。 |
| `LVGLSharp.Runtime.MacOs` | [![LVGLSharp.Runtime.MacOs](https://img.shields.io/nuget/v/LVGLSharp.Runtime.MacOs.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.MacOs/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.MacOs) | 早期 macOS 运行时包，已包含诊断与宿主骨架。 |
| `LVGLSharp.Runtime.Remote` | [![LVGLSharp.Runtime.Remote](https://img.shields.io/nuget/v/LVGLSharp.Runtime.Remote.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.Remote/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.Remote) | 远程会话抽象、帧传输与 VNC/RDP 相关运行时能力。 |

## 仓库内附带库

| 库 | 发布方式 | 说明 |
|---|---|---|
| `LVGLSharp.Drawing` | 仓库辅助库 | 运行时与 UI 层共享的跨平台绘图基础类型。 |
| `LVGLSharp.WPF` | 仓库实验库 | 基于 `LVGLSharp.Forms` 和 `LVGLSharp.Runtime.Windows` 的 WPF 风格启动与 XAML 运行时加载层。 |
| `LVGLSharp.Analyzers` | 由 `LVGLSharp.Forms` 携带 | 用于校验运行时包引用关系和仓库约束的 Roslyn 分析器。 |

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
