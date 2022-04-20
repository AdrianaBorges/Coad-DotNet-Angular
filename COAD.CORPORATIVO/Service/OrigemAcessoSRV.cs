
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
using GenericCrud.Config.DataAttributes;
using Coad.GenericCrud.Dao.Base.Pagination;
using System.Transactions;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("OAC_ID")]
    public class OrigemAcessoSRV : GenericService<ORIGEM_ACESSO, OrigemAcessoDTO, int>
    {
        public OrigemAcessoDAO _dao = new OrigemAcessoDAO();

        public OrigemAcessoSRV()
        {
            this.Dao = _dao;
        }

        public IList<OrigemAcessoDTO> ListarPorNome(string nome)
        {
            return _dao.ListarPorNome(nome);
        }
        public Pagina<OrigemAcessoDTO> ListarPorNome(string nome, int _pagina = 1, int _itensPorPagina = 10)
        {
            return _dao.ListarPorNome(nome, _pagina, _itensPorPagina);
        }
    

      
    }
}
