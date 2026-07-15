# LVGLSharp 路线图

更新时间：2026-07-16

## 状态图例

| 标记 | 含义 |
|---|---|
| ✅ | 已完成，已有本地验证结果或稳定基线 |
| 🔄 | 进行中，已有实现但仍有闭环工作 |
| 🧪 | 待验证，需要补自动化或实机验证 |
| ⏳ | 待开始，尚未作为当前主线推进 |

## 当前判断

`LVGLSharp` 已经从“能不能跑起来”的阶段，进入“可以持续工程化迭代”的第一条基线。当前仓库已经具备清晰的包拆分、运行时边界、原生库打包方式、基础 CI 流水线以及统一的文档入口。

下一阶段不应优先扩展更多宿主，而应按顺序先打牢测试、原生加载、包依赖所有权和已有宿主验证闭环。只有这些基础稳定之后，`DRM/KMS`、macOS 和 Remote 的推进才不会继续叠在不稳定基础上。

## 已完成基线

### ✅ 包与发布

1. ✅ 已切换到 `LVGL 9.5` 基线，并以 `9.5.0.5` 作为当前稳定发布说明版本。
2. ✅ 已建立当前包布局：
   - `LVGLSharp.Forms`
   - `LVGLSharp.Core`
   - `LVGLSharp.Interop`
   - `LVGLSharp.Native`
   - `LVGLSharp.Runtime.Windows`
   - `LVGLSharp.Runtime.Linux`
   - `LVGLSharp.Runtime.Headless`
   - `LVGLSharp.Runtime.MacOs`
   - `LVGLSharp.Runtime.Remote`
3. ✅ 已具备 `runtimes/{rid}/native` 形式的多 RID 原生库分发能力。
4. ✅ GitHub Actions 已拆分为可复用的 prepare/build/pack/publish 阶段。

### ✅ 运行时架构

1. ✅ `buildTransitive` 已接入运行时注册生成逻辑。
2. ✅ `ApplicationConfiguration.Initialize()` 已成为 `LVGLSharp.Forms` 路径的标准初始化入口。
3. ✅ Windows 路径已通过 `Win32View` 打通。
4. ✅ Linux 路径已通过 `LinuxView` 统一宿主选择。
5. ✅ 无头渲染已独立为 `LVGLSharp.Runtime.Headless` 与 `OffscreenView`。
6. ✅ 原生库探测已经集中到 `LvglNativeLibraryResolver`。

### ✅ 示例、验证与文档

1. ✅ 仓库内已有 `WinFormsDemo`、`PictureBoxDemo`、`MusicDemo`、`SmartWatchDemo`、`OffscreenDemo`、`MacOsAotDemo` 等示例工程。
2. ✅ `tests/LVGLSharp.Headless.Tests` 已提供首批无头快照回归入口。
3. ✅ 文档站已统一收敛到 <https://lvglsharp.net/>。
4. ✅ `README.md`、`ROADMAP.md` 与 `CHANGELOG.md` 已开始按同一套状态语言维护。

## 当前状态矩阵

| 方向 | 状态 | 说明 |
|---|---|---|
| WinForms API 兼容层 | ✅ 可用 | 核心控件、`Form` 生命周期、基础布局模式已经具备 |
| LVGL 互操作层 | ✅ 可用 | 全量 P/Invoke 绑定已在仓库内 |
| Windows 运行时 | ✅ 可用 | `Win32View` 已是当前支持路径 |
| Linux `WSLg` / `X11` | ✅ 可用 | 当前桌面侧主路径 |
| Linux `FrameBuffer` | ✅ 可用 | 当前设备侧主路径 |
| Linux `Wayland` / `SDL` | 🧪 实验性 | 已实现，但验证覆盖、截图验证和发布纪律仍需增强 |
| Headless `Offscreen` | ✅ 本地闭环 | 本地 Debug/Release 全量测试已通过，native 加载不再依赖手工 `PATH` |
| Linux `DRM/KMS` | 🔄 推进中 | `DrmView` 已有托管入口，但 native 符号、构建选项和设备验证未闭环 |
| macOS 运行时 | 🔄 早期实现 | 已有 Cocoa/Objective-C surface、输入和 framebuffer 路径，但仍缺实机自动化验证 |
| Remote 运行时（`VNC` / `RDP`） | 🔄 早期实现 | VNC transport 已有基础实现，RDP 仍是 skeleton；Remote 应优先复用 Headless frame source |
| 原生打包与 CI | 🔄 待远端验证 | multi-RID 原生资产已建立，本地测试闭环已修复，Headless Release gate 已接入 CI |
| 文档站与发布文档 | ✅ 可用 | 已有统一入口，但仍需随着功能演进持续修订 |

## 顺序路线图

### 1. ✅ 修复本地 Headless 测试与原生加载闭环

已完成内容：

