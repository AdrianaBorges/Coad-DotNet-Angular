using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class AutenticacaoLoginUnicoDTO : WebServiceResult
    {
        public AutenticacaoLoginUnicoDTO()
        {
            Assinaturas = new HashSet<LoginUnicoAssinaturaDTO>();
        }

        public bool LogadoPorLoginUnico { get; set; }
        public AssinaturaDTO AssinaturaPrincipal { get; set; }
        public ICollection<LoginUnicoAssinaturaDTO> Assinaturas { get; set; }
        
    }
}
