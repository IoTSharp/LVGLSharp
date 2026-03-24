---
title: 全志 T113-S3 的 LVGL 编译参数说明：和当前仓库有何不同
description: 结合 T113-S3 上的 Buildroot 配方，说明它的 lv_conf.h 生成方式，以及和当前 LVGLSharp 仓库编译参数体系的主要差异。
lang: zh-CN
---

# 全志 T113-S3 的 LVGL 编译参数说明：和当前仓库有何不同

如果你在看 `T113-S3 + Tina Linux` 这一类板级环境，这份配置最值得注意的地方，不只是“开了哪些宏”，而是“最终那份 `lv_conf.h` 是谁生成的”。

## 先看结论

和当前仓库相比，`T113-S3` 这套配置最大的区别是配置来源不同：

- `T113-S3` 是构建前动态生成一份板级 `lv_conf.h`
- 当前仓库是长期维护一份固定的 [lv_conf.h](d:/source/LVGLSharp/libs/lv_conf.h)，再由少量 CMake 参数覆盖

所以两边的差异不只是参数值不同，更是“板级打包层控制配置”与“项目级基线配置”之间的差异。

## T113-S3 这份配置是怎么工作的

你给出的内容本质上是一份 Buildroot 风格的 `cmake-package` 配方。它主要做三件事：

1. 声明依赖和 CMake 开关。
2. 在 `pre-configure` 阶段，把 `lv_conf_template.h` 复制成 `lv_conf.h`。
3. 再用一串 `sed` 把这份 `lv_conf.h` 改成适合 `T113-S3` 设备环境的版本。

它的核心路径可以概括成：

```make
LVGL_CONF_OPTS = \
	-DLV_CONF_SKIP=ON \
	-DLV_KCONFIG_IGNORE=ON \
	-DBUILD_SHARED_LIBS=ON

LVGL_CONF_OPTS += \
	-DLV_USE_LINUX_FBDEV=ON \
	-DLV_USE_LINUX_INPUT=ON

define LVGL_CREATE_LV_CONF_H
	cp $(@D)/lv_conf_template.h $(@D)/lv_conf.h
	sed -i 's/#if 0/#if 1/' $(@D)/lv_conf.h
	sed -i 's/#define LV_COLOR_DEPTH 16/#define LV_COLOR_DEPTH 32/' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_LOG.*0|#define LV_USE_LOG 1|' $(@D)/lv_conf.h
	...
endef
```

也就是说，它不是长期维护一份已经成型的 `lv_conf.h`，而是每次构建时按板级需求临时产出一份配置。

## 它和当前仓库最核心的区别

当前仓库更像这样：

- 基线配置放在 [lv_conf.h](d:/source/LVGLSharp/libs/lv_conf.h)
- Linux 构建脚本按宿主类型切换少量 `CONFIG_LV_*` 开关
- 配置文件本身是版本化、可审阅、可直接比较的

例如当前 Linux 构建脚本主要传的是：

- `CONFIG_LV_BUILD_EXAMPLES=OFF`
- `CONFIG_LV_BUILD_DEMOS=OFF`
- `CONFIG_LV_USE_LINUX_FBDEV=ON/OFF`
- `CONFIG_LV_USE_SDL=ON/OFF`
- `CONFIG_LV_USE_LINUX_DRM=ON/OFF`
- `CONFIG_LV_USE_WAYLAND=ON/OFF`

而 `T113-S3` 更像：

- 先从模板生成
- 再由板级脚本改值
- 再由 Buildroot 外层包选项决定是否启用 demo、example、PNG、JPEG 等能力

这会带来两个直接结果：

- `T113-S3` 的配置更偏设备板级定制
- 当前仓库的配置更偏项目级公共基线

## 当前 LVGL 9.5.0 下要先说明的一点

你给的 `mk` 里有几项宏名，和当前仓库使用的 `LVGL 9.5.0` 模板并不是完全一一对应的。

例如在你给的配方里会看到：

- `LV_USE_LINUX_INPUT`
- `LV_USE_PNG`
- `LV_USE_JPEG`
- `LV_USE_EXAMPLES`
- `LV_USE_USER_DATA`

