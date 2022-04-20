

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;

namespace COAD.CORPORATIVO.DAO
{
    public class NotaFiscalLoteItemMsgDAO : AbstractGenericDao<NOTA_FISCAL_LOTE_ITEM_MSG, NotaFiscalLoteItemMsgDTO, Int32>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public NotaFiscalLoteItemMsgDAO()
        {
            db = GetDb<CORPORATIVOContext>(false);
        }

        
    }
}
