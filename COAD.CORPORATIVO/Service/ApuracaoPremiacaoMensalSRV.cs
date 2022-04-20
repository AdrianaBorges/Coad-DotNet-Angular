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
	 
    [ServiceConfig("REP_ID","APU_MES","APU_ANO")]
    public class ApuracaoPremiacaoMensalSRV : GenericService<APURACAO_PREMIACAO_MENSAL, ApuracaoPremiacaoMensalDTO, object>
    {
        public ApuracaoPremiacaoMensalDAO _dao { get; set; }

        public ApuracaoPremiacaoMensalSRV()
        {
            _dao = new ApuracaoPremiacaoMensalDAO();
        }
        public List<ApuracaoPremiacaoMensalDTO> ListarApuracaoVendas(int _mes, int _ano, int _repid)
        {
            return _dao.ListarApuracaoVendas(_mes, _ano, _repid);
        }
    }

}
