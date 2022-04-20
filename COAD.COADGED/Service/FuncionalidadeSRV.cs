using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Service
{
    [ServiceConfig("FCI_ID")]
    public class FuncionalidadeSRV : GenericService<FUNCIONALIDADE, FuncionalidadeDTO, int>
    {
        private FuncionalidadeDAO _dao = new FuncionalidadeDAO();

        public FuncionalidadeSRV()
        {
            Dao = _dao;
        }
        public IList<FuncionalidadeDTO> ListarPorReferencia(string _tdc_id)
        {
            return _dao.ListarPorReferencia(_tdc_id);
        }
        public Pagina<FuncionalidadeDTO> ListarFuncionalidades(string _descricao = null, int _pagina = 1, int _itensPorPagina = 10)
        {
            return _dao.ListarFuncionalidades(_descricao, _pagina, _itensPorPagina);
        }
        public IList<FuncionalidadeDTO> ListarFuncionalidadesNaoSelect(int? _origem)
        {
            return _dao.ListarFuncionalidadesNaoSelect(_origem);
        }
    }
}