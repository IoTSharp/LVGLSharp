# Linux View 路线图

本文整理 `LVGLSharp.Runtime.Linux` 当前已经支持的宿主、后续建议支持的 Linux 图形宿主，以及推荐的实施顺序。

## 当前状态

当前 `LinuxView` 统一作为 Linux 运行时入口，并在内部按环境路由到不同 `IView` 实现：

- `WslgView`
- `X11View`
- `FrameBufferView`

当前环境探测由 `LinuxEnvironmentDetector` 负责，`LinuxView` 只负责路由。

## 已支持的 Linux View

### `WslgView`
- 目标场景：`WSL2 + WSLg`
- 当前实现：继承 `X11View`
- 当前价值：
  - 为 `WSLg` 提供单独扩展点
  - 提供更明确的标题与诊断信息
- 后续可增强：
  - 更细粒度的 `DISPLAY` / `WAYLAND_DISPLAY` 诊断
  - `WSLg` 特定窗口行为
  - 启动失败时更明确的引导信息

### `X11View`
- 目标场景：传统 Linux 桌面环境、XWayland 路径
- 当前价值：
  - 桌面 Linux 覆盖面最广
  - 适合当前 demo 与开发调试
- 备注：
  - 很多 Wayland 桌面仍可通过 XWayland 运行现有 X11 程序

### `FrameBufferView`
- 目标场景：`fbdev` 设备、极简环境、旧设备
- 当前价值：
  - 覆盖无桌面环境的简单设备场景
- 备注：
  - 长期更建议补充 `DRM/KMS` 路线，而不是继续把 `fbdev` 作为主方向

## 常见 Linux 图形宿主清单

下面是本仓库后续值得考虑的 Linux 图形宿主 / 显示协议：

### 第一组：建议优先支持

#### `WaylandView`
- 场景：现代 Linux 桌面主流路径
- 价值：
  - GNOME / KDE / 现代发行版越来越常见
  - 长期比单纯依赖 `X11` 更稳妥
- 优先级：高

#### `DrmView` 或 `KmsView`
- 场景：嵌入式设备、全屏 kiosk、无桌面环境
- 价值：
  - 比 `FrameBufferView` 更现代
  - 更贴近本仓库面向设备端部署的目标
- 优先级：高

#### `OffscreenView` 或 `HeadlessView`
- 场景：自动化测试、截图、回归验证、离屏渲染
- 价值：
  - 不依赖真实桌面环境
  - 很适合 CI 与快照测试
- 优先级：高

### 第二组：建议中期补充

#### `SdlView`
- 场景：开发调试、跨平台快速验证
- 价值：
  - 开发体验好
  - 适合做调试宿主或示例宿主
- 优先级：中

#### 强化 `WslgView`
- 场景：Windows 开发者在 `WSL2` 中调试 Linux 路径
- 价值：
  - 直接提升当前开发体验
  - 对仓库维护者很实用
- 优先级：中

### 第三组：低优先级或暂不建议

#### `DirectFB`
- 过旧，生态价值有限。

#### `Mir`
- 场景较窄，不建议优先投入。

#### 专用远程桌面宿主
- 例如专门为 `VNC/RDP` 做独立 `View`
- 当前更适合作为宿主环境的一部分，而不是单独 `IView`

## 推荐实施顺序

建议按下面顺序推进：

1. 强化 `WslgView`
2. 实现 `WaylandView`
3. 实现 `DrmView` / `KmsView`
4. 实现 `OffscreenView`
5. 视需要补充 `SdlView`
6. 继续保留 `FrameBufferView` 作为兼容路径

## 为什么这样排序

### 1. `WslgView`
- 当前仓库已经有基础实现
- 改造成本最低
- 能直接改善日常开发调试体验

### 2. `WaylandView`
- 对桌面 Linux 覆盖面最大
- 是现代桌面 Linux 的主路线

### 3. `DrmView` / `KmsView`
- 对设备端最有价值
- 与项目跨平台嵌入式方向高度一致

### 4. `OffscreenView`
- 对测试、截图、CI 很有价值
- 可帮助建立稳定的图形回归验证能力

### 5. `SdlView`
- 更偏开发调试便利性
- 生产部署价值通常不如 `Wayland/DRM`

## 结构建议

建议持续保持下面的结构：

- `LinuxView`：统一入口与路由
- `LinuxEnvironmentDetector`：环境探测与诊断
- 具体实现：
  - `WslgView`
  - `WaylandView`
  - `X11View`
  - `DrmView`
  - `FrameBufferView`
  - `OffscreenView`
  - `SdlView`（按需）

这样可以保持：
- 外部调用方式稳定
- 自动注册逻辑不变
- 平台探测与宿主实现解耦
- 后续新增 view 时改动面最小
