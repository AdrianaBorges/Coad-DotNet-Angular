using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Service
{

    public class OrigemAcessoRefSRV : GenericService<ORIGEM_ACESSO_REF, OrigemAcessoRefDTO, int>
    {
        private OrigemAcessoRefDAO _dao = new OrigemAcessoRefDAO();

        public OrigemAcessoRefSRV()
        {
            Dao = _dao;
        }

    }
}
