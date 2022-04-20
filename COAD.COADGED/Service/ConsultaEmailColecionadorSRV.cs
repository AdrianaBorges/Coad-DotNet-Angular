using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;

namespace COAD.COADGED.Service
{
    public class ConsultaEmailColecionadorSRV : GenericService<CONSULTA_EMAIL_COLECIONADOR, ConsultaEmailColecionadorDTO, int>
    {
        private ConsultaEmailColecionadorDAO _dao = new ConsultaEmailColecionadorDAO();

        public ConsultaEmailColecionadorSRV()
        {
            Dao = _dao;
        }
    }
}
