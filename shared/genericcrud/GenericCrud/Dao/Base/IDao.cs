using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Dao.Base
{
    interface IDao<T, D,Id>
    {
        IList<T> FindAll();
        T FindById(params object[] id);
        T Save(T t);
        T Update(T t);
        void Delete(T t, params string[] nameId);
        void Delete(Int32 id, params string[] nameId);
        void DeleteAll(IEnumerable<T> lstObj, params string[] nameId);
        void SaveAll(IEnumerable<T> lstObj, bool saveChanges = true);
        void UpdateAll(IEnumerable<T> lstObj, string nameId, bool saveChanges = true);

        /// <summary>
        ///  Atualiza o objeto pegando o objeto do contexto e atualizando seus campos
        ///  de acordo com o objeto passado e depois persistindo a alteração no banco.
        ///  É usado quando o objeto a ser salvo não veio do contexto, ou seja, foi criado 
        ///  fora do contexto e mesmo assim precisa ser atualizado.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        T Merge(T t, params string[] nameId);

        
        /// <summary>
        /// Atualiza vários objetos do banco
        /// </summary>
        /// <param name="lstObj"></param>
        void MergeAll(IEnumerable<T> lstObjn, string nameId = "ID", bool saveChanges = true);
        /// <summary>
        /// Atualiza vários objetos do banco
        /// </summary>
        /// <param name="lstObj"></param>
        void MergeAll(IEnumerable<T> lstObjn, bool saveChanges = true, params string[] nameId);
 }
}
