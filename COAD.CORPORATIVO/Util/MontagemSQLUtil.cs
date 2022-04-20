using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Util
{
    public static class MontagemSQLUtil
    {
        public static string MapearTipoDadoInterfaceUsuario(string nomeTipoDado)
        {

            var tipoDado = MapearTipoDado(nomeTipoDado, false);

            if (typeof(long).Equals(tipoDado) ||
                typeof(int).Equals(tipoDado) ||
                typeof(short).Equals(tipoDado) ||
                typeof(byte).Equals(tipoDado) ||
                typeof(byte[]).Equals(tipoDado))
            {
                return "int";
            }

            if (typeof(double).Equals(tipoDado) ||
                typeof(decimal).Equals(tipoDado) ||
                typeof(float).Equals(tipoDado) ||
                typeof(Single).Equals(tipoDado))
            {
                return "decimal";
            }

            if (typeof(bool).Equals(tipoDado))
                return "bool";

            if (typeof(DateTime).Equals(tipoDado))
                return "date";

            return "text";
        }
        public static Type MapearTipoDado(string nomeTipoDado, bool IsNullable)
        {
            if (!string.IsNullOrEmpty(nomeTipoDado))
            {
                if (nomeTipoDado.Contains("bigint"))
                    return (IsNullable) ? typeof(Nullable<long>) : typeof(long);
                if (nomeTipoDado.Contains("numeric"))
                    return (IsNullable) ? typeof(Nullable<decimal>) : typeof(decimal);
                if (nomeTipoDado.Contains("bit"))
                    return (IsNullable) ? typeof(Nullable<bool>) : typeof(bool);
                if (nomeTipoDado.Contains("smallint"))
                    return (IsNullable) ? typeof(Nullable<short>) : typeof(short);
                if (nomeTipoDado.Contains("decimal"))
                    return (IsNullable) ? typeof(Nullable<decimal>) : typeof(decimal);
                if (nomeTipoDado.Contains("int"))
                    return (IsNullable) ? typeof(Nullable<int>) : typeof(int);
                if (nomeTipoDado.Contains("tinyint"))
                    return (IsNullable) ? typeof(Nullable<byte>) : typeof(byte);
                if (nomeTipoDado.Contains("float"))
                    return (IsNullable) ? typeof(Nullable<float>) : typeof(float);
                if (nomeTipoDado.Contains("real"))
                    return (IsNullable) ? typeof(Nullable<float>) : typeof(float);
                if (nomeTipoDado.Contains("date"))
                    return (IsNullable) ? typeof(Nullable<DateTime>) : typeof(DateTime);
                if (nomeTipoDado.Contains("datetimeoffset"))
                    return (IsNullable) ? typeof(Nullable<DateTimeOffset>) : typeof(DateTimeOffset);
                if (nomeTipoDado.Contains("datetime") || nomeTipoDado.Contains("datetime2"))
                    return (IsNullable) ? typeof(Nullable<DateTime>) : typeof(DateTime);
                if (nomeTipoDado.Contains("char") || nomeTipoDado.Contains("varchar") || nomeTipoDado.Contains("text"))
                    return typeof(string);
                if (nomeTipoDado.Contains("binary"))
                    return typeof(byte[]);
            }

            return null;
        }
    }
}
