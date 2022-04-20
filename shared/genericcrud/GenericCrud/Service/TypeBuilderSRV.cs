using GenericCrud.Models.SqlDinamico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenericCrud.Service
{
    public class TypeBuilderSRV
    {
        public Type CriarTipo(IEnumerable<ColunaSqlDinamicoDTO> lstCampos)
        {
            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = "dynamicClasses";

            AssemblyBuilder assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder module = assemblyBuilder.DefineDynamicModule("dynamicClasses");

            TypeBuilder typeBuilder = module.DefineType("ResultadoQueryDinamica", TypeAttributes.Public | TypeAttributes.Class);

            GerarCamposSettersEGetters(lstCampos, typeBuilder);

            Type tipoClasse = typeBuilder.CreateType();
            var obj = Activator.CreateInstance(tipoClasse);
            return tipoClasse;
        }

        private void GerarCamposSettersEGetters(IEnumerable<ColunaSqlDinamicoDTO> lstCampo, TypeBuilder typeBuilder)
        {
            if (lstCampo != null)
            {
                foreach (var propertyInfo in lstCampo)
                {
                    var campo = (!string.IsNullOrEmpty(propertyInfo.Alias) ? propertyInfo.Alias : propertyInfo.Nome);
                    var tipoDado = propertyInfo.TipoDeDados;

                    //var propertyName = "RG_DESCRICAO";
                    FieldBuilder field = typeBuilder.DefineField("_" + campo, tipoDado, FieldAttributes.Private);

                    PropertyBuilder property = typeBuilder.DefineProperty(campo, PropertyAttributes.HasDefault, tipoDado, null);

                    MethodAttributes GetSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

                    // get
                    MethodBuilder getMethodBuilder = typeBuilder.DefineMethod("get_" + campo, GetSetAttr, tipoDado, Type.EmptyTypes);

                    ILGenerator curreGetIl = getMethodBuilder.GetILGenerator();
                    curreGetIl.Emit(OpCodes.Ldarg_0);
                    curreGetIl.Emit(OpCodes.Ldfld, field);
                    curreGetIl.Emit(OpCodes.Ret);

                    // set
                    MethodBuilder setMethodBuilder = typeBuilder.DefineMethod("set_" + campo, GetSetAttr, null, new Type[] { tipoDado });

                    ILGenerator curreSetIl = setMethodBuilder.GetILGenerator();
                    curreSetIl.Emit(OpCodes.Ldarg_0);
                    curreSetIl.Emit(OpCodes.Ldarg_1);
                    curreSetIl.Emit(OpCodes.Stfld, field);
                    curreSetIl.Emit(OpCodes.Ret);

                    property.SetGetMethod(getMethodBuilder);
                    property.SetSetMethod(setMethodBuilder);
                }
            }
        }
    }
}
