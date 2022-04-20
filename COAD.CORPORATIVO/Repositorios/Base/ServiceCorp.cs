using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Repositorios.Contexto;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;

namespace COAD.CORPORATIVO.Repositorios.Base
{
   public class ServiceCorp<T> : IServiceCorp<T>  where T : class
   {
        protected RepositorioCorp<T> repositorio;

        public ServiceCorp()
        {
            this.repositorio = new RepositorioCorp<T>();
        }
        public virtual void IncluirReg(T item)
        {
            this.repositorio.Incluir(item);
        }
        public virtual void ExcluirReg(T item)
        {
            this.repositorio.Excluir(item);
        }
        public virtual void SalvarReg(T item)
        {
            this.repositorio.Salvar(item);
        }
        public virtual T BuscarPorId(Object id)
        {
            return this.repositorio.Obter(id);
        }
        public virtual IQueryable<T> BuscarTodos()
        {
            return this.repositorio.Tudo();
        }
    }

}
