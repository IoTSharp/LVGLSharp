#include "lvgl_host_x11.h"
#include "lvgl.h"

#include <X11/Xlib.h>
#include <X11/Xutil.h>

#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/time.h>
#include <unistd.h>

typedef struct host_internal {
    Display *display;
    int screen;
    Window window;
    GC gc;
    XImage *ximage;
    uint32_t *framebuffer;
    int width;
   int height;
    int mouse_x;
    int mouse_y;
    int mouse_pressed;
    lv_display_t *lv_display;
    lv_indev_t *mouse_indev;
    uint32_t *draw_buf_1;
    size_t draw_buf_pixels;
} host_internal_t;

static host_internal_t *g_host = NULL;

static uint64_t tick_ms(void)
{
    struct timeval tv;
    gettimeofday(&tv, NULL);
    return (uint64_t)tv.tv_sec * 1000ULL + (uint64_t)tv.tv_usec / 1000ULL;
}

static void flush_cb(lv_display_t *disp, const lv_area_t *area, uint8_t *px_map)
{
    (void)disp;

    if (!g_host || !g_host->framebuffer) {
        lv_display_flush_ready(disp);
        return;
    }

    int32_t w = lv_area_get_width(area);
    int32_t h = lv_area_get_height(area);

    for (int32_t y = 0; y < h; y++) {
        uint32_t *dst = g_host->framebuffer + (area->y1 + y) * g_host->width + area->x1;
        uint32_t *src = ((uint32_t *)px_map) + y * w;
        memcpy(dst, src, (size_t)w * sizeof(uint32_t));
    }

    lv_display_flush_ready(disp);
}

static void mouse_read_cb(lv_indev_t *indev, lv_indev_data_t *data)
{
    (void)indev;

    if (!g_host) {
        data->point.x = 0;
        data->point.y = 0;
        data->state = LV_INDEV_STATE_RELEASED;
        return;
    }

    data->point.x = g_host->mouse_x;
    data->point.y = g_host->mouse_y;
    data->state = g_host->mouse_pressed ? LV_INDEV_STATE_PRESSED : LV_INDEV_STATE_RELEASED;
}

int lvgl_host_x11_init(lvgl_host_x11_t *host, int width, int height, const char *title)
{
    if (!host || width <= 0 || height <= 0) {
        return -1;
    }

    memset(host, 0, sizeof(*host));

    host_internal_t *internal = (host_internal_t *)calloc(1, sizeof(host_internal_t));
    if (!internal) {
        return -1;
    }

    internal->width = width;
    internal->height = height;

    internal->display = XOpenDisplay(NULL);
    if (!internal->display) {
        free(internal);
        return -1;
    }

    internal->screen = DefaultScreen(internal->display);
    internal->window = XCreateSimpleWindow(
        internal->display,
        RootWindow(internal->display, internal->screen),
        10, 10,
        (unsigned int)width,
        (unsigned int)height,
        1,
        BlackPixel(internal->display, internal->screen),
        WhitePixel(internal->display, internal->screen));

    XStoreName(internal->display, internal->window, title ? title : "LVGL X11 Host");
    XSelectInput(internal->display, internal->window,
                 ExposureMask |
                 ButtonPressMask |
                 ButtonReleaseMask |
                 PointerMotionMask |
                 KeyPressMask |
                 StructureNotifyMask);
    XMapWindow(internal->display, internal->window);

    internal->gc = DefaultGC(internal->display, internal->screen);
    internal->framebuffer = (uint32_t *)calloc((size_t)width * (size_t)height, sizeof(uint32_t));
    if (!internal->framebuffer) {
        XDestroyWindow(internal->display, internal->window);
        XCloseDisplay(internal->display);
        free(internal);
        return -1;
    }

    Visual *visual = DefaultVisual(internal->display, internal->screen);
    int depth = DefaultDepth(internal->display, internal->screen);

    internal->ximage = XCreateImage(
        internal->display,
        visual,
        (unsigned int)depth,
        ZPixmap,
        0,
        (char *)internal->framebuffer,
        (unsigned int)width,
        (unsigned int)height,
        32,
        width * (int)sizeof(uint32_t));

    if (!internal->ximage) {
        free(internal->framebuffer);
        XDestroyWindow(internal->display, internal->window);
        XCloseDisplay(internal->display);
        free(internal);
        return -1;
    }

    internal->draw_buf_pixels = (size_t)width * 80U;
    internal->draw_buf_1 = (uint32_t *)malloc(internal->draw_buf_pixels * sizeof(uint32_t));
    if (!internal->draw_buf_1) {
        internal->ximage->data = NULL;
        XDestroyImage(internal->ximage);
        free(internal->framebuffer);
        XDestroyWindow(internal->display, internal->window);
        XCloseDisplay(internal->display);
        free(internal);
        return -1;
    }

    lv_init();

    internal->lv_display = lv_display_create(width, height);
    lv_display_set_buffers(
        internal->lv_display,
        internal->draw_buf_1,
        NULL,
        internal->draw_buf_pixels * sizeof(uint32_t),
        LV_DISPLAY_RENDER_MODE_PARTIAL);
    lv_display_set_flush_cb(internal->lv_display, flush_cb);

    internal->mouse_indev = lv_indev_create();
    lv_indev_set_type(internal->mouse_indev, LV_INDEV_TYPE_POINTER);
    lv_indev_set_read_cb(internal->mouse_indev, mouse_read_cb);

    host->width = width;
    host->height = height;
    host->running = 1;
    host->user_data = internal;

    g_host = internal;
    return 0;
}

