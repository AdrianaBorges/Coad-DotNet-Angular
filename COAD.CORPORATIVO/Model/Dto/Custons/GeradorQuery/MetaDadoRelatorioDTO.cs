using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.GeradorQuery
{
    /// <summary>
    /// Informações sobre como será a estrutura da consulta personalizada
    /// </summary>
    public class MetaDadoRelatorioDTO
    {
        public string NomeRelatorio { get; set; }
        public IList<MetadataColuna> Colunas { get; set; }
        public IList<DadosDeFiltroDTO> Filtros { get; set; }

    }

    public class MetadataColuna
    {  
        public string Name { get; set; }
        public string Tipo { get; set; }
    }
}
