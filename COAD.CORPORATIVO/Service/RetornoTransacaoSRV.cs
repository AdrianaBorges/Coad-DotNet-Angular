using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    public class RetornoTransacaoSRV : GenericService<RETORNO_TRANSACAO, RetornoTransacaoDTO, int>
    {
        public RetornoTransacaoDAO _dao = new RetornoTransacaoDAO();
 
        public RetornoTransacaoSRV()
        {
            this.Dao = _dao;
        }
    }
}
