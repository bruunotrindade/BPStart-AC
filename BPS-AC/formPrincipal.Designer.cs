namespace BPS_AC
{
    partial class FormPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrincipal));
            this.fbPastaGTA = new System.Windows.Forms.FolderBrowserDialog();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.imagem = new System.Windows.Forms.PictureBox();
            this.lbPastaGTA = new System.Windows.Forms.Label();
            this.tbPastaGTA = new System.Windows.Forms.TextBox();
            this.lbNick = new System.Windows.Forms.Label();
            this.tbNick = new System.Windows.Forms.TextBox();
            this.btJogar = new System.Windows.Forms.Button();
            this.lbVersao = new System.Windows.Forms.Label();
            this.panel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.imagem)).BeginInit();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Interval = 5000;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // imagem
            // 
            this.imagem.BackColor = System.Drawing.Color.Transparent;
            this.imagem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.imagem.Image = global::BPS_AC.Properties.Resources.logo;
            this.imagem.Location = new System.Drawing.Point(75, 3);
            this.imagem.Name = "pictureBox2";
            this.imagem.Size = new System.Drawing.Size(393, 147);
            this.imagem.TabIndex = 12;
            this.imagem.TabStop = false;
            // 
            // lbPastaGTA
            // 
            this.lbPastaGTA.AutoSize = true;
            this.lbPastaGTA.Font = new System.Drawing.Font("Gadugi", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPastaGTA.ForeColor = System.Drawing.Color.Black;
            this.lbPastaGTA.Location = new System.Drawing.Point(8, 156);
            this.lbPastaGTA.Name = "label1";
            this.lbPastaGTA.Size = new System.Drawing.Size(321, 25);
            this.lbPastaGTA.TabIndex = 2;
            this.lbPastaGTA.Text = "Localização do GTA San Andreas:";
            //this.lbPastaGTA.Click += new System.EventHandler(this.label1_Click);
            // 
            // tbPastaGTA
            // 
            this.tbPastaGTA.BackColor = System.Drawing.Color.Silver;
            this.tbPastaGTA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbPastaGTA.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbPastaGTA.ForeColor = System.Drawing.Color.Black;
            this.tbPastaGTA.Location = new System.Drawing.Point(13, 184);
            this.tbPastaGTA.Name = "txtSampPath";
            this.tbPastaGTA.ReadOnly = true;
            this.tbPastaGTA.Size = new System.Drawing.Size(522, 25);
            this.tbPastaGTA.TabIndex = 3;
            this.tbPastaGTA.TabStop = false;
            this.tbPastaGTA.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fbPastaGTA_MouseDown);
            // 
            // lbNick
            // 
            this.lbNick.AutoSize = true;
            this.lbNick.Font = new System.Drawing.Font("Gadugi", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNick.ForeColor = System.Drawing.Color.Black;
            this.lbNick.Location = new System.Drawing.Point(8, 222);
            this.lbNick.Name = "lblNickName";
            this.lbNick.Size = new System.Drawing.Size(58, 25);
            this.lbNick.TabIndex = 4;
            this.lbNick.Text = "Nick:";
            // 
            // tbNick
            // 
            this.tbNick.BackColor = System.Drawing.Color.Silver;
            this.tbNick.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbNick.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbNick.ForeColor = System.Drawing.Color.Black;
            this.tbNick.Location = new System.Drawing.Point(13, 250);
            this.tbNick.MaxLength = 24;
            this.tbNick.Name = "txtNickName";
            this.tbNick.Size = new System.Drawing.Size(229, 25);
            this.tbNick.TabIndex = 5;
            // 
            // btJogar
            // 
            this.btJogar.BackColor = System.Drawing.Color.ForestGreen;
            this.btJogar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btJogar.FlatAppearance.BorderSize = 0;
            this.btJogar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.btJogar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btJogar.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btJogar.Location = new System.Drawing.Point(274, 238);
            this.btJogar.Name = "btnPlay";
            this.btJogar.Size = new System.Drawing.Size(261, 37);
            this.btJogar.TabIndex = 8;
            this.btJogar.Text = "Jogar";
            this.btJogar.UseVisualStyleBackColor = false;
            this.btJogar.Click += new System.EventHandler(this.btJogar_Click);
            // 
            // lbVersao
            // 
            this.lbVersao.AutoSize = true;
            this.lbVersao.BackColor = System.Drawing.Color.Transparent;
            this.lbVersao.Font = new System.Drawing.Font("Gadugi", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbVersao.ForeColor = System.Drawing.Color.Red;
            this.lbVersao.Location = new System.Drawing.Point(372, 119);
            this.lbVersao.Name = "lblVersion";
            this.lbVersao.Size = new System.Drawing.Size(83, 19);
            this.lbVersao.TabIndex = 10;
            this.lbVersao.Text = "Versão 1.0";
            //this.lbVersao.Click += new System.EventHandler(this.lblVersion_Click);
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.Color.Gainsboro;
            this.panel.Controls.Add(this.lbVersao);
            this.panel.Controls.Add(this.btJogar);
            this.panel.Controls.Add(this.tbNick);
            this.panel.Controls.Add(this.lbNick);
            this.panel.Controls.Add(this.tbPastaGTA);
            this.panel.Controls.Add(this.lbPastaGTA);
            this.panel.Controls.Add(this.imagem);
            this.panel.Location = new System.Drawing.Point(0, -1);
            this.panel.Name = "panelScreen";
            this.panel.Size = new System.Drawing.Size(549, 299);
            this.panel.TabIndex = 0;
            //this.panel.Paint += new System.Windows.Forms.PaintEventHandler(this.panelScreen_Paint);
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(549, 291);
            this.Controls.Add(this.panel);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Brasil Play Start - Anti Cheater";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPrincipal_FormClosing);
            this.Load += new System.EventHandler(this.FormPrincipal_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormPrincipal_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormPrincipal_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormPrincipal_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.imagem)).EndInit();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog fbPastaGTA;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.PictureBox imagem;
        private System.Windows.Forms.Label lbPastaGTA;
        private System.Windows.Forms.TextBox tbPastaGTA;
        private System.Windows.Forms.Label lbNick;
        private System.Windows.Forms.TextBox tbNick;
        private System.Windows.Forms.Button btJogar;
        private System.Windows.Forms.Label lbVersao;
        private System.Windows.Forms.Panel panel;
    }
}