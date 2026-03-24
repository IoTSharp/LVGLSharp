---
title: NuGet 安装
description: 说明如何安装 LVGLSharp 的主要 NuGet 包，并给出更清晰的场景化安装指导和最小示例。
lang: zh-CN
template: structured
hero:
  eyebrow: "NuGet"
  title: "安装指导、包选择与最小示例"
  lead: "这一页集中说明 `LVGLSharp` 常用包的职责、推荐组合和一个最小可运行入口，方便你快速判断该引用哪些包。"
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
    - "NativeAOT"
  code_title: "Install Example"
  code: |
    dotnet add package LVGLSharp.Forms
    dotnet add package LVGLSharp.Runtime.Windows
    dotnet add package LVGLSharp.Runtime.Linux
  note_title: "怎么选包"
  note_text: "如果只是先跑通整体开发体验，先从 `LVGLSharp.Forms` + 对应运行时包开始。是否同时引用 Windows 和 Linux 运行时，取决于你是否真的要做跨平台目标构建。"
stats:
  - label: "当前基线版本"
    value: "9.5.0.5"
  - label: "已发布包"
    value: "6"
  - label: "待发布包"
    value: "3"
  - label: "最小入口"
    value: "2 段代码"
  - label: "发布组织"
    value: "IoTSharp"
sections:
  - title: "常用包"
    description: "下面这些包已经覆盖大多数初始接入场景。"
    variant: cards
    columns: 3
    items:
      - title: "LVGLSharp.Forms"
        description: "主要的 WinForms 风格 API 兼容层，也是大多数应用接入时的入口包。"
      - title: "LVGLSharp.Runtime.Windows"
        description: "Windows 运行时宿主实现，适合桌面开发与 Windows 平台验证。"
      - title: "LVGLSharp.Runtime.Linux"
        description: "Linux 运行时宿主实现，适合 WSLg、X11、Wayland 等方向的验证与扩展。"
      - title: "LVGLSharp.Runtime.Headless"
        description: "用于无头渲染、截图回归、自动化验证和远程帧源场景。"
      - title: "LVGLSharp.Interop"
        description: "底层 LVGL P/Invoke 绑定，通常由上层包间接带入。"
      - title: "LVGLSharp.Native"
        description: "各平台对应的原生 LVGL 库与打包支持。"
  - title: "按场景安装"
    description: "如果你不想先理解全部包关系，可以直接按使用场景选择。"
    variant: quick-links
    columns: 3
    items:
      - title: "只跑 Windows"
        description: "如果你现在只想先在 Windows 上跑通，先装 `LVGLSharp.Forms` + `LVGLSharp.Runtime.Windows` 就够了。"
        url: "https://github.com/IoTSharp/LVGLSharp"
      - title: "做跨平台验证"
        description: "同时加上 Windows 和 Linux 运行时，方便多目标构建和宿主切换。"
        url: "/zh/navigation.html"
      - title: "做截图与自动化"
        description: "如果你要做快照、测试和无头渲染，再额外加入 `LVGLSharp.Runtime.Headless`。"
        url: "/zh/preview-local.html"
  - title: "推荐安装路径"
    description: "如果你是第一次接入，建议按这个顺序走。"
    variant: list
    ordered: true
    surface: true
    items:
      - label: "先添加 `LVGLSharp.Forms`"
      - label: "按目标平台添加 `LVGLSharp.Runtime.Windows` 或 `LVGLSharp.Runtime.Linux`"
      - label: "如果需要截图与自动化，再增加 `LVGLSharp.Runtime.Headless`"
      - label: "保持 `ApplicationConfiguration.Initialize()` 作为统一入口"
---

## NuGet 包概览

下表与仓库首页保持一致，版本与下载量均由 NuGet 实时徽章显示。

| NuGet名称 | 版本 | 下载量 | 说明 |
|---|---|---|---|
| `LVGLSharp.Forms` | [![LVGLSharp.Forms](https://img.shields.io/nuget/v/LVGLSharp.Forms.svg)](https://www.nuget.org/packages/LVGLSharp.Forms/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Forms) | WinForms API 兼容层与 `buildTransitive` 集成入口。 |
| `LVGLSharp.Core` | [![LVGLSharp.Core](https://img.shields.io/nuget/v/LVGLSharp.Core.svg)](https://www.nuget.org/packages/LVGLSharp.Core/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Core) | 共享运行时抽象、字体与公共辅助能力。 |
| `LVGLSharp.Interop` | [![LVGLSharp.Interop](https://img.shields.io/nuget/v/LVGLSharp.Interop.svg)](https://www.nuget.org/packages/LVGLSharp.Interop/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Interop) | LVGL P/Invoke 绑定。 |
| `LVGLSharp.Native` | [![LVGLSharp.Native](https://img.shields.io/nuget/v/LVGLSharp.Native.svg)](https://www.nuget.org/packages/LVGLSharp.Native/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Native) | 多 RID 原生库分发包。 |
| `LVGLSharp.Runtime.Windows` | [![LVGLSharp.Runtime.Windows](https://img.shields.io/nuget/v/LVGLSharp.Runtime.Windows.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.Windows/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.Windows) | Windows 运行时。 |
| `LVGLSharp.Runtime.Linux` | [![LVGLSharp.Runtime.Linux](https://img.shields.io/nuget/v/LVGLSharp.Runtime.Linux.svg)](https://www.nuget.org/packages/LVGLSharp.Runtime.Linux/) | ![NuGet](https://img.shields.io/nuget/dt/LVGLSharp.Runtime.Linux) | Linux 运行时。 |
| `LVGLSharp.Runtime.Headless` | 待发布 | - | 无头渲染、截图与自动化验证运行时。 |
| `LVGLSharp.Runtime.MacOs` | 待发布 | - | macOS 运行时骨架与诊断结构。 |
| `LVGLSharp.Runtime.Remote` | 待发布 | - | Remote 抽象层与 `VNC` / `RDP` 骨架。 |

当前已发布到 NuGet 的是前 6 个包；`LVGLSharp.Runtime.Headless`、`LVGLSharp.Runtime.MacOs` 与 `LVGLSharp.Runtime.Remote` 仍待后续单独发布。

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
