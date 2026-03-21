# WSL 开发者手册

本文面向本仓库开发者，说明如何在 `WSL2/WSLg` 中运行和调试 Linux 路径下的 demo。

如果你希望专门看 `Visual Studio 2026` 如何参与 `WSL` 调试和本仓库 demo 调试，请同时参考：[`docs/VS2026-WSL-Debug-Guide.md`](./VS2026-WSL-Debug-Guide.md)。

## 先说结论

对本仓库当前最实用的工作流：

1. 在 Windows 上继续使用 `Visual Studio` 编辑代码
2. 使用仓库里的 Linux 发布脚本或 `dotnet publish` 产出 Linux 可执行文件
3. 在 `WSL2` 里运行发布结果
4. 如果需要 Linux 侧断点调试，优先使用 `VS Code + Remote WSL`

## 为什么这样建议

根据当前 Microsoft 官方文档：

- `Windows Subsystem for Linux` 的开发环境文档明确推荐：
  - `Visual Studio Code` 用于 WSL 远程开发与调试
  - `Visual Studio` 的原生 WSL 工作流主要聚焦在 `C++` 跨平台开发
- 对本仓库这种 `.NET + 自定义 Linux runtime` 的路径，最稳的方式仍然是：
  - Windows 侧编辑
  - WSL 侧运行
  - 需要 Linux 断点时切到 `VS Code Remote WSL`

所以，本手册不把“Visual Studio 直接 F5 调试 WSL 中的 .NET demo”作为主路径，而是提供更稳妥、可落地的开发方式。

## 环境准备

### 1. 安装 WSL2

先在 Windows 安装并初始化 WSL：

```powershell
wsl --install
```

安装完成后，建议至少准备一个 Ubuntu 发行版。

### 2. 确认 WSLg 可用

在 WSL 里执行：

```bash
echo $DISPLAY
echo $WAYLAND_DISPLAY
ls /mnt/wslg
```

如果下面任一项成立，通常表示 `WSLg` 可用：

- `DISPLAY` 有值
- `WAYLAND_DISPLAY` 有值
- `/mnt/wslg` 存在

### 3. 在 WSL 中安装 .NET SDK

仓库当前目标包含 `.NET 10`，请确保 WSL 里有对应 SDK：

```bash
dotnet --info
```

如果没有，请在 WSL 内按 .NET 官方安装方式安装对应 SDK。

### 4. 安装 Linux 原生依赖

本仓库的 Linux demo 发布脚本依赖 `cmake` 等工具。在 Ubuntu / Debian 下：

```bash
sudo apt-get update
sudo apt-get install -y cmake ninja-build
```

## 运行 demo 的推荐方式

### 方式 A：在 Windows 侧执行发布脚本，在 WSL 中运行产物

这是当前最推荐的方式。

#### 第一步：在仓库根目录执行 Linux 发布脚本

Windows PowerShell：

```powershell
bash ./build-linux-demos.sh --clean --rid linux-x64
```

执行成功后，产物通常在：

- `dist/linux-x64/WinFormsDemo/`
- `dist/linux-x64/PictureBoxDemo/`
- `dist/linux-x64/SmartWatchDemo/`
- 其他 demo 目录

#### 第二步：在 WSL 中进入仓库目录

假设仓库在 Windows 的 `D:\source\LVGLSharp`，则在 WSL 中路径通常是：

```bash
cd /mnt/d/source/LVGLSharp
```

#### 第三步：运行某个 demo

例如运行 `WinFormsDemo`：

```bash
cd /mnt/d/source/LVGLSharp/dist/linux-x64/WinFormsDemo
./WinFormsDemo
```

例如运行 `PictureBoxDemo`：

```bash
cd /mnt/d/source/LVGLSharp/dist/linux-x64/PictureBoxDemo
./PictureBoxDemo
```

## 按 demo 分类的启动手册

下面按当前仓库里常用 demo 分别列出“发布 + 启动”步骤。默认示例均使用：

- 仓库根目录：`/mnt/d/source/LVGLSharp`
- 目标 RID：`linux-x64`
- 发布方式：优先复用仓库脚本 `build-linux-demos.sh`

### `WinFormsDemo`

适合验证基础控件、窗体生命周期、`LVGLSharp 布局`、通用交互行为。

#### 仅发布这个 demo

Windows PowerShell：

```powershell
bash ./build-linux-demos.sh --clean --rid linux-x64 WinFormsDemo
```

#### 在 WSL 中启动

```bash
cd /mnt/d/source/LVGLSharp/dist/linux-x64/WinFormsDemo
./WinFormsDemo
```

