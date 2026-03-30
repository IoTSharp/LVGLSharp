---
title: 架构拆解：从 Forms 到 Runtime，再到底层 LVGL
description: 理解 LVGLSharp 如何通过 Forms、Core、Drawing、Interop、Native、Runtime 和工具层组织跨平台 GUI 能力。
lang: zh-CN
---

# LVGLSharp 架构拆解：从 Forms 到 Runtime，再到底层 LVGL

> 如果你想先把整个工程的层次看清楚，这篇文章会带你从应用层一直看到运行时、原生资产和文档发布层，重点看分层边界、职责拆分和长期可维护性。

## 为什么这个项目需要分层

如果要同时满足：

- WinForms 风格 API
- 跨平台运行
- 多宿主图形环境
- NativeAOT 友好
- 可打包、可发布、可维护

那么项目不可能只靠一个大程序集解决问题。

`LVGLSharp` 目前最重要的进展之一，就是它已经逐步形成了比较清楚的包与库分层。

## 第一层：Forms 应用兼容层

`LVGLSharp.Forms` 面向应用开发者，负责暴露：

- `Form`
- `Control`
- 常用控件
- 事件与生命周期语义
- `ApplicationConfiguration.Initialize()` 入口体验

这是最接近 WinForms 使用体验的一层，也是大多数应用的首个入口包。

## 第二层：Core 与 Drawing 共享基础层

这层主要由 `LVGLSharp.Core` 和 `LVGLSharp.Drawing` 组成。

`LVGLSharp.Core` 负责：

- 公共抽象
- 视图生命周期基础设施
- 字体与图片辅助能力
- 原生库解析和诊断能力

`LVGLSharp.Drawing` 负责：

- `Point`
- `Size`
- `Rectangle`
- `Color`
- 其他不依赖 `System.Drawing` 的跨平台基础类型

这层存在的意义，是让上层 Forms 和下层 Runtime 不直接耦死。

## 第三层：Interop 绑定层

`LVGLSharp.Interop` 让 .NET 可以完整访问 LVGL C API。

这一层的存在说明项目并不想把自己锁死在“高级封装”里，而是保留向下直达 LVGL 的能力。

## 第四层：Native 分发层

`LVGLSharp.Native` 负责原生库分发。

这是一个很关键但常被忽视的工程层：

- 没有它，多平台原生库分发会很混乱
- 有了它，NuGet 包结构和 CI 打包阶段都能更清晰
- 运行时包可以通过它稳定拿到各自需要的原生资产

## 第五层：平台与场景运行时层

当前运行时包已经不只剩 Windows 和 Linux 两条主线，而是扩展成一组更清楚的运行时边界：

- `LVGLSharp.Runtime.Windows`
- `LVGLSharp.Runtime.Linux`
- `LVGLSharp.Runtime.Headless`
- `LVGLSharp.Runtime.MacOs`
- `LVGLSharp.Runtime.Remote`

它们各自负责的方向大致是：

- Windows 宿主
- Linux 宿主与多图形后端
- 无头渲染与快照验证
- macOS 宿主边界与诊断骨架
- Remote 会话、帧传输与输入抽象

这样应用代码不用自己处理底层宿主差异，也不会把不同运行时路径硬绑在同一个程序集里。

## 第六层：补充应用模型与工具层

除了当前已经形成的 12 个包主线，仓库内还有几类在工程组织上特别关键的补充能力：

- `LVGLSharp.WPF`
- `LVGLSharp.Analyzers`

其中：

- `LVGLSharp.WPF` 用来承接实验性的 WPF 风格启动和 XAML 运行时加载路径
- `LVGLSharp.Analyzers` 用来在编译期校验运行时包引用关系和仓库约束

它们不是当前正式 NuGet 主线的一部分，但对于工程演进很重要。

## 第七层：CI、Packaging 与文档层

一个成熟方向的项目，不只是代码能跑，还要：

- 能打包
- 能发布
- 能解释自己

当前仓库里的 CI 拆分、NuGet 元数据、包级 README 和文档站，就是这一层职责的一部分。

## 结语

`LVGLSharp` 目前最值得肯定的一点，不只是它连接了 WinForms 和 LVGL，而是它正在形成一套完整的工程结构：

- 应用兼容层
- 共享基础层
- 底层绑定层
- 原生分发层
- 运行时层
- 工具与补充应用模型层
- 发布与文档工程层

这才是一个项目能长期演进的基础。
