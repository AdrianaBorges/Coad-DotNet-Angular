using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Util;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Coad.GenericCrud.Service.Base;

namespace COAD.CORPORATIVO.Service
{
//
    [ServiceConfig("CMP_ID", "TTP_ID")]
    public class TipoPeriodoSRV : GenericService<TIPO_PERIODO, TipoPeriodoDTO, int>
    {
        private TipoPeriodoDAO _dao;

        public TipoPeriodoSRV()
        {
            this. _dao = new TipoPeriodoDAO();
            Dao = _dao;

        }

        public TipoPeriodoSRV(TipoPeriodoDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }



        public IList<TipoPeriodoDTO> ListarTipoPeriodoDoProduto(int? cmpId)
        {
            return _dao.ListarTipoPeriodoDoProduto(cmpId);
        }

        public DateTime? CalcularPeriodoDeFimDeVigencia(DateTime? date, TipoPeriodoDTO tipoPeriodo, int? periodoMesBonus = null)
        {
            if (date != null && tipoPeriodo != null)
            {
                var qtdMeses = tipoPeriodo.TTP_QTD_MESES;
                //var dataFimVigencia = ((DateTime)date).AddMonths((int) qtdMeses - 1);

                //var ano = dataFimVigencia.Year;
                //var mes = dataFimVigencia.Month;
                //var ultimoDiaDoMes = DateTime.DaysInMonth(ano, mes);

                //dataFimVigencia = new DateTime(dataFimVigencia.Year, dataFimVigencia.Month, ultimoDiaDoMes);
                //return dataFimVigencia;

                if (periodoMesBonus != null)
                    qtdMeses += periodoMesBonus;

                var dataFimVigencia = DateUtil.CalculaFimDePeriodo(date, (int) qtdMeses);
                return dataFimVigencia;

            }

            return null;
        }

    }
}
