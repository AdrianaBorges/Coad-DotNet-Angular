using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
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
using System.Transactions;
using COAD.CORPORATIVO.Model.Comparators;
using System.Data.Entity.Validation;
using Coad.GenericCrud.Exceptions;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using COAD.CORPORATIVO.Exceptions;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Service.Formatting;
using COAD.CORPORATIVO.Model.Dto.Formatters;
using GenericCrud.Models.HistoryRegister;
using COAD.CORPORATIVO.Service.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons;
using GenericCrud.Models.MessageFormatter;
using COAD.CORPORATIVO.LEGADO.Service;
using GenericCrud.Metadatas;
using GenericCrud.Service;
using COAD.SEGURANCA.Service.Custons.Context;

namespace COAD.CORPORATIVO.Service
{
    
    [ServiceConfig("CAR_ID")]
    public class CarteiramentoSRV : GenericService<CARTEIRA, CarteiraDTO, string>
    {
        private AssinaturaSRV _assinaturaSRV = new AssinaturaSRV();
        private EmpresaSRV _empresaSRV = new EmpresaSRV();
        private CarteiramentoDAO _dao = new CarteiramentoDAO();
        private RepresentanteDAO _daoRep = new RepresentanteDAO();
        private CarteiraRepresentanteSRV _cartRepSrv = new CarteiraRepresentanteSRV();
        private CarteiraAssinaturaSRV _carteiraAssinaturaSRV = new CarteiraAssinaturaSRV();
        private CarteiraClienteSRV _carteiraClienteSRV = new CarteiraClienteSRV();
        private FilaCadastroSRV _filaCadastroSRV = new FilaCadastroSRV();
        private NotificacoesSRV _notificacoeSRV = new NotificacoesSRV();
        private RepresentanteSRV _representanteSRV = new RepresentanteSRV();
        private PrioridadeAtendimentoSRV _prioridadeAtendimentoSRV = new PrioridadeAtendimentoSRV();
        private TabSeqSRV _sequenciaSRV = new TabSeqSRV();
        private HistAtendSRV _historicoAtendimentoService = new HistAtendSRV();
        private RegiaoSRV _regiaoService = new RegiaoSRV();
        private MessageFormatterService formatterService { get; set; }
        private RepresentanteLegadoSRV _repLegado = new RepresentanteLegadoSRV();

        public CarteiramentoSRV()
        {
            Dao = _dao;
           // this.formatterService = FormatterServiceLocalFactory.CriarMessageFormatterServiceCoorporativo();
            
        }

        public Pagina<CarteiraDTO> Carteiramentos(
            string siglaRegiao = null, 
            string representante = null, 
            int areaId = 1, 
            int pagina = 1, 
            int registrosPorPagina = 7,
            int? RG_ID = null,
            int? REP_ID_TO_IGNORE = null,
            int? UEN_ID = null)
        {
            var paginaDto =  _dao.Carteiramentos(siglaRegiao, 
                representante, 
                areaId, 
                pagina, 
                registrosPorPagina, 
                RG_ID, REP_ID_TO_IGNORE,
                UEN_ID);

            _PreencherEmpresa(paginaDto.lista);

            return paginaDto;
        }

       /// <summary>
       /// Traz os carteiramentos de uma determinada região com base no representante passado.
       /// Não traz os carteiramentos do represente passado, porque essa funcionalidade é para listar
       /// os carteiramentos para incluir ao representante.
       /// </summary>
       /// <param name="siglaRegiao"></param>
       /// <param name="representante"></param>
       /// <param name="areaId"></param>
       /// <param name="pagina"></param>
       /// <param name="registrosPorPagina"></param>
       /// <param name="REP_ID">Traz id do representante para trazer as carteiras de acordo com a região </param>
       /// <returns></returns>
        public Pagina<CarteiraDTO> ListarCarteiramemtoParaEscolha(int? REP_ID, int? UEN_ID = null, int pagina = 1, int registrosPorPagina = 7)
        {
            if (REP_ID != null)
            {
                var rep = _representanteSRV.FindById(REP_ID);

                if (rep != null && rep.REGIAO != null)
                {
                    var RG_ID = rep.REGIAO.RG_ID;
                    var paginaDto = _dao.Carteiramentos(null, null, 1, pagina, registrosPorPagina, RG_ID, REP_ID, UEN_ID);

                    _PreencherEmpresa(paginaDto.lista);
                    return paginaDto; 
                }
                return new Pagina<CarteiraDTO>();
                   
            }

            throw new ArgumentException("O parâmetro REP_ID é obrigatório");
            
        }

        /// <summary>
        /// Preenche as empresas da base coadsys no DTO
        /// </summary>
        /// <param name="carteiras"></param>
        private void _PreencherEmpresa(IEnumerable<CarteiraDTO> carteiras)
        {
            if (carteiras != null)
            {
                foreach (var car in carteiras)
                {
                    if (car.EMP_ID != null)
                    {
                        car.EMPRESA = _empresaSRV.FindById(car.EMP_ID);
                    }
                }
            }
        }

        /// <summary>
        /// Troca a representante da carteira
        /// </summary>
        /// <param name="carteiraId"></param>
        /// <param name="repreId"></param>
      //  [Obsolete("A estrutura do encarteiramento foi mudada. Logo, esse método deve ser revisado, ou excluído no futuro.")]
        public void TrocarRepresentante(string carteiraId, int repreId)
        {

            if (String.IsNullOrWhiteSpace(carteiraId))
            {
                throw new ArgumentException("O parâmetro carteiraId não é válido");
            }
            if (repreId < 1)
            {
                throw new ArgumentException("O parâmetro repreId não é válido");
            }

                        
            // Embora a estrutura da carteira para a operadora esteja aberta para vários representantes
            // o sistema restringe para apenas 1 representante por carteira. Casos extraordinários podem ocorrer
            // mas não afetará esse método

            InativarRepresentanteDaCarteira(carteiraId);

            var representanteNovo = _daoRep.FindByIdConverted(repreId);

            if (representanteNovo != null)
            {
                representanteNovo.CAR_ID = carteiraId;
                representanteNovo.REP_ATIVO = 1;
            }
            else
            {
                throw new Exception("Não foi possível achar o representante pelo seu código");
            }
            
            _daoRep.ConvertAndMerge(representanteNovo, "REP_ID");

        }

       


