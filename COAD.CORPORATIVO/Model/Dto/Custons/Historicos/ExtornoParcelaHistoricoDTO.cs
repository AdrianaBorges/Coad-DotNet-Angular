using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Historicos
{
    public class ExtornoParcelaHistoricoDTO
    {
        public ExtornoParcelaHistoricoDTO()
        {
            this.Items = new HashSet<ExtornoParcelaHistoricoItemDTO>();
        }

        public int? RepId { get; set; }
        public string NomeRepresentante { get; set; }
        public int? CliId { get; set; }
        public string NomeCliente { get; set; }
        public int? PedCrm { get; set; }
        public int? IpeId { get; set; }
        public int? PpiId { get; set; }
        public int? PstId { get; set; }
        public string Usuario { get; set; }
        public ICollection<ExtornoParcelaHistoricoItemDTO> Items { get; set; }
    }
}