1. ✅ 测试工程显式导入 `src/LVGLSharp.Native/buildTransitive/LVGLSharp.Native.targets`。
2. ✅ Debug 与 Release 输出目录都会复制 `runtimes/{rid}/native` 资产。
3. ✅ Headless 测试程序集禁用并行，避免多个测试同时操作 LVGL 进程级 native 状态。
4. ✅ 本地测试不再依赖手工设置 `PATH`。

已验证命令：

1. ✅ `dotnet test tests\LVGLSharp.Headless.Tests\LVGLSharp.Headless.Tests.csproj --configuration Debug --no-restore`
2. ✅ `dotnet test tests\LVGLSharp.Headless.Tests\LVGLSharp.Headless.Tests.csproj --configuration Release --no-restore`
3. ✅ `dotnet test tests\LVGLSharp.Headless.Tests\LVGLSharp.Headless.Tests.csproj --configuration Release --no-build`

完成判定：

1. ✅ Headless 测试本地 Debug/Release 全量稳定通过。
2. ✅ Release `--no-build` 可以直接运行，说明 native 文件已经在输出目录中。
3. ✅ 测试不依赖执行顺序，也不依赖外部 `PATH`。

### 2. 🔄 把 Headless 测试接入 CI 阻断关卡

已完成内容：

1. ✅ 新增 `.github/workflows/headless-tests.yml`，在 `windows-latest` 上执行 Headless Release build、native 资产检查和 `dotnet test --no-build`。
2. ✅ `ci-build.yml` 已在全量 managed build 之后调用 Headless Release test gate，PR 阶段失败即阻断。
3. ✅ `nuget-publish.yml` 已在 native build、pack、demo build 和 publish 之前调用 Headless Release test gate，发布阶段失败即阻断。
4. ✅ CI 日志会输出 Headless 测试输出目录、当前 RID 和 `runtimes/{rid}/native` 文件列表，并上传 TRX 测试结果。
5. ✅ Headless 测试失败信息已补 `[NativeMissing]`、`[NativeLoadFailure]`、`[LvglInitializationFailure]`、`[SnapshotMismatch]` 和 `[SnapshotContentRegression]` 分类前缀。

已验证命令：

1. ✅ `dotnet test tests\LVGLSharp.Headless.Tests\LVGLSharp.Headless.Tests.csproj --configuration Release --no-restore --nologo`
2. ✅ `dotnet test tests\LVGLSharp.Headless.Tests\LVGLSharp.Headless.Tests.csproj --configuration Release --no-build --nologo`
3. ✅ 本地 Release 输出目录已确认包含 `runtimes/win-x64/native/lvgl.dll`。

完成判定：

1. 🧪 Windows CI 中 Headless Release 全量测试稳定通过，等待推送后的 GitHub Actions 结果确认。
2. ✅ CI 日志能确认 `runtimes/{rid}/native` 已复制到测试输出目录。
3. ✅ 后续测试失败能在 PR 和发布阶段被阻断。

### 3. ⏳ 收拢 `LVGLSharp.Native` 依赖所有权

目标结果：

1. ⏳ `LVGLSharp.Interop` 回到纯绑定层定位。
2. ⏳ `LVGLSharp.Runtime.Windows`、`LVGLSharp.Runtime.Linux`、`LVGLSharp.Runtime.Headless` 和 `LVGLSharp.Runtime.MacOs` 成为原生依赖的直接拥有者。
3. ⏳ demo 与 NuGet 使用方更容易理解“选运行时包，就得到对应原生载荷”的关系。
4. ⏳ pack workflow 不再需要通过 `LVGLSharp.Interop` 间接携带 native 包。

完成判定：

1. 🧪 `LVGLSharp.Interop` 不再引用 `LVGLSharp.Native`。
2. 🧪 使用 `LVGLSharp.Forms + LVGLSharp.Runtime.*` 的应用可以自动获得对应 native 资产。
3. 🧪 Headless 测试和 demo 发布仍能正常找到 `lvgl` 原生库。

### 4. ⏳ 完成 `DRM/KMS` 第一版原生后端

实施步骤：

1. ⏳ 打开并验证 `LV_USE_LINUX_DRM` 所需的 LVGL native 构建选项。
2. ⏳ 在 Linux native CI 中校验 `lv_linux_drm_create`、`lv_linux_drm_set_file`、`lv_linux_drm_find_device_path` 和相关 mode helper 符号。
3. ⏳ 给 `DrmView` 补清晰的失败诊断，包括设备路径、connector、权限和缺失符号。
4. ⏳ 在真实 DRM/KMS 环境跑最小窗口或 framebuffer smoke test。

完成判定：

1. 🧪 `LVGLSHARP_LINUX_HOST=drm` 能在目标 Linux 设备上启动并渲染实际像素。
2. 🧪 native 包中 Linux RID 的 DRM 相关符号存在。
3. 🧪 文档明确 DRM/KMS 的权限、设备路径和已验证发行版/设备。

