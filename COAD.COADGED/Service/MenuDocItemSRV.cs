using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Service
{
    [ServiceConfig("MNI_ID")]
    public class MenuDocItemSRV : GenericService<MENU_DOC_ITEM, MenuDocItemDTO, int>
    {
        private MenuDocItemDAO _dao = new MenuDocItemDAO();

        public MenuDocItemSRV()
        {
            Dao = _dao;
        }
    }
}
