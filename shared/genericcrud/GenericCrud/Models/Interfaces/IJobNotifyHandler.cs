using GenericCrud.Models.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models.Interfaces
{
    public interface IJobNotifyHandler
    {
        NotifyHandleResult GetJobStatus(int? codRef);
        NotifyHandleResult GetJobStatus(string codRefStr);
    }
}
