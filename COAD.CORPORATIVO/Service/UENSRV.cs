using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;

namespace COAD.CORPORATIVO.Service
{
    public class UENSRV : GenericService<UEN, UENDTO, int>
    {
        public UENDAO _dao = new UENDAO();

        public UENSRV()
        {
            this.Dao = _dao;
        }

        public int? ObterUenIdDoRepresentante(int? REP_ID)
        {
            return _dao.ObterUenIdDoRepresentante(REP_ID);
        }
    }
}
