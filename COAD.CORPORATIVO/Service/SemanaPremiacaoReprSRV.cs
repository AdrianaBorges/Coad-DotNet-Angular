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
	[ServiceConfig("SPR_SEMANA","SPR_DATA_INI","SPR_DATA_FIM","REP_ID")]
    public class SemanaPremiacaoReprSRV : GenericService<SEMANA_PREMIACAO_REPR, SemanaPremiacaoReprDTO, object>
    {
        public SemanaPremiacaoReprDAO _dao { get; set; }

        public SemanaPremiacaoReprSRV()
        {
            _dao = new SemanaPremiacaoReprDAO();
            Dao = _dao;
        }
        public List<SemanaPremiacaoReprDTO> ListarMetaSemanaRep(int _semana, DateTime _dtini, DateTime _dtfim, int _repid)
        {
            return _dao.ListarMetaSemanaRep( _semana,  _dtini,  _dtfim,  _repid);
        }
    }
	
}
