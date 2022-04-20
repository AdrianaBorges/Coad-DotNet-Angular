using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Dao
{
    public static class DaoConfigurator
    {
        public static Dictionary<string, Func<DbContext>> entries = new Dictionary<string,Func<DbContext>>();

        public static void addDbContext(string key, Func<DbContext> expression)
        {
            if (!entries.ContainsKey(key))
            {
                entries.Add(key, expression);
            }
            
        }

        public static DbContext getConfig(string chave = null){

            if(entries.Count() <= 0){

                throw new Exception("Nenhuma configuração do entity foi criada. Use o método addDbContext para definir pelo menos uma referencia do entity");
            }
            if(chave == null){

                var funcFirst = entries.First().Value;
                return funcFirst();
            }

            var func = entries[chave];
            return func();
        }

    }
}
