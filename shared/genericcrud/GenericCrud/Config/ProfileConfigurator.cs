using AutoMapper;
using GenericCrud.Config;
using GenericCrud.Mapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Coad.GenericCrud.Config
{
    public static class ProfileConfigurator
    {
        public static Dictionary<string, Func<DbContext>> entries = new Dictionary<string,Func<DbContext>>();
        public static Dictionary<string, ProfileConfig> profiles = new Dictionary<string, ProfileConfig>();


        public static string[] GetProfileskeys()
        {
            return profiles.Keys.ToArray();
        }

        public static void addConfig(string chave, Func<DbContext> expression, Action<MapperProfileConfig> cfgProfileConfig)
        {
            if (!profiles.Keys.Contains(chave))
            {
                var profile = new ProfileConfig(chave, expression, cfgProfileConfig);
                profiles.Add(chave, profile);
            }
        }

        public static void addConfig(string chave, Func<DbContext> expression, string ScanNamespaces, Assembly assembly)
        {

            if (!profiles.Keys.Contains(chave))
            {
                var profile = new ProfileConfig(chave, expression, ScanNamespaces, assembly);
                profiles.Add(chave, profile);
            }
        }

        public static void addConfig(string chave, Func<DbContext> expression, Action<MapperProfileConfig> cfgProfileConfig, string ScanNameSpaces, Assembly assembly)
        {

            if (!profiles.Keys.Contains(chave))
            {
                var profile = new ProfileConfig(chave, expression, cfgProfileConfig, ScanNameSpaces, assembly);
                profiles.Add(chave, profile);
            }
        }

        public static void addConfig(string chave, Func<DbContext> expression, Action<MapperProfileConfig> cfgProfileConfig, string ScanNameSpaces, Assembly assembly, Type entityType)
        {

            if (!profiles.Keys.Contains(chave))
            {
                var profile = new ProfileConfig(chave, expression, cfgProfileConfig, ScanNameSpaces, assembly, entityType);
                profiles.Add(chave, profile);
            }
        }


        public static ProfileConfig getProfileConfig(string chave = null)
        {

            if (profiles.Count() <= 0)
            {

                throw new Exception("Nenhuma configuração de dao foi criada. Use o método addConfig para definir pelo menos uma configuração");
            }
            if (chave == null)
            {
                chave = "default";

                int count = profiles.Where(op => op.Key == "default").Count();

                if (count > 0)
                {
                    var config = profiles[chave];

                    if (config != null)
                    {
                        return config;
                    }
                    else
                    {
                       throw new Exception("Perfil 'default' não encontrado.");
                    }
                }
                else
                {
                    var config = profiles[profiles.Keys.FirstOrDefault()];
                    return config;
                }
               
            }

            var func = profiles[chave];
            return func;
        }

    }
}
