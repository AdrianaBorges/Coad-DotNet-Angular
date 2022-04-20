

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

using COAD.SEGURANCA.DAO;
using COAD.SEGURANCA.Model.Dto;
using COAD.SEGURANCA.Repositorios.Contexto;

namespace COAD.SEGURANCA.Service
{ 
	[ServiceConfig("CNT_ID")]
	public class ContabilistaSRV : GenericService<CONTABILISTA, ContabilistaDTO, Int32>
	{
        public ContabilistaDAO _dao = new ContabilistaDAO();

        public ContabilistaSRV()
        {
            this.Dao = _dao;
        }


    }
}
