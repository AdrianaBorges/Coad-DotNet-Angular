using Coad.Reflection;
using GenericCrud.Validations.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenericCrud.Validations
{
    public class RequiredIfBase 
    {
        public bool PrecisaValidar(object value, 
            string PropertyName, 
            OperadorLogicoEnum? Operador, 
            Object[] DesiredValues,   
            ValidationContext context)
        {
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = ReflectionProvider.GetMemberValue<Object>(instance, PropertyName);

            if (proprtyvalue == null || DesiredValues == null || DesiredValues.Count() <= 0)
            {
                return true;
            }
            bool resultadoFinal = true;
            foreach (var desiredValue in DesiredValues)
            {
                bool resultado = false;
                if (desiredValue == null && proprtyvalue == null)
                    resultado = true;
                else
                {
                    resultado = (proprtyvalue != null && desiredValue != null && proprtyvalue.ToString() == desiredValue.ToString());

                    if (Operador != null)
                    {
                        switch (Operador)
                        {
                            case OperadorLogicoEnum.IQUAL:
                                resultado = (proprtyvalue.ToString() == desiredValue.ToString());
                                break;
                            case OperadorLogicoEnum.DIFERENTE:
                                resultado = (proprtyvalue.ToString() != desiredValue.ToString());
                                break;
                            case OperadorLogicoEnum.MAIOR:
                                resultado = ((int)proprtyvalue > (int)desiredValue);
                                break;
                            case OperadorLogicoEnum.MENOR:
                                resultado = ((int)proprtyvalue < (int)desiredValue);
                                break;
                            case OperadorLogicoEnum.MAIOR_OU_IGUAL:
                                resultado = ((int)proprtyvalue >= (int)desiredValue);
                                break;
                            case OperadorLogicoEnum.MENOR_OU_IGUAL:
                                resultado = ((int)proprtyvalue <= (int)desiredValue);
                                break;
                        }
                    }
                }
                resultadoFinal = resultadoFinal && resultado;
            }
            return resultadoFinal;        
        }

      
    }
        
}
