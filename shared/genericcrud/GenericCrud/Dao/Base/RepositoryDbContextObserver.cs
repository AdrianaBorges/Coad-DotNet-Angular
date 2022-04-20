using Coad.Reflection;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Dao.Base
{
    public class RepositoryDbContextObserver : IObserver<IRepository>
    {
        public DbContext DbContext { get; set; }
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IRepository value)
        {
            if (value != null && DbContext != null)
            {
                ReflectionProvider.TrySetMemberValue<DbContext>(value, "db", DbContext);
            }
        }
    }
}
