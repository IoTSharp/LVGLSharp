---
title: 全志 T113-S3 的 LVGL 编译参数说明：和当前仓库有何不同
description: 结合 T113-S3 上的 Buildroot 配方，说明它的 lv_conf.h 生成方式，以及与当前 LVGLSharp 仓库编译参数体系的主要差异。
lang: zh-CN
---

# 全志 T113-S3 的 LVGL 编译参数说明：和当前仓库有何不同

> 如果你正在看设备侧落地，尤其是 `T113-S3 + Tina Linux` 这类板级环境，这篇文章会把那份配方是怎么工作的、它和我们当前仓库的编译参数体系差在哪里讲清楚。

## 先看结论

这份 `T113-S3` 配方和我们当前仓库最大的不同，不是某一个宏开了还是关了，而是配置来源完全不同：

- `T113-S3` 方案是“构建前动态生成一份板级 `lv_conf.h`”
- 我们当前仓库是“仓库里维护一份固定的 `libs/lv_conf.h`，再用少量 CMake 参数覆盖”

所以它们的差异，不只是参数值不同，更是“谁负责生成最终配置”这件事不同。

## 这份 T113-S3 配方是怎么工作的

你给出的配置本质上是一份 Buildroot 风格的 `cmake-package` 配方。

它主要做了三件事：

1. 先声明构建依赖和 CMake 选项。
2. 在 `pre-configure` 阶段复制 `lv_conf_template.h` 生成 `lv_conf.h`。
3. 再用一串 `sed` 把这份 `lv_conf.h` 改成适合 `T113-S3` 板级环境的版本。

它的核心路径可以概括成这样：

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

也就是说，它不是把一份已经维护好的 `lv_conf.h` 直接提交进仓库长期复用，而是每次构建时临时产出一份板级配置。

这种方式对 BSP 或 SDK 来说很常见，因为：

- 不同板子往往只差几项关键宏
- 用模板加 `sed` 可以快速做出“板级变种”
- 上层包配置如 `BR2_PACKAGE_*` 也容易接进来

## 它和我们当前仓库最核心的结构差异

我们当前仓库的编译参数体系更像这样：

- 基线配置写在 `libs/lv_conf.h`
- Linux / Windows 构建脚本只覆盖少量 `CONFIG_LV_*` 项
- 配置文件本身是版本化的、可直接审阅的

例如当前 Linux 脚本主要传的是：

- `CONFIG_LV_BUILD_EXAMPLES=OFF`
- `CONFIG_LV_BUILD_DEMOS=OFF`
- `CONFIG_LV_USE_LINUX_FBDEV=ON/OFF`
- `CONFIG_LV_USE_SDL=ON/OFF`
- `CONFIG_LV_USE_LINUX_DRM=ON/OFF`
- `CONFIG_LV_USE_WAYLAND=ON/OFF`

而 `T113-S3` 这份配置更像：

- 先从模板生成
- 再用板级脚本改值
- 再由 Buildroot 包选项决定是否启用例子、图片解码器等

这会带来两个直接结果：

- `T113-S3` 的配置更偏“板级定制”
- 我们当前仓库的配置更偏“项目级基线”

## 一个需要先说明的点：两边的参数名字并不总能一一对上

从我们当前仓库集成的 `LVGL 9.5.0` 子模块源码来看，主线 CMake 选项明确更偏向 `CONFIG_LV_*` 命名。

而你给的 `T113-S3` 配方里出现了一些名字，比如：

- `LV_USE_LINUX_INPUT`
- `LV_USE_PNG`
- `LV_USE_JPEG`
- `LV_USE_EXAMPLES`
- `LV_USE_USER_DATA`

这些名字并没有直接出现在我们当前子模块的 `lv_conf_template.h` 里。

这说明一件事：

这份 `T113-S3` 配方更适合被理解为“板级打包层的参数体系”，而不是和我们仓库当前 `lv_conf.h` 可以逐字逐项对齐的同一层配置。

所以后面的对比，我会按“功能语义”来对齐，而不是机械地按名字对齐。

## 和我们当前仓库相比，最明显的几个不同

## 1. 颜色深度不同：T113-S3 用 32 位，我们当前基线是 16 位

`T113-S3` 里显式把：

