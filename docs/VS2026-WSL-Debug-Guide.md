# Visual Studio 2026 调试 WSL 程序与本项目 Demo

本文补充说明如何使用 `Visual Studio 2026` 调试 `WSL` 程序，以及如何在本仓库中调试 Linux 路径下的 demo。

## 先说结论

分两种情况：

1. **通用 WSL 程序**
   - `Visual Studio` 当前官方一等公民的 `WSL` 调试体验主要集中在 `C++ Linux/WSL` 项目
   - 如果你是 `CMake` 或 `Linux MSBuild C++` 工程，可以直接用 `Visual Studio` 的 `WSL` 目标系统调试

2. **本仓库的 demo**
   - 本仓库是 `.NET + 自定义 Linux runtime + LVGL` 路径
   - 当前最稳的方式不是直接追求 `Visual Studio 2026` 原生 `F5` 调试 Linux demo
   - 推荐工作流是：
     - `Visual Studio 2026` 负责编辑代码、Windows 目标调试、执行发布命令
     - `WSL2/WSLg` 负责运行 Linux 发布产物
     - 需要 Linux 侧断点时，优先使用 `VS Code + Remote WSL`

## 依据

Microsoft 当前公开的 WSL/Visual Studio 一方文档重点在：

- `WSL` 开发环境推荐 `VS Code` 或 `Visual Studio`
- `Visual Studio` 的原生 `WSL` 目标系统工作流主要聚焦 `C++` 跨平台开发
- `Visual Studio` 对 `WSL` 的原生 `F5` 流程，官方完整文档主要覆盖：
  - `CMake` Linux 项目
  - `Linux MSBuild C++` 项目

因此，对于本仓库这种 `.NET Linux GUI demo`，文档建议采用更稳妥的混合工作流，而不是把仓库强行适配成 Visual Studio 的 Linux/C++ 项目模型。

## 一、如何用 VS2026 调试通用 WSL 程序

> 这一节面向“普通 WSL 程序”，不是本仓库专属流程。

### 适用场景

适合：

- `CMake` 工程
- `Linux MSBuild C++` 工程
- 需要在 `Visual Studio` 中直接以 `WSL` 为目标系统 `F5`

不适合：

- 本仓库当前这类 `.NET + 自定义 Linux runtime` demo 启动模型

### 1. 安装前提

在 Windows 上：

- 安装 `Visual Studio 2026`
- 安装 `Linux and embedded development with C++` 工作负载

在 WSL 中安装必要工具，例如 Ubuntu：

```bash
sudo apt update
sudo apt install cmake g++ gdb make ninja-build rsync zip
```

### 2. 准备 WSL 发行版

PowerShell：

```powershell
wsl --install
```

并确认：

```powershell
wsl -l -v
```

### 3. 在 Visual Studio 2026 中选择 WSL 目标

如果是 `CMake` Linux 项目：

1. 打开工程文件夹
2. 在顶部目标系统下拉框中选择你的 `WSL2` 发行版
3. 选择 Linux 配置
4. 配置完成后按 `F5`

如果是 `Linux MSBuild C++` 项目：

1. 右键项目 -> `Properties`
2. 找到 Linux / WSL 相关配置
3. 选择 `WSL2` 工具链或对应 Linux 工具集
4. 按 `F5`

### 4. 什么时候不该套这个方法

如果你的程序：

- 不是 `C++ Linux` 项目模型
- 不是 `CMake` / `Linux MSBuild`
- 运行依赖自定义宿主、图形协议、手工发布产物

那么不要强行套 Visual Studio 的原生 `WSL` 调试模型。

本仓库就是这种情况。

## 二、本项目 demo 在 VS2026 中应该怎么调

### 结论

本仓库 demo 的推荐方法不是“在 VS2026 里直接把 `net10.0` Linux demo 配成原生 WSL 启动项目并 F5”。

推荐拆成三层：

1. `Visual Studio 2026` 调 Windows 目标
2. `Visual Studio 2026` 触发 Linux 发布
3. `WSL2/WSLg` 运行 Linux demo

需要 Linux 断点时，再切 `VS Code Remote WSL`。

## 三、本项目最推荐的 VS2026 工作流

### 方案 A：VS2026 编辑 + Windows 目标调试 + WSL 运行验证

这是最推荐的日常工作流。

#### 步骤 1：在 VS2026 中调 Windows 目标

例如：

- `WinFormsDemo`
- `PictureBoxDemo`
- `SmartWatchDemo`
- `MusicDemo`

在项目属性或启动配置中，优先选择 Windows 目标：

- `net10.0-windows`

这样可以先验证：

- 控件逻辑
- 事件链
- 基础布局
- 数据流

#### 步骤 2：在 VS2026 中打开终端执行 Linux 发布

可以直接使用 `Visual Studio 2026` 的终端，运行：

```powershell
bash ./build-linux-demos.sh --clean --rid linux-x64 WinFormsDemo
```

或者发布全部：

```powershell
bash ./build-linux-demos.sh --clean --rid linux-x64
```

