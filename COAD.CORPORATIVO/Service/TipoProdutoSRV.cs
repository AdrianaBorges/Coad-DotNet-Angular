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
    public class TipoProdutoSRV : GenericService<TIPO_PRODUTO, TipoProdutoDTO, int>
    {
        private TipoProdutoDAO _dao;

        public TipoProdutoSRV()
        {
            this._dao = new TipoProdutoDAO();
            Dao = _dao;
        }

        public TipoProdutoSRV(TipoProdutoDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }
    }
}
