using System.Runtime.InteropServices;
using LVGLSharp.Interop;
using static LVGLSharp.Interop.LVGL;

namespace LVGLSharp.Forms
{
    public unsafe class ContextMenuStrip : IDisposable
    {
        private lv_obj_t* menuObj;
        private Control? owner;
        private readonly List<ToolStripMenuItem> items = new();
        private readonly List<GCHandle> _eventHandles = new();
        private GCHandle _screenClickHandle;
        private GCHandle _screenFocusHandle;
        private bool disposed;

        public ContextMenuStrip()
        {
        }

        public List<ToolStripMenuItem> Items => items;

        public void Show(Control owner, int x, int y)
        {
            this.owner = owner;
            
            // 如果已经有菜单显示，先关闭
            if (menuObj != null)
            {
                Close();
            }

            // 获取活动屏幕作为父对象（不使用 TextBox 作为父对象，避免被裁剪）
            var screen = lv_screen_active();
            if (screen == null) return;

            // 创建菜单容器
            menuObj = lv_obj_create(screen);
            if (menuObj == null) return;

            // 计算菜单尺寸
            int itemHeight = 35;
            int menuWidth = 180;
            int separatorHeight = 10;
            int totalHeight = 0;
            
            foreach (var item in items)
            {
                totalHeight += item.IsSeparator ? separatorHeight : itemHeight;
            }
            totalHeight += 10; // 上下内边距

            // 设置菜单容器尺寸和位置
            lv_obj_set_size(menuObj, menuWidth, totalHeight);
            lv_obj_set_pos(menuObj, x, y);
            
            // 设置为浮动对象，不受父对象布局影响
            lv_obj_add_flag(menuObj, lv_obj_flag_t.LV_OBJ_FLAG_FLOATING);
            
            // 移到最前面，确保在最上层显示
            lv_obj_move_foreground(menuObj);

            // 设置菜单容器样式 - 现代化扁平设计
            lv_obj_set_style_radius(menuObj, 8, 0); // 圆角
            lv_obj_set_style_bg_color(menuObj, lv_color_hex(0xFFFFFF), 0); // 白色背景
            lv_obj_set_style_bg_opa(menuObj, 255, 0);
            lv_obj_set_style_border_width(menuObj, 1, 0);
            lv_obj_set_style_border_color(menuObj, lv_color_hex(0xE0E0E0), 0);
            lv_obj_set_style_shadow_width(menuObj, 10, 0); // 阴影
            lv_obj_set_style_shadow_spread(menuObj, 2, 0);
            lv_obj_set_style_shadow_color(menuObj, lv_color_hex(0x000000), 0);
            lv_obj_set_style_shadow_opa(menuObj, 30, 0);
            lv_obj_set_style_pad_all(menuObj, 5, 0);

            // 移除滚动条
            lv_obj_clear_flag(menuObj, lv_obj_flag_t.LV_OBJ_FLAG_SCROLLABLE);

            // 让菜单可以获得焦点，以便监听失去焦点事件
            lv_obj_add_flag(menuObj, lv_obj_flag_t.LV_OBJ_FLAG_CLICKABLE);
            lv_obj_clear_flag(menuObj, lv_obj_flag_t.LV_OBJ_FLAG_CLICK_FOCUSABLE);
            
            // 添加菜单失去焦点时关闭的监听器
            var menuFocusHandle = GCHandle.Alloc(this);
            _eventHandles.Add(menuFocusHandle);
            lv_obj_add_event_cb(menuObj, &MenuDefocusCallback, lv_event_code_t.LV_EVENT_DEFOCUSED, 
                (void*)GCHandle.ToIntPtr(menuFocusHandle));

            // 创建菜单项
            int currentY = 5;
            foreach (var item in items)
            {
                if (item.IsSeparator)
                {
                    // 创建分隔线
                    var separator = lv_obj_create(menuObj);
                    lv_obj_set_size(separator, menuWidth - 20, 2);
                    lv_obj_set_pos(separator, 10, currentY + 4);
                    lv_obj_set_style_bg_color(separator, lv_color_hex(0xE0E0E0), 0);
                    lv_obj_set_style_border_width(separator, 0, 0);
                    lv_obj_set_style_radius(separator, 0, 0);
                    lv_obj_clear_flag(separator, lv_obj_flag_t.LV_OBJ_FLAG_SCROLLABLE);
                    currentY += separatorHeight;
                    continue;
                }

                // 创建菜单项按钮
                var btn = lv_button_create(menuObj);
                lv_obj_set_size(btn, menuWidth - 10, itemHeight - 5);
                lv_obj_set_pos(btn, 5, currentY);
                
                // 设置按钮样式
                lv_obj_set_style_radius(btn, 4, 0);
                lv_obj_set_style_bg_color(btn, lv_color_hex(0xFFFFFF), 0);
                lv_obj_set_style_bg_color(btn, lv_color_hex(0xE3F2FD), 0x0020); // LV_STATE_PRESSED
                lv_obj_set_style_border_width(btn, 0, 0);
                lv_obj_set_style_shadow_width(btn, 0, 0);

                // 创建标签显示文本和快捷键
                var label = lv_label_create(btn);
                var btnText = item.Text;
                if (item.ShowShortcutKeys && item.ShortcutKeys != Keys.None)
                {
                    btnText += $"\t{item.GetShortcutKeyText()}";
                }

                var textBytes = Control.ToUtf8(btnText);
                fixed (byte* textPtr = textBytes)
                {
                    lv_label_set_text(label, textPtr);
                }
                
                // 设置明确的文字颜色 - 黑色，确保可见性
                lv_obj_set_style_text_color(label, lv_color_hex(0x000000), 0);
                lv_obj_set_style_text_align(label, lv_text_align_t.LV_TEXT_ALIGN_LEFT, 0);
                lv_obj_align(label, lv_align_t.LV_ALIGN_LEFT_MID, 10, 0);
                
                if (!item.Enabled)
                {
                    lv_obj_add_state(btn, 0x0080); // LV_STATE_DISABLED
                    lv_obj_set_style_text_color(label, lv_color_hex(0x999999), 0); // 禁用时使用深灰色
                }

                // 绑定点击事件
                var handle = GCHandle.Alloc((item, this));
                _eventHandles.Add(handle);
                lv_obj_add_event_cb(btn, &MenuItemClickCallback, lv_event_code_t.LV_EVENT_CLICKED, 
                    (void*)GCHandle.ToIntPtr(handle));

                currentY += itemHeight;
            }

            // 添加点击外部关闭的事件监听器（点击屏幕其他地方关闭菜单）
            var closeHandle = GCHandle.Alloc(this);
            _screenClickHandle = closeHandle;
            lv_obj_add_event_cb(screen, &ScreenClickCallback, lv_event_code_t.LV_EVENT_CLICKED, 
                (void*)GCHandle.ToIntPtr(closeHandle));

            // 添加焦点变化监听器（当其他控件获得焦点时关闭菜单）
            var focusHandle = GCHandle.Alloc(this);
            _screenFocusHandle = focusHandle;
            lv_obj_add_event_cb(screen, &ScreenFocusCallback, lv_event_code_t.LV_EVENT_FOCUSED, 
                (void*)GCHandle.ToIntPtr(focusHandle));
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        private static void MenuItemClickCallback(lv_event_t* e)
        {
            try
            {
                var userData = lv_event_get_user_data(e);
                if (userData == null) return;

                var handle = GCHandle.FromIntPtr((nint)userData);
                var tuple = ((ToolStripMenuItem, ContextMenuStrip))handle.Target!;
                var item = tuple.Item1;
                var menu = tuple.Item2;

                // 触发菜单项的点击事件
                item.PerformClick();

                // 关闭菜单
                menu.Close();
            }
            catch
            {
                // 忽略异常
            }
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        private static void ScreenClickCallback(lv_event_t* e)
        {
            try
            {
                var userData = lv_event_get_user_data(e);
                if (userData == null) return;

                var handle = GCHandle.FromIntPtr((nint)userData);
                var menu = (ContextMenuStrip)handle.Target!;

                // 检查点击的目标是否在菜单内
                var target = lv_event_get_target_obj(e);
                if (target != menu.menuObj && !lv_obj_is_child_of(target, menu.menuObj))
                {
                    menu.Close();
                }
            }
            catch
            {
                // 忽略异常
            }
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        private static void ScreenFocusCallback(lv_event_t* e)
        {
            try
            {
                var userData = lv_event_get_user_data(e);
                if (userData == null) return;

                var handle = GCHandle.FromIntPtr((nint)userData);
                var menu = (ContextMenuStrip)handle.Target!;

                // 检查获得焦点的目标是否是菜单或菜单内的控件
                var target = lv_event_get_target_obj(e);
                if (target != menu.menuObj && !lv_obj_is_child_of(target, menu.menuObj))
                {
                    // 其他控件获得焦点，关闭菜单
                    menu.Close();
                }
            }
            catch
            {
                // 忽略异常
            }
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        private static void MenuDefocusCallback(lv_event_t* e)
        {
            try
            {
                var userData = lv_event_get_user_data(e);
                if (userData == null) return;

                var handle = GCHandle.FromIntPtr((nint)userData);
                var menu = (ContextMenuStrip)handle.Target!;

                // 菜单失去焦点，关闭菜单
                menu.Close();
            }
            catch
            {
                // 忽略异常
            }
        }

        public void Close()
        {
            if (menuObj != null)
            {
                // 移除屏幕点击事件
                if (_screenClickHandle.IsAllocated)
                {
                    var screen = lv_screen_active();
                    if
