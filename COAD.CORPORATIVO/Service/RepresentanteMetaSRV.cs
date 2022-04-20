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
   
	[ServiceConfig("REP_ID","TCO_ID","RME_MES","RME_ANO")]
    public class RepresentanteMetaSRV : GenericService<REPRESENTANTE_META, RepresentanteMetaDTO, object>
    {
        public RepresentanteMetaDAO _dao { get; set; }

        public RepresentanteMetaSRV()
        {
            _dao = new RepresentanteMetaDAO();
        }
        public List<RepresentanteMetaDTO> ListarMetaRep(int _mes, int _ano, int _tcoid, int _repid)
        {
            return _dao.ListarMetaRep(_mes, _ano, _tcoid, _repid);
        }
    }

}
