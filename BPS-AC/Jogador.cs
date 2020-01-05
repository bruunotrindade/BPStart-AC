using System;
using System.Collections;
using System.IO;

namespace BPS_AC
{
    public class Jogador
    {
        private string id;
        private string nick;

        public Jogador(string id, string nick)
        {
            this.Id = id;
            this.Nick = nick;
        }

        public string Id { get => id; set => id = value; }
        public string Nick { get => nick; set => nick = value; }
    }
}
