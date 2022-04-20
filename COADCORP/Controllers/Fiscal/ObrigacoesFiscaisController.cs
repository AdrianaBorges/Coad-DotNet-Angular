using System;
using System.Web;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Web.Mvc;
using System.ComponentModel;
using System.Collections.Generic;
using ACBrFramework.Sped;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Service;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COADCORP.Models;
using Coad.GenericCrud.ActionResultTools;
using System.IO;
using COAD.SEGURANCA.Filter;
using System.Threading;
using System.Globalization;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model;

namespace COADCORP.Controllers.ObrigacaoFiscal
{
    public class ObrigacoesFiscaisController : Controller
    {
        IbgeMunicipioSRV _srv = new IbgeMunicipioSRV();
        NotaFiscalSRV _nfsrv = new NotaFiscalSRV();
        ProdutosSRV _prosrv = new ProdutosSRV();
        UnidadeMedidaSRV _unmedida = new UnidadeMedidaSRV();
        EmpresaSRV _empsrv = new EmpresaSRV();
        List<NotaFiscalDTO> _listanf = new List<NotaFiscalDTO>();
        Boolean _creditaicms = false;

        public List<SelectListItem> ListaMeses = new List<SelectListItem>();
        public int emp_id = 0;
        public SituacaoDocto BuscarCodSituacao(int? _cod_sit)
        {
            if (_cod_sit == null)
            {
                return SituacaoDocto.Regular;
            }
            else
            {

                switch ((int)_cod_sit)
                {
                    case 0:
                        return SituacaoDocto.Regular;

                    case 1:
                        return SituacaoDocto.ExtempRegular;

                    case 2:
                        return SituacaoDocto.Cancelado;

                    case 3:
                        return SituacaoDocto.CanceladoExtemp;

                    case 4:
                        return SituacaoDocto.DoctoDenegado;

                    case 5:
                        return SituacaoDocto.DoctoNumInutilizada;

                    case 6:
                        return SituacaoDocto.FiscalCompl;

                    case 7:
                        return SituacaoDocto.ExtempCompl;

                    case 8:
                        return SituacaoDocto.RegimeEspecNEsp;

                    default:
                        return SituacaoDocto.Regular;

                }
            }
        }
        public GrupoTensao BuscarGrupoTensao(int? _cod_grupo_tensao)
        {
                if (_cod_grupo_tensao == null)
                    return GrupoTensao.Nenhum;

                switch (_cod_grupo_tensao)
                {
                    case 0:
                        return GrupoTensao.Nenhum;
                    case 1:
                        return GrupoTensao.A1;
                    case 2:
                        return GrupoTensao.A2;
                    case 3:
                        return GrupoTensao.A3;
                    case 4:
                        return GrupoTensao.A3a;
                    case 5:
                        return GrupoTensao.A4;
                    case 6:
                        return GrupoTensao.AS;
                    case 7:
                        return GrupoTensao.B107;
                    case 8:
                        return GrupoTensao.B108;
                    case 9:
                        return GrupoTensao.B209;
                    case 10:
                        return GrupoTensao.B2Rural;
                    case 11:
                        return GrupoTensao.B2Irrigacao;
                    case 12:
                        return GrupoTensao.B3;
                    case 13:
                        return GrupoTensao.B4a;
                    case 14:
                        return GrupoTensao.B4b;
                    default:
                        return GrupoTensao.Nenhum;
                }
        }
        public TipoLigacao BuscarTipoLigacao(int? _cod_tipo_ligacao)
        {
            if (_cod_tipo_ligacao == null)
                return TipoLigacao.Nenhum;

            switch (_cod_tipo_ligacao)
            {
                case 0:
                    return TipoLigacao.Nenhum;
                case 1:
                    return TipoLigacao.Monofasico;
                case 2:
                    return TipoLigacao.Bifasico;
                case 3:
                    return TipoLigacao.Trifasico;
                default:
                    return TipoLigacao.Nenhum;
            }

        }
        public TipoItem BuscarTipoProduto(int? _tipo_pro)
        {

            switch (_tipo_pro)
            {
                case 0:
                    return TipoItem.MercadoriaRevenda;
                case 1:
                    return TipoItem.MateriaPrima;
                case 2:
                    return TipoItem.Embalagem;
                case 3:
                    return TipoItem.ProdutoProcesso;
                case 4:
                    return TipoItem.ProdutoAcabado;
                case 5:
                    return TipoItem.Subproduto;
                case 6:
                    return TipoItem.ProdutoIntermediario;
                case 7:
                    return TipoItem.MaterialConsumo;
                case 8:
                    return TipoItem.AtivoImobilizado;
                case 9:
                    return TipoItem.Servicos;
                case 10:
                    return TipoItem.OutrosInsumos;
                case 11:
                    return TipoItem.Outras;
                default:
                    return TipoItem.MaterialConsumo;
            }

        }
        public VersaoLeiaute BuscarVersaoLeiaute(int? _versao)
        {

            switch (_versao)
            {
                case 0:
                    return VersaoLeiaute.Versao100;
                case 1:
                    return VersaoLeiaute.Versao101;
                case 2:
                    return VersaoLeiaute.Versao102;
                case 3:
                    return VersaoLeiaute.Versao103;
                case 4:
                    return VersaoLeiaute.Versao104;
                case 5:
                    return VersaoLeiaute.Versao105;
                case 6:
                    return VersaoLeiaute.Versao106;
                case 7:
                    return VersaoLeiaute.Versao107;
                case 8:
                    return VersaoLeiaute.Versao108;
                case 9:
                    return VersaoLeiaute.Versao109;
                default:
                    return VersaoLeiaute.Versao108;
            }

        }
        public void CarregarEmpresa()
        {
            var ListaEmpresa = new EmpresaSRV().FindAll().ToList();
            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_NOME_FANTASIA");
        }
        public void CarregarVersao()
        {


        }
        [Autorizar(PorMenu = true)]
        public ActionResult GerarSped()
        {

            //-----

            ListaMeses.AddRange(new[]{
                            new SelectListItem() { Text = "Janeiro", Value = "1" },
                            new SelectListItem() { Text = "Fevereiro", Value = "2" },
                            new SelectListItem() { Text = "Março", Value = "3" },
                            new SelectListItem() { Text = "Abril", Value = "4" },
                            new SelectListItem() { Text = "Maio", Value = "5" },
                            new SelectListItem() { Text = "Junho", Value = "6" },
                            new SelectListItem() { Text = "Julho", Value = "7" },
                            new SelectListItem() { Text = "Agosto", Value = "8" },
                            new SelectListItem() { Text = "Setembro", Value = "9" },
                            new SelectListItem() { Text = "Outubro", Value = "10" },
                            new SelectListItem() { Text = "Novembro", Value = "11" },
                            new SelectListItem() { Text = "Dezembro", Value = "12" }
            });

            //-----
            
            List<SelectListItem> _lista = new List<SelectListItem>();
                 

            _lista.AddRange(new[]{
                            new SelectListItem() { Text = "Versão 001", Value = "0" },
                            new SelectListItem() { Text = "Versão 002", Value = "1" },
                            new SelectListItem() { Text = "Versão 003", Value = "2" },
                            new SelectListItem() { Text = "Versão 004", Value = "3" },
                            new SelectListItem() { Text = "Versão 005", Value = "4" },
                            new SelectListItem() { Text = "Versão 006", Value = "5" },
                            new SelectListItem() { Text = "Versão 007", Value = "6" },
                            new SelectListItem() { Text = "Versão 008", Value = "7" },
                            new SelectListItem() { Text = "Versão 009", Value = "8" },
                            new SelectListItem() { Text = "Versão 010", Value = "9" }
            });

            //-----
            
            var ListaEmpresa = new EmpresaSRV().FindAll().ToList();

            ViewBag.lnkPath      = "#";
            ViewBag.lnkVisible   = "false";
            ViewBag.lnkLink      = "";
            ViewBag.emp_id       = SessionContext.emp_id;
            ViewBag.ListaMes     = new SelectList(ListaMeses, "Value", "Text");
            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_NOME_FANTASIA");
            ViewBag.ListaVersao  = new SelectList(_lista, "Value", "Text");


            return View();
        }
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult GerarSped(int? mesatual, string anoatual, int emp_id, int versao)
        {

            ViewBag.MesAtual = mesatual;
            ViewBag.AnoAtual = anoatual;

            DateTime _dtini = new DateTime(Convert.ToInt32(anoatual), (int)mesatual, 1);
            DateTime _dtfim = new DateTime(Convert.ToInt32(anoatual), (int)mesatual, DateTime.DaysInMonth(Convert.ToInt32(anoatual), (int)mesatual));

            JSONResponse resultado = new JSONResponse();

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");

                if (_dtini == null || _dtfim == null)
                {
                    resultado.success = false;
                    resultado.message = Message.Fail("Erro período não informado ou inválido!");

                    SysException.RegistrarLog("Erro período não informado ou inválido!", "", SessionContext.autenticado);

                    return Json(resultado, JsonRequestBehavior.AllowGet);
                }

                ACBrSpedFiscal sped = new ACBrSpedFiscal();
                this.emp_id = emp_id;

                sped.DT_INI = (DateTime)_dtini;
                sped.DT_FIN = (DateTime)_dtfim;

                // sped.OnError += new EventHandler<ErrorEventArgs>(sped_OnError);
                /*
                    0 - Abertura, Identificação e Referências
                    C - Documentos Fiscais I  - Mercadorias (ICMS/IPI)
                    D - Documentos Fiscais II - Serviços (ICMS)
                    E - Apuração do ICMS e do IPI
                    G - Controle do Crédito de ICMS do Ativo Permanente - CIAP - modelos “C” e “D”
                    H - Inventário Físico
                    1 - Outras Informações
                    9 - Controle e Encerramento do Arquivo Digital
                */

                SpedBloco0(sped,versao);
                SpedBlocoC(sped);
                SpedBlocoD(sped);
                SpedBlocoE(sped);
                SpedBlocoG(sped);
                SpedBlocoH(sped);
                SpedBloco1(sped);

                string curDir = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());