- `LV_COLOR_DEPTH 16`

改成了：

- `LV_COLOR_DEPTH 32`

而我们当前仓库的 `libs/lv_conf.h` 还是：

- `LV_COLOR_DEPTH 16`

这背后的取舍很明确：

- `T113-S3` 更偏显示效果和通用桌面式颜色路径
- 我们当前仓库更偏控制内存和兼容更轻量的设备环境

对于 `1024x600` 这种分辨率的设备，`32-bit` 配置通常会让颜色和混合路径更直接，但显存和缓冲区压力也会更高。

## 2. 日志策略相反：T113-S3 默认开日志，我们当前默认关日志

`T113-S3` 里明确打开了：

- `LV_USE_LOG 1`
- `LV_LOG_PRINTF 1`
- `LV_LOG_LEVEL LV_LOG_LEVEL_WARN`

我们当前仓库则是：

- `LV_USE_LOG 0`
- `LV_LOG_PRINTF 0`

这说明 `T113-S3` 配方更强调板上排障体验。

这很合理，因为在真机上调触摸、帧缓冲、输入事件、字体和初始化路径时，日志往往比纯粹追求最小开销更重要。

而我们当前仓库更偏“先保持库本身安静”，需要排障时再按场景打开日志。

## 3. 断言策略也相反：T113-S3 全关，我们当前保留基础断言

`T113-S3` 配方把这些都关掉了：

- `LV_USE_ASSERT_NULL 0`
- `LV_USE_ASSERT_MALLOC 0`
- `LV_USE_ASSERT_STYLE 0`
- `LV_USE_ASSERT_MEM_INTEGRITY 0`
- `LV_USE_ASSERT_OBJ 0`

我们当前仓库则保留了最基础的两项：

- `LV_USE_ASSERT_NULL 1`
- `LV_USE_ASSERT_MALLOC 1`

这两种思路代表的是不同阶段的优化重点：

- `T113-S3` 更偏最终设备运行时的开销控制
- 我们当前仓库更偏保留最基础的安全网，方便开发阶段尽早暴露问题

## 4. 字体基线不同：T113-S3 打开了内置 `Montserrat 16`

`T113-S3` 配方显式把：

- `LV_FONT_MONTSERRAT_16 1`

打开了。

我们当前仓库对应位置仍然是：

- `LV_FONT_MONTSERRAT_16 0`

这意味着 `T113-S3` 配方更愿意保留一个稳定的内置位图字体基线，方便板上先把界面和日志跑通。

而我们当前仓库没有把这个默认打开，整体更偏“按项目自己的字体路线处理”，不把内置字体作为默认强依赖。

## 5. Linux 设备路径更收敛：T113-S3 锁定 Framebuffer，当前仓库保留多宿主切换

`T113-S3` 配方的意图很明确：

- `LV_USE_LINUX_FBDEV=ON`
- `LV_USE_LINUX_INPUT=ON`

从功能语义上看，它就是把显示和输入固定在设备侧 Linux 驱动路径上。

而我们当前仓库的策略不是只服务一块板子，而是同时保留多种宿主路线：

- `fb`
- `sdl`
- `drm`
- `wayland`

并且在当前 `LVGL 9.5.0` 子模块里，Linux 输入驱动主线上更直接看到的是：

- `LV_USE_EVDEV`
- `LV_USE_LIBINPUT`

所以如果把两边放在一起看，差异不只是“某个开关叫法不同”，而是：

- `T113-S3` 是面向单一设备环境的收敛配置
- 我们当前仓库是面向桌面、调试和设备侧多场景的通用配置

## 6. 例子、图片解码器这些能力，T113-S3 更依赖外层包管理开关

你给的 `T113-S3` 配方里，这些能力不是直接在仓库固定死的，而是受 `BR2_PACKAGE_*` 控制：

- `BR2_PACKAGE_LVGL_DEMOS`
- `BR2_PACKAGE_LVGL_EXAMPLES`
- `BR2_PACKAGE_LVGL_PNG`
- `BR2_PACKAGE_LVGL_JPEG`

这和我们当前仓库很不一样。

我们当前仓库更像是：

