using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{

	[ServiceConfig("SPR_SEMANA","SPR_DATA_INI","SPR_DATA_FIM")]
    public class SemanaPremiacaoSRV : GenericService<SEMANA_PREMIACAO, SemanaPremiacaoDTO, object>
    {
        public SemanaPremiacaoDAO _dao { get; set; }

        public SemanaPremiacaoSRV()
        {
            _dao = new SemanaPremiacaoDAO();
        }
    }
	
}