#### 步骤 3：在 WSL 中运行 Linux demo

例如：

```bash
cd /mnt/d/source/LVGLSharp/dist/linux-x64/WinFormsDemo
./WinFormsDemo
```

### 方案 B：VS2026 只负责编辑与发布，WSL 负责 Linux 问题定位

适合：

- `X11/WSLg` 宿主问题
- Linux 字体问题
- Linux 输入问题
- 黑屏、窗口有标题但无内容等渲染问题

流程：

1. 用 `Visual Studio 2026` 修改代码
2. 在 VS 终端中执行 Linux 发布
3. 到 `WSL` 运行产物
4. 观察现象、日志、标题、窗口内容

## 四、按 demo 分类：在 VS2026 中如何触发 Linux 发布

下面这些命令都可以直接在 `Visual Studio 2026` 内置终端中运行。

### `WinFormsDemo`

```powershell
bash ./build-linux-demos.sh --clean --rid linux-x64 WinFormsDemo
```

WSL 启动：

```bash
cd /mnt/d/source/LVGLSharp/dist/linux-x64/WinFormsDemo
./WinFormsDemo
```

### `PictureBoxDemo`

```powershell
bash ./build-linux-demos.sh --clean --rid linux-x64 PictureBoxDemo
```

WSL 启动：

```bash
cd /mnt/d/source/LVGLSharp/dist/linux-x64/PictureBoxDemo
./PictureBoxDemo
```

### `SmartWatchDemo`

```powershell
bash ./build-linux-demos.sh --clean --rid linux-x64 SmartWatchDemo
```

WSL 启动：

```bash
cd /mnt/d/source/LVGLSharp/dist/linux-x64/SmartWatchDemo
./SmartWatchDemo
```

### `MusicDemo`

```powershell
bash ./build-linux-demos.sh --clean --rid linux-x64 MusicDemo
```

WSL 启动：

```bash
cd /mnt/d/source/LVGLSharp/dist/linux-x64/MusicDemo
./MusicDemo
```

### `SerialPort`

```powershell
bash ./build-linux-demos.sh --clean --rid linux-x64 SerialPort
```

WSL 启动：

```bash
cd /mnt/d/source/LVGLSharp/dist/linux-x64/SerialPort
./SerialPort
```

## 五、在 VS2026 中建议怎么组织调试目标

推荐把“Windows 逻辑调试”和“Linux 行为验证”分开看。

### Windows 目标：适合在 VS2026 里直接调

适合：

- `net10.0-windows`
- 业务逻辑
- 控件事件
- 布局代码
- 数据绑定/刷新逻辑

### Linux 目标：适合在 WSL 中运行验证

适合：

- `net10.0`
- `X11View`
- `WslgView`
- `LinuxView`
- `LinuxEnvironmentDetector`
- Linux 字体、显示、输入、宿主初始化问题

## 六、如果一定要在 VS2026 里调 WSL 程序，应怎么理解

要区分两种“调 WSL 程序”：

### 情况 1：官方支持路径

- `CMake C++`
- `Linux MSBuild C++`
- `Visual Studio` 原生 `WSL` 目标系统

这条路是官方主路径。

### 情况 2：本仓库这种 .NET Linux GUI demo

- 当前不是官方文档重点覆盖路径
- 不建议为了追求“VS2026 里直接 F5 到 WSL”去重构当前项目结构
- 成本高，收益低，还容易破坏现有 demo 与 runtime 设计

因此本仓库当前阶段的建议仍然是：

- `VS2026` 做编辑、Windows 调试、发布
- `WSL` 做 Linux 运行验证
- `VS Code Remote WSL` 做 Linux 断点调试

## 七、建议的实际使用顺序

推荐按下面顺序工作：

1. 在 `Visual Studio 2026` 中修改代码
2. 先用 Windows 目标确认逻辑没问题
3. 在 VS 终端中执行 Linux 发布脚本
4. 在 `WSL2/WSLg` 中运行 demo
5. 如果 Linux 专属问题仍难定位，再切 `VS Code Remote WSL`

## 八、常见误区

### 1. 误以为 Visual Studio 的 WSL 调试模型能直接套到本仓库

不建议。

因为本仓库当前不是标准 Linux C++ 项目模型，而是：

- `.NET`
- 自定义 runtime 注册
- Linux GUI 宿主
- 运行结果强依赖 `X11/WSLg` 环境

### 2. 误以为 Windows 能调通，Linux 一定没问题

不成立。

Linux 路径还涉及：

- `X11View`
- `WslgView`
- 字体
- 输入
- 图像
- 宿主初始化

### 3. 误以为只要标题栏出来就说明 Linux UI 正常

不成立。

排查时要看：

- 实际客户区是否有像素内容
- 是否进入 `Load`
- 是否卡在控件树创建阶段

## 九、文档配套

建议结合下面文档一起看：

- [`docs/WSL-Developer-Guide.md`](./WSL-Developer-Guide.md)
- [`docs/Linux-View-Roadmap.md`](./Linux-View-Roadmap.md)
