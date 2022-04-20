using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config.ConfigEnuns
{
    public enum LifeStyleTypeEnum
    {
        PER_WEB_REQUEST = 0,
        SCOPE = 1,
        SINGLETON = 2,
        PER_THREAD = 3,
        TRANSIENT = 4
    }
}
