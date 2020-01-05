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

namespace SAMP_AC
{
    public partial class formPrincipal : Form
    {
        /**
         * Callback chamada ao abrir o Launcher para inicializar os componentes
         * 
         */
        public formPrincipal()
        {
            InitializeComponent();
        }

        // -- Cheat Codes
        /** @type {number}*/
        private Int16 CHEATCODE_MULTIPLE_EXEC = (2);
        /** @type {number}*/
        private Int16 CHEATCODE_SUSPECT_OBJECTS = (3);
        /** @type {number}*/
        private Int16 CHEATCODE_DIFF_NAME = (4);
        /** @type {number}*/
        private Int16 CHEATCODE_CHANGED_AC = (5);
        /**@type {number} */
        private Int16 CHEATCODE_DIFF_VERSION = (6);

        // -- Variáveis globais
        /** @type {object}*/
        private ListViewItem _lvItem = new ListViewItem();
        /** @type {object}*/
        private Suspect suscpectClass = new Suspect();
        /** @type {object}*/
        private GetPlayerInfo_ GetPlayerInfo = new GetPlayerInfo_();

        /** @type {number}*/
        private Int16 SOCKET_PORT = 6000;
        /** @type {object}*/
        private Socket socket_ac_pc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        /** @type {object}*/
        private Socket receiveSocket = null;

        /** @type {number}*/
        private Byte[] buffer = new Byte[1024];
        /** @type {number}*/
        private String get_ac_hash = null;
        /** @type {boolean}*/
        private Int16 stage_check = 0;
        /** @type {string}*/
        private String get_hardware_hash = null;

        /**
         * Callback chamada quando o Launcher inicializa
         * 
         * @param {object} sender
         * @param {object} e
         */
        private void formPrincipal_Load(object sender, EventArgs e)
        {
            // -- Impedir mais de uma janela aberta do Launcher

            Int16 count = 0;

            foreach (var proc in Process.GetProcessesByName(Application.ProductName))
            {
                count++;

                if (count > 1)
                {
                    MessageBox.Show(@"Este programa só pode ser executado uma vez!");
                    Application.Exit();
                }
            }

            // -- Configuração inicial
            this.Icon = Properties.Resources.shied_icon;
            this.lblVersion.Text = "v." + ProductVersion;

            String getGtaPath = (String)Registry.GetValue(@"HKEY_CURRENT_USER\Software\SAMP", @"gta_sa_exe", null);
            if (getGtaPath == null)
                MessageBox.Show(@"A pasta de instalação do SAMP não foi encontrada. Busque manualmente.", @"Pasta não encontrada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                txtSampPath.Text = getGtaPath.Substring(0, getGtaPath.Length - 10);

            String getNick = (String)Registry.GetValue(@"HKEY_CURRENT_USER\Software\SAMP", @"PlayerName", "Player_AC");
            txtNickName.Text = getNick;
        }

        // -- Mover formulário
        /** @type {object} */
        private Point formLocation = Point.Empty;
        /** @type {boolean} */
        private Boolean formMoving = false;

        /**
         * Callback chamada quando o mouse encontra-se pressionado no formulário
         * 
         * @param {object} sender
         * @param {object} e
         */
        private void formPrincipal_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.formLocation = e.Location;
                this.formMoving = true;
            }
        }

        /**
         * Callback chamada quando o mouse encontra-se em movimento
         * 
         * @param {object} sender
         * @param {object} e
         */
        private void formPrincipal_MouseMove(object sender, MouseEventArgs e)
        {
            if (formMoving)
                this.Location = new Point(Cursor.Position.X - formLocation.X, Cursor.Position.Y - formLocation.Y);
        }

        /**
         * Callback chamada quando o mouse encontra-se pressionado no formulário
         * 
         * @param {object} sender
         * @param {object} e
         */
        private void formPrincipal_MouseUp(object sender, MouseEventArgs e)
        {
            formMoving = false;
        }

