using Castle.Core;
using Castle.MicroKernel;
using Coad.GenericCrud.Config;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.Reflection;
using GenericCrud.Config;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Config.DataAttributes.Maps;
using GenericCrud.Dao;
using GenericCrud.Models;
using GenericCrud.Models.Comparators;
using GenericCrud.Models.Filtros;
using GenericCrud.Service;
using GenericCrud.Service.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Coad.GenericCrud.Service.Base
{
    public class GenericService<T, D, Id> : IDisposable, IService<T, D, Id>, IServiceAssociator, IBaseService where T : class
    {
        private AbstractGenericDao<T, D, Id> _dao;
        public bool useDbContextCache { get; set; }
        public IKernel kernel { get; set; }
        protected string ProfileName { get; set; }

        public AbstractGenericDao<T,D, Id> Dao {set {

            _dao = value;
            value.useDbContextCache = this.useDbContextCache;
            if (value != null)
            {
                var configAttribute = ReflectionProvider.GetAttribute<ServiceConfigAttribute>(this);

                if (configAttribute != null)
                {
                    AttributeConfigurator.Apply(value, configAttribute);
                }
            }
        }
            get{ return _dao; }
        }

        public GenericService(bool useDbContextCache = true)
        {
            this.useDbContextCache = useDbContextCache;
            init();
        }

        public GenericService(AbstractGenericDao<T, D, Id> _dao)
        {
            this.Dao = _dao;
            this.useDbContextCache = true;
            init();
        }

        private void init()
        {
            ServiceAssociationKeys = new Dictionary<string, ServiceAssociationConfig>();
            var configAttribute = ReflectionProvider.GetAttribute<ServiceConfigAttribute>(this);

            if (configAttribute != null)
            {
                AttributeConfigurator.Apply(this, configAttribute);  
            }
        }

        public void AddServiceAssociationConfig(string nome, ServiceAssociationConfig serviceAssoConfig)
        {
            ServiceAssociationKeys[nome] = serviceAssoConfig;
        }

        public ServiceAssociationConfig GetServiceAssociationConfig(string nome)
        {
            return ServiceAssociationKeys[nome]; 
        }


        /// <summary>
        /// Comparador padrão do objeto 
        /// </summary>
        public IEqualityComparer<D> Comparator {set; get;} 

        /// <summary>
        /// Nome da Chave(s) primária
        /// </summary>
        public string[] keys {set; get;} 
        public Dictionary<string, ServiceAssociationConfig> ServiceAssociationKeys {get; set;}

        public void SetProfileName(string profileName)
        {
            ProfileName = profileName;
            Dao.SetProfileName(profileName);
        }
        public virtual IList<D> FindAll()
        {
            return Dao.FindAllConverted();
        }

        public virtual Pagina<D> FindAll(int pagina, int pageSize = 15)
        {
            return Dao.FindAllCoverted(pagina, pageSize);
        }

        public virtual D FindById(params object[] id)
        {
            return Dao.FindByIdConverted(id);
        }

        public virtual TDestiny FindById<TDestiny>(string profileName, params object[] id)
        {
            return Dao.FindById<TDestiny>(profileName, id);
        }

        public virtual D Save(D source)
        {
            return Dao.ConvertAndSave(source);
        }

        public virtual D Merge(D source, params string[] nameId)
        {
            nameId = GetKeys(nameId);
            return Dao.ConvertAndMerge(source, nameId);
        }

        public virtual D Update(D source)
        {
            return Dao.ConvertAndUpdate(source);
        }

        public virtual void Delete(D source, params string[] nameId)
        {
            nameId = GetKeys(nameId);
            Dao.ConvertAndDelete(source, nameId);
        }

        public void Dispose()
        {
            Dao.Dispose();
        }

        /// <summary>
        /// Salva uma coleção de registros em massa. E cria uma transação para cada quantidade de registros definidos no batchSize
        /// </summary>
        /// <param name="lstObj">Objetos a serem salvos.</param>
        /// <param name="propNames">Nome da chave primária dos items da lista</param>
        /// <param name="saveChanges">Chama o saveChanges do Entity quando a intereção chega no batchsize</param>
        /// <param name="batchSize">Quantos registros devem ser salvos a cada transação</param>
        public virtual IEnumerable<D> SaveAll(IEnumerable<D> lstObj, int batchSize, bool saveChanges = true)
        {
            var obj = Dao.ConvertAndSaveAll(lstObj, batchSize, saveChanges);
            return obj;
        }

        /// <summary>
        /// Salva uma coleção de registros em massa.
        /// </summary>
        /// <param name="lstObj">Objetos a serem salvos.</param>
        /// <param name="propNames">Nome da chave primária dos items da lista</param>      
        public virtual IEnumerable<D> SaveAll(IEnumerable<D> lstObj, bool saveChanges = true)
        {
            var obj = Dao.ConvertAndSaveAll(lstObj,saveChanges);
            return obj;
        }

        [Obsolete()]
        public virtual void UpdateAll(IEnumerable<D> lstObj, string nameId = null, bool saveChanges = true)
        {
            Dao.ConvertAndUpdateAll(lstObj, nameId, saveChanges);
        }

        public virtual void MergeAll(IEnumerable<D> lstObj, string nameId = null, bool saveChanges = true)
        {
            if (nameId == null)
            {
                var lstValue = GetKeys(null);
                Dao.ConvertAndMergeAll(lstObj, saveChanges, lstValue);
            }
            else
            {
                Dao.ConvertAndMergeAll(lstObj, saveChanges, nameId);
            }
        }

        public virtual void MergeAll(IEnumerable<D> lstObj, bool saveChanges = true, params string[] nameId)
        {
            nameId = GetKeys(nameId);
            Dao.ConvertAndMergeAll(lstObj, saveChanges, nameId);
        }

        public virtual void DeleteAll(IEnumerable<D> lstObj, params string[] nameId)
        {

            nameId = GetKeys(nameId);
            Dao.ConvertAndDeleteAll(lstObj, nameId);
        }

        public IList<D> BulkInsert(IEnumerable<D> lstEntities, int batchSize = 100, bool ValidateOnSaveEnabled = false, bool AutoDetectChangesEnabled = false, BatchStatus batchStatus = null)
        {
            return Dao.BulkInsert(lstEntities, batchSize, ValidateOnSaveEnabled, AutoDetectChangesEnabled, batchStatus);
        }

        public void BulkMerge(IEnumerable<D> lstEntities, int batchSize = 100, bool ValidateOnSaveEnabled = false, bool AutoDetectChangesEnabled = false)
        {
            _dao.BulkMerge(lstEntities, batchSize, ValidateOnSaveEnabled, AutoDetectChangesEnabled);
        }

        public IEnumerable<D> BulkInsertOrMerge(IEnumerable<D> list, int batchSize = 100, bool ValidateOnSaveEnabled = false, bool AutoDetectChangesEnabled = false, BatchStatus batchStatus = null)
        {
            return _dao.BulkInsertOrMerge(list, keys, batchSize, ValidateOnSaveEnabled, AutoDetectChangesEnabled, batchStatus);
        }


        /** TODO: Documentar*/
        public void SaveChangesAndRefleshContext()
        {
            if (Dao != null)
            {
                Dao.SaveChangesAndRefleshContext();
            }
        }

        public void DisableChangesAutoDetect()
        {
            if (Dao != null)
            {
                Dao.LigarOtimizacaoDeUpdate();
            }
        }

        public virtual D FindByIdFullLoaded(params object[] id)
        {
            return FindById(id);
        }

        public void GetAssociations(D obj, params string[] names)
        {
            foreach (var name in names)
            {
               var lstServices = AttributeConfigurator.GetPropertyByServicePropertyAttributeName(this, name);

                if (lstServices != null)
                {
                    foreach (var associationService in lstServices)
                    {
                        if (associationService != null)
                        {
                            associationService.FillComplexTypeProperty<D>(obj, name);
                        }
                    }
                }
            }            
            
        }

        public void GetAssociations(IEnumerable<D> lstObj, params string[] names)
        {
            if (lstObj != null)
            {
                foreach (var obj in lstObj)
                {
                    GetAssociations(obj, names);
                }
            }
        }
        public void FillComplexTypeProperty<TDestiny>(TDestiny obj, string name)
        {
            var serviceAssociationConfig = ServiceAssociationKeys[name];

            if (serviceAssociationConfig != null && serviceAssociationConfig.Keys != null)
            {
                if (serviceAssociationConfig.FindById)
                {
                    var lstParam = ReflectionProvider.GetPropertiesValues(obj, serviceAssociationConfig.Keys);
                    var associationObj = FindById(lstParam);
                    ReflectionProvider.SetPropertyValue<D>(obj, serviceAssociationConfig.PropertyName, associationObj);
                }
                else
                {
                    var lstParam = ReflectionProvider.GetPropertiesValuesAsQueryParams(obj, serviceAssociationConfig.Keys);
                    var lstResult = FindAllByFilters(lstParam);

                    ReflectionProvider.SetPropertyValue<IList<D>>(obj, serviceAssociationConfig.PropertyName, lstResult);
                }            
            }
        }

        /// <summary>
        /// Retorna uma cópia do registro atualizado do banco
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private D GetFreshCopy<TReturn>(D obj,params string[] nameId)
        {
            object[] ids = new object[nameId.Count()];

            if (nameId != null && nameId.Count() > 0)
            {
                int index = 0;
                foreach (var idObj in nameId)
                {
                    ids[index] = ReflectionProvider.GetPropertyValue<Id>(obj, idObj);
                    index++;
                }
            }
            else
            {
                ids = new object[] { ReflectionProvider.GetPropertyValue<Id>(obj, "ID") };
            }

            D objEntry = FindByIdFullLoaded(ids); 
            return objEntry;
        }

        public IList<D> FindAllByFilters(IList<QueryParam> lstParam)
        {
            return _dao.FindAllByFilters(lstParam);
        }

        public void ExcluirList<TParentType>(TParentType obj, TParentType freshCopyObj, string collectionPropertyName)
        {
            ExcluirList<TParentType, D>(obj, freshCopyObj, collectionPropertyName);
        }      

        public void ExcluirList<TParentType, TProperty>(TParentType obj, TParentType freshCopyObj, string collectionPropertyName)
        {
            if (obj == null)
            {
                throw new Exception("Argumento obj não pode ser null");
            }

            if (freshCopyObj == null)
            {
                throw new Exception("Argumento obj não pode ser null");
            }

            if (string.IsNullOrWhiteSpace(collectionPropertyName))
            {
                throw new Exception("Argumento obj não pode ser null");
            }

            var collectionProperty = ReflectionProvider.GetPropertyValue<ICollection<TProperty>>(obj, collectionPropertyName);
            if (collectionProperty != null)
            {

                var excecoes = collectionProperty;
                var originalCollection = ReflectionProvider.GetPropertyValue<ICollection<TProperty>>(freshCopyObj, collectionPropertyName);

                if (originalCollection != null)

                    if (originalCollection != null && excecoes != null)
                    {
                        var collectionToExclude = originalCollection.Except(excecoes, (IEqualityComparer<TProperty>) GetComparator(true));

                        if (collectionToExclude != null && collectionToExclude.Count() > 0)
                        {
                            DeleteAll((IEnumerable<D>) collectionToExclude);
                        }

                    }

                }
            }


        /// <summary>
        /// Compara e retorna os itens que estão presentes no obj mais não estão presentes no freshCopyObj
        /// </summary>
        /// <typeparam name="TParentType"></typeparam>
        /// <param name="obj"></param>
        /// <param name="freshCopyObj"></param>
        /// <param name="collectionPropertyName"></param>
        public IEnumerable<D> GetMissinList<TParentType>(TParentType obj, TParentType freshCopyObj, string collectionPropertyName)
        {
            return GetMissinList<TParentType, D>(obj, freshCopyObj, collectionPropertyName);
        }

        /// <summary>
        /// Compara e retorna os itens que estão presentes no obj mais não estão presentes no freshCopyObj
        /// </summary>
        /// <typeparam name="TParentType"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="obj"></param>
        /// <param name="freshCopyObj"></param>
        /// <param name="collectionPropertyName"></param>
        /// <returns></returns>
        public IEnumerable<TProperty> GetMissinList<TParentType, TProperty>(TParentType obj, TParentType freshCopyObj, string collectionPropertyName)
        {
            IEnumerable<TProperty> missinCollection = new List<TProperty>();
            if (obj == null)
            {
                throw new Exception("Argumento obj não pode ser null");
            }

            if (freshCopyObj == null)
            {
                throw new Exception("Argumento obj não pode ser null");
            }

            if (string.IsNullOrWhiteSpace(collectionPropertyName))
            {
                throw new Exception("Argumento obj não pode ser null");
            }

            var collectionProperty = ReflectionProvider.GetPropertyValue<ICollection<TProperty>>(obj, collectionPropertyName);
            if (collectionProperty != null)
            {
                var lstMissin = GetMissinList<TParentType, TProperty>(freshCopyObj, collectionPropertyName, collectionProperty);
                return lstMissin;
                
            }

            return missinCollection;
        }


        /// <summary>
        /// Compara duas propriedades e retorna as que não estão presente no objeto freshCopyObj
        /// </summary>
        /// <typeparam name="TParentType"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="freshCopyObj"></param>
        /// <param name="lstPropriedadeASerTestada"></param>
        /// <param name="collectionPropertyName"></param>
        /// <returns></returns>
        public IEnumerable<TProperty> GetMissinList<TParentType, TProperty>(TParentType freshCopyObj, string collectionPropertyName, ICollection<TProperty> lstPropriedadeASerTestada)
        {
            IEnumerable<TProperty> missinCollection = new List<TProperty>();
            if (lstPropriedadeASerTestada != null)
            {
                var excecoes = lstPropriedadeASerTestada;
                var originalCollection = ReflectionProvider.GetPropertyValue<ICollection<TProperty>>(freshCopyObj, collectionPropertyName);

                    if (originalCollection != null && excecoes != null)
                    {
                    //    missinCollection = originalCollection.Except(excecoes, (IEqualityComparer<TProperty>)GetComparator(true));

                    //    return missinCollection;
                        missinCollection = GetMissinList(originalCollection, excecoes); 
                    }
                    return missinCollection;

            }

            return missinCollection;

        }

        /// <summary>
        /// Compara as duas coleções e traz os elementos presentes no primeiro argumento e que não estão presentes no segundo argumento
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="currentCollection"></param>
        /// <param name="exceptCollection"></param>
        /// <returns></returns>
        public IEnumerable<TProperty> GetMissinList<TProperty>(ICollection<TProperty> currentCollection, ICollection<TProperty> exceptCollection)
        {
            return GetMissinList(currentCollection, exceptCollection, (IEqualityComparer<TProperty>)GetComparator(true));
        }

        /// <summary>
        /// Compara as duas coleções e traz os elementos presentes no primeiro argumento e que não estão presentes no segundo argumento
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="currentCollection"></param>
        /// <param name="exceptCollection"></param>
        /// <returns></returns>
        public IEnumerable<TProperty> GetMissinList<TProperty>(ICollection<TProperty> currentCollection, ICollection<TProperty> exceptCollection, IEqualityComparer<TProperty> comparator)
        {
            IEnumerable<TProperty> missinCollection = new List<TProperty>();
            if (currentCollection != null && exceptCollection != null)
            {
                missinCollection = currentCollection.Except(exceptCollection, (IEqualityComparer<TProperty>)comparator);
                return missinCollection;
            }

            return missinCollection;

        }

        public void ExcluirPropertyList<TObject>(TObject obj, string configName) where TObject : D
        {
            ExcluirPropertyList<TObject, object>(obj, configName);
        }

        /// <summary>
        /// Dado um objeto pai que possui a chave primária, verifica se a lista de filhos possui essa chave.
        /// Se a chave não existir, ela é atribuída.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="obj"></param>
        /// <param name="childLst"></param>
        /// <param name="keys"></param>
        public void CheckAndAssignKeyFromParentToChildsList<TEnum>(object obj, IEnumerable<TEnum> childLst, params string[] objKeys)
        {
            ReflectionProvider.CheckAndAssignKeyFromParentToChildsList(obj, childLst, objKeys);
        }

        public void ExcluirPropertyList<TObject, TProperty>(TObject obj, string name) where TObject : D
        {
            var keyValues = ReflectionProvider.GetPropertiesValues(obj, this.keys);
            D freshObj = FindById(keyValues);

            GetAssociations(freshObj, name);

                var lstServices = AttributeConfigurator.GetPropertyByServicePropertyAttributeName(this, name);

                if (lstServices != null)
                {
                    var ser = lstServices.FirstOrDefault();
                    var config = ser.GetServiceAssociationConfig(name);
                    var propertyName = config.PropertyName;
                    var keys = config.Keys;
                    ReflectionProvider.CheckAndAssignKeyFromParentToChildsList(obj, propertyName, keys);
                    ser.ExcluirList<D>(obj, freshObj, propertyName);                    
                }
            
            
        }

        public ServiceAssociationConfig ObterConfigAssociationPorNome(string name)
        {
            var lstServices = AttributeConfigurator.GetPropertyByServicePropertyAttributeName(this, name);

            if (lstServices != null)
            {
                var ser = lstServices.FirstOrDefault();

                var config = ser.GetServiceAssociationConfig(name);
                return config;
            }

            return null;
        }

        /// <summary>
        /// Retorna propNames se estiver disponível. Senão retorna a propriedade serviceAssociationConfig
        /// </summary>
        /// <param name="propNames"></param>
        /// <returns></returns>
        private string[] GetKeys(string[] nameId)
        {
            if (nameId != null && nameId.Length > 0)
            {
                return nameId;
            }

            if ((nameId == null || nameId.Length <= 0) && (keys != null && keys.Length > 0))
            {
                return keys;
            }
            else 
            {
                throw new InvalidOperationException("Nome(s) da(s) chave(s) primária(s) não encontrado. Defina o(s) nome(s) da(s) chave(s) primária(s) pelo Attribute (ServiceConfigAttribute), pelo método SetKeys(params string[] keys) ou indique isso no argumento do método.");
            
            }
        }


        /// <summary>
        /// Retorna o comparador do objeto
        /// </summary>
        /// <returns></returns>
        public IEqualityComparer<D> GetComparator(bool GetGenericComparator = false)
        {
            
            if (GetGenericComparator)
            {
                IEqualityComparer<D> genericComparer = new GenericComparator<D>(keys);
                return genericComparer;
            }
            else
            if (Comparator != null)
            {
                return Comparator;
            }
            else
            {
                return null;
            }
        }

        public void SetKeys(params string[] keys)
        {
            this.keys = keys; 
        }

        /// <summary>
        /// Salva a entidade que não tem o seu código auto-gerado  pelo banco, e sim fornecido préviamente. Ele verifica no banco se a chave já existe no banco, decidindo se ele deve ser salvo ou atualizado.
        /// </summary>
        /// <param name="list">Lista de objetos a serem salvos</param>
        /// <param name="FindMethodName">Se informado, nome de um método que verifica se o objeto existe no banco. Caso null, o método chama o FindById padrão.</param>
        public void SaveOrUpdateNonIdentityKeyEntity(IEnumerable<D> list, string FindMethodName)
        {
            SaveOrUpdateNonIdentityKeyEntity(list, FindMethodName, null, false);
        }

        /// <summary>
        /// Salva a entidade que não tem o seu código auto-gerado  pelo banco, e sim fornecido préviamente. Ele verifica no banco se a chave já existe no banco, decidindo se ele deve ser salvo ou atualizado.
        /// </summary>
        /// <param name="list">Lista de objetos a serem salvos</param>
        /// <param name="FindMethodName">Se informado, nome de um método que verifica se o objeto existe no banco. Caso null, o método chama o FindById padrão.</param>
        /// <param name="callBack">Ação que será executada com o objeto antes de seu salvamento</param>
        /// <param name="searchNames">Nome das chaves do objeto.</param>
        public void SaveOrUpdateNonIdentityKeyEntity(IEnumerable<D> list, string FindMethodName = null, ActionCallback<D> callBack = null, bool bulkOperation = false)
        {
            if (list != null)
            {
                IList<D> lstMerge = new List<D>();
                IList<D> lstSave = new List<D>();

                foreach (var item in list)
                {
                    var exists = false;

                    var actuallySeachName = GetKeys(null);
                    var searchValues = ReflectionProvider.GetPropertiesValues(item, actuallySeachName);
                    if (!string.IsNullOrWhiteSpace(FindMethodName))
                    {
                        
                        exists = ReflectionProvider.CallMethod<bool>(this, FindMethodName, searchValues);
                    }
                    else 
                    {
                        exists = (FindById(searchValues) != null);
                    }

                    if(exists)
                    {
                        if (callBack != null && callBack.updateCallback != null)
                        {
                            callBack.updateCallback(item);
                        }

                        lstMerge.Add(item);
                    }
                    else
                    {
                        if (callBack != null && callBack.saveCallback != null)
                        {
                            callBack.saveCallback(item);
                        }
                        lstSave.Add(item);
                    }
                }

                if (bulkOperation)
                {
                    BulkMerge(lstMerge);
                    BulkInsert(lstSave);
                }
                else
                {
                    MergeAll(lstMerge, true);
                    SaveAll(lstSave);
                }
            }
        }

        /// <summary>
        /// Salva a entidade que não tem o seu código auto-gerado  pelo banco, e sim fornecido préviamente. Ele verifica no banco se a chave já existe no banco, decidindo se ele deve ser salvo ou atualizado.
        /// </summary>
        /// <param name="list">Lista de objetos a serem salvos</param>
        public void SaveOrUpdateNonIdentityKeyEntity(IEnumerable<D> list)
        {
            SaveOrUpdateNonIdentityKeyEntity(list,null);
        }

        /// <summary>
        /// Salva a entidade que não tem o seu código auto-gerado  pelo banco, e sim fornecido préviamente. Ele verifica no banco se a chave já existe no banco, decidindo se ele deve ser salvo ou atualizado.
        /// </summary>
        /// <param name="obj">Objeto a ser salvo</param>
        /// <param name="FindMethodName">Se informado, nome de um método que verifica se o objeto existe no banco. Caso null, o método chama o FindById padrão.</param>
        /// <param name="callBack">Ação que será executada com o objeto antes de seu salvamento</param>
        public void SaveOrUpdateNonIdentityKeyEntity(D obj, string FindMethodName = null, ActionCallback<D> callBack = null)
        {
            SaveOrUpdateNonIdentityKeyEntity(new List<D>(){ obj}, FindMethodName = null, callBack);
        }


        public virtual D SaveOrUpdate(D model)
        {
            if (ReflectionProvider.HasPropertiesValues(model, keys))
            {
                return Merge(model);
            }
            else
            {
                return Save(model);
            }
        }

        /// <summary>
        /// Salva a entidade que possui chave composta, decidindo se ele deve ser salvo ou atualizado.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="FindMethodName"></param>
        /// <param name="searchValues"></param>
        public IEnumerable<D> SaveOrUpdateAll(IEnumerable<D> list)
        {
            //IEnumerable<D> lstResp = new List<D>();
                
            //if (list != null)
            //{
            //    IList<D> lstMerge = new List<D>();
            //    IList<D> lstSave = new List<D>();

            //    foreach (var item in list)
            //    {
            //        if (ReflectionProvider.HasPropertiesValues(item, keys))
            //        {                 
            //            lstMerge.Add(item);
            //        }
            //        else
            //        {
            //            lstSave.Add(item);
            //        }
            //    }

            //    MergeAll(lstMerge, true);
            //    var lstSaved = SaveAll(lstSave);
                
            //    lstResp = lstResp.Concat(lstMerge).Concat(lstSaved);
            //}

            //return lstResp;
            return _dao.SaveOrUpdateAll(list);
        }

        public A ConvertWithProfile<S, A>(S obj, string profileName)
        {
            var _mapper = MapperEngineFactory.criarMapperEngine(profileName);
            var resp = _mapper.Convert<S, A>(obj);
            return resp;
        }

        public IEnumerable<A> ConvertWithProfile<S, A>(List<S> obj, string profileName)
        {
            var _mapper = MapperEngineFactory.criarMapperEngine(profileName);
            var resp = _mapper.Convert<IEnumerable<S>, IEnumerable<A>>(obj);

            return resp;
        }

        public ProxyTools<D, TProxy> GetProxyTools<TProxy>(string profileName = "proxy")
        {
            return new ProxyTools<D, TProxy>(profileName);
        }
        

        public IList<D> Search(AbstractSearchParams<T, D> pesquisaParams)
        {
            int pagina = 1;
            int registroPorPagina = 15;
            IList<QueryParam> queryParams = null; 
            if (pesquisaParams != null)
            {
                pagina = pesquisaParams.Pagina;
                registroPorPagina = pesquisaParams.PageSize;
                queryParams = pesquisaParams.GetParams();
            }
            return _dao.Search(queryParams, pagina, registroPorPagina);
        }

        public D Salvar(D entity)
        {
            var d = SaveOrUpdate(entity);
            return d;
        }

        public ICollection<FiltroGrupoDTO> GetFilters(string name = null)
        {
            var serviceReflectionPro = new ServiceReflectionProvider<T, D, Id>(ProfileName);
            var lstParams = serviceReflectionPro.GetFilters(name);
            return lstParams;
        }

        public IList<FiltroSelectItemDTO> GerarSelectItems(string valueLabel, string valueName)
        {
            var lstEntity = FindAll();
            var lstSelect = lstEntity.Select(x => new FiltroSelectItemDTO() {
                label = ReflectionProvider.GetPropertyValue<string>(x, valueLabel),
                value = ReflectionProvider.GetPropertyValue<object>(x, valueName)
            })
            .ToList();

            return lstSelect;
        }

        public void Init()
        {
            init();
        }
    }
}