using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas
{
    public class PesquisaImportacaoSuspectDTO
    {
        public PesquisaImportacaoSuspectDTO()
        {
            pagina = 1;
            registrosPorPagina = 7;
        }

        public string Nome { get; set; }
        public string CPF_CNPJ { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string EMail { get; set; }

        public string UF { get; set; }
        public string Bairro { get; set; }

        public string Regiao { get; set; }

        public int? ImsID { get; set; }
        public int? ImpID { get; set; }


        public int pagina { get; set; }
        public int registrosPorPagina { get; set; }
    }
}
