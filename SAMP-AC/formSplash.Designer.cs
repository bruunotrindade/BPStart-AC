namespace SAMP_AC
{
    partial class formSplash
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
            this.picBoxSplash = new System.Windows.Forms.PictureBox();
            this.timerSplash = new System.Windows.Forms.Timer(this.components);
            this.lblInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSplash)).BeginInit();
            this.SuspendLayout();
            // 
            // picBoxSplash
            // 
            this.picBoxSplash.BackColor = System.Drawing.Color.Black;
            this.picBoxSplash.Image = global::SAMP_AC.Properties.Resources.shield_logo;
            this.picBoxSplash.Location = new System.Drawing.Point(0, 0);
            this.picBoxSplash.Name = "picBoxSplash";
            this.picBoxSplash.Size = new System.Drawing.Size(253, 257);
            this.picBoxSplash.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBoxSplash.TabIndex = 0;
            this.picBoxSplash.TabStop = false;
            // 
            // timerSplash
            // 
            this.timerSplash.Interval = 50;
            this.timerSplash.Tick += new System.EventHandler(this.timerSplash_Tick);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.ForeColor = System.Drawing.Color.White;
            this.lblInfo.Location = new System.Drawing.Point(255, 203);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(219, 51);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.Text = "SAMP Anti-Cheat 2018 (v.x.x.x.x)\r\nDesenvolvido por: Eduardo_AC\r\nModificado por: (" +
    "Altere)";
            // 
            // formSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(493, 257);
            this.ControlBox = false;
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.picBoxSplash);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formSplash";
            this.Opacity = 0.02D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SAMP AC Loading";
            this.Load += new System.EventHandler(this.formSplash_Load);
            this.Shown += new System.EventHandler(this.formSplash_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSplash)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picBoxSplash;
        private System.Windows.Forms.Timer timerSplash;
        private System.Windows.Forms.Label lblInfo;
    }
}

