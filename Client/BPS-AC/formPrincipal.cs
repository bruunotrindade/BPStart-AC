using System;
using System.Drawing;
using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Management;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Threading;

namespace BPS_AC
{
    public partial class FormPrincipal : Form
    {
        //Atributos de execução
        private Comunicador comunicador;
        private Verificador verificador;
        private Auditoria auditoria = null;
        private readonly string SERVER_PORTA = "7777";
        private readonly string SERVER_IP = "127.0.0.1";
        private int tempo = 0;

        //Mover formulário
        private Point formLocation = Point.Empty;
        private Boolean formMoving = false;

        // -- Cheat Codes
        /*private Int16 CHEATCODE_MULTIPLE_EXEC = (2);
        private Int16 CHEATCODE_SUSPECT_OBJECTS = (3);
        private Int16 CHEATCODE_DIFF_NAME = (4);
        private Int16 CHEATCODE_CHANGED_AC = (5);
        private Int16 CHEATCODE_DIFF_VERSION = (6);*/

        //Callback chamada ao abrir o Launcher para inicializar os componentes
        public FormPrincipal()
        {
            InitializeComponent();
        }

        //Callback chamada quando o Launcher inicializa
        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            // -- Impedir mais de uma janela aberta do Launcher
            int processos = 0;
            foreach (var proc in Process.GetProcessesByName(Application.ProductName))
            {
                if (++processos > 1)
                {
                    MessageBox.Show(@"Já existe um BPS AC em execução!");
                    Application.Exit();
                }
            }

            // -- Configuração inicial
            this.Icon = Properties.Resources.icon;
            this.lbVersao.Text = "Versão " + ProductVersion;

            String pastaGTA = (String)Registry.GetValue(@"HKEY_CURRENT_USER\Software\SAMP", @"gta_sa_exe", null);
            if (pastaGTA == null)
                MessageBox.Show(@"O diretório do seu GTA San Andreas não foi encontrado. Selecione-o!", @"GTA San Andreas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                tbPastaGTA.Text = pastaGTA.Substring(0, pastaGTA.Length - 10);

            String nick = (String)Registry.GetValue(@"HKEY_CURRENT_USER\Software\SAMP", @"PlayerName", "Player");
            tbNick.Text = nick; 
        }

