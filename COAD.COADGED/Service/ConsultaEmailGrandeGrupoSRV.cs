using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;

namespace COAD.COADGED.Service
{
    public class ConsultaEmailGrandeGrupoSRV : GenericService<CONSULTA_EMAIL_GRANDE_GRUPO, ConsultaEmailGrandeGrupoDTO, string>
    {
        private ConsultaEmailGrandeGrupoDAO _dao = new ConsultaEmailGrandeGrupoDAO();

        public ConsultaEmailGrandeGrupoSRV()
        {
            Dao = _dao;
        }
    }
}
