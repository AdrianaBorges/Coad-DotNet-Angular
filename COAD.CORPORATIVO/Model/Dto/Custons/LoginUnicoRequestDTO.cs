using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class LoginUnicoRequestDTO
    {
        public LoginUnicoRequestDTO()
        {
            Assinaturas = new HashSet<LoginUnicoAssinaturaDTO>();
            Rastreamentos = new HashSet<RastreamentoAlteracaoLoginUnicoDTO>();            
        }

        public AssinaturaDTO AssinaturaPrincipal { get; set; }
        public string SenhaAssinatura { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        //public ClienteDto Cliente { get; set; }
        public ICollection<LoginUnicoAssinaturaDTO> Assinaturas { get; set; }

        public ICollection<RastreamentoAlteracaoLoginUnicoDTO> Rastreamentos { get; set; }

    }
}
