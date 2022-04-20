using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Service.Base
{
    /// <summary>
    /// Define métodoss de acesso para dicionários de ServiceAssociationConfig
    /// </summary>
    public interface IServiceAssociator
    {
        void AddServiceAssociationConfig(string nome, ServiceAssociationConfig serviceAssoConfig);
        ServiceAssociationConfig GetServiceAssociationConfig(string nome);
        void FillComplexTypeProperty<TDestiny>(TDestiny obj, string name);
        void ExcluirList<TParentType>(TParentType obj, TParentType freshCopyObj, string collectionPropertyName);
        void ExcluirList<TParentType, TProperty>(TParentType obj, TParentType freshCopyObj, string collectionPropertyName);
        
        
    }
}
