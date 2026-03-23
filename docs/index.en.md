---
title: LVGLSharp Home
description: Bring a WinForms-style development experience to LVGL-powered cross-platform UI.
lang: en
---

<section class="hero">
  <div class="hero-panel">
    <span class="kicker">Cross-platform GUI · WinForms over LVGL · NativeAOT Ready</span>
    <h1>Keep the familiar WinForms mindset, target much broader runtimes.</h1>
    <p class="lead">
      <strong>LVGLSharp</strong> uses <strong>LVGL</strong> as the rendering core while exposing a .NET-friendly, WinForms-style development model. The goal is to bridge desktop productivity with Windows, Linux, and device-oriented runtime environments, while continuing to improve <strong>NativeAOT</strong>, lightweight deployment, and host portability.
    </p>
    <div class="button-row">
      <a class="button primary" href="./navigation.en.md">Start with docs</a>
      <a class="button secondary" href="./blog/index.en.md">Read the blog</a>
      <a class="button secondary" href="https://github.com/IoTSharp/LVGLSharp">GitHub repository</a>
    </div>
    <div class="pill-row">
      <span class="pill">Windows / Linux</span>
      <span class="pill">LVGL Rendering</span>
      <span class="pill">WinForms-style API</span>
      <span class="pill">NativeAOT Friendly</span>
    </div>
  </div>
  <div class="hero-panel hero-side">
    <div>
      <div class="kicker">Project Snapshot</div>
      <div class="code-window">dotnet add package LVGLSharp.Forms

Application.Run(new MainForm());

// Familiar form model
// LVGL-backed rendering
// Cross-platform runtime layering
// Device-oriented deployment path</div>
    </div>
    <div class="surface">
      <h3>In one sentence</h3>
      <p>This is not just a thin .NET wrapper over LVGL. It is an attempt to preserve the WinForms development model while moving rendering and runtime hosting toward a lighter, more portable, and AOT-friendly architecture.</p>
    </div>
  </div>
</section>

<section class="stats-row">
  <div class="stat-card">
    <div class="stat-label">Core identity</div>
    <div class="stat-value">WinForms over LVGL</div>
  </div>
  <div class="stat-card">
    <div class="stat-label">Main platforms</div>
    <div class="stat-value">Windows + Linux</div>
  </div>
  <div class="stat-card">
    <div class="stat-label">Publishing direction</div>
    <div class="stat-value">NativeAOT</div>
  </div>
  <div class="stat-card">
    <div class="stat-label">Engineering target</div>
    <div class="stat-value">Cross-platform UI Runtime</div>
  </div>
</section>

<section class="section">
  <div class="section-header">
    <h2>Why it matters</h2>
    <p>It connects familiar desktop development productivity with lightweight runtime capabilities for broader display hosts and deployment models.</p>
  </div>
  <div class="card-grid">
    <div>
      <h3>Familiar development flow</h3>
      <p>Preserves forms, controls, events, and layout thinking so developers can reuse the WinForms mental model instead of starting from a lower-level graphics stack.</p>
    </div>
    <div>
      <h3>LVGL rendering foundation</h3>
      <p>Builds on LVGL for lightweight, high-performance, and device-adaptive rendering across broader platform and host combinations.</p>
    </div>
    <div>
      <h3>AOT and lightweight deployment</h3>
      <p>Moves toward NativeAOT, self-contained publishing, and device-friendly packaging with lower runtime overhead.</p>
    </div>
  </div>
</section>

<section class="section">
  <div class="section-header">
    <h2>Quick entry points</h2>
    <p>If you want to evaluate the project quickly, these are the best places to start.</p>
  </div>
  <div class="quick-links">
    <a href="./navigation.en.md">
      <strong>Documentation map</strong>
      <span>Jump into the right reading path by topic, audience, and language.</span>
    </a>
    <a href="./ci-workflows.en.md">
      <strong>Engineering and CI</strong>
      <span>Understand the build, packaging, publishing, and automation structure.</span>
    </a>
    <a href="./WSL-Developer-Guide.en.md">
      <strong>WSL / Linux development</strong>
      <span>Start here if your main interest is Linux graphics hosts and development workflow.</span>
    </a>
  </div>
