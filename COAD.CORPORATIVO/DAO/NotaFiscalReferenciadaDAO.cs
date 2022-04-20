

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;

namespace COAD.CORPORATIVO.DAO
{
    public class NotaFiscalReferenciadaDAO : AbstractGenericDao<NOTA_FISCAL_REFERENCIADA, NotaFiscalReferenciadaDTO, Int32>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public NotaFiscalReferenciadaDAO()
        {
            db = GetDb<CORPORATIVOContext>(false);
        }

        public ICollection<NotaFiscalReferenciadaDTO> ListarNotasReferenciadasPorLoteItem(int? loteItm)
        {
            var query = (from
                            ntRef in db.NOTA_FISCAL_REFERENCIADA
                         where
                            ntRef.NLI_ID == loteItm
                         select ntRef);
            return ToDTO(query);
        }
        
    }
}
