using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.LEGADO.Dao;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Service
{
    public class ender_fatSRV : GenericService<ender_fat, ender_fatDTO, object>
    {
        private ender_fatDAO _dao = new ender_fatDAO();

        public ender_fatSRV()
        {
            Dao = _dao;
        }

    }
 
}
