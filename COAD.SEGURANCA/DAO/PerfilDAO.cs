using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using COAD.SEGURANCA.Model;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;

namespace COAD.SEGURANCA.DAO
{
    [DAOConfig("coadsys")]
    public class PerfilDAO : DAOAdapter<PERFIL, PerfilModel, object>
    {
        public COADSYSEntities db { set; get; }

        public PerfilDAO(bool useDbContextCache = true)
            : base(useDbContextCache)
        {
            db = GetDb<COADSYSEntities>(false);
        }

        public PerfilModel VericarAcessoPerfil(string _per_id, string _dp_nome, int _niv_acesso)
        {
            var _perfil = (from d in db.PERFIL
                           where d.DEPARTAMENTO.DP_NOME == _dp_nome &&
                                 d.NIV_ACE_ID == _niv_acesso &&
                                 d.PER_ID == _per_id
                           select d).FirstOrDefault();


            return ToDTO(_perfil);
        }
        public Boolean VerificarNivelAcesso(string _per_id, string _dp_nome, int _niv_acesso)
        {
            var _perfil = (from d in db.PERFIL
                           where d.DEPARTAMENTO.DP_NOME == _dp_nome &&
                                 d.NIVEL_ACESSO.NIV_ACE_NIVEL == _niv_acesso &&
                                 d.PER_ID == _per_id
                           select d).Count() > 0;


            return _perfil;
        }

        /// <summary>
        /// Verifica se o usuário possui no mínimo o nível no perfil pertencente a um departamento.
        /// </summary>
        /// <param name="_per_id"></param>
        /// <param name="_dp_nome"></param>
        /// <param name="_niv_acesso"></param>
        /// <returns></returns>
        public Boolean VerificarNivelAcessoMinimo(string _per_id, string _dp_nome, int _niv_acesso)
        {
            var count = (from d in db.PERFIL
                           where d.DEPARTAMENTO.DP_NOME == _dp_nome &&
                                 d.NIVEL_ACESSO.NIV_ACE_NIVEL <= _niv_acesso &&
                                 d.PER_ID == _per_id
                           select d).Count();


            return (count > 0);
        }

        public PerfilModel BuscarPorId(int _emp_id, string _per_id)
        {
            PerfilModel _perfil = (from p in db.PERFIL
                                   where (p.EMP_ID == _emp_id) &&
                                         (p.PER_ID == _per_id)
                                   select new PerfilModel()
                                   {
                                       SIS_ID = p.SIS_ID,
                                       EMP_ID = p.EMP_ID,
                                       PER_ID = p.PER_ID,
                                       PER_ATIVO = p.PER_ATIVO,
                                       PER_HORA_INI = p.PER_HORA_INI,
                                       PER_HORA_FIM = p.PER_HORA_FIM,
                                       PER_OUTROS_NIVEIS = p.PER_OUTROS_NIVEIS
                                   }).First();


            return _perfil;
        }
        public List<ITEM_MENU_PERFIL> MontaMenuPerfil(string _per_id)
        {
            List<ITEM_MENU_PERFIL> _itemMenu = (from p in db.ITEM_MENU_PERFIL
                                               where p.PER_ID == _per_id
                                             orderby p.ITEM_MENU.ITM_MENU_NIVEL,
                                                     p.ITEM_MENU.ITM_NODE_ID
                                              select p).ToList();

             return _itemMenu;
        }
        public List<PerfilModel> BuscarLista(int _emp_id, string _per_id, string _sis_id)
        {
            List<PerfilModel> _perfil = (from p in db.PERFIL
                                         where ((_per_id == "" || _per_id == null) || (_per_id != "" && p.PER_ID.StartsWith(_per_id))) &&
                                               ((_sis_id == "" || _sis_id == null) || (_sis_id != "" && p.SIS_ID == _sis_id)) &&
                                               ((_emp_id == 0) || (_emp_id > 0 && p.EMP_ID == _emp_id))
                                         select new PerfilModel() { SIS_ID = p.SIS_ID, 
                                                                    EMP_ID = p.EMP_ID, 
                                                                    PER_ID = p.PER_ID, 
                                                                    PER_ATIVO  = p.PER_ATIVO,
                                                                    PER_HORA_INI = p.PER_HORA_INI,
                                                                    PER_HORA_FIM = p.PER_HORA_FIM,
                                                                    PER_OUTROS_NIVEIS = p.PER_OUTROS_NIVEIS
                                                                   }).ToList();

            return _perfil;
        }
        public List<PerfilModel> BuscarLista(string _per_id, string _sis_id)
        {
            List<PerfilModel> _perfil = (from p in db.PERFIL
                                         where ((_per_id == "" || _per_id == null) || (_per_id != "" && p.PER_ID.StartsWith(_per_id))) &&
                                               ((_sis_id == "" || _sis_id == null) || (_sis_id != "" && p.SIS_ID == _sis_id))
                                         select new PerfilModel()
                                         {
                                             SIS_ID = p.SIS_ID,
                                             EMP_ID = p.EMP_ID,
                                             PER_ID = p.PER_ID,
                                             PER_ATIVO = p.PER_ATIVO,
                                             PER_HORA_INI = p.PER_HORA_INI,
                                             PER_HORA_FIM = p.PER_HORA_FIM,
                                             PER_OUTROS_NIVEIS = p.PER_OUTROS_NIVEIS
                                         }).ToList();

            return _perfil;
        }
        public virtual void IncluirReg(PERFIL _perfil)
        {
            List<ITEM_MENU> ListaItemMenu = new ItemMenuDAO().BuscarLista(_perfil.SIS_ID);

            using (TransactionScope scope = new TransactionScope())
            {
                this.Incluir(_perfil);

                foreach (ITEM_MENU _item in ListaItemMenu)
                {
                    ITEM_MENU_PERFIL imp = new ITEM_MENU_PERFIL();
                    imp.EMP_ID = _perfil.EMP_ID;
                    imp.PER_ID = _perfil.PER_ID;
                    imp.ITM_ID = _item.ITM_ID;
                    imp.NIV_ACESSO = 0;
                    imp.NIV_INSERT = 0;
                    imp.NIV_EDIT = 0;
                    imp.NIV_DELETE = 0;

                    new ItemMenuPerfilDAO().Incluir(imp);
                }

                scope.Complete();
            }
        }

