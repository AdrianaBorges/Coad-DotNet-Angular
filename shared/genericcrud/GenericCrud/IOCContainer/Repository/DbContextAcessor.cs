using Castle.MicroKernel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.IOCContainer.Proxies
{
    public class DbContextAcessor : DbContextAcessor<DbContext>
    {
        public DbContextAcessor(IKernel kernel) : base(kernel)
        {
        }
    }

    public class DbContextAcessor<T> : IDbContextAcessor<T> , IDisposable where T : DbContext
    {
        private readonly IKernel kernel;
        public bool IsDisposed = false;

        public DbContextAcessor(IKernel kernel)
        {
            this.kernel = kernel;
        }

        private DbContext _db;
        public string ProfileName { get; set; }
        public Type EntityType { get; set; }

        
        public System.Data.Entity.DbContext dbContext
        {
            get
            {
                if (IsDisposed == false)
                {
                    if (!string.IsNullOrWhiteSpace(ProfileName))
                    {
                        if (kernel.HasComponent(ProfileName))
                        {
                            return (DbContext)kernel.Resolve<T>(ProfileName);
                        }
                        else if(EntityType != null && kernel.HasComponent(EntityType))
                        {
                            return (DbContext)kernel.Resolve(EntityType);
                        }
                    }
                    else if (EntityType != null && kernel.HasComponent(EntityType))
                    {
                        return (DbContext)kernel.Resolve(EntityType);
                    }
                    if (string.IsNullOrWhiteSpace(ProfileName))
                    {

                        return (DbContext)kernel.Resolve<T>();
                    }
                    
                }
                return null;
            }
            set
            {
                
            }
        }


        /// <summary>
        /// Devolve a instância do DBContext 
        /// </summary>
        public T Db { 
            get {

                return (T)dbContext;
            } 
             private set { } 
        }

        /// <summary>
        /// Devolve a instância do DBContext Fazendo o Cast Especificado
        /// </summary>
        public TCast DbAs<TCast>() where TCast : DbContext
        {
            if(dbContext != null)
                return (TCast)dbContext;
             return null;
        }

        /// <summary>
        /// Faz um cast de TODO o objeto Acessor
        /// </summary>
        /// <typeparam name="TDestiny"></typeparam>
        /// <returns></returns>
        public DbContextAcessor<TDestiny> Cast<TDestiny>() where TDestiny : DbContext
        {
            var dbAcessor =  kernel.Resolve<DbContextAcessor<TDestiny>>();
            dbAcessor.ProfileName = this.ProfileName;

            return dbAcessor;
        }

        public void Dispose()
        {
            this.IsDisposed = true;
        }
    }
}
