using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Model.DTO.Custons;
using COAD.COADGED.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Coad.GenericCrud.Extensions;

namespace COAD.COADGED.Service
{
    [ServiceConfig("MAI_ID")]
    public class ManualDPItemSRV : GenericService<MANUAL_DP_ITEM, ManualDPItemDTO, int>
    {
        private ManualDPItemDAO _dao = new ManualDPItemDAO();
		private ManualDPLinkDAO _manualDPLinkDAO = new ManualDPLinkDAO();

        public ManualDPItemSRV()
        {
            Dao = _dao;
        }

        public ListaManualDPDTO BuscarModuloSelect(int _mai_id)
        {
            return _dao.BuscarModuloSelect(_mai_id);
        }
        public IList<ManualDPItemDTO> Listar(string _mai_titulo)
        {
            return _dao.Listar(_mai_titulo);
        }
        public IList<ManualDPItemDTO> ListarPorAssunto(int _man_id)
        {
            return _dao.ListarPorAssunto(_man_id);
        }
        public IList<ListaManualDPDTO> Listar(string _assunto = null, string _mai_titulo = null)
        {
            return _dao.Listar( _assunto,  _mai_titulo);
        }
        public Pagina<ListaManualDPDTO> Pesquisar(ParamConsultaManualDTO param, int pagina = 1, int registroPorPagina = 10)
        {
            return _dao.Pesquisar(param, pagina, registroPorPagina);
        }
        public IList<ListaManualDPDTO> Pesquisar(string _mai_titulo)
        {
            return _dao.Pesquisar(_mai_titulo);
        }

        public ManualDPItemDTO BuscarItemPrincipal(ManualDPItemDTO _item)
        {
            return _dao.BuscarItemPrincipal(_item);
        }
        public int BuscarQtdeItens(int? _man_id)
        {
            return _dao.BuscarQtdeItens(_man_id);
        }
        public int BuscarQtdeItensAssunto(int? _man_id)
        {
            return _dao.BuscarQtdeItensAssunto(_man_id);
        }
        public IList<ListaManualDPDTO> Listar(string _assunto = null, string _mai_titulo = null, string _mai_descricao = null)
        {
            return _dao.Listar(_assunto, _mai_titulo, _mai_descricao);
        }
        public Pagina<ListaManualDPDTO> ListarPorPagina(string _mai_descricao = null, int Pagina = 1, int NumeroDePaginas = 10)
        {
            return _dao.ListarPorPagina(_mai_descricao, Pagina, NumeroDePaginas);
        }
        public IList<ManualDPItemDTO> BuscarItensAlterados(int _dias, int _itens)
        {
            return _dao.BuscarItensAlterados(_dias, _itens);
        }
        public Pagina<ManualDPItemDTO> BuscarItensAlterados(int _dias, int _itens, int pagina = 1, int registroPorPagina = 30)
        {
            return _dao.BuscarItensAlterados(_dias, _itens, pagina, registroPorPagina);
        }
        public void Salvar(ManualDPItemDTO _manual)
        {

            var _link = new ManualDPLinkSRV();
            var _fund = new FundamentacaoSRV();

            TransactionOptions txOpt = new TransactionOptions();
            txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            txOpt.Timeout = TransactionManager.MaximumTimeout;

            using (var scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
            {
                var _lista = _fund.Listar(_manual.MAI_ID);

                _fund.DeleteAll(_lista);

                if (_manual.MAI_ID == null)
                {
                    _manual.DATA_INSERT = DateTime.Now;
                    _manual.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                    var index = _dao.BuscarQtdeItensAssunto(_manual.MAN_ID);
                    _manual.MAI_INDEX = (index != null) ? index : 0; 
                    _manual.MAI_NIVEL = 0;
                }

                _manual.DATA_ALTERA = DateTime.Now;
                _manual.USU_LOGIN_ALT = SessionContext.autenticado.USU_LOGIN;

                var _fundamentacao = _manual.FUNDAMENTACAO;
                var _maunaldplink = _manual.MANUAL_DP_LINK;

                _manual = this.SaveOrUpdate(_manual);
                
                //---------- Apaga todos os registros antes de inseri novamente (Fundamentação)

                var _excluir = _fund.Listar(_manual.MAI_ID);

                _fund.DeleteAll(_excluir);
                
                //----------

                foreach (var item in _fundamentacao)
                {
                    if (!String.IsNullOrWhiteSpace(item.MAI_DATA_ATO))
                        if (!SysUtils.IsDate(item.MAI_DATA_ATO))
                            throw new Exception("Data invalida !! (" + item.MAI_DATA_ATO + ")");

                    item.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                    item.DATA_INSERT = DateTime.Now;
                    item.MAI_ID = _manual.MAI_ID;

                    _fund.Save(item);

                }

              
                //---------- Apaga todos os registros antes de inseri novamente (Links)

                var _excluirLnk = _link.Listar(_manual.MAI_ID);

                for(int i=0; i< _excluirLnk.Count; i++)
                {
                    _manualDPLinkDAO.Deletar(_excluirLnk[i].LNK_TAG, _excluirLnk[i].MAI_ID);
                }

                //_link.DeleteAll(_excluirLnk);

                //----------

                foreach (var item in _maunaldplink)
                {

                    item.MAI_ID = _manual.MAI_ID;

                    _link.Save(item);

                }

                scope.Complete();

            }
        }
        public IList<ManualDPItemDTO> MontarIndiceAssunto(int _man_id)
        {
            var _itens = this.ListarPorAssunto(_man_id);

            var  _menu = new List<ManualDPItemDTO>();

            foreach (var root in _itens)
            {
                if (root.MAI_NIVEL == 0)
                    _menu.Add(root);

                if (root.MAI_NIVEL == 1)
                {
                    foreach (var nivel0 in _menu)
                    {
                        if (root.MAI_ID_PAI == nivel0.MAI_ID)
                        {   
                            if (nivel0.MANUAL_DP_ITEM1 == null)
                                nivel0.MANUAL_DP_ITEM1 = new List<ManualDPItemDTO>();

                            nivel0.MANUAL_DP_ITEM1.Add(root);
                        }

                    }

                }

                if (root.MAI_NIVEL == 2)
                {
                    foreach (var nivel0 in _menu)
                    {
                        foreach (var nivel1 in nivel0.MANUAL_DP_ITEM1)
                        {
                            if (root.MAI_ID_PAI == nivel1.MAI_ID)
                            {
                                if (nivel1.MANUAL_DP_ITEM1 == null)
                                    nivel1.MANUAL_DP_ITEM1 = new List<ManualDPItemDTO>();

                                nivel1.MANUAL_DP_ITEM1.Add(root);
                            }

                        }
                    }

                }

    

            }

            return _menu;
        }

        public IList<ManualDPItemDTO> BuscarUltimosItemsAlteradosEPublicadosPorData(DateTime dataParametro)
        {
            IList<ManualDPItemDTO> manualDPItemDTO = _dao.BuscarItemAlteradosEPublicadosPorData(dataParametro);
            return manualDPItemDTO;
        }
    }
}
