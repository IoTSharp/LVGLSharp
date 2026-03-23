# LVGLSharp.Forms

> Bringing a WinForms-style development experience to LVGL-powered cross-platform UI.

English | [??](./index.md) | [Navigation](./navigation.en.md)

---

## Badge Area
![Status](https://img.shields.io/badge/status-experimental-orange)
![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux-blue)
![AOT](https://img.shields.io/badge/NativeAOT-supported-success)
![Rendering](https://img.shields.io/badge/rendering-LVGL-5c3ee8)
![License](https://img.shields.io/badge/license-MIT-green)

## Homepage Summary

`LVGLSharp.Forms` is a **WinForms API compatibility layer** designed for cross-platform UI, device-oriented environments, and NativeAOT-friendly deployment. It uses **LVGL** as its rendering engine while trying to preserve the productivity and mental model of WinForms development.

### What you can quickly learn here

- what the project does
- why it matters
- what it already supports
- how the architecture is organized
- where to go next in the documentation

---

## Language Switch

> Current page: **English** | Switch to: [????](./index.md) ?[????](./navigation.md) ?[????](./blog/index.md)

---

## Quick Entry Points

## Start Here

If this is your first visit, start with this sequence:

1. read this page to understand the project抯 purpose
2. read [`ci-workflows.md`](./ci-workflows.md) to understand the engineering pipeline
3. read [`ROADMAP.md`](../ROADMAP.md) to understand where the project is heading
4. move into the blog section for design rationale and implementation discussions

---

## Feature Cards

## Three-Column Entry Area

### Documentation

- [Navigation](./navigation.en.md)
- [CI Workflow Guide](./ci-workflows.en.md)
- [WSL Developer Guide](./WSL-Developer-Guide.en.md)

### Blog

- [Blog Index](./blog/index.en.md)
- [Why WinForms over LVGL](./blog/why-winforms-over-lvgl.en.md)
- [Architecture Breakdown](./blog/architecture.en.md)

### Repository

- [README](../README_en.md)
- [Roadmap](../ROADMAP.md)
- [Changelog](../CHANGELOG.md)

---

## Project Highlights

### WinForms-style Development

Keeps the familiar forms, controls, events, and layout mental model.

### LVGL Rendering Core

Uses LVGL as the rendering backend to support portability, lightweight deployment, and device adaptation.

### NativeAOT Direction

Moves steadily toward self-contained, minimal-runtime, device-friendly deployment.

### Multi-Host Runtime Model

Separates Windows and Linux runtimes while leaving room for future Wayland, SDL, DRM/KMS, and other host paths.

---

## Version / Platform / Package Summary

### Current Version

- Current documented version: `9.5.0.5`
- Release tag: `v9.5.0.5`

### Current Platform Coverage

- Windows: `win-x86` / `win-x64` / `win-arm64`
- Linux: `linux-x64` / `linux-arm` / `linux-arm64`

### Current Package Layout

- `LVGLSharp.Forms`
- `LVGLSharp.Core`
- `LVGLSharp.Interop`
- `LVGLSharp.Native`
- `LVGLSharp.Runtime.Windows`
- `LVGLSharp.Runtime.Linux`

### Getting Started Reading

- [CI Workflow Guide](./ci-workflows.en.md)
- [WSL Developer Guide](./WSL-Developer-Guide.en.md)
- [Roadmap](../ROADMAP.md)
- [Changelog](../CHANGELOG.md)

### Blog Articles

- [Why WinForms over LVGL](./blog/why-winforms-over-lvgl.en.md)
- [NativeAOT and GUI](./blog/nativeaot-gui.en.md)
- [Linux Host Strategy](./blog/linux-hosts.en.md)
- [Architecture Breakdown](./blog/architecture.en.md)

---

## Latest Articles

- [Why WinForms over LVGL](./blog/why-winforms-over-lvgl.en.md)
- [Architecture Breakdown](./blog/architecture.en.md)
- [NativeAOT and GUI](./blog/nativeaot-gui.en.md)
- [Linux Host Strategy](./blog/linux-hosts.en.md)

---

## Recommended Reading Cards

### Best for New Readers

If you want to understand why this project exists, start with:

- [Why WinForms over LVGL](./blog/why-winforms-over-lvgl.en.md)

### Best for Architecture and Engineering

If you care about module boundaries, package layout, and runtime layering, start with:

- [Architecture Breakdown](./blog/architecture.en.md)
- [CI Workflow Guide](./ci-workflows.en.md)

### Best for Linux and Device Work

If your main interest is WSL, Linux graphics hosts, or device-side deployment, start with:

- [Linux Host Strategy](./blog/linux-hosts.en.md)
- [WSL Developer Guide](./WSL-Developer-Guide.en.md)

### Best for AOT and Publishing

If you care most about trimming, self-contained deployment, and NativeAOT, start with:

- [NativeAOT and GUI](./blog/nativeaot-gui.en.md)

---

## Blog Highlights

### Highlight 1: Why WinForms over LVGL

Start here if your main question is why this project should exist at all.

### Highlight 2: Architecture Breakdown

Start here if you care most about module boundaries and engineering structure.

### Highlight 3: NativeAOT and GUI

Start here if device deployment and runtime constraints are your main focus.

---

## Project Overview

`LVGLSharp.Forms` is a **WinForms API compatibility layer built on top of LVGL**. Its goal is not merely to wrap LVGL with another .NET API, but to preserve as much of the familiar WinForms mental model as possible梖orms, controls, layout structure, event handling, and application lifecycle梬hile running the same UI code across Windows, Linux, and broader device-oriented environments.

If summarized in one sentence, the project aims to do this:

> **Let developers keep writing UI with a WinForms-like programming model while delegating rendering to LVGL, gaining cross-platform reach, lightweight deployment, and NativeAOT-friendly runtime behavior.**

This project is especially interesting for developers who:

- already know WinForms and want to move toward Linux or embedded devices
- want to use LVGL from the .NET ecosystem instead of working only in C/C++
- want NativeAOT publishing with reduced runtime footprint
- want to prototype UI on desktop systems and deploy it later to ARM / ARM64 / x64 targets

---

## Why It Matters

WinForms is still one of the most productive UI models in the .NET world:

- straightforward control model
- mature event model
- designer-friendly workflow
- stable application structure for business software

However, WinForms itself is primarily designed for Windows desktop applications and does not naturally target:

- Linux graphical environments
- device-side UI scenarios
- framebuffer / DRM / KMS display stacks
- lightweight ARM deployments
- aggressively trimmed NativeAOT applications

LVGL, on the other hand, is excellent for embedded UI, lightweight rendering, and broad platform adaptation梑ut its native programming model is more low-level and differs significantly from the WinForms way of building applications.

`LVGLSharp.Forms` sits in the middle:

- a WinForms-like API on the top
- LVGL as the rendering engine underneath
- runtime hosts, platform registration, generated interop, and control/event bridges in between

---

## Project Positioning

This project does **not** try to reproduce the entire WinForms ecosystem. Its practical positioning is:

1. provide a high-value WinForms compatibility layer
2. make common forms-and-controls applications portable
3. isolate platform differences inside runtime projects
4. remain friendly to NativeAOT and trimming
5. evolve toward Linux, devices, and embedded environments

That is why the architecture emphasizes:

- stable API at the `Forms` layer
- platform differences hidden from business code as much as possible
- runtime initialization driven by explicit registration and reusable host logic
- lifecycle, controls, and layout patterns that stay close to WinForms expectations

---

## Current Capability Overview

### Capability Snapshot

| Area | Current State |
|---|---|
| WinForms-style API | Core compatibility layer established |
| Full LVGL interop | Available |
| Windows / Linux runtimes | Available |
| NativeAOT direction | Supported and actively shaped |
| Multi-platform native packaging | Available |
| GitHub Actions CI/CD | Decomposed into reusable stages |
| GitHub Pages docs site | Basic structure established |

## What the Project Already Provides

### 1. A WinForms-like UI Layer

The project offers an API shape that is intentionally close to `System.Windows.Forms`, including:

- `Form`
- `Control`
- common container controls
- common input controls
- common display controls
- familiar event patterns
- basic form lifecycle behavior

Currently available controls include:

- `Button`
- `Label`
- `TextBox`
- `CheckBox`
- `RadioButton`
- `ComboBox`
- `ListBox`
- `ProgressBar`
- `TrackBar`
- `NumericUpDown`
- `PictureBox`
- `Panel`
- `GroupBox`
- `FlowLayoutPanel`
- `TableLayoutPanel`
- `RichTextBox`

### 2. Full LVGL Interop Access

The project is not limited to a thin set of high-level controls. Through auto-generated `P/Invoke` bindings, it exposes the **full LVGL C API** to .NET.

That means developers can choose between:

- using only the higher-level `LVGLSharp.Forms` abstraction

or

- dropping down to native LVGL APIs when a lower-level customization is needed

### 3. NativeAOT-Friendly Direction

The project strongly favors AOT-safe and trimming-safe implementation patterns. Instead of relying on fragile runtime reflection paths, it moves toward explicit registration and static-friendly initialization.

This makes it especially suitable for:

- device deployment
- single-file publishing
- self-contained binaries
- minimal runtime environments

### 4. Cross-Platform Runtime Structure

The repository already provides two main runtime paths:

- `LVGLSharp.Runtime.Windows`
- `LVGLSharp.Runtime.Linux`

Combined with `buildTransitive` and automatic runtime registration, application code does not need to manually wire complex platform initialization in every entry point.

---

## Architecture Overview

From a high-level perspective, the repository is organized into six major layers.

### 1. `LVGLSharp.WinForms`

This is the central compatibility layer exposed to application developers.

Responsibilities include:

- control implementations
- form lifecycle behavior
- layout abstraction
- event semantics
- bridging between control trees and LVGL object trees

### 2. `LVGLSharp.Core`

The shared core abstraction layer.

Responsibilities include:

- common runtime abstractions
- view lifetime infrastructure
- shared image/font helper capabilities
- platform-neutral host support logic

### 3. `LVGLSharp.Interop`

The low-level LVGL .NET interop layer.

Responsibilities include:

- auto-generated P/Invoke declarations
- mapping LVGL structs, enums, and functions
- providing the raw LVGL entry points used by upper layers

### 4. `LVGLSharp.Native`

The platform-native library distribution layer.

Responsibilities include:

- packing LVGL native binaries for multiple platforms
- organizing them under `runtimes/{rid}/native`
- serving as the native dependency base for higher-level packages

### 5. Platform Runtime Packages

Currently:

- `LVGLSharp.Runtime.Windows`
- `LVGLSharp.Runtime.Linux`

Responsibilities include:

- selecting the correct host for the platform
- registering `IView`
- managing the render loop, display buffers, and input bridges
- isolating platform-specific differences from application code

### 6. Demo Applications

The demos are not only for visual showcase. They also serve as:

- regression samples
- publishing validation samples
- control behavior examples
- runtime host verification targets

---

## Platforms and Scenarios

### Current platform coverage

- Windows
  - `win-x86`
  - `win-x64`
  - `win-arm64`
- Linux
  - `linux-x64`
  - `linux-arm`
  - `linux-arm64`

### Current target scenarios

- Windows desktop development and design-time verification
- Linux desktop execution
- ARM / ARM64 device deployment
- NativeAOT publishing
- WSL / WSLg / X11 debugging workflows

### Planned directions

According to the roadmap, future work will continue toward:

- `Wayland`
- `SDL`
- `DRM`
- `KMS`
- `Offscreen`
- `DirectFB`
- `Mir`
- `LVGLSharp.Runtime.MacOs`
- remote runtimes such as `VNC` / `RDP`

---

## Recommended Development Model

The repository encourages a **multi-target** project structure:

- `net10.0-windows` for the standard WinForms path
- `net10.0` for the `LVGLSharp.Forms` path

This model lets you:

- preserve a designer-friendly Windows target
- use the LVGL-backed target for cross-platform deployment
- keep a shared UI structure across both targets

That is currently one of the most practical usage patterns for the project.

---

## The 揕VGLSharp Layout?Pattern

The repository uses a specific term for its recommended layout model:

> **LVGLSharp Layout**

The core rules are:

- use an outer `TableLayoutPanel` only for vertical partitioning
- put one `FlowLayoutPanel` in each row
- place real business controls inside the row-level `FlowLayoutPanel`
- use fixed absolute row heights on the main `TableLayoutPanel`
- do not place business controls directly onto the main `TableLayoutPanel`

Why this matters:

- it leads to more predictable line-based layout
- it behaves more consistently across platforms and hosts
- it reduces the impact of font metrics and backend differences on layout stability

For blog readers, this is an important point:

> `LVGLSharp.Forms` is not only translating WinForms APIs onto LVGL. It is also gradually defining layout practices that are more suitable for cross-platform and device-oriented rendering hosts.

---

## Runtime and Host Model

### Windows runtime path

The Windows runtime is responsible for:

- choosing the Windows-side host implementation
- registering image and platform helpers
- cooperating with the desktop-side design and development workflow

### Linux runtime path

On Linux, `LinuxView` currently serves as the unified entry point and routes internally to specific hosts such as:

- `WslgView`
- `X11View`
- `FrameBufferView`

This allows the project to:

- reuse the same upper-layer API across multiple Linux environments
- keep host-specific logic inside runtime packages
- prepare for future expansion toward `Wayland`, `SDL`, and other hosts

---

## AOT and Trimming Philosophy

This is one of the project抯 most technically important aspects.

Many UI frameworks struggle when moving toward NativeAOT or aggressive trimming because they depend on:

- reflection-based activation
- runtime registration patterns that are hard to analyze statically
- warning suppression instead of architectural fixes

This project explicitly tries to avoid that. It emphasizes:

- no 揷omment-based?suppression of AOT or trimming warnings as a design strategy
- replacing risky activation paths with explicit, analyzable implementations
- favoring static-friendly registration and startup patterns

That makes it a strong candidate for:

- serious device-side .NET GUI deployment
- runtime-size-sensitive applications
- startup-optimized cross-platform UI systems

---

## NuGet Package Structure

The current package split is intentionally layered:

| Package | Responsibility |
|---|---|
| `LVGLSharp.Forms` | WinForms API compatibility layer |
| `LVGLSharp.Core` | Shared abstractions and common runtime support |
| `LVGLSharp.Runtime.Windows` | Windows runtime |
| `LVGLSharp.Runtime.Linux` | Linux runtime |
| `LVGLSharp.Interop` | Full LVGL P/Invoke bindings |
| `LVGLSharp.Native` | Native library distribution |

Benefits of this split:

- clear package responsibilities
- manageable cross-platform dependency flow
- independent packaging and release stages
- better CI/CD decomposition

---

## Demo Applications

The repository includes several demos:

- `WinFormsDemo`
- `PictureBoxDemo`
- `MusicDemo`
- `SmartWatchDemo`
- `SerialPort`

These demos are valuable not only as examples, but also as:

- regression validation targets
- publishing validation targets
- control capability demonstrations
- runtime host smoke tests

For blog writing, these are ideal concrete examples and screenshots sources.

---

## Build and Release System

The project already has a structured GitHub Actions setup covering:

- release metadata preparation
- native library build
- demo asset build
- NuGet package creation
- release publishing

See also:

- [`ci-workflows.md`](./ci-workflows.md)

Key characteristics of the current CI design:

- branch and PR runs focus on validation
- tags trigger the full release pipeline
- demo and NuGet packaging are decoupled
- prepare / pack / publish stages are reusable

This means the repository is not just 揵uildable敆it is moving toward a sustainable release engineering structure.

---

## Documentation Site and Blog

The project is a good fit for publishing a GitHub Pages site.

Recommended Pages content includes:

- Chinese and English project overview pages
- CI workflow explanation
- roadmap and runtime plans
- WSL developer guidance
- future deep dives on hosts, AOT, controls, and portability

A practical Pages structure can include:

- `docs/index.md` ?Chinese homepage
- `docs/index.en.md` ?English homepage
- `docs/ci-workflows.md` ?CI explanation
- `docs/WSL-Developer-Guide.md` ?WSL guide
- later additions for host-specific and topic-specific documentation

---

## Recommended Reading Path

If this is your first time exploring the project, a good order is:

1. this page, for project positioning and value
2. [`ci-workflows.md`](./ci-workflows.md), for the engineering pipeline
3. [`ROADMAP.md`](../ROADMAP.md), for future direction
4. the blog articles, for design tradeoffs and deeper technical context

---

## Good Blog Angles for This Project

If you want to write a blog series around this repository, good angles include:

### 1. Why build WinForms over LVGL?

Topics:

- the gap between desktop productivity and device deployment
- why not write only C-based LVGL code
- why keeping the WinForms mental model still matters

### 2. Migrating WinForms toward cross-platform UI

Topics:

- multi-targeting
- automatic runtime registration
- layout migration
- common compatibility pitfalls

### 3. NativeAOT and GUI engineering

Topics:

- what AOT changes in UI framework design
- why reflection-heavy activation becomes a problem
- how explicit startup pipelines improve deployability

### 4. Linux host evolution

Topics:

- X11 / WSLg / framebuffer today
- why `Wayland`, `SDL`, `DRM`, and `KMS` matter next
- differences between desktop environments and device-side hosts

### 5. From 搃t runs?to 搃t ships?

Topics:

- package decomposition
- native asset distribution
- CI/CD workflow decomposition
- Release and Pages organization

---

## The Current State of the Project

`LVGLSharp.Forms` is still experimental, but it is no longer just a proof-of-concept idea.

At this point, the repository already shows strong signs of an emerging framework direction:

- a layered architecture
- clear runtime boundaries
- an explicit host roadmap
- a real package model
- a reusable CI/CD layout
- a growing documentation and release story

In other words, it is moving from:

- 揳n interesting prototype?

into:

- 揳 real engineering direction for cross-platform .NET UI over LVGL?

---

## Who Should Watch This Project

This project may be especially interesting if you are:

- a .NET developer
- a long-time WinForms developer
- an embedded UI developer
- a Linux device-side application developer
- a NativeAOT enthusiast
- someone interested in UI runtime design, cross-platform hosting, and release engineering

---

## Open Source and Community

The project is released under the MIT License.

It is also a strong candidate for ongoing knowledge sharing through:

- GitHub Issues
- GitHub Discussions
- Release Notes
- GitHub Pages
- technical blog posts

---

## Further Reading

You can continue with:

- [`ci-workflows.md`](./ci-workflows.md)
- [`WSL-Developer-Guide.md`](./WSL-Developer-Guide.md)
- [`ROADMAP.md`](../ROADMAP.md)
- [`CHANGELOG.md`](../CHANGELOG.md)
- [`README_en.md`](../README_en.md)

---

## Closing Thoughts

The meaning of `LVGLSharp.Forms` is bigger than 搑unning WinForms on LVGL?

It is really an attempt to answer a deeper question:

> Can we keep the development efficiency of the .NET desktop world while gaining the cross-platform, lightweight, device-friendly, and NativeAOT-friendly advantages of LVGL?

This repository represents a serious engineering path toward that answer.

If you are looking for:

- a bridge from WinForms to cross-platform UI
- a higher-level .NET abstraction over LVGL
- a promising direction for device-deployable .NET GUI applications

then this project is worth following.
