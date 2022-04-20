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
    public class RelatorioTabelaColunasDAO : AbstractGenericDao<RELATORIO_TABELA_COLUNAS, RelatorioTabelaColunasDTO,int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public RelatorioTabelaColunasDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public IList<RelatorioTabelaColunasDTO> ListarRelatorioTabelaColunasPorRetId(int? retId)
        {
            var query = (from relTbCol in
                             db.RELATORIO_TABELA_COLUNAS
                         where relTbCol.RET_ID == retId
                         orderby relTbCol.COR_ORDEM descending
                         select relTbCol);

            return ToDTO(query);
        }

        public IList<RelatorioTabelaColunasDTO> ListarRelatorioTabelaColunasPorRelatorioPersonalizado(int? relId)
        {
            var query = (from relTbCol in
                             db.RELATORIO_TABELA_COLUNAS
                         where relTbCol.REL_ID == relId
                         orderby relTbCol.COR_ORDEM descending
                         select relTbCol);

            return ToDTO(query);
        }
    }
}
