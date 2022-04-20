using Coad.GenericCrud.Dao.Base;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.DAO
{

    public class ManualDPModuloDAO : AbstractGenericDao<MANUAL_DP_MODULO, ManualDPModuloDTO, int>
    {
        public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 
        public ManualDPModuloDAO()
            : base()
        {
            SetProfileName("GED");

            db = GetDb<COADGEDEntities>(false);
        }
        public IList<ManualDPModuloDTO> BuscarModulos()
        {
            var query = (from i in db.MANUAL_DP_MODULO
                         select i).OrderBy(x => x.MOD_DESCRICAO);

            return ToDTO(query);

        }
    }

}
