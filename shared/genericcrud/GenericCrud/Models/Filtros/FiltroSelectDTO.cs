using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models.Filtros
{
    public class FiltroSelectDTO : FiltroDTO
    {
        public FiltroSelectDTO()
        {
            this.TipoEnum = TipoFiltroEnum.Select;
            this.listCombo = new HashSet<FiltroSelectItemDTO>();
        }
        
        public string valueName { get; set; }
        public string labelName { get; set; }
        public ICollection<FiltroSelectItemDTO> listCombo { get; set; }
    }
}
