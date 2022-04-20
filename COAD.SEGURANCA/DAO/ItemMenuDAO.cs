using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;

namespace COAD.SEGURANCA.DAO
{
    [DAOConfig("coadsys")]
    public class ItemMenuDAO : DAOAdapter<ITEM_MENU, ItemMenuModel>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }

        public ItemMenuDAO()
            : base()
        {
            db = GetDb<COADSYSEntities>(false);
        }
        public int BuscarNextID()
        {
            var max = db.ITEM_MENU.Max(x => x.ITM_ID);
            max += 1;

            return max;

        }
        public ITEM_MENU Buscar(int id)
        {
            var _itm = (from i in db.ITEM_MENU
                        where i.ITM_ID == id
                        select i).First();

            return _itm;
        }
        public List<ITEM_MENU> Listar(string _sistema)
        {
            List<ITEM_MENU> _itm = (from i in db.ITEM_MENU
                                    where (i.SIS_ID == _sistema)
                                    select i).ToList();

            return _itm;
        }
        public List<ITEM_MENU> BuscarPorTipo(int _itm_tipo, string _sis_id)
        {
            List<ITEM_MENU> _item_menu = (from p in db.ITEM_MENU
                                          where (p.ITM_TIPO == _itm_tipo) &&
                                                (p.SIS_ID == _sis_id)
                                          select p).ToList();

            return _item_menu;
        }
        public IList<ItemMenuModel> Listar(int _itm_tipo, int _itm_memu_nivel, string _sis_id, int _itm_id_node)
        {
            var _lista = (from t in db.ITEM_MENU
                          where (t.ITM_TIPO == _itm_tipo) &&
                                (t.ITM_MENU_NIVEL == _itm_memu_nivel) &&
                                (t.SIS_ID == _sis_id) &&
                                ((_itm_id_node == 0) || ((_itm_id_node > 0) && (t.ITM_NODE_ID == _itm_id_node)))
                          orderby t.ITM_DESCRICAO, t.ITM_MENU_SEQ ascending
                          select t).ToList();

            return ToDTO(_lista);
        }

        public IQueryable<ITEM_MENU> TemplateListarMenu(string _sis_id, int? _itm_memu_nivel)
        {
            IQueryable<ITEM_MENU> _lista = (from t in db.ITEM_MENU
                                            where (t.ITM_TIPO == 0)
                                            orderby t.ITM_MENU_NIVEL, t.ITM_MENU_SEQ ascending
                                            select t);

            if (!string.IsNullOrWhiteSpace(_sis_id))
            {
                _lista = _lista.Where(x => x.SIS_ID == _sis_id);
            }

            if (_itm_memu_nivel != null)
            {
                _lista = _lista.Where(x => x.ITM_MENU_NIVEL == _itm_memu_nivel);
            }

            return _lista;
        }
        public IList<ItemMenuModel> ListarMenu(string _sis_id, int? _itm_memu_nivel, int? id_node)
        {
            var query = TemplateListarMenu(_sis_id, _itm_memu_nivel);
            return ToDTO(query);
        }
        public Pagina<ItemMenuModel> ListarMenu(string _sis_id, int? _itm_memu_nivel, int? pagina, int? registrosPorPagina)
        {
            var query = TemplateListarMenu(_sis_id, _itm_memu_nivel);
            return ToDTOPage(query, (int)pagina, (int)registrosPorPagina);
        }

        public Pagina<ItemMenuModel> ListarItemMenuPaginas(int? _itm_nivel, string _sis_id, int? _itm_id_node, int numpagina = 1, int linhas = 10)
        {
            IQueryable<ITEM_MENU> _lista = (from t in db.ITEM_MENU
                                            where ((_itm_nivel == null) || (_itm_nivel != null && t.ITM_MENU_NIVEL == _itm_nivel)) &&
                                                  ((_itm_id_node == null) || ((_itm_id_node != null) && (t.ITM_NODE_ID == _itm_id_node)))
                                            orderby t.ITM_MENU_NIVEL, t.ITM_MENU_SEQ ascending
                                            select t);

            if (!string.IsNullOrWhiteSpace(_sis_id))
            {
                _lista = _lista.Where(x => x.SIS_ID == _sis_id);
            }

            return ToDTOPage(_lista, numpagina, linhas);
        }
        public List<ITEM_MENU> BuscarLista(string _sis_id)
        {
            List<ITEM_MENU> _item_menu = (from p in db.ITEM_MENU
                                          where (p.SIS_ID == _sis_id)
                                          orderby p.ITM_NODE_ID, p.ITM_MENU_NIVEL, p.ITM_MENU_SEQ ascending
                                          select p).ToList();

            return _item_menu;
        }

        public ItemMenuModel BuscarPorId(int _itm_id)
        {
            ItemMenuModel _item_menu = (from p in db.ITEM_MENU
                                        where (p.ITM_ID == _itm_id)
                                        select new ItemMenuModel()
                                        {
                                            ITM_ID = p.ITM_ID,
                                            ITM_DESCRICAO = p.ITM_DESCRICAO,
                                            ITM_ARQUIVO = p.ITM_ARQUIVO,
                                            ITM_NODE_ID = (int)p.ITM_NODE_ID,
                                            ITM_MENU_SEQ = (int)p.ITM_MENU_SEQ,
                                            ITM_MENU_NIVEL = (int)p.ITM_MENU_NIVEL,
                                            ITM_TIPO = (int)p.ITM_TIPO,
                                            ITM_EXTERNO = (int)p.ITM_EXTERNO,
                                            ITM_ATIVO = (int)p.ITM_ATIVO,
                                            ITM_RETORNO = p.ITM_RETORNO,
                                            ITM_NOME_ARQUIVO = p.ITM_NOME_ARQUIVO,
                                            SIS_ID = p.SIS_ID
                                        }).First();

            return _item_menu;
        }
        public virtual void IncluirReg(ITEM_MENU _item_menu)
        {
            try
            {
                int qtde = (from p in db.ITEM_MENU
                            select p).Count();

                var prox_item_Id = 0;

                if (qtde > 0)
                {
                    prox_item_Id = (from p in db.ITEM_MENU
                                    select p).Max(m => m.ITM_ID);
                }

                prox_item_Id += 1;

                _item_menu.ITM_ID = prox_item_Id;

                if (_item_menu.ITM_NOME_ARQUIVO == "" || _item_menu.ITM_NOME_ARQUIVO == null)
                    _item_menu.ITM_NOME_ARQUIVO = "";

                if (_item_menu.ITM_NODE_ID == 0 || _item_menu.ITM_NODE_ID == null)
                    _item_menu.ITM_NODE_ID = _item_menu.ITM_ID;


                if ((_item_menu.ITM_TIPO == 0) && (_item_menu.ITM_NODE_ID == 0 || _item_menu.ITM_NODE_ID == null))
                {
                    _item_menu.ITM_NODE_ID = prox_item_Id;
                }

                List<PerfilModel> ListaPerfil = new PerfilDAO().BuscarLista("", _item_menu.SIS_ID);

                using (TransactionScope scope = new TransactionScope())
                {
                    this.Incluir(_item_menu);

                    foreach (PerfilModel item in ListaPerfil)
                    {

                        ITEM_MENU_PERFIL imp = new ITEM_MENU_PERFIL();
                        imp.EMP_ID = (int) item.EMP_ID;
                        imp.PER_ID = item.PER_ID;
                        imp.ITM_ID = _item_menu.ITM_ID;
                        imp.NIV_ACESSO = 0;
                        imp.NIV_INSERT = 0;
                        imp.NIV_EDIT = 0;
                        imp.NIV_DELETE = 0;

                        new ItemMenuPerfilDAO().Incluir(imp);
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);

                throw;
            }
        }
        public virtual void SalvarReg(ITEM_MENU _item_menu)
        {
            try
            {
                this.Salvar(_item_menu);
            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);
                throw;
            }
        }
        public IList<ItemMenuModel> GetByUserLogin(string USU_LOGIN, string sisId, int? empId)
        {
            IQueryable<ITEM_MENU_PERFIL> query = db.ITEM_MENU_PERFIL;

            query = query.Where(x => (from per_usu in db.PERFIL_USUARIO
                                      where per_usu.USU_LOGIN == USU_LOGIN
                                      select per_usu.PER_ID).Contains(x.PER_ID) &&
                                      x.EMP_ID == empId && x.PERFIL.SIS_ID == sisId
                                      && x.NIV_ACESSO == 1);


            if (!string.IsNullOrWhiteSpace(sisId))
            {
                query = query.Where(x => x.PERFIL.SIS_ID == sisId);
            }

            if (empId != null)
            {
                query = query.Where(x => x.EMP_ID == empId);
            }

            var menus = query.Select(s => s.ITEM_MENU).Distinct();

            return ToDTO(menus);
        }
        public ItemMenuPerfilModel TemAcessoFuncionalidade(string _sisid, int _empid, string _perid, string _path)
        {
            var query = (from i in db.ITEM_MENU_PERFIL
                         where (i.ITEM_MENU.ITM_NOME_ARQUIVO == _path) &&
                               (i.ITEM_MENU.SIS_ID == _sisid) &&
                               (i.EMP_ID == _empid) &&
                               (i.PER_ID == _perid)
                         select i).FirstOrDefault();

            if (query != null)           
            {
                ItemMenuPerfilModel acesso = new ItemMenuPerfilModel();
                acesso.EMP_ID = query.EMP_ID;
                acesso.ITM_ID = query.ITM_ID;
                acesso.PER_ID = query.PER_ID;
                acesso.NIV_ACESSO = query.NIV_ACESSO;
                acesso.NIV_DELETE = query.NIV_DELETE;
                acesso.NIV_EDIT = query.NIV_EDIT;
                acesso.NIV_INSERT = query.NIV_INSERT;
                

                return acesso;
            }
            else
               return null;
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

            if (string.IsNullOrWhiteSpace(SIS_ID))
            {
                SIS_ID = null;
            }          


            IQueryable<ITEM_MENU> _lista = (from t in db.ITEM_MENU
                          where (ITM_TIPO == null || t.ITM_TIPO == ITM_TIPO) &&
                                (ITM_MENU_NIVEL == null || t.ITM_MENU_NIVEL == ITM_MENU_NIVEL) &&
                                (SIS_ID == null || t.SIS_ID == SIS_ID) &&
                                ((ITM_ID_NODE == null) || ((ITM_ID_NODE != null) && (t.ITM_NODE_ID == ITM_ID_NODE)))
                          orderby t.ITM_DESCRICAO, t.ITM_MENU_SEQ ascending
                          select t);

            if(!string.IsNullOrWhiteSpace(path)){

                _lista = _lista.Where(x => x.ITM_NOME_ARQUIVO == path);
            }

            if(!string.IsNullOrWhiteSpace(ITM_DESCRICAO)){

                _lista = _lista.Where(x => x.ITM_DESCRICAO == ITM_DESCRICAO);
            }

            return ToDTOPage(_lista, pagina, registrosPorPagina);
        }

        public IQueryable<ITEM_MENU> TemplatePegaListaSubmenus(int? ITM_ID)
        {
            var query = GetDbSet().Where(x => 
                    x.ITM_NODE_ID == ITM_ID // Trazer todos que tiverem ele como pai
                    && x.ITEM_MENU2.ITM_MENU_NIVEL < x.ITM_MENU_NIVEL // Trazer todos abaixo dele
                    && x.ITM_ID != ITM_ID // Não trazer ele mesmo. Pois se ele for nível 0 ele terá ele mesmo como pai.
                    );

            return query;
        }

        public IList<ItemMenuModel> ObterListaSubmenus(int? ITM_ID)
        {
            var query = TemplatePegaListaSubmenus(ITM_ID);
            return ToDTO(query);
        }

        public bool PossuiSubMenus(int? ITM_ID)
        {
            var query = TemplatePegaListaSubmenus(ITM_ID);
            int count = query.Count();

            return (count > 0);
        }

        public IList<ItemMenuModel> FindBySisId(string _sis_id)
        {
            IQueryable<ITEM_MENU> _item_menu = (from p in db.ITEM_MENU
                                          where (p.SIS_ID == _sis_id)
                                          orderby p.ITM_NODE_ID, p.ITM_MENU_NIVEL, p.ITM_MENU_SEQ ascending
                                          select p);

            return ToDTO(_item_menu);
        }

        /// <summary>
        /// Retorna o último código gerado do item de menu
        /// </summary>
        /// <returns></returns>
        public int? GetUltimoCodigo()
        {
            int codigo = (from p in db.ITEM_MENU select p.ITM_ID).Max();
            return codigo;
        }

    }
    
}
