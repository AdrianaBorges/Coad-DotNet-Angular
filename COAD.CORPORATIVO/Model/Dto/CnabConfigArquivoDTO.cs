

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
	[Mapping(typeof(CNAB_CONFIG_ARQUIVO))]
	public class CnabConfigArquivoDTO
	{
		public CnabConfigArquivoDTO(){

			this.CNAB = new HashSet<CnabDTO>();
		}

		// Normal Properties
		public int? CCA_ID { get; set; }
		public int? CNC_ID { get; set; }

        [Required(ErrorMessage = "Informe o Tipo de Arquivo")]
		public string CCA_TIPO { get; set; }

        [Required(ErrorMessage = "Informe a descrição do arquivo. (Cabecalho? / Detalhamento? / Footer?)")]
		public string CCA_DESCRICAO { get; set; }
        
        public DateTime? DATA_EXCLUSAO { get; set; }
        // Object Properties

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual CnabConfigDTO CNAB_CONFIG { get; set; }
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<CnabDTO> CNAB { get; set; }

	}
}
