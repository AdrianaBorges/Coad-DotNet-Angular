using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using Coad.GenericCrud.Dao.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.DAO
{
    [DAOConfig("coadsys")]
    public class DepartamentoDAO : AbstractGenericDao<DEPARTAMENTO, DepartamentoDTO,int>
    {
        public DepartamentoDAO()
        {
        }

        public Pagina<DepartamentoDTO> Departamentos(string nome, int pagina = 1, int numeroRegistrosPagina = 7)
        {
            IQueryable<DEPARTAMENTO> query = GetDbSet().Where(x => x.DATA_DESATIVACAO == null);

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(x => x.DP_NOME.Contains(nome));
            }

            return ToDTOPage(query, pagina, numeroRegistrosPagina);
        }

        public override IList<DEPARTAMENTO> FindAll()
        {
            var query = GetDbSet().Where(x => x.DATA_DESATIVACAO == null);
            return query.ToList();
        }
    }
}
