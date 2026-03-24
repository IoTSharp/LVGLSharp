---
title: v9.5.0.2 发布纪要：述 LVGL 9.5 基线下运行时、文档与依赖之整饬
description: 兹记 v9.5.0.2 发布要旨，述 Headless、MacOs、Remote 路径之设，及 LVGL 9.5 基线下依赖边界与打包关系之整饬。
lang: zh-CN
---

# v9.5.0.2 发布纪要：述 LVGL 9.5 基线下运行时、文档与依赖之整饬

> 兹于 2026 年 3 月 24 日记 `v9.5.0.2` 发布要旨。本版仍承 `LVGL 9.5` 之基线，所务者，不在更易上游主版本，而在整饬运行时边界、文档体例与依赖关系，使工程由“可用”更进一步，臻于“可述、可验、可续”。

## 是版发布要旨

- 增设 `LVGLSharp.Runtime.Headless`，将 `OffscreenView` 自 Linux 宿主分支中独立出来，并配 `OffscreenDemo` 与首批快照回归测试。
- 增设 `LVGLSharp.Runtime.MacOs` 与 `LVGLSharp.Runtime.Remote` 骨架，为 `macOS`、`VNC`、`RDP` 路径预留独立运行时边界。
- 补 `DrmView` 入口，使 Linux 设备侧路线不复止于 `FrameBuffer`。
- 文档站自零散 Markdown 进于结构化形态，补首页、导航、博客、新闻、截图页及本地预览脚本。
- `WinFormsVncDemo`、`WinFormsRdpDemo`、`MacOsAotDemo`、`OffscreenDemo` 等示例同时入仓，表明此番不惟立说，亦同时安置验证入口。

## 是版何以别记

`v9.5.0.2` 虽名补订，实为 `9.5` 一线中承上启下之节点。按仓库差异观之，此版所推进者有三：

- 运行时层：Headless、MacOs、Remote 始具其形。
- 工程层：Native 依赖之引用方式开始收拢，CI、打包与本地构建关系愈加明晰。
- 文档层：站点初具体系，足以正式承载项目叙述与对外说明。

后之 `v9.5.0.5`，更近于对外说明之整编版；而 `v9.5.0.2`，则是此轮结构化演进真正发轫之处。

## 9.5 依赖整饬记

先申一义：`v9.5.0.2` 仍行于同一条 `LVGL 9.5` 基线之上，并非再迁上游主版本。其依赖之变，所重者主要在 .NET 包边界、项目引用策略，以及新运行时包之设。

| 范围 | 变动 | 说明 |
|---|---|---|
| 仓库版本基线 | 新增 `LVGLSharpVersion` 与 `LVGLSharpNativePackageVersion` | 把仓库版本和 Native 包版本统一成可复用属性，便于 repo、demo 和打包脚本共用 |
| `LVGLSharp.Interop` | pack 时保留 `PackageReference`，源码仓库构建优先 `ProjectReference` 到 `LVGLSharp.Native` | 解决本地开发和 CI 在 Native 包尚未预发布时的还原问题，同时保留下游 NuGet 依赖元数据 |
| Demo 工程 | 非 `Release` 默认引用已发布 `LVGLSharp.Native` 包，`Release` 改用本地项目 | 让调试、还原和正式发布各走更合适的依赖路径 |
| 图像与字体栈 | `SixLabors.Fonts 2.1.3`、`SixLabors.ImageSharp 3.1.12`、`SixLabors.ImageSharp.Drawing 2.1.7` | 作为 `Core`、`Forms` 和新 Headless/MacOs/Remote 路径的图像与字体基础 |
| Windows 侧依赖 | `System.Drawing.Common 10.0.5`、`System.IO.Ports 10.0.5`、`System.Resources.Extensions 10.0.5` | 维持 Windows runtime 和相关 Demo 的桌面侧能力 |
| 分析器依赖 | `Microsoft.CodeAnalysis.Analyzers 5.3.0`、`Microsoft.CodeAnalysis.CSharp 5.3.0` | 继续支撑 `LVGLSharp.Analyzers` |
| 测试依赖 | `Microsoft.NET.Test.Sdk 17.14.1`、`xunit 2.9.3`、`xunit.runner.visualstudio 3.1.1` | 给 Headless 快照回归和新 runtime 骨架提供第一批自动化验证入口 |

## 此番整饬所解何事

- 解“仓库源码可编，而 `Native` 包未先发布则还原不便”之患。
- 使 `LVGLSharp.Interop` 既得保全 NuGet 依赖元数据，又不强令本地开发倚赖外部 feed。
- 使 Headless、MacOs、Remote 诸新路径，自始即立于统一图像、字体与测试基线之上，而非俟后零补。
- 使后续继续拆分 runtime ownership、推进 `DRM/KMS`、Remote 与 Headless 验证时，包图不致日益纷乱。

## 于 9.5 一线中之位置

若回观 `9.5` 一线，可作如下理解：

- `v9.5.0.0` 是切到 `LVGL 9.5` 的起点。
- `v9.5.0.2` 是把运行时、文档与依赖关系真正拉开层次之节点。
- `v9.5.0.5` 是把诸能力整理为较完整对外说明之节点。

若欲明白何以仓库中会同时出现 `Headless`、`MacOs`、`Remote`、`OffscreenDemo`、结构化文档站，以及 Native 依赖拆分逻辑，则 `v9.5.0.2` 正是最宜回溯之一版。

## 参阅

- [项目变更日志](https://github.com/IoTSharp/LVGLSharp/blob/main/CHANGELOG.md)
- [项目路线图](https://github.com/IoTSharp/LVGLSharp/blob/main/ROADMAP.md)
