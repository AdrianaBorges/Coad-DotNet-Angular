
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using Coad.Reflection;
using Coad.GenericCrud.Mapping;
using GenericCrud.Dao.Base;
using Coad.GenericCrud.Extensions;
using GenericCrud.DTOConversion;
using System.Transactions;
using AutoMapper;
using COAD.CORPORATIVO.Config;
using Coad.GenericCrud.Config;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using GenericCrud.Models.Comparators;
using GenericCrud.Dao;
using System.Text;
using GenericCrud.Models;
using Castle.Core;
using Castle.MicroKernel;
using GenericCrud.IOCContainer.Proxies;
using GenericCrud.Config.IOCContainer;
using GenericCrud.Models.Filtros;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Config;

namespace Coad.GenericCrud.Dao.Base
{
    public abstract class AbstractGenericDao<T, D, Id> : IDisposable, IDao<T, D, Id>, IRepository where T : class 
    {

        /// <summary>
        /// Define se o db context irá utilizar cache (Quando não utiliza um Container de Inversão de Controle e Injeção de Dependência)
        /// </summary>
        /// <param name="useDbContextCache"></param>
        public AbstractGenericDao(bool useDbContextCache = true)
        {
            this.useDbContextCache = useDbContextCache;
            Init();
        }

        public AbstractGenericDao(IKernel kernel, DbContextAcessor dbAcessor)
        {
            this.kernel = kernel;
            this.DbAcessor = dbAcessor;
            Init();
        }

        /// <summary>
        /// Faz as inicializações necessárias
        /// </summary>
        public void Init()
        {
            var configAttribute = ReflectionProvider.GetAttribute<DAOConfigAttribute>(this);

            if (configAttribute != null)
            {
                AttributeConfigurator.Apply(this, configAttribute);
            }

            _mapper = MapperEngineFactory.criarMapperEngine(profileName);

            if (string.IsNullOrWhiteSpace(profileName))
                profileName = "default";
            
            var profileConfig = ProfileConfigurator.getProfileConfig(profileName);

            if (profileConfig != null && profileConfig.entityType != null)
            {
                this.entityType = profileConfig.entityType;
            }

            if (DbAcessor != null)
            {
                DbAcessor.ProfileName = profileName;
                DbAcessor.EntityType = this.entityType;
            }
        }
        private DbContext _dbCache;

        /// <summary>
        /// Representa a instância do DbContext. Ele funcina tanto na presença do container de IOC quando sem ele.
        /// </summary>
        private DbContext Db 
        {
            get 
            {
                if (kernel != null && DbAcessor != null)
                {
                    return DbAcessor.Db;
                }
                else
                {
                    return _dbCache;
                }
            }
            set 
            {
                if (kernel == null || DbAcessor == null)
                {
                    _dbCache = value;
                }
            }
        }

      
        private DbSet<T> _dbSet { get; set; }
        private MapperEngineWrapper _mapper { set; get; }      
  
        /// <summary>
        /// Instância injetável do container de IOC
        /// </summary>
        public IKernel kernel { get; set; }

        /// <summary>
        /// Classe que trabalha como Proxy para o DbContext
        /// </summary>
        public DbContextAcessor DbAcessor { get; set; }
        public DbContextAcessorNetCore DbAcessorNetCore { get; set; }

        public string[] Keys { set; get; }
        public bool useDbContextCache { get; set; }


        public string profileName { set; get; }
        protected Type entityType { get; set; }

        private DbSet<T> DbSet
        {
            get
            {
                if (kernel != null && DbAcessor != null)
                {
                    return DbAcessor.Db.Set<T>();                    
                }
                else
                {
                    if (_dbSet == null || !IOCContainerProxy.ConfigDbContextInWindsor)
                    {
                        _dbSet = GetDb().Set<T>();
                    }
                    return _dbSet;
                }
            }
            set
            {
                _dbSet = value;
            }
        }

        /// <summary>
        /// Pega o objeto padrão de consulta do entity
        /// </summary>
        /// <returns></returns>
        public DbSet<T> GetDbSet()
        {       
            return DbSet;
        }


        /// <summary>
        /// Pega o objeto padrão do entity utilizando a query padrão
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetDbSetWithTemplate()
        {
            var dbSet = GetDbSet();
            var query = SetTemplateQuery(dbSet);
            return query;
        }

        public void SetProfileName(string entityName)
        {
            this.profileName = entityName;
            Init();
        }

