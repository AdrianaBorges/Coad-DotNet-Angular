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
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model;

namespace COAD.CORPORATIVO.Service
{
    public class CondicaoPagamentoSRV : GenericService<CONDICAO_PAGAMENTO, CondicaoPagamentoDTO, int>
    {
        private CondicaoPagamentoDAO _dao = new CondicaoPagamentoDAO();

        public CondicaoPagamentoSRV()
        {
            Dao = _dao;
        }
    }
}
