using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    public class TotalVendasCartaoSRV : ServiceAdapter<TOTAL_VENDAS_CARTAO, TotalVendasCartaoDTO>
    {

        public TotalVendasCartaoDAO _dao = new TotalVendasCartaoDAO();

        public TotalVendasCartaoSRV()
        {
            SetDao(_dao);
        }
 
        public IList<TotalVendasCartaoDTO> Buscar(int _for_id)
        {
            return _dao.Buscar(_for_id);
        }
        public IList<TotalVendasCartaoDTO> Buscar(int _mesatual, int _anoatual)
        {
            return _dao.Buscar(_mesatual, _anoatual);
        }
        public IList<TotalVendasCartaoDTO> Buscar(int _emp_id, int _mesatual, int _anoatual)
        {
            return _dao.Buscar(_emp_id, _mesatual, _anoatual);
        }
        public Pagina<TotalVendasCartaoDTO> BuscarPaginas(int _for_id, int numpagina = 1, int linhas = 7)
        {
            return _dao.BuscarPaginas(_for_id, numpagina, linhas);
        }
        public IList<FornecedorDTO> BuscarFornecedor(int _emp_id, int _mesatual, int _anoatual)
        {
            return _dao.BuscarFornecedor(_emp_id, _mesatual, _anoatual);
        }
        public Pagina<TotalVendasCartaoDTO> BuscarPorEmpresa(int? _em_id, int? _mesatual, int? _anoatual, int numpagina = 1, int linhas = 7)
        {
            return _dao.BuscarPorEmpresa(_em_id, _mesatual, _anoatual, numpagina, linhas);
        }
        public TotalVendasCartaoDTO BuscarPorEmpresa(int _em_id, int _for_id, int _mesatual, int _anoatual)
        {
            return _dao.BuscarPorEmpresa(_em_id,_for_id, _mesatual, _anoatual);
        }
    }
}
