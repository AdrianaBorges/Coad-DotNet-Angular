using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using COAD.CORPORATIVO.Exceptions;
using System.Web;
using System.IO;
using GenericCrud.Util;
using GenericCrud.Excel;
using COAD.CORPORATIVO.Util;
using Coad.GenericCrud.Extensions;
using GenericCrud.Metadatas;
using COAD.CORPORATIVO.Service.Custons;
using GenericCrud.Service.Formatting;
using GenericCrud.Models.HistoryRegister;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Comparators;
using GenericCrud.Models.MessageFormatter;
using GenericCrud.Models.Comparators;
using COAD.SEGURANCA.Service.Custons;
using COAD.SEGURANCAO.Util;
using COAD.SEGURANCA.Exceptions;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("IMS_ID")]
    public class ImportacaoStatusSRV : GenericService<IMPORTACAO_STATUS, ImportacaoStatusDTO, int>
    {
        private ImportacaoStatusDAO _dao = new ImportacaoStatusDAO();
        
        private MessageFormatterService formatterService { get; set; }
        
        public ImportacaoStatusSRV()
        {
            this.formatterService = FormatterServiceLocalFactory.CriarMessageFormatterServiceCoorporativo();
            this.Comparator = new GenericComparator<ImportacaoStatusDTO>("IPS_NOME", 
                "IPS_CPF_CNPJ", 
                "IPS_TELEFONE", 
                "IPS_FAX", 
                "IPS_CELULAR", 
                "IPS_EMAIL");

            this.Dao = _dao;
        }
    }
}
