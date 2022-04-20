using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Model.DTO.Custons;
using COAD.COADGED.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COAD.COADGED.Service
{
    [ServiceConfig("TDC_ID")]
    public class TabDinamicaConfigSRV : GenericService<TAB_DINAMICA_CONFIG, TabDinamicaConfigDTO, string>
    {
        private TabDinamicaConfigDAO _dao = new TabDinamicaConfigDAO();

        TabDinamicaSRV _tabDinSRV = new TabDinamicaSRV();
        TabDinamicaItemSRV _tabDinItemSRV = new TabDinamicaItemSRV();
        TabDinamicaConfigItemSRV _tabDinConfigItemSRV = new TabDinamicaConfigItemSRV();
                                
        public TabDinamicaConfigSRV()
        {
            Dao = _dao;
        }

        public IList<TabelaDinamicaConfigNovidadeDTO> BuscarLancamentosTabelaDinamica(DateTime dataParametro, int? tgrId, int? tdcTipo)
        {
            var listTabelaConfigDTO = _dao.BuscarLancamentosTabelaDinamicaPorDataPorTgrTipo(dataParametro, tgrId, tdcTipo);
                var listTabelaDinamicaConfigNovidadeDTO = new List<TabelaDinamicaConfigNovidadeDTO>();
                for (int i = 0; i < listTabelaConfigDTO.Count; i++)
                {
                    var tabelaDinamicaConfigNovidadeDTO = new TabelaDinamicaConfigNovidadeDTO()
                    {
                        TDC_ID = listTabelaConfigDTO[i].TDC_ID,
                        TDC_DATA_PUBLICACAO = listTabelaConfigDTO[i].TDC_DATA_PUBLICACAO,
                        TDC_NOME_TABELA = listTabelaConfigDTO[i].TDC_NOME_TABELA
                    };
                    listTabelaDinamicaConfigNovidadeDTO.Add(tabelaDinamicaConfigNovidadeDTO);
                }
                return listTabelaDinamicaConfigNovidadeDTO;
        }

        public IList<TabDinamicaConfigDTO> ListarTabDinamica(string _tdc_id = null, string _tab_descricao = null, int _tdc_tipo = 0, bool _publicados = false, int? _tgr_tipo = null)
        {
            return _dao.ListarTabDinamica(_tdc_id, _tab_descricao, _tdc_tipo, _publicados, _tgr_tipo);
        }
        public Pagina<TabDinamicaConfigDTO> ListarTabDinamicaPag(string _tab_descricao = null, int _tdc_tipo = 0, bool _publicados = false, int _tgr_id = 0, int _tit_id = 0, int pagina = 1, int registroPorPagina = 20)
        {
            return _dao.ListarTabDinamicaPag(_tab_descricao,_tdc_tipo,_publicados,_tgr_id,_tit_id,pagina,registroPorPagina);
        }
        public string SalvarConfig(TabDinamicaConfigDTO _config)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    //-----------
                    if (_config.TDC_ID == null || _config.TDC_ID == "")
                        _config.TDC_ID = Guid.NewGuid().ToString();
                    //-----------

                    //-----------
                    //--- Atualiza configuração das Tabelas
                    //-----------
                    TabDinamicaConfigDTO _tabcfg = this.FindById(_config.TDC_ID);

                    if (SessionContext.usu_login_desktop == null || SessionContext.usu_login_desktop == "")
                        _config.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                    else
                        _config.USU_LOGIN = SessionContext.usu_login_desktop;

                    if (_tabcfg == null)
                    {
                        _config.TDC_DATA_ALTERA = DateTime.Now;
                        _config.TDC_DATA_INCLUSAO = DateTime.Now;

                        _tabcfg = this.Save(_config);
                    }
                    else
                    {
                        _config.TDC_DATA_ALTERA = DateTime.Now;
                        _config.TDC_DATA_INCLUSAO = DateTime.Now;
                        _tabcfg = this.Merge(_config, "TDC_ID");
                    }
                    //-----------
                    //--- Realiza a inclusão dos itens na cofiguração das tabelas
                    //-----------

                    foreach (TabDinamicaConfigItemDTO _item in _config.TAB_DINAMICA_CONFIG_ITEM)
                    {
                        TabDinamicaConfigItemDTO _itemaux = _tabDinConfigItemSRV.FindById(_item.TDC_ID, _item.TCI_ID);

                        if (_itemaux == null)
                        {
                            if (_item.TDC_ID == null)
                                _item.TDC_ID = _tabcfg.TDC_ID;

                            _itemaux = _tabDinConfigItemSRV.Save(_item);
                        }
                        else
                            _itemaux = _tabDinConfigItemSRV.Merge(_item, "TDC_ID", "TCI_ID");

                    }

                    scope.Complete();
                }

                return _config.TDC_ID;

            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }
        public string SalvarTabelaeConfig(string _id, TabDinamicaConfigDTO _config, TabDinamicaDTO _tabela, bool _importacao=false )
        {
            try
            {
                Boolean _addCicloAprov = false;

                if (_importacao)
                    if (_id != null && _id != "")
                    {
                        this.ExcluirTabelaItens(_id);
                        _config.TDC_ID = _id;
                        _addCicloAprov = true;
                    }

                var transactionOptions = new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.Serializable,
                    Timeout = TimeSpan.FromMinutes(30),
                };

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    if (_config.TDC_TIPO == 4) {

                        var tabDinamicaDTO = _dao.BuscarPorNomeIdentificador(_config.TDC_NOME_IDENTIFICADOR);

                        if (tabDinamicaDTO != null)
                        {
                            throw new Exception("Já existe uma tabela persionalizada com este nome identificador criado.");
                        }

                        _config.TDC_NOME_IDENTIFICADOR = _config.TDC_NOME_TABELA.Replace(" ", "_").ToUpper();
                    }
                    //-----------
                    if (_config.TDC_ID == null || _config.TDC_ID == "")
                    {
                        _config.TDC_ID = Guid.NewGuid().ToString();
                        _addCicloAprov = true;
                    }
                    //-----------

                    //-----------
                    //--- Atualiza configuração das Tabelas
                    //-----------
                    TabDinamicaConfigDTO _tabcfg = this.FindById(_config.TDC_ID);
                    _config.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;

                    //if (SessionContext.usu_login_desktop == null || SessionContext.usu_login_desktop == "")
                    //    _config.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                    //else
                    //    _config.USU_LOGIN = SessionContext.usu_login_desktop;

                    if (_tabcfg == null)
                    {
                        _config.TDC_DATA_INCLUSAO = DateTime.Now;
                        _tabcfg = this.Save(_config);
                    }
                    else
                    {
                        _config.TDC_DATA_ALTERA = DateTime.Now;
                        _tabcfg = this.Merge(_config, "TDC_ID");
                    }

                    //-----------
                    //--- Apaga os itens da configuração, para que eles sejam incluidos novamente com as alterações realizadas.
                    //-----------

                    IList<TabDinamicaConfigItemDTO> _lisacfg = _tabDinConfigItemSRV.ListarTabDinamica(_config.TDC_ID);

                    _tabDinConfigItemSRV.DeleteAll(_lisacfg);


                    //-----------
                    //--- Realiza a inclusão dos itens na cofiguração das tabelas
                    //-----------


                    foreach (TabDinamicaConfigItemDTO _item in _config.TAB_DINAMICA_CONFIG_ITEM)
                    {
                        TabDinamicaConfigItemDTO _itemaux = _tabDinConfigItemSRV.FindById(_item.TDC_ID, _item.TCI_ID);

                        if (_itemaux == null)
                        {
                            if (_item.TDC_ID == null)
                                _item.TDC_ID = _tabcfg.TDC_ID;

                            _itemaux = _tabDinConfigItemSRV.Save(_item);
                        }
                        else
                            _itemaux = _tabDinConfigItemSRV.Merge(_item, "TDC_ID", "TCI_ID");

                    }


                    //-----------
                    //--- Atualiza Tabelas
                    //-----------

                    TabDinamicaDTO _tab = _tabDinSRV.FindById(_tabela.TDC_ID);
                    _tabela.TAB_DATA_INCLUSAO = DateTime.Now;

  
                    if (_tab == null)
                    {
                        _tabela.TDC_ID = _tabcfg.TDC_ID;

                        _tab = _tabDinSRV.Save(_tabela);
                    }
                    else
                        _tab = _tabDinSRV.Merge(_tabela, "TDC_ID");

                    //-----------
                    //--- Realiza a inclusão dos itens das tabelas
                    //-----------
                    //-----------
                    //--- Os dados da tabela  TAB_DINAMICA_ITEM serão incluidos apenas quando o tipo for (1-tabela).   
                    //--- No caso dos simuladores os dados desta tabela serão apenas temporários. Nã ha necessidade 
                    //--- de gravar estes dados.  
                    //-----------
                    if (_config.TDC_TIPO == 1)
                    {
                        foreach (TabDinamicaItemDTO _item in _tabela.TAB_DINAMICA_ITEM)
                        {
                            TabDinamicaItemDTO _itemaux = _tabDinItemSRV.FindById(_item.TDC_ID, _item.TAB_ID);

                            if (_item.TDC_ID == null)
                                _item.TDC_ID = _tabcfg.TDC_ID;
                            
                            _item.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;

                            //if (SessionContext.usu_login_desktop == null || SessionContext.usu_login_desktop == "")
                            //    _item.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                            //else
                            //    _item.USU_LOGIN = SessionContext.usu_login_desktop;

                            _item.TAB_DATA_ALTERA = DateTime.Now;
                            _item.TAB_DATA_INCLUSAO = DateTime.Now;

                            if (_itemaux == null)
                            {
                                if (_item.TDC_ID == null)
                                    _item.TDC_ID = _tabcfg.TDC_ID;

                                _itemaux = _tabDinItemSRV.Save(_item);
                            }
                            else
                                _itemaux = _tabDinItemSRV.Merge(_item, "TDC_ID", "TAB_ID");

                            AutenticadoThread.ITEM_PROCESSO_ID = _itemaux.TAB_ID;

                            AutenticadoThread.TOTAL_LINHAS -= 1; 

                        }
                    }

                    // --- Incluindo informações referentes ao ciclo de aprovação
                    //----------
                    if (_addCicloAprov)
                    {
                        TabDinamicaPublicacaoDTO _pub = new TabDinamicaPublicacaoDTO();
                        _pub.TDC_ID = _config.TDC_ID;
                        _pub.TPU_DATA_LANC = DateTime.Now;
                        _pub.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;

                        //if (SessionContext.usu_login_desktop == null || SessionContext.usu_login_desktop == "")
                        //    _pub.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                        //else
                        //    _pub.USU_LOGIN = SessionContext.usu_login_desktop;

                        _pub.TPU_STATUS = "INC";

                        new TabDinamicaPublicacaoSRV().Save(_pub);
                    }

                    //----------

                    scope.Complete();
                }

                return _config.TDC_ID;

            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
            finally
            {
                AutenticadoThread.ITEM_PROCESSO_ID = 0;
                AutenticadoThread.TOTAL_LINHAS = 0; 
            }
        }
        public void ExcluirTabelaItens(string _id)
        {
            try
            {
                _dao.ExcluirTabelaItens(_id);
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }
        public void ExcluirTabelaeConfig(string _id)
        {
            try
            {
                _dao.ExcluirTabelaeConfig(_id);
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }
        public void Publicar(TabDinamicaConfigDTO _tdc)
        {
            TabDinamicaPublicacaoDTO _pub = new TabDinamicaPublicacaoSRV().FindLastPubReg(_tdc.TDC_ID);
            TabDinamicaPublicacaoSRV _tabDimPubSRV = new TabDinamicaPublicacaoSRV();

            using (TransactionScope scope = new TransactionScope())
            {

                if (_pub == null)
                {
                    _pub = new TabDinamicaPublicacaoDTO();
                    _pub.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                    _pub.TDC_ID = _tdc.TDC_ID;
                    _pub.TPU_DATA_LANC = DateTime.Now;
                    _pub.TPU_OBS = "Publicação da tabela";
                    _pub.TPU_DATA_APROV = DateTime.Now;
                    _pub.USU_LOGIN_APROV = SessionContext.autenticado.USU_LOGIN;
                    _pub.TPU_STATUS = "PUB";
                    _tabDimPubSRV.Save(_pub);
                    _tdc.TDC_DATA_PUBLICACAO = DateTime.Now;
                    this.Merge(_tdc);
                }
                else
                {
                    _pub.TPU_OBS = "Publicação da tabela";
                    _pub.TPU_DATA_APROV = DateTime.Now;
                    _pub.USU_LOGIN_APROV = SessionContext.autenticado.USU_LOGIN;
                    _pub.TPU_STATUS = "PUB";
                    _tabDimPubSRV.Merge(_pub, "TPU_ID");
                    _tdc.TDC_DATA_PUBLICACAO = DateTime.Now;
                    this.Merge(_tdc);
                }

                scope.Complete();
            }
        }
        public void RemoverPublicacao(TabDinamicaConfigDTO _tdc)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                TabDinamicaPublicacaoDTO _pub = new TabDinamicaPublicacaoDTO();
                _pub.TDC_ID = _tdc.TDC_ID;
                _pub.TPU_DATA_LANC = DateTime.Now;
                _pub.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                _pub.TPU_STATUS = "REM";

                new TabDinamicaPublicacaoSRV().Save(_pub);

                _tdc.TDC_DATA_PUBLICACAO = null;

                this.Merge(_tdc);

                scope.Complete();

            }
        }

        public virtual void ImportarTabelaDinamica(string _sqlcommand)
        {
            try
            {
                _dao.ImportarTabelaDinamica(_sqlcommand);
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }
    }
}
