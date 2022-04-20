using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class RelatorioCondicaoDAO : DAOAdapter<RELATORIO_CONDICAO, RelatorioCondicaoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public RelatorioCondicaoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public IList<RelatorioCondicaoDTO> ListarRelatorioCondicao(int? relId)
        {
            var query = (from rec 
                             in db.RELATORIO_CONDICAO 
                         where rec.REL_ID == relId &&
                            rec.DATA_EXCLUSAO == null
                         select rec);

            return ToDTO(query);
        }

        public IList<RelatorioCondicaoDTO> ListarRelatorioCondicaoPorTabela(int? retId)
        {
            var query = (from rec
                             in db.RELATORIO_CONDICAO
                         where rec.RET_ID == retId &&
                            rec.DATA_EXCLUSAO == null
                         select rec);

            return ToDTO(query);
        }

    }
}