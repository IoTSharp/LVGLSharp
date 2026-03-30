---
title: LVGLSharp Architecture Breakdown
description: Describes how Forms, Core, Drawing, Interop, Native, Runtime, and tooling layers are organized in the project.
lang: en
---

# LVGLSharp Architecture Breakdown

## Why the project needs layering

To support all of the following at once:

- a WinForms-style API
- cross-platform execution
- multiple host environments
- NativeAOT-friendly deployment
- maintainable packaging and release engineering

the project cannot live in a single monolithic assembly.

One of the strongest signs of maturity in `LVGLSharp` is that it is gradually forming a clear package and library structure.

## Layer 1: The Forms application layer

`LVGLSharp.Forms` is the main application-facing package. It provides:

- `Form`
- `Control`
- common controls
- events and lifecycle semantics
- the familiar `ApplicationConfiguration.Initialize()` startup flow

This is the layer closest to the WinForms development experience and the package most application code should start from.

## Layer 2: The shared Core and Drawing foundation

This layer is mainly built from `LVGLSharp.Core` and `LVGLSharp.Drawing`.

`LVGLSharp.Core` exists to provide:

- common abstractions
- view lifetime infrastructure
- shared font and image helpers
- native-library resolution and diagnostics support

`LVGLSharp.Drawing` provides:

- `Point`
- `Size`
- `Rectangle`
- `Color`
- other drawing-oriented primitives without a direct dependency on `System.Drawing`

Its purpose is to keep the upper application layer and the lower runtime layer from becoming tightly coupled.

## Layer 3: The Interop layer

`LVGLSharp.Interop` gives .NET access to the full LVGL API surface.

This matters because the project is not trying to trap developers inside only one high-level abstraction. It keeps the path open all the way down to LVGL when needed.

## Layer 4: The Native distribution layer

`LVGLSharp.Native` is responsible for native library packaging and distribution.

This is a highly practical engineering layer:

- without it, multi-platform native library distribution becomes messy
- with it, NuGet packaging and CI orchestration become much cleaner
- runtime packages can consume the native assets through a predictable distribution path

## Layer 5: Platform and scenario runtime packages

The runtime line now extends beyond the original Windows and Linux split:

- `LVGLSharp.Runtime.Windows`
- `LVGLSharp.Runtime.Linux`
- `LVGLSharp.Runtime.Headless`
- `LVGLSharp.Runtime.MacOs`
- `LVGLSharp.Runtime.Remote`

Together they separate:

- Windows hosting
- Linux hosting and graphics-host selection
- headless rendering and snapshot flows
- macOS host boundaries and diagnostics scaffolding
- remote sessions, frame transport, and input abstractions

That means application code does not need to absorb all host differences directly.

## Layer 6: Supplemental application-model and tooling libraries

The repository also carries important supporting libraries outside the main public NuGet publishing line:

- `LVGLSharp.WPF`
- `LVGLSharp.Analyzers`

They play different roles:

- `LVGLSharp.WPF` is the experimental WPF-like bootstrap and XAML runtime path
- `LVGLSharp.Analyzers` enforces package-usage guidance and repository-specific rules during build

Even though they are not part of the primary package lineup, they are important to how the project evolves.

## Layer 7: CI, packaging, and documentation

A project becomes real not only when it runs, but when it can:

- build repeatedly
- package cleanly
- publish reliably
- explain itself clearly

The repository’s CI decomposition, NuGet metadata, package readmes, and docs site are all part of that final layer.

## Closing thought

What makes `LVGLSharp` interesting is not only that it connects WinForms and LVGL, but that it is forming a complete engineering structure around that idea:

- application layer
- shared foundation layer
- interop layer
- native distribution layer
- runtime layer
- tooling and supplemental application-model layer
- release and documentation layer

That is the kind of structure a project needs if it wants to grow beyond a prototype.