        public void BulkMerge(IEnumerable<D> lstEntities, int batchSize = 100, bool ValidateOnSaveEnabled = false, bool AutoDetectChangesEnabled = false)
        {
            var lstDTO = ToModel(lstEntities);
            BulkMerge(lstDTO, batchSize, ValidateOnSaveEnabled, AutoDetectChangesEnabled);
        }

        public void BulkMerge(IEnumerable<T> lstEntities, int batchSize = 100, bool ValidateOnSaveEnabled = false, bool AutoDetectChangesEnabled = false)
        {
            try
            {
                // garante que o db context desse método é isolado e apenas para essa operação
                bool usaCache = this.useDbContextCache;
                this.useDbContextCache = false;

                var ctx = GetDb(true, ValidateOnSaveEnabled, AutoDetectChangesEnabled);
                var dbSet = ctx.Set<T>();
                int indexRows = 0;

                foreach (var ent in lstEntities)
                {

                    var olderObj = GetFromRegistry(ent, dbSet, Keys);
                    var entry = ctx.Entry(olderObj);
                    entry.CurrentValues.SetValues(ent);
                    entry.State = EntityState.Modified;

                    if (indexRows >= (batchSize - 1))
                    {
                        ctx.SaveChanges();
                        ctx.Dispose();
                        ctx = GetDb(true, ValidateOnSaveEnabled, AutoDetectChangesEnabled);
                        dbSet = ctx.Set<T>();
                        indexRows = 0;
                    }
                    else
                    {
                        indexRows++;
                    }
                }

                if (indexRows > 0)
                {
                    ctx.SaveChanges();
                    ctx.Dispose();
                    ctx = GetDb(true, ValidateOnSaveEnabled, AutoDetectChangesEnabled);
                    dbSet = ctx.Set<T>();

                }

                this.useDbContextCache = usaCache;
            }
            catch (DbEntityValidationException e)
            {
                throw new FormattedDbEntityValidationException(e);
            }
        }

        public IEnumerable<D> BulkInsertOrMerge(IEnumerable<D> lstDTO, string[] keys, int batchSize = 100, bool ValidateOnSaveEnabled = false, bool AutoDetectChangesEnabled = false, BatchStatus batchStatus = null)
        {
            try
            {
                if (batchStatus != null)
                {
                    batchStatus.TotalItens = lstDTO.Count();
                }
                var lstEntities = ToModel(lstDTO);

                // garante que o db context desse método é isolado e apenas para essa operação
                bool usaCache = this.useDbContextCache;
                this.useDbContextCache = false;

                var ctx = GetDb(true, ValidateOnSaveEnabled, AutoDetectChangesEnabled);
                var dbSet = ctx.Set<T>();
                int indexRows = 0;

                foreach (var ent in lstEntities)
                {
                    if (ReflectionProvider.HasPropertiesValues(ent, keys))
                    {
                        var olderObj = GetFromRegistry(ent, dbSet, Keys);
                        var entry = ctx.Entry(olderObj);
                        entry.CurrentValues.SetValues(ent);
                        entry.State = EntityState.Modified;
                    }
                    else
                    {
                        dbSet.Add(ent);
                    }

                    if (batchStatus != null)
                    {
                        batchStatus.ProcessedItens++;
                    }

                    if (indexRows >= (batchSize - 1))
                    {
                        ctx.SaveChanges();
                        ctx.Dispose();
                        ctx = GetDb(true, ValidateOnSaveEnabled, AutoDetectChangesEnabled);
                        dbSet = ctx.Set<T>();
                        indexRows = 0;
                    }
                    else
                    {
                        indexRows++;
                    }
                }

                if (indexRows > 0)
                {
                    ctx.SaveChanges();
                    ctx.Dispose();
                    ctx = GetDb(true, ValidateOnSaveEnabled, AutoDetectChangesEnabled);
                    dbSet = ctx.Set<T>();
                }

                this.useDbContextCache = usaCache;

                return ToDTO(lstEntities);
            }
            catch (DbEntityValidationException e)
            {
                throw new FormattedDbEntityValidationException(e);
            }
        }


        public IEnumerable<D> SaveOrUpdateAll(IEnumerable<D> listDTO)
        {
            try
            {
                var list = ToModel(listDTO);

                if (list != null && list.Count() > 0)
                {
                    foreach (var obj in list)
                    {
                        if (ReflectionProvider.HasPropertiesValues(obj, Keys))
                        {
                            var olderObj = GetFromRegistry(obj, Keys);
                            var entry = Db.Entry(olderObj);
                            entry.CurrentValues.SetValues(obj);
                            entry.State = EntityState.Modified;
                        }
                        else
                        {
                            DbSet.Add(obj);
                        }
                    }
                    
                    Db.SaveChanges();

                    return ToDTO(list);
                }
                return listDTO;
            }
            catch (DbEntityValidationException e)
            {
                throw new FormattedDbEntityValidationException(e);
            }
        }

