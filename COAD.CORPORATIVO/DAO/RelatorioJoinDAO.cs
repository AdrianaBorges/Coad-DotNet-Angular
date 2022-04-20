
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
    public class RelatorioJoinDAO : AbstractGenericDao<RELATORIO_JOIN, RelatorioJoinDTO,int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public RelatorioJoinDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        
        public IList<RelatorioJoinDTO> ListarRelatorioJoinPorRelId(int? relId)
        {
            var query = (from jn in db.RELATORIO_JOIN 
                        where jn.REL_ID == relId &&
                            jn.DATA_EXCLUSAO == null
                        select jn);

            return ToDTO(query);
        }

        public IList<RelatorioJoinDTO> ListarRelatorioJoinPorTabelas(int? retId)
        {
            var query = (from jn in db.RELATORIO_JOIN
                         where (jn.RET_ID1 == retId || jn.RET_ID2 == retId)
                         &&
                             jn.DATA_EXCLUSAO == null
                         select jn);

            return ToDTO(query);
        }

    }
}
