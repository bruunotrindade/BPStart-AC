namespace SAMP_AC
{
    partial class formPrincipal
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
            this.panelScreen = new System.Windows.Forms.Panel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.btnPlay = new System.Windows.Forms.Button();
            this.txtNickName = new System.Windows.Forms.TextBox();
            this.lblNickName = new System.Windows.Forms.Label();
            this.txtSampPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.picBoxSplash = new System.Windows.Forms.PictureBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.folderBrowserSampPath = new System.Windows.Forms.FolderBrowserDialog();
            this.timerCheck = new System.Windows.Forms.Timer(this.components);
            this.panelScreen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSplash)).BeginInit();
            this.SuspendLayout();
            // 
            // panelScreen
            // 
            this.panelScreen.BackColor = System.Drawing.Color.Black;
            this.panelScreen.Controls.Add(this.lblVersion);
            this.panelScreen.Controls.Add(this.txtLog);
            this.panelScreen.Controls.Add(this.lblLog);
            this.panelScreen.Controls.Add(this.btnPlay);
            this.panelScreen.Controls.Add(this.txtNickName);
            this.panelScreen.Controls.Add(this.lblNickName);
            this.panelScreen.Controls.Add(this.txtSampPath);
            this.panelScreen.Controls.Add(this.label1);
            this.panelScreen.Controls.Add(this.picBoxSplash);
            this.panelScreen.Location = new System.Drawing.Point(0, 28);
            this.panelScreen.Name = "panelScreen";
            this.panelScreen.Size = new System.Drawing.Size(675, 307);
            this.panelScreen.TabIndex = 0;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(4, 14);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(58, 17);
            this.lblVersion.TabIndex = 10;
            this.lblVersion.Text = "v.x.x.x.x";
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.Gray;
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLog.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtLog.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.Color.White;
            this.txtLog.Location = new System.Drawing.Point(280, 150);
            this.txtLog.MaxLength = 24;
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(389, 108);
            this.txtLog.TabIndex = 9;
            this.txtLog.TabStop = false;
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(277, 130);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(36, 17);
            this.lblLog.TabIndex = 9;
            this.lblLog.Text = "Log:";
            // 
            // btnPlay
            // 
            this.btnPlay.BackColor = System.Drawing.Color.DimGray;
            this.btnPlay.FlatAppearance.BorderSize = 0;
            this.btnPlay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlay.Location = new System.Drawing.Point(280, 264);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(389, 37);
            this.btnPlay.TabIndex = 8;
            this.btnPlay.Text = "PLAY";
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // txtNickName
            // 
            this.txtNickName.BackColor = System.Drawing.Color.Gray;
            this.txtNickName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNickName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNickName.ForeColor = System.Drawing.Color.White;
            this.txtNickName.Location = new System.Drawing.Point(280, 92);
            this.txtNickName.MaxLength = 24;
            this.txtNickName.Name = "txtNickName";
            this.txtNickName.Size = new System.Drawing.Size(229, 25);
            this.txtNickName.TabIndex = 5;
            // 
            // lblNickName
            // 
            this.lblNickName.AutoSize = true;
            this.lblNickName.Location = new System.Drawing.Point(277, 72);
            this.lblNickName.Name = "lblNickName";
            this.lblNickName.Size = new System.Drawing.Size(40, 17);
            this.lblNickName.TabIndex = 4;
            this.lblNickName.Text = "Nick:";
            // 
            // txtSampPath
            // 
            this.txtSampPath.BackColor = System.Drawing.Color.Gray;
            this.txtSampPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSampPath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txtSampPath.ForeColor = System.Drawing.Color.White;
            this.txtSampPath.Location = new System.Drawing.Point(280, 34);
            this.txtSampPath.Name = "txtSampPath";
            this.txtSampPath.ReadOnly = true;
            this.txtSampPath.Size = new System.Drawing.Size(389, 25);
            this.txtSampPath.TabIndex = 3;
            this.txtSampPath.TabStop = false;
            this.txtSampPath.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtSampPath_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(277, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Diretório do SAMP:";
            // 
            // picBoxSplash
            // 
            this.picBoxSplash.Image = global::SAMP_AC.Properties.Resources.shield_logo;
            this.picBoxSplash.Location = new System.Drawing.Point(7, 34);
            this.picBoxSplash.Name = "picBoxSplash";
            this.picBoxSplash.Size = new System.Drawing.Size(267, 267);
            this.picBoxSplash.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBoxSplash.TabIndex = 2;
            this.picBoxSplash.TabStop = false;
            // 
            // btnExit
            // 
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(638, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(37, 28);
            this.btnExit.TabIndex = 1;
            this.btnExit.TabStop = false;
            this.btnExit.Text = "X";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnMinimize
            // 
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimize.Location = new System.Drawing.Point(601, 0);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(37, 28);
            this.btnMinimize.TabIndex = 0;
            this.btnMinimize.TabStop = false;
            this.btnMinimize.Text = "–";
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // timerCheck
            // 
            this.timerCheck.Interval = 5000;
            this.timerCheck.Tick += new System.EventHandler(this.timerCheck_Tick);
            // 
            // formPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(675, 335);
            this.ControlBox = false;
            this.Controls.Add(this.btnMinimize);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.panelScreen);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SAMP AC";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formPrincipal_FormClosing);
            this.Load += new System.EventHandler(this.formPrincipal_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.formPrincipal_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.formPrincipal_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.formPrincipal_MouseUp);
            this.panelScreen.ResumeLayout(false);
            this.panelScreen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSplash)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelScreen;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.PictureBox picBoxSplash;
        private System.Windows.Forms.TextBox txtSampPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserSampPath;
        private System.Windows.Forms.TextBox txtNickName;
        private System.Windows.Forms.Label lblNickName;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.Timer timerCheck;
        private System.Windows.Forms.Label lblVersion;
    }
}