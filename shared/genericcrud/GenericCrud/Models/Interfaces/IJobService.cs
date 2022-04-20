using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models.Interfaces
{
    interface IJobService
    {
        void ExecutarAcao(ScheduleContext context);
    }
}
