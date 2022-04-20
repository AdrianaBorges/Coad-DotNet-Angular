using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;

namespace COAD.SEGURANCA.DAO
{
    [DAOConfig("coadsys")]
    public class ItemMenuPerfilDAO : DAOAdapter<ITEM_MENU_PERFIL, ItemMenuPerfilModel, object>
    {
        public ItemMenuPerfilDAO()
        {
        }

        private IQueryable<ITEM_MENU_PERFIL> _Template(int? EMP_ID, string perId, int? itm_id)
        {
            var query = GetDbSet().Where(x => x.EMP_ID == EMP_ID && x.PER_ID == perId && x.ITM_ID == itm_id);
            return query;            
        }

        public bool ExisteItemMenuPerfil(int? EMP_ID, string perId, int? itm_id)
        {
            var query = _Template(EMP_ID, perId, itm_id);
            int count = query.Count();

            return (count > 0);
        }

        public IList<ItemMenuPerfilModel> ListarPerfilMenu(string PER_ID, string SIS_ID, int? NIVEL = null, int? ITM_NODE_ID = null)
        {
            var query = GetDbSet().Where(x => x.PER_ID == PER_ID && x.ITEM_MENU.SIS_ID == SIS_ID && x.ITEM_MENU.ITM_ATIVO == 1);

            if (NIVEL != null && ITM_NODE_ID == null)
            {
                query = query.Where(x => x.ITEM_MENU.ITM_MENU_NIVEL == NIVEL);
            }


            if (ITM_NODE_ID != null)
            {
                var nivel = GetDbSet().Where(x => x.ITM_ID == ITM_NODE_ID).Select(sel => sel.ITEM_MENU.ITM_MENU_NIVEL).FirstOrDefault() ;

                if (nivel != null)
                {
                    nivel++;
                    query = query.Where(x => x.ITEM_MENU.ITM_ID != ITM_NODE_ID && x.ITEM_MENU.ITM_NODE_ID == ITM_NODE_ID && (ITM_NODE_ID == null || x.ITEM_MENU.ITM_MENU_NIVEL == nivel));
                }
            }
            return ToDTO(query);
        }
       

    }
}
