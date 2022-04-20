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
    public class LinhaProdutoInformativoSRV : GenericService<LINHA_PRODUTO_INFORMATIVO, LinhaProdutoInformativoDTO, int>
    {
        private  LinhaProdutoInformativoDAO  _dao = new LinhaProdutoInformativoDAO();

        public LinhaProdutoInformativoSRV()
        {
            Dao = _dao;
        }

        public IList<LinhaProdutoInformativoDTO> ListarInformativo(int _LinhaProduto)
        {
            return _dao.ListarInformativo(_LinhaProduto);
        }

    }
}
