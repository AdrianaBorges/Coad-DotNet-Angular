using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;

namespace COAD.COADGED.Service
{
    public class ConsultaEmailVerbeteSRV : GenericService<CONSULTA_EMAIL_VERBETE, ConsultaEmailVerbeteDTO, string>
    {
        private ConsultaEmailVerbeteDAO _dao = new ConsultaEmailVerbeteDAO();

        public ConsultaEmailVerbeteSRV()
        {
            Dao = _dao;
        }
    }
}
