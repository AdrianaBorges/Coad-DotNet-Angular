

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

namespace COAD.CORPORATIVO.Model.Dto
{    
	[Mapping(typeof(NOTA_FISCAL_LOTE_ITEM_MSG))]
	public class NotaFiscalLoteItemMsgDTO : INotaFiscalItemMSG
	{
		// Normal Properties
		public Int32 NLM_ID { get; set; }
		public Nullable<Int32> NLI_ID { get; set; }
		public String NLM_COD { get; set; }
		public String NLM_MSG { get; set; }
        public string NLM_CORRECAO { get; set; }

        // Object Properties

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual NotaFiscalLoteItemDTO NOTA_FISCAL_LOTE_ITEM { get; set; }
        public int? LoteItemID { get => NLI_ID; set => NLI_ID = value; }
        public string CodMsg { get => NLM_COD; set => NLM_MSG = value; }
        public string Msg { get => NLM_MSG; set => NLM_MSG = value; }
        public string Correcao { get => NLM_CORRECAO; set => NLM_CORRECAO = value; }

        // Collections Properties

    }
}
