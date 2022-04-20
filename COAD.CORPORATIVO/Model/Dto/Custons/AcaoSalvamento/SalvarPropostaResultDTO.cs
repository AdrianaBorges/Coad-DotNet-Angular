using COAD.CORPORATIVO.Model.Dto.Custons.Validacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.AcaoSalvamento
{
    public class SalvarPropostaResultDTO
    {
        public SalvarPropostaResultDTO()
        {
            this.ValidacoesItens = new HashSet<ValidacaoRegraPropostaItemDTO>();
        }

        public PropostaDTO PropostaSalva { get; set; }
        public bool EhValido
        {
            get
            {
                bool valido = true;
                if(ValidacoesItens != null && ValidacoesItens.Count > 0)
                {
                    foreach(var valid in ValidacoesItens)
                    {
                        valido = (valido && valid.EhValido);
                    }
                }
                return ((ValidacaoInadimplencia == null || !ValidacaoInadimplencia.ExisteInadimplencia) && valido);
            }
            set { }       
        
        }
        public ValidacaoClienteInadimplenteDTO ValidacaoInadimplencia { get; set; }
        public ICollection<ValidacaoRegraPropostaItemDTO> ValidacoesItens { get; set; }
    }
}
