
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Model.DTO.Custons;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Extensions;
using System.Data.Objects.SqlClient;

namespace COAD.COADGED.DAO
{


    public class ManualDPItemDAO : AbstractGenericDao<MANUAL_DP_ITEM, ManualDPItemDTO, int>
    {
        public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 
        public ManualDPItemDAO()
            : base()
        {
            SetProfileName("GED");

            db = GetDb<COADGEDEntities>(false);
        }
        private List<ListaManualDPDTO> ListarInterno(ParamConsultaManualDTO param)
        {
            //DateTime? dtini = null;
            //DateTime? dtfim = null;

            //if (param.MAI_DATA_ATO != null)
            //{
            //    dtini = new DateTime(param.MAI_DATA_ATO.Value.Year, param.MAI_DATA_ATO.Value.Month, param.MAI_DATA_ATO.Value.Day);
            //    dtfim = new DateTime(param.MAI_DATA_ATO.Value.Year, param.MAI_DATA_ATO.Value.Month, param.MAI_DATA_ATO.Value.Day);

            //    dtfim = dtfim.Value.AddDays(1);
            //}
            var query = (from i in db.MANUAL_DP_ITEM
                         join m in db.MANUAL_DP on i.MAN_ID equals m.MAN_ID
                         where (param.MOD_ID == null || (param.MOD_ID == m.MOD_ID))
                            && (param.MAN_ID == null || (param.MAN_ID == m.MAN_ID))
                            && ((param.MAI_TITULO == null || param.MAI_TITULO == "") || ((param.MAI_TITULO != null && param.MAI_TITULO != "") && i.MAI_TITULO.Contains(param.MAI_TITULO)))
                            && (param.PUBLICADO == false || (param.PUBLICADO == true && i.MAI_DATA_PUBLICACAO != null))
                            && ((param.MAI_DESCRICAO == null || param.MAI_DESCRICAO == "") || ((param.MAI_DESCRICAO != null && param.MAI_DESCRICAO != "") && i.MAI_DESCRICAO.Contains(param.MAI_DESCRICAO)))
                            && ((param.MAN_ASSUNTO == null || param.MAN_ASSUNTO == "") || ((param.MAN_ASSUNTO != null && param.MAN_ASSUNTO != "") && m.MAN_ASSUNTO.Contains(param.MAN_ASSUNTO)))
                            && (( param.TIP_ATO_ID == null && 
                                  param.MAI_NUMERO_ATO == null  && 
                                  param.MAI_DATA_ATO == null  && 
                                  param.ORG_ID == null  && 
                                  param.MAI_NUMERO_ARTIGO == null  &&
                                  param.FUN_NUM_PARAGRAFO == null  &&
                                  param.FUN_INCISO== null ) || 
                                ( db.FUNDAMENTACAO.Count(f => f.MAI_ID == i.MAI_ID && 
                                                        (f.TIP_ATO_ID == param.TIP_ATO_ID || 
                                                         f.MAI_NUMERO_ATO == param.MAI_NUMERO_ATO || 
                                                         f.MAI_DATA_ATO == param.MAI_DATA_ATO || 
                                                         f.ORG_ID == param.ORG_ID || 
                                                         f.MAI_NUMERO_ARTIGO == param.MAI_NUMERO_ARTIGO ||
                                                         f.FUN_NUM_PARAGRAFO == param.FUN_NUM_PARAGRAFO ||
                                                         f.FUN_INCISO == param.FUN_INCISO)) > 0))
                         select new ListaManualDPDTO
                         {
                             MOD_DESCRICAO = m.MANUAL_DP_MODULO.MOD_DESCRICAO,
                             MAN_ASSUNTO = m.MAN_ASSUNTO,
                             MAI_TITULO = i.MAI_TITULO,
                             USU_LOGIN = i.USU_LOGIN,
                             USU_LOGIN_ALT = i.USU_LOGIN_ALT,
                             DATA_INSERT = i.DATA_INSERT,
                             DATA_ALTERA = i.DATA_ALTERA,
                             MAI_DATA_PUBLICACAO = i.MAI_DATA_PUBLICACAO,
                             MOD_ID = m.MOD_ID,
                             MAI_ID = i.MAI_ID,
                             MAI_DESCRICAO = i.MAI_DESCRICAO,
                             MAN_ID = i.MAN_ID
                         });
  


            return query.ToList();

        }
        public ListaManualDPDTO BuscarModuloSelect(int _mai_id)
        {

            var query = (from i in db.MANUAL_DP_ITEM
                         join m in db.MANUAL_DP on i.MAN_ID equals m.MAN_ID
                         where (i.MAI_ID == _mai_id)


                         select new ListaManualDPDTO
                         {
                             MOD_DESCRICAO = m.MANUAL_DP_MODULO.MOD_DESCRICAO,
                             MAN_ASSUNTO = m.MAN_ASSUNTO,
                             MAI_TITULO = i.MAI_TITULO,
                             USU_LOGIN = i.USU_LOGIN,
                             USU_LOGIN_ALT = i.USU_LOGIN_ALT,
                             DATA_INSERT = i.DATA_INSERT,
                             DATA_ALTERA = i.DATA_ALTERA,
                             MAI_DATA_PUBLICACAO = i.MAI_DATA_PUBLICACAO,
                             MOD_ID = m.MOD_ID,
                             MAI_ID = i.MAI_ID,
                             MAI_DESCRICAO = i.MAI_DESCRICAO
                         }).FirstOrDefault();


            return query;

        }
        public IList<ManualDPItemDTO> BuscarItensAlterados(int _dias, int _itens)
        {
            var query = (from i in db.MANUAL_DP_ITEM
                         join a in db.MANUAL_DP on i.MAN_ID equals a.MAN_ID
                         join m in db.MANUAL_DP_MODULO on a.MOD_ID equals m.MOD_ID
                         where (SqlFunctions.DateDiff("day", DateTime.Now, i.DATA_ALTERA) <= _dias) ||
                               (SqlFunctions.DateDiff("day", DateTime.Now, i.DATA_INSERT) <= _dias)
                            && (i.MAI_DATA_PUBLICACAO != null)
                         select new ManualDPItemDTO { MAI_ID = i.MAI_ID,
                                                      MAI_DESCRICAO = i.MAI_DESCRICAO,
                                                      USU_LOGIN = i.USU_LOGIN,
                                                      USU_LOGIN_ALT = i.USU_LOGIN_ALT,
                                                      DATA_INSERT = i.DATA_INSERT,
                                                      DATA_ALTERA = i.DATA_ALTERA,
                                                      MAI_ID_PAI = i.MAI_ID_PAI,
                                                      MAN_ID = i.MAN_ID,
                                                      MAI_TITULO = i.MAI_TITULO,
                                                      MAI_DATA_PUBLICACAO = i.MAI_DATA_PUBLICACAO,
                                                      MAI_INDEX = i.MAI_INDEX,
                                                      MAI_NIVEL = i.MAI_NIVEL,
                                                      TIP_ATO_ID = i.TIP_ATO_ID,
                                                      MAI_NUMERO_ATO = i.MAI_NUMERO_ATO,
                                                      MAI_DATA_ATO = i.MAI_DATA_ATO,
                                                      ORG_ID = i.ORG_ID,
                                                      MAI_NUMERO_ARTIGO = i.MAI_NUMERO_ARTIGO,
                                                      MOD_DESCRICAO = i.MANUAL_DP.MANUAL_DP_MODULO.MOD_DESCRICAO,
                                                      MAN_ASSUNTO = i.MANUAL_DP.MAN_ASSUNTO
                         }).OrderByDescending(x => x.DATA_ALTERA).ThenByDescending(x => x.DATA_INSERT);
            

            var query01 = query.Take(_itens).ToList();

            return query01;

        }

