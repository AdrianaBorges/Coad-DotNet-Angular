using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;

namespace COAD.CORPORATIVO.Service
{
    public class TipoProdComportamentoSRV : GenericService<TIPO_PROD_COMPORTAMENTO, TipoProdComportamentoDTO, int>
    {
        public TipoProdComportamentoDAO _dao = new TipoProdComportamentoDAO();

        public TipoProdComportamentoSRV()
        {
            Dao = _dao;
        }
    }
}
