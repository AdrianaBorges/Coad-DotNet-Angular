using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Metadata.Edm;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GenericCrud.Service
{
    public class ClassMetadata
    {
        public ReadOnlyMetadataCollection<EntitySetBase> ListarEntityModels(DbContext db)
        {
            ReadOnlyMetadataCollection<EntitySetBase> resp = null;

            if (db != null)
            {
                using (db)
                {
                    var objectContext = ((IObjectContextAdapter)db).ObjectContext;
                    var container = objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName, DataSpace.CSpace);

                    if (container != null)
                    {
                        resp = container.BaseEntitySets;
                    }

                }
            }

            return resp;
			
        }

        /// <summary>
        /// ALT: 31/08/2016 - Este método retorna os campos simples de um DTO. Exemplo: com tipos string, int, etc...
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public SelectList RetornaCampos(Object dto, string nomeModel)
        {
            SelectList retorno = null;
            List<SelectListItem> campos = new List<SelectListItem>();

            string tipos = "Str,Dat,Int,Boo,Cha,Byt,Dou,Dec,Tim,Nul";

            foreach (var prop in dto.GetType().GetProperties())
            {
                if (tipos.IndexOf(prop.PropertyType.Name.Substring(0, 3)) != -1)
                {
                    DisplayNameAttribute titulo;
                    titulo = (DisplayNameAttribute)dto.GetType().GetProperty(prop.Name).GetCustomAttributes(typeof(DisplayNameAttribute), true).SingleOrDefault();
                    string tituloDocampo = (titulo == null) ? prop.Name : titulo.DisplayName;
                    
                    campos.AddRange(new[] { new SelectListItem() { Text = tituloDocampo, Value = nomeModel + "." + prop.Name } });

                    retorno = new SelectList(campos, "Value", "Text");
                }
            }

            return retorno;
        }

        /// <summary>
        /// ALT: 31/05/2017 - Este método compara se houve alteração nos dados do DTO da tela e o DTO já salvo.
        /// </summary>
        /// <param name="DTOa"></param>
        /// <param name="DTOb"></param>
        /// <returns></returns>
        public bool CompararAtributosDeObjetos(Object DTOa, Object DTOb)
        {
            bool retorno = false;

            if (DTOa != null && DTOb != null)
            {
                string tipos = "Str,Dat,Int,Boo,Cha,Byt,Dou,Dec,Tim,Nul";

                foreach (var prop in DTOa.GetType().GetProperties())
                {
                    if (tipos.IndexOf(prop.PropertyType.Name.Substring(0, 3)) != -1)
                    {
                        var cpA = DTOa.GetType().GetProperty(prop.Name);
                        if (cpA != null)
                        {
                            var dadoA = cpA.GetValue(DTOa, null);

                            // outro objeto
                            var cpB = DTOb.GetType().GetProperty(prop.Name);
                            if (cpB != null)
                            {
                                var dadoB = cpB.GetValue(DTOb, null);
                                if (dadoB == null || dadoA == null)
                                    retorno = (dadoB == null && dadoA == null);
                                else
                                    retorno = dadoA.Equals(dadoB);
                                if (!retorno)
                                    break;
                            }
                        }
                    }
                }
            }

            return retorno;
        }

        /// <summary>
        /// ALT: 31/05/2017 - Este método compara se houve alteração nos dados do DTO da tela e o DTO já salvo.
        /// </summary>
        /// <param name="lstDTOa"></param>
        /// <param name="lstDTOb"></param>
        /// <returns></returns>
        public bool CompararAtributosDeLstObjetos(List<Object> lstDTOa, List<Object> lstDTOb)
        {
            bool retorno = false;

            if (lstDTOa != null && lstDTOb != null)
            {
                if (lstDTOa.Count() == 0 && lstDTOb.Count() == 0)
                {
                    retorno = true;
                }
                else
                {
                    var a = 0;
                    var b = 0;

                    foreach (var DTOa in lstDTOa)
                    {
                        a++;
                        b = 0;
                        foreach (var DTOb in lstDTOb)
                        {
                            b++;
                            if (a == b)
                            {
                                retorno = CompararAtributosDeObjetos(DTOa, DTOb);
                                break;
                            }
                        }
                        if (!retorno)
                            break;
                    }
                }
            }

            return retorno;
        }
    }

    //
    /// <summary>
    /// ALT: 05/06/2017 - Clone de objetos
    /// </summary>
    public static class CloneHelper
    {
        public static T Clone<T>(T objToClone) where T : class
        {
            return CloneHelper<T>.Clone(objToClone);
        }
    }

    public static class CloneHelper<T> where T : class
    {
        private static readonly Lazy<PropertyHelper.Accessor<T>[]> _LazyCloneableAccessors =
            new Lazy<PropertyHelper.Accessor<T>[]>(CloneableProperties, isThreadSafe: true);

        private static readonly Func<object, object> MemberwiseCloneFunc;

        static CloneHelper()
        {
            var flags = BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic;
            MemberwiseCloneFunc = (Func<object, object>)Delegate.CreateDelegate(
                typeof(Func<object, object>),
                typeof(T).GetMethod("MemberwiseClone", flags));
        }

        [ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
        private static PropertyHelper.Accessor<T>[] CloneableProperties()
        {
            var bindingFlags = BindingFlags.Instance
                               | BindingFlags.FlattenHierarchy
                               | BindingFlags.Public
                               | BindingFlags.NonPublic;

            var result = typeof(T)
                .GetProperties(bindingFlags)
                .Where(p => p.PropertyType != typeof(string) && !p.PropertyType.IsValueType)
                .Where(p => p.PropertyType.GetMethods(bindingFlags).Any(x => x.Name == "Clone"))
                .Select(PropertyHelper.CreateAccessor<T>)
                .Where(a => a != null)
                .ToArray();

            return result;
        }

        public static T Clone(T objToClone)
        {
            var clone = MemberwiseCloneFunc(objToClone) as T;

            // clonando todas as propriedades que possuem um método Clone
            foreach (var accessor in _LazyCloneableAccessors.Value)
            {
                var propToClone = accessor.GetValueObj(objToClone);
                var clonedProp = propToClone == null ? null : ((dynamic)propToClone).Clone() as object;
                accessor.SetValueObj(objToClone, clonedProp);
            }

            return clone;
        }

    }

    public static class PropertyHelper
    {
        public abstract class Accessor<T>
        {
            public abstract void SetValueObj(T obj, object value);
            public abstract object GetValueObj(T obj);
        }

        public class Accessor<TTarget, TValue> : Accessor<TTarget>
        {
            private readonly PropertyInfo _property;
            public Accessor(PropertyInfo property)
            {
                _property = property;

                if (property.GetSetMethod(true) != null)
                    this.Setter = (Action<TTarget, TValue>)
                        Delegate.CreateDelegate(typeof(Action<TTarget, TValue>), property.GetSetMethod(true));

                if (property.GetGetMethod(true) != null)
                    this.Getter = (Func<TTarget, TValue>)
                    Delegate.CreateDelegate(typeof(Func<TTarget, TValue>), property.GetGetMethod(true));
            }

            public Action<TTarget, TValue> Setter { get; private set; }
            public Func<TTarget, TValue> Getter { get; private set; }

            public override void SetValueObj(TTarget obj, object value) { Setter(obj, (TValue)value); }
            public override object GetValueObj(TTarget obj) { return Getter(obj); }
            public override string ToString() { return _property.ToString(); }
        }

        public static Accessor<T> CreateAccessor<T>(PropertyInfo property)
        {
            var getMethod = property.GetGetMethod();
            if (getMethod == null || getMethod.GetParameters().Length != 0)
                return null;
            var accessor = (Accessor<T>)Activator.CreateInstance(
                typeof(Accessor<,>).MakeGenericType(typeof(T),
                    property.PropertyType), property);
            return accessor;
        }

        public static Accessor<TIn, TOut> CreateAccessor<TIn, TOut>(PropertyInfo property)
        {
            return (Accessor<TIn, TOut>)CreateAccessor<TIn>(property);
        }
    }
}
