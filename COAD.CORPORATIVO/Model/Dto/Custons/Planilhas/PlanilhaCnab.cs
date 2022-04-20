using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Planilhas
{
    public class PlanilhaCnab
    {
        public int? Codigo { get; set; }
        public string Campo { get; set; }
        public string Tipo { get; set; }
        public int? PosicaoInicial { get; set; }
        public int? PosicaoFinal { get; set; }
        public string Conteudo { get; set; }
        public int? Tamanho { get; set; }
        public int? Decimais { get; set; }
    }
}
