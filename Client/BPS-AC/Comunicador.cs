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
        private int status = 0;
        private Byte[] buffer = new Byte[1024];

        //Atributos do cliente
        private Jogador player = null;
        private string hashPC = null;
        private string hashAC = null;

        enum Bandeiras
        {
            CONEXAO = 'C',
            DESCONEXAO = 'D',
            ALERTA = 'A'
        };

        public Jogador Player { get => player; set => player = value; }
        public int Status { get => status; set => status = value; }

        public Comunicador(string hashPC, string hashAC)
        {
            this.hashPC = hashPC;
            this.hashAC = hashAC;
        }

        public void Iniciar()
        {
            Console.WriteLine("Aguardando comunicação");

            socketCliente.Bind(new IPEndPoint(IPAddress.Any, SOCKET_PORTA));
            socketCliente.Listen(1);
            socketCliente.BeginAccept(new AsyncCallback(Inicio), null);
        }

        public void AlertarServidor(Utils.Codigos codigo)
        {
            if (codigo == Utils.Codigos.FECHADO)
                EnviarDados(Bandeiras.DESCONEXAO, new string[] { Player.Id });
            else
                EnviarDados(Bandeiras.ALERTA, new string[] { Player.Id, (int)codigo+"" });
        }

        private void EnviarDados(Bandeiras flag, string []args)
        {
            string dados = $"{(char)flag}|";
            foreach (string a in args)
                dados += $"{a}|";

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
                    status = 3;
                    Console.WriteLine("O servidor encerrou a conexão com o cliente.");
                }
                else
                {
                    Byte[] recebido = new Byte[getDataLength];
                    Array.Copy(buffer, recebido, getDataLength);

                    String encoded = Encoding.UTF8.GetString(recebido);
                    Console.WriteLine($"Dados recebidos = {encoded}");

                    //Primeira conexão
                    if (status == 0)
                    {
                        status = 1;
                        string []parts = encoded.Split('|');

                        Player = new Jogador(parts[0], parts[1]);
                        Console.WriteLine($"Nick = {Player.Nick}, ID = {Player.Id}");

                        EnviarDados(Bandeiras.CONEXAO, new string[] { Player.Id, hashPC, hashAC, versao});
                    }
                }

                // -- Loop
                socketServidor.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Recebimento), socketServidor);
            }
            catch
            {
                status = 4;
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
