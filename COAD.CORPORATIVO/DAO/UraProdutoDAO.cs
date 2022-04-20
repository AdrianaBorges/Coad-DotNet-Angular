using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COAD.CORPORATIVO.DAO
{
    public class UraProdutoDAO : DAOAdapter<URA_PRODUTO, UraProdutoDTO>
    {
public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public UraProdutoDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }
        public IList<UraProdutoDTO> BuscarProdutos(string _ura_id)
        {
            IQueryable<URA_PRODUTO> query = GetDbSet();
            query = query.Where(x => x.URA_ID == _ura_id);

            return ToDTO(query);
        }

    }
}
