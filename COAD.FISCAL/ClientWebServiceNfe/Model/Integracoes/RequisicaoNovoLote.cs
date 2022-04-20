using Coad.GenericCrud.Validations;
using COAD.FISCAL.Model.DTO;
using COAD.FISCAL.Model.Integracoes.Enumerados;
using COAD.SEGURANCA.Service.Custons.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.Integracoes
{
    public class RequisicaoNovoLote
    {
        public RequisicaoNovoLote()
        {
            LstRequisicoes = new HashSet<RequisicaoNovoLoteItem>();
            TipoLote = TipoLoteEnum.ENVIO_LOTE_NFE;

        }

        [RequiredList(1, ErrorMessage = "Informe pelo menos um Código de Pedido.")]
        [Required(ErrorMessage = "Informe a lista de Códigos do Pedido")]
        public ICollection<RequisicaoNovoLoteItem> LstRequisicoes { get; set; }
        public TipoLoteEnum TipoLote { get; set; }

        public BatchContext BatchContext { get; set; }

        [Required(ErrorMessage = "Informe o Código da Empresa")]
        public int? EmpresaID { get; set; }

    }
}
