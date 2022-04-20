using System.Collections.Generic;

namespace COAD.COADGED.Model.DTO
{
    public class ConsultaEmailStatusDTO
    {
        public ConsultaEmailStatusDTO()
        {
            this.CONSULTA_EMAIL = new HashSet<ConsultaEmailDTO>();
        }

        public int STS_ID { get; set; }
        public string STS_DESCRICAO { get; set; }

        public virtual ICollection<ConsultaEmailDTO> CONSULTA_EMAIL { get; set; }
    }
}