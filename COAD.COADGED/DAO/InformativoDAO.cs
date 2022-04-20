using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.DAO
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class InformativoDAO : AbstractGenericDao<INFORMATIVO, InformativoDTO, object>
    {

        public InformativoDAO() : base()
        {
            SetProfileName( "GED" );
        }

        public Pagina<InformativoDTO> Informativos(int? informativoId, string ano = null, int? numero = null, int ativoId = 1, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<INFORMATIVO> query = GetDbSet();

            if (!String.IsNullOrEmpty(ano))
            {
                ano = ano.ToString();
                query = query.Where(x => x.INF_ANO.Contains(ano));
            }

            if (numero != null)
            {
                query = query.Where(x => x.INF_NUMERO == numero);
            }

            query = query.Where(x => x.INF_ATIVO == ativoId).OrderByDescending(x => x.INF_ANO).ThenByDescending(x => x.INF_NUMERO);
            
            return ToDTOPage(query, pagina, itensPorPagina);
        }

        
        /* Ler informativos ativos em produção */

        public Pagina<InformativoDTO> LerInformativosEmProducao(int pagina = 1, int itensPorPagina = 10)
        {
            // instanciando objeto query...
            IQueryable<INFORMATIVO> query = GetDbSet();

            // selecionando informativos ativos cujas datas de finalização não estejam preenchidas...
            query = query.Where(x => x.INF_ATIVO == 1 && x.INF_DATA_FINAL == null);
            
            // retornando o objeto resultante...
            return ToDTOPage(query, pagina, itensPorPagina);
        }

        /* Buscar um informativo */
        public InformativoDTO BuscarInformativo(string ano, int numero)
        {
            // instanciando objeto query...
            IQueryable<INFORMATIVO> query = GetDbSet();

            // selecionando informativos ativos cujas datas de finalização não estejam preenchidas...
            if ((!String.IsNullOrEmpty(ano)) && (numero != null))
            {
                query = query.Where(x => x.INF_ANO == ano && x.INF_NUMERO == numero);
            }
            // retornando o objeto resultante...
            return ToDTO(query.FirstOrDefault());
        }
    }
}
