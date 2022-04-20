using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models.SqlDinamico
{
    public class ColunaSqlDinamicoDTO
    {
        public string Nome { get; set; }
        public string Alias { get; set; }
        public Type TipoDeDados { get; set; }
    }
}
