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
    public class GrupoSRV : GenericService<GRUPO, GrupoDTO, int>
    {
        public GrupoDAO _dao = new GrupoDAO();

        public GrupoSRV()
        {
            this._dao = new GrupoDAO();
            Dao = _dao;
        }

        public GrupoSRV(GrupoDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }
    }
}
