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
using System.Data.Objects;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Extensions;
using COAD.CORPORATIVO.Model.Dto.Custons;


namespace COAD.CORPORATIVO.DAO
{
    public class FilaCadastroDAO : AbstractGenericDao<FILA_CADASTRO, FilaCadastroDTO,int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public IQueryable<FILA_CADASTRO> TemplateFilaCadastro(int? regiaoId, DateTime? dataFila, int? REP_ID)
        {
            var query = GetDbSet().Where(x => x.REP_ID == REP_ID && x.RG_ID == regiaoId && (x.FLC_IMPORTACAO == null || x.FLC_IMPORTACAO == false));

            if (dataFila != null)
            {
                query = query.Where(x => EntityFunctions.TruncateTime(x.FLC_DATA) == EntityFunctions.TruncateTime(dataFila));
            }

            query = query.OrderBy(x => x.FLC_ORDEM);
            return query;
        }

        public IList<FilaCadastroDTO> FindByRegiao(int? regiaoId, DateTime? dataFila, int? EXCETO_REP_ID)
        {
            var query = GetDbSet().Where(x => x.REP_ID != EXCETO_REP_ID && x.RG_ID == regiaoId && (x.FLC_IMPORTACAO == null || x.FLC_IMPORTACAO == false));

            if (dataFila != null)
            {
                query = query.Where(x => EntityFunctions.TruncateTime(x.FLC_DATA) == EntityFunctions.TruncateTime(dataFila));
            }

            query = query.OrderBy(x => x.FLC_ORDEM);
            return ToDTO(query);
        }

        public int QuantidadeFilaCadastro(int regiaoId, DateTime dataFila)
        {
            IQueryable<FILA_CADASTRO> query = GetDbSet().Where(x => x.RG_ID == regiaoId && (x.FLC_IMPORTACAO == null || x.FLC_IMPORTACAO == false));

            if (dataFila != null)
            {
                query = query.Where(x => EntityFunctions.TruncateTime(x.FLC_DATA) == EntityFunctions.TruncateTime(dataFila));
            }

            return query.Count();
        }

        public int QuantidadeFilaCadastroImportacao(int regiaoId)
        {
            IQueryable<FILA_CADASTRO> query = GetDbSet().Where(x => x.RG_ID == regiaoId && (x.FLC_IMPORTACAO == true));
            return query.Count();
        }


        public FilaCadastroDTO FindByRepId(int regiaoId, DateTime dataFila, int? REP_ID)
        {
            var model = TemplateFilaCadastro(regiaoId, dataFila, REP_ID).FirstOrDefault();
            return ToDTO(model);
        }

        public bool HasFilaCadastro(int regiaoId, DateTime dataFila, int? REP_ID)
        {
            return (TemplateFilaCadastro(regiaoId, dataFila, REP_ID).Count() > 0);           
        }

        public IList<FilaCadastroDTO> FindAllByRepId(int regiaoId, int REP_ID)
        {
            var query = GetDbSet().Where(x => x.REP_ID == REP_ID && x.RG_ID == regiaoId && (x.FLC_IMPORTACAO == null || x.FLC_IMPORTACAO == false));

            return ToDTO(query);
        }

        public IList<FilaCadastroDTO> FindByRegiao(int? regiaoId, DateTime dataFila)
        {
            IQueryable<FILA_CADASTRO> query = GetDbSet().Where(x => x.RG_ID == regiaoId && (x.FLC_IMPORTACAO == null || x.FLC_IMPORTACAO == false));

            if (dataFila != null)
            {
                query = query.Where(x => EntityFunctions.TruncateTime(x.FLC_DATA) == EntityFunctions.TruncateTime(dataFila));
            }

            query = query.OrderBy(x => x.FLC_ORDEM);
            return ToDTO(query);
        }