        /// <summary>
        /// Inativa o primeiro representante da carteira
        /// </summary>
        /// <param name="carteira"></param>
        public void InativarRepresentanteDaCarteira(string carteiraId)
        {
            var carteira = _dao.FindByIdConverted(carteiraId);
            InativarRepresentanteDaCarteira(carteira);
        }

        /// <summary>
        /// Inativa o primeiro representante da carteira
        /// </summary>
        /// <param name="carteira"></param>        
        public void InativarRepresentanteDaCarteira(CarteiraDTO carteira)
        {
            var rep = _dao.GetCarteiraAtiva(carteira);// pegando a primeira representante ativo para inativa-la

            if (rep != null && rep.REP_ID > 1)
            {
                rep.REP_ATIVO = 0;
                _daoRep.ConvertAndMerge(rep, "REP_ID");
            }

        }


        public void SalvarConfig(CarteiraDTO carteira)
        {
            using (var scope = new TransactionScope()) // crio uma transação
            {
                try
                {
                    if (carteira.CAR_ID != null) // verifico se devo alterar ou incluir o objeto
                    {
                        var carteiraRep = new List<CarteiraRepresentanteDTO>(); //(carteira.CARTEIRA_REPRESENTANTE != null) ? carteira.CARTEIRA_REPRESENTANTE.Where(op => !op.EXCLUIR) : new List<CarteiraRepresentanteDTO>();
                        
                        var carteiraSalva = Merge(carteira, "CAR_ID"); // salvo o prospect (alteração)

                        _cartRepSrv.AdicionarReferencias(carteiraRep);
                        ExcluirRepresentantesCarteira(carteira); // inativo todos os telefones que não foram mantidos na lista

                        if (carteiraRep != null)
                        {
                            _cartRepSrv.SalvarTodos(carteiraRep);
                            //_telService.SalvarTelefones((int)prospectSalvo.ID, telefones.AsQueryable()); // atualizo os demais telefones
                        }
                        scope.Complete(); // comito a transação

                    }
                    else
                    {
                        //carteira.DATA_CADASTRO = DateTime.Now;
                        var cartRep = carteira.CARTEIRA_REPRESENTANTE;
                        var prospectSalvo = Save(carteira); // salvo o prospect (inclusão)
                        //_telService.SalvarTelefones((int)prospectSalvo.ID, telefones.AsQueryable()); // incluo os novos telefones
                        scope.Complete();  // comito a transação
                    }
                }
                catch (DbEntityValidationException dbEx)
                {
                    var _erro = new FormattedDbEntityValidationException(dbEx);

                    SysException.RegistrarLog(_erro.Message, "", SessionContext.autenticado);

                    throw _erro;
                }
                catch (Exception ex)
                {
                    throw new Exception(SysException.Show(ex));
                }
            }
        }


        /// <summary>
        /// Excluir os telefones do prospect, exceto os passados no método
        /// </summary>
        /// <param name="representante"></param>
        /// <param name="excecoes"></param>
        public void ExcluirRepresentantesCarteira(CarteiraDTO representante)
        {

            var carId = representante.CAR_ID; // pego o suspect para compara-lo os telefones salvos com os para salvar

            if (representante.CARTEIRA_REPRESENTANTE != null)
            {

                var excecoes = representante.CARTEIRA_REPRESENTANTE; // pego os telefones existentes para não serem excluídos

                var carteiraOriginal = FindByIdFullLoaded(carId); // pego o prospect original do banco para comparar os telefones salvos no banco
                var carteira_representante = carteiraOriginal.CARTEIRA_REPRESENTANTE; // pego os telefones para comparar

                if (carteira_representante != null && excecoes != null)
                {
                    var carteirasRepresentantesParaExcluir = carteira_representante.Except(excecoes, new CarteiraRepresentanteDTOComparator()).Where(op => op.CAR_ID != null && op.REP_ID != null);

                    foreach (var car in carteirasRepresentantesParaExcluir)
                    {
                        _cartRepSrv.Delete(car, "CAR_ID", "REP_ID");
                    }
                }
            }

            //if (representante.CARTEIRA_REPRESENTANTE != null)
            //{
            //    var paraExcluir = representante.CARTEIRA_REPRESENTANTE.Where(op => op.EXCLUIR);
                
            //    _cartRepSrv.DeleteAll(paraExcluir, "CAR_ID", "REP_ID");       
            //}
        }


        public ICollection<CarteiraDTO> GetCarteirasDoRepresentante(int REP_ID, int? REP_ID_TO_IGNORE = null)
        {
            var lstCarteira = _dao.GetCarteirasDoRepresentante(REP_ID, REP_ID_TO_IGNORE);
            _PreencherEmpresa(lstCarteira);

            return lstCarteira;
        }

        public CarteiraDTO ListarCarteiraDoRepreECliente(int? CLI_ID, int? REP_ID, int? UEN_ID = 1)
        {
            return _dao.ListarCarteiraDoRepreECliente(CLI_ID, REP_ID, UEN_ID);
        }

        public ICollection<CarteiraDTO> GetCarteirasDeFranquiaDoRepresentante(int REP_ID, int? RG_ID = null, int? UEN_ID = 1)
        {
            return _dao.GetCarteirasDeFranquiaDoRepresentante(REP_ID, RG_ID, UEN_ID);
        }

        public CarteiraDTO GetPrimeiraCarteiraDeFranquiaDoRepresentante(int REP_ID, int? RG_ID = null, int? UEN_ID = 1)
        {
            return _dao.GetPrimeiraCarteiraDeFranquiaDoRepresentante(REP_ID, RG_ID, UEN_ID);
        }

