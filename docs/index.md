# LVGLSharp.Forms

> 用 WinForms 的开发体验，连接 LVGL 的跨平台渲染能力。

[中文首页](./index.md) · [English Home](./index.en.md) · [中文导航](./navigation.md) · [English Navigation](./navigation.en.md)

---

## 双语入口

**当前语言：中文**

- 首页：[`index.md`](./index.md) · [`index.en.md`](./index.en.md)
- 导航：[`navigation.md`](./navigation.md) · [`navigation.en.md`](./navigation.en.md)
- 博客：[`blog/index.md`](./blog/index.md) · [`blog/index.en.md`](./blog/index.en.md)

> 建议：分享给中文读者可直接发送 `index.md`，需要双语对照时优先从导航页进入。

---

## 徽章区

![Status](https://img.shields.io/badge/status-experimental-orange)
![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux-blue)
![AOT](https://img.shields.io/badge/NativeAOT-supported-success)
![Rendering](https://img.shields.io/badge/rendering-LVGL-5c3ee8)
![License](https://img.shields.io/badge/license-MIT-green)

## 首页摘要

`LVGLSharp.Forms` 是一个面向跨平台、设备端与 NativeAOT 场景的 **WinForms API 兼容层**。它以 **LVGL** 为底层渲染引擎，试图在保留 WinForms 开发体验的同时，把 UI 运行能力扩展到 Linux、嵌入式和更轻量的运行环境中。

### 你可以在这里快速看到

- 项目做什么
- 为什么值得关注
- 当前支持了什么
- 架构如何组织
- 应该从哪篇文档开始读

---

## 语言切换

> 当前页面：**中文** ｜ 切换到：[English Home](./index.en.md) · [English Navigation](./navigation.en.md) · [English Blog](./blog/index.en.md)

---

## 按角色开始阅读

### 第一次了解项目

- [项目首页](./index.md)
- [为什么要做 WinForms over LVGL](./blog-winforms-over-lvgl.md)
- [英文首页](./index.en.md)

### 架构与工程读者

- [项目架构拆解](./blog-architecture.md)
- [CI 工作流说明](./ci-workflows.md)
- [CI Workflow Guide](./ci-workflows.en.md)

### Linux / 运行时宿主读者

- [Linux 图形宿主路线](./blog-linux-hosts.md)
- [WSL 开发指南](./WSL-Developer-Guide.md)
- [WSL Developer Guide](./WSL-Developer-Guide.en.md)

### AOT / 发布读者

- [NativeAOT 与 GUI](./blog-nativeaot-gui.md)
- [更新记录](../CHANGELOG.md)
- [路线图](../ROADMAP.md)

---

## 快速入口

## Start Here

如果你第一次来到这个项目，建议直接从这里开始：

1. 阅读本页，了解项目想解决什么问题
2. 阅读 [`ci-workflows.md`](./ci-workflows.md)，理解工程化结构
3. 阅读 [`ROADMAP.md`](../ROADMAP.md)，理解未来方向
4. 进入博客区，阅读设计与实现思路

---

## 功能卡片区

## 三列入口区

### 文档

- [文档导航](./navigation.md)
- [CI 工作流说明](./ci-workflows.md)
- [WSL 开发指南](./WSL-Developer-Guide.md)

### 博客

- [博客索引](./blog/index.md)
- [为什么要做 WinForms over LVGL](./blog-winforms-over-lvgl.md)
- [项目架构拆解](./blog-architecture.md)

### 仓库

- [README](../README.md)
- [路线图](../ROADMAP.md)
- [更新记录](../CHANGELOG.md)

---

## 项目亮点卡片

### WinForms 风格开发

保留熟悉的窗体、控件、事件和布局思维，降低迁移成本。

### LVGL 渲染内核

以下层 LVGL 驱动跨平台 UI，兼顾轻量、性能与设备适应性。

### NativeAOT 方向

面向设备端、自包含发布和最小运行时部署持续优化。

### 多运行时宿主

通过 Windows / Linux 运行时分层，当前已经落地 `WSLg`、`X11`、`FrameBuffer` 等主路径，`Wayland` / `SDL` 也已有实现，并继续为 `DRM/KMS` 等路线预留空间。

---

## 版本 / 平台 / 包结构摘要区

### 当前版本

- 当前文档化版本：`9.5.0.5`
- 发布标签：`v9.5.0.5`

### 当前平台

- Windows：`win-x86` / `win-x64` / `win-arm64`
- Linux：`linux-x64` / `linux-arm` / `linux-arm64`

### 当前包结构

- `LVGLSharp.Forms`
- `LVGLSharp.Core`
- `LVGLSharp.Interop`
- `LVGLSharp.Native`
- `LVGLSharp.Runtime.Windows`
- `LVGLSharp.Runtime.Linux`

### 入门阅读

- [CI 工作流说明](./ci-workflows.md)
- [WSL 开发指南](./WSL-Developer-Guide.md)
- [路线图](../ROADMAP.md)
- [更新记录](../CHANGELOG.md)

### 博客专题

- [为什么要做 WinForms over LVGL](./blog-winforms-over-lvgl.md)
- [NativeAOT 与 GUI](./blog-nativeaot-gui.md)
- [Linux 图形宿主路线](./blog-linux-hosts.md)
- [项目架构拆解](./blog-architecture.md)

---

## 最新文章

- [为什么要做 WinForms over LVGL](./blog-winforms-over-lvgl.md)
- [项目架构拆解](./blog-architecture.md)
- [NativeAOT 与 GUI](./blog-nativeaot-gui.md)
- [Linux 图形宿主路线](./blog-linux-hosts.md)

---

## 文章推荐卡片

### 新读者首选

从“项目为什么存在”切入，建议先读：

- [为什么要做 WinForms over LVGL](./blog-winforms-over-lvgl.md)

### 架构与工程读者

如果你更关心模块边界、包拆分与运行时分层，建议先读：

- [项目架构拆解](./blog-architecture.md)
- [CI 工作流说明](./ci-workflows.md)

### Linux / 设备方向读者

如果你关注 WSL、Linux 图形宿主与设备部署，建议先读：

- [Linux 图形宿主路线](./blog-linux-hosts.md)
- [WSL 开发指南](./WSL-Developer-Guide.md)

### AOT / 发布方向读者

如果你关注裁剪、自包含发布与 NativeAOT，建议先读：

- [NativeAOT 与 GUI](./blog-nativeaot-gui.md)

---

## 博客推荐区

### 推荐一：为什么要做 WinForms over LVGL

如果你最关心“这个项目为什么存在”，从这篇开始最合适。

### 推荐二：项目架构拆解

如果你更关心工程结构和模块边界，这篇最值得先读。

### 推荐三：NativeAOT 与 GUI

如果你更关心设备端部署和运行时约束，这篇会更有帮助。

---

## 项目简介

`LVGLSharp.Forms` 是一个基于 **LVGL** 的 **WinForms API 兼容层**。它的目标不是简单地“把 LVGL 包一层 .NET API”，而是希望在尽可能保留 WinForms 开发方式、窗体结构、控件模型和事件习惯的前提下，把同一套 UI 代码运行到 Windows、Linux 以及更广泛的嵌入式与设备侧环境中。

如果用一句话概括这个项目，它想做的是：

> **让开发者继续用熟悉的 WinForms 思维编写界面，同时把最终渲染能力交给 LVGL，从而获得跨平台、轻量、可 AOT、适合设备端部署的 UI 运行时。**

这个项目尤其适合以下场景：

- 已有 WinForms 经验，希望迁移到嵌入式或 Linux 设备端
- 希望在 .NET 生态下使用 LVGL，而不是回到纯 C/C++ 工程
- 希望获得 NativeAOT 发布能力，减少运行时依赖
- 希望在桌面端快速开发 UI，再把它部署到 ARM / ARM64 / x64 设备

---

## 为什么值得关注

在传统桌面开发中，WinForms 提供了极高的开发效率：

- 控件模型直接
- 事件模型成熟
- 设计器体验友好
- 业务代码结构稳定

但 WinForms 本身主要服务于 Windows 桌面场景，并不天然适合：

- Linux 图形环境
- 无桌面设备
- Framebuffer / DRM / KMS 等底层显示宿主
- ARM 设备上的轻量部署
- NativeAOT 的极致裁剪发布

另一方面，LVGL 在嵌入式、轻量设备 UI 和跨平台渲染方面非常强，但它的原生开发方式更偏底层，和 .NET / WinForms 的心智模型存在差异。

`LVGLSharp.Forms` 的价值就在于把两者连接起来：

- 上层使用接近 WinForms 的 API
- 下层使用 LVGL 作为统一渲染内核
- 中间通过运行时宿主、自动注册、P/Invoke 绑定和控件桥接把两套体系接通

---

## 项目定位

这个项目的定位不是“复刻完整 WinForms”，而是：

1. **提供高价值的 WinForms 兼容层**
2. **让常见窗体与控件开发方式可迁移**
3. **把平台差异收敛到运行时项目中**
4. **保持 NativeAOT 与跨平台友好性**
5. **围绕设备端、Linux、嵌入式场景持续演进**

因此，项目设计上强调：

- `Forms` 层 API 尽量稳定
- 平台差异尽量不泄漏到业务代码
- 宿主选择与初始化通过运行时自动完成
- 控件、布局、生命周期尽量兼容 WinForms 思维

---

## 当前能力概览

### 能力速览

| 方向 | 当前状态 |
|---|---|
| WinForms 风格 API | 已建立核心兼容层 |
| LVGL 全量互操作 | 已具备 |
| Windows Runtime | 已具备 |
| Linux Runtime 统一入口 | 已具备 |
| `Wayland` / `SDL` 宿主 | 已实现，当前偏实验性 |
| NativeAOT 方向 | 已明确支持并持续增强 |
| 多平台原生库打包 | 已具备 |
| GitHub Actions CI/CD | 已拆分成多阶段可复用工作流 |
| GitHub Pages 文档站点 | 已建立基础结构 |

### 已确认完成的近期事项

- 已新增 [`ROADMAP.md`](../ROADMAP.md)，统一记录当前里程碑、宿主成熟度与下一步优先事项。
- 已确认 Linux 侧宿主实现包含 `WslgView`、`X11View`、`FrameBufferView`、`WaylandView`、`SdlView`。
- 当前建议的下一步是把 `LVGLSharp.Native` 的依赖归拢到 Runtime 包中，让 `Interop` 回到纯绑定层职责。

### 1. WinForms API 兼容层

项目提供与 `System.Windows.Forms` 高度相似的 API 风格，重点覆盖：

- `Form`
- `Control`
- 常用容器控件
- 常用输入控件
- 常用展示控件
- 常见事件模型
- 基础窗体生命周期

已经具备的控件包括：

- `Button`
- `Label`
- `TextBox`
- `CheckBox`
- `RadioButton`
- `ComboBox`
- `ListBox`
- `ProgressBar`
- `TrackBar`
- `NumericUpDown`
- `PictureBox`
- `Panel`
- `GroupBox`
- `FlowLayoutPanel`
- `TableLayoutPanel`
- `RichTextBox`

### 2. 全量 LVGL 互操作能力

项目并不把自己限制在“仅暴露几个高级控件”层面，而是通过自动生成的 `P/Invoke` 绑定，把 **LVGL 全量 C API** 带到 .NET 世界中。

这意味着开发者既可以：

- 只使用 `LVGLSharp.Forms` 提供的 WinForms 风格控件

也可以：

- 在需要时直接下探到 LVGL 原生 API 做更底层、更精细的控制

### 3. NativeAOT 友好

项目在设计上强调 AOT 兼容性，不依赖脆弱的运行时反射激活路径，而是倾向使用可裁剪、可分析、可静态生成的方式完成初始化和宿主注册。

这使它非常适合：

- 设备端部署
- 单文件发布
- 自包含发布
- 极简运行环境

### 4. 跨平台运行时模型

当前项目已经具备 Windows 与 Linux 两条主要运行时路径：

- `LVGLSharp.Runtime.Windows`
- `LVGLSharp.Runtime.Linux`

并且通过 `buildTransitive` + 自动注册机制，使引用者不需要在业务入口里手动做大量平台初始化。

---

## 架构总览

从整体上看，仓库可以分为 6 个主要层次：

### 1. `LVGLSharp.WinForms`

这是项目最核心的兼容层，对外提供 WinForms 风格的控件、窗体和公共 UI API。

职责包括：

- 控件类实现
- 窗体生命周期桥接
- 布局控件封装
- 事件语义映射
- 与 LVGL 控件树的关系建立

### 2. `LVGLSharp.Core`

共享核心抽象层。

职责包括：

- 公共运行时抽象
- 视图生命周期基础类
- 公共图像/字体辅助能力
- 与平台无关的宿主支撑逻辑

### 3. `LVGLSharp.Interop`

LVGL 的 .NET 绑定层。

职责包括：

- 自动生成 P/Invoke 声明
- 映射 LVGL 原生结构、枚举、函数
- 为上层控件与运行时提供底层调用入口

### 4. `LVGLSharp.Native`

平台原生库分发层。

职责包括：

- 打包各平台 LVGL 原生动态库
- 按 `runtimes/{rid}/native` 组织分发
- 作为其他托管包的原生依赖基础

### 5. 平台运行时

当前包括：

- `LVGLSharp.Runtime.Windows`
- `LVGLSharp.Runtime.Linux`

职责包括：

- 按平台选择宿主
- 注册 `IView`
- 管理消息循环、显示缓冲、输入桥接
- 隔离平台差异

### 6. Demos

示例工程用于：

- 验证控件能力
- 验证多平台发布
- 演示运行效果
- 作为新特性的验证样本

---

## 平台与场景

### 当前支持的平台

- Windows
  - `win-x86`
  - `win-x64`
  - `win-arm64`
- Linux
  - `linux-x64`
  - `linux-arm`
  - `linux-arm64`

### 当前重点场景

- Windows 桌面开发与设计期验证
- Linux 桌面运行
- ARM / ARM64 设备发布
- NativeAOT 发布
- WSL / WSLg / X11 调试

### 已落地但成熟度不同的 Linux 宿主

- 当前更稳定、可持续说明的路径：`WSLg` / `X11` / `FrameBuffer`
- 已实现但仍偏实验性：`Wayland` / `SDL`

### 规划中的方向

根据项目路线图，未来还会逐步扩展：

- `DRM`
- `KMS`
- `Offscreen`
- `DirectFB`
- `Mir`
- `LVGLSharp.Runtime.MacOs`
- 远程运行时（如 `VNC` / `RDP`）

---

## 推荐开发模式

项目推荐采用**多目标框架**方式开发：

- `net10.0-windows`：走标准 WinForms 路径
- `net10.0`：走 `LVGLSharp.Forms` 路径

典型思路是：

- 在 Windows 上保留设计器友好体验
- 在跨平台目标上切换到 LVGL 驱动的 Forms 层
- 用同一套 UI 结构服务两个运行目标

这也是项目当前最具有实战价值的开发模式之一。

---

## LVGLSharp 布局

仓库中对推荐布局方式有一个专门术语：

> **LVGLSharp 布局**

它的核心规则是：

- 外层使用一个 `TableLayoutPanel` 做纵向分区
- 每一行内部放一个 `FlowLayoutPanel`
- 业务控件都挂在每行的 `FlowLayoutPanel` 上
- 主 `TableLayoutPanel` 使用固定绝对行高
- 不在主 `TableLayoutPanel` 上直接放业务控件

这样做的原因是：

- 更容易获得稳定、可预测的行布局
- 更适合 LVGL 后端在不同平台下保持一致排布
- 减少字体度量差异和宿主差异对布局的放大影响

对于博客读者来说，这一点很重要：

> `LVGLSharp.Forms` 不只是“把 WinForms API 翻译给 LVGL”，它还在逐步沉淀出一套更适合跨平台、设备端宿主的 UI 组织方法。

---

## 运行时与宿主模型

### Windows 路线

Windows 运行时负责：

- 选择 Windows 平台宿主
- 注册图像与平台辅助能力
- 保持与 WinForms 设计体验的协同

### Linux 路线

Linux 运行时当前统一通过 `LinuxView` 作为入口，再路由到具体宿主：

- `WslgView`
- `X11View`
- `FrameBufferView`
- `WaylandView`
- `SdlView`

这让项目可以：

- 在不同 Linux 环境下复用上层 API
- 将宿主差异留在运行时层处理
- 在同一入口下继续打磨 `Wayland`、`SDL` 等当前仍偏实验性的宿主路径

---

## AOT 与裁剪设计

这是本项目非常值得单独强调的一点。

很多 UI 框架或兼容层，在转向 NativeAOT / trimming 时容易遇到这些问题：

- 依赖反射动态创建类型
- 大量运行时注册依赖不可分析路径
- 通过注释压警告而不是根治问题

而这个项目明确强调：

- 不通过注释“压掉” AOT / trimming 警告
- 优先替换为 AOT 安全的显式实现
- 避免脆弱的运行时激活路径

这意味着它更适合长期演进为：

- 真正能部署到设备上的 .NET GUI 运行时
- 可以严肃讨论体积、启动速度、依赖收敛的工程系统

---

## NuGet 包设计

当前包结构清晰分层：

| 包名 | 职责 |
|---|---|
| `LVGLSharp.Forms` | WinForms API 兼容层 |
| `LVGLSharp.Core` | 共享抽象与公共运行时能力 |
| `LVGLSharp.Runtime.Windows` | Windows 平台运行时 |
| `LVGLSharp.Runtime.Linux` | Linux 平台运行时 |
| `LVGLSharp.Interop` | LVGL 全量 P/Invoke 绑定 |
| `LVGLSharp.Native` | 各平台原生库分发 |

这种拆分的好处是：

- 职责边界清晰
- 跨平台依赖可控
- 可以分别独立打包与发布
- 更适合 GitHub Actions 中做分阶段构建

---

## 示例工程

当前仓库包含多个 demo：

- `WinFormsDemo`
- `PictureBoxDemo`
- `MusicDemo`
- `SmartWatchDemo`
- `SerialPort`

这些示例不仅是“展示 UI 效果”，更承担了：

- 回归验证
- 平台发布验证
- 控件行为对照
- 运行时宿主验证

如果你打算写博客，这些 demo 是非常好的内容素材来源。

---

## 构建与发布体系

项目已经具备一套拆分明确的 GitHub Actions CI/CD：

- 准备发布元数据
- 构建原生库资产
- 构建 Demo 资产
- 打包 NuGet 资产
- 发布 Release 与 Packages

对应文档可参考：

- [`ci-workflows.md`](./ci-workflows.md)

当前 CI 设计特点：

- 分支 / PR 侧重验证
- tag 触发完整发布
- Demo 与 NuGet 包构建解耦
- prepare / pack / publish 都可复用

这使项目不仅“能跑”，而且具备了适合持续发布的工程形态。

---

## 文档站点与博客

为了更适合对外展示与博客化输出，仓库文档可以通过 GitHub Pages 发布。

推荐 Pages 内容包括：

- 项目总介绍（中文 / 英文）
- CI 工作流说明
- 路线图
- WSL 开发指南
- 未来可继续补充的宿主专题、AOT 专题、控件兼容专题

建议 Pages 首页结构：

- `docs/index.md`：中文首页
- `docs/index.en.md`：英文首页
- `docs/ci-workflows.md`：CI 说明
- `docs/WSL-Developer-Guide.md`：WSL 指南
- 后续按主题继续扩展

---

## 当前优先级判断

如果按当前仓库状态继续往前推进，建议优先做这三件事：

1. 将 `LVGLSharp.Native` 的依赖归拢到 `LVGLSharp.Runtime.Windows` / `LVGLSharp.Runtime.Linux`
2. 为 `Wayland` / `SDL` 增加 smoke test 与回归验证
3. 继续保持 `README`、`ROADMAP`、`CHANGELOG` 与 docs 首页对当前状态的描述一致

---

## 推荐阅读路径

如果你第一次来到这个项目，建议按下面顺序阅读：

1. 本页：理解项目定位与价值
2. [`ci-workflows.md`](./ci-workflows.md)：理解工程发布结构
3. [`ROADMAP.md`](../ROADMAP.md)：理解未来路线
4. 博客专题：理解设计思想和实现取舍

---

## 适合博客写作的几个角度

如果你要基于这个项目写系列博客，建议可以从以下方向展开：

### 1. 为什么要做 WinForms over LVGL

适合讲：

- 桌面开发体验与设备部署之间的断层
- 为什么不用纯 C 写 LVGL
- 为什么要保留 WinForms 的开发心智模型

### 2. 如何把 WinForms 迁移到跨平台 GUI

适合讲：

- 多目标框架
- 运行时自动注册
- 布局迁移
- 常见控件兼容问题

### 3. NativeAOT + GUI 的工程实践

适合讲：

- AOT 对 UI 框架设计的约束
- 为什么不能依赖反射路径
- 如何做可裁剪的运行时初始化

### 4. Linux 图形宿主演进路线

适合讲：

- X11 / WSLg / fbdev 的现状
- 为什么要继续打磨 Wayland / SDL，并推进 DRM / KMS
- 桌面环境与设备环境的不同需求

### 5. 从“项目能跑”到“工程可发布”

适合讲：

- NuGet 包分层
- 原生库分发
- CI/CD 拆分
- Release 与 Pages 的组织方式

---

## 项目现阶段的判断

`LVGLSharp.Forms` 当前仍处于实验阶段，但它已经不只是一个“概念验证工程”。

从仓库当前状态看，它已经具备了这些明显特征：

- 有清晰的架构层次
- 有明确的平台运行时边界
- 有逐步扩展的宿主路线图
- 有可持续发布的包结构
- 有正在成熟的 CI/CD 体系
- 有面向博客、文档站点和社区传播的表达基础

换句话说，它正在从：

- “一个有意思的方向”

逐渐变成：

- “一个有明确技术路线和工程化目标的跨平台 UI 框架项目”

---

## 适合谁关注这个项目

你可能会对这个项目感兴趣，如果你是：

- .NET 开发者
- WinForms 老用户
- 嵌入式 UI 开发者
- Linux 设备端应用开发者
- NativeAOT 爱好者
- 对 UI 框架桥接、跨平台运行时和工程化发布体系感兴趣的开发者

---

## 开源与社区

项目基于 MIT License 开源。

同时，它也非常适合通过：

- GitHub Issues
- GitHub Discussions
- Release Notes
- GitHub Pages
- 技术博客

不断沉淀社区共识与工程经验。

---

## 延伸阅读

你可以继续阅读：

- [`ci-workflows.md`](./ci-workflows.md)
- [`WSL-Developer-Guide.md`](./WSL-Developer-Guide.md)
- [`ROADMAP.md`](../ROADMAP.md)
- [`CHANGELOG.md`](../CHANGELOG.md)
- [`README.md`](../README.md)

---

## 结语

`LVGLSharp.Forms` 的意义不只是“让 WinForms 跑到 LVGL 上”。

它更像是在尝试回答一个长期存在的问题：

> 能不能保留 .NET 桌面开发的效率，同时获得 LVGL 在跨平台、轻量、设备端和 NativeAOT 方面的优势？

这个仓库给出的答案，是一条正在被认真推进的工程路线。

如果你正在寻找：

- WinForms 向跨平台迁移的桥梁
- LVGL 在 .NET 生态中的更高层抽象
- 适合设备部署的 .NET GUI 框架方向

那么这个项目，值得持续关注。