        public IList<D> BulkInsert(IEnumerable<T> lstEntities, int batchSize = 100, bool ValidateOnSaveEnabled = false, bool AutoDetectChangesEnabled = false, BatchStatus batchStatus = null)
        {
            try
            {

                if (batchStatus != null)
                {
                    batchStatus.TotalItens = lstEntities.Count();
                }
                // garante que o db context desse método é isolado e apenas para essa operação
                bool usaCache = this.useDbContextCache;
                this.useDbContextCache = false;

                var ctx = GetDb(true, ValidateOnSaveEnabled, AutoDetectChangesEnabled);
                int indexRows = 0;
                var dbSet = ctx.Set<T>();

                foreach (var ent in lstEntities)
                {
                    dbSet.Add(ent);

                    if (batchStatus != null)
                    {
                        batchStatus.ProcessedItens++;
                    }

                    if (indexRows >= (batchSize - 1))
                    {
                        ctx.SaveChanges();
                        ctx.Dispose();
                        ctx = GetDb(true, ValidateOnSaveEnabled, AutoDetectChangesEnabled);
                        dbSet = ctx.Set<T>();
                        indexRows = 0;
                    }
                    else
                    {
                        indexRows++;
                    }
                }

                if (indexRows > 0)
                {
                    ctx.SaveChanges();
                    ctx.Dispose();
                    ctx = GetDb(true, ValidateOnSaveEnabled, AutoDetectChangesEnabled);
                    dbSet = ctx.Set<T>();
                          
                }

                this.useDbContextCache = usaCache;

                var lstEntitiesDTO = ToDTO(lstEntities);
                return lstEntitiesDTO;
            }
            catch (DbEntityValidationException e)
            {
                throw new FormattedDbEntityValidationException(e);
            }
        }

        public IList<D> BulkInsert(IEnumerable<D> lstEntities, int batchSize = 100, bool ValidateOnSaveEnabled = false, bool AutoDetectChangesEnabled = false, BatchStatus batchStatus = null)
        {
            
            IList<T> lst = ToModel(lstEntities);
            return BulkInsert(lst, batchSize, ValidateOnSaveEnabled, AutoDetectChangesEnabled, batchStatus);
        }

        public Pagina<T> Paginate(int numeroPagina, IQueryable<T> query, int pageSize = 15, bool defaultOrder = false)
        {
            if (pageSize == null)
                pageSize = 15;

            if(defaultOrder)
                query = getDefaultQuery(query);
            
            /*
            Pagina<T> pagina = new Pagina<T>();
            pagina.pagina = numeroPagina;
            pagina.itensPorPagina = pageSize;
           
            var resp = Paginator.paginar<T>(query, pagina);
            pagina.lista = resp.ToList();
             */

            var pagina = query.Paginar(numeroPagina, pageSize);
            return pagina;
        }

        public int NumeroDePaginas(int registrosPorPagina, IQueryable<T> query)
        {
            //try
            //{
            //    query = getDefaultQuery(query);
            //}
            //catch (Exception e)
            //{
            //    throw new NegocioException("Não é possível paginar, extenda o método getDefaultQuery para determinar ordenação padrão.", e);
            //}

            return Paginator.numeroPaginas<T>(query, registrosPorPagina);
        }

        
        /// <summary>
        /// Retorna o Db Context utilizando o container de Injeção de Dependência e Inversão de Controle.
        /// </summary>
        /// <param name="getNew"></param>
        /// <param name="ValidateOnSaveEnabled"></param>
        /// <param name="AutoDetectChangesEnabled"></param>
        /// <returns></returns>
        private DbContext GetDbWithIOC(bool getNew = false, bool ValidateOnSaveEnabled = true, bool AutoDetectChangesEnabled = true)
        {
            ChecarECriarDbContextAcesso();

            if (IOCContainerProxy.ConfigDbContextInWindsor)
            {
                Db = DbAcessor.Db;

                if (DbAcessor.Db != null)
                {
                    DbAcessor.Db.Configuration.ValidateOnSaveEnabled = ValidateOnSaveEnabled;
                    DbAcessor.Db.Configuration.AutoDetectChangesEnabled = AutoDetectChangesEnabled;
                    _dbSet = Db.Set<T>();
                }

                if (getNew)
                {
                    var dbType = ((object)Db).GetType();
                    var db = (DbContext)Activator.CreateInstance(dbType);

                    db.Configuration.ValidateOnSaveEnabled = ValidateOnSaveEnabled;
                    db.Configuration.AutoDetectChangesEnabled = AutoDetectChangesEnabled;

                    return db;

                }
                return Db;
            }
            else
            {
                var dbContext = DbAcessorNetCore.GetDbContext<DbContext>(entityType);
                dbContext.Configuration.ValidateOnSaveEnabled = ValidateOnSaveEnabled;
                dbContext.Configuration.AutoDetectChangesEnabled = AutoDetectChangesEnabled;
                _dbSet = dbContext.Set<T>();
                Db = dbContext;

                return dbContext;
            }
        }

