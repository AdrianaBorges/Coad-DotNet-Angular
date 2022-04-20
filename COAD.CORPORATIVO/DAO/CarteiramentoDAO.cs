using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Extensions;
using COAD.CORPORATIVO.MapperConverters;
using COAD.SEGURANCA.DAO;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Comparators;
using COAD.CORPORATIVO.Model.Dto.Custons;

namespace COAD.CORPORATIVO.DAO
{
    public class CarteiramentoDAO : AbstractGenericDao<CARTEIRA, CarteiraDTO, string>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public CarteiramentoDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public Pagina<CarteiraDTO> Carteiramentos(string siglaRegiao = null, string representante = null, 
            int areaId = 1, 
            int pagina = 1, 
            int registrosPorPagina = 7, 
            int? RG_ID = null,
            int? REP_ID_TO_IGNORE = null,
            int? UEN_ID = null)
        {
            var db = GetDb<COADCORPEntities>(false);
           
            IQueryable<CARTEIRA> query = GetDbSet().Where(obj => obj.AREA_ID == areaId && obj.DATA_EXCLUSAO == null);

            if(!String.IsNullOrWhiteSpace(siglaRegiao))
            {
                query = query.Where(obj => obj.REGIAO_UF == siglaRegiao);
            }

            if (UEN_ID != null)
            {
                query = query.Where(x => x.UEN_ID == UEN_ID);
            }


            if (RG_ID != null)
            {
                query = query.Where(x => x.RG_ID == RG_ID);
            }


            if (!String.IsNullOrWhiteSpace(representante))
            {
                var query2 = GetDb<COADCORPEntities>(false).CARTEIRA_REPRESENTANTE.AsQueryable();
                query2 = query2.Where(obj => obj.REPRESENTANTE.REP_NOME.Contains(representante));
                            
                query = query.Where(op => query2.Select(o => o.CARTEIRA.CAR_ID).Contains(op.CAR_ID));
            }

            if(REP_ID_TO_IGNORE != null){

                var query3 = GetDb<COADCORPEntities>(false).CARTEIRA_REPRESENTANTE.AsQueryable();
                query3 = query3.Where(x => x.REP_ID == REP_ID_TO_IGNORE);
                query = query.Where(x => !query3.Select(s => s.CARTEIRA.CAR_ID).Contains(x.CAR_ID));
            }

            var queryProject = query.Distinct();
            Pagina<CarteiraDTO> page = ToDTOPage(queryProject, pagina, registrosPorPagina); 
            return page;
        }



        /// <summary>
        /// Pega o primeiro representante ativo de uma carteira
        /// </summary>
        /// <param name="carteira"></param>
        /// <returns></returns>
        public RepresentanteDTO GetCarteiraAtiva(CarteiraDTO carteira)
        {
            if (carteira != null && carteira.CARTEIRA_REPRESENTANTE != null)
            {
                return carteira.CARTEIRA_REPRESENTANTE.Where(p => p.REPRESENTANTE.REP_ATIVO == 1).Select(op => op.REPRESENTANTE).FirstOrDefault(); 
            }

            return null;
        }

        /// <summary>
        /// Retorna a carteira com a lista de carteria_representantes
        /// </summary>
        /// <param name="CAR_ID">Código da carteira</param>
        /// <returns></returns>
        public CarteiraDTO FindByIdFullLoaded(string CAR_ID)
        {
            CARTEIRA carteira = FindById(CAR_ID);
            CarteiraDTO dto = ToDTO(carteira);
            dto.CARTEIRA_REPRESENTANTE = Convert<ICollection<CARTEIRA_REPRESENTANTE>, List<CarteiraRepresentanteDTO>>(carteira.CARTEIRA_REPRESENTANTE);

            return dto;
        }

        public ICollection<CarteiraDTO> GetCarteirasDoRepresentante(int REP_ID, int? REP_ID_TO_IGNORE = null)
        {
            var queryCartRep = GetDb<COADCORPEntities>().CARTEIRA_REPRESENTANTE.Where(op => op.REP_ID == REP_ID && op.CARTEIRA.DATA_EXCLUSAO == null);
            
            if (REP_ID_TO_IGNORE != null)
            {
                queryCartRep = queryCartRep.Where(x => x.REP_ID != REP_ID_TO_IGNORE);
            }

            var query = queryCartRep.Select(op => op.CARTEIRA).Distinct();
            
            return ToDTO(query);
        }

