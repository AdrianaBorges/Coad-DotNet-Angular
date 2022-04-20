using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Service.Base;

namespace COAD.CORPORATIVO.Service
{
    public class TelefonesDeClienteSRV : ServiceAdapter<ASSINATURA_TELEFONE, AssinaturaTelefoneDTO>
    {
        private TelefonesDeClienteDAO _dao = new TelefonesDeClienteDAO();

        public TelefonesDeClienteSRV()
        {
        }
        public Pagina<AssinaturaTelefoneDTO> BuscarTelefonesDeClientePorAssinatura(string assinatura, int pagina = 1, int registroPorPagina = 7)
        {
            return _dao.BuscarTelefonesDeCliente(assinatura, pagina, registroPorPagina);
        }
    }
}