        public CarteiraDTO GetPrimeiraCarteiraDeFranquiaDoRepresentante(RepresentanteDTO representante)
        {
            if (representante == null)
            {
                throw new ArgumentException("O argumento representante não pode ser nullo");
            }

            var repId = (int)representante.REP_ID;
            var rgId = representante.RG_ID;
            var uenId = representante.UEN_ID;

            CarteiraDTO car = GetPrimeiraCarteiraDeFranquiaDoRepresentante(repId, rgId, uenId);
            return car; 

        }
        /// <summary>
        /// Encarteira um Suspect para um representante
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="REP_ID"></param>
        public void EncarteirarSuspectFranquia(ClienteDto cliente, int REP_ID)
        {
            if (cliente != null)
            {
                var rep = _daoRep.FindByIdConverted(REP_ID);

                if (rep != null && rep.RG_ID != null)
                {
                    var regiaoId = rep.RG_ID;
                    var uenId = rep.UEN_ID;
                    var carteira = GetPrimeiraCarteiraDeFranquiaDoRepresentante(REP_ID, regiaoId, uenId);

                    if (carteira != null)
                    {
                        _carteiraClienteSRV.CriarESalvarCarteiraCliente(carteira, cliente);
                    }
                    else
                    {
                        throw new Exception("Não foi possível encarteirar o suspect. A representante não possui nenhuma carteira, ou nenhuma carteira pertence a regiao do representante.");
                    }
                }
            }
            else
            {
                throw new Exception("Não foi possível salvar o suspect. A clientes não pode ser gerada.");
            }
        }


        /// <summary>
        /// Gera a clientes do cliente e encartera para a representante
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="REP_ID"></param>
        public void EncarteirarSuspectECriarPrioridade(ClienteDto cliente, int REP_ID, int TP_PRI_ID = 3, int? REP_ID_DEMANDANTE = null)
        {
            //var clientes = _assinaturaSRV.GerarAssinaturaFranquia(cliente);
            EncarteirarSuspectFranquia(cliente, REP_ID);

            var rep = _daoRep.FindByIdConverted(REP_ID);
            if (cliente != null && cliente.CLI_ID != null)
            {
                int clienteId = (int) cliente.CLI_ID;
                _prioridadeAtendimentoSRV.RegistrarPrioridade(REP_ID, clienteId, TP_PRI_ID, "Recebido por rodizio", REP_ID_DEMANDANTE);
            }
            
        }

        /// <summary>
        /// Gera a clientes do cliente e encartera para a representante
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="REP_ID"></param>
        public void EncarteirarSuspectPorRodizio(ClienteDto cliente, int REGIAO_ID, int REP_ID_DEMANDANTE)
        {
            var representante = _filaCadastroSRV.ObterRepresentantePorRodizio(REGIAO_ID); // pego o próximo representante da fila de rodizio da regiao

            if (representante == null)
            {
                throw new OperadoraNaoEncontradaPorRodizioException("Não há operadora logada para rodizio");
            }

            var gerente = _representanteSRV.FindById(REP_ID_DEMANDANTE); // pego o representante do gerente logado

            if (representante != null && representante.REP_ID != null && cliente != null) // verifico se os dados são válidos
            {
                var REP_ID = (int) representante.REP_ID;
                EncarteirarSuspectECriarPrioridade(cliente, REP_ID, 1, REP_ID_DEMANDANTE);        

                _filaCadastroSRV.MoverFila(REGIAO_ID);

                var nomeRepresentante = representante.REP_NOME;
                var clienteId = (int) cliente.CLI_ID;
                var nomeCliente = cliente.CLI_NOME;

                _notificacoeSRV.InserirNotificacaoParaGerenteRecebimentoRodizio(nomeRepresentante, nomeCliente, REP_ID_DEMANDANTE, clienteId);
                _notificacoeSRV.InserirNotificacaoRecebimentoRodizio(nomeRepresentante, nomeCliente, REP_ID, clienteId);
            }

        }

        /// <summary>
        /// Gera a clientes do cliente e encartera para a representante
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="REP_ID"></param>
        ///
        [MetodoTopLevelReferenciavel]
        public void EncarteirarSuspectPorRepresentante(ClienteDto cliente, int REP_ID, int REP_ID_DEDAMANDANTE)
        {
            
            var representante = _representanteSRV.FindById(REP_ID); 

            if (representante != null && representante.REP_ID != null && cliente != null) // verifico se os dados são válidos
            {
               EncarteirarSuspectECriarPrioridade(cliente, REP_ID, 1, REP_ID_DEDAMANDANTE);

                var nomeRepresentante = representante.REP_NOME;
                var clienteId = (int)cliente.CLI_ID;
                var nomeCliente = cliente.CLI_NOME;

                _notificacoeSRV.InserirNotificacaoNovoCliente(nomeRepresentante, nomeCliente, REP_ID, clienteId);
            }

        }

        public void TrocarCarteiramento(int REP_ID, string CAR_ID_ANTIGO, string CAR_ID_NOVO)
        {
            using (var scope = new TransactionScope())
            {
                 _cartRepSrv.TrocarCarteiraRepresentante(REP_ID, CAR_ID_ANTIGO, CAR_ID_NOVO);
                  scope.Complete();
                //}
                //else
                //{
                //    var sb = new StringBuilder();
                //    sb.Append("Não é possível trocar a carteira.");
                //    sb.Append("O representante possuí uma carteira");
                //    sb.Append("esse representante não está configurado para admitir multiplas carteiras.");

                //    throw new CarteiramentoException(sb.ToString());
                //}
                
            }
        }

