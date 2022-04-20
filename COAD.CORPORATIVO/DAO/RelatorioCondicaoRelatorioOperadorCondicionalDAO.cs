using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Model.Dto.Custons;
namespace COAD.CORPORATIVO.DAO
{
    public class RelatorioCondicaoRelatorioOperadorCondicionalDAO : DAOAdapter<RELATORIO_CONDICAO_RELATORIO_OPERADOR_CONDICIONAL, RelatorioCondicaoRelatorioOperadorCondicionalDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public RelatorioCondicaoRelatorioOperadorCondicionalDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public IList<RelatorioCondicaoRelatorioOperadorCondicionalDTO> ListarRelatorioCondicaoOperadorCondicionalPorRecId(int? recId)
        {
            var query = (from x in db.RELATORIO_CONDICAO_RELATORIO_OPERADOR_CONDICIONAL 
                             where x.REC_ID == recId
                             select x);
            return ToDTO(query);

        }

    }
}