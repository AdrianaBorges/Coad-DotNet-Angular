using AutoMapper;
using Coad.GenericCrud.Exceptions;
using Coad.Reflection;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config
{
    public class AnnotationConfigurationStore
    {
        public ConfigurationStore store { get; set; }
        public void AddType<T>()
        {
            Type type = typeof(T);
            AddType(type);
        }

        public void AddTypes(IEnumerable<Type> lstClassType)
        {
            if (lstClassType != null)
            {
                foreach (var classType in lstClassType)
                {
                    AddType(classType);
                }
            }
        }


        public void AddType(Type classType)
        {
          var lstAttributeMapping = ReflectionProvider.GetAttributes<MappingAttribute>(classType);

                foreach (var attr in lstAttributeMapping)
                {
                    var sourceType = attr.Source;
                    var destinyType = classType;

                    try
                    {   
                        if (sourceType != null && destinyType != null)
                        {
                            IMappingExpression config = store.CreateMap(sourceType, destinyType);
                            IMappingExpression reverseConfig = null;
                            if (attr.ReverseMapping)
                            {
                                reverseConfig = store.CreateMap(destinyType, sourceType);
                            }

                            var refName = attr.confRef;
                            ConfigIgnoreAttributes(destinyType, refName, config, reverseConfig, sourceType);

                        }
                    }
                    catch (Exception e)
                    {
                        var strErro = "Erro ao configurar Mapeamento: Mapeamento Atual {0} -> {1}:";
                        strErro = string.Format(strErro, sourceType.Name, classType.Name);
                        throw new MappingException(strErro, e);
                    }
                }           
        }

        private void ConfigIgnoreAttributes(Type classType, string refName,
            IMappingExpression config,
            IMappingExpression reverseConfig,
            Type sourceType)
        {
            string trackProperty = "Não definido.";
            try
            {
                var properties = classType.GetProperties().Where(x => Attribute.IsDefined(x, typeof(IgnoreMemberMappingAttribute)));
                
                foreach (var prop in properties)
                {
                    var lstMappingAttribute = (IgnoreMemberMappingAttribute[])prop.GetCustomAttributes(typeof(IgnoreMemberMappingAttribute), true);
                    var ignoreAttribute = lstMappingAttribute.Where(x => x.MappingRef == refName).FirstOrDefault();

                    if (ignoreAttribute != null)
                    {
                        if (prop == null)
                        {
                            throw new MappingException("Erro ao ignorar uma propriedade na classe" + classType.Name);
                        }
                        var propertyName = prop.Name;
                        trackProperty = propertyName;


                        switch (ignoreAttribute.Direction)
                        {
                            case MappingDirection.SourceToDestiny:
                                {

                                    config.ForMember(propertyName, x => x.Ignore());
                                    break;
                                }
                            case MappingDirection.DestinyToSource:
                                {

                                    if (reverseConfig != null)
                                    {
                                        reverseConfig.ForMember(propertyName, x => x.Ignore());
                                    }

                                    break;
                                }
                            case MappingDirection.Both:
                                {

                                    config.ForMember(propertyName, x => x.Ignore());
                                    if (reverseConfig != null)
                                    {
                                        reverseConfig.ForMember(propertyName, x => x.Ignore());
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var strErro = "Erro ao configurar Mapeamento: Mapeamento Mapeado {0} -> {1}, Propriedade: {2}";
                strErro = string.Format(strErro, sourceType.Name, classType.Name, trackProperty);
                throw new MappingException(strErro, e);
            }
        }

    }
}
