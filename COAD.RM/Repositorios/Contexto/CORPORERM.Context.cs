﻿//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.RM.Repositorios.Contexto
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class CorporeRMEntities : DbContext
    {
        public CorporeRMEntities()
            : base("name=CorporeRMEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<PFPERFF> PFPERFF { get; set; }
        public DbSet<PFPERFFCOMPL> PFPERFFCOMPL { get; set; }
        public DbSet<PFUNC> PFUNC { get; set; }
        public DbSet<PFUNCAO> PFUNCAO { get; set; }
        public DbSet<PPESSOA> PPESSOA { get; set; }
    
        public virtual ObjectResult<COAD_EMISSAO_CONTRACHEQUES_Result> COAD_EMISSAO_CONTRACHEQUES(string cpf, string empresa, string ano, string mes, string periodo)
        {
            var cpfParameter = cpf != null ?
                new ObjectParameter("cpf", cpf) :
                new ObjectParameter("cpf", typeof(string));
    
            var empresaParameter = empresa != null ?
                new ObjectParameter("empresa", empresa) :
                new ObjectParameter("empresa", typeof(string));
    
            var anoParameter = ano != null ?
                new ObjectParameter("ano", ano) :
                new ObjectParameter("ano", typeof(string));
    
            var mesParameter = mes != null ?
                new ObjectParameter("mes", mes) :
                new ObjectParameter("mes", typeof(string));
    
            var periodoParameter = periodo != null ?
                new ObjectParameter("periodo", periodo) :
                new ObjectParameter("periodo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<COAD_EMISSAO_CONTRACHEQUES_Result>("COAD_EMISSAO_CONTRACHEQUES", cpfParameter, empresaParameter, anoParameter, mesParameter, periodoParameter);
        }
    }
}