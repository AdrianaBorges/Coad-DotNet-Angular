using COAD.UTIL.Grafico.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.UTIL.Grafico
{
    public enum TipoGrafico
    {
        Default = 1,
        Categories = 2
    }

    public class JsonGraficoResponse
    {
        public JsonGraficoResponse()
        {
            this.Dados = new List<JsonGrafico>();
        }

        public JsonGraficoResponse(string _titulo, string _descricao, List<JsonGrafico> _dados)
        {
            this.Titulo = _titulo;
            this.Descricao = _descricao;
            this.Dados = _dados;
        }

        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public JsonGraficoResumo DadosResumo { get; set; }
        public IList<JsonGrafico> Dados { get; set; }


        public GraficoDataSource ToDataSource(TipoGrafico tipo = TipoGrafico.Default)
        {
            if (tipo == TipoGrafico.Default)
            {
                return ToDefaultSource();
            }

            return ToDefaultSource();
        }

        public DefaultGraficoDataSource ToDefaultSource()
        {
            DefaultGraficoDataSource dataSource = new DefaultGraficoDataSource()
            {
                chart = new GraficoConfigDTO()
                {
                    caption = this.Titulo,
                    subCaption = this.Descricao
                },

                data = this.Dados
            };

            return dataSource;
        }

    }
    
}
