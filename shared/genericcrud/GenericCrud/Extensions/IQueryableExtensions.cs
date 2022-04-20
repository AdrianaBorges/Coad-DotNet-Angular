using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Mapping;
using GenericCrud.DTOConversion;
using GenericCrud.DTOConversion.Service;
using Coad.GenericCrud.Config;
using GenericCrud.Models.Filtros;
using System.Linq.Dynamic;

namespace Coad.GenericCrud.Extensions
{
    public static class IQueryableExtensions
    {
        public static Pagina<TSource> Paginar<TSource>(this IEnumerable<TSource> query, int numeroPagina = 1, int linhasPorPaginas = 15)
        {
            var pagina = Paginator.paginar<TSource>(query, numeroPagina, linhasPorPaginas);
            return pagina;
        }
        public static Pagina<TSource> Paginar<TSource>(this IEnumerable<TSource> query, RequisicaoPaginacao requisicao)
        {
            if (requisicao == null)
                throw new ArgumentNullException("Informe o argumento requisição");

            if (requisicao.ordenacao != null)
            {
                query = query.Ordenar<TSource>(requisicao.ordenacao);
            }

            var pagina = Paginar<TSource>(query, requisicao.pagina, requisicao.registrosPorPagina);
            return pagina;
        }

        public static Pagina<TDestiny> Paginar<TSource, TDestiny>(this IEnumerable<TSource> query, RequisicaoPaginacao requisicao, string profileName = null)
        {
            if (requisicao == null)
                throw new ArgumentNullException("Informe o argumento requisição");
            
            Pagina<TSource> paginaAnterior = Paginar<TSource>(query, requisicao);
            Pagina<TDestiny> pagina = new Pagina<TDestiny>();
            pagina.pagina = paginaAnterior.pagina;
            pagina.itensPorPagina = paginaAnterior.itensPorPagina;
            pagina.numeroPaginas = paginaAnterior.numeroPaginas;
            pagina.numeroRegistros = paginaAnterior.numeroRegistros;

            var mapper = MapperEngineFactory.criarMapperEngine(profileName);
            pagina.lista = mapper.Convert<IEnumerable<TSource>, List<TDestiny>>(paginaAnterior.lista);
            return pagina;
        }

        public static Pagina<TDestiny> Paginar<TSource, TDestiny>(this IEnumerable<TSource> query, int numeroPagina = 1, int linhasPorPaginas = 15, string profileName = null)
        {
            Pagina<TSource> paginaAnterior = Paginar<TSource>(query, numeroPagina, linhasPorPaginas);
            Pagina<TDestiny> pagina = new Pagina<TDestiny>();
            pagina.pagina = paginaAnterior.pagina;
            pagina.itensPorPagina = paginaAnterior.itensPorPagina;
            pagina.numeroPaginas = paginaAnterior.numeroPaginas;
            pagina.numeroRegistros = paginaAnterior.numeroRegistros;

            var mapper = MapperEngineFactory.criarMapperEngine(profileName);
            pagina.lista = mapper.Convert<IEnumerable<TSource>, List<TDestiny>>(paginaAnterior.lista);
            return pagina;
        }
        

        public static Pagina<TDestiny> Paginar<TConverter, TSource, TDestiny>(this IEnumerable<TSource> query, int numeroPagina = 1, int linhasPorPaginas = 15, string profileName = null) where TConverter : DTOConverter<TSource, TDestiny>
        {
            Pagina<TSource> paginaAnterior = Paginar<TSource>(query, numeroPagina, linhasPorPaginas);
        
            Pagina<TDestiny> pagina = PreConvertPage<TSource, TDestiny>(paginaAnterior);
            PaginaConverterExecutor<TConverter, TSource, TDestiny> conversion = new PaginaConverterExecutor<TConverter, TSource, TDestiny>(paginaAnterior.lista.AsQueryable());
                     
            pagina.lista = conversion.Convert();

            return pagina;
        }

        private static Pagina<TDestiny> PreConvertPage<TSource,TDestiny>(Pagina<TSource> paginaSource)
        {            
            Pagina<TDestiny> pagina = new Pagina<TDestiny>();
            pagina.pagina = paginaSource.pagina;
            pagina.itensPorPagina = paginaSource.itensPorPagina;
            pagina.numeroPaginas = paginaSource.numeroPaginas;
            pagina.numeroRegistros = paginaSource.numeroRegistros;
            return pagina;

        }

        public static IEnumerable<TSource> Ordenar<TSource>(this IEnumerable<TSource> query, FiltroOrdenacao ordenacao)
        {
            if(ordenacao != null && !string.IsNullOrWhiteSpace(ordenacao.property))
            {
                var property = ordenacao.property;
                var order = (!string.IsNullOrWhiteSpace(ordenacao.order)) ? ordenacao.order : "ASC";

                var orderStr = string.Format("{0} {1}", property, order);
                return query.OrderBy(orderStr);                
            }
            return query;
        }
    }
}
