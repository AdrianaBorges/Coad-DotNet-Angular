using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(FORNECEDOR))]
    public class FornecedorDTO
    {
        public FornecedorDTO()
        {
            this.AUDITORIA = new HashSet<AuditoriaDTO>();
            //this.DOCS_CPAGAR = new HashSet<DocsCpagarDTO>();
            //this.FORNECEDOR_HISTORICO = new HashSet<FornecedorHistoricoDTO>();
            //this.FORNECEDOR_MOV_FINAN = new HashSet<FornecedorMovFinanDTO>();
            this.NOTA_FISCAL = new HashSet<NotaFiscalDTO>();
            //this.PRODUTO_FORNECEDOR = new HashSet<ProdutoFornecedorDTO>();
            this.TOTAL_VENDAS_CARTAO = new HashSet<TotalVendasCartaoDTO>();
            this.NOTA_FISCAL_LOTE_ITEM = new HashSet<NotaFiscalLoteItemDTO>();
        }

        [DisplayName("Código")]
        public int FOR_ID { get; set; }
        [DisplayName("Razão Social")]
        public string FOR_RAZAO_SOCIAL { get; set; }
        [DisplayName("Nome Fantasia")]
        public string FOR_NOME_FANTASIA { get; set; }
        [DisplayName("Tipo Pessoa")]
        public string FOR_TIPESSOA { get; set; }
        [DisplayName("CNPJ")]
        public string FOR_CNPJ { get; set; }
        [DisplayName("Endereço")]
        public string FOR_ENDERECO { get; set; }
        [DisplayName("Complemento")]
        public string FOR_END_COMPLEMENTO { get; set; }
        [DisplayName("Bairro")]
        public string FOR_BAIRRO { get; set; }
        [DisplayName("Municipio")]
        public Nullable<int> MUN_ID { get; set; }
        [DisplayName("CEP")]
        public string FOR_CEP { get; set; }
        [DisplayName("Telefone")]
        public string FOR_TEL { get; set; }
        [DisplayName("Celular")]
        public string FOR_CEL { get; set; }
        [DisplayName("Fax")]
        public string FOR_FAX { get; set; }
        [DisplayName("Inscrição")]
        public string FOR_INSCRICAO { get; set; }
        [DisplayName("Contato")]
        public string FOR_CONTATO { get; set; }
        public Nullable<System.DateTime> FOR_DTMOV { get; set; }
        public Nullable<System.DateTime> FOR_DTCAD { get; set; }
        public Nullable<System.DateTime> FOR_DTNASC { get; set; }
        [DisplayName("Status")]
        public string FOR_ATIVO { get; set; }
        [DisplayName("Email")]
        public string FOR_EMAIL { get; set; }
        [DisplayName("Inscrição Municipal")]
        public string FOR_INSCMUNIP { get; set; }
        [DisplayName("Suframa")]
        public string FOR_INSCSUFRAMA { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }
        public string FOR_END_NUMERO { get; set; }
        public string IBGE_COD_COMPLETO { get; set; }
        public Nullable<int> TIPO_FOR_ID { get; set; }
        public string FOR_COD_PAIS { get; set; }

        public string UF { get; set; }
        public int? COD_UF { get; set; }
        
        public virtual ICollection<AuditoriaDTO> AUDITORIA { get; set; }
        //public virtual ICollection<DocsCpagarDTO> DOCS_CPAGAR { get; set; }
        public virtual MunicipioDTO MUNICIPIO { get; set; }
        //public virtual ICollection<FornecedorHistoricoDTO> FORNECEDOR_HISTORICO { get; set; }
        //public virtual ICollection<FornecedorMovFinanDTO> FORNECEDOR_MOV_FINAN { get; set; }
        //public virtual TipoFornecedorDTO TIPO_FORNECEDOR { get; set; }
        public virtual ICollection<NotaFiscalDTO> NOTA_FISCAL { get; set; }
        //public virtual ICollection<ProdutoFornecedorDTO> PRODUTO_FORNECEDOR { get; set; }
        public virtual ICollection<TotalVendasCartaoDTO> TOTAL_VENDAS_CARTAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<NotaFiscalLoteItemDTO> NOTA_FISCAL_LOTE_ITEM { get; set; }


    }
}