        public void AdicionarCarteiramento(int REP_ID, string CAR_ID_NOVO)
        {
            using (var scope = new TransactionScope())
            {
                if(_representanteSRV.RepresentanteAdmiteVariasCarteiras(REP_ID)){

                    _cartRepSrv.AdicionarCarteiraRepresentante(REP_ID, CAR_ID_NOVO);
                    scope.Complete();
                }
                else
                {
                    var sb = new StringBuilder();
                    sb.Append("Não é possível trocar a carteira.");
                    sb.Append("O representante já possuí uma carteira");
                    sb.Append("esse representante não está configurado para admitir multiplas carteiras.");

                    throw new CarteiramentoException(sb.ToString());
                }

            }
        }

        public void RemoverCarteiraDoRepresentante(string CAR_ID, int REP_ID)
        {
            _cartRepSrv.DeletarCarteiraRepresentante(CAR_ID, REP_ID);
        }

        public CarteiraDTO FindByIdFullLoaded(string CAR_ID, bool trazCarteiraRepresentante = true, bool trazCarteiraAssinatura = true)
        {

            CarteiraDTO car = (trazCarteiraRepresentante) ? _dao.FindByIdFullLoaded(CAR_ID) : FindById(CAR_ID);

            if (car != null)
            {
                if (trazCarteiraAssinatura)
                {
                    _carteiraAssinaturaSRV.PreencherCarteiraAssinatura(car);
                }                
                return car;
            }

            return null;
        }

        public void ExcluirCarteira(string CAR_ID)
        {
            CarteiraDTO carteira = FindById(CAR_ID);
            ExcluirCarteira(carteira);
        }

        public int? GerarCarteiraFranquia(int? UEN_ID, int RG_ID)
        {
            int? idCarteira = null;
            using (var scope = new TransactionScope())
            {
                idCarteira = _sequenciaSRV.GetSeqCarteira();
                idCarteira++;
                CarteiraDTO cart = new CarteiraDTO()
                {

                    AREA_ID = 1,
                    CAR_ID = idCarteira.ToString(),
                    REGIAO_UF = "RJ",
                    REGIAO_ID = "RJ",
                    SEQ_REG = "1",
                    CAR_VARIOS_REPRESENTANTES = 0,
                    RG_ID = RG_ID,
                    UEN_ID = UEN_ID
                };
                Save(cart);

                _sequenciaSRV.AcrescentaSeqCarteira((int) idCarteira);

                scope.Complete();
            }

            return idCarteira;

                
        }

        public void ExcluirCarteira(CarteiraDTO carteira)
        {
            if (carteira != null) 
            {
                carteira.DATA_EXCLUSAO = DateTime.Now;
                Merge(carteira);
            }
        }

        /// <summary>
        ///Encarteira o cliente para o representante passado.
        ///Método raiz de encarteiramento.
        /// Este é um método de encarteiramento que serve como base para outros métodos mais especializados.
        /// </summary>
        /// <param name="cliente">Objeto do cliente que será adicionado a carteira do representante</param>
        /// <param name="REP_ID">Id do representante que receberá o cliente</param>
        public void EncarteirarSuspect(ClienteDto cliente, int REP_ID, int TP_PRI_ID = 3, int? REP_DEMANDANTE = null)
        {
            // var clientes = _assinaturaSRV.ExtrairAssinaturaFranquiaCliente(cliente, true);
            EncarteirarSuspectFranquia(cliente, REP_ID);

            if (cliente != null && cliente.CLI_ID != null)
            {
                int clienteId = (int)cliente.CLI_ID;
                _prioridadeAtendimentoSRV.RegistrarPrioridade(REP_ID, clienteId, TP_PRI_ID, "Recebido por rodizio", REP_DEMANDANTE);
            }

        }

        /// <summary>
        /// Encarteira a cliente para o representante por rodízio.
        /// Este é um método de encarteiramento que serve como base para outros métodos mais especializados.
        /// Retorna o id do representante escolhido no rodizio
        /// </summary>
        /// <param name="cliente">cliente que será encarteira</param>
        /// <param name="REGIAO_ID">Região onde será realizado o rodizio</param>
        /// <param name="GERENTE_REP_ID">Id do gerente que realizará o rodizio</param>
        public int? EncarteirarSuspectPorRodizio(ClienteDto cliente, int? REGIAO_ID, int? GERENTE_REP_ID = null)
        {
            var representante = _filaCadastroSRV.ObterRepresentantePorRodizio((int) REGIAO_ID); // pego o próximo representante da fila de rodizio da regiao

            if (representante == null)
            {
                throw new OperadoraNaoEncontradaPorRodizioException("Não há operadora logada para rodizio");
            }
            
            if (representante != null && representante.REP_ID != null && cliente != null) // verifico se os dados são válidos
            {
                var REP_ID = (int)representante.REP_ID;
                EncarteirarSuspect(cliente, REP_ID, 1, GERENTE_REP_ID);

                _filaCadastroSRV.MoverFila(REGIAO_ID);

                var nomeRepresentante = representante.REP_NOME;
                var clienteId = (int)cliente.CLI_ID;
                var nomeCliente = cliente.CLI_NOME;

                if (GERENTE_REP_ID != null)
                {
                    _notificacoeSRV.InserirNotificacaoParaGerenteRecebimentoRodizio(nomeRepresentante, nomeCliente, (int)GERENTE_REP_ID, clienteId);
                }
                _notificacoeSRV.InserirNotificacaoRecebimentoRodizio(nomeRepresentante, nomeCliente, REP_ID, clienteId);
            }

            return representante.REP_ID;

        }

        /// <summary>
        /// Remove o carteiramento do cliente
        /// </summary>
        /// <param name="cliente">Cliente que será removido</param>
        /// <param name="CAR_ID">Id da carteira em que o cliente será removido</param>
        public void RemoverCarteiramento(ClienteDto cliente, int? RG_ID)
        {
            if (cliente != null && cliente.CLI_ID != null && RG_ID != null)
            {
                //ar assinatura = _assinaturaSRV.ExtrairAssinaturaFranquiaCliente(cliente, true);
                _carteiraClienteSRV.RemoverCarteiraClientePorClienteERegiao(cliente, RG_ID);
            }
        }

