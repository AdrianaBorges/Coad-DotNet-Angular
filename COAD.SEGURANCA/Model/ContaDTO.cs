using System;
using System.ComponentModel;
using COAD.SEGURANCA.Repositorios.Contexto;
using System.ComponentModel.DataAnnotations;
using GenericCrud.Config.DataAttributes.Maps;

namespace COAD.SEGURANCA.Model
{

    [Mapping(Source = typeof(CONTA))]
    public partial class ContaDTO 
    {
        [DisplayName("ID")]
        public Nullable<int> CTA_ID { get; set; }

        [DisplayName("Empresa")]
        [Required(ErrorMessage = "O campo empresa é obrigatório.")]
        public int EMP_ID { get; set; }

        [DisplayName("Banco")]
        [Required(ErrorMessage = "Nº do Banco (237[Bradesco], 399[HSBC], etc.) é obrigatório.")]
        [MaxLength(3, ErrorMessage = "Por favor, preencha apenas os 3(três) dígitos do nº do banco. Exemplo: (237[Bradesco], 399[HSBC], etc.)")]
        public string BAN_ID { get; set; }

        [DisplayName("Agência")]
        [Required(ErrorMessage = "O campo agência é obrigatório.")]
        [MaxLength(10, ErrorMessage = "Por favor, preencha apenas os 10(dez) dígitos da agência")]
        public string CTA_AGENCIA { get; set; }

        [DisplayName("Conta")]
        [Required(ErrorMessage = "O campo conta é obrigatório.")]
        [MaxLength(20, ErrorMessage = "Por favor, preencha apenas os 20(vinte) dígitos da conta")]
        public string CTA_CONTA { get; set; }

        [DisplayName("Tipo")]
        [Required(ErrorMessage = "O campo tipo é obrigatório.")]
        [MaxLength(1, ErrorMessage = "Por favor, preencha apenas o dígito do tipo")]
        public string CTA_TIPO { get; set; }

        [DisplayName("Convênio")]
        [MaxLength(30, ErrorMessage = "Por favor, preencha apenas os 30(trinta) dígitos do convênio")]
        public string CTA_CONVENIO { get; set; }

        [DisplayName("Código CNAB 240")]
        [MaxLength(30, ErrorMessage = "Por favor, preencha apenas os 30(trinta) dígitos.")]
        public string CTA_CODIGO_240 { get; set; }

        [DisplayName("Código CNAB 400")]
        [MaxLength(30, ErrorMessage = "Por favor, preencha apenas os 30(trinta) dígitos.")]
        public string CTA_CODIGO_400 { get; set; }

        [DisplayName("Complemento da Conta")]
        [MaxLength(10, ErrorMessage = "Por favor, preencha apenas os 10(dez) dígitos.")]
        public string CTA_COMPL_CTA_COB { get; set; }

        [DisplayName("Carteira p/Remessa")]
        [MaxLength(10, ErrorMessage = "Por favor, preencha apenas os 10(dez) dígitos.")]
        public string CTA_CARTEIRA_REMESSA { get; set; }

        [DisplayName("Carteira p/Boleto")]
        [MaxLength(10, ErrorMessage = "Por favor, preencha apenas os 10(dez) dígitos.")]
        public string CTA_CARTEIRA_BOLETO { get; set; }

        [DisplayName("Cedente p/Remessa")]
        [MaxLength(20, ErrorMessage = "Por favor, preencha apenas os 20(vinte) dígitos.")]
        public string CTA_CEDENTE_REMESSA { get; set; }

        [DisplayName("Cedente p/Boleto")]
        [MaxLength(20, ErrorMessage = "Por favor, preencha apenas os 20(vinte) dígitos.")]
        public string CTA_CEDENTE_BOLETO { get; set; }

        [DisplayName("% Multa")]
        public Nullable<decimal> CTA_PERC_MULTA { get; set; }

        [DisplayName("% Mora/mês")]
        public Nullable<decimal> CTA_PERC_MORA_MES { get; set; }

        public bool CTA_CEDENTE_EMITE_BOLETO { get; set; }

        public Nullable<int> CTA_ALOCAR_TITULO_DA_EMP_ID { get; set; }

        [DisplayName("Nº Arquivo Enviado")]
        public int CTA_NR_ARQ_ENVIADO { get; set; }

        [DisplayName("Enviar Boleto")]
        public Nullable<bool> CTA_ENVIA_BOLETO { get; set; }

        [DisplayName("Data do Registro")]
        [Required(ErrorMessage = "O campo data do cadastro é obrigatório.")]
        public System.DateTime DATA_CADASTRO { get; set; }

        public string CTA_ESPECIE_DOC { get; set; }
        public string CTA_ESPECIE { get; set; }
        public string CTA_ACEITE { get; set; }
        public string CTA_INSTRUCOES_BOLETO { get; set; }
        public Nullable<int> EMP_ID_S_AVS { get; set; }
        public Nullable<bool> CTA_GERA_NOSSO_NUMERO { get; set; }

        public Nullable<System.DateTime> DATA_ALTERACAO { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public virtual BancosRefDTO BANCOS_REF { get; set; }
        public virtual EmpresaModel EMPRESA { get; set; }

        public Nullable<long> CTA_ULTIMO_NOSSO_NUMERO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual EmpresaModel EMPRESA1 { get; set; } // Empresa sacadora avalista
    }
}