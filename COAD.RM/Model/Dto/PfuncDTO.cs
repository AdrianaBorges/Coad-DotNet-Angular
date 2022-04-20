

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
	[Mapping(typeof(PFUNC))]
	public class PfuncDTO
	{
		public PfuncDTO(){

			this.PFPERFF = new HashSet<PfperffDTO>();			this.PFPERFFCOMPL = new HashSet<PfperffcomplDTO>();
		}

		// Normal Properties
		public Int16 CODCOLIGADA { get; set; }
		public String CHAPA { get; set; }
		public Nullable<Int32> NROFICHAREG { get; set; }
		public String CODRECEBIMENTO { get; set; }
		public String CODSITUACAO { get; set; }
		public String CODTIPO { get; set; }
		public String CODSECAO { get; set; }
		public String CODFUNCAO { get; set; }
		public String CODSINDICATO { get; set; }
		public Nullable<Decimal> JORNADA { get; set; }
		public String CODHORARIO { get; set; }
		public Nullable<Int16> NRODEPIRRF { get; set; }
		public Nullable<Int16> NRODEPSALFAM { get; set; }
		public Nullable<DateTime> DTBASE { get; set; }
		public Nullable<Decimal> SALARIO { get; set; }
		public String SITUACAOFGTS { get; set; }
		public Nullable<DateTime> DTOPCAOFGTS { get; set; }
		public String CONTAFGTS { get; set; }
		public Nullable<Decimal> SALDOFGTS { get; set; }
		public Nullable<DateTime> DTSALDOFGTS { get; set; }
		public String CONTRIBSINDICAL { get; set; }
		public Nullable<Int16> APOSENTADO { get; set; }
		public Nullable<Int16> TEMMAIS65ANOS { get; set; }
		public Nullable<Decimal> AJUDACUSTO { get; set; }
		public Nullable<Decimal> PERCENTADIANT { get; set; }
		public Nullable<Decimal> ARREDONDAMENTO { get; set; }
		public Nullable<DateTime> DATAADMISSAO { get; set; }
		public String TIPOADMISSAO { get; set; }
		public Nullable<DateTime> DTTRANSFERENCIA { get; set; }
		public String MOTIVOADMISSAO { get; set; }
		public Nullable<Int16> TEMPRAZOCONTR { get; set; }
		public Nullable<DateTime> FIMPRAZOCONTR { get; set; }
		public Nullable<DateTime> DATADEMISSAO { get; set; }
		public String TIPODEMISSAO { get; set; }
		public String MOTIVODEMISSAO { get; set; }
		public Nullable<DateTime> DTDESLIGAMENTO { get; set; }
		public Nullable<DateTime> DTULTIMOMOVIM { get; set; }
		public Nullable<DateTime> DTPAGTORESCISAO { get; set; }
		public String CODSAQUEFGTS { get; set; }
		public Nullable<Int16> TEMAVISOPREVIO { get; set; }
		public Nullable<DateTime> DTAVISOPREVIO { get; set; }
		public Nullable<Int16> NRODIASAVISO { get; set; }
		public Nullable<DateTime> DTVENCFERIAS { get; set; }
		public Nullable<DateTime> INICPROGFERIAS1 { get; set; }
		public Nullable<DateTime> FIMPROGFERIAS1 { get; set; }
		public Nullable<Int16> QUERABONO { get; set; }
		public Nullable<Int16> QUER1APARC13O { get; set; }
		public Nullable<Int16> NRODIASADIANTFER { get; set; }
		public String EVTADIANTFERIAS { get; set; }
		public Nullable<Int16> FERIASCOLETIVAS { get; set; }
		public Nullable<Decimal> NRODIASFERIAS { get; set; }
		public Nullable<Decimal> NRODIASABONO { get; set; }
		public Nullable<DateTime> INICPROGFERIAS2 { get; set; }
		public Nullable<DateTime> FIMPROGFERIAS2 { get; set; }
		public Nullable<Decimal> SALDOFERIAS { get; set; }
		public Nullable<Decimal> SALDOFERIASANT { get; set; }
		public Nullable<Decimal> SALDOFERANTAUX { get; set; }
		public String OBSFERIAS { get; set; }
		public Nullable<DateTime> DTPAGTOFERIAS { get; set; }
		public Nullable<DateTime> DTAVISOFERIAS { get; set; }
		public Nullable<Decimal> NDIASLICREM1 { get; set; }
		public Nullable<Decimal> NDIASLICREM2 { get; set; }
		public Nullable<DateTime> DTINICIOLICENCA { get; set; }
		public Nullable<Decimal> MEDIASALMATERN { get; set; }
		public String SITUACAORAIS { get; set; }
		public String CONTAPAGAMENTO { get; set; }
		public Nullable<Int16> MEMBROSINDICAL { get; set; }
		public String VINCULORAIS { get; set; }
		public Nullable<Int16> USAVALETRANSP { get; set; }
		public Nullable<Int16> DIASUTEISMES { get; set; }
		public Nullable<Int16> DIASUTMEIOEXP { get; set; }
		public Nullable<Int16> DIASUTPROXMES { get; set; }
		public Nullable<Int16> DIASUTPROXMEIO { get; set; }
		public Nullable<Int16> DIASUTRESTANTES { get; set; }
		public Nullable<Int16> DIASUTRESTMEIO { get; set; }
		public Nullable<Int16> MUDOUENDERECO { get; set; }
		public Nullable<Int16> MUDOUCARTTRAB { get; set; }
		public String ANTIGACARTTRAB { get; set; }
		public String ANTIGASERIECART { get; set; }
		public Nullable<Int16> MUDOUNOME { get; set; }
		public String ANTIGONOME { get; set; }
		public Nullable<Int16> MUDOUPIS { get; set; }
		public String ANTIGOPIS { get; set; }
		public Nullable<Int16> MUDOUCHAPA { get; set; }
		public String ANTIGACHAPA { get; set; }
		public Nullable<Int16> MUDOUADMISSAO { get; set; }
		public Nullable<DateTime> ANTIGADTADM { get; set; }
		public String ANTIGOVINCULO { get; set; }
		public String ANTIGOTIPOFUNC { get; set; }
		public String ANTIGOTIPOADM { get; set; }
		public Nullable<Int16> MUDOUDTOPCAO { get; set; }
		public Nullable<DateTime> ANTIGADTOPCAO { get; set; }
		public Nullable<Int16> MUDOUSECAO { get; set; }
		public String ANTIGASECAO { get; set; }
		public Nullable<Int16> MUDOUDTNASCIM { get; set; }
		public Nullable<DateTime> ANTIGADTNASCIM { get; set; }
		public Nullable<Int16> FALTAALTERFGTS { get; set; }
		public Nullable<Int16> DEDUZIRRF65 { get; set; }
		public String PISPARAFGTS { get; set; }
		public Nullable<DateTime> ULTIMORECALCULODATA { get; set; }
		public Nullable<DateTime> ULTIMORECALCULOHORA { get; set; }
		public Nullable<Int16> DESCONTAAVISOPREVIO { get; set; }
		public Int16 CODFILIAL { get; set; }
		public String NOME { get; set; }
		public Nullable<Int16> INDINICIOHOR { get; set; }
		public String PISPASEP { get; set; }
		public Nullable<DateTime> DTCADASTROPIS { get; set; }
		public Int32 CODPESSOA { get; set; }
		public String CODBANCOFGTS { get; set; }
		public String CODBANCOPAGTO { get; set; }
		public String CODAGENCIAPAGTO { get; set; }
		public String CODBANCOPIS { get; set; }
		public Nullable<Int16> RESCISAOCALCULADA { get; set; }
		public String OPBANCARIA { get; set; }
		public Nullable<Int16> MEMBROCIPA { get; set; }
		public Nullable<Int16> USASALCOMPOSTO { get; set; }
		public Nullable<Int32> REGATUAL { get; set; }
		public Nullable<Int16> NUMVEZESDESCEMPRESTIMO { get; set; }
		public Nullable<DateTime> DATAINICIODESCEMPRESTIMO { get; set; }
		public String GRUPOSALARIAL { get; set; }
		public Nullable<Int16> JORNADAMENSAL { get; set; }
		public Nullable<DateTime> PREVDISP { get; set; }
		public Int16 CODOCORRENCIA { get; set; }
		public Nullable<Int16> CODCATEGORIA { get; set; }
		public Nullable<Int16> CLASSECONTRIB { get; set; }
		public String CODEQUIPE { get; set; }
		public Nullable<Int16> ESUPERVISOR { get; set; }
		public String INTEGRCONTABIL { get; set; }
		public String INTEGRGERENCIAL { get; set; }
		public Nullable<Int16> USACONTROLEDESALDO { get; set; }
		public String CI { get; set; }
		public Nullable<Int16> MUDOUCI { get; set; }
		public String ANTIGOCI { get; set; }
		public Nullable<Int16> PERIODORESCISAO { get; set; }
		public String CODGRPQUIOSQUE { get; set; }
		public Nullable<Int16> FGTSMESANTRECOLGRFP { get; set; }
		public String CODNIVELSAL { get; set; }
		public Nullable<Int16> TRABALHOUNADEMISSAO { get; set; }
		public Nullable<Int16> NRODIASFERIASJORNRED { get; set; }
		public Nullable<Int16> POSSUIALVARAMENOR16 { get; set; }
		public Nullable<DateTime> DATARESCISAO { get; set; }
		public Nullable<Int16> SITUACAOINSS { get; set; }
		public Nullable<DateTime> DTAPOSENTADORIA { get; set; }
		public String CODTABELASALARIAL { get; set; }
		public Nullable<Int16> TEMDEDUCAOCPMF { get; set; }
		public Nullable<Int16> NRODIASFERIASCORRIDOS { get; set; }
		public Nullable<Int16> NRODIASABONOCORRIDOS { get; set; }
		public Nullable<Int16> POSICAOABONO { get; set; }
		public String REGIMEREVEZAMENTO { get; set; }
		public Nullable<Int16> QUERADIANTAMENTO { get; set; }
		public Nullable<DateTime> DTPROXAQUISFERIAS { get; set; }
		public Nullable<Int16> CODCOLFORNEC { get; set; }
		public String CODFORNECEDOR { get; set; }
		public Nullable<Int16> ISENTOIRRF { get; set; }
		public Nullable<Int16> ANOCOMPTRANSF { get; set; }
		public Nullable<Int16> MESCOMPTRANSF { get; set; }
		public Nullable<Int16> NROPERIODOTRANSF { get; set; }
		public Nullable<Int16> TIPOAPOSENTADORIA { get; set; }
		public String REPOEVAGA { get; set; }
		public Nullable<Decimal> SALDOFGTSREAL { get; set; }
		public Nullable<Int16> RESCISAOPRECISARECALC { get; set; }
		public Int32 ID { get; set; }
		public Nullable<Int32> BENEFPONTOS { get; set; }
		public String CONTRIBASSOC1OCORRCNPJ { get; set; }
		public String CONTRIBASSOC2OCORRCNPJ { get; set; }
		public String CONTRIBASSISTCNPJ { get; set; }
		public String CONTRIBCONFEDCNPJ { get; set; }
		public Nullable<Decimal> CONTRIBASSOC1OCORRVALOR { get; set; }
		public Nullable<Decimal> CONTRIBASSOC2OCORRVALOR { get; set; }
		public Nullable<Decimal> CONTRIBASSISTVALOR { get; set; }
		public Nullable<Decimal> CONTRIBCONFEDVALOR { get; set; }
		public String LOCALTRABCODMUNCIPIO { get; set; }
		public Nullable<Int32> MESESHORAEXTRAS { get; set; }
		public Nullable<Int32> MESESGRATIFICACAO { get; set; }
		public Nullable<Int32> MESESDISSIDIOCOLETIVO { get; set; }
		public Nullable<Int16> INDICADORSINDICALIZADO { get; set; }
		public Nullable<DateTime> DTULTIMOMOVIMPGTOEXTRAS { get; set; }
		public Nullable<DateTime> FIMPRAZOPRORROGCONTR { get; set; }
		public Nullable<Int16> CODCOLIGADAORIGEM { get; set; }
		public String CHAPAORIGEM { get; set; }
		public String RECCREATEDBY { get; set; }
		public Nullable<DateTime> RECCREATEDON { get; set; }
		public String RECMODIFIEDBY { get; set; }
		public Nullable<DateTime> RECMODIFIEDON { get; set; }
		public String NUMEROCARTAOSUS { get; set; }
		public Nullable<Int16> FERIASFINALIZADASPROXMES { get; set; }
		public Nullable<Int16> INDSIMPLES { get; set; }
		public String TPCONTABANCARIA { get; set; }
		public String NRPROCJUD { get; set; }
		public Nullable<Int16> RESIDENCIAPROPRIA { get; set; }
		public Nullable<Int16> RESIDENCIARECURSOSFGTS { get; set; }
		public String TPREGIMEPREV { get; set; }
		public Nullable<Int16> INDADMISSAO { get; set; }
		public Nullable<Int16> TIPOREINTEGRACAO { get; set; }
		public Nullable<DateTime> DATAREINTEGRACAO { get; set; }
		public Nullable<DateTime> DATARETORNOEFETIVO { get; set; }
		public String NROLEIANISTIA { get; set; }
		public String NROPROCESSOJUDICIAL { get; set; }
		public String NATUREZAESTAGIO { get; set; }
		public String CODNIVELESTAGIO { get; set; }
		public String AREAATUACAOESTAGIO { get; set; }
		public String NUMEROAPOLICEESTAGIO { get; set; }
		public Nullable<DateTime> DTPREVTERMINOESTAGIO { get; set; }
		public String CODINSTITUICAOENSINOESTAGIO { get; set; }
		public String CODAGENTEINTEGRACAOESTAGIO { get; set; }
		public String CPFCOORDENADORESTAGIO { get; set; }
		public String NOMECOORDENADORESTAGIO { get; set; }
		public String CNPJEMPRESAORIGEM { get; set; }
		public Nullable<DateTime> DTADMISSAOEMPRESAORIGEM { get; set; }
		public String MATRICULAEMPRESAORIGEM { get; set; }
		public Nullable<Int16> CODCATEGORIAEMPRESAORIGEM { get; set; }
		public Nullable<Int16> RECEBSEGDESEMP { get; set; }
		public Nullable<Int16> TIPOREDUCAOAVISO { get; set; }
		public Nullable<Int16> FORMAREDUCAOAVISO { get; set; }
		public Nullable<Int16> MOTIVOTRABTEMP { get; set; }
		public String CHAPASUBSTRABTEMP { get; set; }
		public Nullable<Int16> MOTIVOCANCELAMENTOAVISO { get; set; }
		public Nullable<DateTime> DTCANCELAMENTOAVISO { get; set; }
		public String NROATESTADOOBITO { get; set; }
		public String NROPROCESSOTRAB { get; set; }
		public String OBSERVACAORESCISAO { get; set; }
		public String OBSERVACAOAVISOPREVIO { get; set; }
		public Nullable<Int16> CARREGOUAVISOPREVIO { get; set; }
		public String DESCRICAOSALVARIAVEL { get; set; }
		public String OBSCANCELAMENTOAVISO { get; set; }
		public Nullable<Int16> SUCESSAOVINCULO { get; set; }
		public String CNPJEMPRESAANTERIOR { get; set; }
		public String MATRICULAANTERIOR { get; set; }
		public Nullable<DateTime> DTINICIOVINCULO { get; set; }
		public String OBSERVACAOSUCESSAO { get; set; }
		public Nullable<Int16> TRANSFERENCIASUCESSAO { get; set; }
		public Nullable<DateTime> APMISTO_DTAVTRAB { get; set; }
		public Nullable<Int16> APMISTO { get; set; }
		public Nullable<Int16> FERIASDIASUTEIS { get; set; }
		public Nullable<Decimal> FERIASSALDODIASUTEIS { get; set; }
		public Nullable<Int16> SITUACAOIRRF { get; set; }
		public Nullable<Int16> IDDADOSRESID { get; set; }
		public Nullable<Int16> SEQUENCIATRANSF { get; set; }
		public String CODBANCOPAGTO2 { get; set; }
		public String CODAGENCIAPAGTO2 { get; set; }
		public String CONTAPAGAMENTO2 { get; set; }
		public String OPBANCARIA2 { get; set; }
		public String TPCONTABANCARIA2 { get; set; }
		public Nullable<Int16> INDPAGTOJUIZO { get; set; }
		public Nullable<Int16> CODIGORECEITA3533 { get; set; }
		public Nullable<DateTime> DTDESLIGAMENTOREINT { get; set; }
		public String MATRICULAESOCIAL { get; set; }
		public Nullable<Int16> MOTIVOTRANSFERENCIA { get; set; }
		public Nullable<Int32> ANOSCONTRIBINSS { get; set; }
		public String CODORGORIDES { get; set; }
		public String CODREGJURI { get; set; }
		public String CODCCUSTO { get; set; }
		public Nullable<Int32> IDITEMCONTABIL { get; set; }
		public Nullable<Int32> IDCLASSEVALOR { get; set; }
		public Nullable<Int16> TIPOREGIMEJORNADA { get; set; }
		public Nullable<Int16> DISPENSADOAVISO { get; set; }
		public String CNPJEMPRESASUCESSORA { get; set; }
		public String JUSTIFICATIVATRABTEMP { get; set; }
		public Nullable<Int16> TPINCLUSAOCONTRATO { get; set; }
		public Nullable<Int16> TEMPOPARCIAL { get; set; }
		public Nullable<Int16> COTAPCD { get; set; }
		public Nullable<Int16> COLTOMADORTEMP { get; set; }
		public String CODIGOTOMADORTEMP { get; set; }
		public Nullable<DateTime> DTDEMISSAOPREVISTA { get; set; }
		public String JUSTIFICATIVAPRORROGTEMP { get; set; }
		public Nullable<DateTime> DTAVISOPREVIOTRAB { get; set; }
		public Nullable<Int16> TEMCLAUASSEG { get; set; }
		public String CPFSUBSTRABTEMP { get; set; }
		public Nullable<Decimal> TPINSCRICAOTEMP { get; set; }
		public String NROINSCRITEMP { get; set; }
		public Nullable<Int16> TIPOAVISOPREVIO { get; set; }
		public Nullable<Int16> CODCATEGORIAESOCIAL { get; set; }
		public Nullable<Int16> CODTIPOCONTRATO { get; set; }
		public Nullable<Int16> ESOCIALFUNCAOCONF { get; set; }
		public String CODFUNCAOCONF { get; set; }
		public String MOTIVOAVISOPREVIOTRAB { get; set; }
		public Nullable<Int16> CODCATEGORIATRABCEDIDO { get; set; }
		public String CNPJCEDENTE { get; set; }
		public String MATRICULACEDENTE { get; set; }
		public Nullable<DateTime> DATAADMISSAOCEDENTE { get; set; }
		public Nullable<Int16> TIPOREGIMETRABALHISTACEDIDO { get; set; }
		public Nullable<Int16> TIPOREGIMEPREVIDENCIARIOCEDIDO { get; set; }
		public Nullable<Int16> INFOONUSCESSAO { get; set; }
		public String TIPOCONTRATOPRAZO { get; set; }
		public Nullable<Int16> ESOCIALNATATIVIDADE { get; set; }
		public Nullable<Int16> MOTIVOSAIDATRANSFERENCIA { get; set; }
		
		// Object Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual PfuncaoDTO PFUNCAO { get; set; }
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual PfuncaoDTO PFUNCAO1 { get; set; }
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<PfperffDTO> PFPERFF { get; set; }

		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<PfperffcomplDTO> PFPERFFCOMPL { get; set; }

	}
}
