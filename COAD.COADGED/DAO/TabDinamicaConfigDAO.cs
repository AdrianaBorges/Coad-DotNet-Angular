using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.DAO
{
    public class TabDinamicaConfigDAO : AbstractGenericDao<TAB_DINAMICA_CONFIG, TabDinamicaConfigDTO, string>
    {
        public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 
        public TabDinamicaConfigDAO() : base()
        {
            SetProfileName( "GED" );
            db = GetDb<COADGEDEntities>(false);
        }

        private IQueryable<TAB_DINAMICA_CONFIG> Listar(string _tdc_id = null, string _tab_descricao = null, int _tdc_tipo = 0, bool _publicados = false, int _tgr_id = 0, int _tit_id = 0, int? _tgr_tipo=null)
        {
            IQueryable<TAB_DINAMICA_CONFIG> query = (from a in db.TAB_DINAMICA_CONFIG
                                                     where (_tdc_tipo == 0 ||
                                                           (_tdc_tipo == 1 && a.TDC_TIPO == _tdc_tipo) ||
                                                           (_tdc_tipo == 2 && ( a.TDC_TIPO == 2 || a.TDC_TIPO == 3 )))
                                                        && (_tgr_id == 0 ||  (a.TGR_ID == _tgr_id))
                                                        && (_tit_id == 0 ||  (a.TIT_ID == _tit_id))
                                                        && (_publicados == false || (_publicados == true && a.TDC_DATA_PUBLICACAO != null))
                                                        && ((_tab_descricao == null || _tab_descricao == "") || ((_tab_descricao != null && _tab_descricao != "") && a.TDC_NOME_TABELA.Contains(_tab_descricao)))
                                                        && (_tgr_tipo == null ||(_tgr_tipo != null && a.TAB_DINAMICA_GRUPO.TGR_TIPO == _tgr_tipo))
                                                     select a);   

            query = query.OrderBy(x => x.TDC_NOME_TABELA);

            return query;
        }

        public IList<TabDinamicaConfigDTO> BuscarLancamentosTabelaDinamicaPorDataPorTgrTipo(DateTime? dataParametro, int? tgrId, int? tdcTipo)
        {
            int? tdcTabAux = 0;
            if(tdcTipo == 1)
            {
                tdcTabAux = 0;
            }
            else
            {
                tdcTabAux = 3;
            }

            var query = (from tdc in db.TAB_DINAMICA_CONFIG
                         where (tdc.TDC_DATA_PUBLICACAO >= dataParametro || tdc.TDC_DATA_ALTERA >= dataParametro)
                         && tdc.TGR_ID == tgrId
                         && (tdc.TDC_TIPO == tdcTipo || tdc.TDC_TIPO == tdcTabAux)
                         orderby tdc.TDC_DATA_PUBLICACAO descending
                         select tdc
                         );

            var tabDinamicaConfigDTOs = ToDTO(query);
            return tabDinamicaConfigDTOs;
        }



        public IList<TabDinamicaConfigDTO> ListarTabDinamica(string _tdc_id = null, string _tab_descricao = null, int _tdc_tipo = 0, bool _publicados = false, int? _tgr_tipo = null)
        {
            IQueryable<TAB_DINAMICA_CONFIG> query = this.Listar(_tdc_id, _tab_descricao, _tdc_tipo, _publicados, 0, 0, _tgr_tipo);

            return ToDTO(query);
        }
        public Pagina<TabDinamicaConfigDTO> ListarTabDinamicaPag(string _tab_descricao = null, int _tdc_tipo = 0, bool _publicados = false, int _tgr_id = 0, int _tit_id = 0, int pagina = 1, int registroPorPagina = 20)
        {
            IQueryable<TAB_DINAMICA_CONFIG> query = this.Listar(null,_tab_descricao,_tdc_tipo,_publicados,_tgr_id,_tit_id);

            return ToDTOPage(query, pagina, registroPorPagina);
        }
        public virtual void ExcluirTabelaItens(string _id)
        {
            try
            {
                db.EXCLUIR_TABELA_DINAMICA_ITENS(_id);
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }
        public virtual void ExcluirTabelaeConfig(string _id)
        {
            try
            {
                db.EXCLUIR_TABELA_DINAMICA_FULL(_id);
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }
        public virtual void ImportarTabelaDinamica(string _sqlcommand)
        {
            try
            {
                db.IMPORTAR_TAB_DINAMICA(_sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }

        public TabDinamicaConfigDTO BuscarPorNomeIdentificador(string TDC_NOME_IDENTIFICADOR)
        {
            var query = (from tdc in db.TAB_DINAMICA_CONFIG
                         where tdc.TDC_NOME_IDENTIFICADOR == TDC_NOME_IDENTIFICADOR
                         select tdc).FirstOrDefault();
            var dto = ToDTO(query);
            return dto;
        }
    }
}
