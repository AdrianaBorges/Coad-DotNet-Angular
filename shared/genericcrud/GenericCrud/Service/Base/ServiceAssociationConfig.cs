using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Service.Base
{
    public class ServiceAssociationConfig
    {
        public string[] Keys { get; set; }
        public string PropertyName { get; set; }
        public bool FindById { get; set; }

        public void AddKeys(params string[] Keys)
        {
            this.Keys = Keys;
        }
    }
}
