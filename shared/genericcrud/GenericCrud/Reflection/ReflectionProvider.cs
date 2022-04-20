using Coad.GenericCrud.Config;
using Coad.GenericCrud.Service.Base;
using GenericCrud.Dao;
using GenericCrud.Service.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Coad.Reflection
{
    public class ReflectionProvider
    {
        private readonly object Target;
        private const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.FlattenHierarchy | BindingFlags.Public;
        private const BindingFlags publicFlags = BindingFlags.Instance | BindingFlags.Default | BindingFlags.FlattenHierarchy | BindingFlags.Public;
        public delegate void FetchCallback(object value, string name);
        
        public ReflectionProvider(object target)
        {
            this.Target = target;
               
        }

        public static T GetPropertyValue<T>(Object obj, string field) 
        {
            if (field != null)
            {
                Type type = obj.GetType();
                PropertyInfo propertyInfo = type.GetProperty(field, flags);
                if (propertyInfo != null)
                {
                    return (T)propertyInfo.GetValue(obj);
                }
                else
                {
                    throw new FieldAccessException("O campo " + field + " não pode ser encontrado");
                }
                
            }
            throw new FieldAccessException("O campo " + field + " não pode ser encontrado"); ;
        }

        public static void SetPropertyValue<T>(Object obj, string field, T value)
        {
           if (field != null)
            {
                Type type = obj.GetType();
                PropertyInfo propertyInfo = type.GetProperty(field, flags);
                
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(obj, value);
                }
                else
                {
                    throw new FieldAccessException("O campo " + field + " não pode ser encontrado");
                }
                
            }
        }

        public T GetPropertyValue<T>(string field)
        {
            return GetPropertyValue<T>(this.Target, field);
        }

        public static IEnumerable<PropertyInfo> GetProperties(Object obj)
        {
             Type type = obj.GetType();
             return type.GetProperties(flags);
        }

        public static T GetFieldValue<T>(Object obj, string field)
        {
            if (field != null)
            {
                Type type = obj.GetType();
                FieldInfo propertyInfo = type.GetField(field, flags);
                if (propertyInfo != null)
                {
                    return (T)propertyInfo.GetValue(obj);
                }
                else
                {
                    throw new FieldAccessException("O campo " + field + " não pode ser encontrado");
                }

            }
            throw new FieldAccessException("O campo " + field + " não pode ser encontrado"); ;
        }

        /// <summary>
        /// Retorna valor de campo baseado no path.
        /// Suporta paths Ex: obj.outroObjeto.nomeDoCampo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static T GetMemberValue<T>(Object obj, string field)
        {
            if (field != null)
            {
                var pathArray = field.Split('.');
                object currentObj = obj;
                
                foreach (var path in pathArray)
                {
                    if(currentObj != null)
                    {
                        Type type = currentObj.GetType();
                        MemberInfo[] lstInfo = type.GetMember(path, flags);
                        if (lstInfo != null && currentObj != null)
                        {
                            foreach (var pro in lstInfo)
                            {
                                if (pro is PropertyInfo)
                                {
                                    currentObj = ((PropertyInfo)pro).GetValue(currentObj);
                                }
                                else
                                if (pro is FieldInfo)
                                {
                                    currentObj = ((FieldInfo)pro).GetValue(currentObj);
                                }

                                if (pathArray.Length > 1 && currentObj == null)
                                    continue;
                            }

                        }
                        else
                        {
                            throw new FieldAccessException("O campo " + field + " não pode ser encontrado");
                        }
                    }

                }

                return (T) currentObj;
            }

            throw new FieldAccessException("O campo " + field + " não pode ser encontrado");
        }

        public static void SetMemberValue<T>(Object obj, string field, T value)
        {
            if (field != null)
            {
                Type type = obj.GetType();

                MemberInfo[] lstInfo = type.GetMember(field, flags);
                if (lstInfo != null)
                {
                    foreach (var pro in lstInfo)
                    {
                        if (pro is PropertyInfo)
                        {
                            ((PropertyInfo)pro).SetValue(obj, value);
                        }
                        else
                            if (pro is FieldInfo)
                            {
                                ((FieldInfo)pro).SetValue(obj, value);
                            }
                    }

                }
                else
                {
                    throw new FieldAccessException("O campo " + field + " não pode ser encontrado");
                }

            }
       }

        public static bool TrySetMemberValue<T>(Object obj, string field, object value)
        {
            if (field != null && obj != null)
            {

                MemberInfo[] lstInfo = obj.GetType().GetMember(field, flags);
                if (lstInfo != null)
                {
                    foreach (var pro in lstInfo)
                    {
                        Type memberType = pro.DeclaringType;
                        
                        if (pro is PropertyInfo)
                        {
                            ((PropertyInfo)pro).SetValue(obj, value);
                            return true;
                        }
                        else
                            if (pro is FieldInfo)
                            {
                                ((FieldInfo)pro).SetValue(obj, value);
                                return true;
                            }
                    }

                }

            }
            return false;
       }

        public static T GetMemberValue<T>(Object obj, MemberInfo member)
        {
            try
            {            
                if (obj != null)
                {             
                    if (member != null)
                    {
                        if (member is PropertyInfo)
                        {
                            var result = ((PropertyInfo) member).GetValue(obj);
                            return (T)result; 
                        }
                        else
                        if (member is FieldInfo)
                        {
                            var result = ((FieldInfo) member).GetValue(obj);
                            return (T)result; 
                        }
                    }
                    else
                    {
                        throw new FieldAccessException("A Informação do campo não pode ser encontrado");
                    }

                }
                throw new FieldAccessException("A Informação do campo não pode ser encontrado");
            }
            catch (FieldAccessException e)
            {
                throw e;
            }
            catch(Exception e)
            {
                throw new Exception(string.Format("Ocorreu um erro ao tentar recuperar os dados do campo {0} da classe {1}", member.Name, obj.GetType().FullName), e);
            }
        }

        public static IEnumerable<FieldInfo> GetFields(Object obj)
        {
            Type type = obj.GetType();
            return type.GetFields(flags);
        }

        public static IEnumerable<MemberInfo> GetMembers(Object obj, bool trazSomenteProspsEFields = true, bool onlyPublic = false)
        {
            Type type = obj.GetType();
            var flags = (onlyPublic) ? publicFlags : ReflectionProvider.flags;
            IEnumerable<MemberInfo> lstMember = type.GetMembers(flags);

            if (lstMember != null && trazSomenteProspsEFields)
            {
                lstMember = lstMember.Where( // Só pego propriedades e campos
                    x => MemberTypes.Property.Equals(x.MemberType) ||
                        MemberTypes.Field.Equals(x.MemberType));
            }

            return lstMember;
        }
        
        public static void FetchFields(object obj, FetchCallback callback)
        {
            IEnumerable<PropertyInfo> properties = GetProperties(obj);

            foreach (PropertyInfo property in properties)
            {
                callback(property.GetValue(obj), property.Name);
            }

        }

        public void FetchFields(FetchCallback callback)
        {
            IEnumerable<PropertyInfo> properties = GetProperties(this.Target);

            foreach (PropertyInfo property in properties)
            {
                callback(property.GetValue(this.Target), property.Name);
            }

        }

        public static TReturn CallMethod<TReturn>(object obj, string methodName, object[] parametros )
        {
            Type type = obj.GetType();
            var methodInf = type.GetMethod(methodName, flags);

            if (methodInf != null)
            {
                return (TReturn)methodInf.Invoke(obj, parametros);
            }
            else
            {
                return default(TReturn);
            }
        }

        public static object[] GetPropertiesValues(object obj, params string[] nameId)
        {
            object[] ids = new object[nameId.Count()];

            if (nameId != null && nameId.Count() > 0)
            {
                int index = 0;
                foreach (var idObj in nameId)
                {
                    ids[index] = ReflectionProvider.GetPropertyValue<object>(obj, idObj);
                    index++;
                }
            }
            else
            {
                ids = new object[] { ReflectionProvider.GetPropertyValue<object>(obj, "ID") };
            }


            return ids;
        }

        public static IList<QueryParam> GetPropertiesValuesAsQueryParams(object obj, params string[] propNames)
        {
            IList<QueryParam> lstParams = new List<QueryParam>();

            if (propNames != null && propNames.Count() > 0)
            {
                foreach (var idObj in propNames)
                {
                    var param = new QueryParam();
                    param.Key = idObj;
                    param.Value = ReflectionProvider.GetPropertyValue<object>(obj, idObj);

                    lstParams.Add(param);
                }
            }

            return lstParams;
        }


        public static TReturn GetMemberAttribute<TReturn>(MemberInfo member) where TReturn : Attribute
        {
            var attribute = member.GetCustomAttribute<TReturn>(true);            
            return attribute;
        }

        public static TReturn GetAttribute<TReturn>(Type type) where TReturn : Attribute
        {
            var attribute = type.GetCustomAttribute<TReturn>(true);
            return attribute;
        }

        public static TReturn GetAttribute<TReturn>(object obj) where TReturn : Attribute
        {
            return GetAttribute<TReturn>(obj.GetType());
        }

        public static IEnumerable<TReturn> GetAttributes<TReturn>(object obj) where TReturn : Attribute
        {
            return GetAttributes<TReturn>(obj.GetType());
        }

        public static IEnumerable<TReturn> GetAttributes<TReturn>(Type type) where TReturn : Attribute
        {
            var attribute = type.GetCustomAttributes<TReturn>(true);
            return attribute;
        }

        public static IEnumerable<MethodInfo> GetMethodByAttributes<TReturn>(object obj) where TReturn : Attribute
        {
            var type = obj.GetType();
            return GetMethodByAttributes<TReturn>(type);
        }

        public static IEnumerable<MethodInfo> GetMethodByAttributes<TAttribute>(Type type, string name) where TAttribute : Attribute
        {
            var attribute = type.GetMethods().Where(x => Attribute.IsDefined(x, typeof(TAttribute)));
            return attribute;
        }

        public static IEnumerable<MemberInfo> GetMemberByAttributes<TAttribute>(Object obj) where TAttribute : Attribute
        {
            return GetMemberByAttributes<TAttribute>(obj.GetType());
        }

        public static IEnumerable<MemberInfo> GetMemberByAttributes<TAttribute>(Type type) where TAttribute : Attribute
        {
            var attribute = type.GetMembers().Where(x =>  Attribute.IsDefined(x, typeof(TAttribute)));
            return attribute;
        }

        public static IEnumerable<PropertyInfo> GetPropertyByAttributes<TReturn>(object obj) where TReturn : Attribute
        {
            var type = obj.GetType();
            return GetPropertyByAttributes<TReturn>(type);
        }

        public static IEnumerable<PropertyInfo> GetPropertyByAttributes<TAttribute>(Type type) where TAttribute : Attribute
        {
            var attribute = type.GetProperties().Where(x => Attribute.IsDefined(x, typeof(TAttribute)));
            return attribute;
        }

        public static bool HasPropertiesValues(object obj, params string[] nameId)
        {
            bool resp = true;

            if (nameId != null && nameId.Count() > 0)
            {
                foreach (var idObj in nameId)
                {
                    object defaultValue = null;

                    var value = ReflectionProvider.GetPropertyValue<object>(obj, idObj);
                    if(value != null){
                        if (value is string)
                            defaultValue = default(string);
                        else
                            defaultValue = Activator.CreateInstance(value.GetType(), null);
                    }
                    resp = (resp && value != null && (value.Equals(defaultValue) == false));                  
                }
            }
            return resp;
        }


        public static PropertyInfo GetPropertyInfo<TSource>(string propertyName)
        {
            Type type = typeof(TSource);
            PropertyInfo propertyInfo = type.GetProperty(propertyName, flags);
            return propertyInfo;
        }

        public static MemberInfo GetMemberInfo<TSource>(string propertyName)
        {
            Type type = typeof(TSource);
            PropertyInfo propertyInfo = type.GetProperty(propertyName, flags);
            return propertyInfo;
        }

        public static IEnumerable<Type> FindInNamespaces(Assembly assembly, string namespaces)
        {
            return assembly.GetTypes().Where(x => String.Equals(x.Namespace, namespaces, StringComparison.Ordinal)).ToList();
        }

        public static IEnumerable<Type> FindInNamespaces<T>(Assembly assembly, string namespaces)
        {
            IEnumerable<Type> lstTypesResult = null;

            var lstTypes = FindInNamespaces(assembly, namespaces);

            if(lstTypes != null && lstTypes.Count() > 0)
            {
                lstTypesResult = lstTypes.Where(x => IsSubclassOf(x, typeof(T)) == true);
            }

            return lstTypesResult;
        }

        /// <summary>
        /// Dado um objeto pai que possui a chave primária, verifica se a lista de filhos possui essa chave.
        /// Se a chave não existir, ela é atribuída.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="keys"></param>
        public static void CheckAndAssignKeyFromParentToChildsList(object obj, string propertyName, params string[] keys)
        {
            var values = ReflectionProvider.GetPropertiesValuesAsQueryParams(obj, keys);
            var childLst = ReflectionProvider.GetMemberValue<IEnumerable>(obj, propertyName);

            if (values != null && childLst != null)
            {
                foreach (var child in childLst)
                {
                    foreach (var val in values)
                    {
                        var childValue = ReflectionProvider.GetMemberValue<object>(child, val.Key);
                        if (childValue != null || childValue != val.Value)
                        {
                            ReflectionProvider.SetMemberValue<object>(child, val.Key, val.Value);
                        }
                    }
                }
            }
        }

        public static bool TryGetMemberValue<T>(object obj, string field, out T value)
        {
            value = default(T);

            if (field != null)
            {
                var pathArray = field.Split('.');
                object currentObj = obj;

                foreach (var path in pathArray)
                {
                    Type type = currentObj.GetType();
                    MemberInfo[] lstInfo = type.GetMember(path, flags);
                    if (lstInfo != null && lstInfo.Count() > 0 && currentObj != null)
                    {
                        foreach (var pro in lstInfo)
                        {
                            if (pro is PropertyInfo)
                            {
                                currentObj = ((PropertyInfo)pro).GetValue(currentObj);
                            }
                            else
                                if (pro is FieldInfo)
                                {
                                    currentObj = ((FieldInfo)pro).GetValue(currentObj);
                                }
                        }

                    }
                    else
                    {
                        return false;
                    }

                }

                value = (T)currentObj;
                return true;
            }
            return false;
        }


        /// <summary>
        /// Dado um objeto pai que possui a chave primária, verifica se a lista de filhos possui essa chave.
        /// Se a chave não existir, ela é atribuída.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="obj"></param>
        /// <param name="childLst"></param>
        /// <param name="keys"></param>
        public static void CheckAndAssignKeyFromParentToChildsList<TEnum>(object obj, IEnumerable<TEnum> childLst, params string[] keys)
        {
            var values = ReflectionProvider.GetPropertiesValuesAsQueryParams(obj, keys);

            if (values != null && childLst != null)
            {
                foreach (var child in childLst)
                {
                    foreach (var val in values)
                    {
                        var childValue = ReflectionProvider.GetMemberValue<object>(child, val.Key);
                        if (childValue == null || childValue != val.Value)
                        {
                            ReflectionProvider.SetMemberValue<object>(child, val.Key, val.Value);
                        }
                    }
                }
            }
        }


        public static Type GetMemberType(Object obj, string field)
        {
            if (field != null)
            {
                Type type = obj.GetType();
                MemberInfo[] lstInfo = type.GetMember(field, flags);
                    
                if (lstInfo != null && lstInfo.Count() > 0)
                {
                    if (lstInfo[0] is PropertyInfo)
                    {
                        return ((PropertyInfo)lstInfo[0]).PropertyType;
                    }
                    else
                        if (lstInfo[0] is FieldInfo)
                        {
                            return ((FieldInfo)lstInfo[0]).FieldType;
                        }
                }
                
            }

            return null;
        }

        public static bool HasMember(Object obj, string field)
        {
            if (field != null)
            {
                Type type = obj.GetType();
                MemberInfo[] lstInfo = type.GetMember(field, flags);

                if (lstInfo != null && lstInfo.Count() > 0)
                {
                    return true;
                }

            }
            return false;
        }

      
        public static ICollection<Type> GetTypesAnnotedWith<TAnnotation>(Assembly assembly, string nameSpace)
        {
            if (assembly != null && !string.IsNullOrWhiteSpace(nameSpace))
            {
                    
                var types = assembly.GetTypes().Where(x =>
                    Attribute.IsDefined(x, typeof(TAnnotation)) &&
                    x.Namespace == nameSpace
                );
                return types.ToList();
                    
            }
            return null;
        }
        private static bool IsSubclassOf(Type type, Type subClassType)
        {
            if (type != null)
            {
                if (type.BaseType != null)
                {
                    if (type.GetInterfaces().Contains(subClassType))
                    {
                        return true;
                    }
                    else
                    {
                        return IsSubclassOf(type.BaseType, subClassType);
                    }
                }
                return false;
            }
            return false;
        }
        public static Type GetServicesBy(Assembly assembly, Type entityType)
        {
            if(assembly != null && entityType != null)
            {
                var type = assembly.GetTypes().Where(x =>
                        x.GetInterfaces().Contains(typeof(IBaseService)) &&
                        x.BaseType.GenericTypeArguments.Contains(entityType)
                    ).FirstOrDefault();

                return type;
            }
            return null;
        }

    }
}