using GenericCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Jobs.DataSource
{
    public static class JobsRegister
    {
        public static ICollection<JobSchedulerDTO> Jobs = new HashSet<JobSchedulerDTO>();
        
    }
}
