using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using COAD.SEGURANCA.Repositorios.Contexto;
using System.ComponentModel;
using GenericCrud.Config.DataAttributes.Maps;
using COAD.SEGURANCA.Model.Dto;

namespace COAD.SEGURANCA.Model
{
    [Mapping(typeof(EMPRESA))]
    public class EmpresaModel
    {
        public EmpresaModel()
        {
            this.PERFIL = new HashSet<PerfilModel>();
            this.CONTABILISTA = new ContabilistaDTO();
            this.CONTA = new HashSet<ContaDTO>();
            this.CONTA1 = new HashSet<ContaDTO>();
        }

        [Required(ErrorMessage = "O campo id deve ser informado")]
        public int EMP_ID { get; set; }

        [DisplayName("Razão Social")]
        [Required(ErrorMessage = "O campo razao social deve ser informado", AllowEmptyStrings = false)]
        public string EMP_RAZAO_SOCIAL { get; set; }
        
        [Required(ErrorMessage = "O campo nome fantasia deve ser informado", AllowEmptyStrings = false)]
        public string EMP_NOME_FANTASIA { get; set; }
        [Required(ErrorMessage = "O campo cnpj deve ser informado", AllowEmptyStrings = false)]
        public string EMP_CNPJ { get; set; }
        public string EMP_IE { get; set; }
        public string EMP_IM { get; set; }
        public string EMP_SUFRAMA { get; set; }
        public string EMP_LOGRADOURO { get; set; }
        public string EMP_NUMERO { get; set; }
        public string EMP_COMPLEMENTO { get; set; }
        public string EMP_BAIRRO { get; set; }
        public string EMP_CEP { get; set; }
        public string EMP_TEL1 { get; set; }
        public string EMP_TEL2 { get; set; }
        public string EMP_TEL3 { get; set; }
        public string EMP_EMAIL { get; set; }
        public string EMP_SITE { get; set; }
        public Nullable<int> EMP_ULTIMA_NFE { get; set; }
        public string EMP_CNR_AGCEDENTE { get; set; }
        public Nullable<int> EMP_AREA { get; set; }
        public Nullable<int> EMP_TIPO { get; set; }
        public string EMP_IE_ST { get; set; }
        public Nullable<int> CNT_ID { get; set; }
        public string IBGE_COD_COMPLETO { get; set; }
        public Nullable<int> CID_ID { get; set; }
        public bool EMP_APARECE_PRE_PEDIDO { get; set; }
        public bool EMP_GRP_COAD { get; set; }
        public Nullable<decimal> EMP_DESP_ADM_BOLETO { get; set; }
        public Nullable<decimal> EMP_MORA_MES_BOLETO { get; set; }
        public Nullable<System.DateTime> EMP_ULT_FECHAMENTO_FAT { get; set; }
        public Nullable<int> EMP_ULTIMA_NFSE { get; set; }

        /// <summary>
        /// Data do último faturamento
        /// </summary>
        public Nullable<System.DateTime> EMP_DATA_ULT_FAT { get; set; }

        public string EMP_NOME_CERTIFICADO { get; set; }
        public string EMP_CERTIFICADO_SENHA { get; set; }

        public virtual ICollection<PerfilModel> PERFIL { get; set; }
        public virtual ContabilistaDTO CONTABILISTA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ContaDTO> CONTA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)] // Contas cuja esta empresa é sacadora avalista
        public virtual ICollection<ContaDTO> CONTA1 { get; set; }

        // ALT: 20/09/2016 - adicionados \\
        public string MUN_DESCRICAO { get; set; }
        public string UF { get; set; }
        public int? COD_UF { get; set; }
    }
    
}