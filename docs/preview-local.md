---
title: 本地预览说明
description: GitHub Pages 本地预览所需的 Ruby 与 Bundler 依赖说明。
lang: zh-CN
---

# 本地预览说明

当前站点使用 GitHub Pages 兼容的 Jekyll 方案，本地预览需要先准备 Ruby 环境。

## 依赖要求

- Ruby
- Bundler
- 仓库根目录下执行 `bundle install`

## 本地预览步骤

1. 安装 Ruby
2. 安装 Bundler
3. 在仓库根目录执行 `bundle install`
4. 再运行预览脚本或 Pages 构建任务

## 当前已知情况

本地构建失败的直接原因不是页面模板错误，而是当前环境缺少 Bundler。