        /// <summary>
        /// Retorna o Db Context de maneira manual, sem utilizar container de Injeção de Dependência e Inversão de Controle.
        /// </summary>
        /// <param name="getNew"></param>
        /// <param name="ValidateOnSaveEnabled"></param>
        /// <param name="AutoDetectChangesEnabled"></param>
        /// <returns></returns>
        private DbContext GetDbWithoutIOC(bool getNew = false, bool ValidateOnSaveEnabled = true, bool AutoDetectChangesEnabled = true)
        {

            if (getNew)
            {
                if (Db != null)
                {
                    Db = null;
                }
            }
            
            DbContext dbCtx = null;
                dbCtx = DbContextFactory.criarDbContext(profileName, getNew, this, this.useDbContextCache);
            

            dbCtx.Configuration.AutoDetectChangesEnabled = AutoDetectChangesEnabled;
            dbCtx.Configuration.ValidateOnSaveEnabled = ValidateOnSaveEnabled;

            DbSet = dbCtx.Set<T>();
            Db = dbCtx;
            return dbCtx;
        }

        public DbContext GetDb(bool getNew = false, bool ValidateOnSaveEnabled = true, bool AutoDetectChangesEnabled = true)
        {
            var useContainer = IOCContainerProxy.UseIOCContainer;

            if (useContainer)
            {
                if (kernel != null && DbAcessor != null)
                {
                    return GetDbWithIOC(getNew, ValidateOnSaveEnabled, AutoDetectChangesEnabled);

                }
                else
                {
                    var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();

                    if (containerConfig != null && containerConfig.Container != null)
                    {
                        if (kernel == null)
                            kernel = containerConfig.Container.Kernel;

                        ChecarECriarDbContextAcesso();
                        return GetDbWithIOC(getNew, ValidateOnSaveEnabled, AutoDetectChangesEnabled);
                    }
                    else
                    {
                        return GetDbWithoutIOC(getNew, ValidateOnSaveEnabled, AutoDetectChangesEnabled);
                    }
                }
            }
            else
            {
                return GetDbWithoutIOC(getNew, ValidateOnSaveEnabled, AutoDetectChangesEnabled);
            }

        }

        public void ChecarECriarDbContextAcesso()
        {
            if (IOCContainerProxy.ConfigDbContextInWindsor)
            {
                if (DbAcessor == null || (DbAcessor != null && DbAcessor.IsDisposed))
                {
                    DbAcessor = kernel.Resolve<DbContextAcessor>();
                    DbAcessor.ProfileName = profileName;
                    DbAcessor.EntityType = entityType;
                }
            }
            else
            {
                if (DbAcessorNetCore == null || (DbAcessorNetCore != null && DbAcessorNetCore.IsDisposed))
                {
                    DbAcessorNetCore = kernel.Resolve<DbContextAcessorNetCore>();                
                }
            }
        }

        public Db GetDb<Db>(bool getNew = false, bool ValidateOnSaveEnabled = true, bool AutoDetectChangesEnabled = true) where Db : DbContext
        {
            DbContext dbCtx = GetDb(getNew, ValidateOnSaveEnabled, AutoDetectChangesEnabled);
            return (Db) dbCtx;
        }


        //protected abstract DbSet<T> DefineType(COADSATEntities Db);

        public virtual Pagina<T> FindAll(int pagina, int pageSize = 15)
        {
            IQueryable<T> query = DbSet;            
            return Paginate(pagina, query, defaultOrder: true, pageSize: pageSize);
        }

        public virtual IList<T> FindAll()
        {

            return DbSet.ToList();
        }

        public virtual T FindById(params object[] id)
        {
            return DbSet.Find(id);
        }