        /// <summary>
        /// Remove o carteiramento do cliente na regiao do representante
        /// </summary>
        /// <param name="cliente">Cliente que será removido</param>
        /// <param name="CAR_ID">Id da carteira em que o cliente será removido</param>
        public void RemoverCarteiramentoDaRegiaoDoRepresentante(ClienteDto cliente, int? REP_ID)
        {
            if (cliente != null && cliente.CLI_ID != null && REP_ID != null)
            {
                var representante = _representanteSRV.FindById(REP_ID);

                if (representante != null && representante.RG_ID != null)
                {
                    var RG_ID = representante.RG_ID;
                    RemoverCarteiramento(cliente, RG_ID);
                }
                //var assinatura = _assinaturaSRV.ExtrairAssinaturaFranquiaCliente(cliente, true);
                
            }
        }

        /// <summary>
        /// Reencarteira o cliente removendo a carteira anterior, em que o cliente está relacionado com a representante, e encarteirando por rodizio.
        /// </summary>
        /// <param name="cliente">Objeto do cliente</param>
        /// <param name="REGIAO_ID">Região em que o cliente será reencarteirado por rodizio</param>
        /// <param name="REP_ID_USUARIO">Id do representante logado no sistema</param>
        /// <param name="usuario">Usuário logado no sistema</param>
        public void ReencarteiraPorRodizio(ClienteDto cliente, int? REGIAO_ID, int? REP_ID_USUARIO, string usuario = null)
        {
            if (cliente != null && REGIAO_ID != null)
            {
                RemoverCarteiramento(cliente, REGIAO_ID);
                _prioridadeAtendimentoSRV.ConfirmarAtendimentoDePrioridadeDaRegiaoEDoCliente((int)REGIAO_ID, (int)cliente.CLI_ID);           
              var REP_ID = EncarteirarSuspectPorRodizio(cliente,(int) REGIAO_ID, REP_ID_USUARIO);
              _historicoAtendimentoService.RegistraHistoricoDeReencarteiramento(usuario, REP_ID_USUARIO, REP_ID, cliente.CLI_ID);
            }               
            
        }

        /// <summary>
        /// Reencarteira o cliente removendo a carteira anterior, em que o cliente está relacionado com o (a) representante, e encarteirando para o representante.
        /// </summary>
        /// <param name="cliente">Objeto do cliente</param>
        /// <param name="REGIAO_ID">Região em que o cliente será reencarteirado por rodizio</param>
        /// <param name="GERENTE_REP_ID">Id do representante gerente que está executando o rodizio</param>
        /// <param name="REP_ID">Id do representante em será desvinculado ao cliente</param> 
        /// /// <param name="REP_ORIGINAL">Id do representante logado no sistema</param>
        public void ReencarteiraParaRepresentante(ClienteDto cliente, int? REP_ID, int? REP_ID_DO_USUARIO_LOGADO, int? RG_ID, string usuario = null)
        {
            if (cliente != null)
            {               
                RemoverCarteiramentoDaRegiaoDoRepresentante(cliente, REP_ID);
                _prioridadeAtendimentoSRV.ConfirmarAtendimentoDePrioridadeDoClientePorRegiaoRepresentante((int)REP_ID, (int)cliente.CLI_ID); 
                EncarteirarSuspect(cliente, (int)REP_ID, 1, REP_ID_DO_USUARIO_LOGADO);                
                _historicoAtendimentoService.RegistraHistoricoDeReencarteiramento(usuario, REP_ID_DO_USUARIO_LOGADO, REP_ID, cliente.CLI_ID);
                
            }

        }

        /// <summary>
        /// Reencarteira o cliente removendo a carteira anterior, em que o cliente está relacionado com outros representantes da região, e encarteira para rodizio na região.
        /// </summary>
        /// <param name="CLI_ID">Id do cliente</param>
        /// <param name="RG_ID">Região em que o cliente será reencarteirado por rodizio</param>
        /// <param name="GERENTE_REP_ID">Id do representante gerente que está executando o rodizio</param>
        /// <param name="REP_ORIGINAL">Id do representante em será desvinculado ao cliente</param>
        public void ReencarteiraPorRodizio(int? CLI_ID , int? RG_ID, int? REP_ID_USUARIO, string usuario = null)
        {
            using (var scope = new TransactionScope())
            {
                if (CLI_ID != null)
                {
                    VerificarSePodeReencarteirar(CLI_ID, RG_ID);
                    var cliente = new ClienteSRV().FindById(CLI_ID);
                    ReencarteiraPorRodizio(cliente, RG_ID, REP_ID_USUARIO, usuario);
                    scope.Complete();                   
                }
            }
        }

        /// <summary>
        /// Reencarteira o cliente removendo a carteira anterior, em que o cliente está relacionado com outros representantes da região, e encarteira para o representante escolhido.
        /// </summary>
        /// <param name="cliente">Objeto do cliente</param>
        /// <param name="REGIAO_ID">Região em que o cliente será reencarteirado por rodizio</param>
        /// <param name="GERENTE_REP_ID">Id do representante gerente que está executando o rodizio</param>
        /// <param name="REP_ORIGINAL">Id do representante em será desvinculado ao cliente</param>
        /// /// <param name="REP_ORIGINAL">Id do representante logado no sistema</param>
        public void ReencarteiraParaRepresentante(int? CLI_ID, int? REP_ID, int? REP_ID_DO_USUARIO_LOGADO, int? RG_ID, string usuario = null)
        {
            if (CLI_ID != null && REP_ID != null)
            {
                using (var scope = new TransactionScope())
                {
                    VerificarSePodeReencarteirarPorRepresentante(CLI_ID, REP_ID);
                    ClienteDto cliente = new ClienteSRV().FindById(CLI_ID);
                    ReencarteiraParaRepresentante(cliente, REP_ID, REP_ID_DO_USUARIO_LOGADO, RG_ID, usuario);
                    scope.Complete();
                }
            }
        }

