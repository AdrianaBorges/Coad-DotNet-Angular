using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Transferencia
{
    public class RepresentanteTransferenciaDTO
    {
        public int? REP_ID { get; set; }
        public string NOME { get; set; }
        public bool MOSTRAR { get; set; }
        public int QtdClientesTransferidos { get; set; }

        public RepresentanteTransferenciaDTO()
        {
            MOSTRAR = true;
        }
    }
   
}
