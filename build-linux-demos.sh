#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
CONFIGURATION="Release"
RID="linux-x64"
CLEAN=0
JOBS="${JOBS:-$(nproc)}"

usage() {
    cat <<'EOF'
Usage: ./build-linux-demos.sh [options] [demo...]

Builds the Linux LVGL shared library and publishes the demo apps as
self-contained single-file executables.

Options:
  --clean                 Remove previous build/publish output first.
  --configuration <cfg>   Build configuration. Default: Release
  --rid <rid>             Runtime identifier. Default: linux-x64
  -j, --jobs <count>      Parallel build jobs. Default: nproc
  -h, --help              Show this help.

Demos:
  SerialPort
  WinFormsDemo
  PictureBoxDemo
  MusicDemo
  SmartWatchDemo

If no demo is specified, all demos are published.
EOF
}

get_demo_target_framework() {
    case "$1" in
        SerialPort|WinFormsDemo|PictureBoxDemo|MusicDemo|SmartWatchDemo)
            printf 'net10.0'
            ;;
        *)
            fail "no target framework mapping for demo: $1"
            ;;
    esac
}

get_demo_project_path() {
    case "$1" in
        MusicDemo)
            printf '%s' "$ROOT_DIR/src/Demos/MusicDemo/MusicDemo.csproj"
            ;;
        *)
            printf '%s' "$ROOT_DIR/src/Demos/$1/$1.csproj"
            ;;
    esac
}

fail() {
    printf 'error: %s\n' "$*" >&2
    exit 1
}

get_cmd_path_or_null() {
    command -v "$1" 2>/dev/null || true
}

require_cmd() {
    local name="$1"
    local install_hint="${2:-}"
    local command_path

    command_path="$(get_cmd_path_or_null "$name")"
    if [[ -z "$command_path" ]]; then
        local message="缺少必需命令: $name"
        if [[ -n "$install_hint" ]]; then
            message+=$'\n安装建议: '
            message+="$install_hint"
        fi

        message+=$'\n最小依赖: .NET SDK 10、CMake、Ninja、基础 GNU 文件工具(cp/find/ln/rm/chmod)。'
        fail "$message"
    fi

    printf '%s' "$command_path"
}

show_minimal_requirements() {
    printf '==> 最小安装清单\n'
    printf '    1. .NET SDK 10.x\n'
    printf '    2. CMake\n'
    printf '    3. Ninja\n'
    printf '    4. 基础 GNU 文件工具: cp / find / ln / rm / chmod\n'
    printf '    5. Linux 构建环境（Ubuntu/Debian 可直接 apt 安装上述依赖）\n'
}

assert_build_prerequisites() {
    local cmake_path dotnet_path ninja_path

    cmake_path="$(require_cmd cmake '安装 CMake，并确保 cmake 在 PATH 中。')"
    dotnet_path="$(require_cmd dotnet '安装 .NET SDK 10，并确保 dotnet 在 PATH 中。')"
    ninja_path="$(require_cmd ninja '安装 ninja-build，并确保 ninja 在 PATH 中。')"
    require_cmd cp '安装 coreutils。' >/dev/null
    require_cmd find '安装 findutils。' >/dev/null
    require_cmd ln '安装 coreutils。' >/dev/null
    require_cmd rm '安装 coreutils。' >/dev/null
    require_cmd chmod '安装 coreutils。' >/dev/null

    printf '==> 检查构建依赖\n'
    printf '    dotnet: %s\n' "$dotnet_path"
    printf '    cmake : %s\n' "$cmake_path"
    printf '    ninja : %s\n' "$ninja_path"
}

normalize_demo() {
    case "$1" in
        serialport|SerialPort)
            printf 'SerialPort'
            ;;
        winformsdemo|WinFormsDemo)
            printf 'WinFormsDemo'
            ;;
        pictureboxdemo|PictureBoxDemo)
            printf 'PictureBoxDemo'
            ;;
        musicdemo|MusicDemo)
            printf 'MusicDemo'
            ;;
        smartwatchdemo|SmartWatchDemo)
            printf 'SmartWatchDemo'
            ;;
        *)
            return 1
            ;;
    esac
}

