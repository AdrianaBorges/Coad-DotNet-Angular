using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.UTIL.Grafico
{

    public class JsonGraficoDataSource
    {
        public JsonGraficoDataSource()
        {
            this.chart = new tp_chart();
            this.categories = new HashSet<tp_categories>();
            this.dataset = new HashSet<tp_data>();
            this.data = new HashSet<tp_dataPieColumn3d>();
        }
        public virtual tp_chart chart { get; set; }
        /// <summary>
        /// Utilizado para graficos mais complexos (dragcolumn2d e outros)
        /// </summary>
        /// <summary>
        /// Utilizado para graficos (Pie e Column3d)
        /// </summary>
        public virtual ICollection<tp_dataPieColumn3d> data { get; set; }
        /// <summary>
        /// Utilizado para graficos mais complexos (dragcolumn2d e outros)
        /// </summary>
        public virtual ICollection<tp_categories> categories { get; set; }
        /// <summary>
        /// Utilizado para graficos mais complexos (dragcolumn2d e outros)
        /// </summary>
        public virtual ICollection<tp_data> dataset { get; set; }
        public virtual Nullable<decimal> totaldataset { get; set; }

    }
    public partial class tp_categories
    {
        public tp_categories()
        {
            this.category = new HashSet<tp_label>();
        }

        public virtual ICollection<tp_label> category { get; set; }
    }

    //public partial class tp_category
    //{
    //    public tp_category()
    //    {
    //        this.category = new HashSet<tp_label>();
    //    }

    //    public virtual ICollection<tp_label> category { get; set; }
    //}

    public partial class tp_chart
    {
        public string caption { get; set; }
        public string subCaption { get; set; }
        public string captionFontSize { get; set; }
        public string xAxisName { get; set; }
        public string yAxisName { get; set; }
        public string paletteColors { get; set; }
        public string bgColor { get; set; }
        public string showAlternateHGridColor { get; set; }
        public string showBorder { get; set; }
        public string showCanvasBorder { get; set; }
        public string baseFontColor { get; set; }
        public string baseFont { get; set; }
        public string subcaptionFontSize { get; set; }
        public string subcaptionFontBold { get; set; }
        public string usePlotGradientColor { get; set; }
        public string toolTipColor { get; set; }
        public string toolTipBorderThickness { get; set; }
        public string toolTipBgColor { get; set; }
        public string toolTipBgAlpha { get; set; }
        public string toolTipBorderRadius { get; set; }
        public string toolTipPadding { get; set; }
        public string legendBgAlpha { get; set; }
        public string legendBorderAlpha { get; set; }
        public string legendShadow { get; set; }
        public string legendItemFontSize { get; set; }
        public string legendItemFontColor { get; set; }
        public string legendCaptionFontSize { get; set; }
        public string divlineAlpha { get; set; }
        public string divlineColor { get; set; }
        public string divlineThickness { get; set; }
        public string divLineIsDashed { get; set; }
        public string divLineDashLen { get; set; }
        public string divLineGapLen { get; set; }
        public string showlegend { get; set; }
        public string showpercentvalues { get; set; }
        public string formatNumber { get; set; }
        public string formatNumberScale { get; set; }
        public string showValues { get; set; }
        public string numberPrefix { get; set; }
        public string theme { get; set; }
        public string yaxisname { get; set; }
        public string valueFontColor { get; set; }
        public string xaxisname { get; set; }
        public string plottooltext { get; set; }
        public string plotBorderAlpha { get; set; }
        public string plotFillAlpha { get; set; }
        public string showXAxisLine { get; set; }
        public string axisLineAlpha { get; set; }
        public string divLineAlpha { get; set; }
        public string plotGradientColor { get; set; }

    }

    public partial class tp_dataPieColumn3d
    {
        public string label { get; set; }
        public string value { get; set; }
        public decimal? vlrdouble { get; set; }

        public decimal? perc { get; set; }
    }

    public partial class tp_data
    {
        public tp_data()
        {
            this.data = new HashSet<tp_value>();
        }
        public string seriesname { get; set; }
        public virtual ICollection<tp_value> data { get; set; }

    }

    public partial class tp_label
    {
        public string label { get; set; }

    }
    public partial class tp_value
    {
        public string value { get; set; }
    }


    //------------

    public class JsonGraficoResumo
    {
        public JsonGraficoResumo()
        {
            this.grafico = new JsonGraficoDataSource();
        }

        public Nullable<decimal> total { get; set; }
        public Nullable<decimal> cancelada { get; set; }
        public Nullable<decimal> pagos { get; set; }
        public virtual JsonGraficoDataSource grafico { get; set; }
    }

    public class JsonGrafico
    {
        public string label { get; set; }
        public int data { get; set; }
        public object value { get; set; }
        public int intData { get { if (value is int) return (int)value; else return 0; } set { this.value = value; } }
        public decimal decimalData { get { if (value is decimal) return (decimal)value; else return 0; } set { this.value = value; } }
    }
}
