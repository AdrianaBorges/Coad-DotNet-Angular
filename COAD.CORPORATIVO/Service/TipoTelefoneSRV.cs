using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;

namespace COAD.CORPORATIVO.Service
{
    public class TipoTelefoneSRV : ServiceAdapter<TIPO_TELEFONE, TipoTelefoneDTO, string>
    {
        private TipoTelefoneDAO _dao = new TipoTelefoneDAO();

        public TipoTelefoneSRV()
        {
            SetDao(_dao);
        }
    }
}
