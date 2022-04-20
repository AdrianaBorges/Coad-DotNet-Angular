using AutoMapper;
using COAD.CORPORATIVO.Config;
using GenericCrud.Config;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Coad.GenericCrud.Config
{
    public class ProfileConfig
    {
        public string profileName { get; set; }
        public Func<DbContext> dbContextMethod { get; set; }
        public Profile profile { get; set; }
        public Action<MapperProfileConfig> cfgProfileConfig { get; set; }
        public MapperEngineWrapper engine { get; set; }
        public string ScanNameSpaces { get; set; }
        public Assembly assembly { get; set; }
        public Type entityType { get; set; }

        public ProfileConfig(string profileName, Func<DbContext> funcDbContext, Action<MapperProfileConfig> cfgProfileConfig)
        {
            this.profileName = profileName;
            this.dbContextMethod = funcDbContext;
            this.cfgProfileConfig = cfgProfileConfig;            
        }

        public ProfileConfig(string profileName, Func<DbContext> funcDbContext, string ScanNameSpaces, Assembly assembly)
        {
            this.profileName = profileName;
            this.dbContextMethod = funcDbContext;
            this.ScanNameSpaces = ScanNameSpaces;
            this.assembly = assembly;
        }

        public ProfileConfig(string profileName, Func<DbContext> funcDbContext, Action<MapperProfileConfig> cfgProfileConfig, string ScanNameSpaces, Assembly assembly)
        {
            this.profileName = profileName;
            this.dbContextMethod = funcDbContext;
            this.cfgProfileConfig = cfgProfileConfig;
            this.ScanNameSpaces = ScanNameSpaces;
            this.assembly = assembly;
        }

        public ProfileConfig(string profileName, Func<DbContext> funcDbContext, Action<MapperProfileConfig> cfgProfileConfig, string ScanNameSpaces, Assembly assembly, Type entityType)
        {
            this.profileName = profileName;
            this.dbContextMethod = funcDbContext;
            this.cfgProfileConfig = cfgProfileConfig;
            this.ScanNameSpaces = ScanNameSpaces;
            this.assembly = assembly;
            this.entityType = entityType;
        }

    }
}
