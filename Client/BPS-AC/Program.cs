/**
 * Brasil Play Start - Anti Cheater
 * Desenvolvido por: iHollyZinhO
 **/

using System;
using System.Windows.Forms;

namespace BPS_AC
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormPrincipal());
        }
    }
}
