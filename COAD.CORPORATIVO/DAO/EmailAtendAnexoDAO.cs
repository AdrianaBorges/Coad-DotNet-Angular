using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class EmailAtendAnexoDAO : DAOAdapter<EMAIL_ATEND_ANEXO, EmailAtendAnexoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public EmailAtendAnexoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public IList<EmailAtendAnexoDTO> BuscarPorEmail(int _eat_id)
        {
            IQueryable<EMAIL_ATEND_ANEXO> query = (from e in db.EMAIL_ATEND_ANEXO
                                                  where e.EAT_ID == _eat_id
                                                 select e);

            return ToDTO(query);
            
        }
    }
}
