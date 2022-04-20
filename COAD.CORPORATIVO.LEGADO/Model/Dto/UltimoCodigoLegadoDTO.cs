using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Model.Dto
{
    [Mapping(typeof(ULTIMO_CODIGO))]
    public class UltimoCodigoLegadoDTO
    {   
        public decimal CODIGO2 { get; set; }
        public string DV { get; set; }
        public Nullable<decimal> codigo { get; set; }
        
    }
}
