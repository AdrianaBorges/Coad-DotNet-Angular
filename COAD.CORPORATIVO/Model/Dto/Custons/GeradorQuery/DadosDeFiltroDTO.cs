using COAD.CORPORATIVO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace COAD.CORPORATIVO.Model.Dto.Custons.GeradorQuery
{
    public class DadosDeFiltroDTO
    {
        public string Label { get; set; }
        public string NomeCampo { get; set; }

        [ScriptIgnore]
        public Type TipoDeDado { get; set; }
        public object Valor { get; set; }

        public string NomeTipo { get; set; }

    }
}
