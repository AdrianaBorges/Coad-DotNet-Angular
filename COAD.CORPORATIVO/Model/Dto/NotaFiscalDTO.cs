using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.FISCAL.Model.Integracoes.Interfaces;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.FISCAL.Model.Integracoes.Enumerados;
using System.Web.Script.Serialization;

namespace COAD.CORPORATIVO.Model.Dto
{

    public class TipoNotaFiscalCustomDTO
    {
        public TipoNotaFiscalCustomDTO()
        {
            this.NotaFiscalDTO = new HashSet<NotaFiscalDTO>();
        }
        public Nullable<int> NF_TIPO { get; set; }
        public Nullable<decimal> NF_VLR_TOTAL { get; set; }
        public virtual ICollection<NotaFiscalDTO> NotaFiscalDTO { get; set; }
    }
    public class NotaFiscalCustomDTO
    {
        public Nullable<int> EMP_ID { get; set; }
        public string EMP_RAZAO_SOCIAL { get; set; }
        public Nullable<int> NF_QTDE { get; set; }
        public Nullable<decimal> NF_VLR_ENTRADA { get; set; }
        public Nullable<decimal> NF_VLR_ENTRADA_SERV { get; set; }
        public Nullable<decimal> NF_VLR_SAIDA { get; set; }
        public Nullable<decimal> NF_VLR_SAIDA_SERV { get; set; }
    }

    [Mapping(typeof(NOTA_FISCAL))]
    public class NotaFiscalDTO : INotaFiscal
    {
        public NotaFiscalDTO()
        {
            this.NOTA_FISCAL_ITEM = new HashSet<NotaFiscalItemDTO>();
            this.NOTA_FISCAL_LOTE_ITEM = new HashSet<NotaFiscalLoteItemDTO>();
            this.NOTA_FISCAL_EVENTO = new HashSet<NotaFiscalEventoDTO>();
        }

