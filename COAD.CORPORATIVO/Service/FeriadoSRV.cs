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
    [ServiceConfig("FER_ID")]
    public class FeriadoSRV : ServiceAdapter<FERIADO, FeriadoDTO, int>
    {
        private FeriadoDAO _dao = new FeriadoDAO();

        public FeriadoSRV()
        {
            SetDao(_dao);
        }
        public IList<FeriadoMesDTO> ListarAgrupado(int ano)
        {
            return _dao.ListarAgrupado(ano);
        }
        public int BuscarDiasNaoUteis(DateTime _dataini, DateTime _datafim)
        {
            return _dao.BuscarDiasNaoUteis(_dataini, _datafim);
        }
        public int BuscarDiasUteisFeriado(DateTime _dataini, DateTime _datafim)
        {
            return _dao.BuscarDiasUteisFeriado(_dataini, _datafim);
        }
    }
}
