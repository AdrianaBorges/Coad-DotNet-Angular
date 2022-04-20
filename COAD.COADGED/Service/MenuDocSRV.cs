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

    [ServiceConfig("MND_ID")]
    public class MenuDocSRV : GenericService<MENU_DOC, MenuDocDTO, int>
    {
        private MenuDocDAO _dao = new MenuDocDAO();

        public MenuDocSRV()
        {
            Dao = _dao;
        }
    }
}