        public IQueryable<CARTEIRA> TemplateCarteiraDeFranquiaDoRepresentante(int REP_ID, int? RG_ID = null, int? UEN_ID = 1)
        {
            var query = GetDb<COADCORPEntities>().CARTEIRA_REPRESENTANTE.Where(op => op.REP_ID == REP_ID && op.CARTEIRA.UEN_ID == UEN_ID && op.CARTEIRA.DATA_EXCLUSAO == null).Select(op => op.CARTEIRA).Distinct();

            if (RG_ID != null)
            {
                query = query.Where(x => x.RG_ID == RG_ID);
            }

            return query;
        }

        public ICollection<CarteiraDTO> GetCarteirasDeFranquiaDoRepresentante(int REP_ID, int? RG_ID = null, int? UEN_ID = 1)
        {
            var query = TemplateCarteiraDeFranquiaDoRepresentante(REP_ID, RG_ID, UEN_ID);
            return ToDTO(query);
        }

        public CarteiraDTO GetPrimeiraCarteiraDeFranquiaDoRepresentante(int REP_ID, int? RG_ID = null, int? UEN_ID = 1)
        {
            var query = TemplateCarteiraDeFranquiaDoRepresentante(REP_ID, RG_ID, UEN_ID).FirstOrDefault();
            return ToDTO(query);
        }


        public CarteiraDTO ListarCarteiraDoRepreECliente(int? CLI_ID, int? REP_ID, int? UEN_ID = 1)
        {
            var db = GetDb<COADCORPEntities>(false);
            var query = GetDbSet().Where(x =>
                        x.UEN_ID == UEN_ID
                        && x.DATA_EXCLUSAO == null 
                        &&
                        (from car_ass in db.CARTEIRA_CLIENTE
                        where 
                            car_ass.CLI_ID == CLI_ID 
                        select car_ass.CAR_ID).Contains(x.CAR_ID) 
                    &&
                        (from car_rep in db.CARTEIRA_REPRESENTANTE // exists carteira
                         where car_rep.REP_ID == REP_ID
                         select car_rep.CAR_ID).Contains(x.CAR_ID)
                    ).FirstOrDefault();

            return ToDTO(query);
                    
        }

        public IQueryable<CARTEIRA_CLIENTE> TemplateCarteiramentosCliente(int? CLI_ID)
        {
            var db = GetDb<COADCORPEntities>(false);
            var query = db.CARTEIRA_CLIENTE.Where(x => x.CLI_ID == CLI_ID);
            return query;
        }

        public int QtdCarteiramentosCliente(int? CLI_ID)
        {
            var db = GetDb<COADCORPEntities>(false);
            var query = db.CARTEIRA_CLIENTE.Where(x => x.CLI_ID == CLI_ID);
            return query.Count();
        }

        public bool HasCarteiramentosCliente(int? CLI_ID)
        {
            var db = GetDb<COADCORPEntities>(false);
            var query = db.CARTEIRA_CLIENTE.Where(x => x.CLI_ID == CLI_ID);
            return (query.Count() > 0);
        }

        public bool HasCarteiramentoClienteNaRegiao(int? CLI_ID, int? RG_ID)
        {
            var db = GetDb<COADCORPEntities>(false);
            var count = db.CARTEIRA_CLIENTE.Where(x => x.CLI_ID == CLI_ID && x.CARTEIRA.RG_ID == RG_ID).Count();

            return (count > 0);
        }

        public int QtdClienteCarteiramento(string CAR_ID)
        {
            var query = (from car_ass in db.CARTEIRA_CLIENTE 
                            where car_ass.CAR_ID == CAR_ID
                            select car_ass).Count();
            return query;
            
        }

        public IList<CarteiramentoClienteRepresentanteDTO> ListarCarteiramentoDoClientesPorRegiao(IEnumerable<int?> lstIdsClientes, int? rgID)
        {

            var query = (from
                            car_cli in db.CARTEIRA_CLIENTE
                         join
                             car in db.CARTEIRA on car_cli.CAR_ID equals car.CAR_ID
                         join
                             car_rep in db.CARTEIRA_REPRESENTANTE on car.CAR_ID equals car_rep.CAR_ID
                         where lstIdsClientes.Contains(car_cli.CLI_ID) &&
                            car.RG_ID == rgID
                         select new CarteiramentoClienteRepresentanteDTO()
                         {
                             CLI_ID = car_cli.CLI_ID,
                             REP_ID = car_rep.REP_ID,
                             RG_ID = car_rep.CARTEIRA.RG_ID,
                             DescricaoRegiao = car_rep.CARTEIRA.REGIAO.RG_DESCRICAO,
                             NomeRepresentante = car_rep.REPRESENTANTE.REP_NOME,
                             NomeCliente = car_cli.CLIENTES.CLI_NOME
                         });

            return query.ToList();
        }