但在当前仓库这份 `LVGL 9.5.0` 模板里，很多能力已经换成了别的命名或别的组织方式。比如 Linux 输入在当前模板里更接近：

- `LV_USE_EVDEV`
- `LV_USE_LIBINPUT`

所以比较这两套配置时，更合理的方式不是机械按宏名逐项对齐，而是按“功能语义”来对齐。

## 和当前仓库相比，最明显的几个不同

## 1. 颜色深度不同

`T113-S3` 配方把：

- `LV_COLOR_DEPTH 16`

改成了：

- `LV_COLOR_DEPTH 32`

而当前仓库的 [lv_conf.h](d:/source/LVGLSharp/libs/lv_conf.h) 基线仍然是：

- `LV_COLOR_DEPTH 16`

这说明 `T113-S3` 更偏向固定屏幕设备上的显示效果和更直接的颜色路径，而当前仓库更偏向控制内存开销和保持更轻量的公共基线。

## 2. 日志策略不同

`T113-S3` 明确打开了：

- `LV_USE_LOG 1`
- `LV_LOG_PRINTF 1`
- `LV_LOG_LEVEL LV_LOG_LEVEL_WARN`

当前仓库基线则更安静：

- `LV_USE_LOG 0`
- `LV_LOG_PRINTF 0`

这很符合板级 bring-up 的需要。设备侧调 framebuffer、输入、字体和初始化路径时，日志通常比极致安静更重要。

## 3. 断言策略不同

`T113-S3` 配方把这些都关掉了：

- `LV_USE_ASSERT_NULL 0`
- `LV_USE_ASSERT_MALLOC 0`
- `LV_USE_ASSERT_STYLE 0`
- `LV_USE_ASSERT_MEM_INTEGRITY 0`
- `LV_USE_ASSERT_OBJ 0`

当前仓库则保留了最基础的两项：

- `LV_USE_ASSERT_NULL 1`
- `LV_USE_ASSERT_MALLOC 1`

这代表两边优化重点不同：

- `T113-S3` 更偏设备运行时开销控制
- 当前仓库更偏开发阶段尽早暴露问题

## 4. 字体基线不同

`T113-S3` 显式打开了：

- `LV_FONT_MONTSERRAT_16 1`

当前仓库这项默认没有打开。

这说明 `T113-S3` 更愿意保留一个稳定的内置字体基线，方便先把设备界面和日志跑起来；当前仓库则更偏项目自己管理字体策略。

## 5. Linux 设备路径更收敛

`T113-S3` 配方的意图很明确：

- `LV_USE_LINUX_FBDEV=ON`
- `LV_USE_LINUX_INPUT=ON`

从语义上看，它就是把显示和输入收敛到设备侧 Linux 驱动路径上。

当前仓库则保留了多种宿主路线：

- `fb`
- `sdl`
- `drm`
- `wayland`

而在当前 `LVGL 9.5.0` 模板里，Linux 输入更接近：

- `LV_USE_EVDEV`
- `LV_USE_LIBINPUT`

所以这里的差异不是简单的“开关名不同”，而是：

- `T113-S3` 面向单一设备场景
- 当前仓库面向桌面调试和多宿主并存的公共场景

## 6. demo、example 和图片解码器的控制层级不同

在 `T113-S3` 配方里，这些能力更多由外层 Buildroot 包配置控制：

- `BR2_PACKAGE_LVGL_DEMOS`
- `BR2_PACKAGE_LVGL_EXAMPLES`
- `BR2_PACKAGE_LVGL_PNG`
- `BR2_PACKAGE_LVGL_JPEG`

当前仓库更像是：

- `lv_conf.h` 提供项目基线
- 构建脚本再通过 `CONFIG_LV_BUILD_DEMOS`、`CONFIG_LV_BUILD_EXAMPLES` 等开关去覆盖

这意味着：

- `T113-S3` 更适合 Buildroot/BSP 集成
- 当前仓库更适合项目源码级维护和跨平台统一控制

## 7. `lv_conf.h` 生命周期不同

