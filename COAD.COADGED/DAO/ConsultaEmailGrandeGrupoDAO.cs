using Coad.GenericCrud.Dao.Base;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;

namespace COAD.COADGED.DAO
{
    public class ConsultaEmailGrandeGrupoDAO : AbstractGenericDao<CONSULTA_EMAIL_GRANDE_GRUPO, ConsultaEmailGrandeGrupoDTO, string>
    {
        public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } }
        public ConsultaEmailGrandeGrupoDAO() : base()
        {
            SetProfileName("GED");
            db = GetDb<COADGEDEntities>(false);
        }
    }
}