        public Pagina<CarteiraDTO> BuscarCarteiras(
            string CAR_ID = null, 
            int? rgId = null, 
            int? uenId = null,
            int? repId = null,
            int pagina = 1, 
            int registosPorPagina =  7,
            bool associadoARepresentante = false)
        {
            if (string.IsNullOrWhiteSpace(CAR_ID))
                CAR_ID = null;

            IQueryable<CARTEIRA> query = null;

            if (associadoARepresentante || repId != null)
            {
                query = (from carRe in 
                             db.CARTEIRA_REPRESENTANTE
                         where (repId == null || carRe.REP_ID == repId)
                         select carRe.CARTEIRA);
            }
            else
            {
                query = (from carRe in
                             db.CARTEIRA
                         select carRe);
            }

             query = (from car in query
                         where
                            (CAR_ID == null || car.CAR_ID.Contains(CAR_ID)) &&
                            (rgId == null || car.RG_ID == rgId) &&
                            (uenId == null || car.UEN_ID == uenId)
                         select car);
            
            return ToDTOPage(query, pagina, registosPorPagina);
        }

        public bool ChecarCarteiraExiste(string carId)
        {
            var query = (from car in db.CARTEIRA
                         where car.CAR_ID  == carId
                         select car).Count();
            return (query > 0);
        }

        public CarteiraDTO FindCarteiraByCliIdAndRepId(int CLI_ID, int REP_ID, int? UEN_ID = null)
        {
            var db = GetDb<COADCORPEntities>(false);
            var query = GetDbSet().Where(x =>
                        (UEN_ID == null || x.UEN_ID == UEN_ID)
                        && x.DATA_EXCLUSAO == null
                        &&
                        (from car_ass in db.CARTEIRA_CLIENTE
                         where
                             car_ass.CLI_ID == CLI_ID
                         select car_ass.CAR_ID).Contains(x.CAR_ID)
                    &&
                        (from car_rep in db.CARTEIRA_REPRESENTANTE // exists carteira
                         where car_rep.REP_ID == REP_ID
                         select car_rep.CAR_ID).Contains(x.CAR_ID)
                    ).FirstOrDefault();

            return ToDTO(query);

        }

        public IList<CarteiraDTO> ListaCarteiraByCliIdAndRepId(int? CLI_ID, int? REP_ID, int? UEN_ID = null)
        {
            var db = GetDb<COADCORPEntities>(false);
            var query = GetDbSet().Where(x =>
                        (UEN_ID == null || x.UEN_ID == UEN_ID)
                        && x.DATA_EXCLUSAO == null
                        &&
                        (from car_ass in db.CARTEIRA_CLIENTE
                         where
                             car_ass.CLI_ID == CLI_ID
                         select car_ass.CAR_ID).Contains(x.CAR_ID)
                    &&
                        (from car_rep in db.CARTEIRA_REPRESENTANTE // exists carteira
                         where car_rep.REP_ID == REP_ID
                         select car_rep.CAR_ID).Contains(x.CAR_ID)
                    );
            return ToDTO(query);

        }

        public IList<CarteiraDTO> ListaCarteirasDoClienteERepresentante(int? cliID, int? repID, int? UEN_ID = null)
        {
            var queryCarCli = (from

                                car_cli in db.CARTEIRA_CLIENTE join
                                car in db.CARTEIRA on car_cli.CAR_ID equals car.CAR_ID join
                                car_rep in db.CARTEIRA_REPRESENTANTE on car.CAR_ID equals car_rep.CAR_ID
                         where
                             car.DATA_EXCLUSAO == null &&
                             car_rep.REPRESENTANTE.REP_ATIVO == 1 &&
                             car_cli.CLI_ID == cliID &&
                             car_rep.REP_ID == repID &&
                            (UEN_ID == null || car.UEN_ID == UEN_ID)
                               select car);
            var queryCarAssi = (from
                                cli in db.CLIENTES join
                                ass in db.ASSINATURA on cli.CLI_ID equals ass.CLI_ID join
                                car_ass in db.CARTEIRA_ASSINATURA on ass.ASN_NUM_ASSINATURA equals car_ass.ASN_NUM_ASSINATURA join
                                car in db.CARTEIRA on car_ass.CAR_ID equals car.CAR_ID join
                                car_rep in db.CARTEIRA_REPRESENTANTE on car.CAR_ID equals car_rep.CAR_ID
                                where
                                    car.DATA_EXCLUSAO == null &&
                                    car_rep.REPRESENTANTE.REP_ATIVO == 1 &&
                                    cli.CLI_ID == cliID &&
                                    car_rep.REP_ID == repID &
                                    (UEN_ID == null || car.UEN_ID == UEN_ID)
                                    
                                select car);
            var query = queryCarCli.Union(queryCarAssi).Distinct();
            return ToDTO(query);

        }

    }
}