</section>

<section class="section">
  <div class="section-header">
    <h2>Recommended reading paths</h2>
    <p>Choose a path based on your interest instead of browsing everything manually.</p>
  </div>
  <div class="article-grid">
    <div class="article-card">
      <h3>First-time readers</h3>
      <p>Best if you want to understand why the project exists and what it is trying to achieve.</p>
      <ul>
        <li><a href="./blog/why-winforms-over-lvgl.en.md">Why WinForms over LVGL</a></li>
        <li><a href="./index.md">中文首页</a></li>
      </ul>
    </div>
    <div class="article-card">
      <h3>Architecture and engineering</h3>
      <p>Best if you care about module boundaries, runtime layering, and repository organization.</p>
      <ul>
        <li><a href="./blog/architecture.en.md">Architecture Breakdown</a></li>
        <li><a href="./ci-workflows.en.md">CI Workflow Guide</a></li>
      </ul>
    </div>
    <div class="article-card">
      <h3>Linux and host strategy</h3>
      <p>Best if you care about X11, WSLg, FrameBuffer, Wayland, and future display-host paths.</p>
      <ul>
        <li><a href="./blog/linux-hosts.en.md">Linux Host Strategy</a></li>
        <li><a href="./WSL-Developer-Guide.en.md">WSL Developer Guide</a></li>
      </ul>
    </div>
    <div class="article-card">
      <h3>AOT and publishing</h3>
      <p>Best if you care about trimming, self-contained deployments, runtime size, and publishing shape.</p>
      <ul>
        <li><a href="./blog/nativeaot-gui.en.md">NativeAOT and GUI</a></li>
        <li><a href="../CHANGELOG.md">Changelog</a></li>
      </ul>
    </div>
  </div>
</section>

<section class="section">
  <div class="section-header">
    <h2>Capability map</h2>
    <p>The project value is not only about drawing UI, but about the runtime structure and engineering model behind it.</p>
  </div>
  <div class="card-grid">
    <div>
      <h3>Forms and controls model</h3>
      <p>Keeps the traditional WinForms programming style, including lifecycle, control tree, events, and layout organization.</p>
    </div>
    <div>
      <h3>Layered runtime structure</h3>
      <p>Organizes capabilities across `Core`, `Interop`, `Runtime.Windows`, and `Runtime.Linux` so rendering and hosting stay decoupled.</p>
    </div>
    <div>
      <h3>Device deployment path</h3>
      <p>Opens a realistic path for x64, ARM, and ARM64 device-side UI scenarios in the .NET ecosystem.</p>
    </div>
  </div>
</section>

<section class="section">
  <div class="section-header">
    <h2>Demos and usage scenarios</h2>
    <p>This project is not only a concept exploration. It is being shaped against real UI scenarios, real runtime hosts, and real publishing paths.</p>
  </div>
  <div class="card-grid">
    <div>
      <h3>Desktop validation</h3>
      <p>Windows and Linux hosts are used to continuously validate lifecycle behavior, control models, and cross-platform runtime consistency.</p>
    </div>
    <div>
      <h3>Device-oriented exploration</h3>
      <p>The runtime roadmap keeps expanding around FrameBuffer, Wayland, SDL, DRM/KMS, and other device-side host strategies.</p>
    </div>
    <div>
      <h3>Demo-driven evolution</h3>
      <p>Examples such as `MusicDemo`, `SmartWatchDemo`, `PictureBoxDemo`, and `WinFormsDemo` help anchor the project in practical usage.</p>
    </div>
  </div>
</section>

