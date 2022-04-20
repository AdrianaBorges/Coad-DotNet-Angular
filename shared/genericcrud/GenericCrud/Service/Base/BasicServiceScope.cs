using Coad.GenericCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Service.Base
{
    /// <summary>
    /// Utilize em aplicações stand alone para criar um dbContext no inicio 
    /// da execução de uma funcionalidade e fechar no seu fim
    /// </summary>
    public class BasicServiceScope : IDisposable
    {
        public BasicServiceScope()
        {
            DbContextFactory.CriarTodosOsDbContexts();
        }

        public void Dispose()
        {
            DbContextFactory.FecharTodosOsDbContexts();
        }
    }
}
