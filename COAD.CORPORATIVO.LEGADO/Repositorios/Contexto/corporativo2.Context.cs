﻿//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.CORPORATIVO.LEGADO.Repositorios.Contexto
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class corporativo2Entities : DbContext
    {
        public corporativo2Entities()
            : base("name=corporativo2Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AGENDA> AGENDA { get; set; }
        public DbSet<CLIENTES> CLIENTES { get; set; }
        public DbSet<CONTRATOS> CONTRATOS { get; set; }
        public DbSet<EMAILS> EMAILS { get; set; }
        public DbSet<ender_fat> ender_fat { get; set; }
        public DbSet<HIST_ATEND> HIST_ATEND { get; set; }
        public DbSet<PARAM> PARAM { get; set; }
        public DbSet<Parcelas> Parcelas { get; set; }
        public DbSet<representante> representante { get; set; }
        public DbSet<TELEFONES2> TELEFONES2 { get; set; }
        public DbSet<ULTIMO_CODIGO> ULTIMO_CODIGO { get; set; }
        public DbSet<bloqueia_consulta_individual> bloqueia_consulta_individual { get; set; }
        public DbSet<liquidacao> liquidacao { get; set; }
        public DbSet<datas_fat> datas_fat { get; set; }
        public DbSet<empresas> empresas { get; set; }
        public DbSet<SEQ_PROD> SEQ_PROD { get; set; }
        public DbSet<ASSINATURA> ASSINATURA { get; set; }
        public DbSet<cart_coad> cart_coad { get; set; }
    
        public virtual int TRANSF_ASSIN(string vASSIN_ANT, string vASSIN_ATU, string vSOLIC, string vDATA_TRANSF, string vVIGENCIA, string vCONTRATO, Nullable<int> vMES_REFERENCIA, Nullable<System.DateTime> vDATA_INI_VIGENCIA, Nullable<System.DateTime> vDATA_FIM_VIGENCIA, string uSU_LOGIN, string aSN_TRANSF_MOTIVO, ObjectParameter vRETORNO)
        {
            var vASSIN_ANTParameter = vASSIN_ANT != null ?
                new ObjectParameter("vASSIN_ANT", vASSIN_ANT) :
                new ObjectParameter("vASSIN_ANT", typeof(string));
    
            var vASSIN_ATUParameter = vASSIN_ATU != null ?
                new ObjectParameter("vASSIN_ATU", vASSIN_ATU) :
                new ObjectParameter("vASSIN_ATU", typeof(string));
    
            var vSOLICParameter = vSOLIC != null ?
                new ObjectParameter("vSOLIC", vSOLIC) :
                new ObjectParameter("vSOLIC", typeof(string));
    
            var vDATA_TRANSFParameter = vDATA_TRANSF != null ?
                new ObjectParameter("vDATA_TRANSF", vDATA_TRANSF) :
                new ObjectParameter("vDATA_TRANSF", typeof(string));
    
            var vVIGENCIAParameter = vVIGENCIA != null ?
                new ObjectParameter("vVIGENCIA", vVIGENCIA) :
                new ObjectParameter("vVIGENCIA", typeof(string));
    
            var vCONTRATOParameter = vCONTRATO != null ?
                new ObjectParameter("vCONTRATO", vCONTRATO) :
                new ObjectParameter("vCONTRATO", typeof(string));
    
            var vMES_REFERENCIAParameter = vMES_REFERENCIA.HasValue ?
                new ObjectParameter("vMES_REFERENCIA", vMES_REFERENCIA) :
                new ObjectParameter("vMES_REFERENCIA", typeof(int));
    
            var vDATA_INI_VIGENCIAParameter = vDATA_INI_VIGENCIA.HasValue ?
                new ObjectParameter("vDATA_INI_VIGENCIA", vDATA_INI_VIGENCIA) :
                new ObjectParameter("vDATA_INI_VIGENCIA", typeof(System.DateTime));
    
            var vDATA_FIM_VIGENCIAParameter = vDATA_FIM_VIGENCIA.HasValue ?
                new ObjectParameter("vDATA_FIM_VIGENCIA", vDATA_FIM_VIGENCIA) :
                new ObjectParameter("vDATA_FIM_VIGENCIA", typeof(System.DateTime));
    
            var uSU_LOGINParameter = uSU_LOGIN != null ?
                new ObjectParameter("USU_LOGIN", uSU_LOGIN) :
                new ObjectParameter("USU_LOGIN", typeof(string));
    
            var aSN_TRANSF_MOTIVOParameter = aSN_TRANSF_MOTIVO != null ?
                new ObjectParameter("ASN_TRANSF_MOTIVO", aSN_TRANSF_MOTIVO) :
                new ObjectParameter("ASN_TRANSF_MOTIVO", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TRANSF_ASSIN", vASSIN_ANTParameter, vASSIN_ATUParameter, vSOLICParameter, vDATA_TRANSFParameter, vVIGENCIAParameter, vCONTRATOParameter, vMES_REFERENCIAParameter, vDATA_INI_VIGENCIAParameter, vDATA_FIM_VIGENCIAParameter, uSU_LOGINParameter, aSN_TRANSF_MOTIVOParameter, vRETORNO);
        }
    }
}