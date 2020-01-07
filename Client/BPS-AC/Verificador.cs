/**
 * Brasil Play Start - Anti Cheater
 * Desenvolvido por: iHollyZinhO
 **/

using System;
using System.Collections;
using System.IO;

namespace BPS_AC
{
    public class Verificador
    {
        private readonly string[] PASTAS_SUSPEITAS = { "mod_sa", "modsa" };
        private readonly string[] CONTEUDO_SUSPEITO = {"Cryptor", "Voron295", "Aimbot", "SPRINGFIELD", "UGBASE", "CROSSHAIR", "Wall", "Silent", "raknet", "CBUG", "OPCODEXE", "WALLHACK", "samp.dll", "speedhack", "hack"};
        private readonly ArrayList ARQUIVOS_LIBERADOS_ASI = new ArrayList();
        private string diretorio;

        public Verificador(string dir)
        {
            diretorio = dir;

            LiberarArquivos();
        }

        private void LiberarArquivos()
        {
            ARQUIVOS_LIBERADOS_ASI.Add(new Liberado("crashes.asi", new string[]{ "Whitetigerswt/gtasa_crashfix" }));
            ARQUIVOS_LIBERADOS_ASI.Add(new Liberado("audio.asi", new string[] { "audio.asi" }));
            ARQUIVOS_LIBERADOS_ASI.Add(new Liberado("CLEO.asi", new string[] { "cleo.log", "Dinkumware" }));
            ARQUIVOS_LIBERADOS_ASI.Add(new Liberado("modloader.asi", new string[] { "modloader.ini", "modloader.log" }));
            ARQUIVOS_LIBERADOS_ASI.Add(new Liberado("NormalMapFix.asi", new string[] { "TLOSS error", "SING error", "DOMAIN error" }));
            ARQUIVOS_LIBERADOS_ASI.Add(new Liberado("OutFitFix.asi", new string[] { "էRich", "TLOSS error", "SING error", "DOMAIN error" }));
            ARQUIVOS_LIBERADOS_ASI.Add(new Liberado("SAMPGraphicRestore.asi", new string[] { "TLOSS error", "SING error", "DOMAIN error" }));
            ARQUIVOS_LIBERADOS_ASI.Add(new Liberado("sensfix.asi", new string[] { "IsValidCodePage", "LCMapStringW", "GetUserDefaultLCID", "GetLocaleInfoA", "AVlogic_error", "Dinkumware" }));
            ARQUIVOS_LIBERADOS_ASI.Add(new Liberado("ShellFix.asi", new string[] { "TLOSS error", "SING error", "DOMAIN error" }));
            ARQUIVOS_LIBERADOS_ASI.Add(new Liberado("StreamMemFix.asi", new string[] { "TLOSS error", "SING error", "DOMAIN error" }));
            ARQUIVOS_LIBERADOS_ASI.Add(new Liberado("samp.asi", new string[] { "RefreshSize()", "Controls::Base:ImagePanel", "fontsize", "fontweight", "samp.dll", "san_andreas_multiplayer.dll", "disablekillchat" }));
        }

        /**
            Retorno - VerificarArquivoCLEO(string):
            int - Quantidade de palavras-chave encontradas 
        **/
        public int VerificarArquivoCLEO(FileInfo file)
        {
            int encontrados = 0;
            string arquivoPath = file.FullName;
            foreach (var line in File.ReadAllLines(arquivoPath))
                foreach (var chave in CONTEUDO_SUSPEITO)
                    if (line.ToLower().Contains(chave.ToLower()))
                        ++encontrados;

            return encontrados;
        }

        /**
            Retorno - VerificarArquivoASI(string):
            0 - Arquivo conforme o que foi liberado
            1 - Arquivo adulterado
            2 - Arquivo não liberado
        **/
        public int VerificarArquivoASI(FileInfo file)
        {
            int encontrados = 0;
            string arquivoNome = file.Name, arquivoPath = file.FullName;

            //Verificando se o arquivo passado está na lista de liberados
            foreach (Liberado liberado in ARQUIVOS_LIBERADOS_ASI)
            {
                if (liberado.Nome.Equals(arquivoNome))
                {
                    //Verificando a existência das palavras-chave
                    foreach (var line in File.ReadAllLines(arquivoPath))
                        foreach (var chave in liberado.Conteudo)
                            if (line.ToLower().Contains(chave.ToLower()))
                                encontrados += 1;

                    //Console.WriteLine($"ENCONTRADOS = {encontrados} / {liberado.Conteudo.Length}");
                    if (encontrados >= liberado.Conteudo.Length)
                        return 0;
                    
                    return 1;
                }
            }
            return 2;
        }

        public Auditoria BuscarArquivos()
        {
            Auditoria a = new Auditoria();
            DirectoryInfo dirInfo = new DirectoryInfo(diretorio);

            //Verificando todos os arquivos
            foreach(FileInfo arquivo in dirInfo.GetFiles("*", SearchOption.AllDirectories))
            {
                int val = -1;
                //Verificando os arquivos .cs encontrados
                if (arquivo.Extension.Equals(".cs"))
                {
                    val = VerificarArquivoCLEO(arquivo);
                    //Console.WriteLine($"{arquivo.FullName} resultado = {val}");
                    a.CLEO.Add(new Arquivo(arquivo));
                }
                //Verificando os arquivos .asi encontrados
                else if (arquivo.Extension.Equals(".asi"))
                {
                    val = VerificarArquivoASI(arquivo);
                    a.ASI.Add(new Arquivo(arquivo));
                }
                //Verificando os arquivos .sf encontrados
                else if (arquivo.Extension.Equals(".sf"))
                {
                    a.SF.Add(new Arquivo(arquivo));
                }

                if (val > 0)
                {
                    a.Suspeitos.Add(new Arquivo(arquivo));
                    a.LoginPermitido = false;
                }
            }

            //Verificando a existência de pastas suspeitas
            foreach(string pasta in PASTAS_SUSPEITAS)
            {
                foreach (DirectoryInfo p in dirInfo.GetDirectories(pasta, SearchOption.AllDirectories))
                    a.Suspeitos.Add(new Arquivo(p));
            }
            return a;
        }
    }
}
