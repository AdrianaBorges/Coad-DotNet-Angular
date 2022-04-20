

using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using GenericCrud.Config.DataAttributes.Maps;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.FISCAL.Model.Integracoes.Interfaces;
using COAD.FISCAL.Model.Integracoes.Enumerados;
using System.Web.Script.Serialization;

namespace COAD.CORPORATIVO.Model.Dto
{    
	[Mapping(typeof(NOTA_FISCAL_EVENTO))]
	public class NotaFiscalEventoDTO : INotaFiscalEventoDTO
    {
		// Normal Properties
		public Int32? NEV_ID { get; set; }
		public Nullable<Int32> NIT_ID { get; set; }
		public Nullable<Int32> NF_ID { get; set; }
		public String NEV_NUMERO_PROTOCOLO { get; set; }
		public String NEV_JUSTIFICATIVA { get; set; }
		public String NEV_ID_EVENTO { get; set; }
		public Nullable<DateTime> NEV_DATA { get; set; }
		public String NEV_CNPJ { get; set; }
		public String NEV_DESC_CARTA_CORRECAO { get; set; }
		public String NEV_COND_USO { get; set; }
		public Nullable<Int32> NEV_COD_RETORNO { get; set; }
		public String NEV_COD_DESCRICAO_RETORNO { get; set; }
        public string NEV_ARQUIVO_NOME { get; set; }
        public byte[] NEV_ARQUIVO { get; set; }

        // Object Properties

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual NotaFiscalDTO NOTA_FISCAL { get; set; }
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual NotaFiscalLoteItemTipoDTO NOTA_FISCAL_LOTE_ITEM_TIPO { get; set; }

        public int? EventoID { get => NEV_ID; set => NEV_ID = value; }
        public int? NotaFiscalID { get => NF_ID; set => NF_ID = value; }
        public TipoLoteItemEnum Tipo
        {
            get
            {
                switch (NIT_ID)
                {
                    case 1: return TipoLoteItemEnum.ENVIO;
                    case 2: return TipoLoteItemEnum.CANCELAMENTO;
                    case 3: return TipoLoteItemEnum.DEVOLUCAO;
                    case 4: return TipoLoteItemEnum.CARTA_CORRECAO;
                    default: return TipoLoteItemEnum.ENVIO;
                }
            }
            set
            {
                switch (value)
                {
                    case TipoLoteItemEnum.ENVIO: NIT_ID = 1; break;
                    case TipoLoteItemEnum.CANCELAMENTO: NIT_ID = 2; break;
                    case TipoLoteItemEnum.DEVOLUCAO: NIT_ID = 3; break;
                    case TipoLoteItemEnum.CARTA_CORRECAO: NIT_ID = 4; break;
                }
            }
        }

        public string CNPJ { get => NEV_CNPJ; set => NEV_CNPJ = value; }
        public string CondicaoUso { get => NEV_COND_USO; set => NEV_COND_USO = value; }
        public DateTime? Data { get => NEV_DATA; set => NEV_DATA = value; }
        public string DescCartaCorrecao { get => NEV_DESC_CARTA_CORRECAO; set => NEV_DESC_CARTA_CORRECAO = value; }
        public string EventoXMLID { get => NEV_ID_EVENTO; set => NEV_ID_EVENTO = value; }
        public string DescJustificativa { get => NEV_JUSTIFICATIVA; set => NEV_JUSTIFICATIVA = value; }
        public string NumeroProtocolo { get => NEV_NUMERO_PROTOCOLO; set => NEV_NUMERO_PROTOCOLO = value; }
        public int? CodRetorno { get => NEV_COD_RETORNO; set => NEV_COD_RETORNO = value; }
        public string DescRetorno { get => NEV_COD_DESCRICAO_RETORNO; set => NEV_COD_DESCRICAO_RETORNO = value; }
        public string ArquivoNome { get => NEV_ARQUIVO_NOME; set => NEV_ARQUIVO_NOME = value; }

        [ScriptIgnore]
        public byte[] Arquivo { get => NEV_ARQUIVO; set => NEV_ARQUIVO = value; }

        // Collections Properties

    }
}
