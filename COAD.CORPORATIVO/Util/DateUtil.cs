using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Util
{
    public static class DateUtil
    {
        public static DateTime? RetornaDiaPrimeiro(DateTime? data)
        {
            if (data == null)
            {
                data = DateTime.Now;
            }
            var dt = (DateTime)data;
            dt = new DateTime(dt.Year, dt.Month, 1);

            return dt;
        }

        public static DateTime? AlteraDia(DateTime? data, int dia)
        {
            if (data == null)
            {
                data = DateTime.Now;
            }
            var dt = (DateTime)data;
            dt = new DateTime(dt.Year, dt.Month, dia);

            return dt;
        }


        public static DateTime? AdicionaMes(DateTime? data, int mes, int? controleDia = null)
        {
            if (data == null)
            {
                data = DateTime.Now;
            }

            data = ((DateTime)data).AddMonths((int)mes);

            var dia = data.Value.Day;
            if (controleDia != null && dia != controleDia)
            {
                var dataTeste = DateUtil.RetornaDateComUltimoDiaDoMes(data);

                var ultimoDiaMes = dataTeste.Value.Day;
                if (dia != ultimoDiaMes)
                {
                    data = DateUtil.AlteraDia(data, controleDia.Value);
                }
            }
            return data;
        }

        public static DateTime? AdicionaDia(DateTime? data, int dias)
        {
            if (data == null)
            {
                data = DateTime.Now;
            }

            data = ((DateTime)data).AddDays((int)dias);
            return data;
        }

        public static DateTime? RetornaDateComUltimoDiaDoMes(DateTime? data)
        {

            if (data == null)
            {
                data = DateTime.Now;
            }

            var dt = (DateTime)data;
            var ano = dt.Year;
            var mes = dt.Month;
            var ultimoDiaDoMes = DateTime.DaysInMonth(ano, mes);

            data = new DateTime(dt.Year, dt.Month, ultimoDiaDoMes);

            return data;
        }

        public static DateTime? CalculaFimDePeriodo(DateTime? dataInicial, int qtdMeses)
        {
            dataInicial = RetornaDiaPrimeiro(dataInicial);

            var dataFinal = AdicionaMes(dataInicial, qtdMeses - 1);
               dataFinal = RetornaDateComUltimoDiaDoMes(dataFinal);

            return dataFinal;

        }
        
    }
}