void lvgl_host_x11_poll(lvgl_host_x11_t *host)
{
    if (!host || !host->user_data) {
        return;
    }

    host_internal_t *internal = (host_internal_t *)host->user_data;

    while (XPending(internal->display)) {
        XEvent ev;
        XNextEvent(internal->display, &ev);

        switch (ev.type) {
        case MotionNotify:
            internal->mouse_x = ev.xmotion.x;
            internal->mouse_y = ev.xmotion.y;
            break;
        case ButtonPress:
            if (ev.xbutton.button == Button1) {
                internal->mouse_pressed = 1;
                internal->mouse_x = ev.xbutton.x;
                internal->mouse_y = ev.xbutton.y;
            }
            break;
        case ButtonRelease:
            if (ev.xbutton.button == Button1) {
                internal->mouse_pressed = 0;
                internal->mouse_x = ev.xbutton.x;
                internal->mouse_y = ev.xbutton.y;
            }
            break;
        case DestroyNotify:
            host->running = 0;
            break;
        default:
            break;
        }
    }
}

void lvgl_host_x11_present(lvgl_host_x11_t *host)
{
    static uint64_t last_tick = 0;

    if (!host || !host->user_data) {
        return;
    }

    host_internal_t *internal = (host_internal_t *)host->user_data;

    uint64_t now = tick_ms();
    if (last_tick == 0) {
        last_tick = now;
    }

    uint32_t diff = (uint32_t)(now - last_tick);
    last_tick = now;

    lv_tick_inc(diff);
    lv_timer_handler();

    XPutImage(internal->display, internal->window, internal->gc, internal->ximage,
              0, 0, 0, 0, (unsigned int)host->width, (unsigned int)host->height);
    XFlush(internal->display);

    usleep(5000);
}

void lvgl_host_x11_shutdown(lvgl_host_x11_t *host)
{
    if (!host || !host->user_data) {
        return;
    }

    host_internal_t *internal = (host_internal_t *)host->user_data;

    if (internal->ximage) {
        internal->ximage->data = NULL;
        XDestroyImage(internal->ximage);
    }
    if (internal->framebuffer) {
        free(internal->framebuffer);
    }
    if (internal->draw_buf_1) {
        free(internal->draw_buf_1);
    }
    if (internal->window) {
        XDestroyWindow(internal->display, internal->window);
    }
    if (internal->display) {
        XCloseDisplay(internal->display);
    }

    if (g_host == internal) {
        g_host = NULL;
    }

    free(internal);
    host->user_data = NULL;
    host->running = 0;
}

int lvgl_host_x11_is_running(lvgl_host_x11_t *host)
{
    if (!host) {
        return 0;
    }
    return host->running;
}