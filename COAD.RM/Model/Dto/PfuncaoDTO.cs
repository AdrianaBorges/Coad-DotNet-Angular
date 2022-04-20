

using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using GenericCrud.Config.DataAttributes.Maps;
using COAD.RM.Repositorios.Contexto;

namespace COAD.RM.Model.Dto
{    
	[Mapping(typeof(PFUNCAO))]
	public class PfuncaoDTO
	{
		public PfuncaoDTO(){

			this.PFUNC = new HashSet<PfuncDTO>();
            this.PFUNC1 = new HashSet<PfuncDTO>();
		}

		// Normal Properties
		public Int16 CODCOLIGADA { get; set; }
		public String CODIGO { get; set; }
		public String NOME { get; set; }
		public Nullable<Decimal> NUMPONTOS { get; set; }
		public String CBO { get; set; }
		public String CARGO { get; set; }
		public Nullable<Int16> INATIVA { get; set; }
		public Nullable<Int16> ATIVTRANSP { get; set; }
		public String DESCRICAO { get; set; }
		public String FAIXASALARIAL { get; set; }
		public Nullable<Int32> LIMITEFUNC { get; set; }
		public Nullable<Decimal> VERBAQUADROVAGAS { get; set; }
		public Nullable<Decimal> PERCQUADROVAGAS { get; set; }
		public Nullable<DateTime> DATAULTIMAREVISAO { get; set; }
		public String NUMREVISAO { get; set; }
		public String CBO2002 { get; set; }
		public String CODTABELA { get; set; }
		public String CODPERFILCAND { get; set; }
		public Int32 ID { get; set; }
		public Nullable<Int32> BENEFPONTOS { get; set; }
		public String OBJETIVO { get; set; }
		public String DESCRICAOPPP { get; set; }
		public String EXIBEORGANOGRAMA { get; set; }
		public String CODFUNCAOCHEFIA { get; set; }
		public Nullable<Decimal> JORNADAREF { get; set; }
		public String RECCREATEDBY { get; set; }
		public Nullable<DateTime> RECCREATEDON { get; set; }
		public String RECMODIFIEDBY { get; set; }
		public Nullable<DateTime> RECMODIFIEDON { get; set; }
		public String CODTIPOFUNCAO { get; set; }
		public String SIGLA { get; set; }
		public Nullable<Int16> ESOCIALFUNCAOCONF { get; set; }
		
		// Object Properties
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<PfuncDTO> PFUNC { get; set; }

		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<PfuncDTO> PFUNC1 { get; set; }

	}
}
