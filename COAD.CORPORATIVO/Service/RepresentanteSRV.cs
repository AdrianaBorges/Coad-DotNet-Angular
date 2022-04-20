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
using GenericCrud.Config.DataAttributes;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Model;
using System.Transactions;
using COAD.SEGURANCA.Constants;
using COAD.CORPORATIVO.Model.Dto.Custons.Relatorios;
using Coad.GenericCrud.Exceptions;
using GenericCrud.Config.DataAttributes.Maps;
using COAD.CORPORATIVO.LEGADO.Service;
using GenericCrud.Service;
using COAD.CORPORATIVO.Exceptions;
using COAD.SEGURANCA.Model.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.Notificacoes;
using GenericCrud.Util;
using System.Text.RegularExpressions;
using COAD.SEGURANCA.Service.Interfaces;
using COAD.CORPORATIVO.Model.Dto.FiltersInfo;
using GenericCrud.Models.Filtros;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("REP_ID")]
    public class RepresentanteSRV : GenericService<REPRESENTANTE, RepresentanteDTO, int>
    {
        protected RepresentanteDAO _dao;
        public FilaCadastroSRV _filaCadastroService { get; set; }
        public UsuarioSRV _usuarioSRV { get; set; }
        public IEmailSRV emailSRV { get; set; }
        public CarteiraRepresentanteSRV _carteiraRepresentanteSRV { get; set; }

        [ServiceProperty("REP_ID", Name = "areaConsultoriaRepresentante", PropertyName = "AREA_CONSULTORIA_REPRESENTANTE")]
        protected AreaConsultoriaRepresentanteSRV _areaConsultoriaRepreSRV = new AreaConsultoriaRepresentanteSRV();

        public RepresentanteSRV(RepresentanteDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public RepresentanteSRV()
        {
            this._dao = new RepresentanteDAO();
            this._filaCadastroService = new FilaCadastroSRV();
            this._usuarioSRV = new UsuarioSRV();

            Dao = _dao;
        }

        public IList<RelRepresentanteCarteiraDTO> BuscarRepresentanteCarteira()
        {
            return _dao.BuscarRepresentanteCarteira();
        }

            //public Pagina<RepresentanteDTO> Representantes(string nomeRepresentante = null, string descricaoUf = null, int pagina = 1, int registrosPorPagina = 15)
            //{
            //    return _dao.Representantes(nomeRepresentante, descricaoUf, pagina, registrosPorPagina);
            //}

            /// <summary>
            /// Pega todos os representantes que pertencem a regiao do representante passado
            /// </summary>
            /// <param name="nomeRepresentante"></param>
            /// <param name="REP_ID"></param>
            /// <param name="pagina"></param>
            /// <param name="registrosPorPagina"></param>
            /// <returns></returns>
            public Pagina<RepresentanteDTO> RepresentantesPorRegiaoDoRepresentante(string nomeRepresentante = null, int? REP_ID = null, int pagina = 1, 
            int registrosPorPagina = 15, 
            int? UEN_ID = null,
            bool? gerente = null,
            int? nivelRepresentanteId = null)
        {
            var representante = FindById(REP_ID);

            if(representante != null){

                var RG_ID = representante.RG_ID;
                return Representantes(nomeRepresentante, RG_ID, UEN_ID, gerente, pagina, registrosPorPagina, nivelRepresentanteId: nivelRepresentanteId);
            }
            return new Pagina<RepresentanteDTO>();
        }


        public Pagina<RepresentanteDTO> Representantes(string nomeRepresentante, int? RG_ID, int? UEN_ID, bool? gerente = null, int pagina = 1, 
            int registrosPorPagina = 7, 
            bool uenIdPreenchido = false,
            int? nivelAcesso = null,
            int? nivelRepresentanteId = null)
        {
           return _dao.Representantes(nomeRepresentante, null, pagina, registrosPorPagina, RG_ID, UEN_ID, gerente, uenIdPreenchido, nivelAcesso, nivelRepresentanteId);
        }

        public Pagina<RepresentanteDTO> RepresentantesComUsuario(string nomeRepresentante, int? RG_ID, int? UEN_ID, bool? gerente = null, int pagina = 1, 
            int registrosPorPagina = 7,
            int? nivelAcesso = null,
            int? nivelRepresentanteId = null)
        {
            Pagina<RepresentanteDTO> lstRepresentantes = Representantes(nomeRepresentante, RG_ID, UEN_ID, gerente, pagina, registrosPorPagina, true, nivelAcesso, nivelRepresentanteId);
            PreencherUsuario(lstRepresentantes.lista);

            return lstRepresentantes;
        }

        public IList<RepresentanteDTO> BuscarRepresentantes()
        {
            return _dao.BuscarRepresentantes();
        }
        public void ChecaEInsereFilaRepresentante(int? REP_ID)
        {
            var rep = FindById(REP_ID);

            if (rep != null)
            {
                _filaCadastroService.RegistrarFilaCadastro(rep);
            }
        }
        public IList<RepresentanteDTO> BuscarSupervisores()
        {
            return _dao.BuscarSupervisores();
        }

        public bool RepresentantesExistemNaMesmaRegiao(int? REP_ID1, int? REP_ID2)
        {
            return _dao.RepresentantesExistemNaMesmaRegiao(REP_ID1, REP_ID2);
        }

        public void InserirRepresentante(RepresentanteDTO representante)
        {
            if (representante != null)
            {
                if (representante.REP_ID == null)
                {

                }
            }
        }

        public bool RepresentanteAdmiteVariasCarteiras(int REP_ID)
        {
            return _dao.RepresentanteAdmiteVariasCarteiras(REP_ID);
        }

        public IList<RepresentanteDTO> RepresentantesDaRegiao(int? RG_ID, string CAR_ID_PARA_IGNORAR = null, int? UEN_ID = 1)
        {
            return _dao.RepresentantesDaRegiao(RG_ID, CAR_ID_PARA_IGNORAR, UEN_ID);
        }

        public Pagina<RepresentanteDTO> RepresentantesDaRegiaoPaginado(
            RepresentanteFiltrosDTO filtros)
        {
            var lstRepId = _usuarioSRV.BuscarRepIdDosUsuarios(filtros.login, filtros.nome, filtros.cpf, filtros.cpfExato, null, filtros.query);
            Pagina<RepresentanteDTO> lstRepresentantes = _dao.RepresentantesDaRegiaoPaginado(filtros, lstRepId);
            PreencherUsuario(lstRepresentantes.lista);

            return lstRepresentantes;
        }

        public Pagina<RepresentanteDTO> RepresentantesDaRegiaoPaginado(
            bool naoAssociadosARepresentante,
            bool cpfExato,
            int? UEN_ID = null, 
            string REP_NOME = null,
            string login = null,
            string nome = null,
            string cpf = null,
            string email = null,
            int? REP_ID_SUP = null,
            int pagina = 1, 
            int registrosPorPagina = 12,
            int? rgId = null)
        {
            var lstRepId = _usuarioSRV.BuscarRepIdDosUsuarios(login, nome, cpf, cpfExato, null);
            Pagina<RepresentanteDTO> lstRepresentantes = _dao.RepresentantesDaRegiaoPaginado(UEN_ID, REP_NOME, REP_ID_SUP, pagina, registrosPorPagina, lstRepId, email, rgId);
            PreencherUsuario(lstRepresentantes.lista);

            return lstRepresentantes;
        }

        public IList<RepresentanteDTO> GetRepresentantesLogados(DateTime data, int? regiaoId, string excetoRepId)
        {
            return _dao.GetRepresentantesLogados(data, regiaoId, excetoRepId);
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
        /// <returns></returns>
        public Pagina<RepresentanteDTO> RepresentantesDoCliente(int? CLI_ID, string nomeRepresentante = null, string descricaoUf = null,
            int? RG_ID = null, int? UEN_ID = null, bool? gerente = null,
            int pagina = 1, int registrosPorPagina = 7, int? REP_ID_PARA_EXCLUIR = null)
        {
            return _dao.RepresentantesDoCliente(CLI_ID, nomeRepresentante, descricaoUf, RG_ID, UEN_ID, gerente, pagina, registrosPorPagina, REP_ID_PARA_EXCLUIR);
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

            return _dao.RepresentantesQueNaoSaoDoCliente(CLI_ID, nomeRepresentante, RG_ID, UEN_ID, gerente, pagina, registrosPorPagina);
        }

        public IList<AutoCompleteDTO<string>> RepresentantesDeFranquiaAutoCompleteDTO(int? RG_ID = null, int? UEN_ID = null)
        {
            return _dao.RepresentantesDeFranquiaAutoCompleteDTO(RG_ID, UEN_ID);
        }


        public void PreencherUsuario(RepresentanteDTO representante)
        {
            if (representante != null)
            {
                int? REP_ID = representante.REP_ID;
                UsuarioModel usuario = _usuarioSRV.FindFirstByRepId(REP_ID);
                representante.USUARIO = usuario;
            }
        }

        public void PreencherUsuario(IEnumerable<RepresentanteDTO> representantes)
        {
            if (representantes != null && representantes.Count() > 0)
            {
                foreach (var rep in representantes)
                {
                    PreencherUsuario(rep);
                }
            }
        }

        /// <summary>
        /// Carrega o representante do banco e logo em seguida,
        /// carrega o usuário, no banco de gestão de acessos do sistema,
        /// associado ao representante.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RepresentanteDTO FindByIdWithUser(int? id, bool trazAreaConsultoriaRepresentante = false, bool trazCarteiramentos = false)
        {
            RepresentanteDTO representante = FindById(id);
            PreencherUsuario(representante);

            if (trazAreaConsultoriaRepresentante)
            {
                GetAssociations(representante, "areaConsultoriaRepresentante");
                //new AreaConsultoriaRepresentanteSRV().PreencherAreaConsultoriaRepresentante(representante);
            }

            if (trazCarteiramentos)
            {
                _carteiraRepresentanteSRV.PreencherCarteiraRepresentante(representante);
            }

            return representante;
        }

        /// <summary>
        /// Determina que tipo de nivelRepresentante o usuário irá receber e marca esse tipo
        /// atravéz do atributo PER_ID_A_ATRIBUIR
        /// </summary>
        /// <param name="representante"></param>
        //public void AtribuiTipoPerfilAoUsuarioDoRepresentante(RepresentanteDTO representante)
        //{
        //    if (representante != null && representante.USUARIO != null)
        //    {
        //        var usu = representante.USUARIO;
                
        //        if (representante.NIVEL_REPRESENTANTE != null && !string.IsNullOrWhiteSpace(representante.NIVEL_REPRESENTANTE.PER_ID))
        //        {
        //            usu.PER_ID_A_ATRIBUIR = representante.NIVEL_REPRESENTANTE.PER_ID;
        //        }
        //    }
        //}

        public void SalvarRepresentanteEUsuario(RepresentanteDTO representante, bool update, int? RG_ID = null, int? UEN_ID = 1)
        {
            RepresentanteDTO repNovo = null;
            SenhaDTO senha = null;
            UsuarioModel usuario = null;
            update = true; // Todo Usuario sera atualizado sempre.
            using (var scope = new TransactionScope())
            {              
                if (representante != null)
                {
                    usuario = representante.USUARIO;

                    if (usuario != null)
                    {
                        _usuarioSRV.ValidarUsuarioDisponivel(usuario);
                    }
                    
                    if (representante.REP_ID == null)
                    {
                        representante.REP_OPER_ID = "0000";

                        if(representante.CAR_ID == null)
                            representante.CAR_ID = "0";
                        representante.REP_ATIVO = 1;

                        if (UEN_ID != null)
                        {
                            representante.UEN_ID = UEN_ID;
                        }
                        else if(representante.UEN_ID == null)
                        {
                            throw new NegocioException("Não é possível criar o representante. Informações importantes não puderam ser obtidas automáticamente (uen).");
                        }

                        if (RG_ID != null)
                        {
                            representante.RG_ID = RG_ID;
                        }
                    }

                    repNovo = SaveOrUpdate(representante);

                    if (representante.REP_ID != null)
                    {
                        RemoverUsuarioNaoAssociado(representante);
                    }

                    if (representante.REP_ID == null)
                    {
                        representante.REP_ID = repNovo.REP_ID;
                    }

                    if (usuario != null)
                    {
                        //usuario = representante.USUARIO;
                        usuario.REP_ID = repNovo.REP_ID;
                        
                        string perId = null;

                        if(representante.NIVEL_REPRESENTANTE != null)
                        {
                            perId = representante.NIVEL_REPRESENTANTE.PER_ID;
                        }
                        senha = _usuarioSRV.SalvarUsuarioPorOutraAplicacao(usuario, update, true, perId);                    
                    }

                _carteiraRepresentanteSRV.SalvarEExcluirCarteiraRepresentante(representante);
                        
            }
            
            scope.Complete();

            if (usuario != null && senha != null && senha.SenhaLiteral != null)
            {
                _usuarioSRV.EnviaEmail(usuario.USU_EMAIL, usuario.USU_LOGIN, senha.SenhaLiteral);
            }
               
            }
        }

     
        /// <summary>
        /// Verifica se existe um usuário associado ao representante não deve ser mais associado.
        /// </summary>
        /// <param name="representante"></param>
        protected void RemoverUsuarioNaoAssociado(RepresentanteDTO representante)
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
             if (representante != null)
             {
                int? REP_ID = representante.REP_ID;
                var usuario = _usuarioSRV.FindFirstByRepId(REP_ID);

                if (usuario != null)
                {
                    if (representante.USUARIO == null)
                    {
                        _usuarioSRV.RemoverUsuarioDaFranquia(usuario);
                    }
                    else if (representante.USUARIO != null
                                    && !string.IsNullOrWhiteSpace(representante.USUARIO.USU_LOGIN)
                                    && !representante.USUARIO.USU_LOGIN.Equals(usuario.USU_LOGIN))
                    {
                        _usuarioSRV.RemoverUsuarioDaFranquia(usuario);
                    }
                }

                //    }

                //    scope.Complete();
                //}
            }
        }

        public void DesativarRepresentante(int? REP_ID)
        {
            if (REP_ID != null)
            {
                var rep = FindByIdWithUser(REP_ID);

                if (rep.USUARIO != null)
                {
                    var usuario = rep.USUARIO;
                    _usuarioSRV.RemoverUsuarioDaFranquia(usuario);
                }
                
                rep.REP_ATIVO = 0;
                Merge(rep);
            }
        }

        public Pagina<RelatorioRodizioDTO> RelatorioDePassivos(
            int? uenId, 
            int? regiaoId,
            DateTime dataInicial,
            DateTime? dataFinal = null,
            int pagina = 1, 
            int registrosPorPagina = 100)
        {
            return _dao.RelatorioDePassivos(uenId, regiaoId, dataInicial, dataFinal, 1, registrosPorPagina);
        }

        public Pagina<RelatorioRodizioDTO> RelatorioDePassivosRepresentantesLogados(
            int? uenId, 
            int? regiaoId,
            DateTime dataInicial,
            DateTime? dataFinal = null,
            int pagina = 1, 
            int registrosPorPagina = 100)
        {
            return _dao.RelatorioDePassivosRepresentantesLogados(uenId, regiaoId, dataInicial, dataFinal, pagina, registrosPorPagina);
        }

        public string GetRepOperIdDoRepresentante(int? REP_ID, string carId)
        {
            try
            {
                if (REP_ID == null)
                    throw new ArgumentNullException("O argumento REP_ID é obrigatório.");

                if (string.IsNullOrWhiteSpace(carId))
                    throw new ArgumentNullException("O argumento CAR_ID é obrigatório.");


                var codigoRepresentanteLegado = ServiceFactory.RetornarServico<CarteiraRepresentanteSRV>()
                .RepOperIdDaCarteirasRepresentantes(REP_ID, carId);

                if (codigoRepresentanteLegado == null)
                {
                    string msg = "Não existe carteiramento ou código de representante legado para a carteira {0} e representante {1}. Verifique esse encarteiramento.";
                    msg = string.Format(msg, carId, REP_ID);
                    throw new CarteiramentoException(msg);
                }

                return codigoRepresentanteLegado;
            
            }
            catch(Exception e)
            {
                throw new Exception("Não é possível retornar o código do representante legado.", e);
            }
        }

        public string GetCodRepresentanteNoCoorporativo(int? REP_ID)
        {
            var codOperador = "0000";
            if (REP_ID != null)
            {
                var representante = FindById(REP_ID);

                if (!string.IsNullOrWhiteSpace(representante.REP_COD_CARTEIRA_ANTIGO))
                {
                    var codCarteiraAntigo = representante.REP_COD_CARTEIRA_ANTIGO;
                    var codigo = new RepresentanteLegadoSRV().GetCodOperadorRepresentante(codCarteiraAntigo);

                    if (!string.IsNullOrWhiteSpace(codigo))
                    {
                        return codigo;
                    }
                }
            }

            return codOperador;
        }

        public string GetCodRepresentanteNoCoorporativo(ContextoFaturamentoDTO faturamento)
        {
            if (faturamento != null && faturamento.PEDIDO != null && faturamento.PEDIDO.REP_ID != null)
            {
                var repId = faturamento.PEDIDO.REP_ID;
                return GetCodRepresentanteNoCoorporativo(repId);
            }

            return "0000";
        }

        public IList<RepresentanteDTO> ListarRepresentantesComCarteiramento(int? rgId)
        {
            return _dao.ListarRepresentantesComCarteiramento(rgId);
        }

        public bool HasRepresentantesComCarteiramento(int? rgId)
        {
            return _dao.HasRepresentantesComCarteiramento(rgId);
        }

        public RepresentanteDTO ListarRepresentantePorCarteira(string carId)
        {
            var objRepresentante = _dao.ListarRepresentantePorCarteira(carId);

            //if (objRepresentante == null)
            //{
            //    var representanteLegado = ServiceFactory.RetornarServico<RepresentanteLegadoSRV>().BuscarPorCodigosCarteiramento(carId);

            //    if (representanteLegado != null)
            //    {
            //        int? rgId = null;
            //        int? uenId = null;

            //        var regiao = ServiceFactory.RetornarServico<RegiaoSRV>().EncontrarRegiaoPorNome(representanteLegado.REGIAO, true);

            //        if(regiao != null)
            //        {
            //            rgId = regiao.RG_ID;
            //            uenId = regiao.UEN_ID;
            //        }

            //        objRepresentante = new RepresentanteDTO()
            //        {
            //            AREA_ID = int.Parse(representanteLegado.AREA),
            //            CAR_ID = carId,
            //            NRP_ID = 4,
            //            REGIAO_UF = representanteLegado.REGIAO,
            //            REP_ATIVO = 1,
            //            REP_COD_CARTEIRA_ANTIGO = (carId != null && carId.Length > 4) ? carId.Substring(3, 4) : carId,
            //            REP_NOME = representanteLegado.NOME,
            //            REP_OPER_ID = "0000",
            //            REP_VARIAS_CARTEIRAS = 1,
            //            RG_ID = rgId,
            //            UEN_ID = uenId                       
                        
            //        };

            //        objRepresentante = SaveOrUpdate(objRepresentante);
            //    }
            //}

            return objRepresentante;
        }

        public IList<AutoCompleteDTO<int>> ListarRepresentanteAutocomplete(string nome)
        {
            return _dao.ListarRepresentanteAutocomplete(nome);
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
        /// <returns></returns>
        public Pagina<ProspectRepresentanteDTO> ListarTodosOsRepresentantesDoCliente(int? CLI_ID, string nomeRepresentante = null, string descricaoUf = null,
            int? RG_ID = null, int? UEN_ID = null, bool? gerente = null, int? empId = null,
            int pagina = 1, int registrosPorPagina = 7, int? REP_ID_PARA_EXCLUIR = null)
        {
            return _dao.ListarTodosOsRepresentantesDoCliente(CLI_ID, nomeRepresentante, descricaoUf, RG_ID, UEN_ID, gerente, empId, pagina, registrosPorPagina, REP_ID_PARA_EXCLUIR);
        }

        
        public void NotificarRepresentantePropostaPaga(int? ppiId, bool suprimirErro = false)
        {
            try
            {
                var dados = GerarDadosDeNotificacaoProposptaPaga(ppiId, suprimirErro);
                EnviarEmailNotiPagProposta(dados);
                ServiceFactory.RetornarServico<NotificacoesSRV>().InserirNotiPagPropostaPraRep(dados);                
            }
            catch (Exception e)
            {
                ServiceFactory.RetornarServico<HistoricoExecucaoSRV>().Incluir(new RegistroNotificacaoSistemaDTO()
                {
                    codReferencia = ppiId,
                    data = DateTime.Now,
                    descricaoCodigoReferencia = "Código do Item da Proposta",
                    exception = e,
                    nomeDaExecucao = "Notificação de Proposta Paga para Representante",
                    nomeProjeto = "COADCORP",
                    nomeServico = "RepresentanteSRV",
                    codTipoJob = 5,
                    descricao = string.Format("Ocorreu um erro ao tentar notificar o representante que o item de proposta {0} foi pago.", ppiId),
                });

                if (!suprimirErro)
                    throw e;
            }
        }

        public void NotificarRepresentanteRegistroLiberacao(int? rliId, int? repIdExecutouAcao, string observacoes, bool? aceito)
        {
            var dados = GerarDadosDeNotificacaoRegistroLiberacao(rliId, repIdExecutouAcao, observacoes, aceito);
            EnviarEmailNotiPagRegistroLiberacao(dados);
            ServiceFactory.RetornarServico<NotificacoesSRV>().InserirNotiRegistroLiberacao(dados);

        }

        public NotificacaoPropostaPagaDTO GerarDadosDeNotificacaoProposptaPaga(int? ppiId, bool suprimirErro = false)
        {
            var notificacaoProposta = new NotificacaoPropostaPagaDTO();
            try
            {
                if (ppiId != null)
                {
                    var propostaItem = ServiceFactory.RetornarServico<PropostaItemSRV>().FindById(ppiId);
                    if (propostaItem != null && propostaItem.PROPOSTA != null)
                    {
                        var proposta = propostaItem.PROPOSTA;
                        var repId = proposta.REP_ID;
                        var representante = FindById(repId);
                        var cliente = proposta.CLIENTES;

                        if(cliente == null)
                        {
                            string mensagem = string.Format("Não é possível notificar representante que a proposta {0} foi paga. Os dados do cliente não foram encontrados.", ppiId);
                            throw new NotificacaoException(mensagem);
                        }

                        if (representante == null)
                        {
                            string mensagem = string.Format("Não é possível notificar representante que a proposta {0} foi paga. O representante de código {1} não pode ser encontrado.", ppiId, repId);
                            throw new NotificacaoException(mensagem);
                        }

                        if (string.IsNullOrWhiteSpace(representante.REP_EMAIL))
                        {
                            string mensagem = string.Format("Não é possível notificar por E-Email representante que a proposta {0} foi paga. O representante de código {1} não possui E-Mail cadastrado. Entre no cadastro de representante e cadastre um E-Email.", ppiId, repId);
                            throw new NotificacaoException(mensagem);
                        }

                        notificacaoProposta.Assinatura = propostaItem.ASN_NUM_ASSINATURA;
                        notificacaoProposta.Email = representante.REP_EMAIL;
                        notificacaoProposta.NomeCliente = cliente.CLI_NOME;
                        notificacaoProposta.codItemProposta = ppiId;
                        notificacaoProposta.codProposta = proposta.PRT_ID;
                        notificacaoProposta.RepId = repId;
                        notificacaoProposta.CliId = cliente.CLI_ID;
                        notificacaoProposta.MensagemHTML = string.Format(
                            "O cliente de <strong>Assinatura: </strong> <em>{0}</em> e de <strong>Nome</strong> <em>'{1}'</em> realizou o pagamento da proposta de Item <strong>{2}</strong> e proposta <strong>{3}</strong>",
                            propostaItem.ASN_NUM_ASSINATURA,
                            cliente.CLI_NOME,
                            ppiId,
                            proposta.PRT_ID
                            );

                        notificacaoProposta.Mensagem = Regex.Replace(notificacaoProposta.MensagemHTML, "<.*?>", String.Empty); ;

                    }
                }
            }
            catch (Exception e)
            {
                ServiceFactory.RetornarServico<HistoricoExecucaoSRV>().Incluir(new RegistroNotificacaoSistemaDTO()
                {
                    codReferencia = ppiId,
                    data = DateTime.Now,
                    descricaoCodigoReferencia = "Código do Item da Proposta",
                    exception = e,
                    nomeDaExecucao = "Notificação de Proposta Paga para Representante",
                    nomeProjeto = "COADCORP",
                    nomeServico = "RepresentanteSRV",
                    codTipoJob = 5,
                    descricao = string.Format("Ocorreu um erro ao tentar notificar o representante que o item de proposta {0} foi pago.", ppiId),
                });
                
                if (!suprimirErro)
                    throw e;
            }
            return notificacaoProposta;
        }



        public NotificacaoRegistroLiberacaoDTO GerarDadosDeNotificacaoRegistroLiberacao(int? RLI_ID, int? REP_ID_EXECUTOU_ACAO, string observacoes, bool? aceito)
        {
            var notificacaoProposta = new NotificacaoRegistroLiberacaoDTO();
            notificacaoProposta.Aceito = aceito;

            if (RLI_ID != null && REP_ID_EXECUTOU_ACAO != null)
            {
                var registroLiberacaoSRV = ServiceFactory.RetornarServico<RegistroLiberacaoSRV>();
                var registroLiberacao = registroLiberacaoSRV.FindById(RLI_ID);
                var propostaItem = ServiceFactory.RetornarServico<PropostaItemSRV>().FindByRegistroLiberacao(RLI_ID);
                if (propostaItem != null && propostaItem.PROPOSTA != null)
                {
                    var ppiId = propostaItem.PPI_ID;
                    var proposta = propostaItem.PROPOSTA;
                    var repId = proposta.REP_ID;
                    var representante = FindById(repId);
                    var representanteSupervisor = FindById(REP_ID_EXECUTOU_ACAO);
                    var cliente = proposta.CLIENTES;

                    if (cliente == null)
                    {
                        string mensagem = string.Format("Não é possível notificar representante sobre a liberação/rejeição da proposta {0}. Os dados do cliente não foram encontrados.", ppiId);
                        throw new NotificacaoException(mensagem);
                    }

                    if (representante == null)
                    {
                        string mensagem = string.Format("Não é possível notificar representante sobre a liberação/rejeição da proposta {0}. O representante de código {1} não pode ser encontrado.", ppiId, repId);
                        throw new NotificacaoException(mensagem);
                    }

                    if (string.IsNullOrWhiteSpace(representante.REP_EMAIL))
                    {
                        string mensagem = string.Format("Não é possível notificar por E-Email representante sobre a liberação/rejeição da proposta {0}. O representante de código {1} não possui E-Mail cadastrado. Entre no cadastro de representante e cadastre um E-Email.", ppiId, repId);
                        throw new NotificacaoException(mensagem);
                    }
                    var semantica = (notificacaoProposta.Aceito ==  true) ? "Aprovou" : "Não Aprovou";
                    notificacaoProposta.Assinatura = propostaItem.ASN_NUM_ASSINATURA;
                    notificacaoProposta.Email = representante.REP_EMAIL;
                    notificacaoProposta.NomeCliente = cliente.CLI_NOME;
                    notificacaoProposta.codItemProposta = propostaItem.PPI_ID;
                    notificacaoProposta.codProposta = proposta.PRT_ID;
                    notificacaoProposta.RepId = repId;
                    notificacaoProposta.CliId = cliente.CLI_ID;
                    notificacaoProposta.MensagemHTML = string.Format(
                        "O representante <strong>{0}</strong> <strong><em>{1}</em></strong> a pendência do seguinte item de proposta {2}. Com as seguintes observações: {3}",
                        representanteSupervisor.REP_NOME,
                        semantica,
                        ppiId,
                        observacoes
                        );

                    notificacaoProposta.Mensagem = Regex.Replace(notificacaoProposta.MensagemHTML, "<.*?>", String.Empty); ;

                }
            }
            return notificacaoProposta;
        }


        /// <summary>
        /// Enviar um email e notificação de proposta.
        /// </summary>
        /// <param name="dadosNotificacao"></param>
        public void EnviarEmailNotiPagProposta(NotificacaoPropostaPagaDTO dadosNotificacao)
        {

            var tabelaPedido = @"
            <div>
                <div><strong>Assinatura: </strong>{{codAssinatura}}</div>               
                <div><strong>Cliente: </strong> {{nomeCliente}}</div>                    
                <div><strong>Item de Proposta: </strong>{{codItemProposta}}</div>
                <div><strong>Proposta: </strong>{{codProposta}}</div>
            </div>";


            tabelaPedido = tabelaPedido.Replace("{{codAssinatura}}", dadosNotificacao.Assinatura);
            tabelaPedido = tabelaPedido.Replace("{{nomeCliente}}", dadosNotificacao.NomeCliente);
            tabelaPedido = tabelaPedido.Replace("{{codItemProposta}}", "" + dadosNotificacao.codItemProposta);
            tabelaPedido = tabelaPedido.Replace("{{codProposta}}", "" + dadosNotificacao.codProposta);

            var endEmail = SysUtils.DecidirEnderecoDeEmail(dadosNotificacao.Email);
            if (endEmail != null)
            {
                var url = "https://ci4.googleusercontent.com/proxy/GgWnRPBud6_dbgT5a4AZGD1cXJaq7heSiSI6uRSLpqrbeRczzyf8rGzRft8ARSffAAjCKNryW9c1grWR6aZ4DfbBnsH6SAPgdbI5SsEUK5ISOjmLsiZKwAW0iJfwmKPQF_ufrNjh0VNiRRastLGv7F1SB7KA=s0-d-e1-ft#http://emkt.coad.com.br/emkt/dados/10268/10767/Image/Cursos_Novo/Header_Contabilidade_Geral.png";

                var templateEmail =
                    @"<div style='padding:15px;'>
                        <fieldset style='border:none;'>
                            <legend style='font-size:16px; color: #0970a3;'><strong>Pagamento do Pedido Realizado!!!</strong></legend>
                            <form>
                                <br />
                                <div style='font-size:14px'>
                                    {0}                            
                                </div>

                                <br />
                                <br />
                                {1}
                                <br /> 
                                <br />
                            </form>
                        </fieldset>                    
                    </div>";

                templateEmail = string.Format(templateEmail, dadosNotificacao.MensagemHTML, tabelaPedido);
                emailSRV.EnviarEmailParaCliente(endEmail, string.Format("Item de Proposta {0} pago.", dadosNotificacao.codItemProposta), templateEmail);
            }

        }

        public string RetornarEmailCCRepresentante(int? repId)
        {
            //string emailCC = null;
            //var representante = ServiceFactory.RetornarServico<RepresentanteSRV>().FindById(repId);
            //if (representante != null && !string.IsNullOrWhiteSpace(representante.REP_EMAIL))
            //{
            //    emailCC = SysUtils.DecidirEnderecoDeEmail(representante.REP_EMAIL);
            //}
            //return emailCC;
            return null;
        }
        /// <summary>
        /// Enviar um email e notificação de proposta.
        /// </summary>
        /// <param name="dadosNotificacao"></param>
        public void EnviarEmailNotiPagRegistroLiberacao(NotificacaoRegistroLiberacaoDTO dadosNotificacao)
        {

            var tabelaPedido = @"
            <div>            
                <div><strong>Cliente: </strong> {{nomeCliente}}</div>                    
                <div><strong>Item de Proposta: </strong>{{codItemProposta}}</div>
                <div><strong>Proposta: </strong>{{codProposta}}</div>
            </div>";

            
            tabelaPedido = tabelaPedido.Replace("{{nomeCliente}}", dadosNotificacao.NomeCliente);
            tabelaPedido = tabelaPedido.Replace("{{codItemProposta}}", "" + dadosNotificacao.codItemProposta);
            tabelaPedido = tabelaPedido.Replace("{{codProposta}}", "" + dadosNotificacao.codProposta);

            var endEmail = SysUtils.DecidirEnderecoDeEmail(dadosNotificacao.Email);
            if (endEmail != null)
            {
                var url = "https://ci4.googleusercontent.com/proxy/GgWnRPBud6_dbgT5a4AZGD1cXJaq7heSiSI6uRSLpqrbeRczzyf8rGzRft8ARSffAAjCKNryW9c1grWR6aZ4DfbBnsH6SAPgdbI5SsEUK5ISOjmLsiZKwAW0iJfwmKPQF_ufrNjh0VNiRRastLGv7F1SB7KA=s0-d-e1-ft#http://emkt.coad.com.br/emkt/dados/10268/10767/Image/Cursos_Novo/Header_Contabilidade_Geral.png";

                var templateEmail =
                    @"<div style='padding:15px;'>
                        <fieldset style='border:none;'>
                            <legend style='font-size:16px; color: #0970a3;'><strong>O Status de aprovação/rejeição da proposta foi alterado!!!</strong></legend>
                            <form>
                                <br />
                                <div style='font-size:14px'>
                                    {0}                            
                                </div>

                                <br />
                                <br />
                                {1}
                                <br /> 
                                <br />
                            </form>
                        </fieldset>                    
                    </div>";

                templateEmail = string.Format(templateEmail, dadosNotificacao.MensagemHTML, tabelaPedido);
                emailSRV.EnviarEmailParaCliente(endEmail, string.Format("Status da aprovação/rejeição da proposta {0} foi alterado.", dadosNotificacao.codItemProposta), templateEmail);
            }

        }

        /// <summary>
        /// Retornar somente o nome do representante. Caso o representante não seja encontrado será retornado a string 'Não encontrato' (sem aspas)
        /// Para alterar esse comportamente e retornar null informe true no segundo parâmetro.
        /// </summary>
        /// <param name="repID">Código do Representante</param>
        /// <param name = "returnNullWhenNotFind"> Flag para informar o retorno caso o representante não seja encontrado.</param>
        /// <returns></returns>
        public string RetornarNomeRepresentante(int? repID, bool returnNullWhenNotFind = false)
        {
            string nome = (returnNullWhenNotFind) ? null : "Não encontrato";
            var representante = FindById(repID);
            if (representante != null && !string.IsNullOrWhiteSpace(representante.REP_NOME))
            {
                nome = representante.REP_NOME;
            }
            return nome;
        }

        public ICollection<RepresentanteDTO> ListarRepresentantePorPerfil(string perfil)
        {
            var lstRep = new List<RepresentanteDTO>();
            var lstUsuario = ServiceFactory.RetornarServico<UsuarioSRV>().ListarUsuariosPorPerfil(perfil);

            if(lstUsuario != null && lstUsuario.Count > 0)
            {
                foreach(var usu in lstUsuario)
                {
                    if(usu.REP_ID != null)
                    {
                        var rep = FindById(usu.REP_ID);

                        if(rep != null)
                        {
                            lstRep.Add(rep);
                        }
                    }
                }
            }

            return lstRep;
        }

        public void NotificarDeNotaAntecipada(int? ppiID)
        {
            var notify = ServiceFactory.RetornarServico<NotificacoesSRV>();
            var lstRepre = ListarRepresentantePorPerfil("CONTROLADORIA");

            if(lstRepre != null && lstRepre.Count > 0)
            {
                foreach(var rep in lstRepre)
                {
                    notify.InserirNotificacaoNotaDeAntecipacao(rep.REP_ID, ppiID);
                }
            }
        }


        public NotificacaoPropostaPagaDTO GerarDadosDeNotiPropostaAprovadaCliente(int? ppiId, bool suprimirErro = false)
        {
            var notificacaoProposta = new NotificacaoPropostaPagaDTO();
            try
            {
                if (ppiId != null)
                {
                    var propostaItem = ServiceFactory.RetornarServico<PropostaItemSRV>().FindById(ppiId);
                    if (propostaItem != null && propostaItem.PROPOSTA != null)
                    {
                        var proposta = propostaItem.PROPOSTA;
                        var repId = proposta.REP_ID;
                        var representante = FindById(repId);
                        var cliente = proposta.CLIENTES;

                        if (cliente == null)
                        {
                            string mensagem = string.Format("Não é possível notificar representante que a proposta {0} foi aprovada pelo cliente. Os dados do cliente não foram encontrados.", ppiId);
                            throw new NotificacaoException(mensagem);
                        }

                        if (representante == null)
                        {
                            string mensagem = string.Format("Não é possível notificar representante que a proposta {0} foi aprovada pelo cliente. O representante de código {1} não pode ser encontrado.", ppiId, repId);
                            throw new NotificacaoException(mensagem);
                        }

                        if (string.IsNullOrWhiteSpace(representante.REP_EMAIL))
                        {
                            string mensagem = string.Format("Não é possível notificar por E-Email representante que a proposta {0} foi aprovada pelo cliente. O representante de código {1} não possui E-Mail cadastrado. Entre no cadastro de representante e cadastre um E-Email.", ppiId, repId);
                            throw new NotificacaoException(mensagem);
                        }

                        notificacaoProposta.Assinatura = propostaItem.ASN_NUM_ASSINATURA;
                        notificacaoProposta.Email = representante.REP_EMAIL;
                        notificacaoProposta.NomeCliente = cliente.CLI_NOME;
                        notificacaoProposta.codItemProposta = ppiId;
                        notificacaoProposta.codProposta = proposta.PRT_ID;
                        notificacaoProposta.RepId = repId;
                        notificacaoProposta.CliId = cliente.CLI_ID;
                        notificacaoProposta.MensagemHTML = string.Format(
                            "O cliente de <strong>Assinatura: </strong> <em>{0}</em> e de <strong>Nome</strong> <em>'{1}'</em> aprovou da proposta à prazo de Item <strong>{2}</strong> e proposta <strong>{3}</strong>",
                            propostaItem.ASN_NUM_ASSINATURA,
                            cliente.CLI_NOME,
                            ppiId,
                            proposta.PRT_ID
                            );

                        notificacaoProposta.Mensagem = Regex.Replace(notificacaoProposta.MensagemHTML, "<.*?>", String.Empty); ;

                    }
                }
            }
            catch (Exception e)
            {
                ServiceFactory.RetornarServico<HistoricoExecucaoSRV>().Incluir(new RegistroNotificacaoSistemaDTO()
                {
                    codReferencia = ppiId,
                    data = DateTime.Now,
                    descricaoCodigoReferencia = "Código do Item da Proposta",
                    exception = e,
                    nomeDaExecucao = "Notificação de Proposta Paga para Representante",
                    nomeProjeto = "COADCORP",
                    nomeServico = "RepresentanteSRV",
                    codTipoJob = 5,
                    descricao = string.Format("Ocorreu um erro ao tentar notificar o representante que o item de proposta {0} foi pago.", ppiId),
                });

                if (!suprimirErro)
                    throw e;
            }
            return notificacaoProposta;
        }


        public void NotificarPropostaAprovadaCliente(int? ppiId, bool suprimirErro = false)
        {
            try
            {
                var dados = GerarDadosDeNotiPropostaAprovadaCliente(ppiId, suprimirErro);
                EnviarEmailNotiPagProposta(dados);
                ServiceFactory.RetornarServico<NotificacoesSRV>().InserirNotiPropostaApro(dados);
            }
            catch (Exception e)
            {
                ServiceFactory.RetornarServico<HistoricoExecucaoSRV>().Incluir(new RegistroNotificacaoSistemaDTO()
                {
                    codReferencia = ppiId,
                    data = DateTime.Now,
                    descricaoCodigoReferencia = "Código do Item da Proposta",
                    exception = e,
                    nomeDaExecucao = "Notificação de Proposta Paga para Representante",
                    nomeProjeto = "COADCORP",
                    nomeServico = "RepresentanteSRV",
                    codTipoJob = 5,
                    descricao = string.Format("Ocorreu um erro ao tentar notificar o representante que o item de proposta {0} foi pago.", ppiId),
                });

                if (!suprimirErro)
                    throw e;
            }
        }


    }
}
