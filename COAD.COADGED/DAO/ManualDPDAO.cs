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
    public class ManualDPDAO : AbstractGenericDao<MANUAL_DP, ManualDPDTO, int>
    {
                public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 
        public ManualDPDAO()
            : base()
        {
            SetProfileName("GED");

            db = GetDb<COADGEDEntities>(false);
        }

        public IList<ManualDPDTO> Listar(string _assunto, int _mod_id)
        {
            var query = (from i in db.MANUAL_DP
                         where ((_assunto == null || _assunto == "") || ((_assunto != null && _assunto != "") && i.MAN_ASSUNTO.Contains(_assunto)))
                            && (i.MOD_ID == _mod_id)
                         select i);

            query = query.OrderBy(x => x.MAN_INDEX);

            return ToDTO(query);

        }
        public IList<ManualDPDTO> Listar(string _assunto = null)
        {
            var query = (from i in db.MANUAL_DP
                         where ((_assunto == null || _assunto == "") || ((_assunto != null && _assunto != "") && i.MAN_ASSUNTO.Contains(_assunto)))
                         select i);

            query = query.OrderBy(x => x.MAN_INDEX);

            return ToDTO(query);

        }
    }
}
