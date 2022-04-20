using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class ProdutoFornecedorDAO : RepositorioCorp<PRODUTO_FORNECEDOR>
    {
        public PRODUTO_FORNECEDOR VerficarIncluir(PRODUTO_FORNECEDOR _profor)
        {
            var _produtofornecedor = this.Buscar(_profor.PRO_ID, _profor.FOR_ID);

            if (_produtofornecedor == null)
                this.Incluir(_profor);

            return _produtofornecedor;
        }
        public PRODUTO_FORNECEDOR Buscar(int _pro_id, int _for_id)
        {
            var _produtofornecedor = (from pf in db.PRODUTO_FORNECEDOR
                                      where pf.PRO_ID == _pro_id && pf.FOR_ID == _for_id
                                      select pf).FirstOrDefault();

            return _produtofornecedor;
        }
    }
}
