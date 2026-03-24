# WinFormsVncDemo

这是一个把 `LVGLSharp.Forms` 窗口直接挂到 `VncView` 上的示例程序，用来验证：

- `WinForms` 风格控件可以通过远程 LVGL 宿主对外发布
- `VncView` 可以作为 Demo 的默认窗口运行时
- 桌面 VNC 客户端能够连接到 Demo 并观察界面结果

## 案例截图

下面这张图展示了 `WinFormsVncDemo` 在 Windows 上通过 VNC Viewer 访问时的实际效果：

![WinFormsVncDemo case screenshot](../../../docs/images/winformsvncdemo-vnc-case.png)

## 当前默认行为

- Demo 默认使用 `VncView`
- 默认监听地址：`0.0.0.0:5900`
- 启动时会在控制台输出本机可访问地址

## 说明

- 如果你使用发布产物测试，请保留完整 `publish` 目录，不要只单独拷贝 `exe`
- 当前截图对应的是一个桌面 VNC 客户端连接 `WinFormsVncDemo` 的案例
