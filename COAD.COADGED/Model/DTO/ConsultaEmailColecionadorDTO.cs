using System.Collections.Generic;

namespace COAD.COADGED.Model.DTO
{
    public class ConsultaEmailColecionadorDTO
    {
        public ConsultaEmailColecionadorDTO()
        {
            this.CONSULTA_EMAIL = new HashSet<ConsultaEmailDTO>();
            this.CONSULTA_EMAIL_GRANDE_GRUPO = new HashSet<ConsultaEmailGrandeGrupoDTO>();
            this.CONSULTA_EMAIL_PERFIL_COLEC = new HashSet<ConsultaEmailPerfilColecDTO>();
            this.CONSULTA_EMAIL_VERBETE = new HashSet<ConsultaEmailVerbeteDTO>();
        }

        public int COLEC_ID { get; set; }
        public string COLEC_DESCRICAO { get; set; }

        public virtual ICollection<ConsultaEmailDTO> CONSULTA_EMAIL { get; set; }
        public virtual ICollection<ConsultaEmailGrandeGrupoDTO> CONSULTA_EMAIL_GRANDE_GRUPO { get; set; }
        public virtual ICollection<ConsultaEmailPerfilColecDTO> CONSULTA_EMAIL_PERFIL_COLEC { get; set; }
        public virtual ICollection<ConsultaEmailVerbeteDTO> CONSULTA_EMAIL_VERBETE { get; set; }
    }
}