- `lv_conf.h` 提供一份项目基线
- 构建脚本再通过 `CONFIG_LV_BUILD_DEMOS`、`CONFIG_LV_BUILD_EXAMPLES` 这类选项做覆盖

而 `T113-S3` 方案里，是否启用这些能力更明显受发行版/BSP 包配置驱动。

这使得它更适合进 Buildroot 菜单体系，但也意味着可读性更多分散在外层包定义里。

## 7. `lv_conf.h` 生命周期不同：T113-S3 是临时生成，我们当前是仓库持久维护

`T113-S3` 的 `lv_conf.h` 是构建产物，不是长期维护的主文件。

我们当前仓库的 `libs/lv_conf.h` 则是直接提交进仓库的版本化文件。

两者区别非常实际：

- `T113-S3` 方便快速做板级变体
- 当前仓库方便代码审查、文档说明和跨平台统一维护

如果后面还会继续做更多设备移植，这其实是一个重要选择：

- 是每块板子都有一份生成脚本
- 还是尽量收敛到一份可复用的项目级配置

## 哪些地方其实又很像

虽然差异不少，但两边并不是两套完全相反的路线。

有几项其实很接近：

- `LV_MEM_SIZE` 都是 `64KB`
- 都保留了软件渲染为主的思路
- `LV_USE_FREETYPE` 都是关的
- `LV_USE_THORVG_INTERNAL` / `EXTERNAL` 都是关的
- `LV_USE_SYSMON`、`LV_USE_PERF_MONITOR`、`LV_USE_MEM_MONITOR` 都是关的
- 在 Linux 构建语境下，`BUILD_SHARED_LIBS=ON` 也是一致的

这说明两边的共同点是：

- 先把核心显示链路跑稳
- 不急着上更重的图形库
- 不把高级监控能力作为默认配置

## 为什么 T113-S3 会更适合这样的参数组合

如果把场景放回 `T113-S3 + Tina Linux`，这套配置其实很有针对性：

- 设备就是固定屏幕和固定输入，不需要桌面宿主切换
- 板上调试更需要日志，而不是极致安静
- 打开一个稳定的内置字体，更利于先把界面跑起来
- 关闭更多断言和监控项，更偏向最终设备运行时

也就是说，它不是一套“通用最优参数”，而是一套“这块板子上更实用的参数”。

## 如果要把 T113-S3 的思路迁回我们当前仓库，应该怎么理解

更稳的方式不是逐字照抄，而是做语义迁移。

真正值得迁回来的主要是这几类思路：

1. 如果目标是固定分辨率设备屏，可以评估是否把 `LV_COLOR_DEPTH` 提到 `32`。
2. 如果当前阶段重点是板上排障，可以临时打开 `LV_USE_LOG` 和 `LV_LOG_PRINTF`。
3. 如果需要一个稳定的兜底字体，可以考虑打开 `LV_FONT_MONTSERRAT_16` 或等价字体基线。
4. 如果是纯设备侧构建，可以把后端收敛到 `fbdev + 输入驱动`，而不是同时保留多种桌面宿主。

但不建议直接照抄的地方也很明确：

1. 不要直接把 `LV_USE_LINUX_INPUT` 这类名字原封不动搬进我们当前仓库，先确认它在当前子模块里的对应能力。
2. 不要把 Buildroot 包层变量和我们当前 CMake 覆盖层混为一谈。
3. 不要忽略我们当前仓库已经有一份长期维护的 `libs/lv_conf.h`，否则后面会出现两套配置来源并存。

## 结语

`T113-S3` 这份编译参数更像一套设备板级配方，而我们当前仓库的参数更像一套项目级通用基线。

前者的优点是：

- 对单一板子更直接
- 更适合 BSP / Buildroot 体系
- 更容易快速按板子裁剪

后者的优点是：

- 更容易统一维护
- 更适合跨平台和多宿主演进
- 更适合文档化和持续审查

所以真正值得拿来比较的，不只是“开了哪些宏”，而是“配置权到底掌握在板级配方层，还是项目基线层”。

## 延伸阅读

- [LVGL 编译参数说明：从 lv_conf.h 到 CMake 开关](/zh/blog/lvgl-build-options.html)
- [全志 T113-S3 成功移植案例](/zh/news/allwinner-t113-s3-tina-linux-case.html)
