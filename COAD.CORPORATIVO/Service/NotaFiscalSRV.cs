using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.DAO;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model;
using System.Xml;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Exceptions;
using COAD.FISCAL.Model;
using GenericCrud.Util;
using COAD.FISCAL.Model.Enumerados;
using COAD.FISCAL.Service;
using System.Web;
using GenericCrud.Service;
using COAD.CORPORATIVO.Service.Custons;
using GenericCrud.Validations;
using Coad.GenericCrud.Exceptions;
using COAD.SEGURANCA.Service;
using System.Transactions;
using GenericCrud.Excel;
using COAD.CORPORATIVO.Model.Dto.Custons.FonteDadosTemplate;
using System.IO;
using COAD.SEGURANCA.Service.Interfaces;
using COAD.SEGURANCA.Model.Custons;
using COAD.CORPORATIVO.Util;
using COAD.SEGURANCA.DAO;
using System.Globalization;
using GenericCrud.Config.DataAttributes;
using System.Xml.Serialization;
using System.Xml.Linq;
using COAD.FISCAL.Model.Servicos;
using COAD.FISCAL.Model.Servicos.Retornos;
using COAD.FISCAL.Service.Integracoes;
using COAD.FISCAL.Model.Integracoes.Interfaces;
using COAD.FISCAL.Service.Integracoes.Interfaces;
using COAD.FISCAL.Model.Integracoes;
using COAD.FISCAL.Exceptions;
using COAD.SEGURANCA.Service.Custons.Context;
using COAD.FISCAL.Model.Integracoes.Enumerados;
using COAD.SEGURANCA.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto.FiltersInfo;
using COAD.FISCAL.XmlUtils;
using COAD.FISCAL.Model.NFSe;
using COAD.FISCAL.Model.NFSe.Retornos;
using COAD.FISCAL.Model.NFSe.Enumerados;
using System.Threading;
using COAD.CORPORATIVO.Model.Dto.Custons.Batch;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("NF_ID")]
    public class NotaFiscalSRV : ServiceAdapter<NOTA_FISCAL, NotaFiscalDTO>
    {
        private NotaFiscalDAO _dao;
        public TemplateHTMLSRV _templateHTMLSRV { get; set; }
        public IntegrNfeSRV _integrNFe { get; set; }
        public NFeXmlSRV _nfeXmlSRV { get; set; }
        

        public FornecedorDTO _frn = new FornecedorDTO();
        public ClienteDto _cli = new ClienteDto();
        public TransportadorDTO _tra = new TransportadorDTO();
        public MunicipioSRV _munsrv { get; set; }
        public MunicipioDTO _mun = new MunicipioDTO();
        public ClienteSRV _clisrv { get; set; }
        public EmpresaSRV _empsrv { get; set; }
        public EmpresaModel _emp = null;


        public NotaFiscalSRV()
        {
            this._dao = new NotaFiscalDAO();
            SetDao(_dao);
            _templateHTMLSRV = new TemplateHTMLSRV();
            _integrNFe = new IntegrNfeSRV();
            _nfeXmlSRV = new NFeXmlSRV();
            _munsrv = new MunicipioSRV();
            _clisrv = new ClienteSRV();
            _empsrv = new EmpresaSRV();
        }  

        public NotaFiscalSRV(NotaFiscalDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
        }

        public NotaFiscalDTO Buscar(int _nf_id)
        {
            return _dao.Buscar(_nf_id);
        }
        public NotaFiscalDTO Buscar(int _nf_numero, string _nf_serie, int _nf_tipo, int _cli_id)
        {
            return _dao.Buscar(_nf_numero, _nf_serie, _nf_tipo, _cli_id);
        }
        public IList<NotaFiscalDTO> BuscarNfeCliente(int _cli_id)
        {
            return _dao.BuscarNfeCliente(_cli_id);
        }
        public Pagina<NotaFiscalDTO> BuscarPorPeriodo(int? _nf_numero, string _cpfCnpj, int numpagina = 1, int linhas = 10, bool? antecipada = null, bool? avulsa = null)
        {
            Pagina<NotaFiscalDTO> _listanotas = _dao.BuscarPorPeriodo(_nf_numero, _cpfCnpj, numpagina, linhas, antecipada, avulsa);

            return _listanotas;
        }
        public Pagina<NotaFiscalDTO> BuscarPorPeriodo(int _nf_tipo, int _for_id, int _emp_id, DateTime? _dtini, DateTime? _dtfim, int numpagina = 1, int linhas = 10, bool? antecipada = null, bool? avulsa = null)
        {
            return _dao.BuscarPorPeriodo(_nf_tipo, _for_id, _emp_id, _dtini, _dtfim, numpagina, linhas, antecipada, avulsa);
        }
        public Pagina<NotaFiscalDTO> BuscarNotasPeriodo(int _emp_id, int _nf_tipo, DateTime _dtini, DateTime _dtfim, int numpagina = 1, int linhas = 10)
        {
            return _dao.BuscarNotasPeriodo(_emp_id, _nf_tipo, _dtini, _dtfim, numpagina, linhas);
        }

        public List<NotaFiscalDTO> BuscarPorPeriodo(int _nf_tipo, int _for_id, int _emp_id, DateTime? _dtini, DateTime? _dtfim)
        {
            return _dao.BuscarPorPeriodo(_nf_tipo, _for_id, _emp_id, _dtini, _dtfim).ToList();
        }
        public IList<NotaFiscalCustomDTO> BuscarNotasPeriodoSintetico(DateTime _dtini, DateTime _dtfim)
        {
            return _dao.BuscarNotasPeriodoSintetico(_dtini, _dtfim);
        }
  

        public byte[] XmlDocToBytes(XmlDocument _doc)
        {
            return _dao.XmlDocToBytes(_doc);
        }

        public XmlDocument BytesToXmlDoc(byte[] _linhas)
        {

            return _dao.BytesToXmlDoc(_linhas);
        }
        public ProdutosDTO VerficarIncluir(ProdutosDTO _pro, int _tiponf)
        {
            var _produto = new ProdutosDTO();
            var _prosrv = new ProdutosSRV();

            if (_pro.PRO_ID > 0)
                _produto = _prosrv.FindById(_pro.PRO_ID);
            else
                _produto = _prosrv.BuscarPorNCMDescricao(_pro.NCM_ID, _pro.PRO_NOME);

            if (_produto == null)
            {
                if (_tiponf == 1)
                    _prosrv.InsertSemIdentity(_pro.PRO_ID, _pro.PRO_SIGLA, _pro.PRO_NOME, _pro.NCM_ID);
                else
                    _prosrv.SalvarProduto(_pro);
            }
            else
            {
                _pro.PRO_ID = _produto.PRO_ID;
                _pro.PRO_SIGLA = _produto.PRO_SIGLA;
                _pro.PRO_NOME = _produto.PRO_NOME;
                _pro.PRO_ID_DERVADO = _produto.PRO_ID_DERVADO;
                _pro.PRO_MOD_CARTA_URA = _produto.PRO_MOD_CARTA_URA;
                _pro.PRO_TIPO_REMESSA = _produto.PRO_TIPO_REMESSA;
                _pro.PRO_RECEBE_MALA = _produto.PRO_RECEBE_MALA;
                _pro.PRO_RECEBE_PASTA_SN = _produto.PRO_RECEBE_PASTA_SN;
                _pro.PRO_PRODUTO_ACABADO = _produto.PRO_PRODUTO_ACABADO;
                _pro.PRO_STATUS = _produto.PRO_STATUS;
                _pro.PRO_EMITE_NF = _produto.PRO_EMITE_NF;
                _pro.NCM_ID = _produto.NCM_ID;
                _pro.GRUPO_ID = _produto.GRUPO_ID;
                _pro.DATA_CADASTRO = _produto.DATA_CADASTRO;
                _pro.DATA_ALTERA = _produto.DATA_ALTERA;
                _pro.PRO_UN_COMPRA = _produto.PRO_UN_COMPRA;
                _pro.PRO_UN_VEND = _produto.PRO_UN_VEND;
                _pro.PRO_PRECO_COMPRA = _produto.PRO_PRECO_COMPRA;
                _pro.PRO_PRECO_CUSTO = _produto.PRO_PRECO_CUSTO;
                _pro.AREA_ID = _produto.AREA_ID;
                _pro.TIPO_PRO = _produto.TIPO_PRO;
            }

            return _pro;
        }

        /// <summary>
        /// Esse método é identico ao "BuscarNotasPeriodo".  Foi criado por uma questão de compatibilidade 
        /// com telas que ja utilizavam esta consulta antes da implementação das classes DTO. Para novas 
        /// implementações o metodo "BuscarNotasPeriodo" deve ser utilizado.
        /// </summary>
        /// <param name="_emp_id"></param>
        /// <param name="_dtini"></param>
        /// <param name="_dtfim"></param>
        /// <returns></returns>
        public IList<NotaFiscalDTO> BuscarNotasPeriodoModel(int _emp_id, DateTime _dtini, DateTime _dtfim)
        {
            return _dao.BuscarNotasPeriodoModel(_emp_id, _dtini, _dtfim);
        }
        public List<NOTA_FISCAL> BuscarNotasPeriodoTipo(int _nf_tipo, int _emp_id, DateTime? _dtini, DateTime? _dtfim)
        {
            List<NOTA_FISCAL> _listanotas = _dao.BuscarNotasPeriodoTipo(_nf_tipo, _emp_id, _dtini, _dtfim);

            return _listanotas;
        }
        public virtual void IncluirNf(NotaFiscalDTO _nf)
        {
            if (_nf.NF_STATUS == "" || _nf.NF_STATUS == null)
            {
                _nf.NF_STATUS = "ATI";
            }

            decimal _totaitens = 0;

            if (_nf.NOTA_FISCAL_ITEM == null)
                throw new Exception("Nota fiscal sem itens. Verifique!!");

            if (_nf.NF_DATA_EMISSAO == null)
                throw new Exception("Data de emissão não informada. Verifique!!");

            if (_nf.NF_TIPO == 0)
            {
                if (_nf.NF_DATA_ENTRADA == null)
                    throw new Exception("Data de entrada não informada. Verifique!!");
            }

            if (_nf.NF_TIPO == 0 && _nf.FORNECEDOR == null)
                throw new Exception("Fornecedor não informado. Verifique!!");


            if (_nf.FORNECEDOR != null)
            {
                var _fornecedor = new FornecedorDTO();

                _fornecedor = _nf.FORNECEDOR;
                _fornecedor = new FornecedorDAO().VerficarIncluir(_fornecedor);
                _nf.FOR_ID = _fornecedor.FOR_ID;
                _nf.FORNECEDOR = null;
            }

            if (_nf.TRANSPORTADOR != null)
            {
                var _Transportador = new TransportadorDTO();

                _Transportador = _nf.TRANSPORTADOR;
                _Transportador = new TransportadorDAO().VerficarIncluir(_Transportador);
                _nf.TRA_ID = _Transportador.TRA_ID;
                _nf.TRANSPORTADOR = null;
            }

            foreach (NotaFiscalItemDTO _item in _nf.NOTA_FISCAL_ITEM)
            {
                //_prod.PRO_TIPO (0 - Consumo, 1 - Materia Prima, 2 - Revenda, 3 - Serviço)


                var _prod = new ProdutosDTO();
                _prod.PRO_ID = _item.PRO_ID;
                _prod.NCM_ID = _item.NCM_ID;
                _prod.PRO_SIGLA = _item.NFI_PRO_NOME.Length > 10 ? _item.NFI_PRO_NOME.Substring(0, 9) : _item.NFI_PRO_NOME;
                _prod.PRO_NOME = _item.NFI_PRO_NOME.Length > 70 ? _item.NFI_PRO_NOME.Substring(0, 69) : _item.NFI_PRO_NOME;
                _prod.PRO_UN_COMPRA = _item.NFI_UN;
                _prod.PRO_UN_VEND = _item.NFI_UN;
                _prod.PRO_PRECO_COMPRA = _item.NFI_VLR_UNIT;
                _prod.PRO_PRECO_CUSTO = _item.NFI_VLR_UNIT;
                _prod.PRO_PRECO_VENDA = 0;
                _prod.PRO_TIPO_REMESSA = 0;
                _prod.PRO_RECEBE_MALA = 0;
                _prod.PRO_RECEBE_PASTA_SN = 0;
                _prod.PRO_PRODUTO_ACABADO = 0;
                _prod.PRO_STATUS = 0;
                _prod.GRUPO_ID = 6;
                _prod.AREA_ID = 0;
                _prod.TIPO_PRO = 7;
                _prod.DATA_CADASTRO = DateTime.Now;

                this.VerficarIncluir(_prod, _nf.NF_TIPO);

                _item.PRO_ID = (int)_prod.PRO_ID;
                _item.NFI_PRO_NOME = _item.NFI_PRO_NOME.Length > 70 ? _item.NFI_PRO_NOME.Substring(0, 69) : _item.NFI_PRO_NOME;
                _item.PRODUTOS = null;

                _totaitens += (decimal)_item.NFI_VLR_TOTAL;

            }

            double _dif = (double)(_totaitens - (decimal)_nf.NF_VLR_PROD);
            double _vlrnf = (double)_nf.NF_VLR_PROD;

            if (_nf.NF_TIPO == 0)
            {
                if ((_dif > 0.01) || (_dif < -0.01))
                    throw new Exception("Existe diferença entre soma dos itens (" + _totaitens.ToString("R$ 0.00") + ") e o valor total dos produtos (" + _vlrnf.ToString("R$ 0.00") + ") informado na nota. A diferença é de (" + _dif.ToString("R$ 0.00") + "). Verifique!");
            }

            if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                _nf.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
            else
                _nf.USU_LOGIN = SessionContext.usu_login_desktop;


            foreach (NotaFiscalItemDTO _item in _nf.NOTA_FISCAL_ITEM)
            {
                _item.NF_ID = _nf.NF_ID;
                _item.NF_NUMERO = _nf.NF_NUMERO;
                _item.NF_SERIE = _nf.NF_SERIE;
                _item.NF_TIPO = _nf.NF_TIPO;
            }

            
            this.Save(_nf);

            if (_nf.FOR_ID != null)
            {
                foreach (var _item in _nf.NOTA_FISCAL_ITEM)
                {

                    PRODUTO_FORNECEDOR _proforn = new PRODUTO_FORNECEDOR();
                    _proforn.PRO_ID = _item.PRO_ID;
                    _proforn.FOR_ID = (int)_nf.FOR_ID;
                    _proforn.PFO_ATIVO = 1;
                    new ProdutoFornecedorDAO().VerficarIncluir(_proforn);

                }
            }

        }
        public virtual void SalvarNf(NotaFiscalDTO _nota)
        {
            decimal _totaitens = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                if (_nota.NF_ID == 0)
                    new NotaFiscalSRV().IncluirNf(_nota);
                else
                {
                    if (_nota.NOTA_FISCAL_ITEM == null)
                        throw new Exception("Nota fiscal sem itens. Verifique!!");

                    if (_nota.NF_DATA_EMISSAO == null)
                        throw new Exception("Data de emissão não informada. Verifique!!");

                    if (_nota.NF_TIPO == 0)
                        if (_nota.NF_DATA_ENTRADA == null)
                            throw new Exception("Data de entrada não informada. Verifique!!");

                    if (_nota.NF_TIPO == 0 && _nota.FOR_ID == null)
                        throw new Exception("Fornecedor não informado. Verifique!!");

                    _nota.FORNECEDOR = null;

                    if (_nota.CLIENTES != null)
                        _nota.CLIENTES = null;

                    if (_nota.TRANSPORTADOR != null)
                        _nota.TRANSPORTADOR = null;

                    foreach (var _item in _nota.NOTA_FISCAL_ITEM)
                    {
                        _item.PRODUTOS = null;
                        _totaitens += (decimal)_item.NFI_VLR_TOTAL;
                    }

                    double _dif = (double)(_totaitens - (decimal)_nota.NF_VLR_PROD);
                    double _vlrnf = (double)_nota.NF_VLR_PROD;

                    if (_nota.NF_TIPO == 0)
                    {
                        if ((_dif > 0.01) || (_dif < -0.01))
                            throw new Exception("Existe diferença entre soma dos itens (" + _totaitens.ToString("R$ 0.00") + ") e o valor total dos produtos (" + _vlrnf.ToString("R$ 0.00") + ") informado na nota. A diferença é de (" + _dif.ToString("R$ 0.00") + "). Verifique!");
                    }

                    if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                        _nota.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                    else
                        _nota.USU_LOGIN = SessionContext.usu_login_desktop;

                    var _nfitemSRV = ServiceFactory.RetornarServico<NotaFiscalItemSRV>();

                    _nfitemSRV.DeleteAll(_nota.NOTA_FISCAL_ITEM);

                    foreach (var _item in _nota.NOTA_FISCAL_ITEM)
                    {
                        
                        _nfitemSRV.SaveOrUpdate(_item);
                    }
                    
                
                    this.Merge(_nota);
                }

                scope.Complete();
            }
        }
    
        public virtual void ExcluirNf(NotaFiscalDTO _nota)
        {
            _dao.ExcluirNf(_nota);
        }
        public IList<NotaFiscalDTO> BuscarNFEntradaPeriodo(int _emp_id, DateTime? _dtini, DateTime? _dtfim)
        {
            return _dao.BuscarNFEntradaPeriodo(_emp_id, _dtini, _dtfim);
        }
        public IList<NotaFiscalDTO> BuscarNFEntradaServicoPeriodo(int _emp_id, DateTime? _dtini, DateTime? _dtfim)
        {
            return _dao.BuscarNFEntradaServicoPeriodo(_emp_id, _dtini, _dtfim);
        }
        public IList<NotaFiscalDTO> BuscarNFSaidaPeriodo(int _emp_id, DateTime? _dtini, DateTime? _dtfim)
        {
            return _dao.BuscarNFSaidaPeriodo(_emp_id, _dtini, _dtfim);
        }
        public IList<NotaFiscalDTO> BuscarNFSaidaServicoPeriodo(int _emp_id, DateTime? _dtini, DateTime? _dtfim)
        {
            return _dao.BuscarNFSaidaServicoPeriodo(_emp_id, _dtini, _dtfim);
        }
        public IList<FORNECEDOR> BuscarFornecedorNFPeriodo(int _emp_id, DateTime _dtinicial, DateTime _dtfinal)
        {
            return _dao.BuscarFornecedorNFPeriodo(_emp_id, _dtinicial, _dtfinal);
        }
        public IList<CLIENTES> BuscarClientesNFPeriodo(int _emp_id, DateTime _dtinicial, DateTime _dtfinal)
        {
            return _dao.BuscarClientesNFPeriodo(_emp_id, _dtinicial, _dtfinal);
        }
        public List<PRODUTOS> BuscarProdutosNFPeriodo(int _emp_id, DateTime _dtini, DateTime _dtfim)
        {
            return _dao.BuscarProdutosNFPeriodo(_emp_id, _dtini, _dtfim);
        }
        /// <summary>
        /// Este metodo foi alterado e atualmente so retorna CFOP de notas de entrada.
        /// </summary>
        /// <param name="_emp_id"></param>
        /// <param name="_dtini"></param>
        /// <param name="_dtfim"></param>
        /// <returns></returns>
        public List<CFOP_TABLE> BuscarNFCFOP(int _emp_id, DateTime _dtini, DateTime _dtfim)
        {
            return _dao.BuscarNFCFOP(_emp_id, _dtini, _dtfim);
        }
        public NotaFiscalDTO ImportarXML(string _arqXml, int _emp_id)
        {
            XmlDocument _doc = new XmlDocument();

            _doc.Load(HttpContext.Current.Server.MapPath("~/temp/" + _arqXml));

            var _nf = this.GravarDadosXML(_doc, _emp_id);

            if (_nf != null && _nf.NF_STATUS != "ATI")
                return _nf;
            else
                return null;

        }
        /// <summary>
        /// Gera o xml da nota fiscal baseado no pedido
        /// </summary>
        /// <param name="nfeRequest"></param>
        public IList<NfeXmlDTO> GerarXMLDaNotaFiscal(GeracaoNFeRequestDTO nfeRequest)
        {
            if (nfeRequest == null)
            {
                throw new NFeException("Não é possível gerar o xml da nota fiscal. O argumento está nullo.");
            }

            var cliente = nfeRequest.cliente;
            var endereco = nfeRequest.endereco;
            var empresa = nfeRequest.empresa;
            var itemPedido = nfeRequest.itemPedido;
            var produto = nfeRequest.produto;
            var produtoComposicao = nfeRequest.produtoComposicao;
            var cfop = nfeRequest.cfop;
            var path = nfeRequest.path;
            var nfx_id = nfeRequest.NFX_ID;
            TNFeInfNFeCobr cobranca = nfeRequest.cobranca;
            
            if (cliente == null)
            {
                throw new NFeException("Não é possível gerar o xml da nota fiscal. Falta informações do Cliente");
            }

            if (endereco == null)
            {
                throw new NFeException("Não é possível gerar o xml da nota fiscal. Falta endereço do Cliente");
            }

            if (empresa == null)
            {
                throw new NFeException("Não é possível gerar o xml da nota fiscal. Falta dados da empresa.");
            }


            if (itemPedido == null)
            {
                throw new NFeException("Não é possível gerar o xml da nota fiscal. Falta informações do item do pedido");
            }


            if (produto == null)
            {
                throw new NFeException("Não é possível gerar o xml da nota fiscal. Falta informações do produto");
            }

            if (produtoComposicao == null)
            {
                throw new NFeException("Não é possível gerar o xml da nota fiscal. Falta informações do produto composição");
            }

            if (nfeRequest.DataFaturamento == null)
            {
                string mensagem = "Não é possível gerar o xml da nota fiscal. Não é possível encontrar a data de faturamento no Item de Pedido {0}";
                mensagem = string.Format(mensagem, itemPedido.IPE_ID);
                throw new NFeException(mensagem);
            }

            if (nfx_id == null)
                ValidarDataFaturamento(empresa.EMP_ID, nfeRequest.DataFaturamento, nfeRequest.CodContrato, nfeRequest.IpeId);

            var produtoComposicaoSRV = new ProdutoComposicaoSRV();
            produtoComposicaoSRV.ChecaEMarcaProdutoCurso(produtoComposicao);

            if (produtoComposicao.EhCurso)
            {
                // TODO: Alterar comportamento específico no futuro.
                var retorno = GerarNfeDTO(empresa, cliente, endereco, produtoComposicao, produto, itemPedido, nfeRequest.DataFaturamento, path,null, nfx_id, cobranca);
                return new List<NfeXmlDTO>() { retorno };
            }
            else
            {
                var retorno = GerarNfeDTO(empresa, cliente, endereco, produtoComposicao, produto, itemPedido, nfeRequest.DataFaturamento, path, null, nfx_id, cobranca);
                return new List<NfeXmlDTO>() { retorno };
            }
        }

        public NotaFiscalDTO AtualizarNotaFiscalDadosXML(XmlDocument _doc, int _emp_id)
        {
            var _nf = new NotaFiscalDTO();

            try
            {
                string _cnpjcpf = this.RetornaStringDoCampo(_doc, "dest", "CNPJ");

                if (_cnpjcpf == null || _cnpjcpf == "")
                    _cnpjcpf = this.RetornaStringDoCampo(_doc, "dest", "CPF");

                var _cli = new ClienteDAO().BuscarPorCNPJ(_cnpjcpf);

                if (_cli == null)
                    throw new Exception("Cliente não encontrado! Cnpj/Cpf => " + _cnpjcpf);

                if (_cli != null)
                    _nf.CLI_ID = _cli.CLI_ID;

                if (this.RetornaStringDoCampo(_doc, "ide", "tpNF") == "1")
                {
                    _nf.NF_SERIE = this.RetornaStringDoCampo(_doc, "ide", "serie");
                    _nf.NF_NUMERO = System.Convert.ToInt32(this.RetornaStringDoCampo(_doc, "ide", "nNF"));

                    if ((this.RetornaStringDoCampo(_doc, "emit", "CNPJ") == "33832759000190") ||
                        (this.RetornaStringDoCampo(_doc, "emit", "CNPJ") == "27922913000111") ||
                        (this.RetornaStringDoCampo(_doc, "emit", "CNPJ") == "15165950000143") ||
                        (this.RetornaStringDoCampo(_doc, "emit", "CNPJ") == "33832759000351"))
                        _nf.NF_TIPO = (int)EnumTipoNF.Saida;
                    else
                        _nf.NF_TIPO = (int)EnumTipoNF.Entrada;

                    _nf = this.Buscar(_nf.NF_NUMERO, _nf.NF_SERIE, _nf.NF_TIPO, (int)_nf.CLI_ID);

                    if (_nf != null)
                    {
                        _nf.NF_CHAVE = this.RetornaStringDoCampo(_doc, "infProt", "chNFe");
                        _nf.NF_PATH_ARQUIVO = "..//Temp//" + _nf.NF_CHAVE;
                        _nf.NF_ARQUIVO = this.XmlDocToBytes(_doc);

                        this.Merge(_nf);

                    }
                }

                return _nf;

            }
            catch (Exception ex)
            {
                string _num_nf = _nf.NF_TIPO + " - " + _nf.NF_NUMERO + " - " + _nf.NF_SERIE;
                throw new Exception("NF => " + _num_nf + " --- " + SysException.Show(ex), ex);
            }

        }
        public NotaFiscalDTO GravarDadosXML(int _emp_id)
        {
            try
            {
                NotaFiscalDTO _nf = new NotaFiscalDTO();
                var _path = @"I:\nfe_gerada\XML APC 1\LEANDRO\Teste";
                var _doc = new XmlDocument();

                DirectoryInfo _dir = new DirectoryInfo(_path);
                FileInfo[] _files = _dir.GetFiles("*.xml", SearchOption.AllDirectories);


                foreach (FileInfo _file in _files)
                {
                    var _ser = new XmlSerializer(typeof(NotaFiscal));
                    _doc.Load(_file.FullName);

                    var nodes = _doc.DocumentElement.SelectSingleNode("*");

                    XmlNodeReader reader = new XmlNodeReader(nodes);

                    var _notafiscal = (NotaFiscal)_ser.Deserialize(reader);

                    var _nota = _notafiscal.lstInfNFe.FirstOrDefault();
                    _nf.NF_SERIE = _nota.Identificacao.Serie.ToString();
                    _nf.NF_NUMERO = System.Convert.ToInt32(this.RetornaStringDoCampo(_doc, "ide", "nNF"));

                    if ((this.RetornaStringDoCampo(_doc, "emit", "CNPJ") == "33832759000190") ||
                        (this.RetornaStringDoCampo(_doc, "emit", "CNPJ") == "27922913000111") ||
                        (this.RetornaStringDoCampo(_doc, "emit", "CNPJ") == "15165950000143") ||
                        (this.RetornaStringDoCampo(_doc, "emit", "CNPJ") == "33832759000351"))
                        _nf.NF_TIPO = (int)EnumTipoNF.Saida;
                    else
                        _nf.NF_TIPO = (int)EnumTipoNF.Entrada;

                     

                }

                return _nf;
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex), ex);
            }
        }
        public NotaFiscalDTO GravarDadosXML(XmlDocument _doc, int _emp_id)
        {
            NotaFiscalDTO _nf = new NotaFiscalDTO();

            try
            {
                string _dataemissao = "";
                string _datasaida = "";
                _nf.NF_ARQUIVO = this.XmlDocToBytes(_doc);

                if (this.RetornaStringDoCampo(_doc, "ide", "tpNF") == "1")
                {

                    // Transportador
                    if (this.RetornaStringDoCampo(_doc, "transporta", "CNPJ") != "")
                    {
                        _tra = new TransportadorDTO();
                        _tra.TRA_ID = 0;
                        _tra.TRA_CNPJ = this.RetornaStringDoCampo(_doc, "transporta", "CNPJ");
                        _tra.TRA_ENDERECO = this.RetornaStringDoCampo(_doc, "transporta", "xEnder");
                        _tra.TRA_INSCRICAO = this.RetornaStringDoCampo(_doc, "transporta", "IE");
                        _tra.TRA_NOME_FANTASIA = this.RetornaStringDoCampo(_doc, "transporta", "xNome");
                        _tra.TRA_RAZAO_SOCIAL = this.RetornaStringDoCampo(_doc, "transporta", "xNome");
                        _tra.TRA_TIPESSOA = (_tra.TRA_CNPJ.Length == 11) ? "F" : "J";

                        _mun = _munsrv.BuscarPorUFDescricao(this.RetornaStringDoCampo(_doc, "transporta", "xMun").Trim(), this.RetornaStringDoCampo(_doc, "transporta", "UF").Trim());

                        if (_mun != null)
                            _tra.MUN_ID = _mun.MUN_ID;

                    }

                    string _cnpj = this.RetornaStringDoCampo(_doc, "emit", "CNPJ");

                    _emp = _empsrv.BuscarPorCNPJ(_cnpj);

                    if (_emp != null)
                    {
                        // Clientes
                        _emp_id = _emp.EMP_ID;

                        string _cnpjcpf = this.RetornaStringDoCampo(_doc, "dest", "CNPJ");

                        if (_cnpjcpf == null || _cnpjcpf == "")
                            _cnpjcpf = this.RetornaStringDoCampo(_doc, "dest", "CPF");

                        _cli = _clisrv.BuscarPorCNPJ(_cnpjcpf);

                        if (_cli != null)
                            _nf.CLI_ID = _cli.CLI_ID;

                        if (_cli == null)
                        {
                            throw new Exception("Cliente não encontrado! Cnpj/Cpf => " + _cnpjcpf);
                        }
                    }
                    else
                    {
                        // Fornecedor
                        _frn = new FornecedorDTO();
                        _frn.FOR_ID = 0;
                        _frn.FOR_CNPJ = this.RetornaStringDoCampo(_doc, "emit", "CNPJ");
                        _frn.FOR_NOME_FANTASIA = this.RetornaStringDoCampo(_doc, "emit", "xFant");
                        _frn.FOR_RAZAO_SOCIAL = this.RetornaStringDoCampo(_doc, "emit", "xNome");
                        _frn.FOR_ENDERECO = this.RetornaStringDoCampo(_doc, "emit", "xLgr");
                        _frn.FOR_END_NUMERO = this.RetornaStringDoCampo(_doc, "emit", "nro");
                        _frn.FOR_BAIRRO = this.RetornaStringDoCampo(_doc, "emit", "xBairro");
                        _frn.FOR_INSCRICAO = this.RetornaStringDoCampo(_doc, "emit", "IE");
                        _frn.IBGE_COD_COMPLETO = this.RetornaStringDoCampo(_doc, "emit", "cMun");
                        _frn.FOR_END_COMPLEMENTO = this.RetornaStringDoCampo(_doc, "emit", "xCpl");
                        _frn.FOR_CEP = this.RetornaStringDoCampo(_doc, "emit", "CEP");
                        _frn.FOR_TEL = this.RetornaStringDoCampo(_doc, "emit", "fone");
                        _mun = _munsrv.BuscarPorIBGE(_frn.IBGE_COD_COMPLETO);

                        if (_mun != null)
                            _frn.MUN_ID = _mun.MUN_ID;

                        _frn.FOR_TIPESSOA = (_frn.FOR_CNPJ.Length == 11) ? "F" : "J";
                        _frn.TIPO_FOR_ID = 3;
                    }

                    // Nota Fiscal (Cabeçalho)
                    _nf.NF_SERIE = this.RetornaStringDoCampo(_doc, "ide", "serie");
                    _nf.NF_NUMERO = System.Convert.ToInt32(this.RetornaStringDoCampo(_doc, "ide", "nNF"));

                    if ((this.RetornaStringDoCampo(_doc, "emit", "CNPJ") == "33832759000190") ||
                        (this.RetornaStringDoCampo(_doc, "emit", "CNPJ") == "27922913000111") ||
                        (this.RetornaStringDoCampo(_doc, "emit", "CNPJ") == "15165950000143") ||
                        (this.RetornaStringDoCampo(_doc, "emit", "CNPJ") == "33832759000351"))
                        _nf.NF_TIPO = (int)EnumTipoNF.Saida;
                    else
                        _nf.NF_TIPO = (int)EnumTipoNF.Entrada;

                    _nf.CFOP = this.RetornaStringDoCampo(_doc, "prod", "CFOP");
                    _nf.CFOPENT = this.RetornaStringDoCampo(_doc, "prod", "CFOP");

                    _nf.NF_BASE_CALC_ICMS = System.Convert.ToDecimal(this.RetornaStringDoCampo(_doc, "ICMSTot", "vBC", "0"));
                    _nf.NF_BASE_CALC_ST = System.Convert.ToDecimal(this.RetornaStringDoCampo(_doc, "ICMSTot", "vBCST", "0"));
                    _nf.NF_CHAVE = this.RetornaStringDoCampo(_doc, "infProt", "chNFe");
                    _nf.NF_PATH_ARQUIVO = "\\Temp\\" + _nf.NF_CHAVE;

                    if (this.Buscar(_nf.NF_NUMERO, _nf.NF_SERIE, _nf.NF_TIPO, (int)_nf.CLI_ID) == null)
                    {

                        CultureInfo _culture = CultureInfo.GetCultureInfo("en-US");

                        _dataemissao = this.RetornaStringDoCampo(_doc, "ide", "dEmi");

                        if (_dataemissao == "")
                            _dataemissao = this.RetornaStringDoCampo(_doc, "ide", "dhEmi");

                        DateTime dt = DateTime.Parse(_dataemissao, _culture, System.Globalization.DateTimeStyles.AssumeLocal);

                        if (_dataemissao != "")
                            _nf.NF_DATA_EMISSAO = dt;

                        if (_nf.NF_TIPO == 0)
                        {
                            _nf.NF_DATA_ENTRADA = DateTime.Now;
                            _nf.NF_DATA_SAIDA = null;

                        }
                        else
                        {
                            _nf.NF_DATA_ENTRADA = null;

                            _datasaida = this.RetornaStringDoCampo(_doc, "ide", "dSaiEnt");

                            if (_datasaida == "")
                                _datasaida = this.RetornaStringDoCampo(_doc, "ide", "dhSaiEnt");

                            dt = DateTime.Parse(_datasaida, _culture, System.Globalization.DateTimeStyles.AssumeLocal);

                            if (_datasaida != "")
                                _nf.NF_DATA_SAIDA = dt;
                        }


                        _nf.NF_HORA_SAIDA = this.RetornaStringDoCampo(_doc, "ide", "hSaiEnt");
                        _nf.NF_FRETE_TIPO = System.Convert.ToInt32(this.RetornaStringDoCampo(_doc, "transp", "modFrete"));
                        _nf.NF_PROTOCOLO_AUT = this.RetornaStringDoCampo(_doc, "infProt", "nProt") + " " + this.RetornaStringDoCampo(_doc, "infProt", "dhRecbto");
                        _nf.NF_PESO_BRUTO = System.Convert.ToDecimal(this.RetornaStringDoCampo(_doc, "transp", "pesoB", "0"), _culture);
                        _nf.NF_PESO_LIQUIDO = System.Convert.ToDecimal(this.RetornaStringDoCampo(_doc, "transp", "pesoL", "0"), _culture);
                        _nf.NF_VLR_FRETE = System.Convert.ToDecimal(this.RetornaStringDoCampo(_doc, "prod", "vFrete", "0"), _culture);
                        _nf.NF_VLR_NOTA = System.Convert.ToDecimal(this.RetornaStringDoCampo(_doc, "ICMSTot", "vNF", "0"), _culture);
                        _nf.NF_VLR_OUTRAS = System.Convert.ToDecimal(this.RetornaStringDoCampo(_doc, "ICMSTot", "vOutro", "0"), _culture);
                        _nf.NF_VLR_PROD = System.Convert.ToDecimal(this.RetornaStringDoCampo(_doc, "ICMSTot", "vProd", "0"), _culture);
                        _nf.NF_VLR_SEGURO = System.Convert.ToDecimal(this.RetornaStringDoCampo(_doc, "ICMSTot", "vSeg", "0"), _culture);
                        _nf.NF_VLR_ICMS = System.Convert.ToDecimal(this.RetornaStringDoCampo(_doc, "ICMSTot", "vICMS", "0"), _culture);
                        _nf.NF_VLR_IPI = System.Convert.ToDecimal(this.RetornaStringDoCampo(_doc, "ICMSTot", "vIPI", "0"), _culture);
                        _nf.NF_VLR_ST = System.Convert.ToDecimal(this.RetornaStringDoCampo(_doc, "ICMSTot", "vST", "0"), _culture);
                        _nf.NF_INF_COMPLEMENTAR = this.RetornaStringDoCampo(_doc, "infAdic", "infAdFisco");


                        // Nota Fiscal (Itens) ------------
                        var _listanfi = new List<NotaFiscalItemDTO>();
                        var no = _doc.DocumentElement.GetElementsByTagName("det");

                        foreach (var prod in no)
                        {
                            // -----------
                            var _nfi = new NotaFiscalItemDTO();
                            _nfi.NF_NUMERO = _nf.NF_NUMERO;
                            _nfi.NF_SERIE = _nf.NF_SERIE;
                            _nfi.NF_TIPO = _nf.NF_TIPO;


                            _nfi.PRO_ID = ((XmlElement)prod).GetElementsByTagName("cProd").Count > 0 ? System.Convert.ToInt32(((XmlElement)prod).GetElementsByTagName("cProd")[0].InnerText) : 0;
                            _nfi.CFOP = ((XmlElement)prod).GetElementsByTagName("CFOP").Count > 0 ? ((XmlElement)prod).GetElementsByTagName("CFOP")[0].InnerText : "";
                            _nfi.CST_ID = this.RetornaStringDoCampo(_doc, "ICMS", "orig") + this.RetornaStringDoCampo(_doc, "ICMS", "CST");
                            _nfi.NFI_PRO_NOME = ((XmlElement)prod).GetElementsByTagName("xProd").Count > 0 ? ((XmlElement)prod).GetElementsByTagName("xProd")[0].InnerText : "";
                            _nfi.NCM_ID = ((XmlElement)prod).GetElementsByTagName("NCM").Count > 0 ? ((XmlElement)prod).GetElementsByTagName("NCM")[0].InnerText : "";
                            _nfi.NFI_ALIQ_ICMS = ((XmlElement)prod).GetElementsByTagName("pICMS").Count > 0 ? System.Convert.ToDecimal(((XmlElement)prod).GetElementsByTagName("pICMS")[0].InnerText, _culture) : 0;
                            _nfi.NFI_ALIQ_IPI = ((XmlElement)prod).GetElementsByTagName("pIPI").Count > 0 ? System.Convert.ToDecimal(((XmlElement)prod).GetElementsByTagName("pIPI")[0].InnerText, _culture) : 0;
                            _nfi.NFI_BASE_CALC_ICMS = ((XmlElement)prod).GetElementsByTagName("vBC").Count > 0 ? System.Convert.ToDecimal(((XmlElement)prod).GetElementsByTagName("vBC")[0].InnerText, _culture) : 0;
                            _nfi.NFI_QTDE = ((XmlElement)prod).GetElementsByTagName("qCom").Count > 0 ? System.Convert.ToDecimal(((XmlElement)prod).GetElementsByTagName("qCom")[0].InnerText, _culture) : 0;
                            _nfi.NFI_UN = ((XmlElement)prod).GetElementsByTagName("uCom").Count > 0 ? ((XmlElement)prod).GetElementsByTagName("uCom")[0].InnerText : "";
                            _nfi.NFI_VLR_ICMS = ((XmlElement)prod).GetElementsByTagName("vICMS").Count > 0 ? System.Convert.ToDecimal(((XmlElement)prod).GetElementsByTagName("vICMS")[0].InnerText, _culture) : 0;
                            _nfi.NFI_VLR_IPI = ((XmlElement)prod).GetElementsByTagName("vIPI").Count > 0 ? System.Convert.ToDecimal(((XmlElement)prod).GetElementsByTagName("vIPI")[0].InnerText, _culture) : 0;
                            _nfi.NFI_VLR_UNIT = ((XmlElement)prod).GetElementsByTagName("vUnCom").Count > 0 ? System.Convert.ToDecimal(((XmlElement)prod).GetElementsByTagName("vUnCom")[0].InnerText, _culture) : 0;
                            _nfi.NFI_VLR_TOTAL = ((XmlElement)prod).GetElementsByTagName("vProd").Count > 0 ? System.Convert.ToDecimal(((XmlElement)prod).GetElementsByTagName("vProd")[0].InnerText, _culture) : 0;

                            _nfi.NFI_UN = StringUtil.Truncate(_nfi.NFI_UN, 2);
                            _listanfi.Add(_nfi);

                        }

                        _nf.NF_STATUS = "PEN";

                        //------------


                        _nf.EMP_ID = _emp_id;
                        _nf.TDF_ID = "55";
                        _nf.NF_DATA_CADASTRO = DateTime.Now;

                        if (!String.IsNullOrWhiteSpace(_frn.FOR_RAZAO_SOCIAL))
                            _nf.FORNECEDOR = _frn;

                        if (!String.IsNullOrWhiteSpace(_tra.TRA_RAZAO_SOCIAL))
                            _nf.TRANSPORTADOR = _tra;

                        _nf.NOTA_FISCAL_ITEM = _listanfi;
                        //------------

                        _dao.IncluirNfModel(_nf);

                        if (_nf.NF_STATUS != "ATI")
                            return _nf;
                        else
                            return null;
                    }
                    else
                        return null;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                string _num_nf = _nf.NF_TIPO + " - " + _nf.NF_NUMERO + " - " + _nf.NF_SERIE;
                throw new Exception("NF => " + _num_nf + " --- " + SysException.Show(ex), ex);
            }

        }


        public NfeXmlDTO GerarNfeDTO(EmpresaModel empresa, ClienteDto cliente, ClienteEnderecoDto endereco, ProdutoComposicaoDTO produtoComposicao, 
            ProdutosDTO produto, ItemPedidoDTO itemPedido, DateTime? dataFaturamento, string path = null, ContratoDTO contrato = null, int? NFX_ID = null, TNFeInfNFeCobr cobranca = null)
        {
            Random rand = new Random();
            DateTime dataEntradaSaida = DateTime.Now;

            if (dataFaturamento == null)
            {
                string mensagem = "Não é possível gerar o xml da nota fiscal. Não é possível encontrar a data de faturamento no Item de Pedido {0}";
                mensagem = string.Format(mensagem, itemPedido.IPE_ID);
                throw new NFeException(mensagem);
            }

            var dataTemp = dataFaturamento.Value;
            dataFaturamento = new DateTime(dataTemp.Year, dataTemp.Month, dataTemp.Day, 12, 38, 34);

            TipoAmbienteEnum tipoAmbiente;
            string ufOrigem = null;
            string ufDestino = null;

            NfeXmlDTO retorno = null;
            if (NFX_ID != null)
            {
                retorno = _nfeXmlSRV.FindById(NFX_ID);
                if(retorno.NFX_DATA_EMI_NOTA != null)
                {
                    dataEntradaSaida = retorno.NFX_DATA_EMI_NOTA.Value;
                }
            }
            else
            {
                retorno = new NfeXmlDTO();
            }

            var numeroAleatorio = rand.Next(100, 99999999);
            var codigoNf = StringUtil.PreencherZeroEsquerda(numeroAleatorio, 8);
            var codigoPaisDestino = 1058;
            var codigoUFEmitente = (empresa.UF == "RJ" ? 33 : 31);
            var codigoUFDestino = 33;
            var codigoDoPedido = itemPedido.IPE_ID;
            int cfop = 0;
            int? sequencialEmpresa = null;

            if(NFX_ID != null && retorno != null)
            {
                if (retorno.NFX_NUMERO_NOTA == null ||
                    retorno.NFX_NUMERO_NOTA == 0)
                    throw new FaturamentoException("Não é possível regerar a nota. O número da nota não foi encontrado.");

                sequencialEmpresa = retorno.NFX_NUMERO_NOTA;
            }
            else
            {
                sequencialEmpresa = ServiceFactory.RetornarServico<CustomEmpresaSRV>().RetornarSequencialNFEEmpresa(empresa.EMP_ID);
            }
            
            int codigoMunicipioIBGE = 0;
            int.TryParse(empresa.IBGE_COD_COMPLETO, out codigoMunicipioIBGE);
            
            NotaFiscal nfe = new NotaFiscal();

            InfoNfeDTO infoNotaFiscal = new InfoNfeDTO()
            {
                Versao = "3.10"
            };


            nfe.lstInfNFe.Add(infoNotaFiscal);

            tipoAmbiente = (SysUtils.InHomologation()) ? TipoAmbienteEnum.Homologacao : TipoAmbienteEnum.Producao;
         
            var emitente = GerarDadosDoEmitente(empresa);
            var destino = GerarDadosDoDestinatario(cliente, endereco);

            if (emitente != null)
            {
                UFSRV _ufSRV = ServiceFactory.RetornarServico<UFSRV>();
                ufOrigem = emitente.EnderecoEmitente.UF;

                var objUF = _ufSRV.FindById(ufOrigem);
                if(objUF == null)
                    throw new FaturamentoException(string.Format("Não é possível gerar a nota fiscal. A empresa {0} não possui UF cadastrada.", empresa.EMP_NOME_FANTASIA));
                int.TryParse(objUF.UF_COD, out codigoUFEmitente);               
            }

            if (destino != null)
            {
                UFSRV _ufSRV = ServiceFactory.RetornarServico<UFSRV>();
                ufDestino = destino.Endereco.UF;

                var objUF = _ufSRV.FindById(ufDestino);
                if (objUF == null)
                    throw new FaturamentoException(string.Format("Não é possível gerar a nota fiscal. O endereço do cliente {0} não possui UF cadastrada.", cliente.CLI_NOME));
                int.TryParse(objUF.UF_COD, out codigoUFDestino);

                if (codigoUFDestino == 0)
                    throw new FaturamentoException(string.Format("Não é possível gerar a nota fiscal. O código da UF '{0}' do cliente '{1}' não está cadastrada.", destino.Endereco.UF, cliente.CLI_NOME));
            }

            infoNotaFiscal.Emitente = emitente;
            infoNotaFiscal.Destino = destino;

            infoNotaFiscal.Cobranca = cobranca;

            TipoLocalDestinoOperacaoEnum tipoLocalOperacao;

            if (codigoPaisDestino != 1058)
            {
                tipoLocalOperacao = TipoLocalDestinoOperacaoEnum.OperacaoComExterior;
            }
            else
                if (codigoUFEmitente == codigoUFDestino)
                {
                    tipoLocalOperacao = TipoLocalDestinoOperacaoEnum.OperacaoInterna;
                    cfop = 5101;
                }
                else
                {
                    tipoLocalOperacao = TipoLocalDestinoOperacaoEnum.OperacaoInterestadual;
                    cfop = 6101;
                }

            var ehPagamentoAvista = ServiceFactory.RetornarServico<ItemPedidoSRV>().EhVendaAVista(codigoDoPedido);
            IndPagEnum tipoPagamento = (ehPagamentoAvista) ? IndPagEnum.PagamentoAVista : IndPagEnum.PagamentoAPrazo;

            NfeIdentificacaoDTO nfeIdent = new NfeIdentificacaoDTO()
            {
                CodigoNumerico = codigoNf,
                CodigoDoMunicipio = codigoMunicipioIBGE,
                IndicadorLocalDestino = tipoLocalOperacao,
                CodigoUFEmitente = codigoUFEmitente,
                DataDeEmissao = (DateTime)dataFaturamento,
                DataEntradaSaida = (DateTime)dataEntradaSaida,  
                finNFe = FinalidadeNotaFiscalEnum.NfeNormal,
                //IndicacaoPagamento = tipoPagamento,
                IndicacaoPrensenca = IndicacaoPresencaEnum.OPERACAO_NAO_PRESENCIAL_OUTROS,
                NaturezaOperacao = "Venda de produção do estabelecimento",
                NumeroNotaFiscal = sequencialEmpresa,
                Serie = 1,
                TipoAmbiente = tipoAmbiente,
                TipoEmissao = TipoEmissaoEnum.Normal,
                FormatoImpressao = FormatoImpressaoEnum.Retrato,
                TipoOperacao = TipoNotaFiscalEnum.Saida,
                VersaoProcesso = "3.10.86"
            };
            
            infoNotaFiscal.Identificacao = nfeIdent;

            var det = GerarItensDaNota(produto, itemPedido, cfop);
            infoNotaFiscal.Detalhamento.Add(det);

            var total = GerarTotaisDaNotaFiscal(infoNotaFiscal.Detalhamento);
            infoNotaFiscal.Total = total;

            infoNotaFiscal.Transporte = new NfeInfoTransporteDTO()
            {
                ModalidadeFrete = TipoModalidadeTransporteEnum.SEM_CORRENCIA_FRETE
            };

            infoNotaFiscal.InformacoesAdicionais = new NfeInformacoesAdicionaisDTO()
            {
                infAdFisco = "Informações Adicionais de Interesse do Fisco: Produto Imune Dec.7212/10-RIPI;CF/88 Art. 150 Inc.VI Alínea d. - Não Incidência do ICMS Art.40, inc.I Lei 2657/96.PROCON-RJ Tel. 151 End.Rua da ajuda, 05 - S.Solo-Centro/RJ - Comissão de Defesa do Consumidor da ALERJ Tel.:0800 282706,Conforme Lei 5817/10.- Não incidência de PIS e COFINS Art.5º,parágrafo único,inc.I, alínea C da IN 1234."
            };
            
            _integrNFe.GerarCodigoDaNFe(nfe);
            string arquivoJaExistente = null;

            if (NFX_ID != null)
                arquivoJaExistente = _nfeXmlSRV.RetornarPath(NFX_ID);


            var result = ValidatorProxy.RecursiveValidate<NotaFiscal>(nfe);

            if (result.IsValid)
            {
                _integrNFe.defaultPath = path;
                var resultado = _integrNFe.SerializarNotaFiscal(nfe, TipoQualificacaoNFeEnum.PRODUTO, empID: empresa.EMP_ID);

                if (resultado != null)
                {
                    retorno.IPE_ID = codigoDoPedido;
                    retorno.NFX_NUMERO_NOTA = resultado.NumeroDaNotaFiscal;
                    retorno.NFX_CHAVE_NOTA = resultado.ChaveNotaFiscal;
                    retorno.NFX_PATH_NOTA = resultado.FileName;
                    retorno.NFX_TIPO = (int)resultado.TipoQualificacao;
                    retorno.NFX_DATA_EMI_NOTA = dataEntradaSaida;

                    return retorno;
                }
            }
            else {
                throw new ValidacaoException("Ocorre um problema ao gerar o xml da nota fiscal.", result);
            }

            return null;
        }

        public NfeEmitenteDTO GerarDadosDoEmitente(EmpresaModel empresa)
        {
            string codigoIBGE = empresa.IBGE_COD_COMPLETO;
            string municipio = null;
            string UF = null;
            string razaoSocial = null;

            if (string.IsNullOrWhiteSpace(empresa.EMP_IE))
                throw new FaturamentoException(string.Format("Não é possível gerar a nota fiscal. A empresa {0} não possui inscrição estadual cadastrada.", empresa.EMP_NOME_FANTASIA));

            int? codMunicipio = null;


            if (SysUtils.InHomologation())
            {
                razaoSocial = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
            }
            else
            {

                razaoSocial = empresa.EMP_RAZAO_SOCIAL;
            }

            if(!string.IsNullOrWhiteSpace(codigoIBGE))
            {
                codMunicipio = int.Parse(codigoIBGE);
                var municipioDTO = ServiceFactory.RetornarServico<MunicipioSRV>().BuscarPorIBGE(codigoIBGE);                

                if(municipioDTO != null)
                {
                    municipio = StringUtil.LimparAcentuacao(municipioDTO.MUN_DESCRICAO);
                    UF = municipioDTO.UF;
                }
            }
            
            NfeEmitenteDTO emitente = new NfeEmitenteDTO()
            {
                CNPJ = empresa.EMP_CNPJ,
                InscricaoEstadual = empresa.EMP_IE,
                xNome = razaoSocial,
                NomeFantasia = empresa.EMP_NOME_FANTASIA,
                EnderecoEmitente = new NfeEnderecoEmitenteDTO()
                {
                    CEP = empresa.EMP_CEP,
                    CodigoMunicipio = codMunicipio,
                    Telefone = empresa.EMP_TEL1,
                    Numero = StringUtil.PreencherZeroEsquerda(empresa.EMP_NUMERO, 3),
                    UF = UF,
                    Bairro = empresa.EMP_BAIRRO,
                    Complemento = empresa.EMP_COMPLEMENTO,
                    Logradouro = empresa.EMP_LOGRADOURO,
                    Municipio = municipio
                }
            };

            return emitente;

        }

        public NfeDestinoDTO GerarDadosDoDestinatario(ClienteDto cliente, ClienteEnderecoDto endereco)
        {
            string nome = null;
            string CNPJ = null;
            string CPF = null;
            string email = null;
            string telefone = null;
            string codUf = null;
            int? MUN_ID = null;
            int? COD_IBGE = null;
            string nomeMunicipio = null;
            var codigoPaisDestino = 1058;

            AssinaturaEmailSRV _emailSRV = ServiceFactory.RetornarServico<AssinaturaEmailSRV>();
            AssinaturaTelefoneSRV _telefoneSRV = ServiceFactory.RetornarServico<AssinaturaTelefoneSRV>();
            MunicipioSRV _municipioSRV = ServiceFactory.RetornarServico<MunicipioSRV>();

            if (cliente.TIPO_CLI_ID == null)
                throw new Exception("O tipo de cliente não está preenchido. Por favor, informe.");
            
            if(SysUtils.InHomologation())
            {
                nome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
            }
            else{

                nome = cliente.CLI_NOME;
            }

            _emailSRV.PreencherEmailAssinaturaNoCliente(cliente);
            _telefoneSRV.PreencherTelefoneAssinaturaNoCliente(cliente);
            
            var emailObj = ServiceFactory.RetornarServico<AssinaturaEmailSRV>().RetornarEmailDeContato(cliente.CLI_ID);
            var telefoneObj = ServiceFactory.RetornarServico<AssinaturaTelefoneSRV>().RetornarTelefoneContato(cliente.CLI_ID);

            if (emailObj != null)
            {
                email = emailObj.AEM_EMAIL;
            }
            
            if (telefoneObj != null)
            {
                telefone = telefoneObj.ATE_DDD + telefoneObj.ATE_TELEFONE;
            }

            string inscricaoEstadual = cliente.CLI_INSCRICAO;

            if (!string.IsNullOrWhiteSpace(inscricaoEstadual))
            {
                decimal inscricao = 0;
                if(!decimal.TryParse(inscricaoEstadual, out inscricao))
                {
                    inscricaoEstadual = null;
                }
            }

            codUf = endereco.END_UF;
            MUN_ID = endereco.MUN_ID;
            var objMunicipio = _municipioSRV.FindById(MUN_ID);

            if (objMunicipio != null)
            {
                if(!string.IsNullOrWhiteSpace(objMunicipio.IBGE_COD_COMPLETO))
                    COD_IBGE = int.Parse(objMunicipio.IBGE_COD_COMPLETO);
                nomeMunicipio = StringUtil.LimparAcentuacao(objMunicipio.MUN_DESCRICAO);
            }

            if (cliente.TIPO_CLI_ID != null)
            {
                
                if (cliente.TIPO_CLI_ID == 2)
                {
                    CPF = StringUtil.PreencherZeroEsquerda(cliente.CLI_CPF_CNPJ, 11);
                    CPF = StringUtil.Truncate(CPF, 11);
                }
                else
                {
                    CNPJ = StringUtil.PreencherZeroEsquerda(cliente.CLI_CPF_CNPJ, 14);
                    CNPJ = StringUtil.Truncate(CNPJ, 14);
                }

            }


            NfeDestinoDTO destino = new NfeDestinoDTO()
            {
                CPF = CPF,
                CNPJ = CNPJ,
                Email = email,
                xNome = nome,
                Endereco = new NfeEnderecoDestinatarioDTO()
                {
                    CEP = endereco.END_CEP,
                    CodigoPais = codigoPaisDestino,
                    CodigoMunicipio = COD_IBGE,
                    Telefone = telefone,
                    Numero = StringUtil.PreencherZeroEsquerda(endereco.END_NUMERO, 3),
                    UF = codUf,
                    Bairro = StringUtil.RetirarCaractereEspecialComTrim(endereco.END_BAIRRO),
                    Complemento = StringUtil.RetirarCaractereEspecialComTrim(endereco.END_COMPLEMENTO),
                    Logradouro = StringUtil.RetirarCaractereEspecialComTrim(endereco.END_LOGRADOURO),
                    Municipio = nomeMunicipio,
                    Pais = "Brasil",
                },
                TipoIEDestinatario = (inscricaoEstadual != null) ? 
                    TipoIEDestinatarioEnum.ContribuinteICMS : 
                    TipoIEDestinatarioEnum.ContribuinteIsentoDoICMS,
                IncricaoEstadual = inscricaoEstadual
            };

            return destino;
        }

        public NFeDetalhamentoItem GerarItensDaNota(ProdutosDTO produto, ItemPedidoDTO itemPedido, int? cfop, ContratoDTO contrato = null)
        {
            if (produto == null)
                throw new Exception("Não é possível gerar os itens da nota. O produto não foi encontrado");

            if(string.IsNullOrWhiteSpace(produto.PRO_NOME))
                throw new Exception("Não é possível gerar os itens da nota. O produto não possui nenhum nome.");


            string nomeDoProduto = "LIVRO TÉC " + produto.PRO_NOME.ToUpper();
            string codProduto = null;
            decimal quantidadeDecimal = 1.00m;
            int quantidade = (int) itemPedido.IPE_QTD;
            decimal valorUnitario = (decimal) itemPedido.IPE_PRECO_UNITARIO;

            decimal valorTotal = (contrato != null) ? (decimal) contrato.CTR_VLR_CONTRATO : (decimal)itemPedido.IPE_TOTAL;
            //int porcentagemDesconto = (itemPedido.IPE_DESCONTO != null) ? (int) itemPedido.IPE_DESCONTO : 0;
           // decimal valorTotalBruto = quantidade * valorUnitario;
           // decimal valorDesconto = ((porcentagemDesconto / 100) * valorTotalBruto);

            //if (itemPedido.IFF_ID != null)
            //{
            //    var infoFatura = new InfoFaturaSRV().FindById(itemPedido.IFF_ID);
            //    valorDesconto += (infoFatura.IFF_TOTAL_DESCONTADO != null) ? (decimal)infoFatura.IFF_TOTAL_DESCONTADO : 0;
            //}
            
            if(quantidade != null)
            {
                quantidadeDecimal = ((int)quantidade);
                quantidadeDecimal = decimal.Round(quantidadeDecimal, 2, MidpointRounding.AwayFromZero);

            }

            if (produto.PRO_ID != null)
            {
                codProduto = produto.PRO_ID.ToString();
            }

            
            NfeItemDTO item = new NfeItemDTO()
            {
                CodigoProduto = StringUtil.PreencherZeroEsquerda(codProduto, 3),
                NomeProduto = nomeDoProduto,
                CFOP = cfop,
                NCM = "49019900",
                Quantidade = quantidadeDecimal,
                Unidade = "UNID",
                ValorUnitario = valorUnitario,
                QuantidadeTributavel = quantidadeDecimal,
                UnidadeTributavel = "UNID",
                ValorUnitarioTributacao = valorUnitario,
                ValorTotal = valorTotal,
                IndicacaoTotal = 1
            };

            NFeImpostoICMSGrupo40 icms40 = new NFeImpostoICMSGrupo40();
            icms40.CST = 41;

            NFeImposto imposto = new NFeImposto();
            imposto.ICMS = new NFeImpostoICMS()
            {
                ICMS40 = icms40
            };

            imposto.PIS = new NFeImpostoPIS()
            {
                PISGrupoNaoTributado = new NFeImpostoPISGrupoNaoTributadoDTO()
            };

            imposto.COFINS = new NFeImpostoCOFINS()
            {
                COFINSGrupoNaoTributado = new NFeImpostoCOFINSGrupoNaoTributadoDTO()
            };

            imposto.IPI = new NfeImpostoIPI()
            {
                CodigoEnquadramento = "999",
                IPINaoTributado = new NfeImpostoIPIGrupoNaoTributado()
                {
                    CST = TipoTributacaoIPIEnum.SaidaNaoTributada
                }
            };

            NFeDetalhamentoItem det = new NFeDetalhamentoItem()
            {
                NumeroItem = 1,
                Produto = item,
                Imposto = imposto
            };

            return det;
        }


        public void EnviarNotaFiscal(NotaFiscalDTO notafiscal)
        {

            using (var scope = new TransactionScope())
            {
                var _nf_arquivo = this.FindById(notafiscal.NF_ID);

                if (_nf_arquivo == null)
                    throw new Exception("Arquivo xml não encontrado !!");

                notafiscal.NF_ARQUIVO = _nf_arquivo.NF_ARQUIVO;

                //var _linhas = this.BytesToXmlDoc(notafiscal.NF_ARQUIVO);

                string curDir = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());

                curDir = curDir + "\\temp\\" + notafiscal.NF_CHAVE +".xml";

                notafiscal.NF_PATH_ARQUIVO = curDir;

                System.IO.File.WriteAllBytes(curDir, notafiscal.NF_ARQUIVO);
                
                var repID = SessionContext.GetIdRepresentante();
                this.EnviarEmailNotaFiscal(notafiscal.NF_EMAIL, notafiscal.NF_ID, repID, null, notafiscal.NF_PATH_ARQUIVO);
               
                notafiscal.NF_STATUS = "ENV";

                this.Merge(notafiscal);

                var _histAtend = new HistoricoAtendimentoDTO();

                _histAtend.ASN_NUM_ASSINATURA = null;
                _histAtend.ACA_ID = 1;
                _histAtend.HAT_DATA_RESOLUCAO = DateTime.Now;
                _histAtend.TIP_ATEND_ID = 7;
                _histAtend.HAT_ORIGEM_ATEND = null;
                _histAtend.CLI_ID = notafiscal.CLI_ID;
                _histAtend.HAT_DATA_HIST = DateTime.Now;
                _histAtend.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                _histAtend.HAT_SOLICITANTE = SessionContext.autenticado.USU_LOGIN;
                _histAtend.HAT_DESCRICAO = "Nota fiscal enviada para ao cliente pelo email (" + notafiscal.NF_EMAIL +" )";
                _histAtend.UEN_ID = 3;

                new HistAtendSRV().Save(_histAtend);


                scope.Complete();
            }
        }
        public string GerarDanfe(int _nf_id)
        {
            string _retorno = "";

            using (var scope = new TransactionScope())
            {
                var _nf_arquivo = this.FindById(_nf_id);

                if (_nf_arquivo == null)
                    throw new Exception("Arquivo xml não encontrado !!");
                
                string curDir = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());

                curDir = curDir + "\\temp\\" + _nf_arquivo.NF_CHAVE + ".xml";

                _nf_arquivo.NF_PATH_ARQUIVO = curDir;

                System.IO.File.WriteAllBytes(curDir, _nf_arquivo.NF_ARQUIVO);

                var repID = SessionContext.GetIdRepresentante();

                _retorno =  new DanfeSRV().GerarPdf(_nf_arquivo.NF_CHAVE);

                var _histAtend = new HistoricoAtendimentoDTO();

                _histAtend.ASN_NUM_ASSINATURA = null;
                _histAtend.ACA_ID = 1;
                _histAtend.HAT_DATA_RESOLUCAO = DateTime.Now;
                _histAtend.TIP_ATEND_ID = 7;
                _histAtend.HAT_ORIGEM_ATEND = null;
                _histAtend.CLI_ID = _nf_arquivo.CLI_ID;
                _histAtend.HAT_DATA_HIST = DateTime.Now;
                _histAtend.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                _histAtend.HAT_SOLICITANTE = SessionContext.autenticado.USU_LOGIN;
                _histAtend.HAT_DESCRICAO = "Danfe gerarda para o cliente (" + _nf_arquivo.CLI_ID + " )";
                _histAtend.UEN_ID = 3;

                new HistAtendSRV().Save(_histAtend);


                scope.Complete();
            }

            return _retorno;
        }


        public NFeTotalDTO GerarTotaisDaNotaFiscal(IEnumerable<NFeDetalhamentoItem> detalhamentos)
        {

            decimal desconto = 0.00m;
            decimal totalDosProdutos = 0.00m;
            decimal totalDaNota = 0.00m;

            foreach(var det in detalhamentos)
            {
                //desconto += det.prod.desconto;
                totalDosProdutos += det.Produto.ValorTotal;
            }

            totalDaNota = totalDosProdutos; //- desconto;

            NFeTotalDTO total = new NFeTotalDTO()
            {
                ICMSTotal = new NFeICMSTotalDTO()
                {
                    BaseCalculoICMS = 0.00m,
                    ValorICMS = 0.00m,
                    ICMSDesoneracao = 0.00m,
                    BaseCalculoST = 0.00m,
                    ValorST = 0.00m,
                    TotalProduto = totalDosProdutos,
                    ValorFrete = 0.00m,
                    TotalSeguro = 0.00m,
                    TotalDesconto = desconto,
                    ValorII = 0.00m,
                    ValorIPI = 0.00m,
                    ValorPIS = 0.00m,
                    ValorCOFINS = 0.00m,
                    ValorOutrasDespesas = 0.00m,
                    TotalNotaFiscal = totalDaNota,
                    
                }
            };

            return total;

        }

        public DateTime RetornarSegundaMaisProxima(DateTime? data)
        {
            if(data != null)
            {
                int diaPretendido = 1;
                int diaSemana = (int) data.Value.DayOfWeek;

                int diaFinal = (diaSemana - diaPretendido) * -1;
                var dataFinal = data.Value.AddDays(diaFinal);

                return dataFinal;
            }

            return DateTime.Now;
        }

        public void SalvarNotaDeServico(HttpPostedFileBase file, string serverPath, int? ipeId, string chaveNota)
        {
            if (file != null)
            {
                var path = SysUtils.RetornarPathNFeXML();
                var fluentFile = new FileFluent(file);
                var savedPath =
                    fluentFile.CheckExtensions("xml").
                    SetLocations(serverPath, path).
                    CheckValidations().TrySave();

                var fileName = fluentFile.fileName;

                _nfeXmlSRV.Incluir(ipeId, chaveNota, fileName, 1);

            }
        }


        /// <summary>
        /// Valida as informações de data de faturamento da empresa
        /// para impedir a geração de uma nota de uma data retroativa a última data gerada para essa empresa
        /// e impedir gerar uma nota de data de faturamento superior a contratos que não tiveram suas notas geradas ainda.
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="dataFaturamento"></param>
        public void ValidarDataFaturamento(int? empId, DateTime? dataFaturamento, string contrato, int? ipeId)
        {
            try
            {

                if (empId == null)
                    throw new ArgumentNullException("O argumento empId é nulo.");
                if(dataFaturamento == null)
                    throw new ArgumentNullException("O argumento dataFaturamento é nulo.");

                var ultimoFaturamento = ServiceFactory.RetornarServico<ContratoSRV>().RetornarDataDoUltimoFaturamentoPorEmpresa(empId);

                if (ultimoFaturamento != null && dataFaturamento < ultimoFaturamento)
                {
                    string msg =
                        "Não é possível gerar o número da nota. " +
                        "A data de faturanento do contrato {0} é {1:dd/MM/yyyy}.  " +
                        "A última data de faturamento da empresa {2} é {3:dd/MM/yyyy}." +
                        "Não é mais possível gerar nota de contratos faturados antes do dia {4:dd/MM/yyyy}";
                    throw new NFeException(string.Format(msg, contrato, dataFaturamento, empId, ultimoFaturamento, ultimoFaturamento));
                }

                var qtdContratosRetroativos = ServiceFactory
                    .RetornarServico<ItemPedidoSRV>()
                    .RetornarQtdPedidosNotaNaoGeradaPorData(dataFaturamento, empId, ipeId);

                if(qtdContratosRetroativos > 0 && dataFaturamento > ultimoFaturamento)
                {
                    string msg =
                        "Não é possível gerar o número da nota. " +
                        "Existem {0} pedido(s) que ainda não possui(em) nota(s) gerada(s) com data de faturamento " +
                        "anterior a {1:dd/MM/yyyy}. (Data de faturamento deste contrato {2}) " +
                        "Gere primeiro as notas destes {3} pedidos antes de gerar essa nota.";
                    throw new NFeException(string.Format(msg, qtdContratosRetroativos, dataFaturamento, contrato, qtdContratosRetroativos));
                }
            }
            catch(NFeException e)
            {
                throw e;
            }
            catch(Exception e)
            {
                throw new NFeException("Não é possível validar a data de faturamento ao gerar a nota.", e);
            }
        }

        public DetalhesDaNotaFiscalDTO RetornarDetalhesDaNotaFiscal(int? nfID)
        {
            var detalhe = _dao.RetornarDetalhesDaNotaFiscal(nfID);

            if(detalhe != null && detalhe.EmpID != null)
            {
                var empresa = ServiceFactory.RetornarServico<EmpresaSRV>().FindById(detalhe.EmpID);
                if(empresa != null)
                {
                    detalhe.EmpresaNome = empresa.EMP_RAZAO_SOCIAL;
                    detalhe.EmpresaCNPJ = empresa.EMP_CNPJ;
                    detalhe.EmissorInscricaoMunicipal = empresa.EMP_IM;
                }

                var link = GerarLinkDanfe(nfID);
                if(link != null)
                {
                    detalhe.LinkDetalhamento = link.Link;
                }
            }
            return detalhe;
        }

        private string ProcessarPathParaAnexo(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                path = path.Replace("file:///", "");
                path = path.Replace("FILE:///", "");

                path = path.Replace("I:/", "\\\\rj-app-srv\\share\\Relatorios\\");
                path = path.Replace("i:/", "\\\\rj-app-srv\\share\\Relatorios\\");
                path = path.Replace(@"/", @"\");

                if (!File.Exists(path))
                    throw new Exception(string.Format("Não é possível achar o arquivo de nota no caminho '{0}'", path));

                var downloadDTO = new FileInfoDTO(path);
                string newPath = SysUtils.FormatarPathComNomeAmbiente(@"\\rj-app-srv\share\DEV\anexos-email\{{ambiente}}\{0}");
                newPath = string.Format(newPath, downloadDTO.FileName);

                FileUtil.CriarDiretorioPermisaoServicoWin(newPath);
                if (FileUtil.IsDirectoryWritable(newPath))
                {
                    File.Copy(path, newPath, true);
                }
                return newPath;
            }
            return null;

        }

        public string ProcessarPathParaAnexo(HttpPostedFileBase arquivo)
        {
            if(arquivo != null)
            {
                string newPath = SysUtils.FormatarPathComNomeAmbiente(@"\\rj-app-srv\share\DEV\anexos-email\{{ambiente}}\{0}");
                newPath = string.Format(newPath, arquivo.FileName);

                FileUtil.CriarDiretorioPermisaoServicoWin(newPath);
                if (FileUtil.IsDirectoryWritable(newPath))
                {
                    arquivo.SaveAs(newPath);
                }
                return newPath;
            }
            return null;
        }
        public string RetornaStringDoCampo(XmlDocument _doc, string cNo, string cCampo, string cDefault = null)
        {
            // variável de retorno...
            string cRetorno = "";

            // foram informados os dois parâmetros?...
            if ((cNo != "") && (cCampo != ""))
            {
                // preparando a leitura do Nó no XML...
                XmlNodeList no = _doc.DocumentElement.GetElementsByTagName(cNo);

                // retornando com a informação do campo...
                if (no.Count > 0)
                {
                    if (((XmlElement)no[0]).GetElementsByTagName(cCampo).Count > 0)
                    {
                        cRetorno = ((XmlElement)no[0]).GetElementsByTagName(cCampo)[0].InnerText;
                    }
                }
            }

            if (cRetorno == "" && (cDefault != "" && cDefault != null))
            {
                cRetorno = cDefault;
            }

            return cRetorno;
        }

        public void EnviarEmailNotaFiscal(string email, 
            int? nfID, 
            int? repIDEnvio = null,
            HttpPostedFileBase arquivo = null,
            string path = null)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(email))
                    throw new ArgumentNullException("Informe o E-Mail.");
                if(nfID == null)
                    throw new ArgumentNullException("Informe o Código da Nota Fiscal.");

                var detalhesProposta = RetornarDetalhesDaNotaFiscal(nfID);
                if(detalhesProposta != null)
                {
                    string pathArquivo = null;
                    if (arquivo != null)
                    {
                        pathArquivo = ProcessarPathParaAnexo(arquivo);
                        if (string.IsNullOrWhiteSpace(pathArquivo))
                            throw new Exception("O caminho do arquivo de nota não existe.");
                    }
                    else
                    {
                        pathArquivo = (!string.IsNullOrWhiteSpace(path)) ? path : detalhesProposta.PathArquivoNota;
                        if (string.IsNullOrWhiteSpace(pathArquivo))
                            throw new Exception("O caminho do arquivo de nota não existe.");

                        pathArquivo = ProcessarPathParaAnexo(pathArquivo);
                    }

                    string assunto = "[COAD] - Detalhes da Sua Nota Fiscal";
                    var template = _templateHTMLSRV.RetornarTemplatePorFuncionalidade(6);
                    var corpoEmail = _templateHTMLSRV.ProcessarTemplate(template, detalhesProposta);
                    
                    email = SysUtils.DecidirEnderecoDeEmail(email);

                    var emailSRV = ServiceFactory.RetornarServico<IEmailSRV>();


                    emailSRV.EnviarEmail(new EmailRequestDTO()
                    {
                        Assunto = assunto,
                        CorpoEmail = corpoEmail,
                        EmailDestino = email,
                        codSMTP = 3,
                        codRepresentante = repIDEnvio,
                        pathAnexo = pathArquivo
                    });

                }
                else
                {
                    throw new Exception(string.Format("Não é possível achar a nota de código '{0}'", nfID));
                }
            }
            catch(Exception e)
            {
                throw new Exception("Não é possível enviar o E-Mail com a Nota Fiscal", e);
            }
        }

        public void EnviarEmailNotaFiscalDeServico(string email,
            int? nfID,
            int? repIDEnvio = null,
            HttpPostedFileBase arquivo = null,
            string path = null)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(email))
                    throw new ArgumentNullException("Informe o E-Mail.");
                if (nfID == null)
                    throw new ArgumentNullException("Informe o Código da Nota Fiscal.");

                var detalhesProposta = RetornarDetalhesDaNotaFiscal(nfID);
                if (detalhesProposta != null)
                {
                    string pathArquivo = null;
                    if (arquivo != null)
                    {
                        pathArquivo = ProcessarPathParaAnexo(arquivo);
                        if (string.IsNullOrWhiteSpace(pathArquivo))
                            throw new Exception("O caminho do arquivo de nota não existe.");
                    }
                    else
                    {
                        pathArquivo = (!string.IsNullOrWhiteSpace(path)) ? path : detalhesProposta.PathArquivoNota;
                        if (string.IsNullOrWhiteSpace(pathArquivo))
                            throw new Exception("O caminho do arquivo de nota não existe.");

                        pathArquivo = ProcessarPathParaAnexo(pathArquivo);
                    }

                    string assunto = "[COAD] - Detalhes da Sua Nota Fiscal de Serviço";
                    var template = _templateHTMLSRV.RetornarTemplatePorFuncionalidade(12);
                    var corpoEmail = _templateHTMLSRV.ProcessarTemplate(template, detalhesProposta);

                    email = SysUtils.DecidirEnderecoDeEmail(email);

                    var emailSRV = ServiceFactory.RetornarServico<IEmailSRV>();


                    emailSRV.EnviarEmail(new EmailRequestDTO()
                    {
                        Assunto = assunto,
                        CorpoEmail = corpoEmail,
                        EmailDestino = email,
                        codSMTP = 3,
                        codRepresentante = repIDEnvio,
                        pathAnexo = pathArquivo
                    });

                }
                else
                {
                    throw new Exception(string.Format("Não é possível achar a nota de código '{0}'", nfID));
                }
            }
            catch (Exception e)
            {
                throw new Exception("Não é possível enviar o E-Mail com a Nota Fiscal", e);
            }
        }

        public void EnviarEmailNotaFiscal(string email,
            NFeDistribuicaoDTO nfe,
            int? repIDEnvio = null,
            HttpPostedFileBase arquivo = null,
            string path = null)
        {
            try
            {

                if(nfe.lstInfNFe != null && nfe.lstInfNFe.Count > 0)
                {
                    var inf = nfe.lstInfNFe[0];

                    if (string.IsNullOrWhiteSpace(email))
                        throw new ArgumentNullException("Informe o E-Mail.");

                    var detalhesProposta = new DetalhesDaNotaFiscalDTO()
                    {
                        ClienteNome = inf.Destino.xNome,
                        EmpresaCNPJ = inf.Emitente.CNPJ,
                        EmpresaNome = inf.Emitente.xNome,
                        NotaDataEmissao = inf.Identificacao.DataDeEmissao,
                        NotaFiscalNumero = inf.Identificacao.NumeroNotaFiscal,
                        NotaValor = inf.Total.ICMSTotal.TotalNotaFiscal,
                        ProdutoNome = inf.Detalhamento[0].Produto.NomeProduto,
                        PathArquivoNota = path,                    
                    };


                    if (detalhesProposta != null)
                    {
                        string pathArquivo = null;
                        if (arquivo != null)
                        {
                            pathArquivo = ProcessarPathParaAnexo(arquivo);
                            if (string.IsNullOrWhiteSpace(pathArquivo))
                                throw new Exception("O caminho do arquivo de nota não existe.");
    }
                        else
                        {
                            pathArquivo = (!string.IsNullOrWhiteSpace(path)) ? path : detalhesProposta.PathArquivoNota;
                            if (string.IsNullOrWhiteSpace(pathArquivo))
                                throw new Exception("O caminho do arquivo de nota não existe.");

                            pathArquivo = ProcessarPathParaAnexo(pathArquivo);
}

                        string assunto = "[COAD] - Detalhes da Sua Nota Fiscal";
                        var template = _templateHTMLSRV.RetornarTemplatePorFuncionalidade(6);
                        var corpoEmail = _templateHTMLSRV.ProcessarTemplate(template, detalhesProposta);

                        email = SysUtils.DecidirEnderecoDeEmail(email);

                        var emailSRV = ServiceFactory.RetornarServico<IEmailSRV>();


                        emailSRV.EnviarEmail(new EmailRequestDTO()
                        {
                            Assunto = assunto,
                            CorpoEmail = corpoEmail,
                            EmailDestino = email,
                            codSMTP = 3,
                            codRepresentante = repIDEnvio,
                            pathAnexo = pathArquivo
                        });
                    }

                }
                else
                {
                    throw new Exception("Não é possível achar a nota de código");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Não é possível enviar o E-Mail com a Nota Fiscal", e);
            }
        }
        public INFeLote AdicionarPedidoLoteVigente(int? CodPedido, int? EmpresaID, string CodContrato = null)
        {
           return _integrNFe.AdicionarPedidoLoteVigente(CodPedido, EmpresaID, CodContrato);
        }

        public void EnviarLoteVigente(string defaultPath, int? numeroDeTentativas = 5)
        {
            if (!string.IsNullOrWhiteSpace(defaultPath))
            {
                _integrNFe.defaultPath = defaultPath;
            }
            _integrNFe.EnviarLoteVigente(numeroDeTentativas);
        }

        public void ProcessarRetornoLoteEnviado(string defaultPath)
        {
            if (!string.IsNullOrWhiteSpace(defaultPath))
            {
                _integrNFe.defaultPath = defaultPath;
            }
            _integrNFe.ProcessarRetornoLoteEnviado();
        }

        public INFeLote CriarNovoLote(RequisicaoNovoLote requisicaoCriacao)
        {
            return _integrNFe.CriarNovoLote(requisicaoCriacao);
        }

        public NotaFiscalDTO CadastrarNotaFiscal(NFeProcessada procNFe, INFeLoteItem loteItem, string path, byte[] conteudoNFe)
        {
            try
            {
                if (procNFe != null && 
                    procNFe.NFe != null && 
                    procNFe.NFe[0] != null && 
                    loteItem != null && 
                    procNFe.NFe[0].lstInfNFe != null &&
                    procNFe.NFe[0].lstInfNFe.Count > 0)
                {
                    int? cliID = loteItem.ClienteID;
                    int? empID = loteItem.EmpresaID;

                    string codContrato = loteItem.CodContrato;

                    ContratoDTO contrato = null;

                    if (cliID == null)
                    {
                        if(loteItem.CodPedido != null)
                        {
                            var itemPedido = ServiceFactory.RetornarServico<ItemPedidoSRV>().FindById(loteItem.CodPedido);

                            if (itemPedido == null)
                            {
                                throw new GeracaoNotaException(string.Format("O item de pedido {0} não foi encontrado", loteItem.CodPedido));
                            }
                            empID = itemPedido.PEDIDO_CRM.EMP_ID;
                            cliID = itemPedido.PEDIDO_CRM.CLI_ID;



                            contrato = ServiceFactory.RetornarServico<ContratoSRV>().FindById(loteItem.CodContrato);
                            if (contrato == null)
                            {
                                throw new GeracaoNotaException(string.Format("O contrato {0} não foi encontrado", loteItem.CodContrato));
                            }
                        }
                        else if(loteItem.CodProposta != null)
                        {
                            var propostaItem = ServiceFactory.RetornarServico<PropostaItemSRV>().FindById(loteItem.CodProposta);

                            if (propostaItem == null)
                            {
                                throw new GeracaoNotaException(string.Format("O item de proposta {0} não foi encontrada.", loteItem.CodProposta));
                            }
                            empID = propostaItem.PROPOSTA.EMP_ID;
                            cliID = propostaItem.PROPOSTA.CLI_ID;
                        }

                    }

                    if(empID == null)
                    {
                        throw new GeracaoNotaException("A empresa de faturamento não foi encontrada");
                    }

                    string protocolo = null;
                    string protocoloConcat =  null;

                    if (!string.IsNullOrWhiteSpace(procNFe.protNFe.infProt.nProt))
                        protocolo = procNFe.protNFe.infProt.nProt;

                    if (procNFe.protNFe.infProt.dhRecbto != null)
                        protocoloConcat += string.Format("{0} {1:yyyy-MM-ddTHH:mm:sszzz}", protocolo, procNFe.protNFe.infProt.dhRecbto);

                    var infNFe = procNFe.NFe[0].lstInfNFe[0];

                    if(infNFe.Identificacao.TipoOperacao == TipoNotaFiscalEnum.Saida && cliID == null)
                    {
                        throw new GeracaoNotaException("O cliente não foi encontrado");
                    }

                    var nota = new NotaFiscalDTO()
                    {
                        NF_TIPO = (int) infNFe.Identificacao.TipoOperacao,
                        NF_NUMERO = (infNFe.Identificacao.NumeroNotaFiscal != null) ? infNFe.Identificacao.NumeroNotaFiscal.Value : 0,
                        NF_SERIE = (infNFe.Identificacao.Serie != null) ? infNFe.Identificacao.Serie.ToString() : null,
                        NF_DATA_EMISSAO = infNFe.Identificacao.DataDeEmissao,
                        NF_DATA_SAIDA = (infNFe.Identificacao.TipoOperacao == TipoNotaFiscalEnum.Saida) ? (DateTime?) infNFe.Identificacao.DataEntradaSaida : null,
                        NF_DATA_ENTRADA = (infNFe.Identificacao.TipoOperacao == TipoNotaFiscalEnum.Entrada) ? (DateTime?)infNFe.Identificacao.DataEntradaSaida : null,
                        NF_BASE_CALC_ICMS = infNFe.Total.ICMSTotal.BaseCalculoICMS,
                        NF_VLR_ICMS = infNFe.Total.ICMSTotal.ValorICMS,
                        NF_BASE_CALC_ST = infNFe.Total.ICMSTotal.BaseCalculoST,
                        NF_VLR_ST = infNFe.Total.ICMSTotal.ValorST,
                        NF_VLR_FRETE = infNFe.Total.ICMSTotal.ValorFrete,
                        NF_VLR_SEGURO = infNFe.Total.ICMSTotal.TotalSeguro,
                        NF_VLR_OUTRAS = infNFe.Total.ICMSTotal.ValorOutrasDespesas,
                        NF_VLR_IPI = infNFe.Total.ICMSTotal.ValorIPI,
                        NF_VLR_PROD = infNFe.Total.ICMSTotal.TotalProduto,
                        NF_VLR_NOTA = infNFe.Total.ICMSTotal.TotalNotaFiscal,
                        NF_VLR_FCP = infNFe.Total.ICMSTotal.ValorTotalFCP,
                        NF_VLR_FCP_ST = infNFe.Total.ICMSTotal.ValorTotalFCPST,
                        NF_VLR_FCP_ST_RETIDO = infNFe.Total.ICMSTotal.ValorTotalFCPSTRetido,
                        NF_VLR_IPE_DEVOLVIDO = infNFe.Total.ICMSTotal.ValorIPIDevolvido,
                        NF_VLR_DESCONTO = infNFe.Total.ICMSTotal.TotalDesconto,
                        NF_CHAVE = (!string.IsNullOrWhiteSpace(infNFe.Id)) ? infNFe.Id.Replace("NFe", "") : null,
                        NF_FRETE_TIPO = (int)infNFe.Transporte.ModalidadeFrete,
                        NF_PESO_BRUTO = 0,
                        NF_PESO_LIQUIDO = 0,
                        EMP_ID = empID,
                        NF_NUMERO_PROTOCOLO = protocolo,
                        NF_PROTOCOLO_AUT = protocoloConcat,
                        USU_LOGIN = "COADSYS",
                        CLI_ID = cliID,
                        NF_EMAIL = infNFe.Destino.Email,
                        NF_INF_COMPLEMENTAR = $"{infNFe.InformacoesAdicionais.infAdFisco} \n {infNFe.InformacoesAdicionais.InformacoesComplementares}",
                        NF_DATA_CADASTRO = DateTime.Now,
                        TDF_ID = "55",
                        NF_PATH_ARQUIVO = path,
                        NF_ARQUIVO = conteudoNFe,
                        CTR_NUM_CONTRATO = (contrato != null) ? contrato.CTR_NUM_CONTRATO : null,
                        IPE_ID = loteItem.CodPedido,
                        PPI_ID = loteItem.CodProposta,
                        NF_NOTA_ANTECIPADA = loteItem.NotaAntecipada,

                    };

                    nota.AdicionarStatus(1);

                    var notaSalva = Save(nota);
                    nota.NF_ID = notaSalva.NF_ID;

                    var notaFiscalItemSRV = ServiceFactory.RetornarServico<NotaFiscalItemSRV>()
                        .SalvarItens(nota, infNFe);

                    return nota;
                }
                return null;
            }
            catch(Exception e)
            {
                throw new GeracaoNotaException("Não é possível Salvar a Nota Fiscal na base de dados.", e);
            }
        }
        
        public NotaFiscalDTO CadastrarNotaFiscalServico(CompNfse compNfse, INFeLoteItem loteItem, string path, byte[] conteudoNFe)
        {
            try
            {
                if (compNfse != null)
                {
                    int? cliID = loteItem.ClienteID;
                    int? empID = loteItem.EmpresaID;

                    string codContrato = loteItem.CodContrato;

                    ContratoDTO contrato = null;

                    if (cliID == null)
                    {
                        if (loteItem.CodPedido != null)
                        {
                            var itemPedido = ServiceFactory.RetornarServico<ItemPedidoSRV>().FindById(loteItem.CodPedido);

                            if (itemPedido == null)
                            {
                                throw new GeracaoNotaException(string.Format("O item de pedido {0} não foi encontrado", loteItem.CodPedido));
                            }
                            empID = itemPedido.PEDIDO_CRM.EMP_ID;
                            cliID = itemPedido.PEDIDO_CRM.CLI_ID;

                            contrato = ServiceFactory.RetornarServico<ContratoSRV>().FindById(loteItem.CodContrato);
                            if (contrato == null)
                            {
                                throw new GeracaoNotaException(string.Format("O contrato {0} não foi encontrado", loteItem.CodContrato));
                            }

                            contrato.ASSINATURA = ServiceFactory.RetornarServico<AssinaturaSRV>().FindById(contrato.ASN_NUM_ASSINATURA);
                        }
                        else if (loteItem.CodProposta != null)
                        {
                            var propostaItem = ServiceFactory.RetornarServico<PropostaItemSRV>().FindById(loteItem.CodProposta);

                            if (propostaItem == null)
                            {
                                throw new GeracaoNotaException(string.Format("O item de proposta {0} não foi encontrada.", loteItem.CodProposta));
                            }
                            empID = propostaItem.PROPOSTA.EMP_ID;
                            cliID = propostaItem.PROPOSTA.CLI_ID;
                        }

                    }

                    if (empID == null)
                    {
                        throw new GeracaoNotaException("A empresa de faturamento não foi encontrada");
                    }
                    

                    var infNFe = compNfse.Nfse.InfNfse;
                    var nota = new NotaFiscalDTO()
                    {
                        NF_TIPO = 3,
                        NF_NUMERO = (infNFe.Numero != null) ? infNFe.Numero.Value : 0,
                        NF_SERIE = infNFe.IdentificacaoRps.Serie,
                        NF_DATA_EMISSAO = infNFe.DataEmissaoDateTime,
                        NF_VLR_PROD = infNFe.Servico.Valores.ValorServicos,
                        NF_VLR_NOTA = infNFe.Servico.Valores.ValorLiquidoNfse,
                        NF_VLR_DESCONTO = infNFe.Servico.Valores.DescontoCondicionado + infNFe.Servico.Valores.DescontoCondicionado,
                        EMP_ID = empID,
                        USU_LOGIN = "COADSYS",
                        CLI_ID = cliID,
                        NF_EMAIL = (infNFe.TomadorServico.Contato != null) ? infNFe.TomadorServico.Contato.Email : null,
                        NF_DATA_CADASTRO = DateTime.Now,
                        TDF_ID = "55",
                        NF_PATH_ARQUIVO = path,
                        NF_ARQUIVO = conteudoNFe,
                        CTR_NUM_CONTRATO = (contrato != null) ? contrato.CTR_NUM_CONTRATO : null,
                        IPE_ID = loteItem.CodPedido,
                        PPI_ID = loteItem.CodProposta,
                        NF_NOTA_ANTECIPADA = loteItem.NotaAntecipada,
                        NF_VLR_PIS = infNFe.Servico.Valores.ValorPis,
                        NF_VLR_COFINS = infNFe.Servico.Valores.ValorCofins,
                        NF_VLR_INSS = infNFe.Servico.Valores.ValorInss,
                        NF_VLR_IR = infNFe.Servico.Valores.ValorIr,
                        NF_VLR_CSLL = infNFe.Servico.Valores.ValorCsll,
                        NF_VLR_ISS = infNFe.Servico.Valores.ValorIss,
                        NF_VLR_ISS_RETIDO = infNFe.Servico.Valores.ValorIssRetido,
                        NF_ALIQUODA = infNFe.Servico.Valores.Aliquota,
                        NF_VLR_SERVICO = infNFe.Servico.Valores.ValorServicos,
                        NF_COD_VERIFICACAO = (!string.IsNullOrWhiteSpace(infNFe.CodigoVerificacao)) ? infNFe.CodigoVerificacao.Replace("-", "") : null
                    };

                    nota.AdicionarStatus(1);

                    var notaSalva = Save(nota);
                    nota.NF_ID = notaSalva.NF_ID;

                    var proId = (contrato != null) ? contrato.ASSINATURA.PRO_ID : 0;
                    var notaFiscalItemSRV = ServiceFactory.RetornarServico<NotaFiscalItemSRV>()
                        .SalvarItensNfse(nota, compNfse.Nfse.InfNfse, (int)proId);

                    return nota;
                }
                return null;
            }
            catch (Exception e)
            {
                throw new GeracaoNotaException("Não é possível Salvar a Nota Fiscal na base de dados.", e);
            }
        }

        private void AnularPropriedades(ICollection<NotaFiscalDTO> notas)
        {
            if (notas != null && notas.Count > 0)
            {
                foreach (var nota in notas)
                {
                    nota.CONTRATOS = null;
                    nota.EMPRESA_REF = null;
                    nota.ITEM_PEDIDO = null;
                    nota.CLIENTES = null;
                    nota.FORNECEDOR = null;
                    nota.PROPOSTA_ITEM = null;
                    nota.TRANSPORTADOR = null;
                    nota.UF = null;
                }
            }
        }


        public ICollection<NotaFiscalDTO> RetornarNotasDoPedidoItem(int? ipeID)
        {
            var notas = _dao.RetornarNotasDoPedidoItem(ipeID);
            AnularPropriedades(notas);

            return notas;
            
        }

        public ICollection<NotaFiscalDTO> RetornarNotasServicoDoPedidoItem(int? ipeID)
        {
            var notas = _dao.RetornarNotasServicoDoPedidoItem(ipeID);
            AnularPropriedades(notas);

            return notas;

        }

        public ICollection<NotaFiscalDTO> RetornarNotasDaPropostaItem(int? ppiID)
        {
            var notas = _dao.RetornarNotasDaPropostaItem(ppiID);
            AnularPropriedades(notas);

            return notas;
        }

        public ICollection<NotaFiscalDTO> RetornarNotasServicoDaPropostaItem(int? ppiID)
        {
            var notas = _dao.RetornarNotasServicoDaPropostaItem(ppiID);
            AnularPropriedades(notas);

            return notas;
        }

        public ICollection<NotaFiscalDTO> ListarNotasDeEntradaEnviadaProposta(int? ppiID)
        {
            var notas = _dao.ListarNotasDeEntradaEnviadaProposta(ppiID);
            AnularPropriedades(notas);

            return notas;
        }


        public void EnviarEmailEvento(string email,
            int? nfID,
            TipoLoteItemEnum TipoLoteItem,
            int? repIDEnvio = null,
            string path = null)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(email))
                    throw new ArgumentNullException("Informe o E-Mail.");
                if (nfID == null)
                    throw new ArgumentNullException("Informe o Código da Nota Fiscal.");

                string tipo = "Não informado";
                if (TipoLoteItem == TipoLoteItemEnum.CARTA_CORRECAO)
                    tipo = "Carta de Correção";
                if (TipoLoteItem == TipoLoteItemEnum.CANCELAMENTO)
                    tipo = "Cancelamento";

                var detalhesNotaFiscal = RetornarDetalhesDaNotaFiscal(nfID);
                if (detalhesNotaFiscal != null)
                {
                    string pathArquivo = null;
                    detalhesNotaFiscal.Texto = string.Format("A nota informada abaixo teve um evento de {0} anexado. Segue o arquivo do evento.", tipo);

                    pathArquivo = (!string.IsNullOrWhiteSpace(path)) ? path : detalhesNotaFiscal.PathArquivoNota;
                    if (string.IsNullOrWhiteSpace(pathArquivo))
                        throw new Exception("O caminho do arquivo de nota não existe.");
                    pathArquivo = ProcessarPathParaAnexo(pathArquivo);
                    

                    string assunto = string.Format("[COAD] - Novo Evento de {0} Anexado a Nota Fiscal", tipo);
                    var template = _templateHTMLSRV.RetornarTemplatePorFuncionalidade(7);
                    var corpoEmail = _templateHTMLSRV.ProcessarTemplate(template, detalhesNotaFiscal);

                    email = SysUtils.DecidirEnderecoDeEmail(email);

                    var emailSRV = ServiceFactory.RetornarServico<IEmailSRV>();


                    emailSRV.EnviarEmail(new EmailRequestDTO()
                    {
                        Assunto = assunto,
                        CorpoEmail = corpoEmail,
                        EmailDestino = email,
                        codSMTP = 3,
                        codRepresentante = repIDEnvio,
                        pathAnexo = pathArquivo
                    });

                }
                else
                {
                    throw new Exception(string.Format("Não é possível achar a nota de código '{0}'", nfID));
                }
            }
            catch (Exception e)
            {
                throw new Exception("Não é possível enviar o E-Mail com a Nota Fiscal", e);
            }
        }

        public void EnviarEmailCancNotaServico(string email,
            int? nfID,
            int? repIDEnvio = null,
            string path = null)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(email))
                    throw new ArgumentNullException("Informe o E-Mail.");
                if (nfID == null)
                    throw new ArgumentNullException("Informe o Código da Nota Fiscal.");
                
                var detalhesNotaFiscal = RetornarDetalhesDaNotaFiscal(nfID);
                if (detalhesNotaFiscal != null)
                {
                    string pathArquivo = null;
                    detalhesNotaFiscal.Texto = "A nota informada abaixo foi cancelada. Segue o arquivo do evento.";

                    pathArquivo = (!string.IsNullOrWhiteSpace(path)) ? path : detalhesNotaFiscal.PathArquivoNota;
                    if (string.IsNullOrWhiteSpace(pathArquivo))
                        throw new Exception("O caminho do arquivo de nota não existe.");
                    pathArquivo = ProcessarPathParaAnexo(pathArquivo);


                    string assunto = "[COAD] - Nota Fiscal de Serviço Cancelada";
                    var template = _templateHTMLSRV.RetornarTemplatePorFuncionalidade(7);
                    var corpoEmail = _templateHTMLSRV.ProcessarTemplate(template, detalhesNotaFiscal);

                    email = SysUtils.DecidirEnderecoDeEmail(email);

                    var emailSRV = ServiceFactory.RetornarServico<IEmailSRV>();


                    emailSRV.EnviarEmail(new EmailRequestDTO()
                    {
                        Assunto = assunto,
                        CorpoEmail = corpoEmail,
                        EmailDestino = email,
                        codSMTP = 3,
                        codRepresentante = repIDEnvio,
                        pathAnexo = pathArquivo
                    });

                }
                else
                {
                    throw new Exception(string.Format("Não é possível achar a nota de serviço de código '{0}'", nfID));
                }
            }
            catch (Exception e)
            {
                throw new Exception("Não é possível enviar o E-Mail com a Nota Fiscal de Serviço", e);
            }
        }

        public ICollection<NotaFiscalDTO> ListarNotasDeEntradaEnviada(int? ipeID, int? nfcId = null)
        {
            return _dao.ListarNotasDeEntradaEnviada(ipeID, nfcId);
        }

        public NotaFiscalDTO BuscarPorChave(string chaveNota)
        {
            return _dao.BuscarPorChave(chaveNota);
        }

        public FileInfoDTO RetornarArquivoDaNota(int? ntID)
        {
            var nota = FindById(ntID);

            if(nota != null && nota.Arquivo != null && !string.IsNullOrWhiteSpace(nota.NomeArquivo))
            {
                FileInfoDTO info = new FileInfoDTO(nota.NomeArquivo);
                info.Bytes = nota.Arquivo;
                
                return info;
            }

            return null;
        }

        public bool AssociarNotasFiscaisAntecipadas(int? ppiId, int? ipeId)
        {
            if (ppiId != null && ipeId != null)
            {
                var contratoSRV = ServiceFactory.RetornarServico<ContratoSRV>();
                var itemPedidoSRV = ServiceFactory.RetornarServico<ItemPedidoSRV>();

                var contratos = contratoSRV.ListarContratosDoItemPedido(ipeId);
                var itemPedido = itemPedidoSRV.FindById(ipeId);

                if(contratos != null && contratos.Count > 0)
                {
                    var contrato = contratos.FirstOrDefault();
                    var lstNotaFiscal = ListarNotasDeEntradaEnviadaProposta(ppiId);
                    foreach (var nfe in lstNotaFiscal)
                    {
                        nfe.IPE_ID = ipeId;
                        nfe.CTR_NUM_CONTRATO = contrato.CTR_NUM_CONTRATO;
                        itemPedidoSRV.AlterarStatusPedidoNotaFiscalAntecipada(itemPedido, nfe.NumeroNota);
                    }
                    SaveOrUpdateAll(lstNotaFiscal);
                    return true;
                }

            }
            return false;
        }

        public void CancelarOuDevolverNota(ICollection<NotaFiscalDTO> lstNotaFiscal)
        {
            if(lstNotaFiscal != null)
            {
                var _integrNfeSRV = ServiceFactory.RetornarServico<IntegrNfeSRV>();
                var lstEmpId = lstNotaFiscal.Select(x => x.EMP_ID).Distinct();

                foreach (var empId in lstEmpId)
                {

                    var lstNotaEmpresa = lstNotaFiscal.Where(x => x.EMP_ID == empId).ToList();
                    var lstCodNotaCan = lstNotaEmpresa
                        .Where(x => (x.NF_DATA_EMISSAO != null && x.NF_DATA_EMISSAO.Value.AddDays(1) >= DateTime.Now))
                        .Select(x => x.NF_ID).ToList();
                    var lstCodNotaDev = lstNotaEmpresa.Where(x => !lstCodNotaCan.Contains(x.NF_ID)).Select(x => x.NF_ID).ToList();

                    _integrNfeSRV.CancelarNotaFiscal(lstCodNotaCan);
                    _integrNfeSRV.GerarDevolucao(lstCodNotaDev, empId);
                }
            }
        }

        public Pagina<NotaFiscalDTO> PesquisarNotaFiscal(PesquisaNotaFiscalDTO pesquisa)
        {
            return _dao.PesquisarNotaFiscal(pesquisa);
        }

        public ConsultarLoteRpsResposta TestarNotaServico(int? codItemPedido)
        {
            if(codItemPedido != null)
            {
                using(var scope = new TransactionScope())
                {
                    var contratos = ServiceFactory.RetornarServico<ContratoSRV>().ListarContratosDoItemPedido(codItemPedido);
                    ContratoDTO contrato = null;
                    var servico = ServiceFactory.RetornarServico<ClienteNfseSRV>();
                    if (contratos != null)
                    {
                        contrato = contratos.Where(x => x.CTR_GERA_NOTA_FISCAL == true).FirstOrDefault();
                        var enviarLoteRpsEnvio = _integrNFe.GerarNotaDeServico(codItemPedido, contrato.CTR_NUM_CONTRATO);
                        var certificado = CertificateUtil.RetornarCertificado(2);
                        var retorno = servico.EnviarLoteNotaFiscal(enviarLoteRpsEnvio, certificado);

                        if (!string.IsNullOrWhiteSpace(retorno.Protocolo) 
                            && enviarLoteRpsEnvio != null && enviarLoteRpsEnvio.LoteRps != null 
                            && enviarLoteRpsEnvio.LoteRps.ListaRps.Count > 0)
                        {
                            ConsultarSituacaoLoteRpsEnvio consultaLoteSituacao = new ConsultarSituacaoLoteRpsEnvio()
                            {
                                Prestador = enviarLoteRpsEnvio.LoteRps.ListaRps[0].InfRps.Prestador,
                                Protocolo = retorno.Protocolo
                            };

                            var situacao = servico.ChecarSituacaoDoLote(consultaLoteSituacao, certificado);

                            var numeroTentativas = 5;

                            for(var index = 0; index < numeroTentativas; index++)
                            {
                                if(situacao.Situacao == SituacaoLoteRpsEnum.NAO_PROCESSADO)
                                {
                                    Thread.Sleep(15000);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if(situacao.Situacao != SituacaoLoteRpsEnum.NAO_PROCESSADO)
                            {
                                ConsultarLoteRpsEnvio consultaLote = new ConsultarLoteRpsEnvio()
                                {
                                    Prestador = enviarLoteRpsEnvio.LoteRps.ListaRps[0].InfRps.Prestador,
                                    Protocolo = retorno.Protocolo
                                };

                                var situacaoRps = servico.ConsultarLoteRps(consultaLote, certificado);
                                scope.Complete();

                                return situacaoRps;
                            }
                        }
                    }

                }
            }
            return null;
        }

        public ICollection<NotaFiscalDTO> ListarNotasDeServicoDeEntradaEnviada(int? ipeID, int? nfcId)
        {
            return _dao.ListarNotasDeServicoDeEntradaEnviada(ipeID, nfcId);
        }

        public IList<INFeLote> AdicionarVariasNotasAoLote(ICollection<int> ListCod, string path,  BatchContext batchContext)
        {
            var service = ServiceFactory.RetornarServico<NotaFiscalLoteItemSRV>();
            var itemSRV = ServiceFactory.RetornarServico<ItemPedidoSRV>();

            var lstLotes = service.ListarItens(ListCod);

            if(lstLotes != null && lstLotes.Count > 0)
            {
                NotaFiscalBatchDTO nfeContext = new NotaFiscalBatchDTO()
                {
                    Path = path
                };

                foreach(var lote in lstLotes)
                {
                    var itemPedido = itemSRV.FindById(lote.IPE_ID);

                    if(itemPedido != null)
                    {
                        nfeContext.ListCodPedidos.Add(new NotaFiscaBatchItemDTO()
                        {
                            CodPedido = itemPedido.PED_CRM_ID
                        });
                    }
                }

                var lstLotesRetorno = itemSRV.AdicionarVariasNotasAoLote(nfeContext, batchContext);
                return lstLotesRetorno;
            }
            return new List<INFeLote>();
        }

        public void SalvarNotaFiscalAvulsa(NotaFiscalDTO notaFiscal)
        {
            try
            {
                using(var scope = new TransactionScope())
                {
                    if(notaFiscal != null)
                    {
                        if(notaFiscal.NF_TIPO == 3)
                        {
                            _salvarNotaServico(notaFiscal);
                        }
                        else
                        {
                            throw new Exception("A nota fiscal não é uma nota de serviço avulsa");
                        }
                    }


                    scope.Complete();
                }
            }
            catch(Exception e)
            {
                throw new Exception("Não é possível salvar a Nota Fiscal Avulsa", e);
            }
        }

        private void _salvarNotaServico(NotaFiscalDTO notaFiscal)
        {
            if(notaFiscal != null)
            {
                if(notaFiscal.NF_AVULSA != true)
                {
                    throw new Exception("Não é possível salvar uma nota comum como nota avulsa.");
                }

                if(notaFiscal.NST_ID != null && notaFiscal.NST_ID != 5)
                {
                    throw new Exception("Essa nota não pode ser alterada. Só é possível alterar dados de notas não transmitidas.");
                }

                if(notaFiscal.NST_ID == null)
                {
                    notaFiscal.AdicionarStatus(5);
                }

                if (string.IsNullOrWhiteSpace(notaFiscal.NF_SERIE))
                {
                    notaFiscal.NF_SERIE = "ABC";
                }

                if(notaFiscal.NF_DATA_CADASTRO == null)
                {
                    notaFiscal.NF_DATA_CADASTRO = DateTime.Now;
                }

                if (notaFiscal.NF_DATA_ENTRADA == null && (notaFiscal.NF_TIPO == 0 || notaFiscal.NF_TIPO == 2))
                {
                    notaFiscal.NF_DATA_ENTRADA = DateTime.Now;
                }

                if(notaFiscal.NF_DATA_SAIDA == null && (notaFiscal.NF_TIPO == 1 || notaFiscal.NF_TIPO == 3))
                {
                    notaFiscal.NF_DATA_SAIDA = DateTime.Now;
                }

                if(notaFiscal.NF_DATA_EMISSAO == null)
                {
                    notaFiscal.NF_DATA_EMISSAO = DateTime.Now;
                }

                if(notaFiscal.TDF_ID == null)
                {
                    notaFiscal.TDF_ID = "55";
                }

                var lstNotaItem = notaFiscal.NOTA_FISCAL_ITEM;
                notaFiscal.NOTA_FISCAL_ITEM = null;

                var notaSalva = SaveOrUpdate(notaFiscal);
                notaFiscal.NOTA_FISCAL_ITEM = lstNotaItem;

                if (notaSalva != null && notaFiscal.NF_ID == 0)
                    notaFiscal.NF_ID = notaSalva.NF_ID;

                ServiceFactory.RetornarServico<NotaFiscalItemSRV>().SalvarNotaFiscalItem(notaFiscal);
            }
            
        }

        public NotaFiscalDTO FindByIdFullLoaded(int nfId, bool trazItens)
        {
            var nf = FindById(nfId);
            if(nf != null)
            {
                if (trazItens)
                {
                    ServiceFactory.RetornarServico<NotaFiscalItemSRV>().PreencherNotaFiscalItemNaNota(nf);
                }
            }
            return nf;
        }

        private void MontarLinkDetalheNfse(NfeLinkDanfeDTO link)
        {
            if(link != null)
            {
                var baseUrl = SysUtils.RetornarUrlDetalheNfse();

                if (!string.IsNullOrWhiteSpace(baseUrl))
                {
                    var @params = $"?ccm={link.IM}&nf={link.NumeroNota}&cod={link.CodigoVerificacao}";
                    link.Link = baseUrl + @params;
                }
            }
        }

        private void MontarLinkDANFENfe(NfeLinkDanfeDTO link)
        {
            if (link != null)
            {
                var baseUrl = SysUtils.RetornarUrlDANFENfe();
            
                if (!string.IsNullOrWhiteSpace(baseUrl))
                {
                    var @params = $"?tipoConteudo=XbSeqxE8pl8=&tipoConsulta=completa&nfe={link.ChaveAcesso}&";
                    link.Link = baseUrl + @params;
                }
            }
        }

        public NfeLinkDanfeDTO GerarLinkDanfe(int? nfeId)
        {
            try
            {
                NfeLinkDanfeDTO link = null;
                var notaFiscal = FindById(nfeId);

                if (notaFiscal == null)
                {
                    throw new Exception($"Não é possível achar a nota de código {nfeId}");
                }

                if (notaFiscal.NF_TIPO == 2 || notaFiscal.NF_TIPO == 3)
                {
                    if (string.IsNullOrWhiteSpace(notaFiscal.NF_COD_VERIFICACAO))
                    {
                        throw new Exception("O código de verificação da nota de serviço está ausente.");
                    }

                    if (notaFiscal.EMP_ID == null)
                    {
                        throw new Exception("O código da empresa está ausente.");
                    }

                    var empresa = _empsrv.FindById(notaFiscal.EMP_ID);

                    if (empresa != null)
                    {
                        if (string.IsNullOrWhiteSpace(empresa.EMP_IM))
                        {
                            throw new Exception("A inscrição municipal da empresa está ausente.");
                        }

                        link = new NfeLinkDanfeDTO()
                        {
                            NumeroNota = notaFiscal.NF_NUMERO,
                            IM = empresa.EMP_IM,
                            CodigoVerificacao = notaFiscal.NF_COD_VERIFICACAO.Replace("-", "")
                        };

                        MontarLinkDetalheNfse(link);
                    }
                }

                if (notaFiscal.NF_TIPO == 0 || notaFiscal.NF_TIPO == 1)
                {
                    if (string.IsNullOrWhiteSpace(notaFiscal.NF_CHAVE))
                    {
                        throw new Exception("O chave da nota esta ausente.");
                    }


                    link = new NfeLinkDanfeDTO()
                    {
                        NumeroNota = notaFiscal.NF_NUMERO,
                        ChaveAcesso = notaFiscal.NF_CHAVE
                    };
                    MontarLinkDANFENfe(link);

                }
                return link;
            }
            catch(Exception e)
            {
                throw new Exception("Ocorreu um erro ao tentar gerar as informaçãos para o detalhamento/DANFE da NF", e);
            }
        }


        public void ReexecutarCallbackNfeAutorizada()
        {
            var nfLote = ServiceFactory.RetornarServico<NotaFiscalLoteSRV>();
            var lstlotePendente = nfLote.RetornarLoteAutorizadoItemNaoEnviado();

            if(lstlotePendente != null && lstlotePendente.Count > 0)
            {
                foreach(var lote in lstlotePendente)
                {
                    using(var scope = new TransactionScope())
                    {
                        _integrNFe.ExecutarCallBacksLoteItem(lote);
                        scope.Complete();
                    }
                }
            }
        }
        
        public void ExecutarCallBacksLoteItem(int? nflId)
        {
            try
            {
                var nfLoteSRV = ServiceFactory.RetornarServico<NotaFiscalLoteSRV>();

                var lote = nfLoteSRV.FindByIdFullLoaded(nflId, true);
                if(lote == null)
                {
                    throw new Exception($"O Lote {nflId} não foi encontrado");
                }
                using (var scope = new TransactionScope())
                {
                    _integrNFe.ExecutarCallBacksLoteItem(lote, null, true);
                    scope.Complete();
                }
            }
            catch(Exception e)
            {
                throw new Exception("Não foi possível executar os callbacks da nota fiscal", e);
            }
        }
    }
}
