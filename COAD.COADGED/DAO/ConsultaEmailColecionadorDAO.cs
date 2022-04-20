using Coad.GenericCrud.Dao.Base;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;

namespace COAD.COADGED.DAO
{
    public class ConsultaEmailColecionadorDAO : AbstractGenericDao<CONSULTA_EMAIL_COLECIONADOR, ConsultaEmailColecionadorDTO, int>
    {
        public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } }
        public ConsultaEmailColecionadorDAO() : base()
        {
            SetProfileName("GED");
            db = GetDb<COADGEDEntities>(false);
        }
    }
}
