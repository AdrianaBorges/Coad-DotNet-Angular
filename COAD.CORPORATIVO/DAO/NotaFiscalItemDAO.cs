using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COAD.CORPORATIVO.DAO
{

    public class NotaFiscalItemDAO : AbstractGenericDao<NOTA_FISCAL_ITEM, NotaFiscalItemDTO, object>
    {
public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public NotaFiscalItemDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }

        public IList<NotaFiscalItemDTO> ListarNotaFiscalItensPorNota(int nfId)
        {
            var query = (from nfi
                            in db.NOTA_FISCAL_ITEM
                         where nfi.NF_ID == nfId
                         select nfi);
            return ToDTO(query);
        }
    }
}
