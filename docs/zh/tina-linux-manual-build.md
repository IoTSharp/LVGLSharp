---
title: Tina Linux 手工构建说明
description: 说明如何在已经准备好的 Tina Linux 交叉编译环境中，使用仓库里的全志脚本生成 lv_conf.h，并执行 lunch、make 和 pack。
lang: zh-CN
---

# Tina Linux 手工构建说明

本文只保留手工构建方案，不再引入 Docker 镜像、GitHub Actions 自动构建或镜像仓库发布流程。

适用前提：

- 你已经按全志官方文档准备好了 Tina Linux 主机依赖和交叉编译环境。
- 你已经成功获取了 Tina SDK 源码。
- 你知道自己要构建的 `lunch` 目标。
- 你知道 Tina SDK 里最终要使用的 `lv_conf.h` 目标路径，或者仓库里只能找到唯一一个 LVGL 配置位置。

官方参考文档：

- 环境准备：<https://docs.aw-ol.com/d1/study/study_2ubuntu/>
- 获取 SDK：<https://docs.aw-ol.com/d1/study/study_3getsdktoc/>
- 编译与打包：<https://docs.aw-ol.com/d1/study/study_4compile/>

## 1. 最小手工流程

进入 Tina SDK 根目录后，最小命令链就是：

```bash
source build/envsetup.sh
lunch d1-h_nezha-tina
make -j"$(nproc)"
pack
```

说明：

- `d1-h_nezha-tina` 只是全志 D1 文档里的示例目标。
- 如果你使用的是别的板级包、别的产品分支，`lunch` 目标要替换成你自己的配置。
- `pack` 的输出目录以 Tina 版本和板级配置为准，通常在 `out/` 相关目录下查看。

## 2. 使用仓库脚本构建

仓库里提供了一个辅助脚本，用来简化 `lv_conf.h` 生成、`lunch`、`make` 和 `pack` 这几步。

脚本路径：

- [allwinner-tina-build.sh](d:/source/LVGLSharp/libs/allwinner-tina-build.sh)

这个脚本会按你之前提供的全志 `mk` 配方，从 `libs/lvgl/lv_conf_template.h` 生成一份全志专用 `lv_conf.h`。

默认生成位置：

- `libs/lv_conf_allwinner_tina.h`

如果你只是想先生成这份文件：

```bash
bash /path/to/LVGLSharp/libs/allwinner-tina-build.sh --generate-lv-conf-only
```

如果你已经在 Tina SDK 根目录中：

```bash
bash /path/to/LVGLSharp/libs/allwinner-tina-build.sh \
  --lunch d1-h_nezha-tina \
  --lv-conf-target /path/to/tina-sdk/somewhere/lvgl/lv_conf.h
```

如果你不在 SDK 根目录中：

```bash
bash /path/to/LVGLSharp/libs/allwinner-tina-build.sh \
  --sdk-dir /path/to/tina-sdk \
  --lunch d1-h_nezha-tina \
  --lv-conf-target /path/to/tina-sdk/somewhere/lvgl/lv_conf.h
```

## 3. 常用参数

只编译不打包：

```bash
bash /path/to/LVGLSharp/libs/allwinner-tina-build.sh \
  --sdk-dir /path/to/tina-sdk \
  --lunch d1-h_nezha-tina \
  --lv-conf-target /path/to/tina-sdk/somewhere/lvgl/lv_conf.h \
  --no-pack
```

编译前先清理：

```bash
bash /path/to/LVGLSharp/libs/allwinner-tina-build.sh \
  --sdk-dir /path/to/tina-sdk \
  --lunch d1-h_nezha-tina \
  --lv-conf-target /path/to/tina-sdk/somewhere/lvgl/lv_conf.h \
  --clean
```

指定并行度：

```bash
bash /path/to/LVGLSharp/libs/allwinner-tina-build.sh \
  --sdk-dir /path/to/tina-sdk \
  --lunch d1-h_nezha-tina \
  --lv-conf-target /path/to/tina-sdk/somewhere/lvgl/lv_conf.h \
  -j 8
```

给 `pack` 追加参数：

```bash
bash /path/to/LVGLSharp/libs/allwinner-tina-build.sh \
  --sdk-dir /path/to/tina-sdk \
  --lunch d1-h_nezha-tina \
  --lv-conf-target /path/to/tina-sdk/somewhere/lvgl/lv_conf.h \
  --pack-arg -d
```

如果你已经有自己维护的 `lv_conf.h`，也可以显式指定：

```bash
bash /path/to/LVGLSharp/libs/allwinner-tina-build.sh \
  --sdk-dir /path/to/tina-sdk \
  --lunch d1-h_nezha-tina \
  --lv-conf /path/to/custom/lv_conf.h \
  --lv-conf-target /path/to/tina-sdk/somewhere/lvgl/lv_conf.h
```

## 4. 脚本行为说明

这个脚本会做四件事：

1. 根据全志 `mk` 配方生成或使用指定的 `lv_conf.h`。
2. 在构建前把这份 `lv_conf.h` 复制到 Tina SDK 指定目标位置。
3. `source build/envsetup.sh` 并调用 `lunch <target>`。
4. 执行 `make`，并按需要执行 `pack`。

它不会负责：

- 安装宿主依赖。
- 拉取 Tina SDK。
- 自动判断你的板级目标。
- 自动修复环境问题。

## 5. 和原始全志 `mk` 的映射说明

你之前给的 `mk` 里有几项宏名，和当前仓库使用的 `LVGL 9.5.0` 模板并不是一字不差对应的，所以脚本里做了语义映射：

- `LV_USE_LINUX_INPUT` 映射为 `LV_USE_EVDEV`
- `LV_USE_DRAW_ARM2D` 映射为 `LV_USE_DRAW_ARM2D_SYNC`

另外两项在当前 `LVGL 9.5.0` 模板里没有直接对应的同名开关，所以这次没有机械写入：

- `LV_TICK_CUSTOM`
- `LV_USE_USER_DATA`

脚本生成的全志专用配置，已经覆盖了那份 `mk` 里最关键的设备侧参数：

- `LV_COLOR_DEPTH 32`
- `LV_MEM_SIZE (64 * 1024U)`
- `LV_FONT_MONTSERRAT_16 1`
- `LV_USE_LINUX_FBDEV 1`
- `LV_USE_EVDEV 1`
- `LV_USE_LOG 1`
- `LV_LOG_PRINTF 1`
- `LV_LOG_LEVEL LV_LOG_LEVEL_WARN`
- `LV_USE_ASSERT_NULL 0`
- `LV_USE_ASSERT_MALLOC 0`
- `LV_USE_ASSERT_STYLE 0`
- `LV_USE_ASSERT_MEM_INTEGRITY 0`
- `LV_USE_ASSERT_OBJ 0`

## 6. 推荐给团队的使用方式

如果团队成员已经各自准备好了 Tina 构建环境，推荐统一做法是：

1. 先按官方文档准备环境并拉好 SDK。
2. 再统一使用 [allwinner-tina-build.sh](d:/source/LVGLSharp/libs/allwinner-tina-build.sh) 执行构建。
3. 把板级差异收敛到 `--lunch` 和 `--lv-conf-target` 参数，而不是把流程分散到不同人的手工命令里。

这样做的好处是：

- 命令更一致。
- 更容易复现别人机器上的构建过程。
- `lv_conf.h` 来源更清楚，不会一会儿手改、一会儿模板生成。
- 后续如果要补日志、清理或参数校验，只需要改一个脚本。
