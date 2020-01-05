using System;
using System.Collections;
using System.IO;

namespace BPS_AC
{
    public class Auditoria
    {
        private ArrayList cleo = new ArrayList();
        private ArrayList asi = new ArrayList();
        private ArrayList sf = new ArrayList();
        private ArrayList suspeitos = new ArrayList();
        private bool loginPermitido = true;

        public ArrayList CLEO { get => cleo; set => cleo = value; }
        public ArrayList ASI { get => asi; set => asi = value; }
        public ArrayList Suspeitos { get => suspeitos; set => suspeitos = value; }
        public ArrayList SF { get => sf; set => sf = value; }
        public bool LoginPermitido { get => loginPermitido; set => loginPermitido = value; }

        public bool Equals(Auditoria b)
        {
            Auditoria a = this;
            return comparar(a.ASI, b.ASI) && comparar(a.CLEO, b.CLEO) && comparar(a.SF, b.SF);
        }

        private static bool comparar(ArrayList a, ArrayList b)
        {
            ArrayList listaA = a, listaB = b;
            if (b.Count > a.Count)
            {
                listaA = b;
                listaB = a;
            }

            foreach (Arquivo arqA in listaA)
            {
                bool encontrado = false;
                foreach (Arquivo arqB in listaB)
                {
                    if (arqA.Equals(arqB))
                    {
                        encontrado = true;
                        break;
                    }
                }
                if (!encontrado)
                    return false;
            }
            return true;
        }
    }
}
