using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;

namespace COAD.COADGED.Service
{
    public class ConsultaEmailSRV : GenericService<CONSULTA_EMAIL, ConsultaEmailDTO, string>
    {
        private ConsultaEmailDAO _dao = new ConsultaEmailDAO();

        public ConsultaEmailSRV()
        {
            Dao = _dao;
        }
    }
}
