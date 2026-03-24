# LVGLSharp 路线图

更新时间：2026-03-24

## 当前判断

`LVGLSharp` 已经从“能不能跑起来”的阶段，进入“可以持续工程化迭代”的第一条基线。当前仓库已经具备清晰的包拆分、运行时边界、原生库打包方式、基础 CI 流水线以及统一的文档入口。

下一阶段的重点不再是从零搭架子，而是三件事：

1. 继续收拢依赖边界，降低包关系的理解成本。
2. 补齐已经存在宿主路径的验证深度，明确哪些是稳定可用、哪些是实验性实现。
3. 把 `DRM/KMS`、`macOS`、Remote 等骨架路径逐步推进成可落地能力。

## 已完成基线

### 包与发布

- 已切换到 `LVGL 9.5` 基线，并以 `9.5.0.5` 作为当前稳定发布说明版本。
- 已建立当前包布局：
  - `LVGLSharp.Forms`
  - `LVGLSharp.Core`
  - `LVGLSharp.Interop`
  - `LVGLSharp.Native`
  - `LVGLSharp.Runtime.Windows`
  - `LVGLSharp.Runtime.Linux`
  - `LVGLSharp.Runtime.Headless`
  - `LVGLSharp.Runtime.MacOs`
  - `LVGLSharp.Runtime.Remote`
- 已具备 `runtimes/{rid}/native` 形式的多 RID 原生库分发能力。
- GitHub Actions 已拆分为可复用的 prepare/build/pack/publish 阶段。

### 运行时架构

- `buildTransitive` 已接入运行时注册生成逻辑。
- `ApplicationConfiguration.Initialize()` 已成为 `LVGLSharp.Forms` 路径的标准初始化入口。
- Windows 路径已通过 `Win32View` 打通。
- Linux 路径已通过 `LinuxView` 统一宿主选择。
- 无头渲染已独立为 `LVGLSharp.Runtime.Headless` 与 `OffscreenView`。
- 原生库探测已经集中到 `LvglNativeLibraryResolver`。

### 示例、验证与文档

- 仓库内已有 `WinFormsDemo`、`PictureBoxDemo`、`MusicDemo`、`SmartWatchDemo`、`OffscreenDemo`、`MacOsAotDemo` 等示例工程。
- `tests/LVGLSharp.Headless.Tests` 已提供首批无头快照回归入口。
- 文档站已统一收敛到 <https://lvglsharp.net/>。
- `README.md`、`ROADMAP.md` 与 `CHANGELOG.md` 已开始按同一套状态语言维护。

## 当前状态矩阵

| 方向 | 状态 | 说明 |
|---|---|---|
| WinForms API 兼容层 | 可用 | 核心控件、`Form` 生命周期、基础布局模式已经具备 |
| LVGL 互操作层 | 可用 | 全量 P/Invoke 绑定已在仓库内 |
| Windows 运行时 | 可用 | `Win32View` 已是当前支持路径 |
| Linux `WSLg` / `X11` | 可用 | 当前桌面侧主路径 |
| Linux `FrameBuffer` | 可用 | 当前设备侧主路径 |
| Linux `Wayland` / `SDL` | 实验性 | 已实现，但验证覆盖与发布纪律仍需增强 |
| Headless `Offscreen` | 可用（持续补验证） | 已具备 PNG 快照、自动化入口和首批回归测试 |
| Linux `DRM/KMS` | 骨架中 | `DrmView` 已预留，原生后端尚未完成 |
| macOS 运行时 | 骨架中 | `MacOsViewOptions`、`MacOsHostDiagnostics`、`MacOsHostContext`、`MacOsFrameBuffer` 等骨架已存在 |
| Remote 运行时（`VNC` / `RDP`） | 骨架中 | 会话抽象、输入事件、帧编码与 transport skeleton 已存在 |
| 原生打包与 CI | 可用 | multi-RID 原生资产与分阶段 workflow 已建立 |
| 文档站与发布文档 | 可用 | 已有统一入口，但仍需随着功能演进持续修订 |

## 近期优先级

### 1. 把 `LVGLSharp.Native` 的依赖所有权收拢到运行时包

目标结果：

- `LVGLSharp.Interop` 回到纯绑定层定位。
- `LVGLSharp.Runtime.Windows` 与 `LVGLSharp.Runtime.Linux` 成为原生依赖的直接拥有者。
- demo 与 NuGet 使用方更容易理解“选运行时包，就得到对应原生载荷”的关系。

这是当前最值得优先推进的一步，因为它能直接减少包图复杂度，也会让后续支持矩阵更清晰。

### 2. 完成 `DRM/KMS` 第一版原生后端

当前 `DrmView` 已经有入口，但还缺真正可运行的后端实现。这个方向一旦补齐，会让 Linux 设备侧路径更完整。

### 3. 扩大 `Headless` 快照验证与 Remote 复用

重点包括：

- 扩大 `OffscreenView` 的快照回归覆盖范围。
- 让 `OffscreenDemo` 更适合作为稳定的验证入口。
- 持续打磨 `Headless -> Remote` 的帧源适配链路。

### 4. 建立宿主支持矩阵和自动化验证

应优先围绕这些路径补齐验证：

- `WSLg`
- `X11`
- `Wayland`
- `SDL`
- `FrameBuffer`

目标不是“能启动”就算完成，而是要具备可重复验证和可回归检查的基础。

### 5. 推进 `macOS` 与 Remote 从骨架走向可用实现

这两条路径当前已经不是空白，但离“可对外承诺的运行时”还有明显距离。后续应先补宿主后端，再逐步补发布、验证与示例说明。

## 建议起步顺序

如果现在要继续往下做，推荐顺序是：

1. 收拢 `LVGLSharp.Native` 依赖所有权。
2. 完成 `DrmView` 背后的原生实现。
3. 扩大 `OffscreenView` 与快照测试覆盖。
4. 补 `WSLg`、`X11`、`Wayland`、`SDL`、`FrameBuffer` 的验证矩阵。
5. 再推进 `macOS` 与 Remote 路径。

## 长期方向

这些方向仍然有效，但不属于当前最优先的近期工作：

- `DirectFB`
- `Mir`

它们适合放在 `DRM/KMS`、`Headless` 验证和现有宿主支持矩阵更稳定之后再推进。
