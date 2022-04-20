using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class NfeLinkDanfeDTO
    {
        public bool Servico { get; set; }
        public int? NumeroNota { get; set; }
        public string IE { get; set; }
        public string IM { get; set; }
        public string ChaveAcesso { get; set; }
        public string  CodigoVerificacao { get; set; }

        public string Link { get; set; }

        public override string ToString()
        {
            return Link;
        }
    }
}