        /// <summary>
        /// Adiciona uma nova região ao cliente, realizando um rodizio e adicionando a uma representante dessa região
        /// </summary>
        /// <param name="CLI_ID">Id do cliente que será adicionado a região selecionado</param>
        /// <param name="RG_ID">Id da região onde o cliente será adicionada</param>
        /// <param name="REP_ID_LOGADO">Id do representante logado</param>
        public void AdicionarRegiao(int? CLI_ID, int? RG_ID, int? REP_ID_LOGADO = null, string usuario = null)
        {
            if (CLI_ID != null && RG_ID != null)
            {
                using (var scope = new TransactionScope())
                {
                    if (!HasCarteiramentoClienteNaRegiao(CLI_ID, RG_ID))
                    {
                        ClienteDto cliente = new ClienteSRV().FindById(CLI_ID);
                        int? REP_ID = EncarteirarSuspectPorRodizio(cliente, RG_ID, REP_ID_LOGADO);
                        _historicoAtendimentoService.RegistraHistoricoDeAdicionarRegiao(usuario, REP_ID_LOGADO, REP_ID, CLI_ID, RG_ID);

                        scope.Complete();
                    }
                    else
                    {
                        throw new NegocioException("O cliente já está encarteirado na região selecionada.");
                    }

                }
            }
            else
            {
                if (RG_ID == null)
                {
                    throw new NegocioException("Selecione a região.");
                }
            }
        }

        /// <summary>
        /// Verifica se um determinado cliente possui algum carteiramento com um representante da região
        /// selecionada.
        /// </summary>
        /// <param name="CLI_ID">Id do cliente a ser verificado.</param>
        /// <param name="RG_ID">Id da região
        /// </param>
        /// <returns></returns>
        public bool HasCarteiramentoClienteNaRegiao(int? CLI_ID, int? RG_ID)
        {
            return _dao.HasCarteiramentoClienteNaRegiao(CLI_ID, RG_ID);
        }

        public int QtdCarteiramentosCliente(int? CLI_ID)
        {
            return _dao.QtdCarteiramentosCliente(CLI_ID);
        }

        /// <summary>
        /// Quantidade de clientes que estão contidos na carteira
        /// </summary>
        /// <param name="CAR_ID"></param>
        /// <returns></returns>
        public int QtdClienteCarteiramento(string CAR_ID)
        {
            return _dao.QtdClienteCarteiramento(CAR_ID);
        }

        private void VerificarSePodeReencarteirar(int? CLI_ID, int? RG_ID)
        {
            if (!_regiaoService.ClientePossuiRegiao(CLI_ID, RG_ID))
            {
                throw new Exception("O cliente só pode ser reencarteirado em uma região da qual ele já pertença.");
            }
        }

        private void VerificarSePodeReencarteirarPorRepresentante(int? CLI_ID, int? REP_ID)
        {
            var representante = _representanteSRV.FindById(REP_ID);

            if (representante != null && representante.RG_ID != null)
            {
                var RG_ID = representante.RG_ID;
                VerificarSePodeReencarteirar(CLI_ID, RG_ID);

            }
            
        }

        ///// <summary>
        ///// Remove o carteiramento do cliente para a região selecionada.
        ///// </summary>
        ///// <param name="CLI_ID"></param>
        ///// <param name="RG_ID"></param>
        //public void RemoverRegiao(int? CLI_ID, int? RG_ID)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        if (CLI_ID != null && RG_ID != null)
        //        {
        //            var cliente = new ClienteSRV().FindById(CLI_ID);
        //            RemoverCarteiramento(cliente, RG_ID);
        //            _prioridadeAtendimentoSRV.ConfirmarAtendimentoDePrioridadeDaRegiaoEDoCliente(RG_ID, (int)CLI_ID);
        //        }

        //        scope.Complete();
        //    }
        //}


        /// <summary>
        /// Adiciona uma nova região ao cliente, realizando um rodizio e adicionando a uma representante dessa região
        /// </summary>
        /// <param name="CLI_ID">Id do cliente que será adicionado a região selecionado</param>
        /// <param name="RG_ID">Id da região onde o cliente será adicionada</param>
        /// <param name="REP_ID_LOGADO">Id do representante logado</param>
        public void RemoverRegiao(int? CLI_ID, int? RG_ID, int? REP_ID_LOGADO = null, string usuario = null)
        {
            if (CLI_ID != null && RG_ID != null)
            {
                using (var scope = new TransactionScope())
                {
                    if (HasCarteiramentoClienteNaRegiao(CLI_ID, RG_ID))
                    {
                        if (QtdCarteiramentosCliente(CLI_ID) > 1)
                        {
                            ClienteDto cliente = new ClienteSRV().FindById(CLI_ID);
                            RemoverCarteiramento(cliente, RG_ID);
                            _prioridadeAtendimentoSRV.ConfirmarAtendimentoDePrioridadeDaRegiaoEDoCliente(RG_ID, (int)CLI_ID);
                            _historicoAtendimentoService.RegistraHistoricoDeRemocaoDaRegiao(usuario, REP_ID_LOGADO, CLI_ID, RG_ID);

                            scope.Complete();
                        }
                        else
                        {
                            throw new NegocioException("O cliente não pode ser removido se ele só possuir uma carteira.");
                        }
                    }
                    else
                    {
                        throw new NegocioException("O cliente não está encarteirado na região selecionada.");
                    }

                }
            }
            else
            {
                if (RG_ID == null)
                {
                    throw new NegocioException("Selecione a região.");
                }
            }
        }
        
