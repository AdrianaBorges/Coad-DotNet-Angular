

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
  using COAD.RM.Repositorios.Base;
using COAD.RM.DAO;
using COAD.RM.Model.Dto;
using COAD.RM.Repositorios.Contexto;

namespace COAD.RM.Service
{ 
	[ServiceConfig("CODCOLIGADA", "CHAPA", "ANOCOMP", "MESCOMP", "NROPERIODO")]
	public class PfperffcomplSRV : GenericService<PFPERFFCOMPL, PfperffcomplDTO, object>
	{

        public PfperffcomplDAO _dao { get; set; }

        public PfperffcomplSRV(PfperffcomplDAO _dao)
        {
			this._dao = _dao;
			this.Dao = _dao;
        }

		

    }
}
