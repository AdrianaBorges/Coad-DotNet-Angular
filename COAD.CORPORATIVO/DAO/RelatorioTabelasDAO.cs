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
    public class RelatorioTabelasDAO : AbstractGenericDao<RELATORIO_TABELAS, RelatorioTabelasDTO,int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public RelatorioTabelasDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public IList<RelatorioTabelasDTO> ListarRelatorioTabelasPorRelId(int? relId)
        {
            var query = (from reT in 
                             db.RELATORIO_TABELAS 
                         where reT.REL_ID == relId &&
                            reT.DATA_EXCLUSAO == null
                             select reT);
            return ToDTO(query);
        }
    }
}
