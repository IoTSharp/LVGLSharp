---
title: Screenshot Gallery
description: A visual preview page for current LVGLSharp screenshots, ready to grow as more demos and hosts are captured.
lang: en
template: structured
intro:
  eyebrow: "Screenshots"
  title: "Screenshot Gallery"
  description: "This page is meant to collect the current `LVGLSharp` visual previews in one place. As more demos, runtime hosts, and UI states are captured, they can be added here without mixing them into regular articles."
sections:
  - title: "Current screenshots"
    description: "Use this area for representative runtime, host, and UI screenshots. Click an image to open the original file."
    variant: showcase
    columns: 2
    items:
      - title: "Desktop Preview A"
        badge: "Desktop"
        image: "/images/showcase/desktop-preview-1.png"
        alt: "LVGLSharp desktop preview screenshot A"
        description: "A compact screenshot for showing layout direction, control style, and the overall UI structure."
      - title: "Desktop Preview B"
        badge: "Desktop"
        image: "/images/showcase/desktop-preview-2.png"
        alt: "LVGLSharp desktop preview screenshot B"
        description: "A fuller desktop capture that shows denser content, control composition, and the current demo surface."
      - title: "WSLg / Wayland PictureBox Check"
        badge: "WSLg"
        image: "/images/wslg-pictureboxdemo-wayland-embedded-font-check.png"
        alt: "WSLg Wayland PictureBox embedded font check"
        description: "Tracks Linux-host rendering and embedded-font validation in a real WSLg / Wayland scenario."
  - title: "This round of X11 validation"
    description: "These screenshots came from the current X11 bring-up work and pair well with the [X11 Demo Bring-up Notes](/en/blog/x11-demo-bringup.html)."
    variant: showcase
    columns: 3
    items:
      - title: "PictureBoxDemo on X11"
        badge: "X11"
        image: "/images/x11-pictureboxdemo.png"
        alt: "PictureBoxDemo running on X11"
        description: "`PictureBoxDemo` is now launchable on X11 and has a reusable screenshot asset. The current stable capture uses the built-in font fallback path, so CJK labels still need a later recovery pass."
      - title: "MusicDemo on X11"
        badge: "X11"
        image: "/images/x11-musicdemo.png"
        alt: "MusicDemo running on X11"
        description: "After fixing default font installation and the glyph bitmap callback contract, `MusicDemo` can now open stably on X11 and be captured."
      - title: "SmartWatchDemo on X11"
        badge: "X11"
        image: "/images/x11-smartwatchdemo.png"
        alt: "SmartWatchDemo running on X11"
        description: "`SmartWatchDemo` now has a working X11 first-screen capture path and serves as the representative multi-page UI case from this round."
  - title: "What to add next"
    description: "This page can grow over time without turning screenshots into top-level navigation clutter."
    variant: cards
    columns: 3
    items:
      - title: "Demo screenshots"
        description: "Add key views from `MusicDemo`, `SmartWatchDemo`, `PictureBoxDemo`, `WinFormsDemo`, and future samples."
      - title: "Multi-host comparisons"
        description: "Capture the same UI across Windows, WSLg, X11, Wayland, FrameBuffer, and Offscreen environments."
      - title: "Before / after UI changes"
        description: "Use side-by-side progress shots to record rendering improvements, theme updates, and control refinements."
  - title: "How to maintain it"
    description: "Keep future additions simple and consistent."
    variant: list
    surface: true
    items:
      - label: "Place new screenshots under `docs/images/`; keep using `docs/images/showcase/` for generic curated gallery assets when helpful"
      - label: "Append a new item under the page front matter `sections.items` collection"
      - label: "Prefer screenshots that highlight real runtime hosts, representative demos, and meaningful visual progress"
---
