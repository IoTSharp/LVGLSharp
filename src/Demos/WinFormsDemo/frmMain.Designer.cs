

 

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
            portLabel = new Label();
            portDropdown = new ComboBox();
            refreshButton = new Button();
            baudLabel = new Label();
            baudDropdown = new ComboBox();
            openButton = new Button();
            tpMain = new TableLayoutPanel();
            sendPanel = new FlowLayoutPanel();
            sendInputTextBox = new TextBox();
            sendButton = new Button();
            fillSampleButton = new Button();
            appendNewLineCheckBox = new CheckBox();
            previewPictureBox = new PictureBox();
            zoomPreviewRadioButton = new RadioButton();
            loadLogoButton = new Button();
            clearLogoButton = new Button();
            stretchPreviewCheckBox = new CheckBox();
            receivePanel = new FlowLayoutPanel();
            receiveTextArea = new TextBox();
            copyReceiveButton = new Button();
            clearButton = new Button();
            hexSwitch = new CheckBox();
            toolbar.SuspendLayout();
            tpMain.SuspendLayout();
            sendPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)previewPictureBox).BeginInit();
            receivePanel.SuspendLayout();
            SuspendLayout();
            // 
            // toolbar
            // 
            toolbar.Controls.Add(portLabel);
            toolbar.Controls.Add(portDropdown);
            toolbar.Controls.Add(refreshButton);
            toolbar.Controls.Add(baudLabel);
            toolbar.Controls.Add(baudDropdown);
            toolbar.Controls.Add(openButton);
            toolbar.Location = new Point(3, 3);
            toolbar.Name = "toolbar";
            toolbar.Size = new Size(771, 49);
            toolbar.TabIndex = 0;
            // 
            // portLabel
            // 
            portLabel.Location = new Point(3, 0);
            portLabel.Name = "portLabel";
            portLabel.Size = new Size(100, 50);
            portLabel.TabIndex = 0;
            portLabel.Text = "串口";
            portLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // portDropdown
            // 
            portDropdown.DropDownHeight = 105;
            portDropdown.FlatStyle = FlatStyle.Flat;
            portDropdown.Font = new Font("Microsoft YaHei UI", 9F);
            portDropdown.FormattingEnabled = true;
            portDropdown.IntegralHeight = false;
            portDropdown.ItemHeight = 17;
            portDropdown.Location = new Point(109, 3);
            portDropdown.Name = "portDropdown";
            portDropdown.Size = new Size(150, 25);
            portDropdown.TabIndex = 2;
            // 
            // refreshButton
            // 
            refreshButton.Location = new Point(265, 3);
            refreshButton.Name = "refreshButton";
            refreshButton.Size = new Size(111, 46);
            refreshButton.TabIndex = 1;
            refreshButton.Text = "刷新串口";
            refreshButton.UseVisualStyleBackColor = true;
            // 
            // baudLabel
            // 
            baudLabel.Location = new Point(382, 0);
            baudLabel.Name = "baudLabel";
            baudLabel.Size = new Size(100, 49);
            baudLabel.TabIndex = 3;
            baudLabel.Text = "波特率";
            baudLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // baudDropdown
            // 
            baudDropdown.Font = new Font("Microsoft YaHei UI", 9F);
            baudDropdown.FormattingEnabled = true;
            baudDropdown.Location = new Point(488, 3);
            baudDropdown.Name = "baudDropdown";
            baudDropdown.Size = new Size(121, 25);
            baudDropdown.TabIndex = 4;
            // 
            // openButton
            // 
            openButton.Location = new Point(615, 3);
            openButton.Name = "openButton";
            openButton.Size = new Size(75, 49);
            openButton.TabIndex = 5;
            openButton.Text = "打开串口";
            openButton.UseVisualStyleBackColor = true;
            // 
            // tpMain
            // 
            tpMain.ColumnCount = 1;
            tpMain.ColumnStyles.Add(new ColumnStyle());
            tpMain.Controls.Add(sendPanel, 0, 2);
            tpMain.Controls.Add(toolbar, 0, 0);
            tpMain.Controls.Add(receivePanel, 3, 1);
            tpMain.Dock = DockStyle.Fill;
            tpMain.Location = new Point(0, 0);
            tpMain.Name = "tpMain";
            tpMain.RowCount = 3;
            tpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            tpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 185F));
            tpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            tpMain.Size = new Size(800, 441);
            tpMain.TabIndex = 1;
            // 
            // sendPanel
            // 
            sendPanel.Controls.Add(sendInputTextBox);
            sendPanel.Controls.Add(sendButton);
            sendPanel.Controls.Add(fillSampleButton);
            sendPanel.Controls.Add(appendNewLineCheckBox);
            sendPanel.Controls.Add(previewPictureBox);
            sendPanel.Controls.Add(zoomPreviewRadioButton);
            sendPanel.Controls.Add(loadLogoButton);
            sendPanel.Controls.Add(clearLogoButton);
            sendPanel.Controls.Add(stretchPreviewCheckBox);
            sendPanel.Dock = DockStyle.Fill;
            sendPanel.Location = new Point(3, 243);
            sendPanel.Name = "sendPanel";
            sendPanel.Size = new Size(794, 195);
            sendPanel.TabIndex = 2;
            // 
            // sendInputTextBox
            // 
            sendInputTextBox.Location = new Point(3, 3);
            sendInputTextBox.Multiline = true;
            sendInputTextBox.Name = "sendInputTextBox";
            sendInputTextBox.PlaceholderText = "输入的数据";
            sendInputTextBox.Size = new Size(570, 50);
            sendInputTextBox.TabIndex = 0;
            // 
            // sendButton
            // 
            sendButton.Location = new Point(579, 3);
            sendButton.Name = "sendButton";
            sendButton.Size = new Size(106, 50);
            sendButton.TabIndex = 2;
            sendButton.Text = "发送";
            sendButton.UseVisualStyleBackColor = true;
            sendButton.Click += send_btn_Click;
            // 
            // fillSampleButton
            // 
            fillSampleButton.Location = new Point(691, 3);
            fillSampleButton.Name = "fillSampleButton";
            fillSampleButton.Size = new Size(75, 23);
            fillSampleButton.TabIndex = 4;
            fillSampleButton.Text = "button2";
            fillSampleButton.UseVisualStyleBackColor = true;
            // 
            // appendNewLineCheckBox
            // 
            appendNewLineCheckBox.AutoSize = true;
            appendNewLineCheckBox.Location = new Point(3, 59);
            appendNewLineCheckBox.Name = "appendNewLineCheckBox";
            appendNewLineCheckBox.Size = new Size(89, 21);
            appendNewLineCheckBox.TabIndex = 5;
            appendNewLineCheckBox.Text = "checkBox1";
            appendNewLineCheckBox.UseVisualStyleBackColor = true;
            // 
            // previewPictureBox
            // 
            previewPictureBox.Location = new Point(98, 59);
            previewPictureBox.Name = "previewPictureBox";
            previewPictureBox.Size = new Size(100, 50);
            previewPictureBox.TabIndex = 6;
            previewPictureBox.TabStop = false;
            // 
            // zoomPreviewRadioButton
            // 
            zoomPreviewRadioButton.AutoSize = true;
            zoomPreviewRadioButton.Location = new Point(204, 59);
            zoomPreviewRadioButton.Name = "zoomPreviewRadioButton";
            zoomPreviewRadioButton.Size = new Size(102, 21);
            zoomPreviewRadioButton.TabIndex = 7;
            zoomPreviewRadioButton.TabStop = true;
            zoomPreviewRadioButton.Text = "radioButton1";
            zoomPreviewRadioButton.UseVisualStyleBackColor = true;
            // 
            // loadLogoButton
            // 
            loadLogoButton.Location = new Point(312, 59);
            loadLogoButton.Name = "loadLogoButton";
            loadLogoButton.Size = new Size(75, 23);
            loadLogoButton.TabIndex = 8;
            loadLogoButton.Text = "button3";
            loadLogoButton.UseVisualStyleBackColor = true;
            // 
            // clearLogoButton
            // 
            clearLogoButton.Location = new Point(393, 59);
            clearLogoButton.Name = "clearLogoButton";
            clearLogoButton.Size = new Size(75, 23);
            clearLogoButton.TabIndex = 9;
            clearLogoButton.Text = "button4";
            clearLogoButton.UseVisualStyleBackColor = true;
            // 
            // stretchPreviewCheckBox
            // 
            stretchPreviewCheckBox.AutoSize = true;
            stretchPreviewCheckBox.Location = new Point(474, 59);
            stretchPreviewCheckBox.Name = "stretchPreviewCheckBox";
            stretchPreviewCheckBox.Size = new Size(89, 21);
            stretchPreviewCheckBox.TabIndex = 10;
            stretchPreviewCheckBox.Text = "checkBox2";
            stretchPreviewCheckBox.UseVisualStyleBackColor = true;
            // 
            // receivePanel
            // 
            tpMain.SetColumnSpan(receivePanel, 3);
            receivePanel.Controls.Add(receiveTextArea);
            receivePanel.Controls.Add(copyReceiveButton);
            receivePanel.Controls.Add(clearButton);
            receivePanel.Controls.Add(hexSwitch);
            receivePanel.Dock = DockStyle.Fill;
            receivePanel.Location = new Point(3, 58);
            receivePanel.Name = "receivePanel";
            receivePanel.Size = new Size(794, 179);
            receivePanel.TabIndex = 1;
            // 
            // receiveTextArea
            // 
            receiveTextArea.Location = new Point(3, 3);
            receiveTextArea.Multiline = true;
            receiveTextArea.Name = "receiveTextArea";
            receiveTextArea.Size = new Size(504, 183);
            receiveTextArea.TabIndex = 0;
            receiveTextArea.Text = "接收的数据...";
            // 
            // copyReceiveButton
            // 
            copyReceiveButton.Location = new Point(513, 3);
            copyReceiveButton.Name = "copyReceiveButton";
            copyReceiveButton.Size = new Size(75, 23);
            copyReceiveButton.TabIndex = 4;
            copyReceiveButton.Text = "button1";
            copyReceiveButton.UseVisualStyleBackColor = true;
            // 
            // clearButton
            // 
            clearButton.Location = new Point(594, 3);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(75, 41);
            clearButton.TabIndex = 2;
            clearButton.Text = "清空";
            clearButton.UseVisualStyleBackColor = true;
            clearButton.Click += button1_Click;
            // 
            // hexSwitch
            // 
            hexSwitch.Location = new Point(675, 3);
            hexSwitch.Name = "hexSwitch";
            hexSwitch.Size = new Size(91, 41);
            hexSwitch.TabIndex = 3;
            hexSwitch.Text = "HEX模式";
            hexSwitch.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 441);
            Controls.Add(tpMain);
            Name = "frmMain";
            Text = "LVGLSharp";
            Load += Form1_Load;
            toolbar.ResumeLayout(false);
            tpMain.ResumeLayout(false);
            sendPanel.ResumeLayout(false);
            sendPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)previewPictureBox).EndInit();
            receivePanel.ResumeLayout(false);
            receivePanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel toolbar;
        private Label portLabel;
        private ComboBox portDropdown;
        private Button refreshButton;
        private TableLayoutPanel tpMain;
        private FlowLayoutPanel receivePanel;
        private Label baudLabel;
        private ComboBox baudDropdown;
        private Button openButton;
        private TextBox receiveTextArea;
        private Button clearButton;
        private CheckBox hexSwitch;
        private FlowLayoutPanel sendPanel;
        private TextBox sendInputTextBox;
        private Button sendButton;
        private Button fillSampleButton;
        private CheckBox appendNewLineCheckBox;
        private PictureBox previewPictureBox;
        private RadioButton zoomPreviewRadioButton;
        private Button loadLogoButton;
        private Button clearLogoButton;
        private CheckBox stretchPreviewCheckBox;
        private Button copyReceiveButton;
    }

 
}



