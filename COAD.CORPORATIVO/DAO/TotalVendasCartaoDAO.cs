using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class TotalVendasCartaoDAO : DAOAdapter<TOTAL_VENDAS_CARTAO, TotalVendasCartaoDTO>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public TotalVendasCartaoDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }

        public IList<TotalVendasCartaoDTO> Buscar(int _for_id)
        {

            var _lista = (from t in db.TOTAL_VENDAS_CARTAO
                          where t.FOR_ID == _for_id
                          orderby t.TVC_ANO, t.TVC_MES descending
                          select t);

            return ToDTO(_lista);
        }
        public IList<TotalVendasCartaoDTO> Buscar(int _mesatual, int _anoatual)
        {
            var _lista = (from t in db.TOTAL_VENDAS_CARTAO
                          where t.TVC_MES == _mesatual && t.TVC_ANO == _anoatual
                          orderby t.TVC_ANO, t.TVC_MES descending
                          select t);

            return ToDTO(_lista);

        }

        public IList<TotalVendasCartaoDTO> Buscar(int _emp_id, int _mesatual, int _anoatual)
        {
            var _lista = (from t in db.TOTAL_VENDAS_CARTAO
                          where t.EMP_ID == _emp_id && t.TVC_MES == _mesatual && t.TVC_ANO == _anoatual
                          orderby t.TVC_ANO, t.TVC_MES descending
                          select t);

            return ToDTO(_lista);

        }

        public IList<FornecedorDTO> BuscarFornecedor(int _em_id, int _mesatual, int _anoatual)
        {
            var _lista = (from t in db.TOTAL_VENDAS_CARTAO
                          where t.TVC_MES == _mesatual && t.TVC_ANO == _anoatual && t.EMP_ID == _em_id  
                          orderby t.TVC_ANO, t.TVC_MES descending
                          select t.FORNECEDOR).Distinct();

            return Convert<IQueryable<FORNECEDOR>, List<FornecedorDTO>>(_lista);

        }


        public Pagina<TotalVendasCartaoDTO> BuscarPaginas(int _for_id, int numpagina = 1, int linhas = 7)
        {
            var _lista = (from t in db.TOTAL_VENDAS_CARTAO
                          where t.FOR_ID == _for_id
                          orderby t.TVC_ANO, t.TVC_MES descending
                          select t);

            return ToDTOPage(_lista, numpagina, linhas);
        }
        public Pagina<TotalVendasCartaoDTO> BuscarPorEmpresa(int? _em_id, int? _mesatual, int? _anoatual, int numpagina = 1, int linhas = 7)
        {
            var _lista = (from t in db.TOTAL_VENDAS_CARTAO
                          where t.EMP_ID == _em_id &&
                                ((_mesatual == null) || (_mesatual != null && t.TVC_MES == _mesatual)) &&
                                ((_anoatual == null) || (_anoatual != null && t.TVC_ANO == _anoatual))
                          orderby t.TVC_ANO, t.TVC_MES descending
                          select t);

            return ToDTOPage(_lista, numpagina, linhas);
        }
        public TotalVendasCartaoDTO BuscarPorEmpresa(int _em_id, int _for_id, int _mesatual, int _anoatual)
        {
            var _lista = (from t in db.TOTAL_VENDAS_CARTAO
                          where t.EMP_ID == _em_id  &&
                                t.FOR_ID == _for_id &&
                                t.TVC_MES == _mesatual &&
                                t.TVC_ANO == _anoatual 
                          orderby t.TVC_ANO, t.TVC_MES descending
                          select t).FirstOrDefault();

            return ToDTO(_lista);
        }



    }
}
