using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.DAO;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Exceptions;
using GenericCrud.Config.DataAttributes;
using System.Transactions;

namespace COAD.SEGURANCA.Service
{
    [ServiceConfig("ITM_ID")]
    public class ItemMenuSRV : ServiceAdapter<ITEM_MENU, ItemMenuModel>
    {
        public ItemMenuDAO _dao = new ItemMenuDAO();
        public ItemMenuSRV()
        {
            SetDao(_dao);
        }

        public int BuscarNextID()
        {
            return _dao.BuscarNextID();
        }

        public IList<ItemMenuModel> Listar(int _itm_tipo, int _itm_memu_nivel, string _sis_id, int _itm_id_node)
        {
            return _dao.Listar(_itm_tipo, _itm_memu_nivel, _sis_id, _itm_id_node);
        }

        public IList<ItemMenuModel> ListarMenu(string _sis_id, int? _itm_memu_nivel, int? id_node)
        {
            return _dao.ListarMenu(_sis_id, _itm_memu_nivel, id_node);
        }

        public Pagina<ItemMenuModel> ListarMenu(string _sis_id, int? _itm_memu_nivel, int? pagina, int? registrosPorPagina)
        {
            return _dao.ListarMenu(_sis_id, _itm_memu_nivel, pagina, registrosPorPagina);
        }

        public Pagina<ItemMenuModel> ListarItemMenuPaginas(int? _itm_nivel, string _sis_id, int? _itm_id_node, int numpagina = 1, int linhas = 10)
        {
            return _dao.ListarItemMenuPaginas(_itm_nivel, _sis_id, _itm_id_node, numpagina, linhas);
        }

        public List<ITEM_MENU> BuscarPorTipo(int _itm_tipo, string _sis_id)
        {
            return new ItemMenuDAO().BuscarPorTipo(_itm_tipo, _sis_id);
        }
        public List<ITEM_MENU> BuscarLista(string _sis_id)
        {
            return new ItemMenuDAO().BuscarLista(_sis_id);
        }
        public ItemMenuModel BuscarPorId(int _itm_id)
        {
            return new ItemMenuDAO().BuscarPorId(_itm_id);
        }
        public void IncluirRegistro(ItemMenuModel _item_menu)
        {
            ITEM_MENU itm = new ITEM_MENU();
            itm.ITM_ID = (_item_menu.ITM_ID != null) ? (int) _item_menu.ITM_ID : 0;
            itm.ITM_DESCRICAO = _item_menu.ITM_DESCRICAO;
            itm.ITM_ARQUIVO = _item_menu.ITM_ARQUIVO;
            itm.ITM_NODE_ID = _item_menu.ITM_NODE_ID;
            itm.ITM_MENU_SEQ = _item_menu.ITM_MENU_SEQ;
            itm.ITM_MENU_NIVEL = _item_menu.ITM_MENU_NIVEL;
            itm.ITM_TIPO = _item_menu.ITM_TIPO;
            itm.ITM_EXTERNO = _item_menu.ITM_EXTERNO;
            itm.ITM_ATIVO = _item_menu.ITM_ATIVO;
            itm.ITM_RETORNO = _item_menu.ITM_RETORNO;
            itm.ITM_NOME_ARQUIVO = _item_menu.ITM_NOME_ARQUIVO;
            itm.SIS_ID = _item_menu.SIS_ID;

            new ItemMenuDAO().IncluirReg(itm);
        }
        public void SalvarRegistro(ItemMenuModel _item_menu)
        {
            ITEM_MENU itm = new ITEM_MENU();
            itm.ITM_ID = (_item_menu.ITM_ID != null) ? (int)_item_menu.ITM_ID : 0;
            itm.ITM_DESCRICAO = _item_menu.ITM_DESCRICAO;
            itm.ITM_ARQUIVO = _item_menu.ITM_ARQUIVO;
            itm.ITM_NODE_ID = _item_menu.ITM_NODE_ID;
            itm.ITM_MENU_SEQ = _item_menu.ITM_MENU_SEQ;
            itm.ITM_MENU_NIVEL = _item_menu.ITM_MENU_NIVEL;
            itm.ITM_TIPO = _item_menu.ITM_TIPO;
            itm.ITM_EXTERNO = _item_menu.ITM_EXTERNO;
            itm.ITM_ATIVO = _item_menu.ITM_ATIVO;
            itm.ITM_RETORNO = _item_menu.ITM_RETORNO;
            itm.ITM_NOME_ARQUIVO = _item_menu.ITM_NOME_ARQUIVO;
            itm.SIS_ID = _item_menu.SIS_ID;

            new ItemMenuDAO().SalvarReg(itm);
        }
        public void ExcluirReg(ItemMenuModel _item_menu)
        {
            ITEM_MENU itm = new ITEM_MENU();
            itm.ITM_ID = (_item_menu.ITM_ID != null) ? (int)_item_menu.ITM_ID : 0; 
            itm.ITM_DESCRICAO = _item_menu.ITM_DESCRICAO;
            itm.ITM_ARQUIVO = _item_menu.ITM_ARQUIVO;
            itm.ITM_NODE_ID = _item_menu.ITM_NODE_ID;
            itm.ITM_MENU_SEQ = _item_menu.ITM_MENU_SEQ;
            itm.ITM_MENU_NIVEL = _item_menu.ITM_MENU_NIVEL;
            itm.ITM_TIPO = _item_menu.ITM_TIPO;
            itm.ITM_EXTERNO = _item_menu.ITM_EXTERNO;
            itm.ITM_ATIVO = _item_menu.ITM_ATIVO;
            itm.ITM_RETORNO = _item_menu.ITM_RETORNO;
            itm.ITM_NOME_ARQUIVO = _item_menu.ITM_NOME_ARQUIVO;
            itm.SIS_ID = _item_menu.SIS_ID;

            new ItemMenuDAO().Excluir(itm);
        }
        public IList<ItemMenuModel> GetByUserLogin(string USU_LOGIN, string sisId, int? empId)
        {
            return _dao.GetByUserLogin(USU_LOGIN, sisId, empId);
        }
        public ItemMenuPerfilModel TemAcessoFuncionalidade(string _sisid, int _empid, string _perid, string _path)
        {
            return _dao.TemAcessoFuncionalidade(_sisid, _empid, _perid, _path);
        }


