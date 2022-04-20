using Coad.GenericCrud.Dao.Base;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.DAO
{
    public class MenuDocItemDAO : AbstractGenericDao<MENU_DOC_ITEM, MenuDocItemDTO, int>
    {
                public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 
        public MenuDocItemDAO()
            : base()
        {
            SetProfileName("GED");

            db = GetDb<COADGEDEntities>(false);
        }
    }
}
