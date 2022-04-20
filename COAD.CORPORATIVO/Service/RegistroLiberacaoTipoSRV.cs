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
    public class RegistroLiberacaoTipoSRV : GenericService<REGISTRO_LIBERACAO_TIPO, RegistroLiberacaoTipoDTO, int>
    {
        private RegistroLiberacaoTipoDAO _dao;

        public RegistroLiberacaoTipoSRV()
        {
            this._dao = new RegistroLiberacaoTipoDAO();
            this.Dao = _dao;
        }

        public RegistroLiberacaoTipoSRV(RegistroLiberacaoTipoDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }
    }
}