        public void EncarteirarClienteDaImportacao(ClienteDto cliente, int? rgId, ContextoImportacaoDTO contexto, int? repID = null)
        {
            try
            {
                if (!HasCarteiramentoClienteNaRegiao(cliente.CLI_ID, rgId))
                {
                    var existeCarteiramento = _representanteSRV.HasRepresentantesComCarteiramento(rgId);
                    var regiao = _regiaoService.FindById(rgId);
                    var nomeDaRegiao = regiao.RG_DESCRICAO;

                    if (existeCarteiramento == false)
                    {
                        throw new CarteiramentoException(string.Format("Não é possível importar esse cliente. A regiao '{0}' não possui nenhum representante para encarteirar", nomeDaRegiao));
                    }

                    RepresentanteDTO representante = null;

                    if(repID != null)
                    {
                        representante = _representanteSRV.FindById(repID);
                    }
                    else
                    {
                        representante = _filaCadastroSRV.ObterRepresentanteDeImportacaoEOrganizarFila(rgId);
                        _filaCadastroSRV.MoverFilaDeImportacao(rgId);
                        if (representante == null)
                        {
                            throw new CarteiramentoException(string.Format("Não é possível importar esse cliente. Não existe nenhum representante com o código '{0}'", repID));
                        }

                        repID = representante.REP_ID;
                    }

                    if(representante.REP_INATIVO_RODIZIO_IMP == true)
                    {
                        throw new CarteiramentoException(string.Format("Não é possível importar esse cliente. O representante de código '{0}' e nome '{1}' não pode receber suspects por rodizio. Ele está marcado para não receber suspets por rodizo.", repID, representante.REP_NOME));
                    }

                    var nomeRepresentante = representante.REP_NOME;

                    var carteira = GetPrimeiraCarteiraDeFranquiaDoRepresentante(representante);
                    if (carteira == null) // disparo o erro
                    {
                        LevantarErroCarteiramentoAusenciaCarteira(rgId, representante.REP_ID);
                    }

                    IList<CarteiraClienteDTO> lstCarteiraCliente = new List<CarteiraClienteDTO>();
                    _carteiraClienteSRV.PreencherCarteiramento(carteira, cliente, lstCarteiraCliente);
                    _carteiraClienteSRV.SalvarCarteiraClienteEmMassa(lstCarteiraCliente);

                    var importacao = contexto.Importacao;
                    var importacaoSuspect = contexto.ImportacaoSuspect;

                    if (importacao != null)
                    {
                        ServiceFactory.RetornarServico<ImportacaoResultadoRodizioSRV>().AdicionarResultado(importacao.IMP_ID, repID, rgId, 1);
                        string usuario = "sistema";
                        
                        if (!string.IsNullOrWhiteSpace(importacao.USU_LOGIN))
                            usuario = importacao.USU_LOGIN;

                        _historicoAtendimentoService.RegistraHistoricoEncarteiramentoPorImportacao(usuario, repID, cliente.CLI_ID, rgId);
                    }

                    if (importacaoSuspect != null)
                    {
                        string mensagemHistorico = string.Format("O cliente foi encarteirado na regiao {0} e atribuído ao representante {1}", nomeDaRegiao, nomeRepresentante);
                        importacaoSuspect.IMS_ID = 4;
                        ServiceFactory.RetornarServico<ImportacaoHistoricoSRV>().IncluirHistoricoImportacaoSuspect(mensagemHistorico, importacaoSuspect);
                        
                    }
                }
            }
            catch (Exception e)
            {
                throw new CarteiramentoException("Ocorreu um erro ao encarteirar os clientes", e);
            }
        }

        private void LevantarErroCarteiramentoAusenciaCarteira(int? rgId, int? repId)
        {
            string mensagem = @"Não é possível encarteirar o cliente ao representante {{representante}} da regiao {{regiao}}. Esse representante não possui nenhuma carteira.";

            RegistroHistDTO messageSetup = new RegistroHistDTO();
            messageSetup.ParametrosAdicionais.Add("rgId", rgId);
            messageSetup.RepId = repId;

            mensagem = formatterService.FormatarMensagem(messageSetup, mensagem);

            throw new CarteiramentoException(mensagem);
        }

        public IList<CarteiramentoClienteRepresentanteDTO> ListarCarteiramentoDoClientesPorRegiao(IEnumerable<int?> lstIdsClientes, int? rgID)
        {
            return _dao.ListarCarteiramentoDoClientesPorRegiao(lstIdsClientes, rgID);
        }

        public void EncarteirarRepresentantesPorCodCarteirar(int? cliId, string carId)
        {
            var representante = _representanteSRV.ListarRepresentantePorCarteira(carId);

            if (representante != null && cliId != null)
            {

            }
        }

        /// <summary>
        /// Verifica se a carteira existe. Se não axiste cria. <para></para>
        /// Verifica se a carteira já possui associação com o representante. Se não existe associa. <para></para>
        /// Por ultimo, verifica se o cliente está associado a esta carteira. Se não existir associa. <para></para>
        /// </summary>
        public void ResolverEncarteiramento(ClienteDto cliente, int? repDemandante = null)
        {
            
            if(cliente != null && cliente.CARTEIRA_CLIENTE != null)
            {
                var carteirasCliente = cliente.CARTEIRA_CLIENTE;
                int index = 0;

                foreach (var carCli in carteirasCliente)
                {
                    string codigoCarteira = null;
                    RepresentanteDTO representante = null;
                    int? REP_ID = null;

                    if (!string.IsNullOrWhiteSpace(carCli.CAR_ID))
                    {
                        codigoCarteira = carCli.CAR_ID;
                        representante = _representanteSRV.ListarRepresentantePorCarteira(codigoCarteira);

                        if (representante == null)
                        {
                            throw new CarteiramentoException("Não é possível encontrar o representante. Essa carteira pode estar vaga.");
                        }

                        REP_ID = representante.REP_ID;
                    }
                    else
                    {
                        throw new CarteiramentoException("Informe a Carteira");
                    }

                    EncarteirarSuspectPorCarteira(cliente, codigoCarteira, REP_ID, repDemandante, true);
                    if (index == 0)
                    {
                        ServiceFactory
                            .RetornarServico<ClienteSRV>()
                            .SalvarComoProspectado(cliente, REP_ID, codigoCarteira);
                    }
                }
                
            }
        }

