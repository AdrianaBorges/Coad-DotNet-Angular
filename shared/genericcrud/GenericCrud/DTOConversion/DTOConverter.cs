using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.DTOConversion.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.DTOConversion
{
    public abstract class DTOConverter<TSource, TDestiny>
    {
        public abstract void Convert(TSource objOrigem, TDestiny objDestino, ConversionService<TSource, TDestiny> conversionService);       
    }
}
