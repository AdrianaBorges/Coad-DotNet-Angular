using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(DOC_LIQUIDACAO))]
    public partial class DocLiquidacaoDTO
    {
        public DocLiquidacaoDTO()
        {
            this.PARCELA_LIQUIDACAO = new HashSet<ParcelaLiquidacaoDTO>();
        }

        [DisplayName("Sigla")]
        [Required(ErrorMessage = "Por favor, informe a sigla deste documento de liquidação!")]
        public string DLI_SIGLA { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Por favor, informe uma descrição para este documento de liquidação!")]
        public string DLI_DESCRICAO { get; set; }

        public System.DateTime DLI_DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DLI_DATA_EXCLUSAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ParcelaLiquidacaoDTO> PARCELA_LIQUIDACAO { get; set; }
    }
}
