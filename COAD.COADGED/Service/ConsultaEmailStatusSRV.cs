using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;

namespace COAD.COADGED.Service
{
    class ConsultaEmailStatusSRV : GenericService<CONSULTA_EMAIL_STATUS, ConsultaEmailStatusDTO, string>
    {
        private ConsultaEmailStatusDAO _dao = new ConsultaEmailStatusDAO();

        public ConsultaEmailStatusSRV()
        {
            Dao = _dao;
        }
    }
}