        // -- Caixa de controle
        /**
         * Callback chamada quando o botão btnMinimize é clicado
         * 
         * @param {object} sender
         * @param {object} e
         */
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        /**
         * Callback chamada quando o botão btnExit é clicado
         * 
         * @param {object} sender
         * @param {object} e
         */
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // -- Finalizar programa
        /**
         * Callback chamada quando o formulário está fechando
         * 
         * @param {object} sender
         * @param {object} e
         */
        private void formPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var proc in Process.GetProcessesByName("gta_sa"))
                proc.Kill();

            Environment.Exit(0);
        }

        // -- Abre a janela para buscar a pasta do jogo
        /**
         * Callback chamada quando o mouse encontra-se pressionado no na caixa de texto txtSampPath
         * 
         * @param {object} sender
         * @param {object} e
         */
        private void txtSampPath_MouseDown(object sender, MouseEventArgs e)
        {
            if (folderBrowserSampPath.ShowDialog() == DialogResult.OK)
                txtSampPath.Text = folderBrowserSampPath.SelectedPath + @"\";
        }

        // -- Busca arquivos suspeitos e inicia o jogo
        /**
         * Callback chamada quando o btnPlay é clicado
         * 
         * @param {object} sender
         * @param {object} e
         */
        private void btnPlay_Click(object sender, EventArgs e)
        {
            btnPlay.Enabled = false;
            txtSampPath.Enabled = false;
            txtNickName.Enabled = false;

            txtLog.Text += @"Adquirindo informações..." + Environment.NewLine;

            // -- Cria um hash do programa para checar se o mesmo não foi alterado
            Byte[] getBytesAC = File.ReadAllBytes(Environment.CurrentDirectory + @"\" + Application.ProductName + ".exe");
            MD5 MD5Hash = MD5.Create();
            Byte[] getComputedHash = MD5Hash.ComputeHash(getBytesAC);

            foreach (var x in getComputedHash)
                get_ac_hash += x.ToString("x2");

            // -- Pegar ID parcial de hardware
            get_hardware_hash = GetHardwareID();

            if (get_hardware_hash == null)
            {
                txtLog.BeginInvoke((Action)delegate () { txtLog.Text += @"Informações inconsistentes." + Environment.NewLine; });

                MessageBox.Show(@"Informações de hardware inconsistentes!" + Environment.NewLine + "@Execute este Launcher com privilégios administrativos e tente novamente.", @"Hardware ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                txtLog.BeginInvoke((Action)delegate () { txtLog.Text += @"Informações OK." + Environment.NewLine; });

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

                txtLog.BeginInvoke((Action)delegate () { txtLog.Text += @"Aguardando sua entrada no servidor..." + Environment.NewLine; });

                // -- Aguardar conexão do servidor
                socket_ac_pc.Bind(new IPEndPoint(IPAddress.Any, SOCKET_PORT));
                socket_ac_pc.Listen(1);
                socket_ac_pc.BeginAccept(new AsyncCallback(AcceptCallback), null);

                timerCheck.Start();
            }
        }

        // -- Verifica a pasta do GTA em busca de arquivos suspeitos
        /**
         * Callback chamada quando o temporizador timerCheck é acionado
         * 
         * @param {object} sender
         * @param {object} e
         */
        private void timerCheck_Tick(object sender, EventArgs e)
        {
            Int32 detectedFiles = GetSuspectFiles(txtSampPath.Text);
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
                }*/
            }

            if (countProcess > 1)
            {
                try
                {
                    Byte[] getData = Encoding.UTF8.GetBytes(@"cheat_" + CHEATCODE_MULTIPLE_EXEC + @"*id_" + GetPlayerInfo.Playerid);
                    receiveSocket.BeginSend(getData, 0, getData.Length, SocketFlags.None, new AsyncCallback(SendCallback), receiveSocket);
                }
                catch { }
            }
        }

        // -- Funções em thread para conexão
        /**
         * Callback chamada quando o servidor envia requisição de conexão
         * 
         * @param {object} ar
         */
        private void AcceptCallback(IAsyncResult ar)
        {
            Socket socket = socket_ac_pc.EndAccept(ar);

            // -- Loop
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            socket_ac_pc.BeginAccept(new AsyncCallback(AcceptCallback), socket);
        }

        /**
         * Callback chamada quando o servidor envia informações
         * 
         * @param {object} ar
         */
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                receiveSocket = (Socket)ar.AsyncState;
                int getDataLength = receiveSocket.EndReceive(ar);

                if (getDataLength == 0)
                {
                    txtLog.BeginInvoke((Action)delegate () { txtLog.Text += @"Servidor fechou a conexão!" + Environment.NewLine; });
                    MessageBox.Show(@"O servidor fechou a conexão com este computador.", @"Conexão encerrada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    Application.Exit();
                }
                else
                {
                    Byte[] getData = new Byte[getDataLength];
                    Byte[] sendData = new Byte[512];
                    Array.Copy(buffer, getData, getDataLength);

                    String encoded = Encoding.UTF8.GetString(getData);

                    if (stage_check == 0)
                    {
                        stage_check++;

                        Int32 idx1 = encoded.IndexOf('*');
                        GetPlayerInfo.Name = encoded.Substring(5, idx1 - 5);
                        GetPlayerInfo.Playerid = Convert.ToInt32(encoded.Substring(idx1 + 1, encoded.Length - idx1 - 1));

                        txtLog.BeginInvoke((Action)delegate () { txtLog.Text += @"Conectado ao servidor." + Environment.NewLine; });
                        txtLog.BeginInvoke((Action)delegate () { txtLog.Text += @"Enviando informações..." + Environment.NewLine; });

                        sendData = Encoding.UTF8.GetBytes(@"id_" + GetPlayerInfo.Playerid + @"*pchash_" + get_hardware_hash + "*achash_" + get_ac_hash + "*version_" + Application.ProductVersion);
                        receiveSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, new AsyncCallback(SendCallback), receiveSocket);

                        // -- Verifica se o nome que entrou no servidor é diferente do informado no Launcher
                        if (GetPlayerInfo.Name != txtNickName.Text)
                        {
                            Thread.Sleep(2000);

                            sendData = Encoding.UTF8.GetBytes(@"cheat_" + CHEATCODE_DIFF_NAME + @"*id_" + GetPlayerInfo.Playerid);
                            receiveSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, new AsyncCallback(SendCallback), receiveSocket);
                        }

                        txtLog.BeginInvoke((Action)delegate () { txtLog.Text += @"Informações enviadas."; });
                    }
                }

                // -- Loop
                receiveSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), receiveSocket);
            }
            catch
            {
                Environment.Exit(0);
            }
        }

        /**
         * Callback chamada quando o cliente envia uma resposta para o servidor
         * 
         * @param {object} ar
         */
        private void SendCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
        }

        // -- Funções que verificam arquivos suspeitos no diretório
        /**
         * Callback chamada para verificar arquivos suspeitos
         * 
         * @param {string} directory - Diretório do jogo
         */
        public Int32 GetSuspectFiles(string directory)
        {
            Int32 detectionCount = 0;

            Suspect susp = new Suspect();
            DirectoryInfo dirInfo = new DirectoryInfo(directory);

            foreach (var suspect_file_name in susp._suspect_files)
            {
                foreach (var get_file in dirInfo.GetFiles(suspect_file_name + @".*", SearchOption.AllDirectories))
                    detectionCount++;
            }
            return detectionCount;
        }

        /**
         * Callback chamada para verificar pastas suspeitas
         * 
         * @param {string} directory - Diretório do jogo
         */
        public Int32 GetSuspectFolders(string directory)
        {
            Int32 detectionCount = 0;

            Suspect susp = new Suspect();
            DirectoryInfo dirInfo = new DirectoryInfo(directory);

            foreach (var suspect_file_name in susp._suspect_folders)
            {
                foreach (var get_file in dirInfo.GetDirectories(suspect_file_name, SearchOption.AllDirectories))
                    detectionCount++;
            }
            return detectionCount;
        }

        // -- Pegar ID parcial de hardware
        /**
         * Callback chamada para pegar algumas informações de hardware do computador
         * 
         */
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
    }

    /**
     * Classe contendo informações de pastas e arquivos suspeitos
     * 
     * @property {string} _suspect_files - contém nomes de arquivos suspeitos
     * @property {string} _suspect_folders - contém nomes de pastas suspeitas
     */
    public class Suspect
    {
        // -- Pastas e arquivos suspeitos
        /** @type {string} */
        public string[] _suspect_files =
        {
            @"mod_sa"
        };

        /** @type {string} */
        public string[] _suspect_folders =
        {
            @"cleo"
        };
    }

    /**
     * Classe contendo informações de pastas e arquivos suspeitos
     * 
     * @property {string} Name - Nome do jogador
     * @property {number} Playerid - ID do jogador
     */
    public class GetPlayerInfo_
    {
        /** @type {string} */
        public string Name { get; set; }
        /** @type {number} */
        public int Playerid { get; set; }
    }
}