<section class="section surface">
  <div class="section-header">
    <h2>Quick start path</h2>
    <p>If you want to get a fast feel for the project, this is the shortest useful path.</p>
  </div>
  <ul>
    <li>Inspect the <a href="https://github.com/IoTSharp/LVGLSharp">GitHub repository</a> for project structure</li>
    <li>Use the <a href="./navigation.en.md">documentation map</a> to choose a reading path</li>
    <li>Read the <a href="./blog/index.en.md">blog index</a> for rationale and architecture tradeoffs</li>
    <li>Study the demo projects under `src/Demos` for practical usage patterns</li>
  </ul>
</section>

<section class="section surface">
  <div class="section-header">
    <h2>Start with these</h2>
    <p>If you only read three pieces first, use this shortlist.</p>
  </div>
  <ul>
    <li><a href="./blog/why-winforms-over-lvgl.en.md">Why WinForms over LVGL</a></li>
    <li><a href="./blog/architecture.en.md">Architecture Breakdown</a></li>
    <li><a href="./blog/nativeaot-gui.en.md">NativeAOT and GUI</a></li>
  </ul>
</section>

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
| Windows runtime | Available |
| Unified Linux runtime entry | Available |
| `Wayland` / `SDL` hosts | Implemented, but currently experimental |
| NativeAOT direction | Supported and actively shaped |
| Multi-platform native packaging | Available |
| GitHub Actions CI/CD | Decomposed into reusable stages |
| GitHub Pages docs site | Basic structure established |

### Recently confirmed progress

- [`ROADMAP.md`](../ROADMAP.md) is now the single place for completed milestones, host maturity, and the recommended next step.
- The Linux runtime already includes `WslgView`, `X11View`, `FrameBufferView`, `WaylandView`, and `SdlView`.
- The best next engineering step is still moving `LVGLSharp.Native` dependency ownership into the runtime packages so `Interop` remains a pure binding layer.

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

## The LVGLSharp Layout Pattern

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
- `WaylandView`
- `SdlView`

This allows the project to:

- reuse the same upper-layer API across multiple Linux environments
- keep host-specific logic inside runtime packages
- keep `Wayland` and `SDL` on the same runtime path while they continue to mature as experimental hosts

---

## AOT and Trimming Philosophy

This is one of the project's most technically important aspects.

Many UI frameworks struggle when moving toward NativeAOT or aggressive trimming because they depend on:

- reflection-based activation
- runtime registration patterns that are hard to analyze statically
- warning suppression instead of architectural fixes

This project explicitly tries to avoid that. It emphasizes:

- no comment-based suppression of AOT or trimming warnings as a design strategy
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

This means the repository is not just buildable; it is moving toward a sustainable release engineering structure.

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

- `docs/index.md` - Chinese homepage
- `docs/index.en.md` - English homepage
- `docs/ci-workflows.md` - CI explanation
- `docs/WSL-Developer-Guide.md` - WSL guide
- later additions for host-specific and topic-specific documentation

---

## Current priority

If we continue from the current repository state, the next three things to prioritize are:

1. move `LVGLSharp.Native` dependency ownership into `LVGLSharp.Runtime.Windows` / `LVGLSharp.Runtime.Linux`
2. add smoke tests and regression validation for `Wayland` / `SDL`
3. keep `README`, `ROADMAP`, `CHANGELOG`, and the docs home pages aligned on the same project state

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
- why `Wayland` / `SDL` still need validation, and why `DRM` / `KMS` matter next
- differences between desktop environments and device-side hosts

### 5. From "it runs" to "it ships"

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

- "an interesting prototype"

into:

- "a real engineering direction for cross-platform .NET UI over LVGL"

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

The meaning of `LVGLSharp.Forms` is bigger than simply "running WinForms on LVGL".

It is really an attempt to answer a deeper question:

> Can we keep the development efficiency of the .NET desktop world while gaining the cross-platform, lightweight, device-friendly, and NativeAOT-friendly advantages of LVGL?

This repository represents a serious engineering path toward that answer.

If you are looking for:

- a bridge from WinForms to cross-platform UI
- a higher-level .NET abstraction over LVGL
- a promising direction for device-deployable .NET GUI applications

then this project is worth following.

