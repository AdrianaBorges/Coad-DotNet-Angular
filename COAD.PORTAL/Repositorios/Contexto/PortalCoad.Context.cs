//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.PORTAL.Repositorios.Contexto
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class coadEntities : DbContext
    {
        public coadEntities()
            : base("name=coadEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<clientes> clientes { get; set; }
        public DbSet<idc_agregado> idc_agregado { get; set; }
        public DbSet<noticias> noticias { get; set; }
        public DbSet<noticias_busca> noticias_busca { get; set; }
        public DbSet<noticias_conteudo> noticias_conteudo { get; set; }
        public DbSet<noticias_grupos> noticias_grupos { get; set; }
        public DbSet<noticias_permissoes> noticias_permissoes { get; set; }
        public DbSet<noticias_tipos> noticias_tipos { get; set; }
        public DbSet<tab_30> tab_30 { get; set; }
        public DbSet<tab_30_html> tab_30_html { get; set; }
        public DbSet<tab_31> tab_31 { get; set; }
        public DbSet<tab_31_html> tab_31_html { get; set; }
        public DbSet<VW_BuscarNoticias> VW_BuscarNoticias { get; set; }
    }
}