        public void SalvarMenuPerfil(List<Menu> _listamenu)
        {
            
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (Menu _item in _listamenu)
                {
                    ITEM_MENU_PERFIL imp = new ITEM_MENU_PERFIL();
                    imp.EMP_ID     = _item.MenuEmpId;
                    imp.PER_ID     = _item.MenuPerid;
                    imp.ITM_ID     = _item.MenuId;
                    imp.NIV_ACESSO = _item.MenuNivAcesso;
                    imp.NIV_INSERT = _item.MenuNivInsert;
                    imp.NIV_EDIT   = _item.MenuNivEdit;
                    imp.NIV_DELETE = _item.MenuNivDelete;

                    new ItemMenuPerfilDAO().Salvar(imp);

                    if (_item.MenuItens.Count > 0)
                    {
                        foreach (Menu _subMenu in _item.MenuItens)
                        {
                            ITEM_MENU_PERFIL imp2 = new ITEM_MENU_PERFIL();
                            imp2.EMP_ID     = _subMenu.MenuEmpId;
                            imp2.PER_ID     = _subMenu.MenuPerid;
                            imp2.ITM_ID     = _subMenu.MenuId;
                            imp2.NIV_ACESSO = _subMenu.MenuNivAcesso;
                            imp2.NIV_INSERT = _subMenu.MenuNivInsert;
                            imp2.NIV_EDIT   = _subMenu.MenuNivEdit;
                            imp2.NIV_DELETE = _subMenu.MenuNivDelete;

                            new ItemMenuPerfilDAO().Salvar(imp2);

                            if (_subMenu.MenuItens.Count > 0)
                            {
                                foreach (Menu _subMenuSub in _subMenu.MenuItens)
                                {
                                    ITEM_MENU_PERFIL imp3 = new ITEM_MENU_PERFIL();
                                    imp3.EMP_ID = _subMenuSub.MenuEmpId;
                                    imp3.PER_ID = _subMenuSub.MenuPerid;
                                    imp3.ITM_ID     = _subMenuSub.MenuId;
                                    imp3.NIV_ACESSO = _subMenuSub.MenuNivAcesso;
                                    imp3.NIV_INSERT = _subMenuSub.MenuNivInsert;
                                    imp3.NIV_EDIT   = _subMenuSub.MenuNivEdit;
                                    imp3.NIV_DELETE = _subMenuSub.MenuNivDelete;

                                    new ItemMenuPerfilDAO().Salvar(imp3);
                                }
                            }
                        }
                    }

                }

                scope.Complete();
            }
        }

        public Pagina<PerfilModel> Perfis(string nome = null, string sysId = null, int? DP_ID = null, int? NIV_ACE_ID = null, int pagina = 1, int registrosPorPagina = 10)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                nome = null;
            }

            if (string.IsNullOrWhiteSpace(sysId))
            {
                sysId = null;
            }

            var query = GetDbSet().Where(x =>
                (nome == null || x.PER_ID.Contains(nome)) &&
                (sysId == null || x.SIS_ID == sysId) &&
                (DP_ID == null || x.DP_ID == DP_ID) &&
                (NIV_ACE_ID == null || x.NIV_ACE_ID == NIV_ACE_ID));

            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        public IList<PerfilModel> ListarPerfis(string nome = null, string sysId = null, int? DP_ID = null, int? NIV_ACE_ID = null, int pagina = 1, int registrosPorPagina = 10)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                nome = null;
            }

            if (string.IsNullOrWhiteSpace(sysId))
            {
                sysId = null;
            }

            var query = GetDbSet().Where(x =>
                (nome == null || x.PER_ID.Contains(nome)) &&
                (sysId == null || x.SIS_ID == sysId) &&
                (DP_ID == null || x.DP_ID == DP_ID) &&
                (NIV_ACE_ID == null || x.NIV_ACE_ID == NIV_ACE_ID));

            return ToDTO(query);
        }
        
    }
}