        public IList<ManualDPItemDTO> BuscarItemAlteradosEPublicadosPorData(DateTime dataParametro)
        {
            var query = (from mai in db.MANUAL_DP_ITEM
                         where mai.DATA_ALTERA >= dataParametro
                         && mai.MAI_DATA_PUBLICACAO >= dataParametro
                         orderby mai.MAI_DATA_PUBLICACAO descending, mai.DATA_ALTERA descending
                         select mai).ToList();
            var manualDpItemDTO = ToDTO(query);
            return manualDpItemDTO;
        }

        public Pagina<ManualDPItemDTO> BuscarItensAlterados(int _dias, int _itens, int pagina = 1, int registroPorPagina = 5)
        {
            var query = (from i in db.MANUAL_DP_ITEM
                         where (SqlFunctions.DateDiff("day", DateTime.Now, i.DATA_ALTERA) <= _dias) ||
                                (SqlFunctions.DateDiff("day", DateTime.Now, i.DATA_INSERT) <= _dias)
                           && (i.MAI_DATA_PUBLICACAO != null)
                         select i).OrderByDescending(x => x.DATA_ALTERA).ThenByDescending(x => x.DATA_INSERT);

            return ToDTOPage(query, pagina, registroPorPagina);

        }
        public ManualDPItemDTO BuscarItemPrincipal(ManualDPItemDTO _item)
        {
            var query = (from i in db.MANUAL_DP_ITEM
                         where i.MAN_ID == _item.MAN_ID
                            && i.MAI_INDEX <= (_item.MAI_INDEX - 1)
                            && i.MAI_NIVEL == _item.MAI_NIVEL
                            && i.MAI_ID != _item.MAI_ID
                         select i).OrderByDescending(x => x.MAI_INDEX).FirstOrDefault();

            return ToDTO(query);

        }
        public int BuscarQtdeItens(int? _mai_id)
        {
            var query = (from i in db.MANUAL_DP_ITEM
                         where i.MAI_ID_PAI == _mai_id
                         select i).Count();

            return query;
        }
        public int BuscarQtdeItensAssunto(int? _man_id)
        {
            var query = (from i in db.MANUAL_DP_ITEM
                         where i.MAI_ID_PAI == null
                            && i.MAN_ID == _man_id
                         select i).Count();

            return query;
        }

