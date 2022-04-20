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
    public class LinhaProdutoSRV : GenericService<LINHA_PRODUTO, LinhaProdutoDTO, int>
    {
        private  LinhaProdutoDAO _dao = new LinhaProdutoDAO();

        public LinhaProdutoSRV()
        {
            Dao = _dao;
        }
    }

}
