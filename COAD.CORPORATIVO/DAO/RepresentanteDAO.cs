using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Extensions;
using COAD.CORPORATIVO.Model.Dto.Custons;
using System.Data.Objects.SqlClient;
using COAD.SEGURANCA.Model;
using System.Data.Objects;
using COAD.CORPORATIVO.Model.Dto.Custons.Relatorios;
using GenericCrud.Models.Comparators;
using COAD.CORPORATIVO.Model.Dto.FiltersInfo;
using GenericCrud.Models.Filtros;

namespace COAD.CORPORATIVO.DAO
{
    public class RepresentanteDAO : DAOAdapter<REPRESENTANTE, RepresentanteDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }
        private FilaCadastroDAO _filaCadastroDAO = new FilaCadastroDAO();

        public RepresentanteDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }
       
        public Pagina<RepresentanteDTO> Representantes(string nomeRepresentante = null, string descricaoUf = null, int pagina = 1, 
            int registrosPorPagina = 15, 
            int? RG_ID = null, 
            int? UEN_ID = null,
            bool? gerente = null,
            bool uenIdPreenchido = false,
            int? nivelAcesso = null,
            int? nivelRepresentanteId = null)
        {
            IQueryable<REPRESENTANTE>  query = GetDbSet().Where(x => x.REP_ATIVO == 1 && x.NRP_ID != 5);

            if (!String.IsNullOrWhiteSpace(nomeRepresentante))
            {
                query = query.Where(obj => obj.REP_NOME.Contains(nomeRepresentante));
            }
            if (!String.IsNullOrWhiteSpace(descricaoUf))
            {
              //  query = query.Where(obj => obj.REGIAO_UF == descricaoUf);
            }

            if (RG_ID != null)
            {
                query = query.Where(obj => obj.RG_ID == RG_ID);
            }

            if (UEN_ID != null)
            {
                query = query.Where(obj => obj.UEN_ID == UEN_ID);
            }
            else if (uenIdPreenchido) //Se o uen id for nullo e essa flag for true traz representantes somente que possuiem algum uen preenchido
            {
                query = query.Where(obj => obj.UEN_ID != null);
            }

            if (gerente != null)
            {
                query = query.Where(obj => (gerente == false && obj.REP_GERENTE == null) || obj.REP_GERENTE == gerente);
            }

            if (nivelAcesso != null)
            {
                query = query.Where(obj => obj.NIVEL_REPRESENTANTE.NRP_NIVEL >= nivelAcesso);
            }

            if (nivelRepresentanteId != null)
            {
                query = query.Where(obj => obj.NRP_ID == nivelRepresentanteId);
            }

            return ToDTOPage(query, pagina, registrosPorPagina);
        }   

        public bool RepresentantesExistemNaMesmaRegiao(int? REP_ID1, int? REP_ID2){

            var db = GetDb<COADCORPEntities>(false);

            var query = (from rep1 in db.REPRESENTANTE
                        join rep2 in db.REPRESENTANTE
                        on rep1.RG_ID equals rep2.RG_ID
                        where rep1.REP_ID == REP_ID1
                        && rep2.REP_ID == REP_ID2
                        select 1);

            return query.Count() > 0;
        }

        public IList<RelRepresentanteCarteiraDTO> BuscarRepresentanteCarteira()
        {


            var query00 = (from a in db.REPRESENTANTE
                           join c in db.CARTEIRA_REPRESENTANTE on a.REP_ID equals c.REP_ID
                           join r in db.REPRESENTANTE on a.REP_ID_SUPERVISOR equals r.REP_ID into j1
                           from j2 in j1.DefaultIfEmpty()
                           group c by new { j2.REP_ID, j2.REP_NOME } into grp
                           select new RelRepresentanteCarteiraDTO()
                           {
                               REP_ID_SUPERVISOR = grp.Key.REP_ID,
                               REP_NOME_SUPERVISOR = grp.Key.REP_NOME,
                           }).OrderBy(x => x.REP_NOME_SUPERVISOR).ToList();

            var query = (from a in db.REPRESENTANTE
                         join c in db.CARTEIRA_REPRESENTANTE on a.REP_ID equals c.REP_ID
                         join r in db.REPRESENTANTE on a.REP_ID_SUPERVISOR equals r.REP_ID into j1
                         from j2 in j1.DefaultIfEmpty()
                         select new RelRepresentanteCarteiraDTO()
                         {
                             REP_ID_SUPERVISOR = a.REP_ID_SUPERVISOR,
                             REP_NOME_SUPERVISOR = j2.REP_NOME,
                             REP_ID = a.REP_ID,
                             REP_NOME = a.REP_NOME,
                             CAR_ID = c.CAR_ID,
                             REP_EMAIL = a.REP_EMAIL,
                             REP_RAMAL = a.REP_RAMAL,
                             REP_DDD_TELEFONE = a.REP_DDD_TELEFONE ,
                             REP_TELEFONE = a.REP_TELEFONE,
                             
                         }).OrderBy(x => x.REP_ID_SUPERVISOR).ThenBy(x => x.REP_NOME).ToList();

            

            foreach (var itemcabec in query00)
            {

                var _rep_ant = 0;
                List<RelRepresentanteCarteiraDTO> _listaitem = new List<RelRepresentanteCarteiraDTO>();
                RelRepresentanteCarteiraDTO _itemlista = new RelRepresentanteCarteiraDTO();

                foreach (var item in query.Where(x => x.REP_ID_SUPERVISOR == itemcabec.REP_ID_SUPERVISOR))
                {

                    if (_rep_ant == 0)
                    {
                        _rep_ant = (int)item.REP_ID;
                        _itemlista = new RelRepresentanteCarteiraDTO();
                        _itemlista = item;
                    }

                    if (item.REP_ID != _rep_ant)
                    {
                        _listaitem.Add(_itemlista);
                        _itemlista = new RelRepresentanteCarteiraDTO();
                        _itemlista = item;
                        _rep_ant = (int)item.REP_ID;
                    }
                    else
                    {
                        _itemlista.CAR_ID += " / " + item.CAR_ID;
                    }

                }

                itemcabec.LISTA =_listaitem;

            }

            return query00;

        }
        public IList<RepresentanteDTO> BuscarRepresentantes()
        {
            var db = GetDb<COADCORPEntities>(false);

            var query = (from r in db.REPRESENTANTE
                         select r).OrderBy(x => x.REP_NOME);

            return ToDTO(query);
        }

        public bool RepresentanteAdmiteVariasCarteiras(int REP_ID)
        {
            var query = GetDbSet().Where(x => x.REP_ID == REP_ID && x.REP_VARIAS_CARTEIRAS == 1).Select(x => 1).Count();
            return (query > 0);
        }

        public IList<RepresentanteDTO> RepresentantesDaRegiao(int? RG_ID, string CAR_ID_PARA_IGNORAR = null, int? UEN_ID = 1)
        {
           
            var query = GetDbSet().Where(x => x.RG_ID == RG_ID);

            if (CAR_ID_PARA_IGNORAR != null)
            {
                query = query.Where(x => 
                    !(from car in db.CARTEIRA_REPRESENTANTE
                     where car.CAR_ID == CAR_ID_PARA_IGNORAR 
                      && car.CARTEIRA.DATA_EXCLUSAO == null 
                        select car.REP_ID).Contains(x.REP_ID)
                    && x.UEN_ID == UEN_ID
                    && x.NRP_ID == 4

                    && (x.REP_GERENTE == null || (bool) x.REP_GERENTE == false));
            }

            return ToDTO(query);
        }

        public Pagina<RepresentanteDTO> RepresentantesDaRegiaoPaginado(
            RepresentanteFiltrosDTO filtros,
            IList<int> lstRepId = null)
        {
            int? UEN_ID = filtros.UEN_ID;
            string REP_NOME = filtros.REP_NOME;
            int? REP_ID_SUP = filtros.REP_ID_SUP;
            string email = filtros.email;
            int? rgId = filtros.rgId;
            string queryStr = filtros.query;
            
            IQueryable<REPRESENTANTE> query = 
                (from r in db.REPRESENTANTE
                     where (queryStr == null ||
                                     (r.REP_NOME.Contains(queryStr) ||
                                     r.REP_RAMAL.Contains(queryStr) ||
                                     r.REP_TELEFONE.Contains(queryStr) ||
                                     r.REP_DDD_TELEFONE2.Contains(queryStr))
                                 )
                     select r);

            if (lstRepId != null)
            {
                if (!string.IsNullOrWhiteSpace(queryStr))
                {
                    var subQuery = (from rep in db.REPRESENTANTE
                                    where lstRepId.Contains(rep.REP_ID)
                                    select rep);
                    query = subQuery.Union(query).Distinct();
                }
                else
                {
                    query = (from rep in db.REPRESENTANTE
                             where lstRepId.Contains(rep.REP_ID)
                             select rep);
                }

            }

            query = (from r in query where
                    (UEN_ID == null || UEN_ID == r.UEN_ID)
                            && ((REP_NOME == null || REP_NOME == "") || ((REP_NOME != null && REP_NOME != "") && r.REP_NOME.Contains(REP_NOME)))
                            && (REP_ID_SUP == null || (REP_ID_SUP != null && r.REP_ID_SUPERVISOR == REP_ID_SUP))
                            && (email == null || r.REP_EMAIL.Contains(email))
                            && (rgId == null || r.RG_ID == rgId)
                        select r);

            if (filtros.requisicao != null)
            {
                return ToDTOPage(query, filtros.requisicao);
            }
            else
            {
                return ToDTOPage(query, filtros.pagina, filtros.registrosPorPagina);
            }

        }
            public Pagina<RepresentanteDTO> RepresentantesDaRegiaoPaginado(
            int? UEN_ID, 
            string REP_NOME = null, 
            int? REP_ID_SUP = null, 
            int pagina = 1, 
            int registrosPorPagina = 12,
            IList<int> lstRepId = null,
            string email = null,
            int? rgId = null,
            string queryStr = null)
        {
            return RepresentantesDaRegiaoPaginado(new RepresentanteFiltrosDTO()
            {
                UEN_ID = UEN_ID,
                REP_NOME = REP_NOME,
                REP_ID_SUP = REP_ID_SUP,
                pagina = pagina,
                registrosPorPagina = registrosPorPagina,
                email = email,
                rgId = rgId,
                query = queryStr
            },
            lstRepId);
        }

        public IList<RepresentanteDTO> BuscarSupervisores()
        {
            var query = (from r in db.REPRESENTANTE
                         where r.REP_SUPERVISOR == true
                         select r);

            return ToDTO(query);
        }

        public IList<RepresentanteDTO> GetRepresentantesLogados(DateTime data, int? regiaoId, string excetoCarId)
        {            
            IList<FilaCadastroDTO> filaCadastro = _filaCadastroDAO.FindByRegiao(regiaoId, data);
            IEnumerable<int?> listIds = filaCadastro.Select(s => s.REP_ID);

            var query = GetDbSet().Where(x => listIds.Contains(x.REP_ID) && (x.REP_GERENTE == null || (bool) x.REP_GERENTE == false));

            if (!string.IsNullOrWhiteSpace(excetoCarId))
            {
                query = query.Where(x =>
                 !(from car in db.CARTEIRA_REPRESENTANTE
                   where car.CAR_ID == excetoCarId
                   && car.CARTEIRA.DATA_EXCLUSAO == null
                   select car.REP_ID).Contains(x.REP_ID));
            }

            return ToDTO(query);
        }

        /// <summary>
        /// Dado um Id de cliente, retorna todos os representantes que possuem ele em sua(s) carteira(s)
        /// </summary>
        /// <param name="CLI_ID">Id do cliente</param>
        /// <param name="nomeRepresentante">filtra o representante por nome</param>
        /// <param name="RG_ID">Filtra pelo id da regiao</param>,
        /// <param name="descricaoUf"></param>
        /// <param name="UEN_ID">Filtra pelo Id da UEN</param>
        /// <param name="gerente">Indica se é para retornar representante que são gerentes</param>
        /// <param name="pagina">Número da página</param>
        /// <param name="registrosPorPagina">Quantidade de registros retornados por página</param>
        /// <param name="REP_ID_PARA_EXCLUIR">Id do representante (e região dele) que não deve aparecer na lista</param>
        /// <returns></returns>
        public Pagina<RepresentanteDTO> RepresentantesDoCliente(int? CLI_ID, string nomeRepresentante = null, string descricaoUf = null, 
            int? RG_ID = null, int? UEN_ID = null, bool? gerente = null, 
            int pagina = 1, int registrosPorPagina = 7, int? REP_ID_PARA_EXCLUIR = null)
        {
            var query = (from car_rep in db.CARTEIRA_REPRESENTANTE
                         join car_cli in db.CARTEIRA_CLIENTE
                            on car_rep.CAR_ID equals car_cli.CAR_ID
                         where car_rep.REPRESENTANTE.REP_ATIVO == 1
                         && car_cli.CARTEIRA.DATA_EXCLUSAO == null
                         && car_cli.CLIENTES.DATA_EXCLUSAO == null
                         && car_cli.CLIENTES.CLI_ID == CLI_ID
                         select car_rep.REPRESENTANTE);


            if (!String.IsNullOrWhiteSpace(nomeRepresentante))
            {
                query = query.Where(obj => obj.REP_NOME.Contains(nomeRepresentante));
            }
            if (!String.IsNullOrWhiteSpace(descricaoUf))
            {
               // query = query.Where(obj => obj.REGIAO_UF == descricaoUf);
            }

            if (RG_ID != null)
            {
                query = query.Where(obj => obj.RG_ID == RG_ID);
            }

            if (UEN_ID != null)
            {
                query = query.Where(obj => obj.UEN_ID == UEN_ID);
            }

            if (gerente != null)
            {
                query = query.Where(obj => (gerente == false && obj.REP_GERENTE == null) || obj.REP_GERENTE == gerente);
            }

            if (REP_ID_PARA_EXCLUIR != null)
            {
                //var repParaExcluir = FindById(REP_ID_PARA_EXCLUIR);

                //if (repParaExcluir != null)
                //{
                //    //var regiaoId = repParaExcluir.RG_ID;
                //    query = query.Where(x => x.REP_ID != REP_ID_PARA_EXCLUIR /*|| (x.RG_ID != regiaoId)*/);
                //}   

                query = query.Where(x => x.REP_ID != REP_ID_PARA_EXCLUIR);
            }

            return ToDTOPage(query, pagina, registrosPorPagina);

        }

        /// <summary>
        /// Dado um Id usuário retorna todas os representantes que não possuem o cliente em sua(s) carteira(s)
        /// </summary>
        /// <param name="CLI_ID">Id do cliente</param>
        /// <param name="nomeRepresentante">filtra o representante por nome</param>
        /// <param name="RG_ID">Filtra pelo id da regiao</param>
        /// <param name="UEN_ID">Filtra pelo Id da UEN</param>
        /// <param name="gerente">Indica se é para retornar representante que são gerentes</param>
        /// <param name="pagina">Número da página</param>
        /// <param name="registrosPorPagina">Quantidade de registros retornados por página</param>
        /// <returns></returns>
        public Pagina<RepresentanteDTO> RepresentantesQueNaoSaoDoCliente(int? CLI_ID, string nomeRepresentante = null,
            int? RG_ID = null, int? UEN_ID = null, bool? gerente = null,
            int pagina = 1, int registrosPorPagina = 7)
        {
            var query = GetDbSet().Where(x => // busca todos os representantes que não estão na subquery
                
                        !(from car_rep in db.CARTEIRA_REPRESENTANTE
                         join car_cli in db.CARTEIRA_CLIENTE
                            on car_rep.CAR_ID equals car_cli.CAR_ID
                         where car_rep.REPRESENTANTE.REP_ATIVO == 1
                         && car_cli.CARTEIRA.DATA_EXCLUSAO == null
                         && car_cli.CLIENTES.DATA_EXCLUSAO == null
                         && car_cli.CLIENTES.CLI_ID == CLI_ID
                         select car_rep.REPRESENTANTE.REP_ID).Contains(x.REP_ID));


            if (!String.IsNullOrWhiteSpace(nomeRepresentante))
            {
                query = query.Where(obj => obj.REP_NOME.Contains(nomeRepresentante));
            }           

            if (RG_ID != null)
            {
                query = query.Where(obj => obj.RG_ID == RG_ID);
            }

            if (UEN_ID != null)
            {
                query = query.Where(obj => obj.UEN_ID == UEN_ID);
            }

            if (gerente != null)
            {
                query = query.Where(obj => (gerente == false && obj.REP_GERENTE == null) || obj.REP_GERENTE == gerente);
            }

            return ToDTOPage(query, pagina, registrosPorPagina);


        }


        public IList<AutoCompleteDTO<string>> RepresentantesDeFranquiaAutoCompleteDTO(int? RG_ID = null, int? UEN_ID = 1)
        {
            var query =
                (from rep in GetDbSet()
                 where
                   (rep.UEN_ID == UEN_ID) &&
                   (RG_ID == null || rep.RG_ID == RG_ID)
                 orderby rep.REP_NOME
                 select new AutoCompleteDTO<string>()
                 {
                     value = SqlFunctions.StringConvert((double) rep.REP_ID),
                     label = rep.REP_NOME
                 }
           );

            return query.ToList();
        }

        public IQueryable<CARTEIRA_REPRESENTANTE> TemplateClientesCadastradosNoDia(int? UEN_ID,
            DateTime dataInicial,
            DateTime? dataFinal = null, 
            bool preReserva = false)
        {
            var query = (from car_rep in db.CARTEIRA_REPRESENTANTE
                         join car_cli in db.CARTEIRA_CLIENTE
                          on car_rep.CAR_ID equals car_cli.CAR_ID
                         where 
                          (
                            (EntityFunctions.TruncateTime(car_cli.CLIENTES.DATA_CADASTRO) >= EntityFunctions.TruncateTime(dataInicial)) &&
                            (EntityFunctions.TruncateTime(car_cli.CLIENTES.DATA_CADASTRO) <= EntityFunctions.TruncateTime(dataFinal))                                                        
                          )  
                         && (
                               ((!preReserva) && car_cli.CLIENTES.INFO_MARKETING.O_CAD_ID != 6) ||
                               ((preReserva) && car_cli.CLIENTES.INFO_MARKETING.O_CAD_ID == 6)
                            )
                         && car_rep.REPRESENTANTE.REP_ATIVO == 1
                         && car_cli.CARTEIRA.DATA_EXCLUSAO == null
                         && car_cli.CLIENTES.DATA_EXCLUSAO == null
                         && (UEN_ID == null || (car_rep.REPRESENTANTE.UEN_ID == UEN_ID && car_rep.CARTEIRA.UEN_ID == UEN_ID))
                         select car_rep);
            return query;
        }

        public IQueryable<PRIORIDADE_ATENDIMENTO> TemplateEncaminhados(
            DateTime dataInicial,
            DateTime? dataFinal = null, 
            bool preReserva = false)
        {
            var query = (from pri_ate in db.PRIORIDADE_ATENDIMENTO
                         where pri_ate.TP_PRI_ID == 4
                         && 
                        (
                            (EntityFunctions.TruncateTime(pri_ate.PRI_DATA) >= EntityFunctions.TruncateTime(dataInicial)) &&
                            (EntityFunctions.TruncateTime(pri_ate.PRI_DATA) <= EntityFunctions.TruncateTime(dataFinal))
                        ) 
                        && 
                        (
                            (!preReserva &&  pri_ate.CLIENTES.INFO_MARKETING.O_CAD_ID != 6) ||
                            (preReserva && pri_ate.CLIENTES.INFO_MARKETING.O_CAD_ID == 6)
                        )
                         select pri_ate);

            return query;
        }

        public IQueryable<CARTEIRA_REPRESENTANTE> TemplateClientesComPrioriadesDeImportacaoNoDia(int? UEN_ID,
            DateTime dataInicial,
            DateTime? dataFinal = null)
        {
            var query = (from
                            rep in db.REPRESENTANTE join
                            car_rep in db.CARTEIRA_REPRESENTANTE on rep.REP_ID equals car_rep.REP_ID join
                            pri in db.PRIORIDADE_ATENDIMENTO on rep.REP_ID equals pri.REP_ID join
                            rg in db.REGIAO on pri.RG_ID equals rg.RG_ID
                         where
                          (
                            (EntityFunctions.TruncateTime(pri.PRI_DATA) >= EntityFunctions.TruncateTime(dataInicial)) &&
                            (EntityFunctions.TruncateTime(pri.PRI_DATA) <= EntityFunctions.TruncateTime(dataFinal))
                          ) && 
                            rg.UEN_ID == UEN_ID && 
                            pri.TP_PRI_ID == 5 &&
                             (from 
                                car_cli in db.CARTEIRA_CLIENTE join
                                cli in db.CLIENTES on car_cli.CLI_ID equals cli.CLI_ID join
                                car in db.CARTEIRA on car_cli.CAR_ID equals car.CAR_ID
                              where
                                 rep.REP_ATIVO == 1 && 
                                 car.DATA_EXCLUSAO == null && 
                                 cli.DATA_EXCLUSAO == null &&
                                 (UEN_ID == null || (rep.UEN_ID == UEN_ID && car.UEN_ID == UEN_ID))
                              select car.CAR_ID).Contains(car_rep.CAR_ID)                            
                         select car_rep);
            return query;
        }

        public Pagina<RelatorioRodizioDTO> RelatorioDePassivosRepresentantesLogados(
            int? uenId, 
            int? regiaoId,
            DateTime dataInicial,
            DateTime? dataFinal = null,
            int pagina = 1, 
            int registrosPorPagina = 100)
        {

            if (dataFinal == null)
            {
                dataFinal = dataInicial;
            }
            var fila = GetDbSet();

            var queryPassivos = TemplateClientesCadastradosNoDia(uenId, dataInicial, dataFinal);
            var queryPreReserva = TemplateClientesCadastradosNoDia(uenId, dataInicial, dataFinal, true);
            var queryEncaminhados = TemplateEncaminhados(dataInicial, dataFinal);
            var queryEncaminhadosPre = TemplateEncaminhados(dataInicial, dataFinal, true);
            //var queryPrioridadesDeImportacao = TemplateClientesComPrioriadesDeImportacaoNoDia(uenId, dataInicial, dataFinal);

            IQueryable<RelatorioRodizioDTO> query = (from 
                                                        fi in db.FILA_CADASTRO join 
                                                        rep in db.REPRESENTANTE on fi.REP_ID equals rep.REP_ID
                                                        let passivosRecebidos = (queryPassivos.Where(x => x.REP_ID == rep.REP_ID).Select(x => x.REP_ID)).Count()
                                                        let prereserva = (queryPreReserva.Where(x => x.REP_ID == rep.REP_ID).Select(x => x.REP_ID)).Count()
                                                        let encaminhados = (queryEncaminhados.Where(x => x.REP_ID == rep.REP_ID).Select(x => x.REP_ID)).Count()
                                                        let encaminhadosPreR = (queryEncaminhadosPre.Where(x => x.REP_ID == rep.REP_ID).Select(x => x.REP_ID)).Count()
                                                        //let importados = (queryPrioridadesDeImportacao.Where(x => x.REP_ID == rep.REP_ID).Select(x => x.REP_ID)).Count()
                                                     where
                                                         (EntityFunctions.TruncateTime(fi.FLC_DATA) == EntityFunctions.TruncateTime(dataInicial))  
                                                         && (regiaoId == null || fi.RG_ID == regiaoId && rep.RG_ID == regiaoId)
                                                         && rep.UEN_ID == uenId
                                                         && (rep.REP_GERENTE == null || rep.REP_GERENTE == false) 
                                                         && rep.REP_ATIVO == 1
                                                         && rep.REGIAO.UEN_ID == uenId
                                                     orderby fi.RG_ID ascending, fi.FLC_ORDEM ascending
                                                     select new RelatorioRodizioDTO()
                                                     {
                                                         REP_ID = rep.REP_ID,
                                                         REP_NOME = rep.REP_NOME,
                                                         Ordem = fi.FLC_ORDEM,
                                                         Encaminhados = encaminhados,
                                                         EncaminhadosPR = encaminhadosPreR,
                                                         PassivosRecebidos = passivosRecebidos,
                                                         PreReservasRecebidas = prereserva,
                                                         //Importados = importados,
                                                         RG_DESCRICAO = rep.REGIAO.RG_DESCRICAO
                                                     });

            return query.Paginar(pagina, registrosPorPagina);
        }

        public Pagina<RelatorioRodizioDTO> RelatorioDePassivos(
            int? uenId, 
            int? regiaoId,            
            DateTime dataInicial,
            DateTime? dataFinal = null,
            int pagina = 1, 
            int registrosPorPagina = 100)
        {

            if (dataFinal == null)
            {
                dataFinal = dataInicial;
            }

            var fila = GetDbSet();

            var queryPassivos = TemplateClientesCadastradosNoDia(uenId, dataInicial, dataFinal);
            var queryPreReserva = TemplateClientesCadastradosNoDia(uenId, dataInicial, dataFinal, true);
            var queryEncaminhados = TemplateEncaminhados(dataInicial, dataFinal);
            var queryEncaminhadosPre = TemplateEncaminhados(dataInicial, dataFinal, true);
            //var queryPrioridadesDeImportacao = TemplateClientesComPrioriadesDeImportacaoNoDia(uenId, dataInicial, dataFinal);

            IQueryable<RelatorioRodizioDTO> query = (from rep in db.REPRESENTANTE
                                                         let passivosRecebidos = (queryPassivos.Where(x => x.REP_ID == rep.REP_ID).Select(x => x.REP_ID)).Count()
                                                         let prereserva = (queryPreReserva.Where(x => x.REP_ID == rep.REP_ID).Select(x => x.REP_ID)).Count()
                                                         let encaminhados = (queryEncaminhados.Where(x => x.REP_ID == rep.REP_ID).Select(x => x.REP_ID)).Count()
                                                         let encaminhadosPreR = (queryEncaminhadosPre.Where(x => x.REP_ID == rep.REP_ID).Select(x => x.REP_ID)).Count()
                                                         //let importados = (queryPrioridadesDeImportacao.Where(x => x.REP_ID == rep.REP_ID).Select(x => x.REP_ID)).Count()
                                                     where (regiaoId == null || rep.RG_ID == regiaoId) && 
                                                         rep.UEN_ID == uenId
                                                         && (rep.REP_GERENTE == null || rep.REP_GERENTE == false) 
                                                         && rep.REP_ATIVO == 1
                                                         && rep.REGIAO.UEN_ID == uenId
                                                     orderby rep.REP_NOME ascending
                                                     select new RelatorioRodizioDTO()
                                                     {
                                                         REP_ID = rep.REP_ID,
                                                         REP_NOME = rep.REP_NOME,
                                                         Encaminhados = encaminhados,
                                                         EncaminhadosPR = encaminhadosPreR,
                                                         PassivosRecebidos = passivosRecebidos,
                                                         PreReservasRecebidas = prereserva,
                                                         //Importados = importados,
                                                         RG_DESCRICAO = rep.REGIAO.RG_DESCRICAO
                                                     });

            

            var paginaObj = query.Paginar(pagina, registrosPorPagina);

            var list = paginaObj.lista.ToList();

            var totais = (from to in list
                              select new RelatorioRodizioDTO()
                              {
                                  Encaminhados = list.Sum(x => x.Encaminhados),
                                  EncaminhadosPR = list.Sum(x => x.EncaminhadosPR),
                                  PassivosRecebidos = list.Sum(x => x.PassivosRecebidos),
                                  PreReservasRecebidas = list.Sum(x => x.PreReservasRecebidas),
                                 // Importados = list.Sum(x => x.Importados)
                              }).FirstOrDefault();
            list.Add(totais);
            paginaObj.lista = list;
            return paginaObj;
        }

       public Pagina<RepresentanteDTO> ListarProfessor(
               string nomeProfessor = null, 
               int pagina = 1,
               int registrosPorPagina = 15,
               int? RG_ID = null,
               int? UEN_ID = null,
               bool uenIdPreenchido = false,
               int? nivelAcesso = null)
        {
            IQueryable<REPRESENTANTE> query = GetDbSet().
                Where(x =>
                x.REP_ATIVO == 1 &&
                x.NRP_ID == 5);

            if (!String.IsNullOrWhiteSpace(nomeProfessor))
            {
                query = query.Where(obj => obj.REP_NOME.Contains(nomeProfessor));
            }

            if (RG_ID != null)
            {
                query = query.Where(obj => obj.RG_ID == RG_ID);
            }

            if (UEN_ID != null)
            {
                query = query.Where(obj => obj.UEN_ID == UEN_ID);
            }
            else if (uenIdPreenchido) //Se o uen id for nullo e essa flag for true traz representantes somente que possuiem algum uen preenchido
            {
                query = query.Where(obj => obj.UEN_ID != null);
            }

            if (nivelAcesso != null)
            {
                query = query.Where(obj => obj.NIVEL_REPRESENTANTE.NRP_NIVEL >= nivelAcesso);
            }

            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        public IList<RepresentanteDTO> ListarRepresentantesComCarteiramento(int? rgId)
        {
            var query = TemplateRepresentantesComCarteiramento(rgId);
            return ToDTO(query);
        }

        public bool HasRepresentantesComCarteiramento(int? rgId)
        {
            var query = TemplateRepresentantesComCarteiramento(rgId, true);
            return (query.Count() > 0);
        }

        public IQueryable<REPRESENTANTE> TemplateRepresentantesComCarteiramento(int? rgId, bool checaInativo = false)
        {
            var query = (from rep in
                             db.REPRESENTANTE
                         where rep.NRP_ID == 4 &&
                         (checaInativo == false || (checaInativo == true && (rep.REP_INATIVO_RODIZIO_IMP == null || rep.REP_INATIVO_RODIZIO_IMP == false))) &&
                         (from carRep in db.CARTEIRA_REPRESENTANTE
                          where
                              rep.UEN_ID == carRep.CARTEIRA.UEN_ID &&
                              carRep.CARTEIRA.RG_ID == rep.RG_ID &&
                              carRep.CARTEIRA.RG_ID == rgId
                          select carRep.REP_ID).Contains(rep.REP_ID)
                         select rep);
            return query;
        }

        public RepresentanteDTO ListarRepresentantePorCarteira(string carId)
       {
           var query = (from carRep in db.CARTEIRA_REPRESENTANTE
                        where carRep.CAR_ID == carId
                        select carRep.REPRESENTANTE)
                        .FirstOrDefault();

           return ToDTO(query);
       }

       public IList<AutoCompleteDTO<int>> ListarRepresentanteAutocomplete(string nome)
       {
           var query = (from rep in db.REPRESENTANTE
                        where 
                            rep.REP_NOME.Contains(nome) && 
                            rep.REP_ATIVO == 1
                        select new AutoCompleteDTO<int>() { 
                            label = rep.REP_NOME,
                            value = rep.REP_ID
                        });

           return query.ToList();
       }

        /// <summary>
        /// Dado um Id de cliente, retorna todos os representantes que possuem ele em sua(s) carteira(s),
        /// (Carteiras de Assinaturas e Cursos)
        /// </summary>
        /// <param name="CLI_ID">Id do cliente</param>
        /// <param name="nomeRepresentante">filtra o representante por nome</param>
        /// <param name="RG_ID">Filtra pelo id da regiao</param>,
        /// <param name="descricaoUf"></param>
        /// <param name="UEN_ID">Filtra pelo Id da UEN</param>
        /// <param name="gerente">Indica se é para retornar representante que são gerentes</param>
        /// <param name="pagina">Número da página</param>
        /// <param name="registrosPorPagina">Quantidade de registros retornados por página</param>
        /// <param name="REP_ID_PARA_EXCLUIR">Id do representante (e região dele) que não deve aparecer na lista</param>
        /// <returns></returns>
        public Pagina<ProspectRepresentanteDTO> ListarTodosOsRepresentantesDoCliente(int? CLI_ID, string nomeRepresentante = null, string descricaoUf = null,
            int? RG_ID = null, int? UEN_ID = null, bool? gerente = null, int? empId = null,
            int pagina = 1, int registrosPorPagina = 7, int? REP_ID_PARA_EXCLUIR = null)
        {
            var representanteCarteiraCliente =
                (from car_rep in db.CARTEIRA_REPRESENTANTE
                 join car_cli in db.CARTEIRA_CLIENTE
                    on car_rep.CAR_ID equals car_cli.CAR_ID
                 where car_rep.REPRESENTANTE.REP_ATIVO == 1
                    && car_cli.CARTEIRA.DATA_EXCLUSAO == null
                    && car_cli.CLIENTES.DATA_EXCLUSAO == null
                    && car_cli.CLIENTES.CLI_ID == CLI_ID
                    && (empId == null || car_cli.CARTEIRA.EMP_ID == empId)
                 select 
                 new {
                     REPRESENTANTE = car_rep.REPRESENTANTE,
                     CARTEIRA = car_rep.CARTEIRA
                 });

            var query = representanteCarteiraCliente;

            if(UEN_ID != null && UEN_ID == 2)
            {
                var representanteCarteiraAssinatura = 
                    (from car_rep in db.CARTEIRA_REPRESENTANTE
                             join car_ass in db.CARTEIRA_ASSINATURA
                                on car_rep.CAR_ID equals car_ass.CAR_ID
                             where car_rep.REPRESENTANTE.REP_ATIVO == 1
                                && car_ass.CARTEIRA.DATA_EXCLUSAO == null
                                && car_ass.ASSINATURA.CLIENTES.DATA_EXCLUSAO == null
                                && car_ass.ASSINATURA.CLIENTES.CLI_ID == CLI_ID
                            select
                             new
                             {
                                 REPRESENTANTE = car_rep.REPRESENTANTE,
                                 CARTEIRA = car_rep.CARTEIRA
                             });

                 query = representanteCarteiraCliente
                    .Concat(representanteCarteiraAssinatura)
                    .Distinct();
            }

            if (!String.IsNullOrWhiteSpace(nomeRepresentante))
            {
                query = query.Where(obj => obj.REPRESENTANTE.REP_NOME.Contains(nomeRepresentante));
            }
            if (!String.IsNullOrWhiteSpace(descricaoUf))
            {
                query = query.Where(obj => obj.REPRESENTANTE.REGIAO_UF == descricaoUf);
            }

            if (RG_ID != null)
            {
                query = query.Where(obj => obj.REPRESENTANTE.RG_ID == RG_ID);
            }

            if (gerente != null)
            {
                query = query.Where(obj => ((gerente == false && obj.REPRESENTANTE.REP_GERENTE == null) || obj.REPRESENTANTE.REP_GERENTE == gerente));
            }

            if (REP_ID_PARA_EXCLUIR != null)
            {
                query = query.Where(x => x.REPRESENTANTE.REP_ID != REP_ID_PARA_EXCLUIR);
            }

            if (UEN_ID != null)
            {
                query = query.Where(obj => obj.CARTEIRA.UEN_ID == UEN_ID && obj.CARTEIRA.REGIAO.UEN_ID == UEN_ID);
            }

            var objPagina = query.Paginar(pagina, registrosPorPagina);

            if(objPagina != null) {
                var lstDTO = objPagina.lista.Select(x => new ProspectRepresentanteDTO()
                {
                    CarId = x.CARTEIRA.CAR_ID,
                    Representante = ToDTO(x.REPRESENTANTE),
                    Regiao = x.CARTEIRA.REGIAO.RG_DESCRICAO,
                    Uen = x.CARTEIRA.UEN.UEN_DESCRICAO
                });

                var representantes = objPagina.Derivar(lstDTO);
                return representantes;
            }

            return null;
        }
    }
}
