using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using COAD.CORPORATIVO.Relatorio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Relatorios
{
    public class InfoArquivo
    {
        public string nome { get; set; }
        public string path { get; set; }
        public DateTime data { get; set; }
        public long tamanho { get; set; }
    }
    public class etiqueta
    {
        public string texto { get; set; }
        public string nome { get; set; }
        public string nome2 { get; set; }
        public string logadouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string municipio { get; set; }
    }
    public class etiquetaAvulsa
    {
        public string assinatura { get; set; }
        public int produto { get; set; }
    }

    public class EtiquetasController : Controller
    {
        //
        // GET: /Etiquetas/
        private HistAtendSRV _histatendSRV = new HistAtendSRV();
        private TipoAtendimentoSRV _tipoAtendSRV = new TipoAtendimentoSRV();
        private AcaoAtendimentoSRV _acaoAtendSRV = new AcaoAtendimentoSRV();
        private ClienteEnderecoSRV _endSRV = new ClienteEnderecoSRV();
        AssinaturaSRV _anssrv = new AssinaturaSRV();
        ProdutosSRV _prosrv = new ProdutosSRV();

        public ActionResult Avulsa()
        {
            return View();
        }
        [Autorizar(PorMenu = true)]
        public ActionResult Index()
        {
            _preencherCombos();

            return View();
        }

        public ActionResult ImprimirEtiquetaSolicitacao()
        {
            JSONResponse result = new JSONResponse();
        
            var  lstetiqueta = new List<etiqueta>();

            try
            {
                var  _listahistatend = _histatendSRV.BuscarEtiquetas();

                if (_listahistatend.Count > 0)
                {  
                    
                    foreach (var _item in _listahistatend)
                    {
                        AssinaturaDTO _asn = _anssrv.FindById(_item.ASN_NUM_ASSINATURA);

                        if (_asn != null)
                        {
                            ClienteDto _cli = _asn.CLIENTES;
                            ProdutosDTO _pro = _prosrv.FindById(_asn.PRO_ID);

                            var _end = _endSRV.FindEnderecoCliente((int)_cli.CLI_ID, 1);
                            var _adicional = _asn.ASN_MATERIA_ADICIONAL;

                            etiqueta etiq = new etiqueta();

                            var _nome = _asn.CLIENTES.CLI_NOME.PadRight(70, ' ');

                            etiq.texto = _item.HAT_DESCRICAO + _adicional;
                            etiq.nome = _asn.PRO_ID.ToString() + " " + _nome.Substring(0, 30);
                            etiq.nome2 = _nome.Substring(30, 33);

                            if (_end != null)
                            {
                                etiq.logadouro = _end.END_LOGRADOURO;
                                etiq.complemento = _end.END_NUMERO + " " + _end.END_COMPLEMENTO;
                                etiq.bairro = _end.END_CEP + " " + _end.END_BAIRRO;

                                etiq.texto = _item.HAT_DESCRICAO + _adicional;
                                etiq.nome = _asn.PRO_ID.ToString() + " " + _nome.Substring(0, 30);
                                etiq.nome2 = _nome.Substring(30, 33);

                                if (_end != null)
                                {
                                    etiq.logadouro = _end.END_LOGRADOURO;
                                    etiq.complemento = _end.END_NUMERO + " " + _end.END_COMPLEMENTO;
                                    etiq.bairro = _end.END_CEP + " " + _end.END_BAIRRO;

                                    if (_end.MUNICIPIO != null)
                                        etiq.municipio = _end.MUNICIPIO.MUN_DESCRICAO + " " + _end.MUNICIPIO.UF;
                                }

                                lstetiqueta.Add(etiq);
                            }
                        }
                    }

                    _histatendSRV.MarcarEtiquetasEmitidas();

                    var _retorno = this.Imprimir(lstetiqueta);

                    result.Add("retorno", _retorno);
                    result.success = true;
                    result.message = Message.Info("Etiqueta impressa com sucesso!!");

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.success = false;
                    result.message = Message.Fail("Nenhuma etiqueta foi encontrada para a impressão");
                    return Json(result, JsonRequestBehavior.AllowGet);
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

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [Autorizar(IsAjax = true)]
        public ActionResult ImprimirAvulsa(List<etiquetaAvulsa> _etiquetas)
        {
            JSONResponse result = new JSONResponse();

            List<etiqueta> lstetiqueta = new List<etiqueta>();

            try
            {
                if (_etiquetas.Count > 0)
                {
                    foreach (var _item in _etiquetas)
                    {
                        AssinaturaDTO _asn = _anssrv.FindById(_item.assinatura);
                        ClienteDto _cli = _asn.CLIENTES;
                        ProdutosDTO _pro = _prosrv.FindById(_item.produto);

                        var _end = _endSRV.FindEnderecoCliente((int)_cli.CLI_ID, 1);
                        var _adicional = _asn.ASN_MATERIA_ADICIONAL;

                        etiqueta etiq = new etiqueta();

                        etiq.texto = _pro.PRO_NOME + _adicional;
                        etiq.nome = _cli.CLI_NOME;
                        etiq.nome2 = _asn.ASN_A_C;

                        if (_end != null)
                        {
                            etiq.logadouro = _end.END_LOGRADOURO;
                            etiq.complemento = _end.END_NUMERO + " " + _end.END_COMPLEMENTO;
                            etiq.bairro = _end.END_CEP + " " + _end.END_BAIRRO;

                            if (_end.MUNICIPIO != null)
                                etiq.municipio = _end.MUNICIPIO.MUN_DESCRICAO + " " + _end.MUNICIPIO.UF;
                        }

                        lstetiqueta.Add(etiq);
                    }

                    var _retorno = this.Imprimir(lstetiqueta);
                    result.Add("retorno", _retorno);
                    result.success = true;
                    result.message = Message.Info("Etiqueta avulsa impressa com sucesso!!");

                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    result.success = false;
                    result.message = Message.Fail("Nenhuma etiqueta foi encontrada para a impressão");
                    return Json(result, JsonRequestBehavior.AllowGet);
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

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [Autorizar(IsAjax = true)]
        public string Imprimir(List<etiqueta> _lista)
        {
            List<etiqueta> lstetiqueta = new List<etiqueta>();
            try
            {
                string _linhas = "";
                char chr = Convert.ToChar(" ");

                if (_lista.Count > 0)
                {
                    foreach (var _item in _lista)
                    {

                        etiqueta etiq = new etiqueta();

                        etiq.texto = _item.texto;
                        etiq.nome = _item.nome;
                        etiq.nome2 = _item.nome2;
                        etiq.logadouro = _item.logadouro;
                        etiq.complemento = _item.complemento;
                        etiq.bairro = _item.bairro;
                        etiq.municipio = _item.municipio;

                        //------------

                        etiq.texto = etiq.texto == null ? " " : etiq.texto;
                        etiq.nome = etiq.nome == null ? " " : etiq.nome;
                        etiq.nome2 = etiq.nome2 == null ? " " : etiq.nome2;
                        etiq.logadouro = etiq.logadouro == null ? " " : etiq.logadouro;
                        etiq.complemento = etiq.complemento == null ? " " : etiq.complemento;
                        etiq.bairro = etiq.bairro == null ? " " : etiq.bairro;
                        etiq.municipio = etiq.municipio == null ? " " : etiq.municipio;

                        //-------------

                        string _nome = etiq.nome.PadRight(35, chr);
                        string _logaradouro = etiq.logadouro.PadRight(35, chr);

                        etiq.texto = etiq.texto.PadRight(34, chr);
                        etiq.nome = _nome.Substring(0, 33).PadRight(34, chr);
                        etiq.nome2 = etiq.nome2.PadRight(34, chr);
                        etiq.logadouro = _logaradouro.Substring(0, 33).PadRight(34, chr);
                        etiq.complemento = etiq.complemento.PadRight(34, chr);
                        etiq.bairro = etiq.bairro.PadRight(34, chr);
                        etiq.municipio = etiq.municipio.PadRight(34, chr);

                        //-------------

                        lstetiqueta.Add(etiq);

                        if (lstetiqueta.Count > 2)
                        {
                            _linhas += " " + lstetiqueta[0].texto + "  " + lstetiqueta[1].texto + "  " + lstetiqueta[2].texto + "\n";
                            _linhas += " " + lstetiqueta[0].nome + "  " + lstetiqueta[1].nome + "  " + lstetiqueta[2].nome + "\n";
                            _linhas += " " + lstetiqueta[0].nome2 + "  " + lstetiqueta[1].nome2 + "  " + lstetiqueta[2].nome2 + "\n";
                            _linhas += " " + lstetiqueta[0].logadouro + "  " + lstetiqueta[1].logadouro + "  " + lstetiqueta[2].logadouro + "\n";
                            _linhas += " " + lstetiqueta[0].complemento + "  " + lstetiqueta[1].complemento + "  " + lstetiqueta[2].complemento + "\n";
                            _linhas += " " + lstetiqueta[0].bairro + "  " + lstetiqueta[1].bairro + "  " + lstetiqueta[2].bairro + "\n";
                            _linhas += " " + lstetiqueta[0].municipio + "  " + lstetiqueta[1].municipio + "  " + lstetiqueta[2].municipio + "\n";
                            _linhas += "\n\n";

                            lstetiqueta.Clear();

                        }

                    }
                }
                else
                {
                    throw new Exception(@"Nenhum resultado encontrado para a pesquisa.");
                }

                if (lstetiqueta.Count > 0)
                {
                    bool col2 = (lstetiqueta.Count > 1);
                    bool col3 = (lstetiqueta.Count > 2);

                    if (!col2 && !col3)
                    {
                        _linhas += " " + lstetiqueta[0].texto + "\n";
                        _linhas += " " + lstetiqueta[0].nome + "\n";
                        _linhas += " " + lstetiqueta[0].nome2 + "\n";
                        _linhas += " " + lstetiqueta[0].logadouro + "\n";
                        _linhas += " " + lstetiqueta[0].complemento + "\n";
                        _linhas += " " + lstetiqueta[0].bairro + "\n";
                        _linhas += " " + lstetiqueta[0].municipio + "\n";
                    }

                    if (col2 && !col3)
                    {
                        _linhas += " " + lstetiqueta[0].texto + "   " +
                                         lstetiqueta[1].texto + "\n";
                        _linhas += " " + lstetiqueta[0].nome + "   " +
                                         lstetiqueta[1].nome + "\n";
                        _linhas += " " + lstetiqueta[0].nome2 + "   " +
                                         lstetiqueta[1].nome2 + "\n";
                        _linhas += " " + lstetiqueta[0].logadouro + "   " +
                                         lstetiqueta[1].logadouro + "\n";
                        _linhas += " " + lstetiqueta[0].complemento + "   " +
                                         lstetiqueta[1].complemento + "\n";
                        _linhas += " " + lstetiqueta[0].bairro + "   " +
                                         lstetiqueta[1].bairro + "\n";
                        _linhas += " " + lstetiqueta[0].municipio + "   " +
                                         lstetiqueta[1].municipio + "\n";
                    }
                    if (col2 && col3)
                    {
                        _linhas = " " + lstetiqueta[0].texto + "   " +
                                         lstetiqueta[1].texto + "   " +
                                         lstetiqueta[2].texto + "\n";
                        _linhas += " " + lstetiqueta[0].nome + "   " +
                                         lstetiqueta[1].nome + "   " +
                                         lstetiqueta[2].nome + "\n";
                        _linhas += " " + lstetiqueta[0].nome2 + "   " +
                                         lstetiqueta[1].nome2 + "   " +
                                         lstetiqueta[2].nome2 + "\n";
                        _linhas += " " + lstetiqueta[0].logadouro + "   " +
                                         lstetiqueta[1].logadouro + "   " +
                                         lstetiqueta[2].logadouro + "\n";
                        _linhas += " " + lstetiqueta[0].complemento + "   " +
                                         lstetiqueta[1].complemento + "   " +
                                         lstetiqueta[2].complemento + "\n";
                        _linhas += " " + lstetiqueta[0].bairro + "   " +
                                         lstetiqueta[1].bairro + "   " +
                                         lstetiqueta[2].bairro + "\n";
                        _linhas += " " + lstetiqueta[0].municipio + "   " +
                                         lstetiqueta[1].municipio + "   " +
                                         lstetiqueta[2].municipio + "\n";
                    }


                }

                string curDir = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());

                var arq = DateTime.Now.Millisecond.ToString() +
                          DateTime.Now.Second.ToString() +
                          DateTime.Now.Minute.ToString();

                curDir = curDir + "\\temp\\ET015" + arq + ".txt";

                System.IO.File.WriteAllText(curDir, _linhas, System.Text.Encoding.GetEncoding("ISO-8859-1"));

                System.IO.File.Copy(curDir, "\\\\rj-app-srv\\share\\DADOS\\e\\Ferramentas_Delphi\\SPOOL\\PJ005\\ET015" + arq + ".txt", true);
                
                string _Path = "Http://" + HttpContext.Request.Url.Authority + "/temp/ET015" + arq + ".txt";

                return _Path;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            

        }

        private void _preencherCombos()
        {
            var ListaTipoAtendimento = _tipoAtendSRV.FindAll();

            ViewBag.ListaTipoAtendimento = new SelectList(ListaTipoAtendimento, "TIP_ATEND_ID", "TIP_ATEND_DESCRICAO");

            var ListaAcaoAtendimento = _acaoAtendSRV.FindAll();

            ViewBag.ListaAcaoAtendimento = new SelectList(ListaAcaoAtendimento, "ACA_ID", "ACA_DESCRICAO");

        }
        public ActionResult Pesquisar(string _asn_id, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, bool _etiqueta = false, int pagina = 1)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                Pagina<HistoricoAtendimentoDTO> _listahistatend = _histatendSRV.BuscarPorPeriodo(_asn_id, _dtini, _dtfim, _etiqueta, pagina);

                if (_listahistatend.numeroPaginas > 0)
                {
                    response.success = true;
                    response.message = Message.Info("OK");
                    response.AddPage("listahistatend", _listahistatend);
                }
                else
                {
                    response.success = false;
                    response.message = Message.Fail(@"Nenhum resultado encontrado para a pesquisa.");
                }
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BuscarProduto(int _pro_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                ProdutosDTO _pro = _prosrv.FindById(_pro_id);

                if (_pro != null)
                {
                    response.success = true;
                    response.message = Message.Info("OK");
                    response.Add("produto", _pro);
                }
                else
                {
                    response.success = false;
                    response.message = Message.Fail(@"Nenhum resultado encontrado para a pesquisa.");
                }
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BuscarCliente(string _asn_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var _ass = _anssrv.FindById(_asn_id);

                if (_ass != null)
                {
                    
                    var _cli = new ClienteSRV().FindById(_ass.CLI_ID);

                    if (_cli != null)
                    {
                        _ass.CLIENTES = _cli;

                        var _end = _endSRV.FindEnderecoCliente((int)_cli.CLI_ID, 1);

                        if (_end == null)
                            throw new Exception("Cliente com endereço inválido!! Verifique!!");

                        if (_end.END_CEP == "" ||
                            _end.END_CEP == null ||
                            _end.END_LOGRADOURO == "" ||
                            _end.END_LOGRADOURO == null)
                            throw new Exception("Cliente com endereço inválido!! Verifique!!");

                        response.success = true;
                        response.Add("cliente", _cli);
                    }
                    else
                        throw new Exception("Cliente não encontrado");
                }
                else
                {
                    throw new Exception("Assinatura não encontrada");
                }

                response.success = true;
                return Json(response, JsonRequestBehavior.AllowGet);
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

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ListarArquivos(string _patharquivo)
        {
            List<InfoArquivo> _lista = new List<InfoArquivo>();
            JSONResponse response = new JSONResponse();
            try
            {
                if (_patharquivo == null || _patharquivo == "")
                    _patharquivo = "\\\\rj-app-srv\\share\\DADOS\\e\\Ferramentas_Delphi\\SPOOL\\PJ005\\";

                DirectoryInfo Dir = new DirectoryInfo(_patharquivo);
                FileInfo[] Files = Dir.GetFiles("*", SearchOption.AllDirectories);
                foreach (FileInfo File in Files)
                {
                    InfoArquivo arq = new InfoArquivo();

                    arq.nome = File.Name;
                    arq.path = "file://rj-app-srv/share/DADOS/e/Ferramentas_Delphi/SPOOL/PJ005/" + File.Name;
                    arq.data = File.CreationTime;
                    arq.tamanho = File.Length;

                    _lista.Add(arq);
                }

                if (_lista.Count() > 0)
                {
                    response.success = true;
                    response.message = Message.Info("OK");
                    response.Add("listaarquivos", _lista);
                }
                else
                {
                    response.success = false;
                    response.message = Message.Fail(@"Nenhum resultado encontrado para a pesquisa.");
                }

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = Message.Fail(ex);
            }

            return Json("Erro ao buscar lista de arquivos..", JsonRequestBehavior.AllowGet);
        }

    }

}
