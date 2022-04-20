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
    public class TelefoneProspectDAO : AbstractGenericDao<TELEFONES_PROSP, TelefoneProspectDTO, string>
    {
        public prospectadosEntities db { get { return GetDb<prospectadosEntities>(); } set { } }

        public TelefoneProspectDAO()
        {
            SetProfileName("prospectados");
            db = GetDb<prospectadosEntities>(false);
        }

        public IList<TelefoneProspectDTO> FindByCodigo(string codigo)
        {
            var query = GetDbSet().Where(x => x.CODIGO == codigo);
            return ToDTO(query);
        }
    }
}