        public int NF_ID { get; set; }
        public int NF_TIPO { get; set; }
        public int NF_NUMERO { get; set; }
        public string NF_SERIE { get; set; }
        public Nullable<System.DateTime> NF_DATA_EMISSAO { get; set; }
        public Nullable<System.DateTime> NF_DATA_SAIDA { get; set; }
        public Nullable<decimal> NF_BASE_CALC_ICMS { get; set; }
        public Nullable<decimal> NF_VLR_ICMS { get; set; }
        public Nullable<decimal> NF_BASE_CALC_ST { get; set; }
        public Nullable<decimal> NF_VLR_ST { get; set; }
        public Nullable<decimal> NF_VLR_FRETE { get; set; }
        public Nullable<decimal> NF_VLR_SEGURO { get; set; }
        public Nullable<decimal> NF_VLR_OUTRAS { get; set; }
        public Nullable<decimal> NF_VLR_IPI { get; set; }
        public Nullable<decimal> NF_VLR_PROD { get; set; }
        public Nullable<decimal> NF_VLR_NOTA { get; set; }
        public Nullable<decimal> NF_VLR_DESCONTO { get; set; }
        public string NF_CHAVE { get; set; }
        public string NF_NOTA_XML { get; set; }
        public Nullable<int> FOR_ID { get; set; }
        public string CFOP { get; set; }
        public Nullable<int> NF_FRETE_TIPO { get; set; }
        public string NF_RNTRC { get; set; }
        public string NF_PLACA { get; set; }
        public Nullable<decimal> NF_QTDE { get; set; }
        public string NF_ESPECIE { get; set; }
        public string NF_MARCA { get; set; }
        public Nullable<decimal> NF_PESO_BRUTO { get; set; }
        public Nullable<decimal> NF_PESO_LIQUIDO { get; set; }
        public Nullable<int> TRA_ID { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public string PED_NUM_PEDIDO { get; set; }
        public string NF_PLACA_UF { get; set; }
        public string NF_NUMERACAO { get; set; }
        public Nullable<System.DateTime> NF_DATA_ENTRADA { get; set; }
        public string NF_PROTOCOLO_AUT { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public string CFOPENT { get; set; }
        public Nullable<decimal> NF_VLR_PIS { get; set; }
        public Nullable<decimal> NF_VLR_COFINS { get; set; }
        public Nullable<int> NF_TIPO_LIGACAO { get; set; }
        public Nullable<int> NF_COD_GRUPO_TENSAO { get; set; }
        public string NF_COD_CONSUMO { get; set; }
        public string NF_HORA_SAIDA { get; set; }
        public string NF_STATUS { get; set; }
        public string NF_INF_COMPLEMENTAR { get; set; }
        public Nullable<System.DateTime> NF_DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> NF_DATA_ALTERACAO { get; set; }
        public string NF_COD_SIT { get; set; }
        public Nullable<decimal> NF_VLR_SERVICO { get; set; }
        public Nullable<decimal> NF_BASE_CALC_ISSQN { get; set; }
        public Nullable<decimal> NF_VLR_ISSQN { get; set; }
        public string TDF_ID { get; set; }
        public string NF_PATH_ARQUIVO { get; set; }
        public Nullable<decimal> NF_ALIQ_ICMS_SERV { get; set; }
        public string CST_ID_SERV { get; set; }
        public Nullable<decimal> NF_VLR_INSS { get; set; }
        public Nullable<decimal> NF_VLR_IR { get; set; }
        public Nullable<decimal> NF_VLR_CSLL { get; set; }
        public Nullable<decimal> NF_VLR_ISS { get; set; }
        public Nullable<decimal> NF_VLR_ISS_RETIDO { get; set; }
        public Nullable<decimal> NF_ALIQUODA { get; set; }
        public string NF_COD_VERIFICACAO { get; set; }

        [ScriptIgnore]
        public byte[] NF_ARQUIVO { get; set; }
        public string NF_EMAIL { get; set; }
        public string CTR_NUM_CONTRATO { get; set; }
        public Nullable<int> IPE_ID { get; set; }
        public string NF_NUMERO_PROTOCOLO { get; set; }
        public Nullable<int> NLI_ID { get; set; }
        public string NF_NOME_ARQ_EVENTO { get; set; }

        [ScriptIgnore]
        public byte[] NF_ARQ_EVENTO { get; set; }
        public Nullable<int> NST_ID { get; set; }
        public Nullable<bool> NF_EVENTO_ANEXADO { get; set; }
        public Nullable<decimal> NF_VLR_FCP { get; set; }
        public Nullable<decimal> NF_VLR_FCP_ST { get; set; }
        public Nullable<decimal> NF_VLR_FCP_ST_RETIDO { get; set; }
        public Nullable<decimal> NF_VLR_IPE_DEVOLVIDO { get; set; }
        public Nullable<int> PPI_ID { get; set; }

        public Nullable<bool> NF_NOTA_ANTECIPADA { get; set; }        
        public Nullable<bool> NF_AVULSA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ContratoDTO CONTRATOS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ItemPedidoDTO ITEM_PEDIDO { get; set; }

        public virtual CFOTableDTO CFOP_TABLE { get; set; }
        public virtual ClienteDto CLIENTES { get; set; }
        public virtual CSTDTO CST { get; set; }
        public virtual EmpresaRefDTO EMPRESA_REF { get; set; }
        public virtual FornecedorDTO FORNECEDOR { get; set; }
        public virtual ICollection<NotaFiscalItemDTO> NOTA_FISCAL_ITEM { get; set; }
        public virtual NotaFiscalSituacaoDTO NOTA_FISCAL_SITUACAO { get; set; }
        public virtual TipoDocFiscalDTO TIPO_DOC_FISCAL { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TransportadorDTO TRANSPORTADOR { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual NotaFiscalStatusDTO NOTA_FISCAL_STATUS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<NotaFiscalEventoDTO> NOTA_FISCAL_EVENTO { get; set; }

        public virtual UFDTO UF { get; set; }

        [IgnoreMemberMapping(MappingDirection.Both)]
        public virtual ICollection<NotaFiscalLoteItemDTO> NOTA_FISCAL_LOTE_ITEM { get; set; }

        [IgnoreMemberMapping(MappingDirection.Both)]
        public virtual PropostaItemDTO PROPOSTA_ITEM { get; set; }

        public int CodNotaFiscal { get => NF_ID; set => NF_ID = value; }
        public int NumeroNota { get => NF_NUMERO; set => NF_NUMERO = value; }
        public string ChaveNota { get => NF_CHAVE; set => NF_CHAVE = value; }
        public string ProtocoloAutorizacao { get => NF_NUMERO_PROTOCOLO; set => NF_NUMERO_PROTOCOLO = value; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual NotaFiscalTipoDTO NOTA_FISCAL_TIPO { get; set; }

        public StatusNotaFiscalEnum Status {

            get
            {
                switch (NST_ID)
                {
                    case 1: return StatusNotaFiscalEnum.EMITIDA;
                    case 2: return StatusNotaFiscalEnum.ENVIADA;
                    case 3: return StatusNotaFiscalEnum.CANCELADA;
                    case 4: return StatusNotaFiscalEnum.DEVOLVIDA;
                    case 5: return StatusNotaFiscalEnum.NAO_ENVIADA;
                    default: return StatusNotaFiscalEnum.NAO_ENVIADA;
                }
            }
            set
            {
                switch (value)
                {
                    case StatusNotaFiscalEnum.EMITIDA : AdicionarStatus(1); break;
                    case StatusNotaFiscalEnum.ENVIADA: AdicionarStatus(2); break;
                    case StatusNotaFiscalEnum.CANCELADA: AdicionarStatus(3); ; break;
                    case StatusNotaFiscalEnum.DEVOLVIDA: AdicionarStatus(4); break;
                    case StatusNotaFiscalEnum.NAO_ENVIADA: AdicionarStatus(5); break;
                }
            }
        }

        public void AdicionarStatus(int? nstID)
        {
            NST_ID = nstID;
            switch (NST_ID)
            {
                case 1: NF_STATUS = "PEN"; break;
                case 2: NF_STATUS = "ENV"; break;
                case 3: NF_STATUS = "CAN"; break;
                case 4: NF_STATUS = "DEV"; break;
                case 5: NF_STATUS = "NEN"; break;
                default: NF_STATUS = null; break;
            }
        }

        public string NomeArquivoEvento { get => NF_NOME_ARQ_EVENTO; set => NF_NOME_ARQ_EVENTO = value; }

        [ScriptIgnore]
        public byte[] ArquivoEvento { get => NF_ARQ_EVENTO; set => NF_ARQ_EVENTO = value; }
        public int? CodEmpresa { get => EMP_ID; set => EMP_ID = value; }
        public string NomeArquivo { get => NF_PATH_ARQUIVO; set => NF_PATH_ARQUIVO = value; }

        [ScriptIgnore]
        public byte[] Arquivo { get => NF_ARQUIVO; set => NF_ARQUIVO = value; }
        public int? CodPedido { get => IPE_ID; set => IPE_ID = value; }
        public string CodContrato { get => CTR_NUM_CONTRATO; set => CTR_NUM_CONTRATO = value; }
        public int? CodCliente { get => CLI_ID; set => CLI_ID = value; }
        public bool? EventoAnexado { get => NF_EVENTO_ANEXADO; set => NF_EVENTO_ANEXADO = value; }
        public int? CodProposta { get => PPI_ID; set => PPI_ID = value; }
        public bool? NotaAntecipada { get => NF_NOTA_ANTECIPADA; set => NF_NOTA_ANTECIPADA = value; }

        public TipoNFEnum Tipo
        {
            get
            {
                switch (NF_TIPO)
                {
                    case 0: return TipoNFEnum.ENTRADA;
                    case 1: return TipoNFEnum.SAIDA;
                    case 2: return TipoNFEnum.ENTRADA_SERVICO;
                    case 3: return TipoNFEnum.SAIDA_SERVICO;
                    default: return TipoNFEnum.SAIDA;
                }
            }
            set
            {
                switch (value)
                {
                    case TipoNFEnum.ENTRADA: NF_TIPO = 0; break;
                    case TipoNFEnum.SAIDA: NF_TIPO = 1; break;
                    case TipoNFEnum.ENTRADA_SERVICO: NF_TIPO = 2; break;
                    case TipoNFEnum.SAIDA_SERVICO: NF_TIPO = 3; break;
                }
            }
        }

        public string CodVerificacao { get => NF_COD_VERIFICACAO; set => NF_COD_VERIFICACAO = value; }
    }

}
