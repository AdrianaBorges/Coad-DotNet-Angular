using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;


namespace COAD.CORPORATIVO.DAO
{
    public class UENDAO : AbstractGenericDao<UEN, UENDTO,int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }
        
        public UENDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public int? ObterUenIdDoRepresentante(int? REP_ID)
        {
            var query = (from rep in db.REPRESENTANTE where rep.REP_ID == REP_ID select rep.UEN_ID).FirstOrDefault();
            return query; 
        }
    }
}
