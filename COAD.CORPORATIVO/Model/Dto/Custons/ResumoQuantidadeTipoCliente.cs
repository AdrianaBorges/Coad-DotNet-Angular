using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ResumoQuantidadeTipoCliente
    {
        public ResumoQuantidadeTipoCliente()
        {
            QuantidadeClassificacaoClienteDTO = new HashSet<QuantidadeClassificacaoClienteDTO>();
        }

        public int Total { 
            get 
            {
                int total = 0;
                if(QuantidadeClassificacaoClienteDTO != null){

                
                    foreach(var quantidade in QuantidadeClassificacaoClienteDTO){

                        total += quantidade.QUANTIDADE;
                    }

                }
                return total;
            }
        }

        public IEnumerable<QuantidadeClassificacaoClienteDTO> QuantidadeClassificacaoClienteDTO { get; set; }
    }
}
