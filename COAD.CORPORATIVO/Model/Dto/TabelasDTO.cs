using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(TABELAS_VW))]
    public class TabelasDTO
    {    
        public string nome { get; set; }
        public string schema { get; set; }        
    }
}
