

using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using GenericCrud.Config.DataAttributes.Maps;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.FISCAL.Model.Integracoes.Interfaces;
using COAD.FISCAL.Model.Integracoes.Enumerados;

namespace COAD.CORPORATIVO.Model.Dto
{    
	[Mapping(typeof(NOTA_FISCAL_LOTE))]
	public class NotaFiscalLoteDTO : INFeLote
	{
		public NotaFiscalLoteDTO(){

			this.NOTA_FISCAL_LOTE_ITEM = new HashSet<NotaFiscalLoteItemDTO>();
		}

		// Normal Properties
		public Int32? NFL_ID { get; set; }
		public Nullable<Int32> EMP_ID { get; set; }
		public Nullable<DateTime> NFL_DATA { get; set; }
		public Nullable<Int32> NLS_ID { get; set; }
        public string NLS_COD_RECIBO { get; set; }
        public Nullable<int> NLS_COD_RETORNO { get; set; }
        public string NLS_MENSAGEM_RETORNO { get; set; }
        public Nullable<int> NFL_COD_RETORNO_PROCESSAMENTO { get; set; }
        public string NFL_MENSAGEM_RETORNO_PROCESSAMENTO { get; set; }
        public Nullable<bool> NFL_ENVIO_IMEDIATO { get; set; }
        public string NFL_MSG_ERRO_SISTEMA { get; set; }
        // Object Properties
        public Nullable<int> NLT_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual NotaFiscalLoteStatusDTO NOTA_FISCAL_LOTE_STATUS { get; set; }
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<NotaFiscalLoteItemDTO> NOTA_FISCAL_LOTE_ITEM { get; set; }


        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual NotaFiscalLoteTipoDTO NOTA_FISCAL_LOTE_TIPO { get; set; }

        public int? LoteID {
            get => NFL_ID;
            set => NFL_ID = value;
        }
        public int? EmpresaID { get => EMP_ID; set => EMP_ID = value; }
        public DateTime? DataCadastro { get => NFL_DATA; set => NFL_DATA = value; }
        public string CodRecibo { get => NLS_COD_RECIBO; set => NLS_COD_RECIBO = value; }
        public int? CodRetorno { get => NLS_COD_RETORNO; set => NLS_COD_RETORNO = value; }
        public string MensagemRetorno { get => NLS_MENSAGEM_RETORNO; set => NLS_MENSAGEM_RETORNO = value; }
        public int? CodRetornoProcessamento { get => NFL_COD_RETORNO_PROCESSAMENTO; set => NFL_COD_RETORNO_PROCESSAMENTO = value; }
        public string MensagemRetornoProcessamento { get => NFL_MENSAGEM_RETORNO_PROCESSAMENTO; set => NFL_MENSAGEM_RETORNO_PROCESSAMENTO = value; }
        public StatusLoteEnum Status {
            get 
            {
                switch (NLS_ID)
                {
                    case 1: return StatusLoteEnum.ENVIO_PENDENTE;
                    case 2: return StatusLoteEnum.ERRO_AO_PROCESSAR;
                    case 3: return StatusLoteEnum.PROCESSADA_COM_EXITO;
                    case 8: return StatusLoteEnum.LOTE_ENVIADO_NAO_PROCESSADO;
                    case 9: return StatusLoteEnum.LOTE_EM_PROCESSAMENTO;
                    default: return StatusLoteEnum.ERRO_AO_PROCESSAR;
                }

            }
            set
            {
                switch (value)
                {
                    case StatusLoteEnum.ENVIO_PENDENTE: NLS_ID = 1; break;
                    case StatusLoteEnum.ERRO_AO_PROCESSAR: NLS_ID = 2; break;
                    case StatusLoteEnum.PROCESSADA_COM_EXITO: NLS_ID = 3; break;
                    case StatusLoteEnum.LOTE_ENVIADO_NAO_PROCESSADO: NLS_ID = 8; break;
                    case StatusLoteEnum.LOTE_EM_PROCESSAMENTO: NLS_ID = 9; break;
                }
            }

        }
        public ICollection<INFeLoteItem> Itens {
            get => 
                (NOTA_FISCAL_LOTE_ITEM != null) ? 
                new List<INFeLoteItem>(NOTA_FISCAL_LOTE_ITEM) : 
                null;
            set => NOTA_FISCAL_LOTE_ITEM =
                (value != null) ?
                NOTA_FISCAL_LOTE_ITEM = value.Cast<NotaFiscalLoteItemDTO>().ToList() : null;  
            }
        public bool? EnvioImediato { get => NFL_ENVIO_IMEDIATO; set => NFL_ENVIO_IMEDIATO = value; }
        public string MsgErroSistema { get => NFL_MSG_ERRO_SISTEMA; set => NFL_MSG_ERRO_SISTEMA = value; }
        public TipoLoteEnum TipoLote
        {
            get
            {
                switch (NLT_ID)
                {
                    case 1: return TipoLoteEnum.ENVIO_LOTE_NFE;
                    case 2: return TipoLoteEnum.ENVIO_EVENTO;
                    case 3: return TipoLoteEnum.ENVIO_LOTE_RPS_NFSE;
                    case 4: return TipoLoteEnum.ENVIO_NFE_AVULSA;
                    case 5: return TipoLoteEnum.ENVIO_NFSE_AVULSA;
                    default: return TipoLoteEnum.ENVIO_LOTE_NFE;
                }

            }
            set
            {
                switch (value)
                {
                    case TipoLoteEnum.ENVIO_LOTE_NFE: NLT_ID = 1; break;
                    case TipoLoteEnum.ENVIO_EVENTO: NLT_ID = 2; break;
                    case TipoLoteEnum.ENVIO_LOTE_RPS_NFSE: NLT_ID = 3; break;
                    case TipoLoteEnum.ENVIO_NFE_AVULSA: NLT_ID = 4; break;
                    case TipoLoteEnum.ENVIO_NFSE_AVULSA: NLT_ID = 5; break;
                }
            }
        }
    }
}
