using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.DAO;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Service.Base;
using GenericCrud.Config.DataAttributes;
using System.Transactions;

namespace COAD.SEGURANCA.Service
{
    [ServiceConfig("EMP_ID", "PER_ID", "ITM_ID")]
    public class ItemMenuPerfilSRV : ServiceAdapter<ITEM_MENU_PERFIL, ItemMenuPerfilModel, object>
    {
        private ItemMenuPerfilDAO _dao = new ItemMenuPerfilDAO();
        private ItemMenuSRV _itemMenuService = new ItemMenuSRV();
        private PerfilSRV _perfilService = new PerfilSRV(); 

        public ItemMenuPerfilSRV()
        {
             SetDao(_dao);

        }
        
        public virtual void CriarAPartirDoPerfil(PerfilModel _perfil)
        {
            
            IList<ItemMenuModel> ListaItemMenu = _itemMenuService.FindBySisId(_perfil.SIS_ID);
            IList<ItemMenuPerfilModel> listaMenuPerfilParaSalvar = new List<ItemMenuPerfilModel>();

            foreach (ItemMenuModel _item in ListaItemMenu)
            {
                var existe = ExisteItemMenuPerfil(_perfil.EMP_ID, _perfil.PER_ID, _item.ITM_ID);

                if (!existe)
                {
                    ItemMenuPerfilModel imp = new ItemMenuPerfilModel();
                    imp.EMP_ID = _perfil.EMP_ID;
                    imp.PER_ID = _perfil.PER_ID;
                    imp.ITM_ID = _item.ITM_ID;
                    imp.NIV_ACESSO = 0;
                    imp.NIV_INSERT = 0;
                    imp.NIV_EDIT = 0;
                    imp.NIV_DELETE = 0;

                    listaMenuPerfilParaSalvar.Add(imp);
                }

                    
            }

            if (listaMenuPerfilParaSalvar.Count() > 0)
            {
                SaveAll(listaMenuPerfilParaSalvar);
            }
        }

        public virtual void CriarAPartirDoMenu(ItemMenuModel _itemmenu)
        {

            IList<PerfilModel> ListaItemMenu = _perfilService.ListarPerfis(null, _itemmenu.SIS_ID);
            IList<ItemMenuPerfilModel> listaMenuPerfilParaSalvar = new List<ItemMenuPerfilModel>();

            foreach (PerfilModel _per in ListaItemMenu)
            {
                var existe = ExisteItemMenuPerfil(_per.EMP_ID, _per.PER_ID, _itemmenu.ITM_ID);

                if (!existe)
                {
                    ItemMenuPerfilModel imp = new ItemMenuPerfilModel();
                    imp.EMP_ID = _per.EMP_ID;
                    imp.PER_ID = _per.PER_ID;
                    imp.ITM_ID = _itemmenu.ITM_ID;
                    imp.NIV_ACESSO = 0;
                    imp.NIV_INSERT = 0;
                    imp.NIV_EDIT = 0;
                    imp.NIV_DELETE = 0;

                    listaMenuPerfilParaSalvar.Add(imp);
                }


            }

            if (listaMenuPerfilParaSalvar.Count() > 0)
            {
                SaveAll(listaMenuPerfilParaSalvar);
            }
        }

        public virtual void Criar(ItemMenuModel _itemmenu, PerfilModel _perfil)
        {
            if (_itemmenu != null && _perfil != null)
            {
                var existe = ExisteItemMenuPerfil(_perfil.EMP_ID, _perfil.PER_ID, _itemmenu.ITM_ID);

                if (!existe)
                {
                    ItemMenuPerfilModel imp = new ItemMenuPerfilModel();
                    imp.EMP_ID = _perfil.EMP_ID;
                    imp.PER_ID = _perfil.PER_ID;
                    imp.ITM_ID = _itemmenu.ITM_ID;
                    imp.NIV_ACESSO = 0;
                    imp.NIV_INSERT = 0;
                    imp.NIV_EDIT = 0;
                    imp.NIV_DELETE = 0;

                    Save(imp);
                }
            }
        }
            
        public bool ExisteItemMenuPerfil(int? EMP_ID, string perId, int? itm_id)
        {
            return _dao.ExisteItemMenuPerfil(EMP_ID, perId, itm_id);
        }

        public IList<ItemMenuPerfilModel> ListarPerfilMenu(string PER_ID, string SIS_ID, int? NIVEL, int? ITM_NODE_ID = null)
        {
            return _dao.ListarPerfilMenu(PER_ID, SIS_ID, NIVEL, ITM_NODE_ID);
        }

        
        public IList<ItemMenuPerfilModel> ListarSubPerfilMenu(string PER_ID, string SIS_ID, int? ITM_NODE_ID)
        {
            return ListarPerfilMenu(PER_ID, SIS_ID, null, ITM_NODE_ID);
        }

        public void BuscarItemMenuPerfilRecursivo(IList<ItemMenuPerfilModel> lstItemMenuPerfil)
        {
            if (lstItemMenuPerfil != null && lstItemMenuPerfil.Count() > 0)
            {
                foreach (var item in lstItemMenuPerfil)
                {
                    var list = ListarSubPerfilMenu(item.PER_ID, item.ITEM_MENU.SIS_ID, item.ITEM_MENU.ITM_ID);
                    BuscarItemMenuPerfilRecursivo(list);
                    
                    item.ITEM_MENU.ITEM_MENU1 = new List<ItemMenuModel>();
                    item.ITEM_MENU.ITEM_MENU2 = null;

                    if (list != null)
                    {
                        item.SUB_ITEM_MENU = list;
                    }
                    
                }  
                
            }
            return;
        }

        public IList<ItemMenuPerfilModel> ListarPerfilMenuCompleto(string PER_ID, string SIS_ID)
        {
            if (!string.IsNullOrWhiteSpace(PER_ID))
            {
                using (var scope = new TransactionScope())
                {
                    var perfil = _perfilService.FindById(1, PER_ID);
                    CriarAPartirDoPerfil(perfil);

                    scope.Complete();
                }
            }

            _dao.ReloadContext();

            var lstItemMenuPerfil = ListarPerfilMenu(PER_ID, SIS_ID, 0);

            if (lstItemMenuPerfil != null && lstItemMenuPerfil.Count() > 0)
            {
                BuscarItemMenuPerfilRecursivo(lstItemMenuPerfil);                               
            }

            return lstItemMenuPerfil;
        }

        public void SalvarItemMenuPerfil(IEnumerable<ItemMenuPerfilModel> lstItemMenuPerfil)
        {
            using (var scope = new TransactionScope())
            {
                SalvarItemMenuPerfilRecursivo(lstItemMenuPerfil);
                scope.Complete();
            }
        }

        public void SalvarItemMenuPerfilRecursivo(IEnumerable<ItemMenuPerfilModel> lstItemMenuPerfil)
        {
            if (lstItemMenuPerfil != null)
            {
                SaveOrUpdateNonIdentityKeyEntity(lstItemMenuPerfil, "ExisteItemMenuPerfil");

                foreach (var itemMenuPerfil in lstItemMenuPerfil)
                {
                    if (itemMenuPerfil.SUB_ITEM_MENU != null && itemMenuPerfil.SUB_ITEM_MENU.Count() > 0)
                    {
                        SalvarItemMenuPerfilRecursivo(itemMenuPerfil.SUB_ITEM_MENU);
                    }
                }
            }


        }
    }
}

