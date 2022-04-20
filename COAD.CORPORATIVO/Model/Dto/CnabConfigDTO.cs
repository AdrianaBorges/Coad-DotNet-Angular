

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
using COAD.SEGURANCA.Model;

namespace COAD.CORPORATIVO.Model.Dto
{    
	[Mapping(typeof(CNAB_CONFIG))]
	public class CnabConfigDTO
	{
		public CnabConfigDTO(){

			this.CNAB_CONFIG_ARQUIVO = new HashSet<CnabConfigArquivoDTO>();
		}

		// Normal Properties
		public Int32 CNC_ID { get; set; }

        [Required(ErrorMessage = "Informe o Código CNAB (Padrão Atual 400)")]
        public String CNC_CODIGO_CNAB { get; set; } = "400";

        [Required(ErrorMessage = "Informe o Tipo de Remessa (Padrão Atual 1Remessa)")]
        public String CNC_ARQUIVO { get; set; } = "1REMESSA";
		public Nullable<Int32> CNC_TIPO_REGISTRO { get; set; }

        [Required(ErrorMessage = "Informe a Empresa")]
        public Nullable<Int32> EMP_ID { get; set; }

        [Required(ErrorMessage = "Informe o Banco")]
        public String BAN_ID { get; set; }
		public Nullable<DateTime> DATA_CADASTRO { get; set; }
		public Nullable<DateTime> DATA_ALTERACAO { get; set; }
		public Nullable<DateTime> DATA_EXCLUSAO { get; set; }
		
		// Object Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual BancosDTO BANCOS { get; set; }
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<CnabConfigArquivoDTO> CNAB_CONFIG_ARQUIVO { get; set; }

        public EmpresaModel EMPRESAS { get; set; }

	}
}
