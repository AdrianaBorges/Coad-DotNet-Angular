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
    [Mapping(Source = typeof(CNAB_REGISTROS_TEMP))]
    public partial class CnabRegistrosDTO
    {
        public int ID { get; set; }
        public int REM_ID { get; set; }
        public string MUN_CEP { get; set; }
        public string MUN_DESCRICAO { get; set; }
        public Nullable<int> MUN_ID { get; set; }
        public string MUN_TIPO { get; set; }
        public Nullable<int> RG_ID { get; set; }
        public string UF { get; set; }
        public string EMP_BAIRRO { get; set; }
        public string EMP_CEP { get; set; }
        public string EMP_CNPJ { get; set; }
        public string EMP_COMPLEMENTO { get; set; }
        public string EMP_EMAIL { get; set; }
        public int EMP_ID { get; set; }
        public string EMP_IE { get; set; }
        public string EMP_IE_ST { get; set; }
        public string EMP_IM { get; set; }
        public string EMP_LOGRADOURO { get; set; }
        public string EMP_NOME_FANTASIA { get; set; }
        public string EMP_NUMERO { get; set; }
        public string EMP_RAZAO_SOCIAL { get; set; }
        public string EMP_SITE { get; set; }
        public string EMP_TEL1 { get; set; }
        public string EMP_TEL2 { get; set; }
        public string EMP_TEL3 { get; set; }
        public Nullable<int> EMP_TIPO { get; set; }
        public string BAN_ID { get; set; }
        public string CTA_AGENCIA { get; set; }
        public string CTA_CARTEIRA_BOLETO { get; set; }
        public string CTA_CARTEIRA_REMESSA { get; set; }
        public string CTA_CEDENTE_BOLETO { get; set; }
        public string CTA_CEDENTE_REMESSA { get; set; }
        public string CTA_CODIGO_240 { get; set; }
        public string CTA_CODIGO_400 { get; set; }
        public string CTA_COMPL_CTA_COB { get; set; }
        public string CTA_CONTA { get; set; }
        public string CTA_CONVENIO { get; set; }
        public Nullable<int> CTA_ID { get; set; }
        public Nullable<decimal> CTA_PERC_MORA_MES { get; set; }
        public Nullable<decimal> CTA_PERC_MULTA { get; set; }
        public string CTA_TIPO { get; set; }
        public bool CTA_CEDENTE_EMITE_BOLETO { get; set; }
        public Nullable<int> CTA_NR_ARQ_ENVIADO { get; set; }
        public string PAR_NUM_PARCELA { get; set; }
        public decimal PAR_VLR_PARCELA { get; set; }
        public Nullable<System.DateTime> PAR_DATA_VENCTO { get; set; }
        public Nullable<decimal> moraDiaria { get; set; }
        public string CLI_CPF_CNPJ { get; set; }
        public string CLI_NOME { get; set; }
        public string ENDERECO_SACADO { get; set; }
        public string END_BAIRRO { get; set; }
        public string END_CEP { get; set; }
        public string END_MUNICIPIO { get; set; }
        public string END_UF { get; set; }
        public string ENDERECO_CEDENTE { get; set; }
        public Nullable<int> CTA_ALOCAR_TITULO_DA_EMP_ID { get; set; }
        public string PAR_NOSSO_NUMERO { get; set; }

        // Esses campos não vem na procedure
        public string SACADOR_AVALISTA_CNPJ { get; set; }
        public string SACADOR_AVALISTA_RAZAO_SOCIAL { get; set; }

    }
}
