

using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using System.Web;
using System.IO;
using GenericCrud.Service;
using GenericCrud.Validations;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Model.Custons;

namespace COAD.CORPORATIVO.Service
{ 
	[ServiceConfig("NEV_ID")]
	public class NotaFiscalEventoSRV : GenericService<NOTA_FISCAL_EVENTO, NotaFiscalEventoDTO, Int32>
	{

        public NotaFiscalEventoDAO _dao { get; set; }

        public NotaFiscalEventoSRV(NotaFiscalEventoDAO _dao)
        {
			this._dao = _dao;
			this.Dao = _dao;
        }

        public Pagina<NotaFiscalEventoDTO> ListarEventosNotaFiscal(int? nfID, int pagina = 1, int registrosPorPagina = 4)
        {
            return _dao.ListarEventosNotaFiscal(nfID, pagina, registrosPorPagina);
        }

        public FileInfoDTO RetornarArquivoEventoNota(int? nfEveID)
        {
            var nota = FindById(nfEveID);

            if (nota != null && nota.Arquivo != null && !string.IsNullOrWhiteSpace(nota.ArquivoNome))
            {
                FileInfoDTO info = new FileInfoDTO();
                info.Bytes = nota.Arquivo;
                info.Path = nota.ArquivoNome;
                return info;
            }

            return null;
        }

    }
}
