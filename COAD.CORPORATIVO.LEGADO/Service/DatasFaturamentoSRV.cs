using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.LEGADO.Dao;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Service
{
    [ServiceConfig("PERIODO", "SEMANA")]
    public class DatasFaturamentoSRV : GenericService<datas_fat, DatasFaturamentoDTO, string>
    {
        private DatasFaturamentoDAO _dao = new DatasFaturamentoDAO();

        public DatasFaturamentoSRV()
        {
            Dao = _dao;
        }

        public DatasFaturamentoDTO GetUltimoPeriodoFaturamento()
        {
            return _dao.GetUltimoPeriodoFaturamento();
        }

        /// <summary>
        /// Retorna as datas de faturamento do mês atual e anterior
        /// </summary>
        /// <returns></returns>
        public ICollection<DatasFaturamentoDTO> ListarDataFaturamentoUltimos2Meses()
        {
            return _dao.ListarDataFaturamentoUltimos2Meses();
        }

        public DatasFaturamentoDTO FindByDate(DateTime? data)
        {
            return _dao.FindByDate(data);
        }
    }
}
