using Coad.GenericCrud.Validations;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(PROPOSTA_ITEM_COMPROVANTE))]
    public class PropostaItemComprovanteDTO
    {
        public int? PIC_ID { get; set; }
        
        [MaxLength(50, ErrorMessage = "O número do documento não pode exceder 50 caracteres.")]
        [RequiredIf("TPG_ID",7, 8, 10, ErrorMessage = "Preecha o número do documento.")]
        public string PIC_NUMERO_DOCUMENTO { get; set; }

        [MaxLength(50, ErrorMessage = "O número do documento não pode exceder 50 caracteres.")]
        public string PIC_COD_REFERENCIA { get; set; }

        [MaxLength(50, ErrorMessage = "O número do documento não pode exceder 50 caracteres.")]
        public string PIC_NUMERO_CONFIRMACAO { get; set; }
        
        public string PIC_TEXTO { get; set; }
        public Nullable<int> PPI_ID { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public Nullable<System.DateTime> PIC_DATA_PAGAMENTO { get; set; }
        public Nullable<System.DateTime> PIC_DATA_TRANSACAO { get; set; }
        public string BAN_ID { get; set; }

        [RequiredIf("TPG_ID", 9, ErrorMessage = "Informe a data de vencimento do cartão")]
        public Nullable<System.DateTime> PIC_VENC_CARTAO { get; set; }
        public Nullable<System.DateTime> PIC_CHEQUE_BOM_PARA { get; set; }

        [RequiredIf("TPG_ID", 9, ErrorMessage = "Informe a bandeira do cartão.")]
        public Nullable<int> BAC_ID { get; set; }
        
        [RequiredIf("TPG_ID", 9, ErrorMessage = "Informe o número do cartão.")]
        public string PIC_NUMERO_CARTAO { get; set; }

        public Nullable<int> TPG_ID { get; set; }
        public string UF_SIGLA { get; set; }
        public Nullable<int> IPE_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual BancosDTO BANCOS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual BandeiraCartaoDTO BANDEIRA_CARTAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]      
        public virtual PropostaItemDTO PROPOSTA_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoPagamentoDTO TIPO_PAGAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual UFDTO UF { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ItemPedidoDTO ITEM_PEDIDO { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Tipo: ");
            sb.Append(TIPO_PAGAMENTO.TPG_DESCRICAO);
            sb.Append(" \n");

            if(TIPO_PAGAMENTO.TPG_ID == 9)
            {
                sb.Append("Bandeira do Cartão: ");
                if(BANDEIRA_CARTAO != null)
                    sb.Append(BANDEIRA_CARTAO.BAC_DESCRICAO);
                sb.Append(" \n, Número do Cartão");
                sb.Append(PIC_NUMERO_CARTAO);
                sb.Append(" \n, Vencimento do Cartão: ");
                sb.Append(PIC_VENC_CARTAO);
                sb.Append(" \n");
            }
            else
            {

                if(TPG_ID == 8 || TPG_ID == 10)
                {
                    sb.Append("Banco: ");
                    if(BANCOS != null)
                    {
                        sb.Append(BANCOS.BAN_NOME);
                        sb.Append(" \n  ");
                    }

                }
                sb.Append("Número do ");

                if (TPG_ID == 8)
                    sb.Append("Cheque");
                else
                if (TPG_ID == 10)
                    sb.Append("Comprovante");
                else
                    sb.Append("Documento");

                sb.Append(PIC_NUMERO_DOCUMENTO);
                sb.Append(" \n , ");

                if(TPG_ID == 8)
                {
                    sb.Append("UF: ");
                    if (UF != null)
                        sb.Append(UF.UF_SIGLA);
                    sb.Append("\n , ");

                }
                if(TPG_ID == 7 || TPG_ID == 10)
                {

                    sb.Append("Código de Referência: ");
                    sb.Append(PIC_COD_REFERENCIA);
                    sb.Append("\n , ");         
                
                }
                sb.Append("Data de Pagamento ");
                sb.Append(PIC_DATA_PAGAMENTO);
                sb.Append("\n , ");
            }

            sb.Append("Informações complementares.");
            sb.Append(PIC_TEXTO);
            
            return sb.ToString();
        }

    }
}
