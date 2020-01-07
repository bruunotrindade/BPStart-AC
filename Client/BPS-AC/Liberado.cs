/**
 * Brasil Play Start - Anti Cheater
 * Desenvolvido por: iHollyZinhO
 **/

using System;
using System.Collections;
using System.IO;

namespace BPS_AC
{
    public class Liberado
    {
        private string nome;
        private string[] conteudo;

        public Liberado(string nome, string[] conteudo)
        {
            this.nome = nome;
            this.conteudo = conteudo;
        }

        public string Nome { get => nome; set => nome = value; }
        public string[] Conteudo { get => conteudo; set => conteudo = value; }
    }
}
