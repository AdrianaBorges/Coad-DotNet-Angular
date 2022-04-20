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

    public class OrigemAcessoRefDAO : AbstractGenericDao<ORIGEM_ACESSO_REF, OrigemAcessoRefDTO, int>
    {

        public OrigemAcessoRefDAO()
            : base()
        {
            SetProfileName("GED");

        }
    }
}
