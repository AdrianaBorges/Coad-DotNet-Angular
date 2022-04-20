using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Service.Base
{
    public class ActionCallback<D>
    {
        private D source { get; set; }
        
        public Action<D> saveCallback { get; set; }
        public Action<D> updateCallback { get; set; }

        public void BeforeSave(Action<D> callback)
        {
            saveCallback = callback;
        }

        public void BeforeUpdate(Action<D> callback)
        {
            updateCallback = callback;
        }
    }
}
