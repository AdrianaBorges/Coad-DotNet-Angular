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
    public class TipoProdutoComposicaoSRV : GenericService<TIPO_PRODUTO_COMPOSICAO, TipoProdutoComposicaoDTO, int>
    {
        private TipoProdutoComposicaoDAO _dao;
       
        public TipoProdutoComposicaoSRV()
        {
            this._dao = new TipoProdutoComposicaoDAO();
            Dao = _dao;
        }

        public TipoProdutoComposicaoSRV(TipoProdutoComposicaoDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }
    }
}
