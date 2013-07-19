namespace MetalDep
{
    partial class frmMetalDep
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMetalDep));
            this.cmbxMachine = new System.Windows.Forms.ComboBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.chkbxAllowClose = new System.Windows.Forms.CheckBox();
            this.chkbxExcel = new System.Windows.Forms.CheckBox();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtbxOutput = new System.Windows.Forms.TextBox();
            this.FatherTime = new System.Windows.Forms.Timer(this.components);
            this.picbxMetalDep = new System.Windows.Forms.PictureBox();
            this.SerialPort = new System.IO.Ports.SerialPort(this.components);
            this.serialCOMcmbbx = new System.Windows.Forms.ComboBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.timer_SerialRead = new System.Windows.Forms.Timer(this.components);
            this.lblTriggerActionPanel = new System.Windows.Forms.Label();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.pnlSettings = new System.Windows.Forms.Panel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnBack2Actions = new System.Windows.Forms.Button();
            this.pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbxMetalDep)).BeginInit();
            this.pnlSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbxMachine
            // 
            this.cmbxMachine.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbxMachine.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbxMachine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbxMachine.FormattingEnabled = true;
            this.cmbxMachine.Items.AddRange(new object[] {
            "PVD",
            "Lesker",
            "Leybold",
            "Veeco",
            "PCD Sputt"});
            this.cmbxMachine.Location = new System.Drawing.Point(51, 82);
            this.cmbxMachine.Name = "cmbxMachine";
            this.cmbxMachine.Size = new System.Drawing.Size(144, 25);
            this.cmbxMachine.TabIndex = 1;
            this.cmbxMachine.Text = "Choose a Machine.";
            this.cmbxMachine.SelectedIndexChanged += new System.EventHandler(this.cmbxMachine_SelectedIndexChanged);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Black;
            this.btnStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.btnStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.btnStart.Location = new System.Drawing.Point(10, 9);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(103, 50);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start Collection";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            this.btnStart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnStart_MouseMove);
            // 
            // pnlActions
            // 
            this.pnlActions.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.pnlActions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlActions.Controls.Add(this.pnlSettings);
            this.pnlActions.Controls.Add(this.btnSettings);
            this.pnlActions.Controls.Add(this.btnQuit);
            this.pnlActions.Controls.Add(this.btnClear);
            this.pnlActions.Controls.Add(this.btnStart);
            this.pnlActions.Location = new System.Drawing.Point(6, 562);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(437, 70);
            this.pnlActions.TabIndex = 3;
            this.pnlActions.Leave += new System.EventHandler(this.pnlActions_Leave);
            this.pnlActions.MouseLeave += new System.EventHandler(this.pnlActions_MouseLeave);
            this.pnlActions.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlActions_MouseMove);
            // 
            // chkbxAllowClose
            // 
            this.chkbxAllowClose.BackColor = System.Drawing.Color.Black;
            this.chkbxAllowClose.Checked = true;
            this.chkbxAllowClose.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbxAllowClose.FlatAppearance.BorderSize = 0;
            this.chkbxAllowClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkbxAllowClose.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkbxAllowClose.Location = new System.Drawing.Point(14, 37);
            this.chkbxAllowClose.Name = "chkbxAllowClose";
            this.chkbxAllowClose.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.chkbxAllowClose.Size = new System.Drawing.Size(101, 22);
            this.chkbxAllowClose.TabIndex = 6;
            this.chkbxAllowClose.Text = "Allow Close ?";
            this.chkbxAllowClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkbxAllowClose.UseVisualStyleBackColor = false;
            this.chkbxAllowClose.CheckedChanged += new System.EventHandler(this.chkbxAllowClose_CheckedChanged);
            // 
            // chkbxExcel
            // 
            this.chkbxExcel.BackColor = System.Drawing.Color.Black;
            this.chkbxExcel.Checked = true;
            this.chkbxExcel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbxExcel.FlatAppearance.BorderSize = 0;
            this.chkbxExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkbxExcel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkbxExcel.Location = new System.Drawing.Point(14, 9);
            this.chkbxExcel.Name = "chkbxExcel";
            this.chkbxExcel.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.chkbxExcel.Size = new System.Drawing.Size(101, 22);
            this.chkbxExcel.TabIndex = 5;
            this.chkbxExcel.Text = "Run Excel ?";
            this.chkbxExcel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkbxExcel.UseVisualStyleBackColor = false;
            // 
            // btnQuit
            // 
            this.btnQuit.BackColor = System.Drawing.Color.Black;
            this.btnQuit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.btnQuit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnQuit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuit.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuit.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.btnQuit.Location = new System.Drawing.Point(367, 9);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(50, 50);
            this.btnQuit.TabIndex = 4;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = false;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Black;
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.btnClear.Location = new System.Drawing.Point(129, 9);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(103, 50);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear Collected";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnClear_MouseMove);
            // 
            // txtbxOutput
            // 
            this.txtbxOutput.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtbxOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtbxOutput.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtbxOutput.Location = new System.Drawing.Point(12, 113);
            this.txtbxOutput.Multiline = true;
            this.txtbxOutput.Name = "txtbxOutput";
            this.txtbxOutput.ReadOnly = true;
            this.txtbxOutput.Size = new System.Drawing.Size(424, 425);
            this.txtbxOutput.TabIndex = 3;
            this.txtbxOutput.Text = "Start a Collection. Use the Actions menu below!";
            this.txtbxOutput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtbxOutput.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtbxOutput_MouseMove);
            // 
            // FatherTime
            // 
            this.FatherTime.Interval = 25;
            this.FatherTime.Tick += new System.EventHandler(this.FatherTime_Tick);
            // 
            // picbxMetalDep
            // 
            this.picbxMetalDep.Image = global::MetalDep.Properties.Resources.IronsidesName;
            this.picbxMetalDep.Location = new System.Drawing.Point(12, 12);
            this.picbxMetalDep.Name = "picbxMetalDep";
            this.picbxMetalDep.Size = new System.Drawing.Size(424, 50);
            this.picbxMetalDep.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picbxMetalDep.TabIndex = 5;
            this.picbxMetalDep.TabStop = false;
            // 
            // serialCOMcmbbx
            // 
            this.serialCOMcmbbx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.serialCOMcmbbx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.serialCOMcmbbx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.serialCOMcmbbx.FormattingEnabled = true;
            this.serialCOMcmbbx.Location = new System.Drawing.Point(253, 82);
            this.serialCOMcmbbx.Name = "serialCOMcmbbx";
            this.serialCOMcmbbx.Size = new System.Drawing.Size(144, 25);
            this.serialCOMcmbbx.TabIndex = 2;
            this.serialCOMcmbbx.Text = "Choose a COM Port.";
            this.serialCOMcmbbx.SelectedIndexChanged += new System.EventHandler(this.serialCOMcmbbx_SelectedIndexChanged);
            this.serialCOMcmbbx.Click += new System.EventHandler(this.serialCOMcmbbx_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.RestoreDirectory = true;
            this.saveFileDialog.Title = "Where shall we save it?";
            // 
            // timer_SerialRead
            // 
            this.timer_SerialRead.Interval = 1000;
            this.timer_SerialRead.Tick += new System.EventHandler(this.timer_SerialRead_Tick);
            // 
            // lblTriggerActionPanel
            // 
            this.lblTriggerActionPanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblTriggerActionPanel.BackColor = System.Drawing.Color.Black;
            this.lblTriggerActionPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTriggerActionPanel.Location = new System.Drawing.Point(51, 541);
            this.lblTriggerActionPanel.Name = "lblTriggerActionPanel";
            this.lblTriggerActionPanel.Size = new System.Drawing.Size(346, 19);
            this.lblTriggerActionPanel.TabIndex = 6;
            this.lblTriggerActionPanel.Text = "| ^ |          Actions          | ^ |";
            this.lblTriggerActionPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTriggerActionPanel.MouseHover += new System.EventHandler(this.lblTriggerActionPanel_MouseHover);
            // 
            // TrayIcon
            // 
            this.TrayIcon.BalloonTipText = "Collection is: ";
            this.TrayIcon.BalloonTipTitle = "Metal Deposition";
            this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
            this.TrayIcon.Text = "Click to Minimize.";
            this.TrayIcon.Visible = true;
            this.TrayIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseDown);
            // 
            // pnlSettings
            // 
            this.pnlSettings.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pnlSettings.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.pnlSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSettings.Controls.Add(this.btnBack2Actions);
            this.pnlSettings.Controls.Add(this.chkbxAllowClose);
            this.pnlSettings.Controls.Add(this.chkbxExcel);
            this.pnlSettings.Location = new System.Drawing.Point(0, 70);
            this.pnlSettings.Name = "pnlSettings";
            this.pnlSettings.Size = new System.Drawing.Size(437, 70);
            this.pnlSettings.TabIndex = 7;
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.Black;
            this.btnSettings.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.btnSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSettings.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.btnSettings.Location = new System.Drawing.Point(248, 9);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(103, 50);
            this.btnSettings.TabIndex = 8;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnBack2Actions
            // 
            this.btnBack2Actions.BackColor = System.Drawing.Color.Black;
            this.btnBack2Actions.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.btnBack2Actions.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnBack2Actions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack2Actions.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack2Actions.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.btnBack2Actions.Location = new System.Drawing.Point(356, 8);
            this.btnBack2Actions.Name = "btnBack2Actions";
            this.btnBack2Actions.Size = new System.Drawing.Size(65, 50);
            this.btnBack2Actions.TabIndex = 9;
            this.btnBack2Actions.Text = "Back";
            this.btnBack2Actions.UseVisualStyleBackColor = false;
            this.btnBack2Actions.Click += new System.EventHandler(this.btnBack2Actions_Click);
            // 
            // frmMetalDep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(449, 560);
            this.Controls.Add(this.serialCOMcmbbx);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.txtbxOutput);
            this.Controls.Add(this.cmbxMachine);
            this.Controls.Add(this.picbxMetalDep);
            this.Controls.Add(this.lblTriggerActionPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMetalDep";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Metal Deposition | (Idle) ";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMetalDep_FormClosing);
            this.Load += new System.EventHandler(this.frmMetalDep_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMetalDep_MouseMove);
            this.pnlActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picbxMetalDep)).EndInit();
            this.pnlSettings.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbxMachine;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtbxOutput;
        private System.Windows.Forms.Timer FatherTime;
        private System.Windows.Forms.PictureBox picbxMetalDep;
        private System.IO.Ports.SerialPort SerialPort;
        private System.Windows.Forms.ComboBox serialCOMcmbbx;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Timer timer_SerialRead;
        private System.Windows.Forms.Label lblTriggerActionPanel;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.CheckBox chkbxAllowClose;
        private System.Windows.Forms.CheckBox chkbxExcel;
        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.Panel pnlSettings;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnBack2Actions;
    }
}

