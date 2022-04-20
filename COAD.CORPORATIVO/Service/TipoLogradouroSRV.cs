using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Service.Base;

namespace COAD.CORPORATIVO.Service
{
    public class TipoLogradouroSRV : ServiceAdapter<TIPO_LOGRADOURO, TipoLogradouroDTO, int>
    {
        private TipoLogradouroDAO _dao = new TipoLogradouroDAO();

        public TipoLogradouroSRV()
        {
            SetDao(_dao);
        }


    }
}
