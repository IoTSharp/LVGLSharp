---
title: 文档导航
description: 按主题、角色和语言快速浏览 LVGLSharp 文档与博客内容。
lang: zh-CN
template: structured
intro:
  eyebrow: "Documentation Map"
  title: "文档导航"
  description: "把首页、博客、工程说明和运行时路线整理成可快速跳转的阅读地图。"
sections:
  - title: "站点入口"
    description: "适合第一次进入站点时快速建立路径感。"
    variant: link-lists
    columns: 2
    items:
      - title: "首页与语言切换"
        links:
          - label: "中文首页"
            url: "/"
          - label: "English Home"
            url: "/index.en.html"
          - label: "English Navigation"
            url: "/navigation.en.html"
          - label: "博客索引"
            url: "/blog/index.html"
      - title: "项目文档"
        links:
          - label: "路线图"
            url: "https://github.com/IoTSharp/LVGLSharp/blob/main/ROADMAP.md"
          - label: "更新记录"
            url: "https://github.com/IoTSharp/LVGLSharp/blob/main/CHANGELOG.md"
          - label: "CI 工作流说明"
            url: "/ci-workflows.html"
          - label: "WSL 开发指南"
            url: "/WSL-Developer-Guide.html"
          - label: "本地预览说明"
            url: "/preview-local.html"
  - title: "按读者类型进入"
    description: "从你的关注点出发，而不是从文件名猜内容。"
    variant: link-lists
    columns: 4
    items:
      - title: "第一次了解项目"
        links:
          - label: "项目首页"
            url: "/"
          - label: "为什么要做 WinForms over LVGL"
            url: "/blog-winforms-over-lvgl.html"
          - label: "英文首页"
            url: "/index.en.html"
      - title: "架构与工程化"
        links:
          - label: "项目架构拆解"
            url: "/blog-architecture.html"
          - label: "CI 工作流说明"
            url: "/ci-workflows.html"
          - label: "CI Workflow Guide"
            url: "/ci-workflows.en.html"
      - title: "Linux / 运行时宿主"
        links:
          - label: "Linux 图形宿主路线"
            url: "/blog-linux-hosts.html"
          - label: "WSL 开发指南"
            url: "/WSL-Developer-Guide.html"
          - label: "WSL Developer Guide"
            url: "/WSL-Developer-Guide.en.html"
      - title: "AOT / 发布"
        links:
          - label: "NativeAOT 与 GUI"
            url: "/blog-nativeaot-gui.html"
          - label: "更新记录"
            url: "https://github.com/IoTSharp/LVGLSharp/blob/main/CHANGELOG.md"
          - label: "路线图"
            url: "https://github.com/IoTSharp/LVGLSharp/blob/main/ROADMAP.md"
  - title: "双语对照入口"
    description: "如果你要发给不同语言读者，可以从这里直接切换。"
    variant: list
    surface: true
    items:
      - label: "English Navigation"
        url: "/navigation.en.html"
      - label: "English Blog Index"
        url: "/blog/index.en.html"
      - label: "README English"
        url: "https://github.com/IoTSharp/LVGLSharp/blob/main/README_en.md"
---
