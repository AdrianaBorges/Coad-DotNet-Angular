using System.Collections.Generic;

namespace COAD.COADGED.Model.DTO
{
    public class ConsultaEmailGrandeGrupoDTO
    {
        public ConsultaEmailGrandeGrupoDTO()
        {
            this.CONSULTA_EMAIL = new HashSet<ConsultaEmailDTO>();
        }

        public int GRG_ID { get; set; }
        public string GRG_DESCRICAO { get; set; }
        public int COLEC_ID { get; set; }

        public virtual ICollection<ConsultaEmailDTO> CONSULTA_EMAIL { get; set; }
        public virtual ConsultaEmailColecionadorDTO CONSULTA_EMAIL_COLECIONADOR { get; set; }
    }
}