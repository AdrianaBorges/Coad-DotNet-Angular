using GenericCrud.Models.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model.Custons.Pesquisas
{
    public class PesquisarFilaEmailDTO
    {
        public int? FilaId { get; set; }
        public string Email { get; set; }
        public string Assunto { get; set; }
        public DateTime? DataCriacaoInicial { get; set; }
        public DateTime? DataCriacaoFinal { get; set; }
        public DateTime? DataEnvioInicial { get; set; }
        public DateTime? DataEnvioFinal { get; set; }
        public bool ExibirEnviados { get; set; }
        public bool ExibirCancelados { get; set; }
        public string Usuario { get; set; }

        public RequisicaoPaginacao requisicao { get; set; }
    }
}
