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
    public class ManualDPLinkDAO : AbstractGenericDao<MANUAL_DP_LINK, ManualDPLinkDTO, string>
    {

        public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } }

        public ManualDPLinkDAO()
            : base()
        {
            SetProfileName("GED");

            db = GetDb<COADGEDEntities>(false);
        }

        public IList<ManualDPLinkDTO> Listar(int? _mai_id)
        {
            var query = (from c in db.MANUAL_DP_LINK
                         where c.MAI_ID == _mai_id
                         select c);

            return ToDTO(query);
        }
        public IList<ManualDPLinkDTO> ListarPorModulo(int? _man_id)
        {
            var query2 = (from c in db.MANUAL_DP
                          join m in db.MANUAL_DP_ITEM on c.MAN_ID equals m.MAN_ID
                          where m.MAN_ID == _man_id
                          select c).FirstOrDefault();

            var query = (from c in db.MANUAL_DP_LINK
                         join m in db.MANUAL_DP_ITEM on c.MAI_ID equals m.MAI_ID
                         join d in db.MANUAL_DP on m.MAN_ID equals d.MAN_ID
                        where d.MOD_ID == query2.MOD_ID
                         select c);

            return ToDTO(query);
        }

        public void Deletar(string linkTag, int? manualDpIntId)
        {
            var query = (from c in db.MANUAL_DP_LINK
                         where c.LNK_TAG == linkTag &&
                         c.MAI_ID == manualDpIntId
                         select c).FirstOrDefault();

            if(query != null)
            {
                db.MANUAL_DP_LINK.Remove(query);
            }
        }
    }
}
