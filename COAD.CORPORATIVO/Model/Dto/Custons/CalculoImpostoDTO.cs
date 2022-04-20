using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class CalculoImpostoDTO
    {
        public CalculoImpostoDTO()
        {
            ListImpostos = new HashSet<ImpostoDTO>();
        }

        public decimal? valorBruto { get; set; }
        public decimal? totalLiquido { get; set; }
        public decimal? totalDescontado { get; set; }
        public decimal? percentualDescontoTotal { get; set; }
        public decimal? valorServico {get; set;}
        public decimal? valorMaterial {get; set;}

        public ICollection<ImpostoDTO> ListImpostos { get; set; }
    }
}