        public virtual TSource FindById<TSource>(string profileName, params object[] id)
        {
            var mapper = MapperEngineFactory.criarMapperEngine(profileName);

            if (mapper == null)
            {
                throw new ValidacaoException("O mapper não pode ser criado.");
            }

            TSource obj = mapper.Convert<T, TSource>(DbSet.Find(id));
            return obj;
        }

        public virtual T Save(T t)
        {
            try
            {
                DbSet.Add(t);
                Db.SaveChanges();
                return t;
            }
            catch (DbEntityValidationException e)
            {
               throw new FormattedDbEntityValidationException(e);
            }
           

        }
        public virtual T Update(T t)
        {
            try
            {
                Db.Entry(t).State = EntityState.Modified;
                Db.SaveChanges();
                return t;
            }
            catch (DbEntityValidationException e)
            {
                throw new FormattedDbEntityValidationException(e);
            }
        }

        
        /// <summary>
        ///  Atualiza o objeto pegando o objeto do contexto e atualizando seus campos
        ///  de acordo com o objeto passado e depois persistindo a alteração no banco.
        ///  É usado quando o objeto a ser salvo não veio do contexto, ou seja, foi criado 
        ///  fora do contexto e mesmo assim precisa ser atualizado.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual T Merge(T t, string nameId = "ID")
        {
            //var olderObj = GetFromRegistry(t, propNames);
            //var entry = _Db.Entry(olderObj);
            //entry.CurrentValues.SetValues(t);
            //entry.State = EntityState.Modified;
            //_Db.SaveChanges();
            //return t;

            return Merge(t, nameId);
        }

        public virtual void Delete(T t, params string[] nameId)
        {
            try
            {
                var olderObj = GetFromRegistry(t, nameId);
                DbSet.Remove(olderObj);
                Db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw new FormattedDbEntityValidationException(e);
            }
        }

        public virtual void Delete(Int32 id, params string[] nameId)
        {
            try
            {
                T obj = DbSet.Find(id);
                var newObj = GetFromRegistry(obj, nameId);
                DbSet.Remove(obj);
                Db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw new FormattedDbEntityValidationException(e);
            }
        }

        public virtual void DeleteAll(IEnumerable<T> lstObj, params string[] nameId)
        {
            try
            {
                if (lstObj != null && lstObj.Count() > 0)
                {
                    foreach (var obj in lstObj)
                    {
                        var newObj = GetFromRegistry(obj, nameId);
                        DbSet.Remove(newObj);
                    }
                }

                Db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw new FormattedDbEntityValidationException(e);
            }
        }


