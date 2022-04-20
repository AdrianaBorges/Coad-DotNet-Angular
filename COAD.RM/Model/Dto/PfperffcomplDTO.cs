

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
	[Mapping(typeof(PFPERFFCOMPL))]
	public class PfperffcomplDTO
	{
		// Normal Properties
		public Int16 CODCOLIGADA { get; set; }
		public String CHAPA { get; set; }
		public Int16 ANOCOMP { get; set; }
		public Int16 MESCOMP { get; set; }
		public Int16 NROPERIODO { get; set; }
		public Nullable<Int16> MESCAIXACOMUM { get; set; }
		public Nullable<Int16> VALORESFORCADOS { get; set; }
		public Nullable<Int16> MOVIMPORTADO { get; set; }
		public Nullable<Int16> EDITADO { get; set; }
		public Nullable<Int16> ALTERADO { get; set; }
		public Nullable<Decimal> SALCOMISSAO { get; set; }
		public Nullable<Decimal> FERIASMES { get; set; }
		public Nullable<Decimal> BASEINSS { get; set; }
		public Nullable<Decimal> BASEINSSOUTROEMP { get; set; }
		public Nullable<Decimal> INSS { get; set; }
		public Nullable<Decimal> INSSFERIAS { get; set; }
		public Nullable<Decimal> INSSOUTROEMP { get; set; }
		public Nullable<Decimal> BASEINSS13 { get; set; }
		public Nullable<Decimal> BASEINSS13OUTRO { get; set; }
		public Nullable<Decimal> INSS13 { get; set; }
		public Nullable<Decimal> BASESALFAMILIA { get; set; }
		public Nullable<Decimal> SALFAMILIA { get; set; }
		public Nullable<Decimal> BASEVALETRANSP { get; set; }
		public Nullable<Decimal> VALETRANSPENTR { get; set; }
		public Nullable<Decimal> VALETRANSPDESC { get; set; }
		public Nullable<Decimal> BASEIRRF { get; set; }
		public Nullable<Decimal> IRRF { get; set; }
		public Nullable<Decimal> INSSCAIXA { get; set; }
		public Nullable<Decimal> DEDUTIVELIRRF { get; set; }
		public Nullable<Decimal> BASEIRRFPART { get; set; }
		public Nullable<Decimal> IRRFPART { get; set; }
		public Nullable<Decimal> BASEIRRFFERIAS { get; set; }
		public Nullable<Decimal> IRRFFERIAS { get; set; }
		public Nullable<Decimal> INSSCOMCPMF { get; set; }
		public String DESCRICAO { get; set; }
		public Nullable<Decimal> BASEFGTS { get; set; }
		public Nullable<Decimal> BASEFGTS13 { get; set; }
		public Nullable<Decimal> SALARIOPAGO { get; set; }
		public Nullable<Int16> STATUSCCUSTO { get; set; }
		public Nullable<Decimal> BASEIRRF13 { get; set; }
		public Nullable<Decimal> SALARIODECALCULO { get; set; }
		public Nullable<Decimal> IRRF13 { get; set; }
		public Nullable<Decimal> INSSFERIASCOMCPMF { get; set; }
		public Nullable<Decimal> INSSCALCUSUARIO { get; set; }
		public Nullable<Decimal> BASEFGTSDIFSAL { get; set; }
		public Nullable<Decimal> INSSDIFSAL { get; set; }
		public Nullable<Decimal> INSSDIFSAL13 { get; set; }
		public Nullable<Decimal> INSSDIFSALFER { get; set; }
		public String EXECID { get; set; }
		public String RECCREATEDBY { get; set; }
		public Nullable<DateTime> RECCREATEDON { get; set; }
		public String RECMODIFIEDBY { get; set; }
		public Nullable<DateTime> RECMODIFIEDON { get; set; }
		public Nullable<Int16> NRODEPENDIRRF { get; set; }
		public Nullable<Decimal> LIQUIDO { get; set; }
		public Nullable<Int16> NRODEPENDSALFAMILIA { get; set; }
		public Nullable<Int16> IDDADOSRESID { get; set; }
		
		// Object Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual PfuncDTO PFUNC { get; set; }
		
		// Collections Properties
		
	}
}
