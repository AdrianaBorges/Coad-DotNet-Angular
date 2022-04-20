using Castle.MicroKernel;
using GenericCrud.Config.IOCContainer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.IOCContainer.Proxies
{
    public class DbContextAcessorNetCore : DbContextAcessorNetCore<DbContext>
    {
        public DbContextAcessorNetCore(IKernel kernel) : base(kernel)
        {
        }
    }

    public class DbContextAcessorNetCore<T> :  IDisposable where T : DbContext
    {
        private readonly IKernel kernel;
        public bool IsDisposed = false;
        public Dictionary<Type, DbContext> DbContexts { get; set; }
        
        public DbContextAcessorNetCore(IKernel kernel)
        {
            this.kernel = kernel;
            DbContexts = new Dictionary<Type, DbContext>();
        }
        

        private TDbContext _GetDbContext<TDbContext>(Type dbContextType) where TDbContext: DbContext
        {
            if (DbContexts.Keys.Contains(dbContextType))
            {
                return (TDbContext) DbContexts[dbContextType];
            }

            return null;
        }

        private void _SetDbContext(Type dbContextType, DbContext dbContext)
        {
            if (!DbContexts.Keys.Contains(dbContextType))
            {
                DbContexts.Add(dbContextType, dbContext);
            }
        }

        private bool HasDbContext(Type dbContextType)
        {
            return (DbContexts.Keys.Contains(dbContextType));
        }

        public TDbContext GetDbContext<TDbContext>(Type dbContextType) where TDbContext : DbContext
        {
            if(dbContextType != null)
            {
                if (HasDbContext(dbContextType))
                {
                    return _GetDbContext<TDbContext>(dbContextType);
                }
                else
                {
                    if (kernel.HasComponent(dbContextType))
                    {
                        var dbContext = (TDbContext) kernel.Resolve(dbContextType);
                        _SetDbContext(dbContextType, dbContext);
                        return dbContext;
                    }
                }
            }

            return null;
        }



        public T Db => null;

        public void Dispose()
        {
            this.IsDisposed = true;
        }
    }
}
