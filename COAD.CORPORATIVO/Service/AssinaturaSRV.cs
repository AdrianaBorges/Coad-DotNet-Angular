
using System;
using System.Linq;
using System.Collections.Generic;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.Model.DTO;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Service.Utils;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.LEGADO.Service.CorporativoAntigo;
using System.Transactions;
using COAD.CORPORATIVO.Util;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Extensions;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Service;
using GenericCrud.Util;
using COAD.CORPORATIVO.Exceptions;
using GenericCrud.Config.DataAttributes.Maps;
using COAD.SEGURANCA.Service.Interfaces;
using COAD.CORPORATIVO.Model.Dto.Custons;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Service;
using COAD.CORPORATIVO.Service.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.Historicos;
using COAD.CORPORATIVO.Model.Dto.Custons.WebAPI;
using System.Security.Cryptography;
using GenericCrud.Validations;
using COAD.CORPORATIVO.Service.Base;

namespace COAD.SEGURANCA.Service
{
    [ServiceConfig("ASN_NUM_ASSINATURA")]
    public class AssinaturaSRV : ServiceAdapter<ASSINATURA, AssinaturaDTO, string>
    {
        private AssinaturaDAO _dao;
        public SeqProdSRV _seqSRV { get; set; }
        public TabSeqSRV _tabSeqSRV { get; set; }
        public IEmailSRV _emailSRV { get; set; }
        public ProdutosSRV _proSRV { get; set; }

        [ServiceProperty("ASN_NUM_ASSINATURA", Name= "contrato", PropertyName = "CONTRATOS")]
        protected ContratoSRV _contratoSRV = new ContratoSRV();

        public AssinaturaSRV(AssinaturaDAO _dao)
        {
            this._dao = _dao;
            SetKeys("ASN_NUM_ASSINATURA");
            SetDao(_dao);
        }

        public AssinaturaSRV()
        {
            this._dao = new AssinaturaDAO();
            this._seqSRV = new SeqProdSRV();
            this._tabSeqSRV = new TabSeqSRV();
            this._emailSRV = new EmailSRV();
            
            SetKeys("ASN_NUM_ASSINATURA");
            SetDao(_dao);
        }
        
        public Pagina<AssinaturaDTO> BuscarAssinaturasProtocoladas(int _pagina = 1, int _registroPorPagina = 7)
        {
            return _dao.BuscarAssinaturasProtocoladas(_pagina, _registroPorPagina);
        }
        
        public IList<AssinaturaTransferenciaDTO> BuscarTrasferenciaPorPeriodo(int _mes, int _ano, string _assinatura)
        {
            return _dao.BuscarTrasferenciaPorPeriodo(_mes, _ano, _assinatura);
        }

        public void TrasferirVigencia(TransfAssinaturaCustomDTO _transf)
        {
            new AssinaturaLegadoSRV().TransferirVigencia(_transf.ASN_NUM_ASSINATURA_ANT
                                                        ,_transf.ASN_NUM_ASSINATURA
                                                        ,_transf.SOLICITANTE
                                                        ,_transf.DATA_TRANSF
                                                        ,_transf.CTR_ANO_VIGENCIA
                                                        ,_transf.CTR_NUM_CONTRATO
                                                        ,_transf.MES_REFERENCIA
                                                        ,_transf.CTR_DATA_INI_VIGENCIA
                                                        ,_transf.CTR_DATA_FIM_VIGENCIA
                                                        ,_transf.USU_LOGIN
                                                        ,_transf.MOTIVO);
        }

