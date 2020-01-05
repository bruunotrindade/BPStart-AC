using System;
using System.Windows.Forms;

namespace SAMP_AC
{
    public partial class formSplash : Form
    {
        public formSplash()
        {
            InitializeComponent();
        }

        private void formSplash_Load(object sender, EventArgs e)
        {
            // -- Configuração inicial
            this.Icon = Properties.Resources.shied_icon;
            this.timerSplash.Start();
            this.lblInfo.Text = 
                @"SAMP Anti-Cheat 2018 (v." + Application.ProductVersion + ")" + Environment.NewLine +
                @"Desenvolvido por: Eduardo_AC" + Environment.NewLine +
                @"Modificado por: (Altere)";
        }

        private void formSplash_Shown(object sender, EventArgs e)
        {
            
        }

        private void timerSplash_Tick(object sender, EventArgs e)
        {
            this.Opacity += 0.02;

            if (this.Opacity > 0.99)
            {
                this.timerSplash.Stop();
                this.Hide();

                formPrincipal frm = new formPrincipal();
                frm.Show();
            }

        }
    }
}
