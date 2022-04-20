using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using Coad.Reflection;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Config.DataAttributes.Maps;
using GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config
{
    public static class AttributeConfigurator
    {
        public static void Apply<T, D, Id>(GenericService<T, D, Id> genericService, ServiceConfigAttribute attributeInstance)  where T : class
        {
            if (genericService != null && attributeInstance != null)
            {
                if(attributeInstance.Keys != null && attributeInstance.Keys.Length > 0){

                    genericService.SetKeys(attributeInstance.Keys);
                }

                if (!string.IsNullOrWhiteSpace(attributeInstance.profileName))
                {
                    genericService.SetProfileName(attributeInstance.profileName);
                }
            }
        }

        public static void Apply<T, D, Id>(AbstractGenericDao<T, D, Id> genericDao, ServiceConfigAttribute attributeInstance) where T : class
        {
            if (genericDao != null && attributeInstance != null)
            {
                if (attributeInstance.Keys != null && attributeInstance.Keys.Length > 0)
                {

                    genericDao.Keys = attributeInstance.Keys;
                }

                if (!string.IsNullOrWhiteSpace(attributeInstance.profileName))
                {
                    genericDao.SetProfileName(attributeInstance.profileName);
                }
            }
        }

        /// <summary>
        /// Obtem propriedades do Tipo GenericService mapeados com o atributo ServiceProperty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="D"></typeparam>
        /// <typeparam name="Id"></typeparam>
        /// <param name="genericService"></param>
        /// <param name="name"></param>
        public static IQueryable<IServiceAssociator> GetPropertyByServicePropertyAttributeName(IServiceAssociator genericService, string name)
        {            
            var members = ReflectionProvider.GetProperties(genericService);
            //var attribute = ReflectionProvider.GetAttributes<ServicePropertyAttribute>(genericService);
            IList<IServiceAssociator> lstPropAchadas = new List<IServiceAssociator>();

            members = members.Where(x => Attribute.IsDefined(x, typeof(ServicePropertyAttribute)));
            
            foreach(var mem in members)
            {
                var custons = mem.GetCustomAttributes<ServicePropertyAttribute>(true);
                if(custons != null && custons.Where(x => x.Name == name).Count() > 0)
                {
                    var attr = mem.GetCustomAttributes<ServicePropertyAttribute>().Where(x => x.Name == name).FirstOrDefault();
                    var service = ReflectionProvider.GetMemberValue<IServiceAssociator>(genericService, mem);

                    if (service != null) {
                        var serviceAssoConfig = new ServiceAssociationConfig()
                        {
                            Keys = attr.Keys,
                            PropertyName = attr.PropertyName,
                            FindById = attr.FindById
                        };

                        service.AddServiceAssociationConfig(name, serviceAssoConfig);
                        lstPropAchadas.Add(service);
                    }
                }
                
            }

            return lstPropAchadas.AsQueryable();
        }

        public static void Apply<T, D, Id>(AbstractGenericDao<T, D, Id> genericDAO, DAOConfigAttribute attributeInstance) where T : class
        {
            if (genericDAO != null && attributeInstance != null)
            {
                if (!string.IsNullOrWhiteSpace(attributeInstance.ProfileName))
                {
                    genericDAO.profileName = attributeInstance.ProfileName;
                }
            }
        }
    }
}
