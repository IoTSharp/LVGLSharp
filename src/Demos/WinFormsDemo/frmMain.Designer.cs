



using System.Drawing;

namespace WinFormsDemo
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            toolbar = new FlowLayoutPanel();
            port_label = new Label();
            port_dropdown = new ComboBox();
            ref_btn = new Button();
            baud_label = new Label();
            baud_dropdown = new ComboBox();
            open_btn = new Button();
            tpMain = new TableLayoutPanel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            textBox1 = new TextBox();
            send_btn = new Button();
            recv_container = new FlowLayoutPanel();
            recv_textarea = new TextBox();
            clear_btn = new Button();
            hex_switch = new CheckBox();
            toolbar.SuspendLayout();
            tpMain.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            recv_container.SuspendLayout();
            SuspendLayout();
            // 
            // toolbar
            // 
            toolbar.Controls.Add(port_label);
            toolbar.Controls.Add(port_dropdown);
            toolbar.Controls.Add(ref_btn);
            toolbar.Controls.Add(baud_label);
            toolbar.Controls.Add(baud_dropdown);
            toolbar.Controls.Add(open_btn);
            toolbar.Location = new Point(3, 3);
            toolbar.Name = "toolbar";
            toolbar.Size = new Size(670, 49);
            toolbar.TabIndex = 0;
            // 
            // port_label
            // 
            port_label.Location = new Point(3, 0);
            port_label.Name = "port_label";
            port_label.Size = new Size(100, 50);
            port_label.TabIndex = 0;
            port_label.Text = "串口";
            port_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // port_dropdown
            // 
            port_dropdown.DropDownHeight = 105;
            port_dropdown.FlatStyle = FlatStyle.Flat;
            port_dropdown.Font = new Font("Microsoft YaHei UI", 9F);
            port_dropdown.FormattingEnabled = true;
            port_dropdown.IntegralHeight = false;
            port_dropdown.ItemHeight = 17;
            port_dropdown.Location = new Point(109, 3);
            port_dropdown.Name = "port_dropdown";
            port_dropdown.Size = new Size(150, 25);
            port_dropdown.TabIndex = 2;
            // 
            // ref_btn
            // 
            ref_btn.Location = new Point(265, 3);
            ref_btn.Name = "ref_btn";
            ref_btn.Size = new Size(87, 46);
            ref_btn.TabIndex = 1;
            ref_btn.Text = "刷新串口";
            ref_btn.UseVisualStyleBackColor = true;
            // 
            // baud_label
            // 
            baud_label.Location = new Point(358, 0);
            baud_label.Name = "baud_label";
            baud_label.Size = new Size(100, 49);
            baud_label.TabIndex = 3;
            baud_label.Text = "波特率";
            baud_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // baud_dropdown
            // 
            baud_dropdown.Font = new Font("Microsoft YaHei UI", 9F);
            baud_dropdown.FormattingEnabled = true;
            baud_dropdown.Location = new Point(464, 3);
            baud_dropdown.Name = "baud_dropdown";
            baud_dropdown.Size = new Size(121, 25);
            baud_dropdown.TabIndex = 4;
            // 
            // open_btn
            // 
            open_btn.Location = new Point(591, 3);
            open_btn.Name = "open_btn";
            open_btn.Size = new Size(75, 46);
            open_btn.TabIndex = 5;
            open_btn.Text = "打开串口";
            open_btn.UseVisualStyleBackColor = true;
            // 
            // tpMain
            // 
            tpMain.ColumnCount = 1;
            tpMain.ColumnStyles.Add(new ColumnStyle());
            tpMain.Controls.Add(flowLayoutPanel1, 0, 2);
            tpMain.Controls.Add(toolbar, 0, 0);
            tpMain.Controls.Add(recv_container, 3, 1);
            tpMain.Dock = DockStyle.Fill;
            tpMain.Location = new Point(0, 0);
            tpMain.Name = "tpMain";
            tpMain.RowCount = 3;
            tpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            tpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 185F));
            tpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            tpMain.Size = new Size(687, 299);
            tpMain.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(textBox1);
            flowLayoutPanel1.Controls.Add(send_btn);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(3, 243);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(681, 144);
            flowLayoutPanel1.TabIndex = 2;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(3, 3);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "输入的数据";
            textBox1.Size = new Size(570, 50);
            textBox1.TabIndex = 0;
            // 
            // send_btn
            // 
            send_btn.Location = new Point(579, 3);
            send_btn.Name = "send_btn";
            send_btn.Size = new Size(75, 37);
            send_btn.TabIndex = 2;
            send_btn.Text = "发送";
            send_btn.UseVisualStyleBackColor = true;
            // 
            // recv_container
            // 
            tpMain.SetColumnSpan(recv_container, 3);
            recv_container.Controls.Add(recv_textarea);
            recv_container.Controls.Add(clear_btn);
            recv_container.Controls.Add(hex_switch);
            recv_container.Dock = DockStyle.Fill;
            recv_container.Location = new Point(3, 58);
            recv_container.Name = "recv_container";
            recv_container.Size = new Size(681, 179);
            recv_container.TabIndex = 1;
            // 
            // recv_textarea
            // 
            recv_textarea.Location = new Point(3, 3);
            recv_textarea.Multiline = true;
            recv_textarea.Name = "recv_textarea";
            recv_textarea.Size = new Size(504, 183);
            recv_textarea.TabIndex = 0;
            recv_textarea.Text = "接收的数据...";
            // 
            // clear_btn
            // 
            clear_btn.Location = new Point(513, 3);
            clear_btn.Name = "clear_btn";
            clear_btn.Size = new Size(75, 41);
            clear_btn.TabIndex = 2;
            clear_btn.Text = "清空";
            clear_btn.UseVisualStyleBackColor = true;
            clear_btn.Click += button1_Click;
            // 
            // hex_switch
            // 
            hex_switch.Location = new Point(3, 192);
            hex_switch.Name = "hex_switch";
            hex_switch.Size = new Size(91, 41);
            hex_switch.TabIndex = 3;
            hex_switch.Text = "HEX模式";
            hex_switch.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(687, 299);
            Controls.Add(tpMain);
            Name = "frmMain";
            Text = "LVGLSharp";
            Load += Form1_Load;
            toolbar.ResumeLayout(false);
            tpMain.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            recv_container.ResumeLayout(false);
            recv_container.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel toolbar;
        private Label port_label;
        private ComboBox port_dropdown;
        private Button ref_btn;
        private TableLayoutPanel tpMain;
        private FlowLayoutPanel recv_container;
        private Label baud_label;
        private ComboBox baud_dropdown;
        private Button open_btn;
        private TextBox recv_textarea;
        private Button clear_btn;
        private CheckBox hex_switch;
        private FlowLayoutPanel flowLayoutPanel1;
        private TextBox textBox1;
        private Button send_btn;

    }

 
}