### 5. ⏳ 扩大 `Headless` 快照验证与 Remote 复用

实施步骤：

1. ⏳ 扩大 `OffscreenView` 的快照回归覆盖范围。
2. ⏳ 让 `OffscreenDemo` 更适合作为稳定的验证入口。
3. ⏳ 持续打磨 `Headless -> Remote` 的帧源适配链路。
4. ⏳ 清理 Remote 中不参与编译、容易误导设计方向的 WinForms 截屏式 frame source 残留。
5. ⏳ 优先让 Remote 复用 Headless/Offscreen frame source，而不是依赖桌面截屏。

完成判定：

1. 🧪 Headless 中文快照、基础控件快照和 Remote frame source 测试在 CI 稳定通过。
2. 🧪 Remote 的基础帧编码和输入事件都有单元测试。
3. 🧪 VNC demo 的最小连接、帧更新和鼠标/键盘输入有手工或自动验证记录。

### 6. ⏳ 建立宿主支持矩阵和自动化验证

验证顺序：

1. ⏳ `WSLg`
2. ⏳ `X11`
3. ⏳ `Wayland`
4. ⏳ `SDL`
5. ⏳ `FrameBuffer`

完成判定：

1. 🧪 每个宿主都有最小 smoke test 命令、期望输出和失败诊断说明。
2. 🧪 对窗口类宿主，不只检查进程启动，还要验证 viewport 像素不是黑屏或空白。
3. 🧪 支持矩阵能清楚区分“稳定支持”“实验性”“仅骨架/预览”。

### 7. ⏳ 推进 `macOS` 与 Remote 从早期实现走向可用实现

macOS 顺序：

1. ⏳ 将 analyzer、buildTransitive runtime 注册和文档状态与 `LVGLSharp.Runtime.MacOs` 对齐。
2. ⏳ 补 macOS 实机启动、窗口像素和输入验证。
3. ⏳ 保持 NativeAOT 发布路径可用。

Remote 顺序：

1. ⏳ 保留 VNC 作为近期可落地路径。
2. ⏳ RDP 继续明确标记为 skeleton，除非补齐真实协议 handshake 和图形更新。
3. ⏳ 避免引入 Windows 截屏式实现作为跨平台 Remote 基础。

完成判定：

1. 🧪 macOS 实机验证闭环。
2. 🧪 VNC 明确可用边界。
3. 🧪 RDP skeleton 状态在代码、文档和示例中保持一致。

### 8. ⏳ 补齐 Forms 兼容层的低风险缺口

实施步骤：

1. ⏳ 实现或明确处理 `ControlCollection.CopyTo(Array, int)`。
2. ⏳ 补 `PictureBoxSizeMode.Zoom` 的按比例缩放实现。
3. ⏳ 为 `TextBox`、Clipboard、右键菜单、输入法和快捷键路径增加 Headless 或 host 级测试覆盖。
4. ⏳ analyzer 覆盖 macOS runtime 引用，并继续维护 LVGLSharp 布局约束。

完成判定：

1. 🧪 已知低风险缺口被测试覆盖并逐项关闭。
2. 🧪 不破坏 `LVGLSharp.Forms` 跨平台定位。
3. 🧪 不重新引入 `LVGLSharp.Forms` 的 Windows 或 `System.Drawing` 依赖。

## 当前实施清单

| 顺序 | 任务 | 状态 | 下一步 |
|---|---|---|---|
| 1 | 本地 Headless 测试与原生加载闭环 | ✅ 已完成 | 作为 CI test gate 的输入基线 |
| 2 | CI Headless 测试阻断关卡 | 🔄 已接入待远端验证 | 推送后观察 GitHub Actions 的 Headless Release test gate |
| 3 | Native 依赖所有权收拢 | ⏳ 待开始 | 让 runtime 包直接拥有 native 依赖 |
| 4 | DRM/KMS native 闭环 | ⏳ 待开始 | 先补 native 构建选项和符号校验 |
| 5 | Headless 与 Remote 验证增强 | ⏳ 待开始 | 扩大快照、帧编码和输入测试 |
| 6 | 宿主支持矩阵 | ⏳ 待开始 | 为 WSLg/X11/Wayland/SDL/FrameBuffer 补 smoke test |
| 7 | macOS 与 Remote 可用化 | ⏳ 待开始 | 补实机验证和文档边界 |
| 8 | Forms 兼容层缺口 | ⏳ 待开始 | 穿插修补低风险缺口 |

## 长期方向

这些方向仍然有效，但不属于当前最优先的近期工作：

1. ⏳ `DirectFB`
2. ⏳ `Mir`

它们适合放在 `DRM/KMS`、`Headless` 验证和现有宿主支持矩阵更稳定之后再推进。
