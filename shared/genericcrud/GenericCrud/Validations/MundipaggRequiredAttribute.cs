using Coad.Reflection;
using GenericCrud.Validations.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Validations
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class MundipaggRequiredAttribute : RequiredAttribute
    {
        public MundipaggRequiredAttribute(MundipaggValidatorEnum mundipaggEnum, String propertyName)
        {
            MundipaggEnum = mundipaggEnum;
            PropertyName = propertyName;
        }

        public MundipaggValidatorEnum MundipaggEnum { get; }
        public string PropertyName { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return Validar(value, validationContext);
        }

        private ValidationResult Validar(object value, ValidationContext validationContext)
        {
            Object instance = validationContext.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = ReflectionProvider.GetMemberValue<Object>(instance, PropertyName);

            string propString = (string)proprtyvalue;

            ValidationResult result = ValidationResult.Success;
            switch (MundipaggEnum)
            {
                case MundipaggValidatorEnum.ENDERECO:
                    result = ValidarEndereco(propString);
                    break;
            }

            return result;
        }

        private ValidationResult ValidarEndereco(string proprtyvalue)
        {
            if(proprtyvalue.Split(',').Count() != 2)
            {
                return new ValidationResult("Este campo deve possuir o seguinte formato: 'numero, rua, bairro' ");
            }
            return ValidationResult.Success;
        }
    }
}
