using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("NIV_ACE_ID")]
    public class NivelAcessoSRV : GenericService<NIVEL_ACESSO, NivelAcessoDTO, int>
    {
        public NivelAcessoDAO _dao = new NivelAcessoDAO();

        public NivelAcessoSRV()
        {
            this.Dao = _dao;
        }

    }
}
