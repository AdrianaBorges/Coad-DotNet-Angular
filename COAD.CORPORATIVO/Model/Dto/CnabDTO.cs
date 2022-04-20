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
    [Mapping(Source = typeof(CNAB))]
    public partial class CnabDTO
    {
        [DisplayName("ID")]
        public Nullable<int> CNB_ID { get; set; }

        [DisplayName("Empresa")]
        [Required(ErrorMessage = "O nome da empresa é obrigatório.")]
        public Nullable<int> EMP_ID { get; set; }

        [DisplayName("Nº Banco")]
        [Required(ErrorMessage = "Nº do Banco (237[Bradesco], 399[HSBC], etc.) é obrigatório.")]
        [MaxLength(3, ErrorMessage = "Por favor, preencha apenas os 3(três) dígitos do nº do banco. Exemplo: (237[Bradesco], 399[HSBC], etc.)")]
        public string BAN_ID { get; set; }

        [DisplayName("Leiaute CNAB")]
        [Required(ErrorMessage = "O Leiaute do CNAB (240 ou 400) é obrigatório.")]
        [MaxLength(3, ErrorMessage = "Por favor, preencha apenas os 3(três) dígitos definidores do Leiaute CNAB. Exemplo: (240 ou 400 posições)")]
        public string CNB_CNAB { get; set; }

        public string CNB_REFERENCIA { get; set; }

        [DisplayName("Arquivo")]
        [Required(ErrorMessage = "O modelo do arquivo (1REMESSA ou 2RETORNO) é obrigatório.")]
        [MaxLength(8, ErrorMessage = "Por favor, preencha apenas os 8(oito) dígitos do modelo do arquivo. Exemplo: (1REMESSA ou 2RETORNO)")]
        public string CNB_ARQUIVO { get; set; }

        [DisplayName("Nº Tipo do registro")]
        [Required(ErrorMessage = "O Nº do tipo do registro (por exemplo: 0-header, 1-detalhe, 9-trailler) é obrigatório.")]
        [MaxLength(1, ErrorMessage = "Por favor, preencha apenas o dígito do tipo do registro. Exemplo: (0-header, 1-detalhe, 9-trailler)")]
        public string CNB_REGISTRO { get; set; }

        [DisplayName("Nome do Campo")]
        [Required(ErrorMessage = "O Nome do Campo é obrigatório.")]
        [MaxLength(50, ErrorMessage = "Por favor, preencha apenas os 50 dígitos do nome do campo.")]
        public string CNB_CAMPO { get; set; }

        [DisplayName("Tipo do Campo")]
        [Required(ErrorMessage = "O Tipo do Campo (T=texto N=número D=data) é obrigatório.")]
        [MaxLength(1, ErrorMessage = "Por favor, preencha apenas o dígito do tipo do campo. Exemplo: (T=texto N=número D=data)")]
        public string CNB_TIPO { get; set; }

        [DisplayName("Coluna Inicial")]
        [Required(ErrorMessage = "A coluna inicial é obrigatória.")]
        public int CNB_INICIO { get; set; }

        [DisplayName("Coluna Final")]
        [Required(ErrorMessage = "A coluna final é obrigatória.")]
        public int CNB_FINAL { get; set; }

        [DisplayName("Tamanho da Informação")]
        [Required(ErrorMessage = "O Tamanho da informação é obrigatória.")]
        public int CNB_TAMANHO { get; set; }

        [DisplayName("Decimais")]
        public Nullable<int> CNB_DECIMAL { get; set; }

        [DisplayName("Conteúdo/Informação")]
        [Required(ErrorMessage = "A Informação ou a TAG da mesma é obrigatória.")]
        [MaxLength(100, ErrorMessage = "Por favor, preencha no máximo os 100(cem) dígitos da informação ou da sua TAG.")]
        public string CNB_CONTEUDO { get; set; }

        [DisplayName("Data do Cadastro")]
        [Required(ErrorMessage = "A Data do Cadastro é obrigatória.")]
        public System.DateTime DATA_CADASTRO { get; set; }

        [DisplayName("Data da Alteração")]
        public Nullable<System.DateTime> DATA_ALTERACAO { get; set; }

        [DisplayName("Data da Exclusão")]
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }

        [DisplayName("Empresa do Grupo COAD?")]
        public bool EMP_GRP_COAD { get; set; }

        public Nullable<int> CCA_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual BancosDTO BANCOS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual EmpresaRefDTO EMPRESA_REF { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual CnabConfigArquivoDTO CNAB_CONFIG_ARQUIVO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual CnabTipoDadosDTO CNAB_TIPO_DADOS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual CnabTipoRegistroDTO CNAB_TIPO_REGISTRO { get; set; }
    }
}
