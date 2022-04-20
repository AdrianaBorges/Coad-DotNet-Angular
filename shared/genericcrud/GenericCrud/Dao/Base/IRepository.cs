using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Dao.Base
{
    public interface IRepository
    {
        void RefrechDbContext();
        void Init();
    }
}
