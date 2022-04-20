using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;


namespace COAD.CORPORATIVO.DAO
{
    public class OrigemAcessoDAO : AbstractGenericDao<ORIGEM_ACESSO, OrigemAcessoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public OrigemAcessoDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public IList<OrigemAcessoDTO> ListarPorNome(string nome)
        {
            IQueryable<ORIGEM_ACESSO> query = GetDbSet();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(x => x.OAC_DESCRICAO.Contains(nome));
            }

            return ToDTO(query);
        }
        public Pagina<OrigemAcessoDTO> ListarPorNome(string nome, int _pagina = 1, int _itensPorPagina = 10)
        {
            IQueryable<ORIGEM_ACESSO> query = GetDbSet();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(x => x.OAC_DESCRICAO.Contains(nome));
            }

            return ToDTOPage(query, _pagina, _itensPorPagina);
        }


    }
}