DEMO_NAMES=()
while (($# > 0)); do
    case "$1" in
        --clean)
            CLEAN=1
            ;;
        --configuration)
            shift
            (($# > 0)) || fail "--configuration requires a value"
            CONFIGURATION="$1"
            ;;
        --rid)
            shift
            (($# > 0)) || fail "--rid requires a value"
            RID="$1"
            ;;
        -j|--jobs)
            shift
            (($# > 0)) || fail "--jobs requires a value"
            JOBS="$1"
            ;;
        -h|--help)
            usage
            exit 0
            ;;
        *)
            demo_name="$(normalize_demo "$1")" || fail "unknown demo: $1"
            DEMO_NAMES+=("$demo_name")
            ;;
    esac
    shift
done

if ((${#DEMO_NAMES[@]} == 0)); then
    DEMO_NAMES=(SerialPort WinFormsDemo PictureBoxDemo MusicDemo SmartWatchDemo)
fi

show_minimal_requirements
assert_build_prerequisites

LVGL_SOURCE_DIR="$ROOT_DIR/libs/lvgl"
LVGL_BUILD_DIR="$ROOT_DIR/libs/build/lvgl-x11"
LVGL_LIB_DIR="$LVGL_BUILD_DIR/lib"
LVGL_SONAME_PATH="$LVGL_LIB_DIR/liblvgl.so.9"
DIST_DIR="$ROOT_DIR/dist/$RID"
NUGET_CONFIG_PATH="$ROOT_DIR/NuGet.Wsl.Config"

NUGET_CONFIG_OPTION=()
if [[ -f "$NUGET_CONFIG_PATH" ]]; then
    NUGET_CONFIG_OPTION=(--configfile "$NUGET_CONFIG_PATH")
fi

LINUX_NUGET_PACKAGES="${HOME}/.nuget/packages"
export NUGET_PACKAGES="$LINUX_NUGET_PACKAGES"
export NUGET_FALLBACK_PACKAGES=""

if ((CLEAN)); then
    rm -rf "$DIST_DIR" "$LVGL_BUILD_DIR"
fi

mkdir -p "$DIST_DIR"

if ((${#NUGET_CONFIG_OPTION[@]} > 0)); then
    find "$ROOT_DIR/src" -type d \( -name obj -o -name bin \) -prune -exec rm -rf {} +
fi

printf '==> Building LVGL shared library (%s)\n' "$RID"
cmake -S "$LVGL_SOURCE_DIR" -B "$LVGL_BUILD_DIR" \
    -G Ninja \
    -DCMAKE_BUILD_TYPE="$CONFIGURATION" \
    -DBUILD_SHARED_LIBS=ON \
    -DCONFIG_LV_BUILD_EXAMPLES=OFF \
    -DCONFIG_LV_BUILD_DEMOS=OFF \
    -DCONFIG_LV_USE_LINUX_FBDEV=OFF \
    -DCONFIG_LV_USE_SDL=OFF \
    -DCONFIG_LV_USE_LINUX_DRM=OFF \
    -DCONFIG_LV_USE_WAYLAND=OFF \
    -DLV_BUILD_CONF_DIR="$ROOT_DIR/libs"
cmake --build "$LVGL_BUILD_DIR" -j"$JOBS"

[[ -f "$LVGL_SONAME_PATH" ]] || fail "missing built LVGL shared library: $LVGL_SONAME_PATH"

publish_demo() {
    local demo_name="$1"
    local project_path
    local publish_dir="$DIST_DIR/$demo_name"
    local executable_path="$publish_dir/$demo_name"
    local target_framework

    target_framework="$(get_demo_target_framework "$demo_name")"
    project_path="$(get_demo_project_path "$demo_name")"

    [[ -f "$project_path" ]] || fail "missing demo project: $project_path"

    printf '==> Publishing %s (%s)\n' "$demo_name" "$target_framework"
    rm -rf "$publish_dir"

    dotnet restore "$project_path" \
        -r "$RID" \
        --force-evaluate \
        "${NUGET_CONFIG_OPTION[@]}" \
        -p:EnableWindowsTargeting=true \
        -p:RestorePackagesPath="$LINUX_NUGET_PACKAGES" \
        -p:NuGetPackageFolders="$LINUX_NUGET_PACKAGES" \
        -p:RestoreFallbackFolders=

    dotnet publish "$project_path" \
        -f "$target_framework" \
        -c "$CONFIGURATION" \
        -r "$RID" \
        -o "$publish_dir" \
        --no-restore \
        "${NUGET_CONFIG_OPTION[@]}" \
        -p:EnableWindowsTargeting=true \
        -p:RestorePackagesPath="$LINUX_NUGET_PACKAGES" \
        -p:NuGetPackageFolders="$LINUX_NUGET_PACKAGES" \
        -p:RestoreFallbackFolders=

    [[ -f "$executable_path" ]] || fail "missing published executable: $executable_path"

    cp -Lf "$LVGL_SONAME_PATH" "$publish_dir/liblvgl.so.9"
    ln -sfn liblvgl.so.9 "$publish_dir/liblvgl.so"

    rm -f "$publish_dir"/*.pdb "$publish_dir"/*.dbg
    chmod +x "$executable_path"

    printf '    output: %s\n' "$publish_dir"
}

for demo_name in "${DEMO_NAMES[@]}"; do
    publish_demo "$demo_name"
done

printf '==> Done\n'
printf 'Published demos under %s\n' "$DIST_DIR"
