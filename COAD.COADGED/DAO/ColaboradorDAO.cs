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

    public class ColaboradorDAO : AbstractGenericDao<COLABORADOR, ColaboradorDTO, int>
    {

        public ColaboradorDAO() : base()
        {
            SetProfileName( "GED" );
        }

        // colecionador deste colaborador...
        public int? BuscarColecionadorDoColaborador(string nome) {
            IQueryable<COLABORADOR> query = GetDbSet();
            
            nome = nome.ToString();
            int? areaId = query.Where(x => x.USU_LOGIN == nome).Select(x => x.ARE_CONS_ID).FirstOrDefault();

            return areaId;
        }

        // busca para listar...
        public Pagina<ColaboradorDTO> Colaboradores(int? colaboradorId, string nome = null, int ativo = 1, int? cargoId = null, int? colecionadorId = null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<COLABORADOR> query = GetDbSet();

            if (!String.IsNullOrWhiteSpace(nome))
            {
                nome = nome.ToString();
                query = query.Where(x => x.USU_LOGIN.Contains(nome));
            }

            if (cargoId != null)
            {
                query = query.Where(x => x.CRG_ID == cargoId);
            }

            if (colaboradorId != null)
            {
                query = query.Where(x => x.COL_ID == colaboradorId);
            }

            if (colecionadorId != null)
            {
                query = query.Where(x => x.ARE_CONS_ID == colecionadorId);
            }

            query = query.Where(x => x.COL_ATIVO == ativo);
         
            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}
