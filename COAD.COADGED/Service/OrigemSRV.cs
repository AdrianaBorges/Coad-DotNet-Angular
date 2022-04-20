using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Service
{
    public class OrigemSRV : GenericService<ORIGEM_FUNCIONALIDADE, OrigemFuncionalidadeDTO, int>
    {
        private OrigemDAO _dao = new OrigemDAO();

        public OrigemSRV()
        {
            Dao = _dao;
        }
        public Pagina<OrigemFuncionalidadeDTO> ListarOrigem(string _descricao = null, int _pagina = 1, int _itensPorPagina = 10)
        {
            return _dao.ListarOrigem(_descricao, _pagina, _itensPorPagina);
        }

    }
}
