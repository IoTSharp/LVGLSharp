using LVGLSharp.Drawing;

namespace WinFormsRdpDemo;

public partial class frmMain : Form
{
    private Label? _statusLabel;
    private TextBox? _messageInput;

    public frmMain()
    {
        InitializeComponent();
    }

    private void frmMain_Load(object sender, EventArgs e)
    {
        lblTitle.Text = "LVGLSharp WinForms RDP Demo";
        Text = "WinFormsRdpDemo";
        lvglHostPanel.BackColor = new Color(24, 30, 42);

        BuildDemoSurface();
    }

    private void BuildDemoSurface()
    {
        if (_statusLabel is not null)
        {
            return;
        }

        var introLabel = new Label
        {
            Text = "这个窗口正在通过 RdpView 运行时托管，为后续 RDP 协议实现预留 Demo 入口。",
            Location = new Point(18, 18),
            Size = new Size(580, 42),
            ForeColor = new Color(255, 255, 255),
        };

        _messageInput = new TextBox
        {
            Text = "hello from WinForms over RDP",
            PlaceholderText = "输入一些文字，然后点右侧按钮",
            Location = new Point(18, 72),
            Size = new Size(420, 36),
        };

        var echoButton = new Button
        {
            Text = "更新状态",
            Location = new Point(456, 72),
            Size = new Size(140, 36),
        };
        echoButton.Click += EchoButton_Click;

        _statusLabel = new Label
        {
            Text = "状态：RDP transport 已接入生命周期，协议协商仍在实现中",
            Location = new Point(18, 128),
            Size = new Size(578, 42),
            ForeColor = new Color(255, 255, 255),
        };

        var hintLabel = new Label
        {
            Text = "提示：这个 Demo 现在先用于验证 RdpView 注册、窗口承载和后续远程宿主接线点。",
            Location = new Point(18, 178),
            Size = new Size(578, 52),
            ForeColor = new Color(220, 220, 220),
        };

        lvglHostPanel.Controls.Add(introLabel);
        lvglHostPanel.Controls.Add(_messageInput);
        lvglHostPanel.Controls.Add(echoButton);
        lvglHostPanel.Controls.Add(_statusLabel);
        lvglHostPanel.Controls.Add(hintLabel);
    }

    private void EchoButton_Click(object? sender, EventArgs e)
    {
        if (_statusLabel is null || _messageInput is null)
        {
            return;
        }

        var value = string.IsNullOrWhiteSpace(_messageInput.Text)
            ? "<empty>"
            : _messageInput.Text;
        _statusLabel.Text = $"状态：最近一次本地交互 -> {value}";
    }
}
