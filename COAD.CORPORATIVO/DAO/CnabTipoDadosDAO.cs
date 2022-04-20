

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
    public class CnabTipoDadosDAO : AbstractGenericDao<CNAB_TIPO_DADOS, CnabTipoDadosDTO, String>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public CnabTipoDadosDAO()
        {
            db = GetDb<CORPORATIVOContext>(false);
        }

        public bool TipoExiste(string tipo)
        {
            if (string.IsNullOrWhiteSpace(tipo))
                return false;

            var query = (from cnTpD in
                             db.CNAB_TIPO_DADOS

                         where
                            cnTpD.CTD_ID.Contains(tipo)
                         select cnTpD);
            return (query.Count() > 0);
        }

        
    }
}
