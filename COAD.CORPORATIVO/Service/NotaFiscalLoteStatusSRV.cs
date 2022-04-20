

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
	[ServiceConfig("NLS_ID")]
	public class NotaFiscalLoteStatusSRV : GenericService<NOTA_FISCAL_LOTE_STATUS, NotaFiscalLoteStatusDTO, Int32>
	{

        public NotaFiscalLoteStatusDAO _dao { get; set; }

        public NotaFiscalLoteStatusSRV(NotaFiscalLoteStatusDAO _dao)
        {
			this._dao = _dao;
			this.Dao = _dao;
        }

		

    }
}
