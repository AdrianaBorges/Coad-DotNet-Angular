using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models.Filtros
{
    public class FiltroDTO
    {
        public string label { get; set; }
        public string chave { get; set; }
        public int ordem { get; set; }
        public int size { get; set; }

        public string tipo {
            get {
                if(TipoEnum != null)
                {
                    switch (TipoEnum)
                    {
                        case TipoFiltroEnum.Texto: return "texto";
                        case TipoFiltroEnum.AutoComplete: return "autocomplete";
                        case TipoFiltroEnum.Data: return "data";
                        case TipoFiltroEnum.Grupo: return "grupo";
                        case TipoFiltroEnum.Select: return "select";
                        case TipoFiltroEnum.Toogle: return "toogle";
                        default: return "texto";
                    }
                }
                return "texto";
            }
        }

        public TipoFiltroEnum? TipoEnum { get; set; }

    }
}
