using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Contexto;

namespace COAD.SEGURANCA.Repositorios.Base
{
   public class ServicePadrao<T>  where T : class
   {
        protected RepositorioPadrao<T> repositorio;
        public ServicePadrao()
        {
           this.repositorio = new RepositorioPadrao<T>();
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