        public IList<ManualDPItemDTO> ListarPorAssunto(int _man_id)
        {
            var query = (from i in db.MANUAL_DP_ITEM
                         where i.MAN_ID == _man_id
                         select i).OrderBy(x => x.MAI_NIVEL).ThenBy(x => x.MAI_INDEX);

            return ToDTO(query);

        }
        public IList<ListaManualDPDTO> Listar(string _assunto = null, string _mai_titulo = null, string _mai_descricao = null)
        {
            var _param = new ParamConsultaManualDTO();
            _param.MAN_ASSUNTO = _assunto;
            _param.MAI_TITULO = _mai_titulo;
            _param.MAI_DESCRICAO = _mai_titulo;

            var query = this.ListarInterno(_param);

            return query;

        }
        public Pagina<ListaManualDPDTO> ListarPorPagina(string _mai_descricao = null, int Pagina = 1, int NumeroDePaginas = 10)
        {
            var _param = new ParamConsultaManualDTO();
            _param.MAI_DESCRICAO = _mai_descricao;
            _param.PUBLICADO = true;

            var query = this.ListarInterno(_param);

            return query.Paginar<ListaManualDPDTO>(Pagina, NumeroDePaginas);
            
        }
      
        public IList<ListaManualDPDTO> Pesquisar(string _mai_titulo)
        {
            var _param = new ParamConsultaManualDTO();

            _param.MAI_TITULO = _mai_titulo;

            var query = this.ListarInterno(_param);

            return query;

        }

        public Pagina<ListaManualDPDTO> Pesquisar(ParamConsultaManualDTO param, int pagina = 1, int registroPorPagina = 10)
        {
            var query = this.ListarInterno(param);

            return query.Paginar<ListaManualDPDTO>(pagina, registroPorPagina);

        }
        public IList<ManualDPItemDTO> Listar(string _mai_titulo)
        {
            var query = (from i in db.MANUAL_DP_ITEM
                        where ((_mai_titulo == null || _mai_titulo == "") || ((_mai_titulo != null && _mai_titulo != "") && i.MAI_DESCRICAO.Contains(_mai_titulo)))
                       select i);

            query = query.OrderBy(x => x.MAI_TITULO);

            return ToDTO(query);

        }
    }
}
