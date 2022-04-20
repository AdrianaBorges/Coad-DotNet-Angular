using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;

namespace COAD.COADGED.DAO
{
    public class CadernoDAO : AbstractGenericDao<CADERNO, CadernoDTO, int>
    {
        public CadernoDAO()
            : base()
        {
            SetProfileName( "GED" );           
        }

        public IList<CadernoDTO> BuscarCadernosCliente(int idCliente)
        {
            IQueryable<CADERNO> query = GetDbSet();
            var lista = query.Where(x => x.CLI_ID == idCliente).ToList();
            return ToDTO(lista);
        }

        public CadernoDTO BuscarCadernoRepetido(int idCliente, string nomeCad)
        {
            IQueryable<CADERNO> query = GetDbSet();
            var lista = query.Where(x => x.CLI_ID == idCliente && x.CAD_NOME.ToLower().Equals(nomeCad.ToLower())).FirstOrDefault();
            return ToDTO(lista);
        }
    }
}
