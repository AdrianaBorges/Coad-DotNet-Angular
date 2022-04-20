using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Excel.Metadatas
{
    public class ExcelIgnoreAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
