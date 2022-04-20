using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(CLIENTE_USUARIO))]
    public class ClienteUsuarioDTO 
    {
        public ClienteUsuarioDTO()
        {
            Assinaturas = new HashSet<LoginUnicoAssinaturaDTO>();
        }

        public int? USC_ID { get; set; }
        public string USC_LOGIN { get; set; }
        public string USC_SENHA { get; set; }
        public Nullable<bool> USC_ATIVO { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public Nullable<int> CLI_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ClienteDto CLIENTES { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual AssinaturaDTO ASSINATURA { get; set; }

        /// <summary>
        /// Esse atributo não existe no banco
        /// </summary>
        public ICollection<LoginUnicoAssinaturaDTO> Assinaturas { get; set; }

        public string COD_ASSINATURA_PRINCIPAL { get; set; }

        

    }
}