        /// <summary>
        /// Checa se um carteiramento existe e cria se necessário.
        /// Depois associa ao representante
        /// </summary>
        /// <param name="repId"></param>
        /// <param name="carId"></param>
        /// <param name="regiaoDesc"></param>
        public void ChecarCriarCarteiraEAssociar(int? repId, string carId, string regiaoDesc)
        {
            if (repId != null && carId != null)
            {
                var carteira = FindById(carId);

                bool checarEncarteiramento = true;
                bool jaEncarteirado = false;

                RegiaoDTO regiao = _regiaoService.EncontrarRegiaoPorNome(regiaoDesc, true);

                if (regiao == null)
                {
                    throw new CarteiramentoException(string.Format("Não é possível encarteirar. A regiao {0} não existe.", regiaoDesc));
                }

                if (carteira == null)
                {
                    var carteiraDTO = new CarteiraDTO()
                    {
                        CAR_ID = carId,
                        REGIAO_UF = regiaoDesc,
                        REGIAO_ID = regiaoDesc,
                        SEQ_REG = "1",
                        AREA_ID = 1,
                        RG_ID = regiao.RG_ID,
                        UEN_ID = regiao.UEN_ID
                    };

                    checarEncarteiramento = false;

                    Save(carteiraDTO);
                }

                if (checarEncarteiramento)
                {
                    jaEncarteirado = _cartRepSrv.HasCarteirasRepresentantes(carId, (int) repId);
                }

                if (!jaEncarteirado)
                {
                    _cartRepSrv.AdicionarCarteiraRepresentante((int)repId, carId);
                }
            }
        }

        /// <summary>
        /// Encarteira o cliente para uma carteira específica.        
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="carId"></param>
        /// <param name="REP_ID"></param>
        /// <param name="REP_ID_DEMANDANTE"></param>
        [MetodoTopLevelReferenciavel]
        public void EncarteirarSuspectPorCarteira(ClienteDto cliente, string carId, int? REP_ID = null, int? REP_ID_DEMANDANTE = null, bool substituirOsComOrigemProspect = false)
        {
            if (cliente == null)
                throw new CarteiramentoException("Não foi possível encarteirar o suspect. Nenhum cliente informado.");

            if (string.IsNullOrWhiteSpace(carId))
                throw new CarteiramentoException("Não foi possível encarteirar o suspect. Nenhuma carteira foi informada.");

            CarteiraDTO carteira = FindById(carId);

            EncarteirarSuspectPorCarteira(cliente, carteira, REP_ID, REP_ID_DEMANDANTE, substituirOsComOrigemProspect);
        }
        
        /// <summary>
        /// Encarteira o cliente para uma carteira específica.
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="carteira"></param>
        /// <param name="REP_ID"></param>
        /// <param name="REP_ID_DEMANDANTE"></param>
        [MetodoTopLevelReferenciavel]
        public void EncarteirarSuspectPorCarteira(ClienteDto cliente, CarteiraDTO carteira, int? REP_ID = null, int? REP_ID_DEMANDANTE = null, bool substituirOsComOrigemProspect = false)
        {
            var representante = _representanteSRV.FindById(REP_ID);

            if (representante != null && representante.REP_ID != null && cliente != null) // verifico se os dados são válidos
            {
                var nomeRepresentante = representante.REP_NOME;
                var clienteId = (int)cliente.CLI_ID;
                var nomeCliente = cliente.CLI_NOME;

                if (carteira != null)
                {
                    if (substituirOsComOrigemProspect)
                        _carteiraClienteSRV.RemoverCarteiraDeProspect(clienteId, carteira.CAR_ID);

                    if(!_carteiraClienteSRV.HasCarteiraCliente(carteira.CAR_ID, clienteId))
                        _carteiraClienteSRV.CriarESalvarCarteiraCliente(carteira, cliente, true);
                }
                else
                {
                    throw new Exception("Não foi possível encarteirar o suspect. A representante não possui nenhuma carteira, ou nenhuma carteira pertence a regiao do representante.");
                }             
                

                if (cliente != null && cliente.CLI_ID != null && REP_ID != null)
                {
                    _prioridadeAtendimentoSRV.RegistrarPrioridade((int) REP_ID, clienteId, 3, "Recebido por Cadastro", REP_ID_DEMANDANTE);
                    _notificacoeSRV.InserirNotificacaoNovoCliente(nomeRepresentante, nomeCliente, (int) REP_ID, clienteId);
                }
            }


        }

        /// <summary>
        /// Checa se a carteira pertence a algum representante
        /// </summary>
        public bool ChecarCarteiraValida(string carId)
        {
            var representante = _representanteSRV.ListarRepresentantePorCarteira(carId);
            return (representante != null) ;
        }

        public bool ChecarCarteiraExiste(string carId)
        {
            return _dao.ChecarCarteiraExiste(carId);
        }

        public Pagina<CarteiraDTO> BuscarCarteiras(
            string CAR_ID = null,
            int? rgId = null,
            int? uenId = null,
            int? repId = null,
            int pagina = 1,
            int registosPorPagina = 7,
            bool associadoARepresentante = false)
        {
            return _dao.BuscarCarteiras(CAR_ID, rgId, uenId, repId, pagina, registosPorPagina, associadoARepresentante);
        }

        public CarteiraDTO FindCarteiraByCliIdAndRepId(int CLI_ID, int REP_ID, int? UEN_ID = null)
        {
            return _dao.FindCarteiraByCliIdAndRepId(CLI_ID, REP_ID, UEN_ID);
        }

        public IList<CarteiraDTO> ListaCarteiraByCliIdAndRepId(int? CLI_ID, int? REP_ID, int? UEN_ID = null)
        {
            return _dao.ListaCarteiraByCliIdAndRepId(CLI_ID, REP_ID, UEN_ID);
        }
        public IList<CarteiraDTO> ListaCarteirasDoClienteERepresentante(int? cliID, int? repID, int? UEN_ID = null)
        {
            return _dao.ListaCarteirasDoClienteERepresentante(cliID, repID, UEN_ID);
        }

    }
}
