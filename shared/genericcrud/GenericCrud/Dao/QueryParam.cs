using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Dao
{
    public class QueryParam
    {
        public bool CanBeNull { get; set; }
        public string Key { get; set; }
        public string PropertyPath { get; set; }
        public object Value { get; set; }
    }
}
