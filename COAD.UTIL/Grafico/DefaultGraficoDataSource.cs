using COAD.UTIL.Grafico.Base;
using COAD.UTIL.Grafico.Intarfaces;
using GenericCrud.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.UTIL.Grafico
{
    public class DefaultGraficoDataSource : GraficoDataSource
    {
        public ICollection<JsonGrafico> data { get; set; }
        
    }
}
