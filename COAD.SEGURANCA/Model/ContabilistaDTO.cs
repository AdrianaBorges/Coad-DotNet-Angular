

using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using GenericCrud.Config.DataAttributes.Maps;
using COAD.SEGURANCA.Repositorios.Contexto;

namespace COAD.SEGURANCA.Model.Dto
{    
	[Mapping(typeof(CONTABILISTA))]
	public class ContabilistaDTO
	{
		public ContabilistaDTO(){

			this.EMPRESA = new HashSet<EmpresaModel>();
		}

		// Normal Properties
		public Int32 CNT_ID { get; set; }
		public String CNT_NOME { get; set; }
		public String CNT_CPF_CNPJ { get; set; }
		public String CNT_CRC_UF { get; set; }
		public String CTR_CRC_NUMERO { get; set; }
		public String CTR_CRC_DIGITO { get; set; }
		public String CTR_CEP { get; set; }
		public String CTR_LOGRADOURO { get; set; }
		public String CTR_NUMERO { get; set; }
		public String CTR_COMPLEMENTO { get; set; }
		public String CTR_BAIRRO { get; set; }
		public String CTR_TEL { get; set; }
		public String CTR_FAX { get; set; }
		public String CTR_CEL { get; set; }
		public String CTR_EMAIL { get; set; }
		public String IBGE_COD_COMPLETO { get; set; }
		
		// Object Properties
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<EmpresaModel> EMPRESA { get; set; }

	}
}
