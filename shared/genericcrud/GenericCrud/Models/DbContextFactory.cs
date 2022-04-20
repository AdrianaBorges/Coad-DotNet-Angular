using Coad.GenericCrud.Config;
using GenericCrud.Dao;
using GenericCrud.Dao.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Coad.GenericCrud.Models
{
    public static class DbContextFactory
    {
        public static Dictionary<string, DbContext> cacheDbContext = new Dictionary<string, DbContext>();
        public static Dictionary<string, RepositoryObservable> observables = new Dictionary<string, RepositoryObservable>();
        public static Dictionary<string, RepositoryDbContextObserver> observers = new Dictionary<string, RepositoryDbContextObserver>();
        public static bool utilizarDbContextPorRequisicao = true;
        
        public static Dictionary<string, DbContext> CriarTodosOsDbContexts()
        {
            Dictionary<string, DbContext> lstDbContextCache = new Dictionary<string, DbContext>();

            var lstChaves = ProfileConfigurator.GetProfileskeys();
            foreach (var key in lstChaves)
            {
                var dbContext = DbContextFactory.criarDbContext(key);
            }

            return lstDbContextCache;
        }

        public static Dictionary<string, DbContext> ObterObjectDeCache()
        {
            Dictionary<string, DbContext> lstCache = null;
            // Ambiente web
            if (HttpContext.Current != null)
            {
                lstCache = HttpContext.Current.Items["lstDbContext"] as Dictionary<string, DbContext>;

                if (lstCache == null)
                {

                    lstCache = new Dictionary<string, DbContext>();
                    HttpContext.Current.Items.Add("lstDbContext", lstCache);
                };
            }
            else
            {
                lstCache = cacheDbContext;
            }

            return lstCache;
        }

        public static void LimparObjectDeCache()
        {
            // Ambiente web
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items.Remove("lstDbContext");
                HttpContext.Current.Items.Add("lstDbContext", new Dictionary<string, DbContext>());
                
            }
            else
            {
                cacheDbContext.Clear();
            }

        }

        public static Dictionary<string, RepositoryObservable> ObterCacheObservable()
        {
            Dictionary<string, RepositoryObservable> lstCache = null;
            // Ambiente web
            if (HttpContext.Current != null)
            {
                lstCache = HttpContext.Current.Items["lstObservables"] as Dictionary<string, RepositoryObservable>;
                
                if (lstCache == null) {

                    lstCache = new Dictionary<string, RepositoryObservable>();

                    HttpContext.Current.Items.Add("lstObservables", lstCache);
                };
            }
            else
            {
                lstCache = observables;
            }

            return lstCache;
        }


        public static Dictionary<string, RepositoryDbContextObserver> ObterCacheObserver()
        {
            Dictionary<string, RepositoryDbContextObserver> lstCache = null;
            // Ambiente web
            if (HttpContext.Current != null)
            {
                lstCache = HttpContext.Current.Items["lstObservers"] as Dictionary<string, RepositoryDbContextObserver>;

                if (lstCache == null)
                {
                    lstCache = new Dictionary<string, RepositoryDbContextObserver>();
                    HttpContext.Current.Items.Add("lstObservers", lstCache);
                };

            }
            else
            {
                lstCache = observers;
            }

            return lstCache;
        }

        public static RepositoryObservable ChecarOuCriarObservaveis(string chave)
        {
            if (string.IsNullOrEmpty(chave))
            {
                chave = "default";
            }
            var cacheObservables = ObterCacheObservable();

            if (cacheObservables.Keys.Contains(chave))
            {
                return cacheObservables[chave];
            }
            else
            {
                var observer = ChecarOuCriarObservadores(chave);
                var observable = new RepositoryObservable(observer);

                cacheObservables.Add(chave, observable);
                return observable;
            }
        }

        public static RepositoryDbContextObserver ChecarOuCriarObservadores(string chave)
        {
            if (string.IsNullOrEmpty(chave))
            {
                chave = "default";
            }

            var cacheObservers = ObterCacheObserver();

            if (cacheObservers.Keys.Contains(chave))
            {
                return cacheObservers[chave];
            }
            else
            {
                var observer = new RepositoryDbContextObserver();
                cacheObservers.Add(chave, observer);
                
                return observer;
            }
        }

        public static DbContext criarDbContext(string chave = null, bool getNew = false, IRepository repository = null, bool useDbContextCache = true)
        {
            DbContext dbContext = null;
            if (useDbContextCache && DbContextFactory.utilizarDbContextPorRequisicao)
            {
                if (repository != null)
                {
                    var repoObservable = ChecarOuCriarObservaveis(chave);
                    repoObservable.AddRepository(repository);
                }

                if (getNew)
                {
                    FecharContexto(chave);
                }
                else
                {
                    dbContext = tentarObterDoCache(chave);

                    if (dbContext != null)
                    {
                        NotificarObservadores(chave, dbContext);
                        return dbContext;
                    }
                }

                var config = ProfileConfigurator.getProfileConfig(chave);
                var method = config.dbContextMethod;

                if (method != null)
                {
                    var newDbContext = method();

                    if (newDbContext != null)
                    {
                        NotificarObservadores(chave, newDbContext);
                    }

                    ArmazenarNoCache(chave, newDbContext);

                    return newDbContext;
                }

            }
            else
            {                
                var config = ProfileConfigurator.getProfileConfig(chave);
                var method = config.dbContextMethod;

                if (method != null)
                {
                    var newDbContext = method();
                    return newDbContext;
                }
            }
            return null;
        }

        public static void NotificarObservadores(string chave, DbContext dbContext)
        {
            var observer = ChecarOuCriarObservadores(chave);
            var observavel = ChecarOuCriarObservaveis(chave);

            observer.DbContext = dbContext;
            observavel.NotificarRenovacaoDoContexto();
        }

        private static void ArmazenarNoCache(string chave, DbContext dbContext)
        {
            if(string.IsNullOrEmpty(chave)){ chave = "default";}

            Dictionary<string, DbContext> lstCache = null;

            // Ambiente web
            if (HttpContext.Current != null)
            {
                lstCache = HttpContext.Current.Items["lstDbContext"] as Dictionary<string, DbContext>;
                
                if (lstCache == null)
                    lstCache = new Dictionary<string, DbContext>();

                if (!lstCache.Keys.Contains(chave))
                {
                    lstCache.Add(chave, dbContext);
                }
                
            }
            else
            {
                lstCache = cacheDbContext;
                if (!lstCache.Keys.Contains(chave))
                {
                    lstCache.Add(chave, dbContext);
                }
            }
        }

        private static DbContext tentarObterDoCache(string key)
        {
            var lstCache = ObterObjectDeCache(); 
            
            if (lstCache != null && lstCache.Where(x => x.Key == key).Count() > 0)
            {
                 try
                {
                    var dbContext = lstCache[key];
                    dbContext.GetValidationErrors();
                    return dbContext;
                }
                catch (Exception e)
                {
                    throw new Exception("Ocorreu um erro ao tentar obter o db contexto do cache", e);
                }                           
            }

            return null;
        }

        public static void FecharContexto(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                key = "default";
            }

            var lstCache = ObterObjectDeCache();
            var dbContext = lstCache[key];

            if (dbContext != null)
            {
                try
                {
                  //  dbContext.SaveChanges();
                    dbContext.Dispose();
                } 
                catch (ObjectDisposedException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (InvalidOperationException ex1)
                {
                    throw new Exception(ex1.Message);
                }
                finally
                {
                    lstCache.Remove(key);
                }
            }
        }

        public static void FecharTodosOsDbContexts()
        {
            var lstCache = ObterObjectDeCache();

            var auxCollection = new Dictionary<string, DbContext>(lstCache);
            foreach (var key in auxCollection.Keys)
            {
                FecharContexto(key);
            }


            foreach (var key in observables.Keys)
            {
                observables[key].Dispose();
            }

            observables.Clear();
            observers.Clear();


            auxCollection.Clear();
            DbContextFactory.LimparObjectDeCache();
        }
    }
}