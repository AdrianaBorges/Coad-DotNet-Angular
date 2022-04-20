

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
	[Mapping(typeof(NOTA_FISCAL_REFERENCIADA))]
	public class NotaFiscalReferenciadaDTO : INotaFiscalReferenciada
	{
		// Normal Properties
		public Int32 NFR_ID { get; set; }
		public String NFR_CHAVE_NOTA { get; set; }
		public Nullable<Int32> NLI_ID { get; set; }
		public Nullable<Int32> NF_ID { get; set; }
		
		// Object Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual NotaFiscalDTO NOTA_FISCAL { get; set; }
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual NotaFiscalLoteItemDTO NOTA_FISCAL_LOTE_ITEM { get; set; }
        public int? LoteItemID { get => NLI_ID; set => NLI_ID = value; }
        public int? CodNotaFiscal { get => NF_ID; set => NF_ID = value; }
        public string ChaveNota { get => NFR_CHAVE_NOTA; set => NFR_CHAVE_NOTA = value; }

        // Collections Properties

    }
}
