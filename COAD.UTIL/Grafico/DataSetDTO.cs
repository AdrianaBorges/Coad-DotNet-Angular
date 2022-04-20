using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.UTIL.Grafico
{
    public class DataSetDTO
    {
        public string seriesName { get; set; }
        public string renderAs { get; set; }
        public int showValues { get; set; }
        public IList<JsonGrafico> data { get; set; }

        public DataSetDTO()
        {
            data = new List<JsonGrafico>();
            renderAs = "column";
        }

        public void AddDataItem(JsonGrafico dataItem)
        {
            if (dataItem != null)
            {
                data.Add(dataItem);
            }
        }

        public void AddDataList(IList<JsonGrafico> dataItem)
        {
            if (dataItem != null)
            {
                data = dataItem;
            }
        }
    }
}