                sped.Path = curDir + "\\temp";
                sped.Arquivo = "SPED_" + emp_id.ToString() +"_" + sped.DT_INI.Year.ToString() + sped.DT_INI.Month.ToString() + ".txt";
                sped.SaveFileTXT();

                //----- Gravando Arquivo do Sped no banco de dados.

                string _lnkPath = "../temp/" + sped.Arquivo;
                string _lnkLink = "Baixar o arquivo SPED ( " + sped.Arquivo + " )";

                //------

                SPED_ARQUIVO _spdarq = new SPED_ARQUIVO();

                _spdarq.EMP_ID = SessionContext.emp_id;
                _spdarq.SPED_DATA = DateTime.Now;
                _spdarq.SPED_DATA_INICIAL = DateTime.Now;
                _spdarq.SPED_DATA_FINAL = DateTime.Now;

                new SpedArquivoSRV().IncluirReg(_spdarq);

                //-----

                resultado.success = true;
                resultado.message = Message.Success("Arquivo Gerado com sucesso !!");
                resultado.Add("lnkPath", _lnkPath);
                resultado.Add("lnkLink", _lnkLink);

                SysException.RegistrarLog("Arquivo Gerado com sucesso!! (" + sped.Arquivo + ")", "", SessionContext.autenticado);

                return Json(resultado, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);

