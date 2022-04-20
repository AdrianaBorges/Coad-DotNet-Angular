using COAD.PROSPECTADOS.Repositorios.Contexto.Base;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROSPECTADOS.Model.Dto
{
    [Mapping(Source = typeof(ULTIMO_CODIGO))]
    public class UltimoCodigoDTO
    {
        public int codigo { get; set; }
        public string dv { get; set; }
    }
}
