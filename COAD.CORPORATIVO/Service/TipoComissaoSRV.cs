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

	[ServiceConfig("TCO_ID")]
    public class TipoComissaoSRV : GenericService<TIPO_COMISSAO, TipoComissaoDTO, int>
    {
        public TipoComissaoDAO _dao { get; set; }

        public TipoComissaoSRV()
        {
            _dao = new TipoComissaoDAO();
        }
    }
	
	
}
