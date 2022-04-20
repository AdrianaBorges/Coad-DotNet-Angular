

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

namespace COAD.CORPORATIVO.Model.Dto
{    
	[Mapping(typeof(PERIODO_FATURAMENTO))]
	public class PeriodoFaturamentoDTO
	{
		// Normal Properties
		public Int32 PEF_ANO { get; set; }
		public Int32 PEF_MES { get; set; }
		public String PEF_SEMANA { get; set; }
		public DateTime PEF_DATA_INI_FAT { get; set; }
		public DateTime PEF_DATA_FIM_FAT { get; set; }
		
		// Object Properties
		
		// Collections Properties
		
	}
}