`T113-S3` 的 `lv_conf.h` 是构建产物，不是长期维护的主文件。

当前仓库的 [lv_conf.h](d:/source/LVGLSharp/libs/lv_conf.h) 则是直接提交进仓库的版本化文件。

这两个思路各有优点：

- `T113-S3` 方便快速做板级变体
- 当前仓库方便代码审查、文档说明和长期统一维护

## 哪些地方其实又很像

虽然差异不小，但两边也有不少共同点：

- `LV_MEM_SIZE` 都是 `64KB`
- 都以软件渲染为主
- `LV_USE_FREETYPE` 都是关的
- `LV_USE_THORVG_INTERNAL` / `LV_USE_THORVG_EXTERNAL` 都是关的
- `LV_USE_SYSMON`、`LV_USE_PERF_MONITOR`、`LV_USE_MEM_MONITOR` 都是关的
- 在 Linux 动态库构建场景下，`BUILD_SHARED_LIBS=ON` 也是一致的

这说明两边共同的取向仍然是：

- 先把核心显示链路跑稳
- 不急着打开更重的图形库
- 不把监控功能作为默认基线

## 如果把 T113-S3 的思路迁回当前仓库，应该怎么理解

更稳妥的做法不是照抄，而是做语义迁移。

真正值得迁回来的主要是这些思路：

1. 如果目标是固定分辨率设备屏，可以评估是否把 `LV_COLOR_DEPTH` 提到 `32`。
2. 如果当前阶段重点是板上排障，可以临时打开 `LV_USE_LOG` 和 `LV_LOG_PRINTF`。
3. 如果需要稳定的兜底字体，可以考虑打开 `LV_FONT_MONTSERRAT_16`。
4. 如果是纯设备侧构建，可以把宿主路线收敛到 `fbdev + 输入驱动`。

但不建议直接照抄的地方也很明确：

1. 不要直接把 `LV_USE_LINUX_INPUT` 原样搬进当前仓库，先确认当前 LVGL 版本里的对应能力。
2. 不要把 Buildroot 包层变量和当前仓库 CMake 覆盖层混为一谈。
3. 不要忽略当前仓库已经长期维护的 [lv_conf.h](d:/source/LVGLSharp/libs/lv_conf.h)，否则会出现两套配置来源并存。

## 结语

`T113-S3` 这套编译参数更像设备板级配方，当前仓库的参数更像项目级公共基线。

前者的优点是：

- 对单板更直接
- 更适合 BSP / Buildroot 体系
- 更容易按板级快速裁剪

后者的优点是：

- 更容易统一维护
- 更适合跨平台和多宿主演进
- 更适合文档化和持续审查

真正值得比较的，不只是“开了哪些宏”，更是“配置权到底掌握在板级打包层，还是项目基线层”。

## 延伸阅读

- [LVGL 编译参数说明：从 lv_conf.h 到 CMake 开关](/zh/blog/lvgl-build-options.html)
- [全志 T113-S3 成功移植案例](/zh/news/allwinner-t113-s3-tina-linux-case.html)

## 附录：原始 `mk` 配方参考

下面这份内容就是前面分析时对应的全志 `T113-S3` 配方原文，放在文末方便对照。

