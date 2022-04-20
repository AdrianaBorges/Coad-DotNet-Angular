using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.PORTAL.Model.DTO;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.DAO
{
    public class IndicesDAO: AbstractGenericDao<INDICES_PORTAL_PROC_Result, IndicesDTO, string>
    {
        private COADIARIOEntities db { get; set; }

        public IndicesDAO()
        {
            SetProfileName("portal");
            db = GetDb<COADIARIOEntities>(false);
        }

        public IList<IndicesDTO> Indices()
        {
            IList<INDICES_PORTAL_PROC_Result> query = db.INDICES_PORTAL_PROC().ToList();
            //GetDbSet().Where(x=> x.usuario.Equals(login) && x.senha.Equals(senha)).FirstOrDefault();
            return ToDTO(query);
        }
    }
}
