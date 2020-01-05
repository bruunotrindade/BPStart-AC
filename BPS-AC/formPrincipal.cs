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
        enum Codigos
        {
            ARQUIVOS_MODIFICADOS = 1
        };

        //Atributos de execução
        private Comunicador comunicador;
        private Auditoria auditoria = null;
        private readonly string SERVER_PORTA = "7777";
        private readonly string SERVER_IP = "127.0.0.1";

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

        //Mover formulário
        private Point formLocation = Point.Empty;
        private Boolean formMoving = false;

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
            foreach (var proc in Process.GetProcessesByName("gta_sa"))
                proc.Kill();

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
            // -- Cria um hash do programa para checar se o mesmo não foi alterado
            string hashAC = GerarHashAC();

            // -- Pegar ID parcial de hardware
            string hashPC = GetHardwareID();

            if (hashPC == null)
            {
                MessageBox.Show(@"Informações de hardware inconsistentes!" + Environment.NewLine + "@Execute este Launcher com privilégios administrativos e tente novamente.", @"Hardware ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        MessageBox.Show(@"Você poderá entrar no servidor após fechar o GTA SA.", @"Feche o GTA SA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                btJogar.Enabled = false;
                tbNick.Enabled = false;
                tbPastaGTA.Enabled = false;

                //Iniciando a comunicação
                comunicador = new Comunicador(hashPC, hashAC);
                comunicador.Iniciar();

                Console.WriteLine($" -c -n {tbNick.Text} -h {SERVER_IP} -p {SERVER_PORTA}");
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\SAMP", @"PlayerName", tbNick.Text);
                Process.Start($"{tbPastaGTA.Text}samp.exe", $"{SERVER_IP}:{SERVER_PORTA}");
            }
        }

        //Gerar o hash do AC
        private string GerarHashAC()
        {
            Byte[] getBytesAC = File.ReadAllBytes(Environment.CurrentDirectory + @"\" + Application.ProductName + ".exe");
            MD5 MD5Hash = MD5.Create();
            Byte[] getComputedHash = MD5Hash.ComputeHash(getBytesAC);

            string hash = "";
            foreach (var x in getComputedHash)
                hash += x.ToString("x2");

            return hash;
        }

        //Obter o hardware ID
        private string GetHardwareID()
        {
            ManagementObjectSearcher searcherHD = new ManagementObjectSearcher(@"SELECT * FROM Win32_DiskDrive");
            ManagementObjectSearcher searcherProcessor = new ManagementObjectSearcher(@"SELECT * FROM win32_processor");
            ManagementObjectSearcher searcherComputer = new ManagementObjectSearcher(@"SELECT * FROM Win32_ComputerSystemProduct");

            String generateHD = null;
            String generateProcessor = null;
            String generateComputer = null;

            foreach (var get in searcherHD.Get())
                generateHD = get.GetPropertyValue(@"SerialNumber").ToString();

            foreach (var get in searcherProcessor.Get())
                generateProcessor = get.Properties[@"processorID"].Value.ToString();

            foreach (var get in searcherComputer.Get())
                generateComputer = get.Properties[@"UUID"].Value.ToString();

            if (generateHD == null && generateProcessor == null && generateComputer == null)
                return null;

            MD5 generateMD5 = MD5.Create();
            Byte[] getHash = generateMD5.ComputeHash(Encoding.UTF8.GetBytes(generateHD + @"*" + generateProcessor + @"*" + generateComputer));

            String hashedString = null;

            foreach (var x in getHash)
                hashedString += x.ToString(@"x2");

            return hashedString;
        }

        //Verificar a pasta do GTA em busca de arquivos suspeitos
        private void Timer_Tick(object sender, EventArgs e)
        {
            Verificador v = new Verificador("D:\\GTA San Andreas\\");
            Auditoria a = v.BuscarArquivos();

            if (auditoria == null)
                auditoria = a;
            else if (auditoria != a)
            {
                Console.WriteLine("Os arquivos foram modificados!!!");
                comunicador.EnviarDados($"cheat_{Codigos.ARQUIVOS_MODIFICADOS}*id_{comunicador.Player.Id}");
            }
            /*Int32 detectedFiles = GetSuspectFiles(txtSampPath.Text);
            Int32 detectedFolders = GetSuspectFolders(txtSampPath.Text);

            // -- Caso detecte objetos suspeitos o processo do GTA SA será finalizado e fechará o Launcher

            if (detectedFiles > 0 || detectedFolders > 0)
            {
                try
                {
                    Byte[] getData = Encoding.UTF8.GetBytes(@"cheat_" + CHEATCODE_SUSPECT_OBJECTS + @"*id_" + GetPlayerInfo.Playerid);
                    receiveSocket.BeginSend(getData, 0, getData.Length, SocketFlags.None, new AsyncCallback(SendCallback), receiveSocket);
                }
                catch { }
            }

            // -- Finalizar processos do GTA SA onde o diretório é diferente do informado

            Int16 countProcess = 0;

            foreach (var proc in Process.GetProcessesByName(@"gta_sa"))
            {
                countProcess++;

                /**
                 * O código abaixo faz comparação de diretório entre o diretório informado no Launcher e o diretório do processo
                 * 
                 */
            /*
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

        if (countProcess > 1)
        {
            try
            {
                Byte[] getData = Encoding.UTF8.GetBytes(@"cheat_" + CHEATCODE_MULTIPLE_EXEC + @"*id_" + GetPlayerInfo.Playerid);
                receiveSocket.BeginSend(getData, 0, getData.Length, SocketFlags.None, new AsyncCallback(SendCallback), receiveSocket);
            }
            catch { }
        }*/
        }
    }
}
