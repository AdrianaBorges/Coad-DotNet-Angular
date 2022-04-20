using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Excel.Metadatas
{
    public class ExcelColumnAttribute : Attribute
    {
        public string Name { get; set; }

        /// <summary>
        /// Nome do Campo de Onde essa célula pegará o valor para inserir um comentário
        /// </summary>
        public string CommentFrom { get; set; }
    }
}
