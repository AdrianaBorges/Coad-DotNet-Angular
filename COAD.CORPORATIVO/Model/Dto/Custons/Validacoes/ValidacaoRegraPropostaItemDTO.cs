using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Validacoes
{
    public class ValidacaoRegraPropostaItemDTO
    {
        public ValidacaoRegraPropostaItemDTO()
        {
            Validacoes = new HashSet<ValidacaoRegraDetalhesDTO>();
        }
        public int? ppiId { get; set; }
        public string DescricaoProduto { get; set; }
        public ICollection<ValidacaoRegraDetalhesDTO> Validacoes { get; set; }

        public bool EhValido {
            get {

                return (Validacoes.Count <= 0);
            }
            private set { }
        }
    }
}
