

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
using COAD.CORPORATIVO.Model.DTO;
using COAD.CORPORATIVO.Repositorios.Contexto;

using COAD.CORPORATIVO.Model.Dto;

namespace COAD.CORPORATIVO.Service
{ 
	[ServiceConfig("PEF_ANO", "PEF_MES", "PEF_SEMANA")]
    public class PeriodoFaturamentoSRV : GenericService<PERIODO_FATURAMENTO, PeriodoFaturamentoDTO, object>
	{
        private PeriodoFaturamentoDAO _dao = new PeriodoFaturamentoDAO();

        public PeriodoFaturamentoSRV()
        {
             this.Dao = _dao;
        }

        public List<PeriodoFaturamentoDTO> ListarSemanas(int _PEF_MES, int _PEF_ANO)
        {
            return _dao.ListarSemanas(_PEF_MES, _PEF_ANO).ToList();
        }

    }
}