        public Pagina<FilaCadastroDTO> FindByData(DateTime dataFila, string nomeRepresentante = null, int pagina = 1, int registrosPorPagina = 50, int? UEN_ID = 1)
        {
            IQueryable<FILA_CADASTRO> query = GetDbSet().Where(x => (x.FLC_IMPORTACAO == null || x.FLC_IMPORTACAO == false));

            if (dataFila != null)
            {
                query = query.Where(x => EntityFunctions.TruncateTime(x.FLC_DATA) == EntityFunctions.TruncateTime(dataFila));
            }

            if (!string.IsNullOrWhiteSpace(nomeRepresentante))
            {
                query = query.Where(x => x.REPRESENTANTE.REP_NOME.Contains(nomeRepresentante));
            }

            if (UEN_ID != null)
            {
                query = query.Where(x => x.REGIAO.UEN_ID == UEN_ID);
            }

            query = query.OrderBy(x => x.RG_ID).ThenBy(x => x.FLC_ORDEM);
            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        public Pagina<FilaCadastroDTO> FindByRegiao(int? regiaoId, DateTime dataFila, string nomeRepresentante = null, int pagina = 1, int registrosPorPagina = 50)
        {
            IQueryable<FILA_CADASTRO> query = GetDbSet().Where(x => x.RG_ID == regiaoId && (x.FLC_IMPORTACAO == null || x.FLC_IMPORTACAO == false));

            if (dataFila != null)
            {
                query = query.Where(x => EntityFunctions.TruncateTime(x.FLC_DATA) == EntityFunctions.TruncateTime(dataFila));
            }

            if (!string.IsNullOrWhiteSpace(nomeRepresentante))
            {
                query = query.Where(x => x.REPRESENTANTE.REP_NOME.Contains(nomeRepresentante));
            }

            query = query.OrderBy(x => x.FLC_ORDEM);
            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        public RepresentanteDTO ObterRepresentantePorRodizio(int REGIAO_ID, DateTime data)
        {
            var obj = GetDbSet().Where(
                x => x.RG_ID == REGIAO_ID
                && (x.FLC_IMPORTACAO == null || x.FLC_IMPORTACAO == false)
                && EntityFunctions.TruncateTime(x.FLC_DATA) == EntityFunctions.TruncateTime(data)).
                OrderBy(or => or.FLC_ORDEM).
                FirstOrDefault();

            var dto = ToDTO(obj);

            if (dto != null)
            {
                return dto.REPRESENTANTE;
            }
            else
            {
                return null;
            }            
        }

        /// <summary>
        /// Pega a lista baseada na ordem passada. O argumento 'superior' indica se a fila obtida será 
        /// superior (true) ou anterior (false)
        /// </summary>
        /// <param name="regiaoId"></param>
        /// <param name="FLC_ORDEM"></param>
        /// <param name="data"></param>
        /// <param name="superior">Indica se sera retornada uma fila superior ou anterior</param>
        /// <returns></returns>
        public FilaCadastroDTO ObterFilaPorOrdem(int? regiaoId, int? FLC_ORDEM, DateTime? data)
        {
            var query = GetDbSet().Where(x => x.RG_ID == regiaoId && (x.FLC_IMPORTACAO == null || x.FLC_IMPORTACAO == false)
                && (x.FLC_ORDEM == FLC_ORDEM)
                && EntityFunctions.TruncateTime(x.FLC_DATA) == EntityFunctions.TruncateTime(data)).OrderBy(order => order.FLC_ORDEM).FirstOrDefault();
            return ToDTO(query);
        }

        /// <summary>
        ///  Pega uma fila cadastro com uma prioridade a menos que a da fila 
        ///  passada. O critério de busca é a mesma a fila de mesma data e região 
        ///  da fila de id passado no argumento do método.
        /// </summary>
        /// <param name="FLC_ID"></param>
        /// <returns></returns>
        public FilaCadastroDTO ObterFilaAnterior(int? FLC_ID)
        {
            var fila = FindById(FLC_ID);
            var data = fila.FLC_DATA;
            var ordem = fila.FLC_ORDEM;
            var regiao = fila.RG_ID;

            ordem++;

            var filaDTO = ObterFilaPorOrdem(regiao, ordem, data);
            return filaDTO;
        }

        /// <summary>
        ///  Pega uma fila cadastro com uma prioridade a mais que a da fila 
        ///  passada. O critério de busca é a mesma a fila de mesma data e região 
        ///  da fila de id passado no argumento do método.
        /// </summary>
        /// <param name="FLC_ID"></param>
        /// <returns></returns>
        public FilaCadastroDTO ObterFilaSuperior(int? FLC_ID)
        {
            var fila = FindById(FLC_ID);
            var data = fila.FLC_DATA;
            var ordem = fila.FLC_ORDEM;
            var regiao = fila.RG_ID;
            ordem--;
            
            var filaDTO = ObterFilaPorOrdem(regiao, ordem, data);
            return filaDTO;
        }

        public IEnumerable<FilaCadastroDTO> FindAllByDataERegiao(DateTime? data, int? RG_ID)
        {
            var query = GetDbSet().Where(x => EntityFunctions.TruncateTime(x.FLC_DATA) == EntityFunctions.TruncateTime(data)
                && x.RG_ID == RG_ID && (x.FLC_IMPORTACAO == null || x.FLC_IMPORTACAO == false)).OrderBy(or => or.FLC_ORDEM);

            return ToDTO(query);
        }

        public RepresentanteDTO ObterRepresentantePorRodizioDeImportacao(int? rgId)
        {
            var obj = (from flc in db.FILA_CADASTRO
                         where 
                             flc.RG_ID == rgId &&
                             flc.FLC_IMPORTACAO == true
                         orderby flc.FLC_ORDEM ascending
                         select flc).FirstOrDefault();
            
            var dto = ToDTO(obj);

            if (dto != null)
            {
                return dto.REPRESENTANTE;
            }
            else
            {
                return null;
            }
        }

        public IList<FilaCadastroDTO> ListarFilaDeCadastroDaImportacao(int? rgID)
        {
            var query = (from fc in db.FILA_CADASTRO
                         where
                            fc.RG_ID == rgID &&
                            fc.FLC_IMPORTACAO == true
                            orderby fc.FLC_ORDEM ascending
                         select fc);
            return ToDTO(query);
        }

        public bool HasFilaCadastroImportacao(int rgID, int? repID)
        {
            var query = (from fil in db.FILA_CADASTRO
                         where
                             fil.RG_ID == rgID &&
                             fil.REP_ID == repID &&
                             fil.FLC_IMPORTACAO == true
                         select fil);
                
                return (query.Count() > 0);
        }

        public IList<FilaCadastroDTO> FindAllByRepId(int REP_ID)
        {
            var query = (from 
                            flC in db.FILA_CADASTRO
                         where 
                             flC.REP_ID == REP_ID &&
                             (flC.FLC_IMPORTACAO == null || flC.FLC_IMPORTACAO == false)
                         select flC);

            return ToDTO(query);
        }


    }
}
