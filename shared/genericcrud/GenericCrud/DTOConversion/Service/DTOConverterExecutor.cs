using Coad.GenericCrud.Dao.Base.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.DTOConversion.Service
{
    public class PaginaConverterExecutor<TConverter, TSource, TDestiny> where TConverter : DTOConverter<TSource, TDestiny>
    {
        public IQueryable<TSource> LstOrigem { get; set; }
        private string _profileName { get; set; }
       
        public PaginaConverterExecutor()
        {

        }

        public PaginaConverterExecutor(IQueryable<TSource> lstOrigem)
        {
            this.LstOrigem = lstOrigem;
        }

        public PaginaConverterExecutor(IQueryable<TSource> lstOrigem, string profileName)
        {
            this.LstOrigem = lstOrigem;
            this._profileName = profileName;
        }

        public IQueryable<TDestiny> Convert()
        {
            var customConverter = Activator.CreateInstance<TConverter>();
            var conversionService = new ConversionService<TSource, TDestiny>(_profileName);
            IList<TDestiny> lstResposta = new List<TDestiny>();

            foreach (var objOrigem in LstOrigem)
            {
                var objDestino = conversionService.Convert(objOrigem);
                customConverter.Convert(objOrigem, objDestino, conversionService);
                lstResposta.Add(objDestino);
            }       

            return lstResposta.AsQueryable();
        }
    }
}
