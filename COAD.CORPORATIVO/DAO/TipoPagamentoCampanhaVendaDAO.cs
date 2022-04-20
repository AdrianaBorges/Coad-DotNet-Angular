using Coad.GenericCrud.Dao.Base.Pagination;
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
    public class TipoPagamentoCampanhaVendaDAO : DAOAdapter<TIPO_PAGAMENTO_CAMPANHA_VENDA, TipoPagamentoCampanhaVendaDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public TipoPagamentoCampanhaVendaDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        
        public ICollection<TipoPagamentoCampanhaVendaDTO> ListarTipoPagamentoCampanhaVenda(int? cveId)
        {
            var query = (from
                            tpPaCamV in db.TIPO_PAGAMENTO_CAMPANHA_VENDA
                         where
                            tpPaCamV.CVE_ID == cveId
                         select tpPaCamV);

            return ToDTO(query);
        }

    }
}
