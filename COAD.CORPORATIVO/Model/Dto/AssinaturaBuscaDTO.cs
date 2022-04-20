using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
     [Mapping(Source = typeof(ASSINATURA))]
    public class AssinaturaBuscaDTO
    {
        public AssinaturaBuscaDTO()
        {
            this.ASSINATURA_EMAIL = new HashSet<AssinaturaEmailDTO>();
            this.ASSINATURA_TELEFONE = new HashSet<AssinaturaTelefoneDTO>();
        }

        public virtual ICollection<AssinaturaEmailDTO> ASSINATURA_EMAIL { get; set; }
        public virtual ICollection<AssinaturaTelefoneDTO> ASSINATURA_TELEFONE { get; set; }
    }
}