        public IList<ClienteAcessosDTO> BuscarAcessoClientesPorPeriodo(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, string _asn_num_assinatura = null, int? _pro_id = null)
        {
            return _dao.BuscarAcessoClientesPorPeriodo(_dtini, _dtfim, _asn_num_assinatura, _pro_id);
        }
        public void SalvarAssinatura(AssinaturaDTO _assinatura)
        {

            try
            {
                AssinaturaSenhaSRV _srvAssSenha = ServiceFactory.RetornarServico<AssinaturaSenhaSRV>();

                using (TransactionScope scope = new TransactionScope())
                {

                    this.Merge(_assinatura, "ASN_NUM_ASSINATURA");

                    new ClienteSRV().GravarHistorico(3, _assinatura.CLI_ID, _assinatura.ASN_NUM_ASSINATURA, "Alteração Dados Assinatura", 19);
                    
                    scope.Complete();
                   
                }
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                throw new Exception(SysException.Show(ex));
            }
        }
        public void GerarSenhaSemTranscao(string _assinatura, int _cli_id)
        {
            try
            {
                AssinaturaSenhaSRV _srvAssSenha = new AssinaturaSenhaSRV();
                _dao.GerarSenha(_assinatura);
                new ClienteSRV().GravarHistorico(3, _cli_id, _assinatura, "Alteração de Senha", 26);

            }
            catch (Exception ex)
            {
                if (SessionContext.PossuiSessao())
                {
                    if (SessionContext.login == "" || SessionContext.login == null)
                        SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                    else
                    {
                        Autenticado aut = new Autenticado();
                        aut.USU_LOGIN = SessionContext.login;

                        SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                    }

                }
                throw new Exception(SysException.Show(ex));
            }

        }
        public void GerarSenha(string _assinatura, int _cli_id)
        {

            try
            {
                AssinaturaSenhaSRV _srvAssSenha = new AssinaturaSenhaSRV();

                using (TransactionScope scope = new TransactionScope())
                {

                    _dao.GerarSenha(_assinatura);
                    new ClienteSRV().GravarHistorico(3, _cli_id, _assinatura, "Alteração de Senha", 26);
                    //_srvAssSenha.BloqueiaSenhaAtiva(_assinatura);
                    //_srvAssSenha.GerarSenha(_assinatura);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                throw new Exception(SysException.Show(ex));
            }

        }
        public ASSINATURA RetornarAssinaturaDeCliente(int id)
        {
            return new AssinaturaDAO().RetornarAssinaturaDeCliente(id);
        }

        public IList<AssinaturaDTO> FindByIdLst(IList<string> lstCodAssinatura = null)
        {
            return _dao.FindByIdLst(lstCodAssinatura);
        }

        /// <summary>
        /// Gera o código da clientes
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public string GerarCodAssinatura(int? prodId, int? mes)
        {
            //var transactionOptions = new TransactionOptions();
            //transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            //transactionOptions.Timeout = TransactionManager.MaximumTimeout;

            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            //{

                if (mes != null)
                {
                    var letra = AssinaturaUtil.GetLetraFromMes((int)mes);
                    var codigoAssinatura = BuscarUltimoCodAssinatura(prodId, letra);
                    codigoAssinatura++; 

                    var prodIdStr = (prodId > 9) ? prodId.ToString() : "0" + prodId.ToString();

                    var codigoAssinaturaStr = MathUtil.PreencherZeroEsquerda((int)codigoAssinatura, 4);
                    codigoAssinaturaStr = AssinaturaUtil.CriaDigito(codigoAssinaturaStr);
                    

                    string ASN_NUM_ASSINATURA = prodIdStr + letra + codigoAssinaturaStr;

                    _seqSRV.SalvarSequencia(prodIdStr, letra, codigoAssinatura.ToString());

                    //scope.Complete();
                    return ASN_NUM_ASSINATURA;

                }

            //}
                return null;
        }

        /// <summary>
        /// Pega o ultimo código de clientes gerado
        /// </summary>
        /// <param name="PRO_ID"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public int? BuscarUltimoCodAssinatura(int? PRO_ID, char letra)
        {
            int ultimoCodigo = -1;

            //var transactionOptions = new TransactionOptions();
            //transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            //transactionOptions.Timeout = TransactionManager.MaximumTimeout;

            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            //{
                if (PRO_ID == null)
                {
                    throw new ValidacaoException("Preencha o código do Produto");
                }

            //if (mes == null)
            //{
            //    throw new ValidacaoException("Preencha o código do Produto");
            //}

            var codProd = _seqSRV.GetSeqProduto((int)PRO_ID, letra);

                if (codProd != null)
                {
                    string sequencia = codProd.SEQUENCIA;

                    if (int.TryParse(sequencia, out  ultimoCodigo))
                    {

                        //scope.Complete();
                        return ultimoCodigo;
                    }
                }
            //}

            return null;
        }

        /// <summary>
        /// Retorna Clientes e suas assinaturas
        /// </summary>
        /// 
        public Pagina<AssinaturaDTO> BuscarClientesPorAssinatura(string asn_id = null, string nome = null, int pagina = 1, int registroPorPagina = 10)
        {
            return _dao.BuscarClientesPorAssinatura(asn_id, nome, pagina, registroPorPagina);
        }
        public Pagina<AssinaturaDTO> BuscarPorCliente(int _cliid, int pagina = 1, int registroPorPagina = 20)
        {
            return _dao.BuscarPorCliente(_cliid, pagina, registroPorPagina);
        }
        public IList<QtdeConsultasPeridoDTO> ConsultasPorPeriodo(string _asn_id, Nullable<DateTime> _dataini = null, Nullable<DateTime> _datafim = null)
        {
            QtdeConsultasPeridoDTO _consultas = new QtdeConsultasPeridoDTO();
            IList<QtdeConsultasPeridoDTO> _lista = new List<QtdeConsultasPeridoDTO>();
            List<string> _listuras = new List<string> { "URARJ", "URAPR", "URAMG" };

            AssinaturaDTO _assinatura = this.FindById(_asn_id);

            IList<QtdeConsumoDTO> _lista1 = new HistAtendEmailSRV().BuscarQtdePorAssinatura(_asn_id, _dataini, _datafim);

            DateTime dtiniselec = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
            DateTime dtfimselec = DateTime.Now;

            if (_dataini != null)
            {
                dtiniselec = (DateTime)_dataini;
                dtfimselec = (DateTime)_datafim;
            }


            var _periodo = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString() + " a " + DateTime.Now.ToString("dd/MM/yyyy");


            if (_lista1.Count() > 0)
            {
                foreach (var _item in _lista1)
                {
                    _consultas.codigo = _item.assinatura;
                    _consultas.nome = _item.nome;
                    _consultas.periodo = _item.periodo;
                    _consultas.contratadas = _item.contratadas;
                    _consultas.qtdemail = _item.qtde;
                    _consultas.qtdurarj = 0;
                    _consultas.qtduramg = 0;
                    _consultas.qtdurapr = 0;
                    _consultas.qtdtotal = 0;

                    _lista.Add(_consultas);
                }
            }
            else
            {
                _consultas.codigo = _asn_id;
                if (_assinatura != null)
                    _consultas.nome = _assinatura.CLIENTES.CLI_NOME;
                _consultas.periodo = "";
                _consultas.qtdemail = 0;
                _consultas.qtdurarj = 0;
                _consultas.qtduramg = 0;
                _consultas.qtdurapr = 0;
                _consultas.qtdtotal = 0;

                _lista.Add(_consultas);
            }

            //-------------

            IList<QtdeConsumoDTO> _lista2 = new HistAtendUraSRV().BuscarQtdePorAssinatura(_asn_id, _dataini, _datafim);

            foreach (var _ura_id in _listuras)
            {
                foreach (var _item in _lista2)
                {
                    if (_item.uraid == _ura_id)
                    {
                        switch (_ura_id)
                        {
                            case "URARJ":
                                _lista[0].qtdurarj = _item.qtde;
                                break;

                            case "URAMG":
                                _lista[0].qtduramg = _item.qtde;
                                break;

                            case "URAPR":
                                _lista[0].qtdurapr = _item.qtde;
                                break;
                        }
                    }

                }

            }

            _lista[0].periodo = _periodo;
            _lista[0].qtdtotal = _lista[0].qtdemail +
                                 _lista[0].qtdurarj +
                                 _lista[0].qtduramg +
                                 _lista[0].qtdurapr;

            return _lista.OrderByDescending(x => x.qtdtotal).ToList();
        }
        public IList<QtdeConsultasPeridoDTO> ConsultasPorPeriodo(string _asn_id, string _contrato)
        {
            
            IList<QtdeConsultasPeridoDTO> _lista = new List<QtdeConsultasPeridoDTO>();
            List<string> _listuras = new List<string> { "URARJ", "URAPR", "URAMG" };

            AssinaturaDTO _assinatura = this.FindById(_asn_id);

            IList<QtdeConsumoDTO> _lista1 = new HistAtendEmailSRV().BuscarQtdePorAssinatura(_asn_id, _contrato);

            foreach (var _item in _lista1)
            {
                var _item2 = _lista.Where(x => x.periodo.Trim() == _item.periodo.Trim()).FirstOrDefault();

                if (_item2 == null)
                {
                    QtdeConsultasPeridoDTO _consultas = new QtdeConsultasPeridoDTO();

                    _consultas.codigo = _item.assinatura;
                    _consultas.nome = _item.nome;
                    _consultas.periodo = _item.periodo;
                    _consultas.contratadas = _item.contratadas;
                    _consultas.qtdemail = 0;
                    _consultas.qtdurarj = 0;
                    _consultas.qtduramg = 0;
                    _consultas.qtdurapr = 0;
                    _consultas.qtdtotal = 0;

                    _lista.Add(_consultas);

                    _item2 = _lista.Where(x => x.periodo.Trim() == _item.periodo.Trim()).FirstOrDefault();
                }

                _item2.qtdemail = _item2.qtdemail;
            }
        

            //-------------

            IList<QtdeConsumoDTO> _lista2 = new HistAtendUraSRV().BuscarQtdePorAssinatura(_asn_id, _contrato);

            var i = 0;

            foreach (var _item in _lista2)
            {

                var _item2 = _lista.Where(x => x.periodo.Trim() == _item.periodo.Trim()).FirstOrDefault();

                if (_item2 == null)
                {
                    QtdeConsultasPeridoDTO _consultas = new QtdeConsultasPeridoDTO();

                    _consultas.codigo = _item.assinatura;
                    _consultas.nome = _item.nome;
                    _consultas.periodo = _item.periodo;
                    _consultas.qtdemail = 0;
                    _consultas.qtdurarj = 0;
                    _consultas.qtduramg = 0;
                    _consultas.qtdurapr = 0;
                    _consultas.qtdtotal = 0;

                    _lista.Add(_consultas);

                    _item2 = _lista.Where(x => x.periodo.Trim() == _item.periodo.Trim()).FirstOrDefault();
                }


                _item2.periodo = _item.periodo;

                switch (_item.uraid)
                {
                    case "URARJ":
                        _item2.qtdurarj = _item.qtde;
                        break;

                    case "URAMG":
                        _item2.qtduramg = _item.qtde;
                        break;

                    case "URAPR":
                        _item2.qtdurapr = _item.qtde;
                        break;
                }
             
            }

            foreach (var _item in _lista)
            {

                _item.qtdtotal = _item.qtdemail +
                                 _item.qtdurarj +
                                 _item.qtduramg +
                                 _item.qtdurapr;
            }

            return _lista.OrderBy(x => x.periodo).ToList();
        }
        public Pagina<QtdeConsultasPeridoDTO> ConsultasPorPeriodoPag(string _asn_id, Nullable<DateTime> _dataini = null, Nullable<DateTime> _datafim = null, int pagina = 1, int registroPorPagina = 7)
        {

            IList<QtdeConsultasPeridoDTO> _lista = new List<QtdeConsultasPeridoDTO>();

            AssinaturaDTO _assinatura = this.FindById(_asn_id);

            IList<QtdeConsumoDTO> _lista1 = new HistAtendEmailSRV().BuscarQtdePorAssinatura(_asn_id, _dataini, _datafim);

            DateTime dtiniselec = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
            DateTime dtfimselec = DateTime.Now;

            if (_dataini != null)
            {
                dtiniselec = (DateTime)_dataini;
                dtfimselec = (DateTime)_datafim;
            }


            var _periodo = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString() + " a " + DateTime.Now.ToString("dd/MM/yyyy");


            if (_lista1.Count() > 0)
            {
                foreach (var _item in _lista1)
                {
                    QtdeConsultasPeridoDTO _consultas = new QtdeConsultasPeridoDTO();

                    _consultas.codigo = _item.assinatura;
                    _consultas.nome = _item.nome;
                    _consultas.periodo = _item.periodo;
                    _consultas.contratadas = _item.contratadas;
                    _consultas.qtdemail = 0;
                    _consultas.qtdurarj = 0;
                    _consultas.qtduramg = 0;
                    _consultas.qtdurapr = 0;
                    _consultas.vlrcontrato = 0;
                    _consultas.qtdtotal = _item.qtde;

                    _lista.Add(_consultas);
                }
            }

            //-------------

            IList<QtdeConsumoDTO> _lista2 = new HistAtendUraSRV().BuscarTotalPorAssinatura(_asn_id, _dataini, _datafim);

            foreach (var _item in _lista2)
            {
                QtdeConsultasPeridoDTO _itemlista = _lista.Where(x => x.codigo == _item.assinatura).FirstOrDefault();

                if (_itemlista == null)
                {
                    QtdeConsultasPeridoDTO _consultas = new QtdeConsultasPeridoDTO();

                    _consultas.codigo = _item.assinatura;
                    _consultas.nome = _item.nome;
                    _consultas.periodo = _item.periodo;
                    _consultas.contratadas = _item.contratadas;
                    _consultas.qtdtotal = _item.qtde;

                    _lista.Add(_consultas);
                }
                else
                {
                    _itemlista.codigo = _item.assinatura;
                    _itemlista.nome = _item.nome;
                    _itemlista.periodo = _item.periodo;
                    _itemlista.contratadas = _item.contratadas;
                    _itemlista.qtdtotal += _item.qtde;

                }
            }

            return _lista.OrderByDescending(x => x.qtdtotal).Paginar(pagina, registroPorPagina);
        }

        public AssinaturaDTO GerarAssinaturaFranquia(ClienteDto cliente)
        {
            //if (cliente != null)
            //{
            //    int? CLI_ID = cliente.CLI_ID;

            //    AssinaturaDTO assFranquia = null;

            //    if (cliente.ASSINATURA != null && cliente.ASSINATURA.Where(x => x.ASN_NUM_ASSINATURA == null && x.UEN_ID == 1).FirstOrDefault() != null)
            //    {
            //        assFranquia = ExtrairAssinaturaFranquiaCliente(cliente);
            //    }
            //    else
            //    {
            //        assFranquia = new AssinaturaDTO();
            //        assFranquia.UEN_ID = 1;
            //        cliente.ASSINATURA.Add(assFranquia);
            //    }

            //    assFranquia.ASN_ANO_COAD = "49";
            //    assFranquia.ASN_CORTESIA = 1;
            //    assFranquia.CLI_ID = CLI_ID;

            //    var mes = DateTime.Now.Month;

            //    string codAssin = GerarCodAssinatura(40, mes);

            //    int? seq = _tabSeqSRV.GetSeqAssinatura();

            //    if (seq != null)

            //        if (!string.IsNullOrWhiteSpace(codAssin))
            //        {     
            //            seq++;
            //            assNova.ASN_NUM_ASSINATURA = seq.ToString();
            //            assFranquia.ASN_NUM_ASSINATURA = codAssin;
            //            var clientes = Save(assFranquia);

            //            _tabSeqSRV.AcrescentaSeqAssinatura();
            //            return clientes;
            //        }
            //}

            //return null;

            return GerarAssinatura(cliente, 40);
        }

        public IList<AssinaturaDTO> FindAssinaturaFranquiaPorCliente(int CLI_ID, bool loadTelefones = false, bool loadEmails = false)
        {
            return _dao.FindAssinaturaFranquiaPorCliente(CLI_ID, loadTelefones, loadEmails);
        }

        public IList<AssinaturaDTO> FindAssinaturaPorCliente(int CLI_ID, int? PROD_ID = null, bool loadTelefones = false, bool loadEmails = false)
        {
            return _dao.FindAssinaturaPorCliente(CLI_ID, PROD_ID, loadTelefones, loadTelefones);
        }
        public AssinaturaDTO FindPrimeiraAssinaturaFranquiaPorCliente(int CLI_ID, bool loadTelefones = false, bool loadEmails = false)
        {
            var assinaturas = FindAssinaturaFranquiaPorCliente(CLI_ID, loadTelefones, loadEmails);
            var primeiraAssinatura = assinaturas.FirstOrDefault();

            return primeiraAssinatura;
        }

        public AssinaturaDTO FindPrimeiraAssinaturaPorCliente(int CLI_ID, int? PROD_ID = null, bool loadTelefones = false, bool loadEmails = false)
        {
            var assinaturas = FindAssinaturaPorCliente(CLI_ID, PROD_ID, loadTelefones, loadEmails);
            var primeiraAssinatura = assinaturas.FirstOrDefault();

            return primeiraAssinatura;
        }

        public void PreencherAssinaturaDaFranquia(ClienteDto clientes, bool trazTelefone = false, bool trazEmail = false)
        {
            //if (clientes != null && clientes.CLI_ID != null)
            //{
            //    IList<AssinaturaDTO> clientes = FindAssinaturaFranquiaPorCliente((int) clientes.CLI_ID, trazTelefone, trazEmail);
            //    clientes.ASSINATURA = clientes;
            //}
            PreencherAssinatura(clientes, 40, trazTelefone, trazEmail);
        }

        public void PreencherAssinatura(ClienteDto clientes, int? PROD_ID, bool trazTelefone = false, bool trazEmail = false)
        {
            if (clientes != null && clientes.CLI_ID != null)
            {
                IList<AssinaturaDTO> assinatura = FindAssinaturaPorCliente((int)clientes.CLI_ID, PROD_ID, trazTelefone, trazEmail);
                clientes.ASSINATURA = assinatura;
            }
        }

        public AssinaturaDTO ExtrairAssinaturaFranquiaCliente(ClienteDto cliente, bool carregarSeNaoAchar = false, bool carregarTelefone = false, bool carregarEmail = false)
        {
            if (cliente != null)
            {
                if (cliente.ASSINATURA != null && cliente.ASSINATURA.Count() > 0)
                {
                    AssinaturaDTO assinatura = cliente.ASSINATURA.Where(x => x.UEN_ID == 1).FirstOrDefault();


                    if (assinatura != null)
                    {
                        return assinatura;
                    }
                }
            }
            
            if(carregarSeNaoAchar)
            {
                return FindPrimeiraAssinaturaFranquiaPorCliente((int) cliente.CLI_ID, carregarTelefone, carregarEmail);
            }
            return null;
        }

        public AssinaturaDTO ExtrairAssinaturaCliente(ClienteDto cliente, int? PROD_ID, bool carregarSeNaoAchar = false, bool carregarTelefone = false, bool carregarEmail = false)
        {
            if (cliente != null)
            {
                if (cliente.ASSINATURA != null && cliente.ASSINATURA.Count() > 0)
                {
                    AssinaturaDTO assinatura = cliente.ASSINATURA.Where(x => x.PRO_ID == PROD_ID).FirstOrDefault();
                    
                    if (assinatura != null)
                    {
                        return assinatura;
                    }
                }
            }

            if (carregarSeNaoAchar)
            {
                return FindPrimeiraAssinaturaPorCliente((int)cliente.CLI_ID, PROD_ID, carregarTelefone, carregarEmail);
            }
            return null;
        }

        public AssinaturaDTO FindByIdFullLoaded(string codigoAssinatura, 
            bool trazAssinaturaTelefone = false, 
            bool trazAssinaturaEmail = false, 
            bool trazContrato = false, 
            bool trazClienteCompleto = false)
        {
            if (!string.IsNullOrWhiteSpace(codigoAssinatura))
            {
                AssinaturaDTO assinatura = FindById(codigoAssinatura);

                if (assinatura != null)
                {
                    if (trazAssinaturaTelefone)
                    {
                        new AssinaturaTelefoneSRV().PreencherTelefoneAssinaturaNaAssinatura(assinatura); // criei um instância local para evitar referência circular
                    }
                    if (trazAssinaturaEmail)
                    {
                        new AssinaturaEmailSRV().PreencherEmailAssinaturaNaAssinatura(assinatura); // criei um instância local para evitar referência circular
                    }

                    if (trazContrato)
                    {
                        PreencherContratos(assinatura);
                    }

                    if (trazClienteCompleto)
                    {
                        assinatura.CLIENTES = ServiceFactory
                            .RetornarServico<ClienteSRV>()
                            .FindByIdFullLoaded(assinatura.CLI_ID, false, true, true, true);
                    }
                    else
                    {
                        assinatura.CLIENTES = ServiceFactory
                            .RetornarServico<ClienteSRV>()
                            .FindById(assinatura.CLI_ID);
                    }
                }

                return assinatura;
            }

            return null;
        }

        public void PreencherContratos(AssinaturaDTO assinatura)
        {
            if (assinatura != null)
            {
                _contratoSRV.PreencherContratos(assinatura);
            }
        }

        public AssinaturaDTO GerarAssinatura(ClienteDto cliente, int? PROD_ID, bool importarTelefonesEEmail = false, int? periodoBonus = null, int? mesVigencia = null, int? produto_composicao = null)
        {
            AssinaturaDTO assinatura = null;

            if (cliente != null && PROD_ID != null)
            {
                var aosCuidados = cliente.CLI_A_C;
                int? CLI_ID = cliente.CLI_ID;

                AssinaturaDTO assNova = null;

                if (cliente.ASSINATURA != null && cliente.ASSINATURA.Where(x => x.ASN_NUM_ASSINATURA == null && x.PRO_ID == PROD_ID).FirstOrDefault() != null)
                {
                    assNova = ExtrairAssinaturaCliente(cliente, PROD_ID);
                }
                else
                {
                    assNova = new AssinaturaDTO();
                    assNova.UEN_ID = 1;
                    assNova.CMP_ID = produto_composicao;
                    cliente.ASSINATURA.Add(assNova);
                }

                assNova.ASN_ANO_COAD =  (DateTime.Now.Year - 1966).ToString();
                assNova.ASN_CORTESIA = 0;
                assNova.CLI_ID = CLI_ID;
                assNova.PRO_ID = PROD_ID;
                assNova.ASN_A_C = aosCuidados;

                var mes = (mesVigencia != null) ? (int) mesVigencia : DateTime.Now.Month;

                if(periodoBonus != null)
                {
                    mes += (int) periodoBonus;
                    if(mes > 12)
                    {
                        mes = mes - 12;
                    }
                }

                string codAssin = GerarCodAssinatura(PROD_ID, mes);

                var assinaturaExistente = FindById(codAssin);

                if (assinaturaExistente != null)
                {
                    var erroMSG = @"Não é possível gerar a Assinatura. O código gerado já existe. É possível que a tabela de seguencia esteja desatualizada, ou inconsistente. Cód Gerado '{0}'";
                    erroMSG = string.Format(erroMSG, codAssin);
                    throw new NegocioException(erroMSG);
                }
                //int? seq = _tabSeqSRV.GetSeqAssinatura();

                //if (seq != null)

                if (!string.IsNullOrWhiteSpace(codAssin))
                {
                    //seq++;
                    //assNova.ASN_NUM_ASSINATURA = seq.ToString();
                    assNova.ASN_NUM_ASSINATURA = codAssin;
                    assNova.CMP_ID = produto_composicao;
                    assinatura = Save(assNova);

                    if(importarTelefonesEEmail)
                    {
                        ImportarTelefoneEEmailDoClienteParaAssinatura(cliente, assinatura);
                    }

                    var proSRV = new ProdutosSRV();

                    if (proSRV.ChecarProdutoPodeGeraSenha(PROD_ID))
                    {
                        GerarSenhaEEnviarEmail(codAssin, CLI_ID);
                    }

                    return assinatura;
                }
                
            }

            return assinatura;
        }
        
        public AssinaturaDTO BuscarAssinaturaPorCLIID(int? PROD_ID)
        {
            return _dao.BuscarAssinaturaPorCLIID(PROD_ID);
        }

        public AssinaturaDTO GerarOuAcharAssinaturaFaturamento(ClienteDto cliente, int? PRO_ID, int? tipoVenda = 1, int? periodoBonus = null, int? mesVigencia = null)
        {
            if (cliente != null)
            {
                AssinaturaDTO assinatura = null;
                
                if(tipoVenda == 2)
                    assinatura = ExtrairAssinaturaCliente(cliente, PRO_ID, true, true, true);

                if (assinatura == null)
                {
                    assinatura = GerarAssinatura(cliente, PRO_ID, true, periodoBonus, mesVigencia);
                    GerarAssinaturaLegado(assinatura, cliente);
                }
                else
                {
                    var codAssin = assinatura.ASN_NUM_ASSINATURA;
                    var cliId = cliente.CLI_ID;

                    if (_proSRV.ChecarProdutoPodeGeraSenha(PRO_ID))
                    {
                        ChecarECriarAssinaturaSenha(codAssin, cliId);
                    }
                    ImportarTelefoneEEmailDoClienteParaAssinatura(cliente, assinatura);
                }
                
                return assinatura;
            }
            return null;
        }

        /// <summary>
        /// Verifica se a assinatura possui alguma assinatura senha ativa e caso não possua cria uma nova.
        /// </summary>
        public void ChecarECriarAssinaturaSenha(string codAssinatura, int? cliId)
        {
            var assinatura = FindById(codAssinatura);
            if (_proSRV.ChecarProdutoPodeGeraSenha(assinatura.PRO_ID))
            {
                var senha = ServiceFactory.RetornarServico<AssinaturaSenhaSRV>().BuscarSenhaAtiva(codAssinatura);
                if (senha == null)
                    GerarSenhaEEnviarEmail(codAssinatura, cliId);
            }
        }

        public void GerarAssinaturaLegado(AssinaturaDTO assinatura, ClienteDto cliente)
        {
            if (assinatura != null)
            {
                var clienteLegado = new ClienteSRV().TentarBuscarClienteLegado(cliente);
                var assinaturaLegadoCoorporativo = new AssinaturaLegadoSRV().FindById(assinatura.ASN_NUM_ASSINATURA);

                if (assinaturaLegadoCoorporativo == null && clienteLegado != null && !string.IsNullOrWhiteSpace(clienteLegado.CODIGO))
                {
                    var date = DateTime.Now;
                    var assinaturaLegado = new AssinaturaLegadoDTO()
                    {
                        CODIGO_UNIX = assinatura.ASN_NUM_ASSINATURA,
                        A_C = assinatura.ASN_A_C,
                        ANO_COAD = assinatura.ASN_ANO_COAD,
                        ANO_REMESSA = assinatura.ASN_ANO_REMESSA,
                        CORTESIA = "N",
                        DATA_ASSINATURA = date.ToString("dd/MM/yyyy"),
                        MES_REFERENCIA = date.Month,
                        DATA_INSERT = date,
                        CODIGO = clienteLegado.CODIGO
                    };
                    
                    var lstEmails = _processarCriacaoEmailProspect(cliente);
                    var lstTelefones = _processarCriacaoTelefoneLegado(cliente);

                    assinaturaLegado.EMAILS = lstEmails;
                    assinaturaLegado.TELEFONES2 = lstTelefones;

                    var assinaturaLegadoSRV = new AssinaturaLegadoSRV();
                    assinaturaLegadoSRV.SalvarAssinaturaLegado(assinaturaLegado);

                    SalvarReferenciaAssinatura(assinaturaLegado);

                }
            }
        }


        private IList<Telefones2DTO> _processarCriacaoTelefoneLegado(ClienteDto cliente)
        {
            IList<Telefones2DTO> listaRetorno = new List<Telefones2DTO>();

            if (cliente != null)
            {
                var lstTelefone = cliente.ASSINATURA_TELEFONE;

                if (lstTelefone != null)
                {
                    foreach (var tel in lstTelefone)
                    {
                        Telefones2DTO telProp = new Telefones2DTO();
                        telProp.DDD_TEL = (!string.IsNullOrWhiteSpace(tel.ATE_DDD) ? tel.ATE_DDD : "");
                        telProp.TELEFONE = StringUtil.Truncate(tel.ATE_TELEFONE, 10, true);
                        telProp.IdTelCoadCorp = tel.ATE_ID;

                        if (tel.TIPO_TELEFONE != null)
                        {
                            telProp.TIPO = tel.TIPO_TELEFONE.TIPO_TEL_DESCRICAO;
                        }
                        else
                        {
                            telProp.TIPO = "TELEFONE";
                        }

                        listaRetorno.Add(telProp);
                    }
                }
            }
            return listaRetorno;
        }

        private IList<EmailsDTO> _processarCriacaoEmailProspect(ClienteDto cliente)
        {
            IList<EmailsDTO> listaRetorno = new List<EmailsDTO>();

            if (cliente != null)
            {
                var lstEmail = cliente.ASSINATURA_EMAIL;

                if (lstEmail != null)
                {
                    foreach (var email in lstEmail)
                    {
                        EmailsDTO emailProp = new EmailsDTO();
                        emailProp.E_MAIL = StringUtil.Truncate(email.AEM_EMAIL, 50);
                        emailProp.IdEmailCoadCorp = email.AEM_ID;
                        listaRetorno.Add(emailProp);
                    }
                }
            }
            return listaRetorno;
        }

        public AssinaturaDTO BuscarAssinaturaPorContrato(string _numcontrato)
        {
            return _dao.BuscarAssinaturaPorContrato(_numcontrato);
        }


        /// <summary>
        /// Pega o telefone cadastrado diretamento no cliente e insere na assinatura
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="assinatura"></param>
        public void ImportarTelefoneEEmailDoClienteParaAssinatura(ClienteDto cliente, AssinaturaDTO assinatura)
        {

            //string USU_LOGIN = COAD.SEGURANCA.Repositorios.Base.SessionContext.usu_login_desktop.ToString();
            string USU_LOGIN = "CORPORATIVO";

            AssinaturaTelefoneSRV _assinaturaTelefoneSRV = new AssinaturaTelefoneSRV();
            AssinaturaEmailSRV _assinaturaEmailSRV = new AssinaturaEmailSRV();
            
            _assinaturaTelefoneSRV.PreencherTelefoneAssinaturaNoCliente(cliente);
            _assinaturaEmailSRV.PreencherEmailAssinaturaNoCliente(cliente);

            var lstTelefone = cliente.ASSINATURA_TELEFONE;
            var lstEmail = cliente.ASSINATURA_EMAIL;
            
            _assinaturaTelefoneSRV.CopiarTelefonesParaAssinaturaESalvar(assinatura, lstTelefone, USU_LOGIN);
            _assinaturaEmailSRV.CopiarEmailParaAssinaturaESalvar(assinatura, lstEmail, USU_LOGIN);

        }

        public void SalvarReferenciaAssinatura(AssinaturaLegadoDTO assinatura)
        {
            AssinaturaTelefoneSRV _assinaturaTelefoneSRV = new AssinaturaTelefoneSRV();
            AssinaturaEmailSRV _assinaturaEmailSRV = new AssinaturaEmailSRV();
            
            if (assinatura != null)
            {
                var email = assinatura.EMAILS.Distinct();
                var lstTelefones = assinatura.TELEFONES2.Distinct();

                _assinaturaEmailSRV.SalvarReferenciaDoTelefoneLegado(email);
                _assinaturaTelefoneSRV.SalvarReferenciaDoTelefoneLegado(lstTelefones);
            }
        }

        /// <summary>
        /// Seleciona a senha da assinatura atual e envia por email
        /// </summary>
        /// <param name="codigoAssinatura"></param>
        /// <param name="cliId"></param>
        /// <param name="emailparam"></param>
        public void EnviarSenhaDaAssinaturaPorEmail(string codigoAssinatura, int? cliId, string emailparam=null)
        {
            //emailparam
            var emailContato = ServiceFactory.RetornarServico<AssinaturaEmailSRV>().RetornarEmailDeContato(cliId);
                        
            if (emailContato != null)
            {
                var emailEnvio = emailContato;
                var endEmail = SysUtils.InHomologation() ? SysUtils.RetornaEmailDeTeste() : emailEnvio.AEM_EMAIL;
                var senhaGerada = ServiceFactory.RetornarServico<AssinaturaSenhaSRV>().BuscarPorAssinatura(codigoAssinatura);
                var cliente = ServiceFactory.RetornarServico<ClienteSRV>().FindById(cliId);
                string senha = null;

                if (senhaGerada != null && senhaGerada.ASN_ATIVO)
                {
                    senha = senhaGerada.ASN_SENHA;
                    var url = "http://www.coad.com.br/imagens/coadcorp/Header_Senha.png";

                    var templateEmail =
                        @"<p style='margin-bottom:0cm;line-height:100%'><br></p>
                            <p style='margin-bottom:0cm;line-height:100%'>
                                <br>
                            </p>
                            <p style='margin-bottom:0cm;line-height:100%'>
                                <font color='#000000'><font size='3'>Seja bem vindo(a) à COAD {0}, </font></font>
                            </p>
                            <p style='margin-bottom:0cm;line-height:100%'>
                                <br>
                            </p>
                            <p style='margin-bottom:0cm;line-height:100%'>
                                <font color='#000000'>
                                    <font size='3'>
                                        Seguem abaixo seus dados (login e senha). O acesso ao site será liberado em até 24h.
                                    </font>
                                </font>
                            </p>
                            <p style='margin-bottom:0cm;line-height:100%'>
                                <br>
                            </p>
                            <p style='margin-bottom:0cm;line-height:100%'>
                                <font color='#000000'>
                                    <font size='3'>USUÁRIO: {1}</font>
                                </font>
                            </p>
                            <p style='margin-bottom:0cm;line-height:100%'>
                                <font color='#000000'>
                                    <font size='3'>SENHA:{2}</font>
                                </font>
                            </p>
                            <p style='margin-bottom:0cm;line-height:100%'>
                                <br>
                            </p>
                            <p align='CENTER' style='margin-bottom:0cm;line-height:100%'>
                                <font color='#000000'>
                                    <font color='#365f91'>
                                    <font size='4'>
                                        <b>Seja bem vindo a COAD!</b></font>
                                    </font>
                                </font>
                            </p>
                            <p style='margin-bottom:0cm;line-height:100%'>
                            <br>
                            </p><p style='margin-bottom:0cm;line-height:100%'><br></p>";

                    //var templateEmail2 =
                    //    @"<div style='padding:15px;'>
                    //    <fieldset style='border:none;'>
                    //        <legend style='font-size:16px; color: #0970a3;'><strong>Senha gerada com sucesso!!!</strong></legend>
                    //        <form>
                    //            <br />
                    //                <div style='font-size:14px'>
                    //                Prezado cliente, seu usuário e senha foram gerados com sucesso.
                    //                Verifique logo abaixo seus dados de acesso e comece agora mesmo!!

                                    
                    //            </div>
                    //            <br />
                    //            <br />
                    //            <div style='font-size:15px'>
                    //                <label><strong>Usuário</strong></label>: <em>{0}</em>
                    //            </div>
                    //            <div style='font-size:15px'>
                    //                <label><strong>Senha</strong></label>: <em>{1}</em>
                    //            </div>
                    //        </form>
                    //    </fieldset>
                    
                    //</div>";

                    templateEmail = string.Format(templateEmail, cliente.CLI_NOME, codigoAssinatura, senha);
                    _emailSRV.EnviarEmailParaCliente(endEmail, "Dados de Acesso COAD", templateEmail, url, codSMTP : 2);

                }
            }
        }

        public void GerarSenhaEEnviarEmail(string codigoAssinatura, int? cliId, string email=null)
        {
            GerarSenhaSemTranscao(codigoAssinatura, (int) cliId);
            EnviarSenhaDaAssinaturaPorEmail(codigoAssinatura, cliId, email);
        }


        public void AdicionarConsultaNaAssinatura(string codAssinatura, int? qtdConsultas, string usuLogin)
        {
            var assinatura = FindById(codAssinatura);
            AdicionarConsultaNaAssinatura(assinatura, qtdConsultas, usuLogin);
        }

        public void AdicionarConsultaNaAssinatura(AssinaturaDTO assinatura, int? qtdConsultas, string usuLogin)
        {
            if (assinatura != null && qtdConsultas != null)
            {
                assinatura.ASN_QTDE_CONS_CONTRATO = (int) qtdConsultas;
                SaveOrUpdateNonIdentityKeyEntity(assinatura);

                AdicionarConsultaNaAssinaturaLegada(assinatura, qtdConsultas, usuLogin);
            }
        }

        public void AdicionarConsultaNaAssinaturaLegada(AssinaturaDTO assinatura, int? qtdConsultas, string usuLogin)
        {
            var assinaturaLegadoSRV = ServiceFactory.RetornarServico<AssinaturaLegadoSRV>();
           
            if (assinatura != null)
            {
                var codAssinatura = assinatura.ASN_NUM_ASSINATURA;
                assinaturaLegadoSRV.AdicionarConsultaNaAssinatura(codAssinatura, qtdConsultas, usuLogin);
            }
        }

        public bool TestarSenhaDaAssinatura(string assinatura, string senha)
        {
            return ServiceFactory
                .RetornarServico<AssinaturaSenhaSRV>()
                .TestarSenhaDaAssinatura(assinatura, senha);
        }

        public LoginUnicoAssinaturaDTO BuscarResumoAssinatura(string assinatura)
        {
            return _dao.BuscarResumoAssinatura(assinatura);
        }

        public IList<LoginUnicoAssinaturaDTO> BuscarResumosDeAssinaturasDoCliente(int? cliId, string excetoAssinatura = null, bool marcarAssinaturasComoNativa = false)
        {
            return _dao.BuscarResumosDeAssinaturasDoCliente(cliId, excetoAssinatura, marcarAssinaturasComoNativa);
        }

        public void TransferirAssinatura(ClienteDto cliente, IList<string> lstCodAssinaturas, LoginUnicoRequestDTO loginUnicoRequest)
        {
            var lstAssinatura = FindByIdLst(lstCodAssinaturas);
            TransferirAssinatura(cliente, lstAssinatura, loginUnicoRequest);
        }

        public void TransferirAssinatura(ClienteDto cliente, ICollection<AssinaturaDTO> lstAssinaturas, LoginUnicoRequestDTO loginUnicoRequest)
        {
            var cliId = cliente.CLI_ID;
            var lstClientes = new List<ClienteDto>();

            if (lstAssinaturas != null)
            {
                foreach (var assi in lstAssinaturas)
                {
                    loginUnicoRequest.Rastreamentos.Add(new RastreamentoAlteracaoLoginUnicoDTO() {
                    
                         CodAssinatura = assi.ASN_NUM_ASSINATURA,
                         CodClienteAnterior = assi.CLI_ID,
                         CodClienteRecebido = cliId
                    });

                    if (assi.CLI_ID != cliId)
                    {
                        var clienteAssi = assi.CLIENTES;
                        clienteAssi.DATA_EXCLUSAO = DateTime.Now;
                        assi.CLI_ID = cliId;

                        if (!lstClientes.Select(x => x.CLI_ID).Contains(clienteAssi.CLI_ID))
                        {
                            lstClientes.Add(clienteAssi);
                        }
                    }
                }

                ServiceFactory
                    .RetornarServico<ClienteSRV>()
                    .SaveOrUpdateAll(lstClientes);
                SaveOrUpdateAll(lstAssinaturas);
            }
        }

        /// <summary>
        /// Concede os acessos necessário para o cliente baseado nos produtos comprados
        /// </summary>
        /// <param name="ppiId"></param>
        /// <param name="cliId"></param>
        public void ConcederAcessosDaProposta(int? ppiId, int? cliId, string usuLogin)
        {
            if (ppiId != null && cliId != null)
            {
                IPedidoItem propostaItem = ServiceFactory.RetornarServico<PropostaItemSRV>().FindById(ppiId);
                
                //if(ipeId != null)
                //    itemPedido = ServiceFactory.RetornarServico<ItemPedidoSRV>().FindById(ipeId);
                
                var cliente = ServiceFactory
                .RetornarServico<ClienteSRV>().FindById(cliId);
                ConcederAcessosDoPedido(propostaItem, cliente, usuLogin);
            }
        }
        /// <summary>
        /// Concede os acessos necessário para o cliente baseado nos produtos comprados
        /// </summary>
        /// <param name="ipeId"></param>
        /// <param name="cliId"></param>
        public void ConcederAcessosDoPedidoItem(int? ipeId, int? cliId, string usuLogin)
        {
            if (ipeId != null && cliId != null)
            {   
                IPedidoItem itemPedido = ServiceFactory
                    .RetornarServico<ItemPedidoSRV>().FindById(ipeId);

                var cliente = ServiceFactory
                .RetornarServico<ClienteSRV>().FindById(cliId);


                ConcederAcessosDoPedido(itemPedido, cliente, usuLogin);
            }
        }

        public AssinaturaDTO GerarAssinaturaTransferencia(int? cliId, ProcessoTransferenciaAssinaturaDTO transferenciaDTO)
        {
            HistoricoConcessaoAcessoDTO histDTO = new HistoricoConcessaoAcessoDTO()
            {
                SemanticaAcao = "Transferência de Assinatura",
                AcaId = 22,
            };

            var cmpId = transferenciaDTO.CodProduto;
            var cliente = ServiceFactory.RetornarServico<ClienteSRV>().FindById(cliId);
            var periodoBonus = transferenciaDTO.AcrescimoNoMes;
            var assinaturaAntiga = transferenciaDTO.CodAssinaturaOrigem;
            int? mesVigencia = null;

            if (!string.IsNullOrWhiteSpace(assinaturaAntiga))
            {   var letra = assinaturaAntiga.Substring(2, 1);

                    if (!string.IsNullOrWhiteSpace(letra))
                    {
                        mesVigencia = AssinaturaUtil.GetNumeroDaLetraIndice1(letra.ToCharArray()[0]);                        
                    }
            }

            var assinatura = GerarOuRetornarAssinaturaVenda(cmpId, cliente, histDTO, 1, periodoBonus, mesVigencia);

            return assinatura;
        }

        /// <summary>
        /// Concede os acessos necessário para o cliente baseado nos produtos comprados
        /// </summary>
        /// <param name="pedidoItem"></param>
        /// <param name="cliente"></param>
        public void ConcederAcessosDoPedido(IPedidoItem pedidoItem, ClienteDto cliente, string usuLogin, bool gerarSenha = true)
        {
            if (pedidoItem.AcessosConcedidos != true)
            {
                pedidoItem.AcessosConcedidos = true;
                int? ppiId = null;
                int? ipeId = null;
                int? qtdConsulta = 0;
                int? tipoDeVenda = null;
                int? mesVigencia = null;
                int? periodoBonus = null;
                string assinaturaCancelamento = null;
                int? repIdEmitente = null;
                int? pstId = null;

                PropostaItemDTO propostaItemConcessao = null;
                ItemPedidoDTO pedidoItemConcessao = null;

                AssinaturaDTO assinatura = null;

                if (pedidoItem is PropostaItemDTO)
                {
                    var propostaItem = pedidoItem as PropostaItemDTO;
                    propostaItemConcessao = propostaItem;
                    ServiceFactory.RetornarServico<PropostaItemSRV>().Merge(propostaItem);
                    ppiId = propostaItem.PPI_ID;
                  
                    if (propostaItem != null &&
                        propostaItem.PROPOSTA != null &&
                        propostaItem.PROPOSTA.TPP_ID != null)
                    {
                        tipoDeVenda = propostaItem.PROPOSTA.TPP_ID;
                        periodoBonus = propostaItem.PPI_PERIODO_MES_BONUS;
                        assinaturaCancelamento = propostaItem.PPI_ASN_NUM_ASS_CANC;
                        pstId = propostaItem.PST_ID;

                        repIdEmitente = (propostaItem.PROPOSTA.REP_ID_EMITENTE != null) ? 
                            propostaItem.PROPOSTA.REP_ID_EMITENTE : 
                            propostaItem.PROPOSTA.REP_ID;

                        if (propostaItem.PROPOSTA.TPP_ID != 1 && 
                            propostaItem.PROPOSTA.TPP_ID != 3 && 
                            propostaItem.ASSINATURA != null)
                        {
                            assinatura = propostaItem.ASSINATURA;
                            if(propostaItem.PPI_QTD_CONSULTA != null && propostaItem.PPI_QTD_CONSULTA > 0)
                            {
                                qtdConsulta = propostaItem.PPI_QTD_CONSULTA;
                            }
                        }
                        else if (propostaItem.PROPOSTA.TPP_ID == 3 && 
                            !string.IsNullOrWhiteSpace(propostaItem.PPI_ASN_NUM_ASS_CANC))
                        {
                            mesVigencia = AssinaturaUtil.ExtrairMesDaAssinatura(propostaItem.PPI_ASN_NUM_ASS_CANC);
                        }

                    }
                    
                }
                else if(pedidoItem is ItemPedidoDTO)
                {
                    var itemPedido = pedidoItem as ItemPedidoDTO;
                    pedidoItemConcessao = itemPedido;
                    ServiceFactory.RetornarServico<ItemPedidoSRV>().Merge(itemPedido);
                    ipeId = itemPedido.IPE_ID;
                    
                    
                    if (itemPedido != null &&
                        itemPedido.PEDIDO_CRM != null &&
                        itemPedido.PEDIDO_CRM.TPD_ID != null)
                    {

                        tipoDeVenda = itemPedido.PEDIDO_CRM.TPD_ID;
                        periodoBonus = itemPedido.IPE_PERIODO_MES_BONUS;
                        assinaturaCancelamento = itemPedido.IPE_ASN_NUM_ASS_CANC;
                        pstId = itemPedido.PST_ID;

                        repIdEmitente = (itemPedido.PEDIDO_CRM.REP_ID_EMITENTE != null) ?
                            itemPedido.PEDIDO_CRM.REP_ID_EMITENTE :
                            itemPedido.PEDIDO_CRM.REP_ID;

                        if (itemPedido.PEDIDO_CRM.TPD_ID != 1 && 
                            itemPedido.PEDIDO_CRM.TPD_ID != 3 && 
                            itemPedido.ASSINATURA != null)
                        {
                            assinatura = itemPedido.ASSINATURA;
                            if(itemPedido.IPE_QTD_CONSULTA != null && itemPedido.IPE_QTD_CONSULTA > 0)
                            {
                                qtdConsulta = itemPedido.IPE_QTD_CONSULTA;
                            }
                        }
                        else if (itemPedido.PEDIDO_CRM.TPD_ID == 3 &&
                            !string.IsNullOrWhiteSpace(itemPedido.IPE_ASN_NUM_ASS_CANC))
                        {
                            mesVigencia = AssinaturaUtil.ExtrairMesDaAssinatura(itemPedido.IPE_ASN_NUM_ASS_CANC);
                        }
                    }
                }
              
                // Geração de uma nova assinatura

                HistoricoConcessaoAcessoDTO histDTO = new HistoricoConcessaoAcessoDTO()
                {
                    SemanticaAcao = pedidoItem.NomeTipo,
                    Usuario = usuLogin,
                    PstId = pedidoItem.PstId,
                    PpiId = ppiId,
                    IpeId = ipeId,
                    AcaId = 22,
                };

                ProdutosSRV proSRV = ServiceFactory.RetornarServico<ProdutosSRV>();

                if (assinatura == null)
                {
                    assinatura = GerarOuRetornarAssinaturaVenda(pedidoItem.CmpId, cliente, histDTO, tipoDeVenda, periodoBonus, mesVigencia);
                    if(propostaItemConcessao != null && 
                        string.IsNullOrWhiteSpace(propostaItemConcessao.ASN_NUM_ASSINATURA))
                    {
                        propostaItemConcessao.ASN_NUM_ASSINATURA = assinatura.ASN_NUM_ASSINATURA;
                        ServiceFactory.RetornarServico<PropostaItemSRV>().Merge(propostaItemConcessao);
                    }
                    else if(pedidoItemConcessao != null && string.IsNullOrWhiteSpace(pedidoItemConcessao.ASN_NUM_ASSINATURA))
                    {
                        pedidoItemConcessao.ASN_NUM_ASSINATURA = assinatura.ASN_NUM_ASSINATURA;
                        ServiceFactory.RetornarServico<ItemPedidoSRV>().Merge(pedidoItemConcessao);
                    }
                }
                else
                {
                    ChecarECriarAssinaturaSenha(assinatura.ASN_NUM_ASSINATURA, cliente.CLI_ID);
                }
                
                if (!proSRV.ChecaProdutoEhCurso(assinatura.PRO_ID))
                {
                    if (qtdConsulta != null && qtdConsulta > 0)
                        AdicionarConsultaNaAssinatura(assinatura, qtdConsulta, usuLogin);
                    else
                        AdicionarConsultasAPartirDoProduto(assinatura, pedidoItem.CmpId, usuLogin, histDTO);
                }

                ServiceFactory.RetornarServico<HistoricoNotificacaoSRV>().RegistrarHistoricoAcessosDoClienteConcedidos(histDTO);

                //Cancelmento de pedidos de Migração

                if (tipoDeVenda == 3)
                {
                    //if (string.IsNullOrWhiteSpace(assinaturaCancelamento))
                    //    throw new PedidoException("Não é possível cancelar a assinatura. O(A) proposta/pedido está marcado como migração, mas a assinatura de origem não foi informada.");

                    //HistoricoTransAssinaturaDTO histTransf = new HistoricoTransAssinaturaDTO()
                    //{
                    //    assinaturaAnterior = assinaturaCancelamento,
                    //    assinaturaNova = assinatura.ASN_NUM_ASSINATURA,
                    //    CLI_ID = cliente.CLI_ID,
                    //    ipeId = ipeId,
                    //    ppiId = ppiId,
                    //    REP_ID_EXECUTOU_A_ACAO = repIdEmitente,
                    //    usuario = usuLogin,
                    //    pstId = pstId

                    //};

                    //CancelarAssinaturaPropostaMigracao(assinaturaCancelamento, histTransf);

                    _contratoSRV.TransferirContratosDaAssinatura(assinaturaCancelamento, assinatura.ASN_NUM_ASSINATURA);
                }
            }

        }


        /// <summary>
        /// Procura a assinatura do produto no qual o pedido se refere. Se não achar, cria outra assinatura
        /// </summary>
        /// <param name="ppiId"></param>
        /// <param name="cliId"></param>
        /// <returns></returns>
        public AssinaturaDTO GerarOuRetornarAssinaturaParaProposta(int? cmpId, int? cliId, HistoricoConcessaoAcessoDTO histDTO,
            int? tipoVenda = 1,
            int? periodoBonus = null)
        {
            if (cmpId != null && cliId != null)
            {
                var cliente = ServiceFactory.RetornarServico<ClienteSRV>().FindById(cliId);
                return GerarOuRetornarAssinaturaVenda(cmpId, cliente, histDTO, tipoVenda, periodoBonus);
            }

            return null;
        }


        /// <summary>
        /// Procura a assinatura do produto no qual o proposta/pedido se refere. Se não achar, cria outra assinatura. Além disso, se o tipoVenda for 
        /// igual a (um) 1 não procura a assinatura, simplesmente cria outra assinatura.
        /// </summary>
        /// <param name="cmpId">Código do Produto Composto</param>
        /// <param name="cliente">DTO do cliente</param>
        /// <param name="histDTO">DTO histórico de acesso</param>
        /// <param name="tipoVenda">Se o tipo de venda for iqual a 1 (um) sempre cria uma assinatura nova.</param>
        /// <param name="periodoBonus">Adiciona a data inicial da assinatura (em caso de uma nova assinatura) de acordo com o período (em meses).</param>
        /// <returns></returns>
        public AssinaturaDTO GerarOuRetornarAssinaturaVenda(int? cmpId, 
            ClienteDto cliente, 
            HistoricoConcessaoAcessoDTO histDTO, 
            int? tipoVenda = 1, 
            int? periodoBonus = null,
            int? mesVigencia = null)
        {
            var proId = ServiceFactory
                .RetornarServico<ProdutoComposicaoItemSRV>()
                .ObterProIdParaGerarAssinatura(cmpId);

            if (histDTO != null)
                histDTO.CodProduto = proId;

            var assinatura = GerarOuAcharAssinaturaFaturamento(cliente, proId, tipoVenda, periodoBonus, mesVigencia);
            return assinatura;
        }


        public void AdicionarConsultasAPartirDoProduto(
            AssinaturaDTO assinatura,
            int? cmpId,
            string usuLogin,
            HistoricoConcessaoAcessoDTO histDTO)
        {
            var qtdConsultas = ServiceFactory.RetornarServico<ProdutoComposicaoSRV>()
                .RetornaQuantidadeDeConsultasDoProdutoComposto(cmpId);

            if (histDTO != null)
                histDTO.QtdConsulta = qtdConsultas;
            AdicionarConsultaNaAssinatura(assinatura, qtdConsultas, usuLogin);

        }

        public int? RetornarIdClienteDaAssinatura(string codAssinatura)
        {
            return _dao.RetornarIdClienteDaAssinatura(codAssinatura);
        }


        public AssinaturaDTO RetornarAssinaturaDoPedido(int? ipeId)
        {
            return _dao.RetornarAssinaturaDoPedido(ipeId);
        }
        public Pagina<AssinaturaDTO> ListarMateriaAdicional(string _asn_num_assinatura, int pagina = 1, int registroPorPagina = 7)
        {
            return _dao.ListarMateriaAdicional(_asn_num_assinatura, pagina, registroPorPagina);
        }
        
      
        public void CancelarAssinaturaPropostaMigracao(string codAssinatura, HistoricoTransAssinaturaDTO histTransAssi)
        {
            //if (codAssinatura != null)
            //{
            //    CancelarAssinaturaLegado(codAssinatura);
            //    ServiceFactory
            //        .RetornarServico<AssinaturaSenhaSRV>()
            //        .DeletarAssinaturaSenha(codAssinatura);


            //    _contratoSRV.CancelarTodosOsContratosDaAssinatura(codAssinatura);

            //    if (histTransAssi != null)
            //    {

            //        ServiceFactory.RetornarServico<HistoricoNotificacaoSRV>()
            //            .RegistrarHistoricoTransferenciaAssinaturaDaProposta(histTransAssi);
            //    }
            //}
        }

        public void CancelarAssinaturaDoItemPedido(ItemPedidoDTO itemPedido, AlteracaoStatusDTO alteracaoStatus = null)
        {
            int? ipeId = itemPedido.IPE_ID;
            var assinatura = RetornarAssinaturaDoPedido(ipeId);
            if(assinatura != null)
            {
                var cliId = assinatura.CLI_ID;
                var codAssinatura = assinatura.ASN_NUM_ASSINATURA;
                var pedido = ServiceFactory.RetornarServico<PedidoCRMSRV>().FindById(itemPedido.PED_CRM_ID);
                if(pedido != null && pedido.TPD_ID == 1)
                {
                    CancelarAssinaturaLegado(codAssinatura);
                }

                if (itemPedido.PST_ID == 3 || (!string.IsNullOrWhiteSpace(itemPedido.ASN_NUM_ASSINATURA) && itemPedido.PEDIDO_CRM.TPD_ID == 1))
                {
                    ServiceFactory
                        .RetornarServico<AssinaturaSenhaSRV>()
                        .DeletarAssinaturaSenha(codAssinatura);
                }

                _contratoSRV.CancelarContratoDaAssinaturaEPedido(ipeId, codAssinatura);

                if (alteracaoStatus != null)
                {
                    var repId = alteracaoStatus.REP_ID;
                    var login = alteracaoStatus.USU_LOGIN;
                    var observacoes = alteracaoStatus.MOTIVO_ALTERACAO;

                    ServiceFactory.RetornarServico<HistoricoNotificacaoSRV>()
                        .RegistrarHistoricoCancelamentoDoPedidoEAssinatura(login, repId, cliId, ipeId, assinatura.ASN_NUM_ASSINATURA, observacoes);
                }
            }
        }

        /// <summary>
        /// Cancela a Assinatura junto com todos os seus Contratos, Parcelas, e Senha. 
        /// Esse método não faz parte de qualquer processamento de cancelamento ou migração de pedido ou proposta.
        /// É um cancelamento direto da assinatura.
        /// </summary>
        public void CancelarAssinatura(string assinatura, AlteracaoStatusDTO alteracaoStatus = null)
        {
            var cliId = RetornarIdClienteDaAssinatura(assinatura);

            if (!string.IsNullOrWhiteSpace(assinatura))
            {
                CancelarAssinaturaLegado(assinatura);
                ServiceFactory
                       .RetornarServico<AssinaturaSenhaSRV>()
                       .DeletarAssinaturaSenha(assinatura);

                _contratoSRV.CancelarTodosOsContratosDaAssinatura(assinatura);

                if(alteracaoStatus != null)
                {
                    var repId = alteracaoStatus.REP_ID;
                    var login = alteracaoStatus.USU_LOGIN;
                    var observacoes = alteracaoStatus.MOTIVO_ALTERACAO;

                    ServiceFactory.RetornarServico<HistoricoNotificacaoSRV>()
                        .RegistrarHistoricoCancelamentoAssinatura(login, repId, cliId, assinatura, observacoes);
                }
            }
        }

        public string MigrarAssinatura(ProcessoTransferenciaAssinaturaDTO transferenciaDTO)
        {
            try
            {


                string codNovoDaAssinatura = "Não gerado";
                using (var scope = new TransactionScope())
                {
                    if (transferenciaDTO == null)
                        throw new ArgumentNullException("Informe o objeto de Transferência");

                    var codAssinatura = transferenciaDTO.CodAssinaturaOrigem;

                    var cmpId = transferenciaDTO.CodProduto;

                    var codContrato = transferenciaDTO.CodContrato;

                    var cliId = RetornarIdClienteDaAssinatura(codAssinatura);

                    var assinaturaNova = GerarAssinaturaTransferencia(cliId, transferenciaDTO);

                    transferenciaDTO.NovoCodAssinatura = assinaturaNova.ASN_NUM_ASSINATURA;
                    codNovoDaAssinatura = assinaturaNova.ASN_NUM_ASSINATURA;

                    _contratoSRV.TransferirUltimoContratoDaAssinatura(codAssinatura, assinaturaNova.ASN_NUM_ASSINATURA, transferenciaDTO);

                    int? ppiId = null;
                    int? ipeId = null;
                    AssinaturaDTO assinaturaAntiga = null;

                    if (!string.IsNullOrWhiteSpace(codAssinatura))
                    {
                        assinaturaAntiga = FindById(codAssinatura);
                        assinaturaAntiga.ASN_NUM_ASS_TRANSFERIDA = codNovoDaAssinatura;
                        assinaturaAntiga.ASN_TRANSFERIDA = true;
                        SaveOrUpdateNonIdentityKeyEntity(assinaturaAntiga);
                    }

                    PropostaItemDTO propostaItem = 
                        ServiceFactory
                           .RetornarServico<PropostaItemSRV>()
                           .ListarPropostaItemDaAssinatura(codAssinatura);

                    if (propostaItem != null)
                        ppiId = propostaItem.PPI_ID;

                    ItemPedidoDTO itemPedido =
                        ServiceFactory
                           .RetornarServico<ItemPedidoSRV>()
                           .ListarItemPedidoDaAssinatura(codAssinatura);

                    if (itemPedido != null)
                        ipeId = itemPedido.IPE_ID;


                    CancelarAssinaturaLegado(codAssinatura);
                    ServiceFactory
                           .RetornarServico<AssinaturaSenhaSRV>()
                           .DeletarAssinaturaSenha(codAssinatura);

                    if(assinaturaAntiga != null && assinaturaNova != null)
                    {
                        assinaturaNova.ASN_QTDE_CONS_CONTRATO = assinaturaAntiga.ASN_QTDE_CONS_CONTRATO;
                        assinaturaNova.ASN_QTDE_CONS_ADICIONAL = assinaturaAntiga.ASN_QTDE_CONS_ADICIONAL;
                        assinaturaNova.ASN_QTDE_CONS_UTILIZADA = assinaturaAntiga.ASN_QTDE_CONS_UTILIZADA;

                        SaveOrUpdateNonIdentityKeyEntity(assinaturaNova);
                    }
                    var histDTO = new HistoricoTransAssinaturaDTO()
                    {
                        assinaturaAnterior = transferenciaDTO.CodAssinaturaOrigem,
                        assinaturaNova = transferenciaDTO.NovoCodAssinatura,
                        CLI_ID = cliId,
                        observacoes = transferenciaDTO.Observacoes,
                        REP_ID_EXECUTOU_A_ACAO = transferenciaDTO.RepId,
                        usuario = transferenciaDTO.Login,
                        PeriodoBonus = transferenciaDTO.AcrescimoNoMes,
                        ipeId = ipeId,
                        ppiId = ppiId
                    };

                    ServiceFactory.RetornarServico<HistoricoNotificacaoSRV>()
                        .RegistrarHistoricoTransferenciaAssinatura(histDTO);

                    scope.Complete();
                }
                return codNovoDaAssinatura;
            }
            catch(Exception e)
            {
                throw new Exception("Não é possível transferir a assinatura", e);
            }
        }

        public void CancelarAssinaturaLegado(string assinatura)
        {
            if (!string.IsNullOrWhiteSpace(assinatura))
            {
                var assinaturaLegadoSRV = ServiceFactory.RetornarServico<AssinaturaLegadoSRV>();
                var assLegado = assinaturaLegadoSRV.FindById(assinatura);
                if(assLegado != null)
                {
                    assinaturaLegadoSRV.Delete(assLegado);
                }
            }
        }

        /// <summary>
        /// Gera uma assinatura com contrato sem pedido
        /// </summary>
        /// <param name="cadusercuston">Informações do usuário para inserção e o mesmo
        /// objeto é atualizado com algumas informações para retornar ao usuário.</param>
        /// <returns>CadastroUsuarioCustonWebAPIDTO</returns>
        public CadastroUsuarioCustonWebAPIDTO GerarAssinaturaComContrato(CadastroUsuarioCustonWebAPIDTO cadusercuston)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ClienteSRV clientesrv = new ClienteSRV();
                AssinaturaEmailSRV _assinaturaemailsrv = new AssinaturaEmailSRV();
                AssinaturaTelefoneSRV _assinaturatelefonesrv = new AssinaturaTelefoneSRV();
                AssinaturaSenhaSRV _assinaturasenhasrv = new AssinaturaSenhaSRV();
                AssinaturaSenhaDTO assinaturaSenha = new AssinaturaSenhaDTO();
                AssinaturaDTO assinaturaCliente = new AssinaturaDTO();
                BuscarClienteDTO cliente = null;
                AssinaturaEmailDTO aedto = new AssinaturaEmailDTO();
                AssinaturaTelefoneDTO atdto = new AssinaturaTelefoneDTO();
                ClienteDto cliente_salvo = new ClienteDto();
                AssinaturaDTO assinaturarecupareda = new AssinaturaDTO();
                ClienteDto cdto = null;
                //bool adicionartelefone = false;

                #region checa duplicação de email ou cpf por tipo de usuário
                var clientesPorCPFCNPJ = clientesrv.BuscarClientesGeral(cpf_cnpj: cadusercuston.cpfcnpj);
                var clientesPorEmail = clientesrv.BuscarClientesGeral(email: cadusercuston.email);

                if (clientesPorEmail != null && clientesPorEmail.lista != null && clientesPorEmail.lista.Count() > 0)
                {
                    var emailUsuario = _assinaturaemailsrv.BuscarEmails(cadusercuston.email);
                    foreach (var emailrecuperado in emailUsuario)
                    {
                        if (emailrecuperado.ASN_NUM_ASSINATURA != null)
                        {
                            var teste = emailrecuperado.ASN_NUM_ASSINATURA.Substring(0, 2);
                            if (emailrecuperado.ASN_NUM_ASSINATURA.Substring(0, 2).Equals(cadusercuston.produto.ToString()))
                            {
                                throw new ValidacaoException("Este email já foi cadastrado para este produto!");
                            }
                        }
                    }

                    //response.Content = new StringContent("{\"mensagem\":\"Este email já foi cadastrado!\"}", Encoding.UTF8, "application/json");
                    //return response;
                }

                if (clientesPorCPFCNPJ != null && clientesPorCPFCNPJ.lista != null && clientesPorCPFCNPJ.lista.Count() > 0)
                {
                    cliente = clientesPorCPFCNPJ.lista.FirstOrDefault();

                    assinaturaCliente = this.FindPrimeiraAssinaturaPorCliente((int)cliente.CLI_ID, cadusercuston.produto);
                    if (assinaturaCliente != null)
                    {
                        string docIdentificacao = cadusercuston.cpfcnpj.Length > 11 ? "CNPJ" : "CPF";
                        throw new ValidacaoException($"Este {docIdentificacao} já foi cadastrado!");
                    }
                }
                #endregion


                bool clientenovo = false;
                if (cliente == null)
                {
                    cdto = new ClienteDto();
                    cdto.CLI_NOME = cadusercuston.nomerazaosocial;
                    cdto.CLI_A_C = cadusercuston.telefone;
                    cdto.CLI_TP_PESSOA = cadusercuston.empresa.Equals("true") ? "J" : "F";
                    cdto.CLI_CPF_CNPJ = cadusercuston.cpfcnpj;
                    cdto.DATA_CADASTRO = DateTime.Now;
                    cdto.USU_LOGIN = "SISTEMA";
                    cdto.TIPO_CLI_ID = 1;
                    cdto.CLI_COD_PAIS = "1058";
                    cdto.CLA_CLI_ID = 3;
                    cdto.CLI_EMAIL = cadusercuston.email;
                    cdto.CLI_NOME_RESPONSAVEL_EMPRESA = cadusercuston.nomeresponsavel;// +telefone;
                    cdto.CLI_CPF_RESPONSAVEL_EMPRESA = cadusercuston.cpfcnpjresponsavel;

                    assinaturarecupareda = new AssinaturaDTO();
                    cdto.ASSINATURA.Add(assinaturarecupareda);
                    clientenovo = true;
                }
                else
                {
                    cdto = clientesrv.FindByIdFullLoadedGeneral((int)cliente.CLI_ID, cadusercuston.produto, false, true, true);
                    assinaturarecupareda = this.ExtrairAssinaturaCliente(cdto, cadusercuston.produto, true, true, true);

                    if (assinaturarecupareda == null)
                    {

                        assinaturarecupareda = new AssinaturaDTO();
                        cdto.ASSINATURA.Add(assinaturarecupareda);
                        assinaturarecupareda = this.GerarAssinatura(cdto, cadusercuston.produto);
                    }

                }

                assinaturarecupareda.ASSINATURA_EMAIL.Add(aedto);
                assinaturarecupareda.ASSINATURA_TELEFONE.Add(atdto);

                cliente_salvo = clientesrv.SalvarClienteEInformacoesDeMarketingSemRodizio(cdto, cadusercuston.produto);

                assinaturaSenha.ASN_ATIVO = true;
                assinaturaSenha.ASN_DATA_CADASTRO = DateTime.Now;
                assinaturaSenha.ASN_DATA_ALTERA = DateTime.Now;

                var assinaturanova = this.BuscarAssinaturaPorCLIID((int)cliente_salvo.CLI_ID);

                assinaturaSenha.ASN_NUM_ASSINATURA = clientenovo ? assinaturanova.ASN_NUM_ASSINATURA : assinaturarecupareda.ASN_NUM_ASSINATURA;

                int inteiro = 0;
                if (int.TryParse(_assinaturasenhasrv.pegarUltimasenha(), out inteiro))
                    assinaturaSenha.ASN_SENHA = inteiro.ToString();

                atdto.ASN_NUM_ASSINATURA = assinaturaSenha.ASN_NUM_ASSINATURA;
                atdto.ATE_DDD = cadusercuston.telefone.Substring(0, 2);
                atdto.ATE_TELEFONE = cadusercuston.telefone.Length == 10 ? cadusercuston.telefone.Substring(2, 8) : cadusercuston.telefone.Substring(2, 9);
                atdto.TIPO_TEL_ID = cadusercuston.telefone.Length == 10 ? 4 : 1;

                aedto.ASN_NUM_ASSINATURA = assinaturaSenha.ASN_NUM_ASSINATURA;
                aedto.AEM_EMAIL = cadusercuston.email;

                _assinaturaemailsrv.Save(aedto);
                _assinaturatelefonesrv.Save(atdto);

                ContratoSRV contratosrv = new ContratoSRV();
                contratosrv.CriarContratoComAssinatura(assinaturanova, cadusercuston.estado, cadusercuston.prod_composicao, true);

                using (MD5 md5Hash = MD5.Create())
                {
                    cadusercuston.senha = COAD.SEGURANCA.Repositorios.Base.SessionContext.HashMD5(assinaturaSenha.ASN_SENHA);
                }
                cadusercuston.assinatura = assinaturanova.ASN_NUM_ASSINATURA;

                scope.Complete();
                return cadusercuston;
            }
        }
    }

    public class QtdeConsultasPeridoDTO
    {
        public string codigo { get; set; }
        public string nome { get; set; }
        public int contratadas { get; set; }
        public int qtdemail { get; set; }
        public int qtdurarj { get; set; }
        public int qtduramg { get; set; }
        public int qtdurapr { get; set; }
        public int qtdtotal { get; set; }
        public decimal vlrcontrato { get; set; }


        public string periodo { get; set; }
    }


}

