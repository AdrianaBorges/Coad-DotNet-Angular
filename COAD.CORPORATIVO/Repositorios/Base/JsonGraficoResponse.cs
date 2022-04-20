using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Repositorios.Base
{

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
        public List<JsonGrafico> Dados { get; set; }

    }

    public class JsonGrafico
    {
        public string label { get; set; }
        public int data { get; set; }

    }
}