```make
LVGL_VERSION = 9.5.0
LVGL_SITE = $(call github,lvgl,lvgl,v$(LVGL_VERSION))
LVGL_LICENSE = MIT
LVGL_LICENSE_FILES = LICENCE.txt
LVGL_INSTALL_STAGING = YES
LVGL_DEPENDENCIES = host-pkgconf

LVGL_CONF_OPTS = \
	-DLV_CONF_SKIP=ON \
	-DLV_KCONFIG_IGNORE=ON \
	-DBUILD_SHARED_LIBS=ON

LVGL_CONF_OPTS += \
	-DLV_USE_LINUX_FBDEV=ON \
	-DLV_USE_LINUX_INPUT=ON
	
ifeq ($(BR2_PACKAGE_LVGL_DEMOS),y)
LVGL_CONF_OPTS += -DLV_BUILD_EXAMPLES=ON
else
LVGL_CONF_OPTS += -DLV_BUILD_EXAMPLES=OFF
endif

ifeq ($(BR2_PACKAGE_LVGL_EXAMPLES),y)
LVGL_CONF_OPTS += -DLV_USE_EXAMPLES=ON
else
LVGL_CONF_OPTS += -DLV_USE_EXAMPLES=OFF
endif

ifeq ($(BR2_PACKAGE_LVGL_PNG),y)
LVGL_DEPENDENCIES += libpng
LVGL_CONF_OPTS += -DLV_USE_PNG=ON
else
LVGL_CONF_OPTS += -DLV_USE_PNG=OFF
endif

ifeq ($(BR2_PACKAGE_LVGL_JPEG),y)
LVGL_DEPENDENCIES += jpeg
LVGL_CONF_OPTS += -DLV_USE_JPEG=ON
else
LVGL_CONF_OPTS += -DLV_USE_JPEG=OFF
endif

define LVGL_CREATE_LV_CONF_H
	cp $(@D)/lv_conf_template.h $(@D)/lv_conf.h
	sed -i 's/#if 0/#if 1/' $(@D)/lv_conf.h
	sed -i 's/#define LV_COLOR_DEPTH 16/#define LV_COLOR_DEPTH 32/' $(@D)/lv_conf.h
	sed -i 's|#define LV_MEM_SIZE.*[0-9]*|#define LV_MEM_SIZE (64 * 1024U)|' $(@D)/lv_conf.h
	sed -i 's|#define LV_FONT_MONTSERRAT_16.*0|#define LV_FONT_MONTSERRAT_16 1|' $(@D)/lv_conf.h
	sed -i 's|#define LV_TICK_CUSTOM.*0|#define LV_TICK_CUSTOM 1|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_FREETYPE.*0|#define LV_USE_FREETYPE 0|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_LINUX_FBDEV.*0|#define LV_USE_LINUX_FBDEV 1|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_LINUX_INPUT.*0|#define LV_USE_LINUX_INPUT 1|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_DRAW_SW.*1|#define LV_USE_DRAW_SW 1|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_DRAW_ARM2D.*0|#define LV_USE_DRAW_ARM2D 0|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_THORVG_INTERNAL.*0|#define LV_USE_THORVG_INTERNAL 0|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_THORVG_EXTERNAL.*0|#define LV_USE_THORVG_EXTERNAL 0|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_NEMA_GFX.*0|#define LV_USE_NEMA_GFX 0|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_SYSMON.*0|#define LV_USE_SYSMON 0|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_PERF_MONITOR.*0|#define LV_USE_PERF_MONITOR 0|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_MEM_MONITOR.*0|#define LV_USE_MEM_MONITOR 0|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_LOG.*0|#define LV_USE_LOG 1|' $(@D)/lv_conf.h
	sed -i 's|#define LV_LOG_LEVEL LV_LOG_LEVEL_TRACE|#define LV_LOG_LEVEL LV_LOG_LEVEL_WARN|' $(@D)/lv_conf.h
	sed -i 's|#define LV_LOG_PRINTF.*0|#define LV_LOG_PRINTF 1|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_USER_DATA.*0|#define LV_USE_USER_DATA 1|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_ASSERT_NULL.*0|#define LV_USE_ASSERT_NULL 0|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_ASSERT_MALLOC.*0|#define LV_USE_ASSERT_MALLOC 0|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_ASSERT_STYLE.*0|#define LV_USE_ASSERT_STYLE 0|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_ASSERT_MEM_INTEGRITY.*0|#define LV_USE_ASSERT_MEM_INTEGRITY 0|' $(@D)/lv_conf.h
	sed -i 's|#define LV_USE_ASSERT_OBJ.*0|#define LV_USE_ASSERT_OBJ 0|' $(@D)/lv_conf.h
endef

LVGL_PRE_CONFIGURE_HOOKS += LVGL_CREATE_LV_CONF_H

$(eval $(cmake-package))
```

说明：

- 这份附录保留的是原始命名和原始 `Buildroot` 上下文。
- 如果要映射到当前仓库的 `LVGL 9.5.0` 模板，请以前文正文分析为准。
