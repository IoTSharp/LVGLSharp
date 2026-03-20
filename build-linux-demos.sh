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

Builds the Linux X11 native dependencies and publishes the demo apps as
self-contained Native AOT single-file executables.

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

If no demo is specified, all demos are published.
EOF
}

fail() {
    printf 'error: %s
' "$*" >&2
    exit 1
}

require_cmd() {
    command -v "$1" >/dev/null 2>&1 || fail "missing required command: $1"
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
    DEMO_NAMES=(SerialPort WinFormsDemo PictureBoxDemo)
fi

require_cmd cmake
require_cmd dotnet
require_cmd cp
require_cmd ln
require_cmd rm
require_cmd chmod

LVGL_SOURCE_DIR="$ROOT_DIR/libs/lvgl"
LVGL_BUILD_DIR="$ROOT_DIR/libs/build/lvgl-x11"
LVGL_LIB_DIR="$LVGL_BUILD_DIR/lib"
LVGL_SONAME_PATH="$LVGL_LIB_DIR/liblvgl.so.9"
HOST_SOURCE_DIR="$ROOT_DIR/libs/lvgl_host_x11"
HOST_BUILD_DIR="$HOST_SOURCE_DIR/build"
HOST_LIB_PATH="$HOST_BUILD_DIR/liblvgl_host_x11.so"
FONT_PATH="$ROOT_DIR/src/LVGLSharp.Runtime.Linux/NotoSansSC-Regular.ttf"
DIST_DIR="$ROOT_DIR/dist/$RID"

if ((CLEAN)); then
    rm -rf "$DIST_DIR" "$LVGL_BUILD_DIR" "$HOST_BUILD_DIR"
fi

mkdir -p "$DIST_DIR"

printf '==> Building LVGL shared library (%s)
' "$RID"
cmake -S "$LVGL_SOURCE_DIR" -B "$LVGL_BUILD_DIR"     -DCMAKE_BUILD_TYPE="$CONFIGURATION"     -DBUILD_SHARED_LIBS=ON     -DCONFIG_LV_BUILD_EXAMPLES=OFF     -DCONFIG_LV_BUILD_DEMOS=OFF     -DCONFIG_LV_USE_LINUX_FBDEV=OFF     -DCONFIG_LV_USE_SDL=OFF     -DCONFIG_LV_USE_LINUX_DRM=OFF     -DCONFIG_LV_USE_WAYLAND=OFF     -DLV_BUILD_CONF_DIR="$ROOT_DIR/libs"
cmake --build "$LVGL_BUILD_DIR" -j"$JOBS"

[[ -f "$LVGL_SONAME_PATH" ]] || fail "missing built LVGL shared library: $LVGL_SONAME_PATH"

printf '==> Building lvgl_host_x11
'
cmake -S "$HOST_SOURCE_DIR" -B "$HOST_BUILD_DIR"     -DCMAKE_BUILD_TYPE="$CONFIGURATION"     -DLVGL_BUNDLED_DIR="$LVGL_SOURCE_DIR"     -DLVGL_BUNDLED_LIB="$LVGL_SONAME_PATH"
cmake --build "$HOST_BUILD_DIR" -j"$JOBS"

[[ -f "$HOST_LIB_PATH" ]] || fail "missing built X11 host library: $HOST_LIB_PATH"
[[ -f "$FONT_PATH" ]] || fail "missing Linux runtime font: $FONT_PATH"

publish_demo() {
    local demo_name="$1"
    local project_path="$ROOT_DIR/src/Demos/$demo_name/$demo_name.csproj"
    local publish_dir="$DIST_DIR/$demo_name"
    local executable_path="$publish_dir/$demo_name"

    [[ -f "$project_path" ]] || fail "missing demo project: $project_path"

    printf '==> Publishing %s
' "$demo_name"
    rm -rf "$publish_dir"

    dotnet publish "$project_path"         -f net10.0         -c "$CONFIGURATION"         -r "$RID"         -o "$publish_dir"         -p:EnableWindowsTargeting=true

    [[ -f "$executable_path" ]] || fail "missing published executable: $executable_path"

    cp -Lf "$HOST_LIB_PATH" "$publish_dir/liblvgl_host_x11.so"
    cp -Lf "$LVGL_SONAME_PATH" "$publish_dir/liblvgl.so.9"
    ln -sfn liblvgl.so.9 "$publish_dir/liblvgl.so"
    cp -f "$FONT_PATH" "$publish_dir/NotoSansSC-Regular.ttf"

    rm -f "$publish_dir"/*.pdb "$publish_dir"/*.dbg
    chmod +x "$executable_path"

    printf '    output: %s
' "$publish_dir"
}

for demo_name in "${DEMO_NAMES[@]}"; do
    publish_demo "$demo_name"
done

printf '==> Done
'
printf 'Published demos under %s
' "$DIST_DIR"
