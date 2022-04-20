using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class RastreamentoAlteracaoLoginUnicoDTO
    {
        public int? CodClienteAnterior { get; set; }
        public string CodAssinatura { get; set; }
        public int? CodClienteRecebido { get; set; }
        
    }
}