        public virtual void SaveAll(IEnumerable<T> lstObj, bool saveChanges = true)
        {
            try
            {
                if (lstObj != null && lstObj.Count() > 0)
                {
                    foreach (var obj in lstObj)
                    {
                        DbSet.Add(obj);
                    }

                    if (saveChanges)
                        Db.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                throw new FormattedDbEntityValidationException(e);
            }
            
        }


        /// <summary>
        /// Salva uma coleção de registros em massa. E cria uma transação para cada quantidade de registros definidos no batchSize
        /// </summary>
        /// <param name="lstObj">Objetos a serem salvos.</param>
        /// <param name="propNames">Nome da chave primária dos items da lista</param>
        /// <param name="saveChanges">Chama o saveChanges do Entity quando a intereção chega no batchsize</param>
        /// <param name="batchSize">Quantos registros devem ser salvos a cada transação</param>
        public virtual void SaveAll(IEnumerable<T> lstObj, int batchSize, bool saveChanges = true)
        {
            try
            {
                batchSize = batchSize | 200;

                if (lstObj != null && lstObj.Count() > 0)
                {

                    int numeroPaginas = NumeroDePaginas(batchSize, lstObj.AsQueryable());

                    for (int batchStep = 1; batchStep <= numeroPaginas; batchStep++)
                    {
                        var pagina = Paginate(batchStep, lstObj.AsQueryable(), batchSize);
                        var lista = pagina.lista;

                        TransactionOptions txOpt = new TransactionOptions();
                        txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                        txOpt.Timeout = TransactionManager.MaximumTimeout;

                        using (var scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                        {

                            foreach (var obj in lista)
                            {
                                DbSet.Add(obj);
                            }

                            if (saveChanges)
                                Db.SaveChanges();
                            scope.Complete();
                        }
                    }

                }
            }
            catch (DbEntityValidationException e)
            {
                throw new FormattedDbEntityValidationException(e);
            }

        }

        public void UpdateAll(IEnumerable<T> lstObj, bool saveChanges = true, params string[] nameId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Atualiza vários objetos do banco
        /// </summary>
        /// <param name="lstObj"></param>
        public virtual void UpdateAll(IEnumerable<T> lstObj, string nameId = "ID", bool saveChanges = true)
        {
            try
            {
                if (lstObj != null && lstObj.Count() > 0)
                {
                    foreach (var obj in lstObj)
                    {
                        Db.Entry(obj).State = EntityState.Modified;
                    }

                    if (saveChanges)
                        Db.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                throw new FormattedDbEntityValidationException(e);
            }
        }

                 
        /// <summary>
        /// O mesmo que o merge, só que é para uma lista de objetos
        /// </summary>
        /// <param name="lstObj"></param>
        public virtual void MergeAll(IEnumerable<T> lstObj, string nameId = "ID", bool saveChanges = true)               
        {

            //if (lstObj != null && lstObj.Count() > 0)
            //{
            //    foreach (var obj in lstObj)
            //    {
            //        var olderObj = GetFromRegistry(obj, propNames);
            //        var entry = _Db.Entry(olderObj);
            //        entry.CurrentValues.SetValues(obj);
            //        entry.State = EntityState.Modified;
            //    }

            //    if (saveChanges)
            //        _Db.SaveChanges();
            //}

            MergeAll(lstObj,true , nameId);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public virtual T GetFromRegistry(T obj, string propNames = "ID")
        //{
        //    Int32 id = ReflectionProvider.GetPropertyValue<Int32>(obj, propNames);
        //    T objEntry = DbSet.Find(id);
        //    return objEntry;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual T GetFromRegistry(T obj, params string[] nameId)
        {
            return GetFromRegistry(obj, DbSet, nameId);
        }

        public virtual T GetFromRegistry(T obj, DbSet<T> dbSet, params string[] nameId)
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

            T objEntry = dbSet.Find(ids);
            return objEntry;
        }


        public void Dispose()
        {
            if (Db != null)
            {
                try
                {
                    Db.Dispose();
                }
                catch(Exception e)
                {
                    throw new Exception("Ocorreu um erro ao tentar dar dispose no objeto", e);
                }

                //try
                //{
                //    //_Db.SaveChanges();
                //}
                //catch(Exception e){
                //    throw new Exception("Ocorreu um erro ao tentar dar dispose no objeto", e);
                //}
                Db = null;
            }
        }

        public void SaveChanges()
        {
            Db.SaveChanges();
        }

        /// <summary>
        /// Lista todos os objetos já convertidos para o modelo da aplicação
        /// </summary>
        /// <returns></returns>
        public virtual IList<D> FindAllConverted()
        {
            return _mapper.Convert<IList<T>, IList<D>>(FindAll());
        }

        /// <summary>
        /// Lista todos os objetos paginados já convertidos para o modelo da aplicação
        /// </summary>
        /// <returns></returns>
        public virtual Pagina<D> FindAllCoverted(int pagina, int pageSize = 15)
        {
            return ToDTO(FindAll(pagina, pageSize));
        }


        public virtual D FindByIdConverted(params object[] id)
        {
            return _mapper.Convert<T, D>(FindById(id));
        }

        public virtual D ConvertAndSave(D source)
        {
            T obj = _mapper.Convert<D, T>(source);
            Save(obj);
            return _mapper.Convert<T, D>(obj);
        }

        public virtual D ConvertAndUpdate(D source)
        {
            T obj = _mapper.Convert<D, T>(source);
            Update(obj);
            return _mapper.Convert<T, D>(obj);
        }

        public virtual D ConvertAndMerge(D source, params string[] nameId)
        {
            T obj = _mapper.Convert<D, T>(source);
            Merge(obj, nameId);
            return _mapper.Convert<T, D>(obj);
        }

        public virtual void ConvertAndDelete(D source, params string[] nameId)
        {
            T obj = _mapper.Convert<D, T>(source);
            Delete(obj, nameId);
        }


        public virtual IEnumerable<D> ConvertAndSaveAll(IEnumerable<D> lstObj, int batchSize, bool saveChanges = true)
        {
            IEnumerable<T> lst = _mapper.Convert<IEnumerable<D>, IEnumerable<T>>(lstObj);
            SaveAll(lst, batchSize, saveChanges);
            IEnumerable<D> lstResp = _mapper.Convert<IEnumerable<T>, IEnumerable<D>>(lst);

            return lstResp;
        }

        public virtual IEnumerable<D> ConvertAndSaveAll(IEnumerable<D> lstObj, bool saveChanges = true)
        {
            IEnumerable<T> lst = _mapper.Convert<IEnumerable<D>, IEnumerable<T>>(lstObj);
            SaveAll(lst, saveChanges);

            IEnumerable<D> lstResp = _mapper.Convert<IEnumerable<T>, IEnumerable<D>>(lst);
            return lstResp;
        }


        public virtual void ConvertAndUpdateAll(IEnumerable<D> lstObj, string nameId = null, bool saveChanges = true)
        {
            IEnumerable<T> lst = _mapper.Convert<IEnumerable<D>, IEnumerable<T>>(lstObj);
            UpdateAll(lst, nameId, saveChanges);
        }

        public virtual void ConvertAndMergeAll(IEnumerable<D> lstObj, string nameId = null, bool saveChanges = true)
        {
            IEnumerable<T> lst = _mapper.Convert<IEnumerable<D>, IEnumerable<T>>(lstObj);
            MergeAll(lst, nameId, saveChanges);
        }

        public virtual void ConvertAndMergeAll(IEnumerable<D> lstObj, bool saveChanges = true, params string[] nameId)
        {
            IEnumerable<T> lst = _mapper.Convert<IEnumerable<D>, IEnumerable<T>>(lstObj);
            MergeAll(lst, saveChanges, nameId);
        }

        public virtual void ConvertAndDeleteAll(IEnumerable<D> lstObj, params string[] nameId)
        {
            IEnumerable<T> lst = _mapper.Convert<IEnumerable<D>, IEnumerable<T>>(lstObj);
            DeleteAll(lst, nameId);
        }

        public virtual D ToDTO(T entityModel)
        {
            return _mapper.Convert<T, D>(entityModel);
        }

        public T ToModel(D dto)
        {
            return _mapper.Convert<D, T>(dto);
        }

        public IList<D> ToDTO(IList<T> entityModel)
        {
            return _mapper.Convert<IList<T>, IList<D>>(entityModel);
        }

        public IList<D> ToDTO(IEnumerable<T> entityModel)
        {
            return _mapper.Convert<IEnumerable<T>, IList<D>>(entityModel);
        }

        public Pagina<D> ToDTOPage(IEnumerable<T> query, int numeroPagina = 1 , int linhasPorPaginas = 10)
        {
            return query.Paginar<T,D>(numeroPagina, linhasPorPaginas, this.profileName);
        }

        public Pagina<D> ToDTOPage<TConverter>(IEnumerable<T> query, int numeroPagina = 1, int linhasPorPaginas = 10) where TConverter : DTOConverter<T, D>
        {
            return query.Paginar<TConverter, T, D>(numeroPagina, linhasPorPaginas, this.profileName);
        }

        public Pagina<D> ToDTOPage(IEnumerable<T> query, RequisicaoPaginacao requisicaoPaginacao)
        {
            return query.Paginar<T, D>(requisicaoPaginacao, this.profileName);
        }

        public Pagina<D> ToDTOPage<TConverter>(IEnumerable<T> query, RequisicaoPaginacao requisicaoPaginacao) where TConverter : DTOConverter<T, D>
        {
            return query.Paginar<TConverter, T, D>(requisicaoPaginacao.pagina, requisicaoPaginacao.registrosPorPagina, this.profileName);
        }

        public Pagina<D> ToDTO(Pagina<T> pageEntityModel)
        {
            Pagina<D> newPage = new Pagina<D>();
            newPage.itensPorPagina = pageEntityModel.itensPorPagina;
            newPage.numeroPaginas = pageEntityModel.numeroPaginas;
            newPage.pagina = pageEntityModel.pagina;
            newPage.lista = ToDTO(pageEntityModel.lista);

            return newPage;
        }

        public IList<T> ToModel(IList<D> dto)
        {
            return _mapper.Convert<IList<D>, IList<T>>(dto);
        }

        public IList<T> ToModel(IEnumerable<D> dto)
        {
            return _mapper.Convert<IList<D>, List<T>>(new List<D>(dto));
        }

        public A Convert<S, A>(S obj)
        {
            return _mapper.Convert<S,A>(obj);
        }

        public IEnumerable<A> Convert<S, A>(List<S> obj)
        {
            return _mapper.Convert<IEnumerable<S>, IEnumerable<A>>(obj);
        }

        /// <summary>
        /// Método para ser sobregarregado para definir queries padrões que serão utilizados por determinados métodos
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public virtual IQueryable<T> getDefaultQuery(IQueryable<T> source)
        {
            return source;
        }

        public void SaveChangesAndRefleshContext()
        {
            GetDb().SaveChanges();
            GetDb().Dispose();
            Db = DbContextFactory.criarDbContext(profileName);
            //_Db.Configuration.AutoDetectChangesEnabled = false;
        }

        public void LigarOtimizacaoDeUpdate()
        {
            Db.Configuration.AutoDetectChangesEnabled = false;
            Db.Configuration.ValidateOnSaveEnabled = false;
        }

        public void DesligarOtimizacaoDeUpdate()
        {
            Db.Configuration.AutoDetectChangesEnabled = true;
            Db.Configuration.ValidateOnSaveEnabled = true;
        }

        public void MergeAll(IEnumerable<T> lstObj, bool saveChanges = true, params string[] nameId)
        {
            try
            {
                if (lstObj != null && lstObj.Count() > 0)
                {
                    foreach (var obj in lstObj)
                    {
                        var olderObj = GetFromRegistry(obj, nameId);
                        var entry = Db.Entry(olderObj);
                        entry.CurrentValues.SetValues(obj);
                        entry.State = EntityState.Modified;
                    }

                    if (saveChanges)
                        Db.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                throw new FormattedDbEntityValidationException(e);
            }
        }


        public T Merge(T t, params string[] nameId)
        {
            try
            {
                var olderObj = GetFromRegistry(t, nameId);
                var entry = Db.Entry(olderObj);
                entry.CurrentValues.SetValues(t);
                entry.State = EntityState.Modified;
                Db.SaveChanges();
                return t;
            }
            catch (DbEntityValidationException e)
            {
                throw new FormattedDbEntityValidationException(e);
            }
        }

        /// <summary>
        /// Define uma query padrão para ser executada a cada execução
        /// </summary>
        /// <param name="dbSet">dbSet</param>
        public virtual IQueryable<T> SetTemplateQuery(DbSet<T> dbSet)
        {
            // Esse método faz o mesmo que o GetDbSet
            // Para utilizar ele da maneira certa esse método deve ser sobreescrito
            return dbSet.AsQueryable();
        }

        public IList<D> FindAllByFilters(IList<QueryParam> lstParam)
        {
            if(lstParam != null)
            {
                StringBuilder sb = new StringBuilder(" ");

                bool primeiro = true;
                foreach (var param in lstParam)
                {
                    if (primeiro)
                    {
                        primeiro = false;
                    }
                    else
                    {
                        sb.Append(" && ");
                    }

                    if (!string.IsNullOrWhiteSpace(param.Key))
                    {
                        sb.Append(param.Key);
                        sb.Append(" == ");

                        if (param.Value == null)
                        {
                            sb.Append("null ");
                        }
                        else
                        if (param.Value is string)
                        {
                            sb.Append("\"");
                            sb.Append(param.Value);
                            sb.Append("\" ");
                        }
                        else
                        {   sb.Append(param.Value);
                            sb.Append(" ");
                        }
                    }
                }

                var query = GetDbSet().Where(sb.ToString());
                return ToDTO(query);      
            
            }
            return new List<D>();
            
        }

        public void ReloadContext()
        {
            Init();
        }

        public void RefrechDbContext()
        {
            GetDb(false);
        }
        ~AbstractGenericDao()
        {
            if (this.useDbContextCache == false)
            {
                Dispose();
            }
        }

        public IList<D> Search(IList<QueryParam> lstParam, int pagina = 1, int registroPorPagina = 15)
        {
            if (lstParam != null)
            {
                StringBuilder sb = new StringBuilder(" ");

                bool primeiro = true;
                foreach (var param in lstParam)
                {
                    if (primeiro)
                    {
                        primeiro = false;
                    }
                    else
                    {
                        sb.Append(" && ");
                    }

                    if (!string.IsNullOrWhiteSpace(param.Key))
                    {
                        if (param.CanBeNull) {

                            sb.Append("( ");
                            sb.Append(param.Key);
                            sb.Append(" IS NULL ||");
                        }

                        sb.Append(param.Key);
                        sb.Append(" == ");

                        if (param.Value == null)
                        {
                            sb.Append("null ");
                        }
                        else
                        if (param.Value is string)
                        {
                            sb.Append("\"");
                            sb.Append(param.Value);
                            sb.Append("\" ");
                        }
                        else
                        {
                            sb.Append(param.Value);
                            sb.Append(" ");
                        }

                        if (param.CanBeNull)
                        {
                            sb.Append(" )");
                        }

                    }
                }

                var query = GetDbSet().Where(sb.ToString());
                return ToDTO(query);

            }
            return new List<D>();

        }
    }
}
