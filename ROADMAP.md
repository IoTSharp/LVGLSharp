# LVGLSharp.Forms Roadmap

Last updated: 2026-03-23

## Current assessment

`LVGLSharp.Forms` has already moved past the "can it run?" stage and into the first real engineering baseline:

- the package split is established
- Windows and Linux runtime boundaries are in place
- native asset packaging and reusable CI workflows exist
- documentation now has enough structure to describe the project as a roadmap-driven system

The next phase is no longer "start from scratch". It is about simplifying dependency ownership, clarifying host maturity, and adding verification discipline around the hosts that are already implemented.

## Confirmed completed

### Foundation and packaging

- Upgraded onto the LVGL 9.5 codebase and aligned the current release baseline around `9.5.0.5`
- Established the package layout:
  - `LVGLSharp.Forms`
  - `LVGLSharp.Core`
  - `LVGLSharp.Interop`
  - `LVGLSharp.Native`
  - `LVGLSharp.Runtime.Windows`
  - `LVGLSharp.Runtime.Linux`
- Added multi-RID native library packaging under `runtimes/{rid}/native`
- Split GitHub Actions into reusable prepare/build/pack/publish stages

### Runtime architecture

- `buildTransitive` runtime registration is now generated automatically from referenced runtime packages
- `ApplicationConfiguration.Initialize()` is the normal entry point for the `LVGLSharp.Forms` path
- Windows runtime is available through `Win32View`
- Linux runtime is unified through `LinuxView`
- Native library probing is centralized through `LvglNativeLibraryResolver`

### Linux host coverage

The repository already contains these Linux-side hosts:

- `WslgView`
- `X11View`
- `WaylandView`
- `SdlView`
- `FrameBufferView`

This means the roadmap should no longer describe `Wayland` and `SDL` as "not started". They are implemented, but not yet at the same maturity level as the more established paths.

### Documentation and release expression

- `CHANGELOG.md` is being used as the release-facing change record
- the docs site structure now has home pages, navigation, CI notes, WSL guidance, and blog-style articles
- README and docs can now point to a real roadmap instead of an implied future placeholder

## Current status by area

| Area | Status | Notes |
|---|---|---|
| WinForms compatibility layer | Available | Core controls, form lifecycle, and common layout patterns exist |
| LVGL interop layer | Available | Full generated P/Invoke surface is present |
| Windows runtime | Available | `Win32View` is part of the current supported path |
| Linux X11 / WSLg | Available | Current desktop-oriented Linux path |
| Linux FrameBuffer | Available | Current device-oriented Linux path |
| Linux Wayland | Experimental | Implemented, but still needs more validation and release discipline |
| Linux SDL | Experimental | Implemented, but still needs more validation and release discipline |
| Native packaging / CI | Available | Multi-stage workflows and native artifacts are already in place |
| Docs site / docs navigation | In progress | Structure exists; link cleanup and publishing flow still need steady polish |

## Immediate next step

The recommended next engineering step is:

### Move `LVGLSharp.Native` dependency ownership into the runtime packages

Target outcome:

- `LVGLSharp.Interop` becomes a pure binding layer
- `LVGLSharp.Runtime.Windows` and `LVGLSharp.Runtime.Linux` become the direct owners of the native dependency
- demo projects no longer need to reference `LVGLSharp.Native` directly
- NuGet consumers can reason about the package graph more naturally: choose runtime package -> get native payload transitively

Why this should go next:

- it matches the actual architectural responsibility boundaries
- it reduces package graph confusion for consumers
- it prepares the repo for clearer support-matrix documentation and future runtime expansion

## After the immediate next step

### 1. Clarify the host support matrix

Document and enforce a distinction between:

- current default paths
- experimental but implemented paths
- future host targets that are not yet started

### 2. Add stronger host validation

Focus especially on:

- `WSLg`
- `X11`
- `Wayland`
- `SDL`
- `FrameBuffer`

The goal is not only "can launch", but also "can be regression-checked".

### 3. Keep release docs and docs site aligned

Continue using the same language across:

- `README`
- `CHANGELOG`
- `ROADMAP`
- docs home/navigation pages

## Later roadmap

These remain valid later-stage directions, but they are not the best immediate next step:

- `DRM`
- `KMS`
- `Offscreen`
- `DirectFB`
- `Mir`
- `LVGLSharp.Runtime.MacOs`
- remote runtimes such as `VNC` / `RDP`

## Practical takeaway

If we want to start the next piece of work now, the best place to begin is:

1. move `LVGLSharp.Native` from the current `Interop` / demo-facing dependency edges into the runtime packages
2. update the package graph docs to match
3. then add host-specific validation and release-matrix clarity on top