### `PictureBoxDemo`

适合验证图片加载、缩放、旋转、抗锯齿，以及 Linux 图像路径行为。

#### 仅发布这个 demo

Windows PowerShell：

```powershell
bash ./build-linux-demos.sh --clean --rid linux-x64 PictureBoxDemo
```

#### 在 WSL 中启动

```bash
cd /mnt/d/source/LVGLSharp/dist/linux-x64/PictureBoxDemo
./PictureBoxDemo
```

#### 建议额外检查

- `Assets/` 是否已复制到输出目录
- 如果图像未显示，优先检查当前路径和图片资源是否存在

### `SmartWatchDemo`

适合验证多页整屏界面、首屏加载、动态 UI 刷新、复杂布局切换。

#### 仅发布这个 demo

Windows PowerShell：

```powershell
bash ./build-linux-demos.sh --clean --rid linux-x64 SmartWatchDemo
```

#### 在 WSL 中启动

```bash
cd /mnt/d/source/LVGLSharp/dist/linux-x64/SmartWatchDemo
./SmartWatchDemo
```

#### 建议额外检查

- 如果出现黑屏，不要只看标题栏，优先确认实际 viewport 是否有像素内容
- 优先确认是否是控件树创建阶段卡住，而不是先怀疑字体问题

### `MusicDemo`

适合验证较复杂界面、列表/详情切换、图片与动画感较强的页面效果。

> 仓库中的项目目录当前仍是 `src/Demos/MusicWinFromsDemo/`，但 Linux 发布产物目录名是 `MusicDemo`。

#### 仅发布这个 demo

Windows PowerShell：

```powershell
bash ./build-linux-demos.sh --clean --rid linux-x64 MusicDemo
```

#### 在 WSL 中启动

```bash
cd /mnt/d/source/LVGLSharp/dist/linux-x64/MusicDemo
./MusicDemo
```

### `SerialPort`

适合验证串口界面与输入交互，但在 `WSL` 中运行时要特别注意设备可见性。

#### 仅发布这个 demo

Windows PowerShell：

```powershell
bash ./build-linux-demos.sh --clean --rid linux-x64 SerialPort
```

#### 在 WSL 中启动

```bash
cd /mnt/d/source/LVGLSharp/dist/linux-x64/SerialPort
./SerialPort
```

#### 特别说明

- 如果只是验证界面行为，可以先不接真实串口设备
- 如果要验证真实串口，需先确认对应设备已经正确映射到 `WSL`
- 如果串口列表为空，先排查设备映射问题，不要先怀疑 UI 逻辑

## 按用途选 demo

如果你不确定先跑哪个 demo，可以按目的选择：

- 验证基础窗体/控件：`WinFormsDemo`
- 验证图片链路：`PictureBoxDemo`
- 验证复杂多页界面：`SmartWatchDemo`
- 验证复杂视觉效果：`MusicDemo`
- 验证串口相关交互：`SerialPort`

## 快速命令对照

### 发布全部 demo

```powershell
bash ./build-linux-demos.sh --clean --rid linux-x64
```

### 发布单个 demo

```powershell
bash ./build-linux-demos.sh --clean --rid linux-x64 WinFormsDemo
bash ./build-linux-demos.sh --clean --rid linux-x64 PictureBoxDemo
bash ./build-linux-demos.sh --clean --rid linux-x64 SmartWatchDemo
bash ./build-linux-demos.sh --clean --rid linux-x64 MusicDemo
bash ./build-linux-demos.sh --clean --rid linux-x64 SerialPort
```

### 在 WSL 中启动单个 demo

```bash
cd /mnt/d/source/LVGLSharp/dist/linux-x64/WinFormsDemo && ./WinFormsDemo
cd /mnt/d/source/LVGLSharp/dist/linux-x64/PictureBoxDemo && ./PictureBoxDemo
cd /mnt/d/source/LVGLSharp/dist/linux-x64/SmartWatchDemo && ./SmartWatchDemo
cd /mnt/d/source/LVGLSharp/dist/linux-x64/MusicDemo && ./MusicDemo
cd /mnt/d/source/LVGLSharp/dist/linux-x64/SerialPort && ./SerialPort
```

### 方式 B：在 WSL 内直接 publish 再运行

如果你希望所有 Linux 步骤都在 WSL 中完成，可直接在 WSL 中执行：

```bash
cd /mnt/d/source/LVGLSharp
dotnet publish src/Demos/WinFormsDemo/WinFormsDemo.csproj -f net10.0 -r linux-x64 -c Release -o ./artifacts/wsl/WinFormsDemo
```

然后运行：

