using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using GenericCrud.Excel.Metadatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(IMPORTACAO_SUSPECT))]
    public class ImportacaoSuspectDTO : ICliente 
    {
        public ImportacaoSuspectDTO()
        {
            this.CLIENTES = new HashSet<ClienteDto>();
            this.IMPORTACAO_HISTORICO = new HashSet<ImportacaoHistoricoDTO>();            
        }

        [ExcelColumn(Name = "CODIGO")]
        public int? IPS_ID { get; set; }

        [ExcelColumn(Name = "NOME")]
        public string IPS_NOME { get; set; }

        [ExcelColumn(Name = "CPF_CNPJ")]
        public string IPS_CPF_CNPJ { get; set; }

        [ExcelColumn(Name = "CONTATO")]
        public string IPS_CONTATO { get; set; }

        [ExcelColumn(Name = "TELEFONE")]
        public string IPS_TELEFONE { get; set; }

        [ExcelColumn(Name = "FAX")]
        public string IPS_FAX { get; set; }

        [ExcelColumn(Name = "CELULAR")]
        public string IPS_CELULAR { get; set; }

        [ExcelColumn(Name = "EMAIL")]
        public string IPS_EMAIL { get; set; }

        [ExcelColumn(Name = "UF")]
        public string IPS_UF { get; set; }

        [ExcelColumn(Name = "CIDADE")]
        public string IPS_CIDADE { get; set; }

        [ExcelColumn(Name = "BAIRRO")]
        public string IPS_BAIRRO { get; set; }

        [ExcelColumn(Name = "CLASSIFICACAO")]
        public string IPS_CLASSIFICACAO { get; set; }

        [ExcelColumn(Name = "REGIAO")]
        public string IPS_REGIAO { get; set; }

        [ExcelColumn(Name = "TIPO_CLIENTE")]
        public string IPS_TIPO_CLIENTE { get; set; }

        [ExcelColumn(Name = "PRODUTO_INTERESSE")]
        public string IPS_PRODUTO_INTERESSE { get; set; }

        [ExcelColumn(Name = "ORIGEM_CADASTRO")]
        public string IPS_ORIGEM_CADASTRO { get; set; }

        [ExcelColumn(Name = "AREA_INTERESSE")]
        public string IPS_AREA_INTERESSE { get; set; }

        [ExcelColumn(Name = "COMENTARIO_CLIENTE")]
        public string IPS_COMENTARIO_CLIENTE { get; set; }

        [ExcelColumn(Name = "CODIGO_REPRESENTANTE")]
        public Nullable<int> IPS_REP_ID { get; set; }

        [ExcelColumn(Name = "TIPO_IMPORTACAO")]
        public string IPS_TIPO_IMPORTACAO { get; set; }

        //[ExcelColumn(Name = "Erros", CommentFrom = "UltimoHistoricoRegistrado")]
        [ExcelIgnore]
        public string Erros { get; set; }

        [ExcelColumn(Name = "ERROS")]
        public string UltimoHistoricoRegistrado { get; set; }

        public void AlterarDados(ImportacaoSuspectDTO importacaoSus)
        {
            this.IPS_NOME = importacaoSus.IPS_NOME;
            this.IPS_ORIGEM_CADASTRO = importacaoSus.IPS_ORIGEM_CADASTRO;
            this.IPS_PRODUTO_INTERESSE = importacaoSus.IPS_PRODUTO_INTERESSE;
            this.IPS_REGIAO = importacaoSus.IPS_REGIAO;
            this.IPS_TELEFONE = importacaoSus.IPS_TELEFONE;
            this.IPS_TIPO_CLIENTE = importacaoSus.IPS_TIPO_CLIENTE;
            this.IPS_UF = importacaoSus.IPS_UF;
            this.IPS_FAX = importacaoSus.IPS_FAX;
            this.IPS_CPF_CNPJ = importacaoSus.IPS_CPF_CNPJ;
            this.IPS_CONTATO = importacaoSus.IPS_CONTATO;
            this.IPS_EMAIL = importacaoSus.IPS_EMAIL;
            this.IPS_BAIRRO = importacaoSus.IPS_BAIRRO;
            this.IPS_CELULAR = importacaoSus.IPS_CELULAR;
            this.IPS_CIDADE = importacaoSus.IPS_CIDADE;
            this.IPS_CLASSIFICACAO = importacaoSus.IPS_CLASSIFICACAO;
            this.IPS_AREA_INTERESSE = importacaoSus.IPS_AREA_INTERESSE;
            this.IPS_COMENTARIO_CLIENTE = importacaoSus.IPS_COMENTARIO_CLIENTE;
            this.IPS_REP_ID = importacaoSus.IPS_REP_ID;
            this.IPS_TIPO_IMPORTACAO = importacaoSus.IPS_TIPO_IMPORTACAO;            
        }


        [ExcelIgnore]
        public Nullable<int> RG_ID { get; set; }
        [ExcelIgnore]
        public Nullable<int> CMP_ID { get; set; }
        
        [ExcelIgnore]
        public Nullable<int> O_CAD_ID { get; set; }

        [ExcelIgnore]
        public Nullable<int> AREA_ID { get; set; }

        [ExcelIgnore]
        public Nullable<int> IMS_ID { get; set; }

        [ExcelIgnore]
        public Nullable<int> IMP_ID { get; set; }

        [ExcelIgnore]
        public Nullable<System.DateTime> IMP_DATA_ULTIMA_EXECUCAO { get; set; }

        [ExcelIgnore]
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual AreasCorpDTO AREAS { get; set; }

        [ExcelIgnore]
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual OrigemCadastroDTO ORIGEM_CADASTRO { get; set; }

        [ExcelIgnore]
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ProdutoComposicaoDTO PRODUTO_COMPOSICAO { get; set; }

        [ExcelIgnore]
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RegiaoDTO REGIAO { get; set; }

        [ExcelIgnore]
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ClienteDto> CLIENTES { get; set; }

        [ExcelIgnore]
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ImportacaoStatusDTO IMPORTACAO_STATUS { get; set; }

        [ExcelIgnore]
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ImportacaoDTO IMPORTACAO { get; set; }

        [ExcelIgnore]
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ImportacaoHistoricoDTO> IMPORTACAO_HISTORICO { get; set; }

        [ExcelIgnore]
        public string CNPJ_CPF
        {
            get { return this.IPS_CPF_CNPJ; }
        }


        [ExcelIgnore]
        public IEnumerable<string> TELEFONE
        {
            get
            {
                var obj = new HashSet<string>();
                if (!string.IsNullOrWhiteSpace(this.IPS_TELEFONE))
                {
                    Regex rgx = new Regex(@"\D");
                    obj.Add(rgx.Replace(this.IPS_TELEFONE, ""));
                }
                return obj;
            }
        }

        [ExcelIgnore]
        public IEnumerable<string> FAX
        {
            get
            {
                var obj = new HashSet<string>();
                if (!string.IsNullOrWhiteSpace(this.IPS_FAX))
                {
                    Regex rgx = new Regex(@"\D");
                    obj.Add(rgx.Replace(this.IPS_FAX, ""));
                }
                return obj;
            }
        }

        [ExcelIgnore]
        public IEnumerable<string> CELULAR
        {
            get
            {
                var obj = new HashSet<string>();
                if (!string.IsNullOrWhiteSpace(this.IPS_CELULAR))
                {
                    Regex rgx = new Regex(@"\D");
                    obj.Add(rgx.Replace(this.IPS_CELULAR, ""));
                }
                return obj;
            }
        }


        [ExcelIgnore]
        public IEnumerable<string> EMAIL
        {
            get
            {   var obj = new HashSet<string>();
                if (!string.IsNullOrWhiteSpace(this.IPS_EMAIL))
                {
                    Regex rgx = new Regex(@"\D");
                    obj.Add(rgx.Replace(this.IPS_EMAIL, ""));
                }
                return obj;
            }
        }

    }
}