                resultado.message = Message.Fail(SysException.Show(ex));

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            }
        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarConfigSped(int _emp_id)
        {
            //--- For_Tipo (0 - Todos , 1 - Fornecedor , 2 - Transportador)

            JSONResponse resultado = new JSONResponse();
            try
            {

                ConfigSpedFiscalDTO a = new ConfigSpedFiscalDTO();

                a = new ConfigSpedFiscalSRV().FindById(_emp_id);

                return Json(a, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                resultado.success = false;
                resultado.message = Message.Fail("Erro ao carregar a configuração. ( " + ex.Message + " )");

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }


        [Autorizar(IsAjax = true)]
        public ActionResult AtualizarConfigSped(ConfigSpedFiscalDTO config)
        {
            //--- For_Tipo (0 - Todos , 1 - Fornecedor , 2 - Transportador)

            JSONResponse resultado = new JSONResponse();
            try
            {
                new ConfigSpedFiscalSRV().Merge(config, "EMP_ID");
                
                resultado.success = true;
                resultado.message = Message.Fail("Atualização realizada com sucesso.");

                return Json(resultado, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                resultado.success = false;
                resultado.message = Message.Fail("Erro ao carregar a configuração. ( " + ex.Message + " )");

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }

        #region SPED
        private void SpedBloco0(ACBrSpedFiscal sped, int versao)
        {
           
            //Dados da Empresa
            //Abertura do Arquivo Digital e Identificação da empresa
           
            sped.Bloco_0.DT_INI = sped.DT_INI;
            sped.Bloco_0.DT_FIN = sped.DT_FIN;

            var _emp = _empsrv.FindById(this.emp_id);

            var _cont = new ContabilistaSRV().FindById(_emp.CNT_ID);


            if (_cont != null)
                _emp.CONTABILISTA = _cont;

            if (_emp.CONTABILISTA == null)
                throw new Exception("Dados do contabilista não informados.");
                        
            Registro0000 r0000 = new Registro0000
            {
                DT_INI = sped.Bloco_0.DT_INI,
                DT_FIN = sped.Bloco_0.DT_FIN,
                COD_VER = this.BuscarVersaoLeiaute(versao),
                COD_FIN = CodFinalidade.Original,
                NOME = _emp.EMP_RAZAO_SOCIAL,
                CNPJ = _emp.EMP_CNPJ,
                CPF = "",
                UF = _srv.Buscar(_emp.IBGE_COD_COMPLETO).IBGE_UF,
                IE = _emp.EMP_IE,
                COD_MUN = Convert.ToInt32(_emp.IBGE_COD_COMPLETO), 
                IM = _emp.EMP_IM,
                SUFRAMA = _emp.EMP_SUFRAMA,
                IND_PERFIL = Perfil.PerfilA,
                IND_ATIV = Atividade.Outros
            };

            sped.Bloco_0.Registro0000 = r0000;

            // FILHO - Dados complementares da Empresa
            Registro0001 r0001 = new Registro0001();
            r0001.IND_MOV = IndicadorMovimento.ComDados;

            Registro0005 r0005 = new Registro0005
            {
                FANTASIA = _emp.EMP_NOME_FANTASIA,
                CEP = _emp.EMP_CEP,
                ENDERECO = _emp.EMP_LOGRADOURO,
                NUM = _emp.EMP_NUMERO,
                COMPL = _emp.EMP_COMPLEMENTO,
                BAIRRO = _emp.EMP_BAIRRO,
                FONE = _emp.EMP_TEL1,
                FAX = _emp.EMP_TEL2,
                EMAIL = _emp.EMP_EMAIL
            };

            r0001.Registro0005 = r0005;

            int _digcnpj = _emp.CONTABILISTA.CNT_CPF_CNPJ.Length;

            //Dados do Contabilista
            Registro0100 r0100 = new Registro0100();
            
            r0100.NOME = _emp.CONTABILISTA.CNT_NOME == null ? "" : _emp.CONTABILISTA.CNT_NOME.Trim();
            r0100.CPF = (_digcnpj  == 11) ? _emp.CONTABILISTA.CNT_CPF_CNPJ : "";
            r0100.CRC = _emp.CONTABILISTA.CNT_CRC_UF;
            r0100.CNPJ = (_digcnpj == 13) ? _emp.CONTABILISTA.CNT_CPF_CNPJ : "";
            r0100.CEP = _emp.CONTABILISTA.CTR_CEP;
            r0100.ENDERECO = _emp.CONTABILISTA.CTR_LOGRADOURO == null ? "" : _emp.CONTABILISTA.CTR_LOGRADOURO.Trim();
            r0100.NUM = _emp.CONTABILISTA.CTR_NUMERO;
            r0100.COMPL = _emp.CONTABILISTA.CTR_COMPLEMENTO == null ? "" : _emp.CONTABILISTA.CTR_COMPLEMENTO.Trim();
            r0100.BAIRRO = _emp.CONTABILISTA.CTR_BAIRRO == null ? "" : _emp.CONTABILISTA.CTR_BAIRRO.Trim();
            r0100.FONE = _emp.CONTABILISTA.CTR_TEL;
            r0100.FAX = _emp.CONTABILISTA.CTR_FAX;
            r0100.EMAIL = _emp.CONTABILISTA.CTR_EMAIL == null ? "" : _emp.CONTABILISTA.CTR_EMAIL.Trim();
            r0100.COD_MUN = Convert.ToInt32(_emp.CONTABILISTA.IBGE_COD_COMPLETO);
               
            

            r0001.Registro0100 = r0100;

            //--- Dados do Participante (Clientes e Fornecedores)

            var _listaclientes = _nfsrv.BuscarClientesNFPeriodo(emp_id, sped.DT_INI, sped.DT_FIN);

            AuditoriaSRV _auditoriaSRV = new AuditoriaSRV();
                
            foreach (var _cliente in _listaclientes)
            {

                var _end = _cliente.CLIENTES_ENDERECO.Where(x => x.END_TIPO == 1).FirstOrDefault();

                if (_end == null)
                    _end = _cliente.CLIENTES_ENDERECO.Where(x => x.END_TIPO == 2).FirstOrDefault();
         
                Registro0150 r0150 = new Registro0150();

                r0150.COD_PART = _cliente.CLI_ID.ToString();
                r0150.NOME = _cliente.CLI_NOME == null ? "" : _cliente.CLI_NOME.Trim();
                r0150.COD_PAIS = _cliente.CLI_COD_PAIS;
                r0150.CNPJ = _cliente.CLI_TP_PESSOA == "J" ? _cliente.CLI_CPF_CNPJ : "";
                r0150.CPF = _cliente.CLI_TP_PESSOA == "F" ? _cliente.CLI_CPF_CNPJ : "";

                if (_cliente.CLI_INSCRICAO != null)
                    r0150.IE = _cliente.CLI_INSCRICAO.ToUpper() == "ISENTO" ? "" : _cliente.CLI_INSCRICAO;
                else
                    r0150.IE = "";

                if (_end != null)
                {
                    CLIENTES_ENDERECO _endereco = (CLIENTES_ENDERECO)_end;
                    r0150.COD_MUN = _endereco.MUNICIPIO != null ? Convert.ToInt32(_endereco.MUNICIPIO.IBGE_COD_COMPLETO) : 0;
                    r0150.SUFRAMA = _cliente.CLI_SUFRAMA;
                    r0150.ENDERECO = _endereco.END_LOGRADOURO == null ? "" : _endereco.END_LOGRADOURO.Trim();
                    r0150.NUM = _endereco.END_NUMERO;
                    r0150.COMPL = _endereco.END_COMPLEMENTO == null ? "" : _endereco.END_COMPLEMENTO.Trim();
                    r0150.BAIRRO = _endereco.END_BAIRRO == null ? "" : _endereco.END_BAIRRO.Trim();
                }

                List<CLIENTES_HISTORICO> _listaAudcliente = _auditoriaSRV.BuscaAuditoriaCliente(_cliente.CLI_ID, sped.DT_INI, sped.DT_FIN);

                foreach (var _auditoria in _listaAudcliente)
                {
                    Registro0175 r0175 = new Registro0175
                    {
                        DT_ALT = (DateTime)_auditoria.DATA_ALTERA,
                        NR_CAMPO = "1",
                        CONT_ANT = _auditoria.CLI_NOME
                    };

                    r0150.Registro0175.Add(r0175);
                }

                r0001.Registro0150.Add(r0150);
            }

            //--- Fornecedores

            var _listafornecedor = _nfsrv.BuscarFornecedorNFPeriodo(emp_id, sped.DT_INI, sped.DT_FIN);

            foreach (var _fornecedor in _listafornecedor)
            {

                if (_fornecedor != null)
                {
                    Registro0150 r0150 = new Registro0150();

                    r0150.COD_PART = _fornecedor.FOR_ID.ToString();
                    r0150.NOME = _fornecedor.FOR_RAZAO_SOCIAL == null ? "" : _fornecedor.FOR_RAZAO_SOCIAL.Trim();
                    r0150.COD_PAIS = _fornecedor.FOR_COD_PAIS;
                    r0150.CNPJ = _fornecedor.FOR_TIPESSOA == "J" ? _fornecedor.FOR_CNPJ : "";
                    r0150.CPF = _fornecedor.FOR_TIPESSOA == "F" ? _fornecedor.FOR_CNPJ : "";
                    r0150.IE = _fornecedor.FOR_INSCRICAO;
                    r0150.COD_MUN = _fornecedor.MUNICIPIO != null ? Convert.ToInt32(_fornecedor.MUNICIPIO.IBGE_COD_COMPLETO) : 0;
                    r0150.SUFRAMA = _fornecedor.FOR_INSCSUFRAMA;
                    r0150.ENDERECO = _fornecedor.FOR_ENDERECO == null ? "" : _fornecedor.FOR_ENDERECO.Trim();
                    r0150.NUM = _fornecedor.FOR_END_NUMERO;
                    r0150.COMPL = _fornecedor.FOR_END_COMPLEMENTO == null ? "" : _fornecedor.FOR_END_COMPLEMENTO.Trim();
                    r0150.BAIRRO = _fornecedor.FOR_BAIRRO == null ? "" : _fornecedor.FOR_BAIRRO.Trim();


                    // Alteração da Tabela de Cadastro de Fornecedores
                    //  1 - Nome do Participante
                    //  2 - Codigo do País
                    //  3 - CNPJ
                    //  4 - CPF
                    //  5 - Inscrição Estatual
                    //  6 - Cód. do Município do IBGE
                    //  7 - Inscrição Suframa
                    //  8 - Logradouro/Endereço
                    //  9 - Número do Endereço
                    // 10 - Complemento do Endereço
                    // 11 - Bairro

                    List<FORNECEDOR_HISTORICO> _listaAudfornecedor = _auditoriaSRV.BuscaAuditoriaFonecendor(_fornecedor.FOR_ID, sped.DT_INI, sped.DT_FIN);

                    foreach (var _auditoria in _listaAudfornecedor)
                    {

                        Registro0175 r0175 = new Registro0175
                        {
                            DT_ALT = (DateTime)_auditoria.DATA_ALTERA,
                            NR_CAMPO = "1",
                            CONT_ANT = _auditoria.FOR_RAZAO_SOCIAL
                        };

                        r0150.Registro0175.Add(r0175);
                    }

                    r0001.Registro0150.Add(r0150);
                }

            }

            //--- Fornecedores - Administradora de Cartão de Crédito

            List<FornecedorDTO> _listaadmcartao = new TotalVendasCartaoSRV().BuscarFornecedor(emp_id, sped.DT_INI.Month, sped.DT_FIN.Year).ToList();

            foreach (var _fornecedor in _listaadmcartao)
            {
          
                Registro0150 r0150 = new Registro0150
                {
                    COD_PART = _fornecedor.FOR_ID.ToString(),
                    NOME = _fornecedor.FOR_RAZAO_SOCIAL == null ? "" : _fornecedor.FOR_RAZAO_SOCIAL.Trim(),
                    COD_PAIS = _fornecedor.FOR_COD_PAIS,
                    CNPJ = _fornecedor.FOR_TIPESSOA == "J" ? _fornecedor.FOR_CNPJ : "",
                    CPF = _fornecedor.FOR_TIPESSOA == "F" ? _fornecedor.FOR_CNPJ : "",
                    IE = _fornecedor.FOR_INSCRICAO,
                    COD_MUN = _fornecedor.MUNICIPIO == null ? 0 :  Convert.ToInt32(_fornecedor.MUNICIPIO.IBGE_COD_COMPLETO),
                    SUFRAMA = _fornecedor.FOR_INSCSUFRAMA,
                    ENDERECO = _fornecedor.FOR_ENDERECO == null ? "" : _fornecedor.FOR_ENDERECO.Trim(),
                    NUM = _fornecedor.FOR_END_NUMERO,
                    COMPL = _fornecedor.FOR_END_COMPLEMENTO == null ? "" : _fornecedor.FOR_END_COMPLEMENTO.Trim(),
                    BAIRRO = _fornecedor.FOR_BAIRRO == null ? "" : _fornecedor.FOR_BAIRRO.Trim()

                };

                // Alteração da Tabela de Cadastro de Fornecedor (Administradora de Cartão de Crédito)
                //  1 - Nome do Participante
                //  2 - Codigo do País
                //  3 - CNPJ
                //  4 - CPF
                //  5 - Inscrição Estatual
                //  6 - Cód. do Município do IBGE
                //  7 - Inscrição Suframa
                //  8 - Logradouro/Endereço
                //  9 - Número do Endereço
                // 10 - Complemento do Endereço
                // 11 - Bairro

                List<FORNECEDOR_HISTORICO> _listaAudfornecedor = _auditoriaSRV.BuscaAuditoriaFonecendor(_fornecedor.FOR_ID, sped.DT_INI, sped.DT_FIN);

                foreach (var _auditoria in _listaAudfornecedor)
                {

                    Registro0175 r0175 = new Registro0175
                    {
                        DT_ALT = (DateTime)_auditoria.DATA_ALTERA,
                        NR_CAMPO = "1",
                        CONT_ANT = _auditoria.FOR_RAZAO_SOCIAL
                    };

                    r0150.Registro0175.Add(r0175);
                }

                r0001.Registro0150.Add(r0150);
            }

            //Identificação das unidades de medida
            List<UNIDADE_MEDIDA> _listaunmedida = _unmedida.BuscarEntradaPorPeriodo(emp_id, sped.DT_INI, sped.DT_FIN);

            foreach (var _item in _listaunmedida)
            {
                Registro0190 r0190 = new Registro0190
                {
                    UNID = _item.UND_ID,
                    DESCR = _item.UND_DESCRICAO
                    
                };

                r0001.Registro0190.Add(r0190);
            }
 
            //Tabela de Identificação do Item (Produtos e Serviços)

            List<PRODUTOS> _listaprodutos = _nfsrv.BuscarProdutosNFPeriodo(emp_id, sped.DT_INI, sped.DT_FIN);

            foreach (var _produtos in _listaprodutos)
            {
                Registro0200 r0200 = new Registro0200();
                
                r0200.COD_ITEM = _produtos.PRO_ID.ToString();
                r0200.DESCR_ITEM = _produtos.PRO_NOME == null ? "" : _produtos.PRO_NOME.Trim(); 
                r0200.COD_BARRA = _produtos.PRODUTO_EAN.Count() > 0 ? _produtos.PRODUTO_EAN.First().PRO_EAN : "";
                r0200.UNID_INV = _produtos.PRO_UN_VEND;
                r0200.TIPO_ITEM = this.BuscarTipoProduto(_produtos.TIPO_PRO);
                r0200.COD_NCM = _produtos.NCM_ID;
                r0200.COD_GEN = "";
                r0200.ALIQ_ICMS = 0;
                r0200.COD_ANT_ITEM = "";
                r0200.COD_LST = "";
                r0200.EX_IPI = "";

                //Tabela de Alteração do Item (Produtos e Serviços)
                List<PRODUTO_HISTORICO> _listaAudProduto = _auditoriaSRV.BuscaAuditoriaProduto(_produtos.PRO_ID, sped.DT_INI, sped.DT_FIN);

                foreach (var _auditoria in _listaAudProduto)
                {

                    Registro0205 r0205 = new Registro0205
                    {
                        COD_ANT_ITEM = _auditoria.PRO_ID.ToString(),
                        DESCR_ANT_ITEM = _auditoria.PRO_NOME,
                        DT_INI = DateTime.Now,
                        DT_FIN = DateTime.Now,
                    };

                    r0200.Registro0205.Add(r0205);
                }

                //Tabela de Fatores de Conversão Exemplo 
                //O Produto entra como Caixa e É armazenado como Unidade  CX => UN = 24 
                //Este registro é obrigatório se a unidade do produto for diferente da nota seja ela de entrada ou saida. 

                //Identificação das unidades de medida
                List<FATOR_CONVERSAO> _listafatorconversao = _unmedida.BuscarFatorConversao(r0200.UNID_INV);

                foreach (var _unconversao in _listafatorconversao)
                {
                    Registro0220 r0220 = new Registro0220
                    {
                        FAT_CONV = (decimal)_unconversao.FAT_CONVERSAO,
                        UNID_CONV = _unconversao.UND_ID
                    };
                    
                    r0200.Registro0220.Add(r0220);
                }
            
                r0001.Registro0200.Add(r0200);
            }

            //Tabela de Natureza da Operação/ Prestação (CFOP?)

            List<CFOP_TABLE> _listacfop = _nfsrv.BuscarNFCFOP(emp_id, sped.DT_INI, sped.DT_FIN);

            foreach (var _cfop in _listacfop)
            {
                Registro0400 r0400 = new Registro0400
                {
                    COD_NAT = _cfop.CFOP,
                    DESCR_NAT = _cfop.CFOP_DESCRICAO
                };

                r0001.Registro0400.Add(r0400);
            
            }

            //Tabela de Informação Complementar do documento fiscal
            _listanf = _nfsrv.BuscarNotasPeriodoModel(this.emp_id, sped.DT_INI, sped.DT_FIN).ToList();

            //if (1 == 2)
            //{
            //    foreach (var _nf in _listanf)
            //    {
            //        Registro0450 r0450 = new Registro0450
            //        {
            //            COD_INF = _nf.NF_NUMERO.ToString() + _nf.EMP_ID.ToString(),
            //            TXT = _nf.NF_INF_COMPLEMENTAR
            //        };

            //        r0001.Registro0450.Add(r0450);
            //    }
            //}

            //Tabela de Observações do Lançamento Fiscal
            foreach (var _nf in _listanf)
            {
                
                foreach (var _nfi in _nf.NOTA_FISCAL_ITEM)
                {
                    if (_nfi.NOTA_FISCAL_ITEM_OBS != null)
                    {
                        Registro0460 r0460 = new Registro0460
                        {
                            COD_OBS = _nfi.NOTA_FISCAL_ITEM_OBS.NFO_ID.ToString(),
                            TXT = _nfi.NOTA_FISCAL_ITEM_OBS.NFO_OBS
                        };

                        r0001.Registro0460.Add(r0460);
                    };
                };
            }
                        
            sped.Bloco_0.Registro0001 = r0001;
        }

        private void SpedBlocoC(ACBrSpedFiscal sped)
        {

            sped.Bloco_C.DT_INI = sped.DT_INI;
            sped.Bloco_C.DT_FIN = sped.DT_FIN;
            
            RegistroC001 c001 = new RegistroC001
            {
              IND_MOV = IndicadorMovimento.ComDados
            };

            //Documento - Nota Fiscal (código 01), 
            //Nota Fiscal Avulsa (código 1B), 
            //Nota Fiscal de Produtor (código 04) e 
            //Nota Fiscal Eletrônica (código 55)

            //---Tabela Documentos Fiscais do ICMS (Tabela 4.1.1): Campo ---->>>  COD_MOD (_nf.TDF_ID)
            //01	Nota Fiscal	1/1A	C100
            //1B	Nota Fiscal Avulsa	-	C100
            //02	Nota Fiscal de Venda a Consumidor	2	C300 ou C350 ou C400 (emissão por ECF)
            //2D	Cupom Fiscal	-	C400
            //2E	Cupom Fiscal Bilhete de Passagem	-	D350 (emissão por ECF)
            //04	Nota Fiscal de Produtor	4	C100
            //06	Nota Fiscal/Conta de Energia Elétrica	6	Se aquisição C500; Se fornecimento: C500 ou C600; C700 (somente empresas obrigadas aos arquivos previstos no Convênio 115/03)
            //07	Nota Fiscal de Serviço de Transporte	7	D100
            //08	Conhecimento de Transporte Rodoviário de Cargas	8	D100
            //8B	Conhecimento de Transporte de Cargas Avulso	-	D100
            //09	Conhecimento de Transporte Aquaviário de Cargas	9	D100
            //10	Conhecimento Aéreo	10	D100
            //11	Conhecimento de Transporte Ferroviário de Cargas	11	D100
            //13	Bilhete de Passagem Rodoviário	13	D300
            //14	Bilhete de Passagem Aquaviário	14	D300
            //15	Bilhete de Passagem e Nota de Bagagem	15	D300
            //16	Bilhete de Passagem Ferroviário	16	D300
            //18	Resumo de Movimento Diário	18	D400
            //21	Nota Fiscal de Serviço de Comunicação	21	Se aquisição: D500; Se prestação: D500 ou D600; D695 (somente empresas obrigadas aos arquivos previstos no Convênio 115/03)
            //22	Nota Fiscal de Serviço de Telecomunicação	22	Se aquisição: D500; Se prestação: D500 ou D600; ou D695 (somente empresas obrigadas aos arquivos previstos no Convênio 115/03)
            //26	Conhecimento de Transporte Multimodal de Cargas	26	D100
            //27	Nota Fiscal De Transporte Ferroviário De Carga		D100
            //28	Nota Fiscal/Conta de Fornecimento de Gás Canalizado	-	C500 ou C600
            //29	Nota Fiscal/Conta de Fornecimento de Água Canalizada	-	C500 ou C600
            //55	Nota Fiscal Eletrônica – NF-e	55	C100
            //57	Conhecimento de Transporte Eletrônico - CT-e	57	D100
            //59	Cupom Fiscal Eletrônico – CF-e-SAT	59	C800 ou C850
            //60	Cupom Fiscal Eletrônico CF-e-ECF	60	C400
            //65	Nota Fiscal Eletrônica ao Consumidor Final – NFC-e	65	C100
          
            bool achei = false;

            //List<RegistroC190> c190Lista = new List<RegistroC190>();

            _listanf = _nfsrv.BuscarNotasPeriodoModel(this.emp_id, sped.DT_INI, sped.DT_FIN).ToList();

            foreach (var _nf in _listanf)
            {
                List<RegistroC190> c190Lista = new List<RegistroC190>();

                if (_nf.TDF_ID == "01" || _nf.TDF_ID == "1B" || _nf.TDF_ID == "04" || _nf.TDF_ID == "55")
                {
                    RegistroC100 c100 = new RegistroC100();

                    c100.NUM_DOC = _nf.NF_NUMERO.ToString();
                    c100.IND_OPER = (_nf.NF_TIPO == 0 || _nf.NF_TIPO == 2) ? TipoOperacao.EntradaAquisicao : TipoOperacao.SaidaPrestacao;
                    if (_nf.NF_TIPO == 1 || _nf.NF_TIPO == 3)
                    {
                        c100.IND_EMIT = Emitente.EmissaoPropria;
                    }
                    else
                    {
                        c100.IND_EMIT = Emitente.Terceiros;
                    }
                    c100.COD_PART = (_nf.NF_TIPO == 0 || _nf.NF_TIPO == 2) ? _nf.FOR_ID.ToString() : _nf.CLI_ID.ToString();
                    c100.COD_MOD = _nf.TDF_ID;
                    c100.COD_SIT = this.BuscarCodSituacao(Convert.ToInt32(_nf.NF_COD_SIT));
                    c100.SER = _nf.NF_SERIE == null ? "" : _nf.NF_SERIE.Trim();
                    c100.CHV_NFE = _nf.NF_CHAVE == null ? "" : _nf.NF_CHAVE.Trim();
                    c100.DT_DOC = (DateTime)_nf.NF_DATA_EMISSAO;
                    if (_nf.NF_TIPO == 0 || _nf.NF_TIPO == 2)
                        c100.DT_E_S = (DateTime)_nf.NF_DATA_ENTRADA;
                    //else
                    //    c100.DT_E_S = (DateTime)_nf.NF_DATA_SAIDA;
                    c100.VL_DOC = _nf.NF_VLR_NOTA == null ? 0 : (decimal)_nf.NF_VLR_NOTA;
                    c100.IND_PGTO = TipoPagamento.Vista;
                    c100.VL_DESC = _nf.NF_VLR_DESCONTO == null ? 0 : (decimal)_nf.NF_VLR_DESCONTO;
                    c100.VL_ABAT_NT = 0;
                    c100.VL_MERC = (decimal)_nf.NF_VLR_PROD;
                    c100.IND_FRT = TipoFrete.PorContaDestinatario;
                    c100.VL_SEG = _nf.NF_VLR_SEGURO == null ? 0 : (decimal)_nf.NF_VLR_SEGURO;
                    c100.VL_OUT_DA = _nf.NF_VLR_OUTRAS == null ? 0 : (decimal)_nf.NF_VLR_OUTRAS;
                    c100.VL_IPI = _nf.NF_VLR_IPI == null ? 0 : (decimal)_nf.NF_VLR_IPI;
                    c100.VL_PIS = _nf.NF_VLR_PIS == null ? 0 : (decimal)_nf.NF_VLR_PIS;
                    c100.VL_COFINS = _nf.NF_VLR_COFINS == null ? 0 : (decimal)_nf.NF_VLR_COFINS;
                    c100.VL_PIS_ST = 0;
                    c100.VL_COFINS_ST = 0;

                    if (_creditaicms)
                    {
                        c100.VL_BC_ICMS = _nf.NF_BASE_CALC_ICMS == null ? 0 : (decimal)_nf.NF_BASE_CALC_ICMS;
                        c100.VL_ICMS = _nf.NF_VLR_ICMS == null ? 0 : (decimal)_nf.NF_VLR_ICMS;
                        c100.VL_BC_ICMS_ST = _nf.NF_BASE_CALC_ICMS == null ? 0 : (decimal)_nf.NF_BASE_CALC_ICMS;
                        c100.VL_ICMS_ST = _nf.NF_VLR_ST == null ? 0 : (decimal)_nf.NF_VLR_ST;
                    }
                    else
                    {
                        c100.VL_BC_ICMS = 0;
                        c100.VL_ICMS = 0;
                        c100.VL_BC_ICMS_ST = 0;
                        c100.VL_ICMS_ST = 0;
                    }


                    int _ind = 1;

                    foreach (var _itemnf in _nf.NOTA_FISCAL_ITEM)
                    {
                        RegistroC170 c170 = new RegistroC170();

                        c170.NUM_ITEM = _ind.ToString();
                        c170.COD_ITEM = _itemnf.PRO_ID.ToString();
                        c170.DESCR_COMPL = _itemnf.PRODUTOS.PRO_NOME;
                        c170.QTD = _itemnf.NFI_QTDE == null ? 0 : (decimal)_itemnf.NFI_QTDE;
                        c170.UNID = _itemnf.NFI_UN;
                        c170.VL_ITEM = _itemnf.NFI_VLR_UNIT == null ? 0 : (decimal)_itemnf.NFI_VLR_UNIT;
                        c170.VL_DESC = 0;
                        c170.IND_MOV = MovimentacaoFisica.Sim;
                        c170.CST_ICMS = _itemnf.CST_ID;
                        c170.CFOP = _itemnf.CFOP;
                        c170.COD_NAT = _itemnf.CFOP;

                        if (_creditaicms)
                        {
                            c170.VL_BC_ICMS = _itemnf.NFI_BASE_CALC_ICMS == null ? 0 : (decimal)_itemnf.NFI_BASE_CALC_ICMS;
                            c170.ALIQ_ICMS = _itemnf.NFI_ALIQ_ICMS == null ? 0 : (decimal)_itemnf.NFI_ALIQ_ICMS;
                            c170.VL_ICMS = _itemnf.NFI_VLR_ICMS == null ? 0 : (decimal)_itemnf.NFI_VLR_ICMS;
                            c170.VL_BC_ICMS_ST = 0;
                            c170.ALIQ_ST = 0;
                            c170.VL_ICMS_ST = 0;
                        }
                        else
                        {
                            c170.VL_BC_ICMS = 0;
                            c170.ALIQ_ICMS = 0;
                            c170.VL_ICMS = 0;
                            c170.VL_BC_ICMS_ST = 0;
                            c170.ALIQ_ST = 0;
                            c170.VL_ICMS_ST = 0;
                        }

                        c170.IND_APUR = ApuracaoIPI.Mensal;
                        c170.CST_IPI = "";
                        c170.COD_ENQ = "";
                        c170.VL_BC_IPI = 0;
                        c170.ALIQ_IPI = _itemnf.NFI_ALIQ_IPI == null ? 0 : (decimal)_itemnf.NFI_ALIQ_IPI;
                        c170.VL_IPI = _itemnf.NFI_VLR_IPI == null ? 0 : (decimal)_itemnf.NFI_VLR_IPI;
                        c170.CST_PIS = "";
                        c170.VL_BC_PIS = 0;
                        c170.ALIQ_PIS_PERC = 0;
                        c170.QUANT_BC_PIS = 0;
                        c170.ALIQ_PIS_R = 0;
                        c170.VL_PIS = 0;
                        c170.CST_COFINS = "";
                        c170.VL_BC_COFINS = 0;
                        c170.ALIQ_COFINS_PERC = 0;
                        c170.QUANT_BC_COFINS = 0;
                        c170.ALIQ_COFINS_R = 0;
                        c170.VL_COFINS = 0;
                        c170.COD_CTA = "000";


                        _ind += 1;

                        if (_itemnf.NFI_ALIQ_ISSQN > 0 && _creditaicms)
                        {
                            RegistroC172 c172 = new RegistroC172
                            {
                                ALIQ_ISSQN = _itemnf.NFI_ALIQ_ISSQN == null ? 0 : (decimal)_itemnf.NFI_ALIQ_ISSQN,
                                VL_BC_ISSQN = _itemnf.NFI_BCALC_ISSQN == null ? 0 : (decimal)_itemnf.NFI_BCALC_ISSQN,
                                VL_ISSQN = _itemnf.NFI_VLR_ISSQN == null ? 0 : (decimal)_itemnf.NFI_VLR_ISSQN
                            };

                            c170.RegistroC172.Add(c172);
                        };

                        if (_itemnf.NFI_BC_RET > 0 || _itemnf.NFI_ICMS_ST_COMPL > 0)
                        {
                            RegistroC179 c179 = new RegistroC179
                            {
                                BC_RET = _itemnf.NFI_BC_RET == null ? 0 : (decimal)_itemnf.NFI_BC_RET,
                                BC_ST_ORIG_DEST = _itemnf.NFI_BC_ST_ORIG_DEST == null ? 0 : (decimal)_itemnf.NFI_BC_ST_ORIG_DEST,
                                ICMS_RET = _itemnf.NFI_ICMS_RET == null ? 0 : (decimal)_itemnf.NFI_ICMS_RET,
                                ICMS_ST_COMPL = _itemnf.NFI_ICMS_ST_COMPL == null ? 0 : (decimal)_itemnf.NFI_ICMS_ST_COMPL,
                                ICMS_ST_REP = _itemnf.NFI_ICMS_ST_REP == null ? 0 : (decimal)_itemnf.NFI_ICMS_ST_REP
                            };

                            c170.RegistroC179.Add(c179);
                        }


                        achei = false;

                        if (c190Lista.Count > 0)
                        {
                            for (int i = 0; i <= c190Lista.Count - 1; i++)
                            {
                                if (c190Lista[i].CFOP == _itemnf.CFOP && c190Lista[i].CST_ICMS == _itemnf.CST_ID && c190Lista[i].ALIQ_ICMS == _itemnf.NFI_ALIQ_ICMS)
                                {
                                    c190Lista[i].VL_OPR = c190Lista[i].VL_OPR + (_itemnf.NFI_VLR_TOTAL == null ? 0 : (decimal)_itemnf.NFI_VLR_TOTAL);

                                    if (_creditaicms)
                                    {
                                        c190Lista[i].VL_BC_ICMS = c190Lista[i].VL_BC_ICMS + (_itemnf.NFI_BASE_CALC_ICMS == null ? 0 : (decimal)_itemnf.NFI_BASE_CALC_ICMS);
                                        c190Lista[i].VL_ICMS = c190Lista[i].VL_ICMS + (_itemnf.NFI_VLR_ICMS == null ? 0 : (decimal)_itemnf.NFI_VLR_ICMS);
                                        c190Lista[i].VL_BC_ICMS_ST = 0;
                                        c190Lista[i].VL_ICMS_ST = 0;
                                        c190Lista[i].VL_RED_BC = 0;
                                        c190Lista[i].VL_IPI = c190Lista[i].VL_IPI + (_itemnf.NFI_VLR_IPI == null ? 0 : (decimal)_itemnf.NFI_VLR_IPI);
                                    }
                                    else
                                    {
                                        c190Lista[i].VL_BC_ICMS = 0;
                                        c190Lista[i].VL_ICMS = 0;
                                        c190Lista[i].VL_BC_ICMS_ST = 0;
                                        c190Lista[i].VL_ICMS_ST = 0;
                                        c190Lista[i].VL_RED_BC = 0;
                                        c190Lista[i].VL_IPI = 0;
                                    }

                                    achei = true;
                                }
                            }
                        }

                        if (!achei)
                        {
                            RegistroC190 c190 = new RegistroC190();

                            c190.CST_ICMS = _itemnf.CST_ID;
                            c190.CFOP = _itemnf.CFOP;
                            c190.ALIQ_ICMS = _itemnf.NFI_ALIQ_ICMS == null ? 0 : (decimal)_itemnf.NFI_ALIQ_ICMS;
                            c190.VL_OPR = _itemnf.NFI_VLR_TOTAL == null ? 0 : (decimal)_itemnf.NFI_VLR_TOTAL; 

                            if (_creditaicms)
                            {
                                c190.VL_BC_ICMS = _itemnf.NFI_BASE_CALC_ICMS == null ? 0 : (decimal)_itemnf.NFI_BASE_CALC_ICMS;
                                c190.VL_ICMS = _itemnf.NFI_VLR_ICMS == null ? 0 : (decimal)_itemnf.NFI_VLR_ICMS;
                                c190.VL_BC_ICMS_ST = 0;
                                c190.VL_ICMS_ST = 0;
                                c190.VL_RED_BC = 0;
                                c190.VL_IPI = _itemnf.NFI_VLR_IPI == null ? 0 : (decimal)_itemnf.NFI_VLR_IPI;
                                c190.COD_OBS = _itemnf.NOTA_FISCAL_ITEM_OBS == null ? null : _itemnf.NOTA_FISCAL_ITEM_OBS.NFO_ID.ToString();
                            }
                            else
                            {
                                c190.VL_BC_ICMS = 0;
                                c190.VL_ICMS = 0;
                                c190.VL_BC_ICMS_ST = 0;
                                c190.VL_ICMS_ST = 0;
                                c190.VL_RED_BC = 0;
                                c190.VL_IPI = 0;
                                c190.COD_OBS = null;
                            }

                            c190Lista.Add(c190);

                        }


                        if (_itemnf.NOTA_FISCAL_ITEM_OBS != null)
                        {
                            RegistroC195 c195 = new RegistroC195();
                            c195.COD_OBS = _itemnf.NOTA_FISCAL_ITEM_OBS.NFO_ID.ToString();
                            c195.TXT_COMPL = _itemnf.NOTA_FISCAL_ITEM_OBS.NFO_OBS;

                            RegistroC197 c197 = new RegistroC197();

                            c197.ALIQ_ICMS = _itemnf.NFI_ALIQ_ICMS == null ? 0 : (decimal)_itemnf.NFI_ALIQ_ICMS;
                            c197.COD_AJ = "";
                            c197.COD_ITEM = _itemnf.PRO_ID.ToString();
                            c197.DESCR_COMPL_AJ = "";
                            c197.VL_BC_ICMS = _itemnf.NFI_BASE_CALC_ICMS == null ? 0 : (decimal)_itemnf.NFI_BASE_CALC_ICMS;
                            c197.VL_ICMS = _itemnf.NFI_VLR_ICMS == null ? 0 : (decimal)_itemnf.NFI_VLR_ICMS;
                            c197.VL_OUTROS = 0;

                            c195.RegistroC197.Add(c197);
                            c100.RegistroC195.Add(c195);

                        };
                        
                        if (c100.IND_EMIT != Emitente.EmissaoPropria)
                            c100.RegistroC170.Add(c170);

                    }

                    for (var ind = 0; ind < c190Lista.Count(); ind++)
                    {
                        if (ind == 0)
                            c190Lista[ind].VL_OPR = c190Lista[ind].VL_OPR + (_nf.NF_VLR_FRETE == null ? 0 : (decimal)_nf.NF_VLR_FRETE);
  
                        c100.RegistroC190.Add(c190Lista[ind]);
                    }

                    //foreach (var _c190 in c190Lista)
                    //{
                    //    c100.RegistroC190.Add(_c190);
                    //}

                    sped.Bloco_C.RegistroC001.RegistroC100.Add(c100);
                    
                    c190Lista.Clear();

                }

            };

           var _listaserviconf = _nfsrv.BuscarNFEntradaServicoPeriodo(this.emp_id, sped.DT_INI, sped.DT_FIN);

            foreach (var _nf in _listaserviconf)
            {

                if (_nf.TDF_ID != "21" && _nf.TDF_ID != "22")
                {
                    RegistroC500 c500 = new RegistroC500();
                    c500.COD_CONS = _nf.NF_COD_CONSUMO;
                    c500.COD_GRUPO_TENSAO = BuscarGrupoTensao(_nf.NF_COD_GRUPO_TENSAO);
                    c500.COD_INF = "";
                    c500.COD_MOD = _nf.TDF_ID;
                    c500.COD_PART = _nf.FOR_ID.ToString();
                    c500.COD_SIT = BuscarCodSituacao(Convert.ToInt32(_nf.NF_COD_SIT));
                    c500.DT_DOC = (DateTime)_nf.NF_DATA_EMISSAO;
                    c500.DT_E_S = (DateTime)_nf.NF_DATA_ENTRADA;
                    c500.IND_EMIT = Emitente.Terceiros;
                    c500.IND_OPER = TipoOperacao.EntradaAquisicao;
                    c500.NUM_DOC = _nf.NF_NUMERO.ToString();
                    c500.SER = _nf.NF_SERIE.Trim();
                    c500.SUB = "0";
                    c500.TP_LIGACAO = BuscarTipoLigacao(_nf.NF_TIPO_LIGACAO);
                    c500.VL_BC_ICMS = _nf.NF_BASE_CALC_ICMS == null ? 0 : (decimal)_nf.NF_BASE_CALC_ICMS;
                    c500.VL_BC_ICMS_ST = _nf.NF_BASE_CALC_ST == null ? 0 : (decimal)_nf.NF_BASE_CALC_ST;
                    c500.VL_COFINS = _nf.NF_VLR_COFINS == null ? 0 : (decimal)_nf.NF_VLR_COFINS;
                    c500.VL_DA = 0;
                    c500.VL_DESC = _nf.NF_VLR_DESCONTO == null ? 0 : (decimal)_nf.NF_VLR_DESCONTO; // Valor total do desconto.
                    c500.VL_DOC = _nf.NF_VLR_NOTA == null ? 0 : (decimal)_nf.NF_VLR_NOTA;    // Valor total do documento fiscal
                    c500.VL_FORN = _nf.NF_VLR_NOTA == null ? 0 : (decimal)_nf.NF_VLR_NOTA;   // Valor total fornecido/consumido.
                    c500.VL_ICMS = _nf.NF_VLR_ICMS == null ? 0 : (decimal)_nf.NF_VLR_ICMS;
                    c500.VL_ICMS_ST = _nf.NF_VLR_ST == null ? 0 : (decimal)_nf.NF_VLR_ST;
                    c500.VL_PIS = _nf.NF_VLR_PIS == null ? 0 : (decimal)_nf.NF_VLR_PIS;
                    c500.VL_SERV_NT = 0; // Valor total dos serviços não tributados pelo ICMS 
                    c500.VL_TERC = 0; //Valor total cobrado em nome de terceiros.

                    int _seq = 1;

                    foreach (var _item in _nf.NOTA_FISCAL_ITEM)
                    {

                        RegistroC510 c510 = new RegistroC510
                        {
                            ALIQ_ICMS = (decimal)_item.NFI_ALIQ_ICMS,
                            ALIQ_ST = 0,
                            CFOP = _item.CFOP,
                            COD_CLASS = "",
                            COD_CTA = "",
                            COD_ITEM = _item.PRO_ID.ToString(),
                            COD_PART = _nf.FOR_ID.ToString(),
                            CST_ICMS = _item.CST_ID,
                            IND_REC = TipoReceita.Propria,
                            NUM_ITEM = _seq.ToString(),
                            QTD = _item.NFI_QTDE == null ? 0 : (decimal)_item.NFI_QTDE,
                            UNID = _item.NFI_UN,
                            VL_BC_ICMS = _item.NFI_BASE_CALC_ICMS == null ? 0 : (decimal)_item.NFI_BASE_CALC_ICMS,
                            VL_BC_ICMS_ST = 0,
                            VL_COFINS = 0,
                            VL_DESC = 0,
                            VL_ICMS = _item.NFI_VLR_ICMS == null ? 0 : (decimal)_item.NFI_VLR_ICMS,
                            VL_ICMS_ST = 0,
                            VL_ITEM = _item.NFI_VLR_UNIT == null ? 0 : (decimal)_item.NFI_VLR_UNIT,
                            VL_PIS = 0

                        };

                        _seq += 1;

                        c500.RegistroC510.Add(c510);

                    }

                    //-----------

                    RegistroC590 c590 = new RegistroC590();
                    c590.ALIQ_ICMS = _nf.NF_ALIQ_ICMS_SERV == null? 0 : (decimal)_nf.NF_ALIQ_ICMS_SERV;
                    c590.CFOP = _nf.CFOPENT;
                    c590.CST_ICMS = _nf.CST_ID_SERV;
                    c590.VL_BC_ICMS_ST = 0;
                    c590.VL_BC_ICMS = _nf.NF_BASE_CALC_ICMS == null ? 0 : (decimal)_nf.NF_BASE_CALC_ICMS;
                    c590.VL_ICMS = _nf.NF_VLR_ICMS == null ? 0 : (decimal)_nf.NF_VLR_ICMS;
                    c590.VL_OPR = _nf.NF_VLR_NOTA == null ? 0 : (decimal)_nf.NF_VLR_NOTA;
                    c590.VL_RED_BC = 0;

                    //-----------

                    c500.RegistroC590.Add(c590);

                    sped.Bloco_C.RegistroC001.RegistroC500.Add(c500);
                }
            }
            
        }

        /// <summary>
        /// Bloco Referente as informações de transporte de cargas;
        /// Não Implementado;
        /// </summary>
        /// <param name="sped"></param>
        private void SpedBlocoD(ACBrSpedFiscal sped)
        {
            sped.Bloco_D.DT_INI = sped.DT_INI;
            sped.Bloco_D.DT_FIN = sped.DT_FIN;

            var _listaserviconf = _nfsrv.BuscarNFEntradaServicoPeriodo(this.emp_id, sped.DT_INI, sped.DT_FIN);

            //RegistroD100 d100 = new RegistroD100
            //{
            //    IND_OPER = TipoOperacao.SaidaPrestacao,
            //    IND_EMIT = Emitente.Terceiros,
            //    COD_PART = "000001",
            //    COD_MOD = "57", // --- _nf.TDF_ID;
            //    COD_SIT = SituacaoDocto.Regular,
            //    SER = "1",
            //    NUM_DOC = "012345",
            //    CHV_CTE = "",
            //    DT_DOC = DateTime.Now.AddDays(-1), //StrToDate("30/11/2011"),
            //    DT_A_P = DateTime.Now.AddDays(-1), //StrToDate("30/11/2011"),
            //    TP_CT_e = "1",
            //    VL_DOC = 100.00m,
            //    VL_DESC = 0.00m,
            //    IND_FRT = TipoFrete.PorContaDestinatario,
            //    VL_SERV = 100.00m,
            //    VL_BC_ICMS = 100.00m,
            //    VL_ICMS = 17.00m,
            //    VL_NT = 10.10m,
            //    COD_INF = "000001",
            //    COD_CTA = "111"

            //};

            ////----------

            //RegistroD110 d110 = new RegistroD110
            //{
            //    COD_ITEM = "0",
            //    NUN_ITEM = 0,
            //    VL_OUT = 0,
            //    VL_SERV = 0

            //};

            //RegistroD120 d120 = new RegistroD120
            //{
            //    COD_MUN_DEST = "",
            //    COD_MUN_ORIG = "",
            //    UF_ID = "",
            //    VEIC_ID = ""
            //};

            //d110.RegistroD120.Add(d120);

            ////----------

            //RegistroD130 d130 = new RegistroD130
            //{
            //    COD_MUN_DEST = "",
            //    COD_MUN_ORIG = "",
            //    COD_PART_CONSG = "",
            //    COD_PART_RED = "",
            //    IND_FRT_RED = 0,
            //    UF_ID = "",
            //    VEIC_ID = "",
            //    VL_DESP = 0,
            //    VL_FRT = 0,
            //    VL_LIQ_FRT = 0,
            //    VL_OUT = 0,
            //    VL_PEDG = 0,
            //    VL_SEC_CAT = 0
            //};

            //RegistroD140 d140 = new RegistroD140
            //{
            //};

            //RegistroD150 d150 = new RegistroD150
            //{
            //};

            //RegistroD161 d161 = new RegistroD161
            //{
            //};

            //RegistroD162 d162 = new RegistroD162
            //{
            //};
            //RegistroD160 d160 = new RegistroD160
            //{

            //};
            //RegistroD170 d170 = new RegistroD170
            //{

            //};

            //RegistroD190 d190 = new RegistroD190
            //{
            //    CST_ICMS = "000",
            //    CFOP = "1252",
            //    ALIQ_ICMS = 17.00m,
            //    VL_OPR = 100.00m,
            //    VL_BC_ICMS = 100.00m,
            //    VL_ICMS = 17.00m,
            //    VL_RED_BC = 0.00m,
            //    COD_OBS = "000001"

            //};


            //RegistroD197 d197 = new RegistroD197
            //{

            //};

            //RegistroD195 d195 = new RegistroD195
            //{

            //};

            Boolean acheinf = false;
                
            foreach (var _nf in _listaserviconf)
            {
                if (_nf.TDF_ID == "21" || _nf.TDF_ID == "22" )
                {
                    acheinf = true;

                    RegistroD500 d500 = new RegistroD500();

                    d500.COD_CTA = "";
                    d500.COD_INF = "";
                    d500.COD_MOD = _nf.TDF_ID; 
                    d500.COD_PART = _nf.FOR_ID.ToString();
                    d500.COD_SIT = BuscarCodSituacao(Convert.ToInt32(_nf.NF_COD_SIT));
                    d500.DT_A_P = (DateTime)_nf.NF_DATA_ENTRADA;
                    d500.DT_DOC = (DateTime)_nf.NF_DATA_EMISSAO; 
                    d500.IND_EMIT = Emitente.Terceiros;
                    d500.IND_OPER = TipoOperacao.EntradaAquisicao;
                    d500.NUM_DOC = _nf.NF_NUMERO.ToString();
                    d500.SER = _nf.NF_SERIE.Trim();
                    d500.SUB = "";
                    d500.TP_ASSINANTE = TipoAssinante.ComercialIndustrial;
                    d500.VL_BC_ICMS = _nf.NF_BASE_CALC_ICMS == null ? 0 : (decimal)_nf.NF_BASE_CALC_ICMS;
                    d500.VL_COFINS = _nf.NF_VLR_COFINS == null ? 0 : (decimal)_nf.NF_VLR_COFINS;
                    d500.VL_DA = 0;
                    d500.VL_DESC = _nf.NF_VLR_DESCONTO == null ? 0 : (decimal)_nf.NF_VLR_DESCONTO;
                    d500.VL_DOC = _nf.NF_VLR_NOTA == null ? 0 : (decimal)_nf.NF_VLR_NOTA;
                    d500.VL_ICMS  = _nf.NF_VLR_ICMS == null ? 0 : (decimal)_nf.NF_VLR_ICMS;
                    d500.VL_PIS = _nf.NF_VLR_PIS == null ? 0 : (decimal)_nf.NF_VLR_PIS;
                    d500.VL_SERV = _nf.NF_VLR_NOTA == null ? 0 : (decimal)_nf.NF_VLR_NOTA; 
                    d500.VL_SERV_NT = 0;
                    d500.VL_TERC = 0;

                    //-----------

                    RegistroD590 d590 = new RegistroD590();
                    d590.ALIQ_ICMS = _nf.NF_ALIQ_ICMS_SERV == null ? 0 : (decimal)_nf.NF_ALIQ_ICMS_SERV;
                    d590.CFOP = _nf.CFOPENT;
                    d590.CST_ICMS = _nf.CST_ID_SERV;
                    d590.VL_BC_ICMS = _nf.NF_BASE_CALC_ICMS == null ? 0 : (decimal)_nf.NF_BASE_CALC_ICMS;
                    d590.VL_ICMS = _nf.NF_VLR_ICMS == null ? 0 : (decimal)_nf.NF_VLR_ICMS;
                    d590.VL_OPR = _nf.NF_VLR_NOTA == null ? 0 : (decimal)_nf.NF_VLR_NOTA;
                    d590.VL_RED_BC = 0;

                    //-----------

                    d500.RegistroD590.Add(d590);

                    sped.Bloco_D.RegistroD001.RegistroD500.Add(d500);
                }
            
           
                //RegistroD510 d510 = new RegistroD510
                //{

                //};

                //d500.RegistroD510.Add(d510);

                //d100.RegistroD110.Add(d110);
                //d100.RegistroD130.Add(d130);
                //d100.RegistroD140.Add(d140);
                //d100.RegistroD150.Add(d150);

                //d160.RegistroD161.Add(d161);
                //d160.RegistroD162.Add(d162);
                //d100.RegistroD160.Add(d160);
                //d100.RegistroD170.Add(d170);

                //d100.RegistroD190.Add(d190);
                //d195.RegistroD197.Add(d197);
                //d100.RegistroD195.Add(d195);

                //sped.Bloco_D.RegistroD001.RegistroD100.Add(d100);
                
            }

            sped.Bloco_D.RegistroD001.IND_MOV = acheinf ? IndicadorMovimento.ComDados : IndicadorMovimento.SemDados;

        }

        private void SpedBlocoE(ACBrSpedFiscal sped)
        {
            sped.Bloco_E.DT_INI = sped.DT_INI;
            sped.Bloco_E.DT_FIN = sped.DT_FIN;

            RegistroE001 e001 = new RegistroE001 {IND_MOV = IndicadorMovimento.ComDados};

            RegistroE100 e100 = new RegistroE100
            {
                DT_INI = sped.DT_INI,
                DT_FIN = sped.DT_FIN
            };

            e100.RegistroE110.VL_TOT_DEBITOS = 0;
            e100.RegistroE110.VL_AJ_DEBITOS = 0;
            e100.RegistroE110.VL_TOT_AJ_DEBITOS = 0;
            e100.RegistroE110.VL_ESTORNOS_CRED = 0;
            e100.RegistroE110.VL_TOT_CREDITOS = 0;
            e100.RegistroE110.VL_AJ_CREDITOS = 0;
            e100.RegistroE110.VL_TOT_AJ_CREDITOS = 0;
            e100.RegistroE110.VL_ESTORNOS_DEB = 0;
            e100.RegistroE110.VL_SLD_CREDOR_ANT = 0;
            e100.RegistroE110.VL_SLD_APURADO = 0;
            e100.RegistroE110.VL_TOT_DED = 0;
            e100.RegistroE110.VL_ICMS_RECOLHER = 0;
            e100.RegistroE110.VL_SLD_CREDOR_TRANSPORTAR = 0;
            e100.RegistroE110.DEB_ESP = 0;
            
            sped.Bloco_E.RegistroE001.RegistroE100.Add(e100);

        }

        private void SpedBlocoG(ACBrSpedFiscal sped)
        {
            sped.Bloco_G.DT_INI = sped.DT_INI;
            sped.Bloco_G.DT_FIN = sped.DT_FIN;

            sped.Bloco_G.RegistroG001.IND_MOV = IndicadorMovimento.SemDados;

        }

        private void SpedBlocoH(ACBrSpedFiscal sped)
        {
            sped.Bloco_H.DT_INI = sped.DT_INI;
            sped.Bloco_H.DT_FIN = sped.DT_FIN;

            RegistroH001 h001 = new RegistroH001
            {
                IND_MOV = (sped.DT_INI.Month == 2) ?  IndicadorMovimento.ComDados : IndicadorMovimento.SemDados
            };

            if (h001.IND_MOV == IndicadorMovimento.ComDados)
            {
                decimal valor_inventario = 0;

                RegistroH005 h005 = new RegistroH005();

                h005.DT_INV = sped.DT_FIN; //o valor informado no campo deve ser menor ou igual ao valor no campo DT_FIN do registro 0000
                h005.VL_INV = valor_inventario;
                h005.MOT_INV = MotivoInventario.FinalPeriodo;

                if (valor_inventario > 0)
                {
                    // --- Criar uma tabela que armazene os dados do inventário. Esta consulta deve 
                    // --- buscar os dados desta tabela ao invéz de buscar diretamente da tabela de 
                    // --- produtos. Como não temos estoque inicialmente não ha problemas. 

                    List<ProdutosDTO> _produtos = new ProdutosSRV().BuscarPorTipoProduto(7).ToList();

                    //---------

                    foreach (var _item in _produtos)
                    {
                        RegistroH010 h010 = new RegistroH010();

                        h010.COD_ITEM = _item.PRO_ID.ToString();
                        h010.UNID = _item.PRO_UN_VEND;
                        h010.QTD = 0;
                        h010.VL_UNIT = (decimal)_item.PRO_PRECO_CUSTO;
                        h010.VL_ITEM = h010.QTD > 0 ? (h010.QTD * h010.VL_UNIT) : 0;
                        h010.IND_PROP = PosseItem.Informante;
                        h010.COD_PART = "";
                        h010.TXT_COMPL = "";
                        h010.COD_CTA = "";

                        //RegistroH020 h020 = new RegistroH020
                        //{
                        //    BC_ICMS = 0,
                        //    CST_ICMS = "",
                        //    VL_ICMS = 0
                        //};

                        //h010.RegistroH020.Add(h020);
                        h005.RegistroH010.Add(h010);

                    }
                }

                h001.RegistroH005.Add(h005);
            }
             
            sped.Bloco_H.RegistroH001 = h001;
            
        }

        private void SpedBloco1(ACBrSpedFiscal sped)
        {
            sped.Bloco_1.DT_INI = sped.DT_INI;
            sped.Bloco_1.DT_FIN = sped.DT_FIN;

            Registro1001 r1001 = new Registro1001();
            
            CONFIG_SPED_FISCAL _cfs = new ConfigSpedFiscalSRV().BuscarPorId(this.emp_id);

            Registro1010 r1010 = new Registro1010
            {
                IND_AER = _cfs.REG1010_IND_AER,
                IND_CART = _cfs.REG1010_IND_CART,
                IND_CCRF = _cfs.REG1010_IND_CCRF,
                IND_COMB = _cfs.REG1010_IND_COMB,
                IND_EE = _cfs.REG1010_IND_EE,
                IND_EXP = _cfs.REG1010_IND_EXP,
                IND_FORM = _cfs.REG1010_IND_FORM,
                IND_USINA = _cfs.REG1010_IND_USINA,
                IND_VA = _cfs.REG1010_IND_VA
            };

            r1001.Registro1010.Add(r1010);
            
            //----------

            if (_cfs.REG1010_IND_CART == "S")
            {

                List<TotalVendasCartaoDTO> _listatvc = new TotalVendasCartaoSRV().Buscar(emp_id, sped.DT_INI.Month, sped.DT_INI.Year).ToList();

                foreach (var _tvc in _listatvc)
                {
                    Registro1600 r1600 = new Registro1600
                    {
                        COD_PART = _tvc.FOR_ID.ToString(),
                        TOT_CREDITO = _tvc.TVC_VLR_CARTAO_CRE == null ? 0 : (decimal)_tvc.TVC_VLR_CARTAO_CRE,
                        TOT_DEBITO = _tvc.TVC_VLR_CARTAO_DEB == null ? 0 : (decimal)_tvc.TVC_VLR_CARTAO_DEB

                    };

                    r1001.Registro1600.Add(r1600);
                }
            }

            //if (r1001.Registro1600 == null || r1001.Registro1600.Count == 0)
            //    r1001.IND_MOV = IndicadorMovimento.SemDados;
            //else
                r1001.IND_MOV = IndicadorMovimento.ComDados;

            sped.Bloco_1.Registro1001 = r1001;
        }
        #endregion
    }
}


