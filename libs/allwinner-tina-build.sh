#!/usr/bin/env bash
set -euo pipefail

script_dir="$(cd "$(dirname "$0")" && pwd)"
default_lv_conf="${script_dir}/lv_conf_allwinner_tina.h"
default_lv_conf_template="${script_dir}/lvgl/lv_conf_template.h"

usage() {
    cat <<'EOF'
Usage:
  allwinner-tina-build.sh --lunch <target> [options]

Options:
  --sdk-dir <path>     Tina SDK root directory. Defaults to the current directory.
  --lunch <target>     Tina lunch target, for example d1-h_nezha-tina.
  --lv-conf <path>     Use an existing Allwinner-style lv_conf.h file.
  --lv-conf-target <path>
                       Copy the lv_conf.h file to this target before building.
  --generate-lv-conf-only
                       Generate the Allwinner lv_conf.h and exit.
  --regenerate-lv-conf Rebuild the generated Allwinner lv_conf.h from the template.
  -j, --jobs <count>   Parallel make jobs. Defaults to nproc.
  --clean              Run make clean before building.
  --no-pack            Build only and skip pack.
  --pack-arg <arg>     Extra argument passed to pack. Can be repeated.
  -h, --help           Show this help message.
EOF
}

sdk_dir="$(pwd)"
lunch_target=""
jobs="$(nproc)"
run_clean=0
run_pack=1
pack_args=()
lv_conf_file="${default_lv_conf}"
lv_conf_target=""
lv_conf_generate_only=0
lv_conf_regenerate=0
lv_conf_is_custom=0

generate_allwinner_lv_conf() {
    local template_path="${1:?missing template path}"
    local output_path="${2:?missing output path}"

    if [[ ! -f "${template_path}" ]]; then
        echo "Missing ${template_path}. Cannot generate the Allwinner lv_conf.h file." >&2
        exit 1
    fi

    cp "${template_path}" "${output_path}"

    sed -i \
        -e '0,/#if 0/s//#if 1/' \
        -e 's/#define LV_COLOR_DEPTH 16/#define LV_COLOR_DEPTH 32/' \
        -e 's|#define LV_MEM_SIZE.*|    #define LV_MEM_SIZE (64 * 1024U)          /**< [bytes] */|' \
        -e 's|#define LV_FONT_MONTSERRAT_16 0|#define LV_FONT_MONTSERRAT_16 1|' \
        -e 's|#define LV_USE_LINUX_FBDEV      0|#define LV_USE_LINUX_FBDEV      1|' \
        -e 's|#define LV_USE_EVDEV    0|#define LV_USE_EVDEV    1|' \
        -e 's|#define LV_USE_LOG 0|#define LV_USE_LOG 1|' \
        -e 's|#define LV_LOG_PRINTF 0|#define LV_LOG_PRINTF 1|' \
        -e 's|#define LV_USE_ASSERT_NULL          1|#define LV_USE_ASSERT_NULL          0|' \
        -e 's|#define LV_USE_ASSERT_MALLOC        1|#define LV_USE_ASSERT_MALLOC        0|' \
        -e 's|#define LV_USE_ASSERT_STYLE         0|#define LV_USE_ASSERT_STYLE         0|' \
        -e 's|#define LV_USE_ASSERT_MEM_INTEGRITY 0|#define LV_USE_ASSERT_MEM_INTEGRITY 0|' \
        -e 's|#define LV_USE_ASSERT_OBJ           0|#define LV_USE_ASSERT_OBJ           0|' \
        "${output_path}"
}

find_lv_conf_target() {
    local search_root="${1:?missing sdk root}"
    local -a candidates=()
    local candidate=""

    while IFS= read -r candidate; do
        candidates+=("${candidate}")
    done < <(find "${search_root}" -type f \( -name lv_conf.h -o -name lv_conf_template.h \) 2>/dev/null | grep -Ei 'lvgl')

    if [[ "${#candidates[@]}" -eq 1 ]]; then
        if [[ "$(basename "${candidates[0]}")" == "lv_conf_template.h" ]]; then
            printf '%s\n' "$(dirname "${candidates[0]}")/lv_conf.h"
        else
            printf '%s\n' "${candidates[0]}"
        fi
    fi
}

sync_lv_conf() {
    local source_path="${1:?missing source path}"
    local target_path="${2:?missing target path}"

    mkdir -p "$(dirname "${target_path}")"
    cp "${source_path}" "${target_path}"
    echo "Using Allwinner lv_conf.h: ${source_path} -> ${target_path}"
}

while [[ $# -gt 0 ]]; do
    case "$1" in
        --sdk-dir)
            sdk_dir="${2:-}"
            shift 2
            ;;
        --lunch)
            lunch_target="${2:-}"
            shift 2
            ;;
        --lv-conf)
            lv_conf_file="${2:-}"
            lv_conf_is_custom=1
            shift 2
            ;;
        --lv-conf-target)
            lv_conf_target="${2:-}"
            shift 2
            ;;
        --generate-lv-conf-only)
            lv_conf_generate_only=1
            shift
            ;;
        --regenerate-lv-conf)
            lv_conf_regenerate=1
            shift
            ;;
        -j|--jobs)
            jobs="${2:-}"
            shift 2
            ;;
        --clean)
            run_clean=1
            shift
            ;;
        --no-pack)
            run_pack=0
            shift
            ;;
        --pack-arg)
            pack_args+=("${2:-}")
            shift 2
            ;;
        -h|--help)
            usage
            exit 0
            ;;
        *)
            echo "Unknown argument: $1" >&2
            usage >&2
            exit 1
            ;;
    esac
done

if [[ "${lv_conf_is_custom}" == "0" && ( "${lv_conf_regenerate}" == "1" || ! -f "${lv_conf_file}" ) ]]; then
    generate_allwinner_lv_conf "${default_lv_conf_template}" "${lv_conf_file}"
fi

if [[ ! -f "${lv_conf_file}" ]]; then
    echo "Missing ${lv_conf_file}. Please provide --lv-conf or regenerate the default file." >&2
    exit 1
fi

if [[ "${lv_conf_generate_only}" == "1" ]]; then
    echo "Generated Allwinner lv_conf.h: ${lv_conf_file}"
    exit 0
fi

if [[ -z "${lunch_target}" ]]; then
    echo "--lunch is required." >&2
    usage >&2
    exit 1
fi

if [[ ! -f "${sdk_dir}/build/envsetup.sh" ]]; then
    echo "Missing ${sdk_dir}/build/envsetup.sh. Please run this script in the Tina SDK root or pass --sdk-dir." >&2
    exit 1
fi

if [[ -z "${lv_conf_target}" ]]; then
    lv_conf_target="$(find_lv_conf_target "${sdk_dir}" || true)"
fi

if [[ -z "${lv_conf_target}" ]]; then
    echo "Unable to determine where the Tina SDK expects lv_conf.h." >&2
    echo "Please pass --lv-conf-target <path> so the Allwinner lv_conf.h can be copied before build." >&2
    exit 1
fi

sync_lv_conf "${lv_conf_file}" "${lv_conf_target}"

cd "${sdk_dir}"

# shellcheck disable=SC1091
source build/envsetup.sh
lunch "${lunch_target}"

if [[ "${run_clean}" == "1" ]]; then
    make clean
fi

make -j"${jobs}"

if [[ "${run_pack}" == "1" ]]; then
    pack "${pack_args[@]}"
fi
