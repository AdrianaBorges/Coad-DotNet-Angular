using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("URA_ID", "PRO_ID")]
    public class UraProdutoSRV : ServiceAdapter<URA_PRODUTO, UraProdutoDTO>
    {
        public UraProdutoDAO _dao = new UraProdutoDAO();

        public UraProdutoSRV()
        {
            SetDao(_dao);
        }
        public IList<UraProdutoDTO> BuscarProdutos(string _ura_id)
        {
            return _dao.BuscarProdutos(_ura_id);
        }

    }
}
