
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Repository.Base
{
    public class CORPORATIVOContext : COADCORPEntities
    {
        public CORPORATIVOContext()
        {

        }

        public CORPORATIVOContext(string connectionString) : base()
        {

        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NOTA_FISCAL_ITEM>().Property(model => model.NFI_QTDE).HasPrecision(10, 4);
            
            base.OnModelCreating(modelBuilder);
        }   

    }
}

