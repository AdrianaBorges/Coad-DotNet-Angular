using COAD.UTIL.Grafico.Intarfaces;
using GenericCrud.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.UTIL.Grafico.Base
{
    public class GraficoDataSource : IGraficoDataSource
    {
        public GraficoConfigDTO chart { get; set; }

        public GraficoDataSource()
        {
            chart = new GraficoConfigDTO();
        }

        public void SetTitle(string title)
        {
            chart.caption = title;
        }

        public void SetSubTitle(string subTitle)
        {
            chart.subCaption = subTitle;
        }

    }
}
