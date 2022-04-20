using GenericCrud.Models.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Service.Base
{
    public interface IBaseService
    {
        void Init();
        IList<FiltroSelectItemDTO> GerarSelectItems(string valueLabel, string valueName);
    }
}
