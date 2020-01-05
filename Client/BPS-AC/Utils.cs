using System;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Management;
using System.Security.Cryptography;
using System.Diagnostics;

namespace BPS_AC
{
    public class Utils
    {
        public enum Codigos
        {
            ARQUIVOS_MODIFICADOS = 1,
            ARQUIVOS_SUSPEITOS = 2,
            AC_ADULTERADO = 3,
            AC_ANTIGO = 4,
            MULTIPLA_EXECUCAO = 5,
            FECHADO = 6
        };

        public enum Erros
        {
            ARQUIVOS_SUSPEITOS = 100,
            MULTIPLA_EXECUCAO = 101,
            HARDWARE_INCONSISTENTE = 102
        }

        //Gerar o hash do AC
        public static string GerarHashAC()
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
        public static string RecuperarHardwareID()
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

        //Transformar segundos em formato de tempo
        public static string FormatarTempo(int tempo)
        {
            int horas, minutos, segundos;
            horas = (int)(tempo / 3600);
            minutos = (int)((tempo % 3600) / 60);
            segundos = (tempo % 3600) % 60;
            return $"{horas:D2}:{minutos:D2}:{segundos:D2}";
        }

        public static void FecharSAMP()
        {
            foreach (var proc in Process.GetProcessesByName(@"samp"))
                proc.Kill();
        }
    }
}
