# Changelog

本文件记录 LVGLSharp 的所有重要变更，遵循 [Keep a Changelog](https://keepachangelog.com/zh-CN/1.1.0/) 规范，版本号遵循 [语义化版本](https://semver.org/lang/zh-CN/)。

All notable changes to this project will be documented in this file.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/).

---

## [未发布 / Unreleased]

### 新增 / Added

- 新增 `ROADMAP.md`，用于统一记录当前能力边界、支持状态与下一阶段优先级。
- 新增无头渲染路径：`LVGLSharp.Runtime.Headless`、`OffscreenView`、`OffscreenOptions`、`OffscreenDemo` 与 `tests/LVGLSharp.Headless.Tests`，用于 PNG 导出、快照回归和自动化验证。
- 在 `LVGLSharp.Runtime.Linux` 中新增 `DrmView` 骨架，为后续 `DRM/KMS` 宿主实现预留入口。
- 新增 `LVGLSharp.Runtime.MacOs` 与 `MacOsAotDemo`，补齐 `MacOsViewOptions`、`IMacOsSurface`、`MacOsSurfaceSkeleton`、`MacOsHostDiagnostics`、`MacOsHostContext` 和 `MacOsFrameBuffer` 等骨架类型。
- 新增 `LVGLSharp.Runtime.Remote`，补齐协议无关会话/输入/帧编码抽象、`HeadlessRemoteFrameSource`，以及 `VNC` / `RDP` 第一版 transport skeleton 与工厂入口。

### 变更 / Changed

- `LVGLSharp.Forms` 改为通过 `buildTransitive` 自动生成平台运行时注册代码，`ApplicationConfiguration.Initialize()` 成为标准初始化入口。
- `LVGLSharp.Interop` 与部分 demo 对 `LVGLSharp.Native` 改为按构建配置切换已发布包 / 本地项目，并统一引入 `LVGLSharpNativePackageVersion`。
- Linux 宿主描述更新：`Wayland` 与 `SDL` 调整为“已实现但偏实验性”，`Offscreen` 从 `LinuxView` 宿主分支独立为 `Headless` 运行时。
- 整理 `README.md`、`ROADMAP.md` 与 `CHANGELOG.md` 的对外口径：README 首页仅保留 `lvglsharp.net` 作为文档入口，同时补上路线图、变更日志、预览图与完整包清单。

### 修复 / Fixed

- 修复多处文档中指向不存在文件的链接。
- 修复 `LVGLSharp.Runtime.Headless` 中 `OffscreenView` 背景色未真正应用到 root 的问题，并为 `RGB565` 快照回归测试补充量化容差断言。
- 修正文档中的 `LVGLSharp.Drawing` 名称以及包/状态描述不一致问题。

---

## [9.5.0.5] - 2026-03-23

### 发布说明 / Release Notes

- 这是围绕 `LVGL 9.5` 基线、`LVGLSharp` 仓库名和当前包职责整理后的首个完整文档化发布，对应 `v9.5.0.5`。
- 本版本重点在于沉淀已经完成的功能边界、运行时结构、包职责以及发布路径，便于后续基于 tag 进行持续发布。

### 新增 / Added

- WinForms API 兼容层核心框架，基于 LVGL 渲染引擎。
- 支持控件：`Button`、`Label`、`TextBox`、`CheckBox`、`RadioButton`、`ComboBox`、`ListBox`、`ProgressBar`、`TrackBar`、`NumericUpDown`、`PictureBox`、`Panel`、`GroupBox`、`FlowLayoutPanel`、`TableLayoutPanel`、`RichTextBox`。
- `LVGLSharp.Drawing` 命名空间：跨平台绘图类型 `Size`、`Point`、`Color` 等，不依赖 `System.Drawing`。
- NativeAOT 支持（`win-x64`、`linux-arm`、`linux-arm64`、`linux-x64`）。
- 基于 ClangSharpPInvokeGenerator 自动生成的 LVGL 全量 P/Invoke 绑定（`LVGLSharp.Interop`）。
- 平台原生库分发包 `LVGLSharp.Native`，支持 `win-x64`、`win-x86`、`win-arm64`、`linux-x64`、`linux-arm`、`linux-arm64`。
- LVGL GCHandle 事件桥接机制：通过 `[UnmanagedCallersOnly]` 静态回调将 LVGL 事件路由到托管控件事件。
- `Application.Run(Form)` 生命周期支持。
- `WinFormsDemo` 演示项目。

### 变更 / Changed

- README 中补充当前发布版本、发布定位与发布记录入口说明。
- 统一发布工作流示例版本，以及 README 与 CHANGELOG 中的发布标识为 `9.5.0.5` / `v9.5.0.5`。

### 修复 / Fixed

- 无。

---

## [9.5.0] - 升级到 LVGL 9.5 / Upgrade to LVGL 9.5

### 新增 / Added

- 项目初始化，基于 [imxcstar/LVGLSharp](https://github.com/imxcstar/LVGLSharp) 构建底层 LVGL .NET 封装。
- 引入 LVGL 9.5 作为 git submodule。
- 基础 `Control` 与 `Form` 类实现，支持 `Controls` 层级管理及 LVGL 对象创建。
- 初步验证 NativeAOT 发布流程（`win-x64`、`linux-arm`）。

---

## 致谢 / Acknowledgements

- **[imxcstar / LVGLSharp](https://github.com/imxcstar/LVGLSharp)**：提供了最底层的 LVGL .NET 封装支撑。
- **[LVGL](https://github.com/lvgl/lvgl)**：轻量级、高性能的嵌入式 GUI 库。
- **[ClangSharpPInvokeGenerator](https://github.com/dotnet/ClangSharp)**：用于自动生成 LVGL P/Invoke 绑定。
- **[SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp)**：跨平台图像处理库。
- **[SixLabors.Fonts](https://github.com/SixLabors/Fonts)**：跨平台字体解析库。
