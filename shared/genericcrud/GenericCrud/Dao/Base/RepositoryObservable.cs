using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Dao.Base
{
    public class RepositoryDisposable : IDisposable
    {
        private ISet<IObserver<IRepository>> observadores = new HashSet<IObserver<IRepository>>();
        private IObserver<IRepository> observador;
        private ISet<IRepository> repositorios = new HashSet<IRepository>();
        private IRepository respository;

        public RepositoryDisposable(ISet<IObserver<IRepository>> observadores, IObserver<IRepository> observador, ISet<IRepository> repositorios, IRepository respository)
        {
            this.observadores = observadores;
            this.observador = observador;
            this.repositorios = repositorios;
            this.respository = respository;
        }

        public void Dispose()
        {
            if (observadores != null && observador != null && observadores.Contains(observador))
            {
                observadores.Remove(observador);
            }

            if (repositorios != null && respository != null && repositorios.Contains(respository))
            {
                repositorios.Remove(respository);
            }
        }
    }

    public class RepositoryObservable : IObservable<IRepository>, IDisposable
    {
        private ISet<IObserver<IRepository>> observadores = new HashSet<IObserver<IRepository>>();
        private ISet<IRepository> repositorios = new HashSet<IRepository>();

        public RepositoryObservable()
        {

        }

        public RepositoryObservable(IObserver<IRepository> observer)
        {
            Subscribe(observer);
        }

        public RepositoryObservable(ISet<IObserver<IRepository>> lstObserver)
        {
            if (lstObserver != null)
            {
                foreach (var obs in lstObserver)
                {
                    Subscribe(obs);
                }
            }
        }

        public IDisposable AddRepository(IRepository repository)
        {
            repositorios.Add(repository);
            return new RepositoryDisposable(null, null, repositorios, repository);
        }

        public void NotificarRenovacaoDoContexto()
        {
            if (observadores != null && repositorios != null)
            {
                foreach (var observer in observadores)
                {
                    foreach (var repo in repositorios)
                    {
                        observer.OnNext(repo);
                    }
                }
            }
        }

        public IDisposable Subscribe(IObserver<IRepository> observer)
        {
            observadores.Add(observer);
            return new RepositoryDisposable(observadores, observer, null, null);
        }



        public void Dispose()
        {
            this.repositorios.Clear();
        }
    }
}
