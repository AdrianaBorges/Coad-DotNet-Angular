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
    [Mapping(Source = typeof(OCORRENCIA_RETORNO))]
    public partial class OcorrenciaRetornoDTO
    {
        public OcorrenciaRetornoDTO()
        {
            this.PARCELA_ALOCADA = new HashSet<ParcelaAlocadaDTO>();
        }

        [DisplayName("Banco")]
        [Required(ErrorMessage = "O código do banco é obrigatório.")]
        [StringLength(3, ErrorMessage = "Por favor, preencha os 3(três) dígitos do nº do banco. Exemplo: (237[Bradesco], 399[HSBC], etc.)")]
        public string BAN_ID { get; set; }

        [DisplayName("Código")]
        [StringLength(2, ErrorMessage = "Por favor, preencha os 2(dois) dígitos do código do retorno.")]
        [Required(ErrorMessage = "O código do retorno é obrigatório.")]
        public string OCT_CODIGO { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Por favor, descreva esta ocorrência.")]
        [MaxLength(60, ErrorMessage = "Por favor, informe uma descrição com até 60 dígitos.")]
        public string OCT_DESCRICAO { get; set; }

        [DisplayName("Título deve ser baixado?")]
        public bool OCT_BAIXAR_TITULO { get; set; }

        [DisplayName("Título deve ser desalocado?")]
        public bool OCT_DESALOCAR_TITULO { get; set; }

        [DisplayName("Título deve ser registrado?")]
        public bool OCT_REGISTRAR_TITULO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual BancosDTO BANCOS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ParcelaAlocadaDTO> PARCELA_ALOCADA { get; set; }
    }
}
