using Coad.GenericCrud.Service.Base;
using COAD.PROSPECTADOS.Dao;
using COAD.PROSPECTADOS.Model.Dto;
using COAD.PROSPECTADOS.Repositorios.Contexto.Base;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROSPECTADOS.Service
{
    [ServiceConfig("CODIGO")]
    public class ProspectsSRV : GenericService<prospects, ProspectsDTO, string>
    {
        private ProspectsDAO _dao = new ProspectsDAO();

        public ProspectsSRV()
        {
            Dao = _dao;
        }
    }
}
