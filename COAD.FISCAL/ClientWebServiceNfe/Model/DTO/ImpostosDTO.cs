using COAD.FISCAL.Model.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.DTO
{
    public class ImpostosDTO
    {
        public int Ordem {
            get {

                switch (TipoImposto)
                {
                    case ImpostosEnum.COFINS: return 0;
                    case ImpostosEnum.CSLL: return 1;
                    case ImpostosEnum.INSS: return 2;
                    case ImpostosEnum.IR: return 3;
                    case ImpostosEnum.PIS: return 4;
                    default: return 1000;
                }
            }
        }
        public ImpostosEnum TipoImposto { get; set; }
        public decimal? Aliguota { get; set; }
        public decimal? ValorDesconto { get; set; }
        public string NomeImposto
        {
            get
            {

                switch (TipoImposto)
                {
                    case ImpostosEnum.COFINS: return "COFINS";
                    case ImpostosEnum.CSLL: return "CSLL";
                    case ImpostosEnum.INSS: return "INSS";
                    case ImpostosEnum.IR: return "IRRF";
                    case ImpostosEnum.PIS: return "PIS";
                    default: return "Outros";
                }
            }
        }
    }
}
