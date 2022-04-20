using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coad.GenericCrud.Service.Base
{
    interface IService<T,D, Id>
    {
         IList<D> FindAll();
         D FindById(params object[] id);
         D Save(D source);
         D Update(D source);
         D Merge(D source, params string[] nameId);
         void Delete(D source, params string[] nameId);
         void DeleteAll(IEnumerable<D> lstObj, params string[] nameId);
         void Dispose();
         IEnumerable<D> SaveAll(IEnumerable<D> lstObj, bool saveChanges = true);
         IEnumerable<D> SaveAll(IEnumerable<D> lstObj, int batchSize, bool saveChanges = true);
         void MergeAll(IEnumerable<D> lstObj, string nameId = "ID", bool saveChanges = true);
         void UpdateAll(IEnumerable<D> lstObj, string nameId = "ID", bool saveChanges = true);
         void MergeAll(IEnumerable<D> lstObj, bool saveChanges = true, params string[] nameId);
          }
}
