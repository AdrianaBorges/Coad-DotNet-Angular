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
    [Mapping(Source = typeof(PRODUTOS))]
    public class ProdutosDTO
    {
        public ProdutosDTO()
        {
            this.ASSINATURA = new HashSet<ASSINATURA>();
            this.NOTA_FISCAL_ITEM = new HashSet<NOTA_FISCAL_ITEM>();
            this.PRODUTO_COMPOSICAO_ITEM = new HashSet<PRODUTO_COMPOSICAO_ITEM>();
            this.PRODUTO_EAN = new HashSet<PRODUTO_EAN>();
            this.PRODUTO_FORNECEDOR = new HashSet<PRODUTO_FORNECEDOR>();
            this.PRODUTO_HISTORICO = new HashSet<PRODUTO_HISTORICO>();
            this.PRODUTO_PERFIL = new HashSet<PRODUTO_PERFIL>();
            this.URA_PRODUTO = new HashSet<URA_PRODUTO>();
            this.AREA_CONSULTORIA = new HashSet<AREA_CONSULTORIA>();

        }
    
        [Key]
        public int? PRO_ID { get; set; }

        [DisplayName("Sigla")]
        [Required(ErrorMessage = "Digite a Sigla")]
        [MaxLength(10,ErrorMessage = "Digite no máximo 10 caracteres")]
        public string PRO_SIGLA { get; set; }

        [DisplayName("Nome")]
        //[Required(ErrorMessage = "Digite o Nome do Produto")]
        [MaxLength(70, ErrorMessage = "Digite no máximo 70 caracteres")]
        public string PRO_NOME { get; set; }


        public Nullable<int> PRO_ID_DERVADO { get; set; }
        //public string PRO_MOD_CARTA_URA { get; set; }
        public Nullable<int> PRO_TIPO_REMESSA { get; set; }
        public Nullable<int> PRO_CODIGO_CORREIO { get; set; }
        public string PRO_SIGLA_CORREIO { get; set; }
        public bool PRO_REMESSA_SEMANAL { get; set; }

        [DisplayName("Manda Mala direta")]
        public Nullable<int> PRO_RECEBE_MALA { get; set; }
        public Nullable<int> PRO_RECEBE_PASTA_SN { get; set; }
        public Nullable<int> PRO_PRODUTO_ACABADO { get; set; }

        [DisplayName("Status")]
        public Nullable<int> PRO_STATUS { get; set; }
        public string PRO_EMITE_NF { get; set; }
        

        [DisplayName("NCM")]
        public string NCM_ID { get; set; }

        [DisplayName("Grupo")]
        [Required(ErrorMessage = "Selecione o Grupo do Produto")]       
        public Nullable<int> GRUPO_ID { get; set; }

        [DisplayName("Data do Cadastro")]
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        
        [DisplayName("Data de Alteração")]
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }

        [DisplayName("Exclusão")]
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }

        [DisplayName("Login Exclusão")]
        public string USU_LOGIN_EXCLUSAO { get; set; }

        [DisplayName("Unidade de Medida da Compra")]
        [MaxLength(2)]
        [Required(ErrorMessage = "Selecione a unidade de Medida da Compra")]       
        public string PRO_UN_COMPRA { get; set; }

        [DisplayName("Unidade de Medida da Venda")]
        [Required(ErrorMessage = "Selecione a unidade de Medida de Venda")]        
        [MaxLength(2)]
        public string PRO_UN_VEND { get; set; }

        //[Required(ErrorMessage = "Digite o preço de Compra")]
        [DisplayName("Preço de Compra")]
        public Nullable<decimal> PRO_PRECO_COMPRA { get; set; }

        [DisplayName("Preço de Custo")]
        //[Required(ErrorMessage = "Digite o preço de Custo")]        
        public Nullable<decimal> PRO_PRECO_CUSTO { get; set; }

        [DisplayName("Área")]
        [Required(ErrorMessage = "Selecione a Área")]
        public Nullable<int> AREA_ID { get; set; }

        [DisplayName("Preço de Venda")]
        public Nullable<decimal> PRO_PRECO_VENDA { get; set; }

        [DisplayName("Tipo de Produto")]
        [Required(ErrorMessage = "Selecione o Tipo do Produto")]
        public Nullable<int> TIPO_PRO { get; set; }

        [DisplayName("Tipo de Comportamento")]
        //[Required(ErrorMessage = "Selecione o tipo de Comportamento do Produto")]        
        public Nullable<int> TPC_ID { get; set; }

        [DisplayName("Vai para a Ura")]
        public bool PRO_URA { get; set; }

        [DisplayName("Produto do portal")]
        public bool PRO_PORTAL { get; set; }

        [DisplayName("Produto do ST")]
        public bool PRO_PORTAL_ST { get; set; }

        [DisplayName("Produto de Venda")]
        public bool PRO_VENDA { get; set; }

        [DisplayName("Linha de Produto")]
        public Nullable<int> LIN_PRO_ID { get; set; }

        [DisplayName("Possui Impresso")]
        public bool PRO_IMPRESSO { get; set; }

        [DisplayName("Familia")]
        public Nullable<int> FAM_ID { get; set; }

        public Nullable<int> OAC_ID { get; set; }

        public Nullable<int> PRO_QTD_CONSULTA_PADRAO { get; set; }
        public string PRO_MOD_CARTA_URA { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual AREAS AREAS { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ASSINATURA> ASSINATURA { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual GRUPO GRUPO { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual LINHA_PRODUTO LINHA_PRODUTO { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<NOTA_FISCAL_ITEM> NOTA_FISCAL_ITEM { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ORIGEM_ACESSO ORIGEM_ACESSO { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PRODUTO_COMPOSICAO_ITEM> PRODUTO_COMPOSICAO_ITEM { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PRODUTO_EAN> PRODUTO_EAN { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual PRODUTO_FAMILIA PRODUTO_FAMILIA { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PRODUTO_FORNECEDOR> PRODUTO_FORNECEDOR { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PRODUTO_HISTORICO> PRODUTO_HISTORICO { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PRODUTO_PERFIL> PRODUTO_PERFIL { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual TIPO_PROD_COMPORTAMENTO TIPO_PROD_COMPORTAMENTO { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual TIPO_PRODUTO TIPO_PRODUTO { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual UNIDADE_MEDIDA UNIDADE_MEDIDA { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual UNIDADE_MEDIDA UNIDADE_MEDIDA1 { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<URA_PRODUTO> URA_PRODUTO { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<AREA_CONSULTORIA> AREA_CONSULTORIA { get; set; }
        




    }
}
