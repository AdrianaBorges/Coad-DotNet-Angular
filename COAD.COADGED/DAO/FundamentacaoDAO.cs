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
    public class FundamentacaoDAO : AbstractGenericDao<FUNDAMENTACAO, FundamentacaoDTO, int>
    {
        public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 
        public FundamentacaoDAO()
            : base()
        {
            SetProfileName("GED");

            db = GetDb<COADGEDEntities>(false);
        }
        public IList<FundamentacaoDTO> Listar(int? _mai_id)
        {
            var query = (from c in db.FUNDAMENTACAO
                         where c.MAI_ID == _mai_id
                         select c);

            return ToDTO(query);
        }

    }
}
