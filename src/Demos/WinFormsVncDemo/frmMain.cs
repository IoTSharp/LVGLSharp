using System;

namespace WinFormsVncDemo
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            // 可选：在这里初始化 LVGLSharp.WinForms 控件或桥接逻辑
        }

        private LVGLSharp.Runtime.Remote.Vnc.VncView? _vncView;

        private void frmMain_Load(object sender, EventArgs e)
        {
            // 只需创建 VncView 并挂载到宿主面板，由 VncView 内部负责 VNC 连接和 LVGLSharp runtime 桥接
            _vncView = new LVGLSharp.Runtime.Remote.Vnc.VncView();
            _vncView.Dock = System.Windows.Forms.DockStyle.Fill;
            lvglHostPanel.Controls.Add(_vncView);
        }
    }
}