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
    public class Comunicador
    {
        //Constantes do software
        private readonly string versao = Application.ProductVersion;
        private readonly int SOCKET_PORTA = 6000;

        //Atributos da comunicação
        private Socket socketCliente = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Socket socketServidor = null;
        private bool iniciado = false;
        private Byte[] buffer = new Byte[1024];

        //Atributos do cliente
        private Jogador player = null;
        private string hashPC = null;
        private string hashAC = null;

        public Jogador Player { get => player; set => player = value; }

        enum Codigos
        {
            ARQUIVOS_MODIFICADOS = 1
        }

        public Comunicador(string hashPC, string hashAC)
        {
            this.hashPC = hashPC;
            this.hashAC = hashAC;
        }

        public void Iniciar()
        {
            Console.WriteLine("Comunicação iniciada");

            socketCliente.Bind(new IPEndPoint(IPAddress.Any, SOCKET_PORTA));
            socketCliente.Listen(1);
            socketCliente.BeginAccept(new AsyncCallback(Inicio), null);
        }

        public void EnviarDados(string dados)
        {
            Byte[] bytesEnvio = Encoding.UTF8.GetBytes(dados);
            socketServidor.BeginSend(bytesEnvio, 0, bytesEnvio.Length, SocketFlags.None, new AsyncCallback(Envio), socketServidor);
        }

        //Callback de conexão iniciada
        private void Inicio(IAsyncResult ar)
        {
            Console.WriteLine("Servidor iniciou a comunicação");
            socketServidor = socketCliente.EndAccept(ar);

            // -- Loop
            socketServidor.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Recebimento), socketServidor);
            socketCliente.BeginAccept(new AsyncCallback(Inicio), socketServidor);
        }

        //Callback de pacote recebido
        private void Recebimento(IAsyncResult ar)
        {
            try
            {
                socketServidor = (Socket)ar.AsyncState;
                int getDataLength = socketServidor.EndReceive(ar);

                if (getDataLength == 0)
                {
                    Console.WriteLine("O servidor encerrou a conexão com o cliente.");
                    Application.Exit();
                }
                else
                {
                    Byte[] recebido = new Byte[getDataLength];
                    Array.Copy(buffer, recebido, getDataLength);

                    String encoded = Encoding.UTF8.GetString(recebido);
                    Console.WriteLine($"Dados recebidos = {encoded}");

                    //Primeira conexão
                    if (iniciado == false)
                    {
                        iniciado = true;
                        int idx1 = encoded.IndexOf('*');

                        Player = new Jogador(encoded.Substring(idx1 + 1, encoded.Length - idx1 - 1), encoded.Substring(5, idx1 - 5));
                        Console.WriteLine($"Nick = {Player.Nick}, ID = {Player.Id}");

                        EnviarDados($"id_{Player.Id}*pchash_{hashPC}*achash_{hashAC}*version_{versao}");
                        

                        // -- Verifica se o nome que entrou no servidor é diferente do informado no Launcher
                        /*if (GetPlayerInfo.Name != txtNickName.Text)
                        {
                            Thread.Sleep(2000);

                            enviado = Encoding.UTF8.GetBytes(@"cheat_" + CHEATCODE_DIFF_NAME + @"*id_" + GetPlayerInfo.Playerid);
                            receiveSocket.BeginSend(enviado, 0, enviado.Length, SocketFlags.None, new AsyncCallback(Envio), receiveSocket);
                        }

                        txtLog.BeginInvoke((Action)delegate () { txtLog.Text += @"Informações enviadas."; });*/
                    }
                }

                // -- Loop
                socketServidor.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Recebimento), socketServidor);
            }
            catch
            {
                Environment.Exit(0);
            }
        }

        //Callback de pacote enviado
        private void Envio(IAsyncResult ar)
        {
            Console.WriteLine("Cliente enviou ao servidor");

            Socket socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
        }
    }
}