```bash
cd /mnt/d/source/LVGLSharp/artifacts/wsl/WinFormsDemo
./WinFormsDemo
```

> 注意：如果你不使用仓库脚本，而是自己 `publish`，请确认 Linux 侧所需的原生库与发布输出完整可用。

## 如何判断当前走的是 WSLg 路径

当前仓库的 Linux runtime 已经会自动探测环境：

- `LinuxView` 是统一入口
- `LinuxEnvironmentDetector` 负责探测
- 检测到 `WSLg` 时会路由到 `WslgView`

当前 `WslgView` 有两个可观察点：

1. 窗口标题会带 `WSLg` 标识
2. `WslgView.ToString()` 会返回简要诊断摘要

WSLg 判断依据包括：

- `WSL_DISTRO_NAME`
- `WSL_INTEROP`
- `WAYLAND_DISPLAY`
- `WSLG_RUNTIME_DIR`
- `/mnt/wslg`
- `DISPLAY`

## 调试建议

## 方案 1：Visual Studio 继续负责编辑，WSL 负责运行

适合日常开发验证。

推荐流程：

1. 在 `Visual Studio` 中改代码
2. 用 PowerShell 执行 Linux publish
3. 到 WSL 中运行 demo
4. 看窗口效果、日志、异常输出

优点：
- 与当前仓库结构最匹配
- 不需要额外改工程配置
- 最稳

缺点：
- 不是单击 `F5` 的 Linux 断点调试体验

## 方案 2：VS Code + Remote WSL 调试 Linux demo

如果你需要 Linux 侧断点调试，推荐这条路径。

### 第一步：在 Windows 安装 VS Code 和 Remote WSL 扩展

在 WSL 中进入仓库目录后执行：

```bash
cd /mnt/d/source/LVGLSharp
code .
```

这会用 `Remote WSL` 方式打开当前仓库。

### 第二步：在 WSL 终端中恢复和发布

```bash
dotnet restore
dotnet build
```

然后按需要运行或调试对应 demo 的 `net10.0` 目标。

### 第三步：按 Linux 目标调试

建议优先针对具体 demo 做启动，而不是一次调整个解决方案。

例如：
- `src/Demos/WinFormsDemo/WinFormsDemo.csproj`
- 目标框架：`net10.0`

## 方案 3：Visual Studio 直接调 Windows 目标，WSL 只做 Linux 行为验证

如果当前主要目的是开发控件逻辑，而不是定位 Linux 宿主问题，可以：

1. 先在 Windows 目标下用 `Visual Studio` 正常调试
2. 功能稳定后再切到 WSL 验证 Linux 行为

这种方式适合：
- 控件逻辑调试
- 事件链调试
- 基础布局验证

不适合：
- X11 / WSLg 宿主问题
- Linux 字体、输入、显示协议问题

## 调试前建议检查项

每次在 WSL 中启动 Linux demo 前，建议先检查：

```bash
echo $DISPLAY
echo $WAYLAND_DISPLAY
echo $WSL_DISTRO_NAME
echo $WSL_INTEROP
ls /mnt/wslg
```

再检查发布结果：

```bash
ls
```

确认：
- 可执行文件存在
- 原生库存在
- 当前目录正确

## 常见问题

### 1. 窗口没出来

优先检查：

- `DISPLAY` 是否存在
- `/mnt/wslg` 是否存在
- 是否真的运行了 Linux 发布产物
- 是否在 `WSL` 中运行，而不是在 Windows shell 中直接执行 Linux ELF

### 2. 标题栏出来了，但内容黑屏

优先排查：

- 是否是控件树创建阶段卡住
- 是否首页挂载内容过多
- 是否隐藏页或大布局在启动阶段一次性挂载

### 3. Visual Studio 里不会配 WSL 启动调试

对本仓库当前阶段，建议不要先追求这条路径。

更建议：
- `Visual Studio` 负责编辑和 Windows 调试
- `WSL` 负责运行 Linux 产物
- 需要 Linux 断点时使用 `VS Code Remote WSL`

这是当前投入最小、稳定性最高的做法。

## 建议的团队工作流

推荐团队内部统一为下面流程：

1. Windows 上使用 `Visual Studio` 开发
2. 功能修改后先保证 Windows / 基础构建通过
3. 用 Linux publish 脚本产出 demo
4. 在 `WSL2/WSLg` 中运行验证
5. 只有遇到 Linux 宿主专属问题时，再切 `VS Code Remote WSL` 做断点调试

这样可以兼顾：
- Windows 侧开发效率
- Linux 侧真实运行环境验证
- 最少的工程与 IDE 配置复杂度
