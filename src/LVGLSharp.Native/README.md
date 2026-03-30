# LVGLSharp.Native

`LVGLSharp.Native` ships the platform-native LVGL binaries and build assets used by LVGLSharp packages.

## What is inside

- RID-specific native libraries under `runtimes/{rid}/native`
- build and `buildTransitive` targets used during publish
- assets for Windows, Linux, and macOS runtime identifiers supported by the repository

Most application code does not need to reference this package directly because it is normally pulled in through `LVGLSharp.Interop` and the runtime packages.

Docs and package guidance: <https://lvglsharp.net/en/nuget.html>  
Repository: <https://github.com/IoTSharp/LVGLSharp>
