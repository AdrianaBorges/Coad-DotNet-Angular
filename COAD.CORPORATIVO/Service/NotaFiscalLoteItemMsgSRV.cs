

using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using System.Web;
using System.IO;
using GenericCrud.Service;
using GenericCrud.Validations;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;

namespace COAD.CORPORATIVO.Service
{ 
	[ServiceConfig("NLM_ID")]
	public class NotaFiscalLoteItemMsgSRV : GenericService<NOTA_FISCAL_LOTE_ITEM_MSG, NotaFiscalLoteItemMsgDTO, Int32>
	{
        public NotaFiscalLoteItemMsgDAO _dao { get; set; }

        public NotaFiscalLoteItemMsgSRV(NotaFiscalLoteItemMsgDAO _dao)
        {
			this._dao = _dao;
			this.Dao = _dao;
        }

        public void SalvarNotaFiscalLoteItemMsg(NotaFiscalLoteItemDTO notaFiscalLoteItem)
        {
            if(notaFiscalLoteItem != null && 
                notaFiscalLoteItem.NOTA_FISCAL_LOTE_ITEM_MSG != null)
            {
                var lstMsg = notaFiscalLoteItem.NOTA_FISCAL_LOTE_ITEM_MSG;
                foreach(var msg in lstMsg)
                {
                    msg.NLI_ID = notaFiscalLoteItem.NLI_ID;
                }
                SaveOrUpdateAll(lstMsg);
            }
        }

    }
}
