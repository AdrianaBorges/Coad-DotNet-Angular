using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;


namespace COAD.CORPORATIVO.DAO
{
    public class AreaConsultoriaRepresentanteDAO : AbstractGenericDao<AREA_CONSULTORIA_REPRESENTANTE, AreaConsultoriaRepresentanteDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public AreaConsultoriaRepresentanteDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public IList<AreaConsultoriaRepresentanteDTO> FindByRepId(int? repId)
        {
            var query = GetDbSet().Where(x => x.REP_ID == repId);
            return ToDTO(query);
        }

    }
}
