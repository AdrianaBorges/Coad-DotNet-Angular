

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
    public class NotaFiscalLoteItemTipoDAO : AbstractGenericDao<NOTA_FISCAL_LOTE_ITEM_TIPO, NotaFiscalLoteItemTipoDTO, Int32>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public NotaFiscalLoteItemTipoDAO()
        {
            db = GetDb<CORPORATIVOContext>(false);
        }

        
    }
}
