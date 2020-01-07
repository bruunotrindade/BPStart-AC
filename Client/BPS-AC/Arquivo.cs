/**
 * Brasil Play Start - Anti Cheater
 * Desenvolvido por: iHollyZinhO
 **/

using System;
using System.Collections;
using System.IO;

namespace BPS_AC
{
    public class Arquivo
    {
        private string nome;
        private string caminho;
        private float tamanho;

        public Arquivo(FileInfo file)
        {
            this.nome = file.Name;
            this.caminho = file.FullName;
            this.tamanho = file.Length / 1024;
        }

        public Arquivo(DirectoryInfo file)
        {
            this.nome = file.Name;
            this.caminho = file.FullName;
            this.tamanho = -1;
        }

        public string Caminho { get => caminho; set => caminho = value; }
        public float Tamanho { get => tamanho; set => tamanho = value; }
        public string Nome { get => nome; set => nome = value; }

        public bool Equals(Arquivo b)
        {
            Arquivo a = this;
            return a.Nome.Equals(b.Nome) && a.Caminho.Equals(b.Caminho) && a.Tamanho == b.Tamanho;
        }
    }
}
