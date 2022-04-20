using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Model.Dto.Custons;


namespace COAD.CORPORATIVO.DAO
{
    public class CarteiraClienteDAO : AbstractGenericDao<CARTEIRA_CLIENTE, CarteiraClienteDTO, object>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public CarteiraClienteDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        private IQueryable<CARTEIRA_CLIENTE> TemplateCarteiraCliente(string CAR_ID, int CLI_ID)
        {
            IQueryable<CARTEIRA_CLIENTE> query = GetDbSet().Where(x => x.CAR_ID == CAR_ID && x.CLI_ID == CLI_ID);
            return query;
        }

        public bool HasCarteiraCliente(string CAR_ID, int CLI_ID)
        {
            var query = TemplateCarteiraCliente(CAR_ID, CLI_ID);
            var resp = (query.Count() > 0);
            return resp;            
        }

        public IList<CarteiraClienteDTO> FindByCarId(string CAR_ID)
        {
            var query = GetDbSet().Where(x => x.CAR_ID == CAR_ID);
            return ToDTO(query);
        }

        public IList<CarteiraClienteDTO> FindByClienteERegiao(int? CLI_ID, int? RG_ID)
        {
            var query = GetDbSet().Where(x => x.CLI_ID == CLI_ID && x.CARTEIRA.RG_ID == RG_ID);
            return ToDTO(query);
        }

        public IList<CarteiraClienteDTO> FindByCliente(int? CLI_ID)
        {
            var query = GetDbSet().Where(x => x.CLI_ID == CLI_ID);
            return ToDTO(query);
        }


        public CarteiraClienteDTO RetornarCarteiraClienteDeProspect(int? cliId, string carIdParaExcluir = null)
        {
            if (string.IsNullOrWhiteSpace(carIdParaExcluir))
                carIdParaExcluir = null;

            var cartCli = (from carCli in db.CARTEIRA_CLIENTE 
                           where 
                           carCli.CLI_ID == cliId && 
                           (carIdParaExcluir == null || carCli.CAR_ID != carIdParaExcluir)
                               && carCli.CCL_ORIGEM_PROSPECT
                           select carCli).FirstOrDefault();

            return ToDTO(cartCli);
        }

    }
}
