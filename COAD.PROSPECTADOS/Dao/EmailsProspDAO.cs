using Coad.GenericCrud.Dao.Base;
using COAD.PROSPECTADOS.Model.Dto;
using COAD.PROSPECTADOS.Repositorios.Contexto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROSPECTADOS.Dao
{
    public class EmailsProspDAO : AbstractGenericDao<EMAILS_PROSP, EmailsProspDTO, string>
    {
        public prospectadosEntities db { get { return GetDb<prospectadosEntities>(); } set { } }

        public EmailsProspDAO()
        {
            SetProfileName("prospectados");
            db = GetDb<prospectadosEntities>(false);
        }

        public IList<EmailsProspDTO> FindByCodigo(string codigo)
        {
            var query = GetDbSet().Where(x => x.CODIGO == codigo);
            return ToDTO(query);
        }
    }
}
