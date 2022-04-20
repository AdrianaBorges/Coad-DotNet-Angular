

using Coad.GenericCrud.Dao.Base;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;

namespace COAD.COADGED.DAO
{
    public class ConsultaEmailStatusDAO : AbstractGenericDao<CONSULTA_EMAIL_STATUS, ConsultaEmailStatusDTO, string>
    {
        public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } }
        public ConsultaEmailStatusDAO() : base()
        {
            SetProfileName("GED");
            db = GetDb<COADGEDEntities>(false);
        }
    }
}