        //Callback chamada quando o mouse encontra-se pressionado no formulário
        private void FormPrincipal_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.formLocation = e.Location;
                this.formMoving = true;
            }
        }

        //Callback chamada quando o mouse encontra-se em movimento
        private void FormPrincipal_MouseMove(object sender, MouseEventArgs e)
        {
            if (formMoving)
                this.Location = new Point(Cursor.Position.X - formLocation.X, Cursor.Position.Y - formLocation.Y);
        }

        //Callback chamada quando o mouse encontra-se pressionado no formulário
        private void FormPrincipal_MouseUp(object sender, MouseEventArgs e)
        {
            formMoving = false;
        }

        //Finalizar programa
        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(comunicador != null && comunicador.Status == 3)
                comunicador.AlertarServidor(Utils.Codigos.FECHADO);

            Environment.Exit(0);
        }

        //Abre a janela para buscar a pasta do jogo
        private void fbPastaGTA_MouseDown(object sender, MouseEventArgs e)
        {
            if (fbPastaGTA.ShowDialog() == DialogResult.OK)
                tbPastaGTA.Text = fbPastaGTA.SelectedPath + @"\";
        }

        //Busca arquivos suspeitos e inicia o jogo
        private void btJogar_Click(object sender, EventArgs e)
        {
            btJogar.Enabled = false;
            tbNick.Enabled = false;
            tbPastaGTA.Enabled = false;

            panelOff.Visible = false;
            panelOn.Visible = true;

            lbNickOn.Text = $"Nick: {tbNick.Text}";
            lbIniciado.Text = "Iniciado às: " + DateTime.Now.ToString("HH:mm:ss");

            // -- Cria um hash do programa para checar se o mesmo não foi alterado
            string hashAC = Utils.GerarHashAC();

            // -- Pegar ID parcial de hardware
            string hashPC = Utils.RecuperarHardwareID();

            if (hashPC == null)
            {
                EnviarErro(Utils.Erros.HARDWARE_INCONSISTENTE, "Falha", @"Informações de hardware inconsistentes!" + Environment.NewLine + "@Execute este Launcher com privilégios administrativos e tente novamente.");
                return;
            }
            else
            {
                foreach (var proc in Process.GetProcessesByName(@"gta_sa"))
                {
                    DialogResult result = MessageBox.Show(@"Você precisa fechar o GTA SA antes de iniciá-lo." + Environment.NewLine + @"Clique em OK para fechá-lo.", @"Fechar o GTA SA?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                    if (result == DialogResult.OK)
                        proc.Kill();
                    else
                    {
                        EnviarErro(Utils.Erros.MULTIPLA_EXECUCAO, "Aviso", "Você poderá entrar no servidor após fechar o GTA San Andreas.");
                        Application.Exit();
                    }
                }
            }

            verificador = new Verificador(tbPastaGTA.Text);
            auditoria = verificador.BuscarArquivos();
            if(auditoria.LoginPermitido == false)
            {
                EnviarErro(Utils.Erros.ARQUIVOS_SUSPEITOS, "Falha", "Você não está autorizado a entrar no servidor!");
                return;
            }

            AtualizarStatus("Iniciando", Color.DimGray);
            timerGeral.Start();

            //Iniciando a comunicação
            comunicador = new Comunicador(hashPC, hashAC);
            comunicador.Iniciar();

            Utils.FecharSAMP();
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\SAMP", @"PlayerName", tbNick.Text);
            Process.Start($"{tbPastaGTA.Text}samp.exe", $"{SERVER_IP}:{SERVER_PORTA}");
        }

        //Atualizar o label de status
        private void AtualizarStatus(string texto, Color color)
        {
            lbStatus.Text = texto;
            lbStatus.ForeColor = color;
        }

        //Enviar mensagem de erro
        public void EnviarErro(Utils.Erros codigo, string titulo, string mensagem)
        {
            AtualizarStatus($"Erro {(int)codigo}", Color.Red);
            MessageBox.Show(mensagem, titulo + $" {(int)codigo}", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        //Validação dos processos de gta_sa.exe
        public bool ValidarProcessos()
        {
            return true;
            /*int processos = 0;
            foreach (var proc in Process.GetProcessesByName(@"gta_sa"))
            {
                processos++;
                using (ManagementObjectSearcher moSearcher = new ManagementObjectSearcher("SELECT ExecutablePath FROM Win32_Process WHERE ProcessId = " + proc.Id))
                {
                    using (ManagementObjectCollection moCollection = moSearcher.Get())
                    {
                        foreach (var execPath in moCollection.Cast<ManagementObject>())
                        {
                            string ExecutablePath = execPath["ExecutablePath"].ToString();

                            String procPath = ExecutablePath.Substring(0, ExecutablePath.Length - proc.MainModule.ModuleName.Length);

                            if (procPath != txtSampPath.Text)
                            {
                                Byte[] getData = Encoding.UTF8.GetBytes("cheat_" + CHEATCODE_DOUBLE_EXEC + "*id_" + GetPlayerInfo.Playerid);
                                receiveSocket.BeginSend(getData, 0, getData.Length, SocketFlags.None, new AsyncCallback(SendCallback), receiveSocket);

                                proc.Kill();
                            }
                        }
                    }
                }
            }

            if (processos > 1)
            {
                try
                {
                    Byte[] getData = Encoding.UTF8.GetBytes(@"cheat_" + CHEATCODE_MULTIPLE_EXEC + @"*id_" + GetPlayerInfo.Playerid);
                    receiveSocket.BeginSend(getData, 0, getData.Length, SocketFlags.None, new AsyncCallback(SendCallback), receiveSocket);
                }
                catch { }
            }*/
        }

        //Verificar a pasta do GTA em busca de arquivos suspeitos
        private void Timer_Tick(object sender, EventArgs e)
        {
            Auditoria a = verificador.BuscarArquivos();

            if (a.Suspeitos.Count > 0)
                comunicador.AlertarServidor(Utils.Codigos.ARQUIVOS_SUSPEITOS);
            else if (auditoria.Equals(a) == false)
                comunicador.AlertarServidor(Utils.Codigos.ARQUIVOS_MODIFICADOS);

            auditoria = a;

            // -- Finalizar processos do GTA SA onde o diretório é diferente do informado
            if (ValidarProcessos() == false)
                comunicador.AlertarServidor(Utils.Codigos.MULTIPLA_EXECUCAO);
        }

        public void TimerGeral_Tick(object sender, EventArgs e)
        {
            if(comunicador.Status > 0)
                tempo += 1;
            lbTempoExecutando.Text = Utils.FormatarTempo(tempo);

            if (comunicador.Status == 1)
            {
                AtualizarStatus("Conectado", Color.ForestGreen);
                comunicador.Status = 2;
                timer.Start();
            }

            if(Process.GetProcessesByName(@"gta_sa").Length == 0 || comunicador.Status >= 3)
            {
                AtualizarStatus("Desconectado", Color.Red);
                timer.Stop();
                timerGeral.Stop();
            }
        }
    }
}
