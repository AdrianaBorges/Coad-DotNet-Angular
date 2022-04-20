using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base.Pagination;

namespace COAD.CORPORATIVO.DAO
{
    public class TelefonesDeClienteDAO : DAOAdapter<ASSINATURA_TELEFONE,AssinaturaTelefoneDTO>
    {
public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public TelefonesDeClienteDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }

        public Pagina<AssinaturaTelefoneDTO> BuscarTelefonesDeCliente(string assinatura, int pagina = 1, int registroPorPagina = 7) 
        {
            var listaDeTelefones = (from x in db.ASSINATURA_TELEFONE where x.ASN_NUM_ASSINATURA == assinatura select x);

            return ToDTOPage(listaDeTelefones, pagina, registroPorPagina);

        }   
    }
}
