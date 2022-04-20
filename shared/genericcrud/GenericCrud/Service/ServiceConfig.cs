using Coad.GenericCrud.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Service
{   
    public class ServiceConfig
    {
        public DbContext DbContext { get; set; }

        public static ServiceConfig GetConfig(ConfigName configName)
        {
            var dbContext = DbContextFactory.criarDbContext(configName.Name);

            return new ServiceConfig()
            {
                DbContext = dbContext
            };
        }

        public ServiceConfig LigarOtimizacaoOparacoesDeUpdateInsert()
        {
            SetAutoDetectChangesEnabled(false);
            SetValidateOnSaveEnabled(false);
            return this;
        }

        public ServiceConfig DesligarOtimizacaoOparacoesDeUpdateInsert()
        {
            SetAutoDetectChangesEnabled(true);
            SetValidateOnSaveEnabled(true);
            return this;
        }

        public ServiceConfig SetValidateOnSaveEnabled(bool Validate)
        {
            DbContext.Configuration.ValidateOnSaveEnabled = Validate;
            return this;
        }

        public ServiceConfig SetAutoDetectChangesEnabled(bool AutoDetect)
        {
            DbContext.Configuration.AutoDetectChangesEnabled = AutoDetect;
            return this;
        }
    }
}
