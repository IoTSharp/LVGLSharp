# LVGLSharp.Forms

`LVGLSharp.Forms` is the main application-facing package in the LVGLSharp lineup. It keeps a WinForms-style programming model while rendering through LVGL.

## Use it when

- you want forms, controls, layout, and application startup that feel close to WinForms
- you plan to target `net10.0` with LVGL-backed rendering
- you want `ApplicationConfiguration.Initialize()` to wire runtime registration automatically through referenced runtime packages

## Typical companion packages

- `LVGLSharp.Runtime.Windows` for Windows hosts
- `LVGLSharp.Runtime.Linux` for Linux hosts
- `LVGLSharp.Runtime.Headless` for snapshots and automation

`LVGLSharp.Forms` carries the repository analyzers transitively so common runtime-package mistakes can be surfaced early.

Docs and package guidance: <https://lvglsharp.net/en/nuget.html>  
Repository: <https://github.com/IoTSharp/LVGLSharp>
