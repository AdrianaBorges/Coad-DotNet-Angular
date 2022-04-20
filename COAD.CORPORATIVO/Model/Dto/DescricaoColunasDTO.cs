using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(DESCREVER_COLUNAS_Result))]
    public class DescricaoColunasDTO
    {
        public string TABLE_QUALIFIER { get; set; }
        public string TABLE_OWNER { get; set; }
        public string TABLE_NAME { get; set; }
        public string COLUMN_NAME { get; set; }
        public Nullable<short> DATA_TYPE { get; set; }
        public string TYPE_NAME { get; set; }
        public Nullable<int> PRECISION { get; set; }
        public Nullable<int> LENGTH { get; set; }
        public Nullable<short> SCALE { get; set; }
        public Nullable<short> RADIX { get; set; }
        public Nullable<short> NULLABLE { get; set; }
        public string REMARKS { get; set; }
        public string COLUMN_DEF { get; set; }
        public Nullable<short> SQL_DATA_TYPE { get; set; }  
        public Nullable<short> SQL_DATETIME_SUB { get; set; }
        public Nullable<int> CHAR_OCTET_LENGTH { get; set; }
        public Nullable<int> ORDINAL_POSITION { get; set; }
        public string IS_NULLABLE { get; set; }
        public Nullable<byte> SS_DATA_TYPE { get; set; }
    }
}