        public Pagina<ItemMenuModel> ItensDeMenu(
            int? ITM_TIPO = null,
            int? ITM_MENU_NIVEL = null,
            string path = null,
            string SIS_ID = null,
            int? ITM_ID_NODE = null,
            string ITM_DESCRICAO = null,
            int pagina = 1,
            int registrosPorPagina = 15)
        {

            var itensDeMenu = _dao.ItensDeMenu(ITM_TIPO, ITM_MENU_NIVEL, path, SIS_ID, ITM_ID_NODE, ITM_DESCRICAO, pagina, registrosPorPagina);
            PreencherItemMenuPai(itensDeMenu.lista);
            return itensDeMenu;

        }

        public void PreencherItemMenuPai(IEnumerable<ItemMenuModel> itensdeMenu)
        {
            if (itensdeMenu != null && itensdeMenu.Count() > 0)
            {
                foreach (var itemMenu in itensdeMenu)
                {
                    PreencherItemMenuPaiEVerificarExistenciaDeSubMenus(itemMenu);
                }
            }
        }

        /// <summary>
        /// Obtem os submenus de nível abaixo do menu de id passado
        /// </summary>
        /// <param name="ITM_ID"></param>
        /// <returns></returns>
        public IList<ItemMenuModel> ObterListaSubmenus(int? ITM_ID)
        {
            
                if (ITM_ID == null)
                {
                    throw new ValidacaoException("Informe do id do menu para obter seus submenus");
                }

            return _dao.ObterListaSubmenus(ITM_ID);
        }

        /// <summary>
        /// Verifica se o menu possui submenus e nível abaixo dele
        /// </summary>
        /// <param name="ITM_ID"></param>
        /// <returns></returns>
        public bool PossuiSubMenus(int? ITM_ID)
        {
            return _dao.PossuiSubMenus(ITM_ID);
        }

        /// <summary>
        /// Pega o menu pai e verifica se o menu em questão possui submenus
        /// </summary>
        /// <param name="itemMenu"></param>
        public void PreencherItemMenuPaiEVerificarExistenciaDeSubMenus(ItemMenuModel itemMenu)
        {
            if (itemMenu != null && itemMenu.ITM_ID != itemMenu.ITM_NODE_ID) // Verifico se o item de menu possui um pai
            {
                var itmId = itemMenu.ITM_NODE_ID;
                var menuPai = FindById(itmId);

                if (menuPai != null)
                {
                    menuPai.ITEM_MENU1 = new List<ItemMenuModel>();
                }

                itemMenu.ITEM_MENU2 = menuPai;

                itemMenu.possuiSubMenus = PossuiSubMenus(itemMenu.ITM_ID);
            }
        }

        public IList<ItemMenuModel> FindBySisId(string _sis_id)
        {
            return _dao.FindBySisId(_sis_id);
        }

        public void SalvarItemMenu(ItemMenuModel itemMenu)
        {
            using(var scope = new TransactionScope()){

                SaveOrUpdateNonIdentityKeyEntity(itemMenu);

                new ItemMenuPerfilSRV().CriarAPartirDoMenu(itemMenu);
                scope.Complete();
            }
        }

        /// <summary>
        /// Retorna o último código gerado para sugerir ser usado como código no item de menu
        /// </summary>
        /// <returns></returns>
        public int? GetSugestaoCodigo()
        {
            int? codigo = _dao.GetUltimoCodigo();

            if (codigo != null)
            {
                return ++codigo;
            }
            return codigo;
        }
    }
}
