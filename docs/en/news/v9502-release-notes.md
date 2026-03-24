---
title: "v9.5.0.2 Release Notes: where the LVGL 9.5 line gained clearer runtime, docs, and dependency boundaries"
description: Records what v9.5.0.2 actually delivered across Headless, MacOs, Remote, the docs site, and the dependency changes made on top of the LVGL 9.5 baseline.
lang: en
---

# v9.5.0.2 Release Notes: where the LVGL 9.5 line gained clearer runtime, docs, and dependency boundaries

> March 24, 2026. `v9.5.0.2` was not just another small tag on top of the same codebase. It stayed on the same `LVGL 9.5` baseline, but it pushed three things forward at once: runtime separation, a first coherent docs site, and a cleaner dependency and packaging story.

## What this release actually moved forward

- It introduced `LVGLSharp.Runtime.Headless`, separating `OffscreenView` out of the Linux host branch and pairing it with `OffscreenDemo` plus the first snapshot regression tests.
- It introduced `LVGLSharp.Runtime.MacOs` and `LVGLSharp.Runtime.Remote` skeletons, creating dedicated runtime boundaries for future `macOS`, `VNC`, and `RDP` work.
- It added the `DrmView` entry point so the Linux device-side path no longer stopped at `FrameBuffer`.
- It pushed the docs site from scattered Markdown into a structured site with a home page, navigation, blog, news, screenshot pages, and local preview tooling.
- It also brought new validation projects into the repo at the same time, including `WinFormsVncDemo`, `WinFormsRdpDemo`, `MacOsAotDemo`, and `OffscreenDemo`.

## Why I think this release deserved its own article

If you only look at the tag name, `v9.5.0.2` can look like a minor patch. In practice, it moved three layers of the repository forward together:

- runtime structure: Headless, MacOs, and Remote started to take shape
- engineering structure: Native dependency handling became more deliberate
- documentation structure: the site became something people could actually use to understand the project

Later, `v9.5.0.5` became a stronger release-facing summary. But `v9.5.0.2` is the point where this more structured phase really began.

## What changed in the 9.5 dependency story

One important clarification first: `v9.5.0.2` did not switch the project to a newer LVGL major line again. It stayed on the same `LVGL 9.5` baseline. The dependency changes were mostly about .NET package boundaries, project-reference strategy, and the new runtime packages introduced around that baseline.

| Area | Change | Why it mattered |
|---|---|---|
| Repository version baseline | Added `LVGLSharpVersion` and `LVGLSharpNativePackageVersion` | Centralized the repo version and the Native package version so repo builds, demos, and packaging logic could share the same baseline |
| `LVGLSharp.Interop` | Keeps a `PackageReference` while packing, but prefers a `ProjectReference` to `LVGLSharp.Native` when building from source | Avoided restore failures when a pre-published Native package was not available, while still preserving correct downstream NuGet metadata |
| Demo projects | Non-`Release` builds use the published `LVGLSharp.Native` package, while `Release` builds switch to the local project | Gave debugging, restore, and release publishing more appropriate dependency paths |
| Image and font stack | `SixLabors.Fonts 2.1.3`, `SixLabors.ImageSharp 3.1.12`, `SixLabors.ImageSharp.Drawing 2.1.7` | Formed the shared image and font base for `Core`, `Forms`, and the new Headless / MacOs / Remote paths |
| Windows-side dependencies | `System.Drawing.Common 10.0.5`, `System.IO.Ports 10.0.5`, `System.Resources.Extensions 10.0.5` | Preserved the desktop-side capabilities used by the Windows runtime and demos |
| Analyzer dependencies | `Microsoft.CodeAnalysis.Analyzers 5.3.0`, `Microsoft.CodeAnalysis.CSharp 5.3.0` | Continued to support `LVGLSharp.Analyzers` |
| Test dependencies | `Microsoft.NET.Test.Sdk 17.14.1`, `xunit 2.9.3`, `xunit.runner.visualstudio 3.1.1` | Added the first real automated validation base for Headless snapshots and the new runtime skeletons |

## What these dependency changes solved

- They removed a real pain point where the source repo could build, but restore became awkward if `LVGLSharp.Native` had not already been published.
- They let `LVGLSharp.Interop` keep correct NuGet dependency metadata without forcing local development to depend on an external feed.
- They gave Headless, MacOs, and Remote a shared image, font, and test baseline from the start instead of leaving those dependencies to drift in later.
- They made it easier to keep pushing on runtime ownership, `DRM/KMS`, and Headless / Remote validation without making the package graph harder to reason about over time.

## How I would place `v9.5.0.2` in the wider 9.5 line

I see it as a structural milestone:

- `v9.5.0.0` was the point where the repo moved onto `LVGL 9.5`
- `v9.5.0.2` was the point where runtime layering, docs, and dependency handling started to separate cleanly
- `v9.5.0.5` was the point where those capabilities were packaged into a more complete release-facing summary

If you want to understand why the repo now contains `Headless`, `MacOs`, `Remote`, `OffscreenDemo`, a structured docs site, and the Native dependency split logic, `v9.5.0.2` is one of the best points to look back at.

## Continue reading

- [Changelog](https://github.com/IoTSharp/LVGLSharp/blob/main/CHANGELOG.md)
- [Roadmap](https://github.com/IoTSharp/LVGLSharp/blob/main/ROADMAP.md)
