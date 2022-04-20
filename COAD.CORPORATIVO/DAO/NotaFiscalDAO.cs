using System;
using System.IO;
using System.Xml;
using System.Web;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using COAD.CORPORATIVO.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.DAO;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using System.Globalization;
using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Exceptions;
using GenericCrud.Exceptions;
using GenericCrud.Util;
using COAD.CORPORATIVO.Model.Dto.Custons.FonteDadosTemplate;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using System.Data.Objects;
using COAD.CORPORATIVO.Model.Dto.FiltersInfo;

namespace COAD.CORPORATIVO.DAO
{
    public class NotaFiscalDAO : DAOAdapter<NOTA_FISCAL, NotaFiscalDTO>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public NotaFiscalDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }
        public NotaFiscalDTO Buscar(int _nf_id)
        {

            var _nota_fiscal = (from n in db.NOTA_FISCAL
                                        where n.NF_ID == _nf_id 
                                        select n).FirstOrDefault();
   
            return ToDTO(_nota_fiscal);
        }

        public IList<NotaFiscalDTO> BuscarNfeCliente(int _cli_id)
        {

            var _nota_fiscal = (from n in db.NOTA_FISCAL
                                where n.CLI_ID == _cli_id
                                select n).OrderByDescending(x => x.NF_DATA_EMISSAO);


            return ToDTO(_nota_fiscal);
        }
        public NotaFiscalDTO Buscar(int _nf_numero, string _nf_serie, int _nf_tipo, int _cli_id)
        {

            var _nota_fiscal = (from n in db.NOTA_FISCAL
                                        where n.NF_NUMERO == _nf_numero &&
                                              n.NF_SERIE == _nf_serie &&
                                              n.NF_TIPO == _nf_tipo &&
                                              n.CLI_ID == _cli_id
                                        select n).FirstOrDefault();
            return ToDTO(_nota_fiscal);
        }
        public Pagina<NotaFiscalDTO> BuscarPorPeriodo(int? _nf_numero, string _cpfCnpj, int numpagina = 1, int linhas = 10, bool? antecipada = null, bool? avulsa = null)
        {
            var _lista = (from n in db.NOTA_FISCAL
                          join l in db.CLIENTES on n.CLI_ID equals l.CLI_ID into l1 from l2 in l1.DefaultIfEmpty()
                          where ((_nf_numero == null) || (_nf_numero != null && _nf_numero == n.NF_NUMERO)) &&
                                ((_cpfCnpj == null) || (_cpfCnpj != null && _cpfCnpj == l2.CLI_CPF_CNPJ)) &&
                                (antecipada == null || antecipada == false || n.NF_NOTA_ANTECIPADA == true) &&
                                (avulsa == null || avulsa == false || n.NF_AVULSA == true)
                          select n);


            var _lstctr = (from n in db.NOTA_FISCAL
                           join a in db.ASSINATURA on n.CLI_ID equals a.CLI_ID
                           join c in db.CONTRATOS  on n.NF_NUMERO equals c.CTR_NUMERO_NOTA
                           join l in db.CLIENTES   on n.CLI_ID equals l.CLI_ID
                           where ((_nf_numero == null) || (_nf_numero != null && _nf_numero == n.NF_NUMERO)) &&
                                 ((_cpfCnpj   == null) || (_cpfCnpj   != null && _cpfCnpj   == l.CLI_CPF_CNPJ)) 
                           select c);

            var _retorno = ToDTOPage(_lista, numpagina, linhas);

            foreach (var _item in _retorno.lista)
            {
                var _contrato = _lstctr.Where(x => x.CTR_NUMERO_NOTA == _item.NF_NUMERO).FirstOrDefault();

                if (_contrato != null)
                    _item.CTR_NUM_CONTRATO = _contrato.CTR_NUM_CONTRATO;
            }

            return _retorno;
        }
        public IList<NotaFiscalDTO> BuscarPorPeriodo(int _nf_tipo, int _for_id, int _emp_id, DateTime? _dtini, DateTime? _dtfim)
        {
            DateTime d = new DateTime();
            d = (DateTime)_dtfim;
            _dtfim = d.AddDays(1);



            var _lista = (from n in db.NOTA_FISCAL
                          where ((_for_id == 0) || ((_for_id > 0) && (n.FOR_ID == _for_id))) &&
                              (((_nf_tipo == 0 || _nf_tipo == 2) && (n.NF_DATA_ENTRADA >= _dtini && n.NF_DATA_ENTRADA < _dtfim)) ||
                                  ((_nf_tipo == 1 || _nf_tipo == 3) && (n.NF_DATA_EMISSAO >= _dtini && n.NF_DATA_EMISSAO < _dtfim))) &&
                              ((_emp_id == 0) || ((_emp_id > 0) && (n.EMP_ID == _emp_id))) &&
                              (n.NF_TIPO == _nf_tipo)
                          select n).ToList();

            if (_nf_tipo == 0 || _nf_tipo == 2)
                _lista.OrderByDescending(x => x.NF_DATA_ENTRADA).ToList();
            else
                _lista.OrderByDescending(x => x.NF_DATA_EMISSAO).ToList();


            return ToDTO(_lista);
        }

        public Pagina<NotaFiscalDTO> BuscarPorPeriodo(int _nf_tipo, int _for_id, int _emp_id, DateTime? _dtini, DateTime? _dtfim, int numpagina = 1, int linhas = 10, bool? antecipada = null, bool? avulsa = null)
        {
            DateTime d = new DateTime();
            d = (DateTime)_dtfim;
            _dtfim = d.AddDays(1);

            var _lista = (from n in db.NOTA_FISCAL
                          join l in db.CLIENTES on n.CLI_ID equals l.CLI_ID into l1
                          from l2 in l1.DefaultIfEmpty()
                          where ((_for_id == 0) || ((_for_id > 0) && (n.FOR_ID == _for_id))) &&
                                (((_nf_tipo == 0 || _nf_tipo == 2) && (n.NF_DATA_ENTRADA >= _dtini && n.NF_DATA_ENTRADA < _dtfim)) ||
                                ((_nf_tipo == 1 || _nf_tipo == 3) && (n.NF_DATA_EMISSAO >= _dtini && n.NF_DATA_EMISSAO < _dtfim))) &&
                                ((_emp_id == 0) || ((_emp_id > 0) && (n.EMP_ID == _emp_id))) &&
                                (n.NF_TIPO == _nf_tipo)
                          select n).OrderByDescending(x => x.NF_DATA_EMISSAO).ThenBy(x => x.NF_NUMERO);

             var _lstctr = (from n in db.NOTA_FISCAL
                           join a in db.ASSINATURA on n.CLI_ID equals a.CLI_ID
                           join c in db.CONTRATOS on n.NF_NUMERO equals c.CTR_NUMERO_NOTA
                           where ((_for_id == 0) || ((_for_id > 0) && (n.FOR_ID == _for_id))) &&
                                 (((_nf_tipo == 0 || _nf_tipo == 2) && (n.NF_DATA_ENTRADA >= _dtini && n.NF_DATA_ENTRADA < _dtfim)) ||
                                 ((_nf_tipo == 1 || _nf_tipo == 3) && (n.NF_DATA_EMISSAO >= _dtini && n.NF_DATA_EMISSAO < _dtfim))) &&
                                 ((_emp_id == 0) || ((_emp_id > 0) && (n.EMP_ID == _emp_id))) &&
                                 (n.NF_TIPO == _nf_tipo)
                           select c);


            if (antecipada == true)
                _lista = _lista.Where(x => x.NF_NOTA_ANTECIPADA == true).OrderByDescending(x => x.NF_DATA_EMISSAO).ThenBy(x => x.NF_NUMERO);

            if (avulsa == true)
                _lista = _lista.Where(x => x.NF_AVULSA == true).OrderByDescending(x => x.NF_DATA_EMISSAO).ThenBy(x => x.NF_NUMERO);

            var _retorno = ToDTOPage(_lista, numpagina, linhas);

            foreach (var _item in _retorno.lista)
            {
                var _contrato = _lstctr.Where(x => x.CTR_NUMERO_NOTA == _item.NF_NUMERO).FirstOrDefault();

                if (_contrato != null)
                    _item.CTR_NUM_CONTRATO = _contrato.CTR_NUM_CONTRATO;
            }
      
            _retorno.lista.OrderByDescending(x => x.NF_DATA_EMISSAO).ThenBy(x => x.NF_NUMERO);


            return _retorno;
        }
        public List<NOTA_FISCAL> BuscarNotasPeriodoTipo(int _nf_tipo, int _emp_id, DateTime? _dtini, DateTime? _dtfim)
        {
            DateTime d = new DateTime();
            d = (DateTime)_dtfim;
            _dtfim = d.AddDays(1);

            List<NOTA_FISCAL> _lista = (from n in db.NOTA_FISCAL
                                       where ((_dtini == null) || ((n.NF_DATA_ENTRADA >= _dtini && n.NF_DATA_ENTRADA < _dtfim))) &&
                                             
                                             ((_emp_id == 0)   || ((_emp_id > 0) && (n.EMP_ID == _emp_id))) &&
                                             (((_nf_tipo == 0  || _nf_tipo == 1) && (new[] { "01", "1B", "04", "55", "65"}.Contains(n.TDF_ID))) ||
                                              ((_nf_tipo == 2  || _nf_tipo == 3) && (new[] { "21", "22" }.Contains(n.TDF_ID))))
                                       orderby n.NF_DATA_ENTRADA descending
                                       select n).ToList();
            return _lista;
        }

        public virtual void IncluirNfModel(NotaFiscalDTO _nf)
        {
            this.IncluirNf(_nf);
        }
        public virtual void IncluirNf(NotaFiscalDTO _nf)
        {

        }


        public virtual void ExcluirNf(NotaFiscalDTO _nota)
        {
            try
            {
                string _notafiscal = _nota.NF_ID.ToString()+" - "+_nota.NF_TIPO + " - " + _nota.NF_NUMERO.ToString() + "/" + _nota.NF_SERIE.ToString();

                db.EXCLUIR_NOTA_FISCAL(_nota.NF_ID, SessionContext.autenticado.IP_ACESSO, SessionContext.autenticado.PATH, SessionContext.autenticado.USU_LOGIN);

                SysException.RegistrarLog("Registro excluído com sucesso !! Nota Fiscal >>>> " + _notafiscal, "", SessionContext.autenticado);

            }
            catch (DbEntityValidationException dbEx)
            {
                string _erro = "";

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        _erro += String.Format("Erro ao gravar a nota fiscal: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                SysException.RegistrarLog(_erro, "", SessionContext.autenticado);

                throw new Exception(_erro);
            }
            catch (DbUpdateException e)
            {
                SysException.RegistrarLog(SysException.Show(e), SysException.ShowIdException(e), SessionContext.autenticado);

                throw new Exception(SysException.Show(e));
            }
            catch (EntityException ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);

                throw new Exception(SysException.Show(ex));
            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);

                throw new Exception(SysException.Show(ex));
            }
        }
        public IList<NotaFiscalCustomDTO> BuscarNotasPeriodoSintetico(DateTime _dtini, DateTime _dtfim)
        {

            DateTime d = new DateTime();
            d = (DateTime)_dtfim;
            _dtfim = d.AddDays(1);

            var _listaEmp = new EmpresaDAO().FindAll();
            var _listaGeral = new List<NotaFiscalCustomDTO>();

            foreach (var item in _listaEmp)
            {
                var _ite = new NotaFiscalCustomDTO();
                _ite.EMP_ID = item.EMP_ID;
                _ite.EMP_RAZAO_SOCIAL = item.EMP_RAZAO_SOCIAL;
                _listaGeral.Add(_ite);
            }
            
            var _listaEntrada = (from n in db.NOTA_FISCAL
                                 where (((n.NF_TIPO == 0 ) && ((n.NF_DATA_ENTRADA >= _dtini && n.NF_DATA_ENTRADA < _dtfim))))
                                    && n.EMP_ID != null
                                 group n by new { n.EMP_ID, n.NF_DATA_ENTRADA.Value.Month, n.NF_DATA_ENTRADA.Value.Year } into f
                                 orderby f.Key.EMP_ID
                                 select new NotaFiscalCustomDTO
                                 {
                                     EMP_ID = f.Key.EMP_ID,
                                     EMP_RAZAO_SOCIAL = "",
                                     NF_QTDE = f.Count(),
                                     NF_VLR_ENTRADA = f.Sum(x => x.NF_VLR_NOTA),
                                     NF_VLR_SAIDA = 0

                                 }).ToList();



            var _listaEntradaServ = (from n in db.NOTA_FISCAL
                                 where (((n.NF_TIPO == 2) && ((n.NF_DATA_ENTRADA >= _dtini && n.NF_DATA_ENTRADA < _dtfim))))
                                    && n.EMP_ID != null
                                 group n by new { n.EMP_ID, n.NF_DATA_ENTRADA.Value.Month, n.NF_DATA_ENTRADA.Value.Year } into f
                                 orderby f.Key.EMP_ID
                                 select new NotaFiscalCustomDTO
                                 {
                                     EMP_ID = f.Key.EMP_ID,
                                     EMP_RAZAO_SOCIAL = "",
                                     NF_QTDE = f.Count(),
                                     NF_VLR_ENTRADA_SERV = f.Sum(x => x.NF_VLR_NOTA),
                                     NF_VLR_SAIDA = 0

                                 }).ToList();

            var _listaSaida = (from n in db.NOTA_FISCAL
                          where  ((n.NF_TIPO == 1) && ((n.NF_DATA_EMISSAO >= _dtini && n.NF_DATA_EMISSAO < _dtfim)))
                              && n.EMP_ID != null
                          group n by new { n.EMP_ID, n.NF_DATA_EMISSAO.Value.Month, n.NF_DATA_EMISSAO.Value.Year } into f
                          orderby f.Key.EMP_ID
                          select new NotaFiscalCustomDTO
                          {
                              EMP_ID = f.Key.EMP_ID,
                              EMP_RAZAO_SOCIAL = "",
                              NF_QTDE = f.Count(),
                              NF_VLR_ENTRADA = 0,
                              NF_VLR_SAIDA = f.Sum(x => x.NF_VLR_NOTA)

                          }).ToList();

            var _listaSaidaServ = (from n in db.NOTA_FISCAL
                               where ((n.NF_TIPO == 3) && ((n.NF_DATA_EMISSAO >= _dtini && n.NF_DATA_EMISSAO < _dtfim)))
                                   && n.EMP_ID != null
                               group n by new { n.EMP_ID, n.NF_DATA_EMISSAO.Value.Month, n.NF_DATA_EMISSAO.Value.Year } into f
                               orderby f.Key.EMP_ID
                               select new NotaFiscalCustomDTO
                               {
                                   EMP_ID = f.Key.EMP_ID,
                                   EMP_RAZAO_SOCIAL = "",
                                   NF_QTDE = f.Count(),
                                   NF_VLR_ENTRADA = 0,
                                   NF_VLR_SAIDA_SERV = f.Sum(x => x.NF_VLR_NOTA)

                               }).ToList();

            foreach (var item in _listaGeral)
            {
                var _ent  = _listaEntrada.Where(x => x.EMP_ID == item.EMP_ID).FirstOrDefault();
                var _ents = _listaEntradaServ.Where(x => x.EMP_ID == item.EMP_ID).FirstOrDefault();
                var _sai  = _listaSaida.Where(x => x.EMP_ID == item.EMP_ID).FirstOrDefault();
                var _sais = _listaSaidaServ.Where(x => x.EMP_ID == item.EMP_ID).FirstOrDefault();

                if (_ent != null)
                    item.NF_VLR_ENTRADA = _ent.NF_VLR_ENTRADA;

                if (_ents != null)
                    item.NF_VLR_ENTRADA_SERV = _ents.NF_VLR_ENTRADA_SERV;

                if (_sai != null)
                    item.NF_VLR_SAIDA = _sai.NF_VLR_SAIDA;

                if (_sais != null)
                    item.NF_VLR_SAIDA_SERV = _sais.NF_VLR_SAIDA_SERV;

            }

            return _listaGeral;

        }
        public Pagina<NotaFiscalDTO> BuscarNotasPeriodo(int _emp_id, int _nf_tipo, DateTime _dtini, DateTime _dtfim, int numpagina = 1, int linhas = 10)
        {

            DateTime d = new DateTime();
            d = (DateTime)_dtfim;
            _dtfim = d.AddDays(1);

            var _listaSaida = (from n in db.NOTA_FISCAL
                               where (((n.NF_TIPO == 0 || n.NF_TIPO == 2) && ((n.NF_DATA_ENTRADA >= _dtini && n.NF_DATA_ENTRADA < _dtfim))) ||
                                      ((n.NF_TIPO == 1 || n.NF_TIPO == 3) && ((n.NF_DATA_EMISSAO >= _dtini && n.NF_DATA_EMISSAO < _dtfim)))) &&
                                       (n.EMP_ID == _emp_id) &&
                                       (n.NF_TIPO == _nf_tipo) 
                               orderby n.NF_TIPO ascending
                               select n).OrderByDescending(x => x.NF_DATA_EMISSAO);

            return ToDTOPage(_listaSaida, numpagina, linhas);

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
            
            DateTime d = new DateTime();
            d = (DateTime)_dtfim;
            _dtfim = d.AddDays(1);

            var _lista = (from n in db.NOTA_FISCAL
                          where (((n.NF_TIPO == 0 || n.NF_TIPO == 2) && ((n.NF_DATA_ENTRADA >= _dtini && n.NF_DATA_ENTRADA < _dtfim))) ||
                                 ((n.NF_TIPO == 1 || n.NF_TIPO == 3) && ((n.NF_DATA_EMISSAO >= _dtini && n.NF_DATA_EMISSAO < _dtfim)))) &&
                                 (n.EMP_ID == _emp_id)
                          orderby n.NF_TIPO ascending
                          select n).ToList();

            return ToDTO(_lista);

        }
        public IList<NotaFiscalDTO> BuscarNFEntradaPeriodo(int _emp_id, DateTime? _dtini, DateTime? _dtfim)
        {

            DateTime d = new DateTime();
            d = (DateTime)_dtfim;
            _dtfim = d.AddDays(1);

            var _lista = (from n in db.NOTA_FISCAL
                                        where ((_dtini == null) || ((n.NF_DATA_ENTRADA >= _dtini && n.NF_DATA_ENTRADA < _dtfim))) &&
                                              ((_emp_id == 0) || ((_emp_id > 0) && (n.EMP_ID == _emp_id))) &&
                                              (n.NF_TIPO == 0)
                                        orderby n.NF_DATA_ENTRADA descending
                                        select n);

            return ToDTO(_lista);

        }
        public IList<NotaFiscalDTO> BuscarNFEntradaServicoPeriodo(int _emp_id, DateTime? _dtini, DateTime? _dtfim)
        {

            DateTime d = new DateTime();
            d = (DateTime)_dtfim;
            _dtfim = d.AddDays(1);

            List<NOTA_FISCAL> _lista = (from n in db.NOTA_FISCAL
                                        where ((_dtini == null) || ((n.NF_DATA_ENTRADA >= _dtini && n.NF_DATA_ENTRADA < _dtfim))) &&
                                              ((_emp_id == 0) || ((_emp_id > 0) && (n.EMP_ID == _emp_id))) &&
                                              (n.NF_TIPO == 2)
                                        orderby n.NF_DATA_ENTRADA descending
                                        select n).ToList();

            return ToDTO(_lista);

        }
        public IList<NotaFiscalDTO> BuscarNFSaidaPeriodo(int _emp_id, DateTime? _dtini, DateTime? _dtfim)
        {
            DateTime d = new DateTime();
            d = (DateTime)_dtfim;
            _dtfim = d.AddDays(1);

            List<NOTA_FISCAL> _lista = (from n in db.NOTA_FISCAL
                                        where ((_dtini == null) || ((n.NF_DATA_EMISSAO >= _dtini && n.NF_DATA_EMISSAO < _dtfim))) &&
                                              ((_emp_id == 0) || ((_emp_id > 0) && (n.EMP_ID == _emp_id))) &&
                                              (n.NF_TIPO == 2)
                                        orderby n.NF_DATA_EMISSAO descending
                                        select n).ToList();

            return ToDTO(_lista);

        }
        public IList<NotaFiscalDTO> BuscarNFSaidaServicoPeriodo(int _emp_id, DateTime? _dtini, DateTime? _dtfim)
        {
            DateTime d = new DateTime();
            d = (DateTime)_dtfim;
            _dtfim = d.AddDays(1);

            List<NOTA_FISCAL> _lista = (from n in db.NOTA_FISCAL
                                        where ((_dtini == null) || ((n.NF_DATA_EMISSAO >= _dtini && n.NF_DATA_EMISSAO < _dtfim))) &&
                                              ((_emp_id == 0) || ((_emp_id > 0) && (n.EMP_ID == _emp_id))) &&
                                              (n.NF_TIPO == 1)
                                        orderby n.NF_DATA_EMISSAO descending
                                        select n).ToList();

            return ToDTO(_lista);

        }
        public IList<FORNECEDOR> BuscarFornecedorNFPeriodo(int _emp_id, DateTime _dtinicial, DateTime _dtfinal)
        {
            DateTime d = new DateTime();
            d = (DateTime)_dtfinal;
            _dtfinal = d.AddDays(1);

            List<FORNECEDOR> _lista = new List<FORNECEDOR>();

            var _dados = (from n in db.NOTA_FISCAL
                          where (n.NF_DATA_ENTRADA >= _dtinicial && n.NF_DATA_ENTRADA < _dtfinal) &&
                                ((_emp_id  == 0) || ((_emp_id > 0) && (n.EMP_ID == _emp_id))) &&
                                (n.NF_TIPO == 0 || n.NF_TIPO == 2) 
                          select n.FORNECEDOR).Distinct().ToList();

            if (_dados != null)
                _lista = (List<FORNECEDOR>)_dados;

            return _lista;

        }
        public IList<CLIENTES> BuscarClientesNFPeriodo(int _emp_id, DateTime _dtinicial, DateTime _dtfinal)
        {
            DateTime d = new DateTime();
            d = (DateTime)_dtfinal;
            _dtfinal = d.AddDays(1);

            List<CLIENTES> _lista = new List<CLIENTES>();

            var _dados = (from n in db.NOTA_FISCAL
                          where (n.NF_DATA_EMISSAO >= _dtinicial && n.NF_DATA_EMISSAO < _dtfinal) &&
                                (n.NF_TIPO == 1 || n.NF_TIPO == 3) && (n.EMP_ID == _emp_id)
                          select n.CLIENTES).Distinct().ToList();

            if (_dados != null)
                _lista = (List<CLIENTES>)_dados;

            return _lista;

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
           
            DateTime d = new DateTime();
            d = (DateTime)_dtfim;
            _dtfim = d.AddDays(1);

            List<CFOP_TABLE> _lista = new List<CFOP_TABLE>();

            var _nota_fiscal_item = (from n in db.NOTA_FISCAL_ITEM
                                     where (((n.NOTA_FISCAL.NF_TIPO == 0 || n.NF_TIPO == 2) && ((n.NOTA_FISCAL.NF_DATA_ENTRADA >= _dtini && n.NOTA_FISCAL.NF_DATA_ENTRADA < _dtfim))) //||
                                            //((n.NOTA_FISCAL.NF_TIPO == 1 || n.NF_TIPO == 3) && ((n.NOTA_FISCAL.NF_DATA_EMISSAO >= _dtini && n.NOTA_FISCAL.NF_DATA_EMISSAO < _dtfim)))
                                            ) &&
                                             (n.NOTA_FISCAL.EMP_ID == _emp_id) && (n.CFOP != null && n.CFOP != "")
                                      orderby n.NF_TIPO ascending
                                      select n).ToList();

            if (_nota_fiscal_item != null)
                _lista = _nota_fiscal_item.Select(x => x.CFOP_TABLE).Distinct().ToList();
            
            return _lista;

        }
        /// <summary>
        /// Este metodo foi alterado e atualmente so retorna Produtos de notas de entrada.
        /// </summary>
        /// <param name="_emp_id"></param>
        /// <param name="_dtini"></param>
        /// <param name="_dtfim"></param>
        /// <returns></returns>
        public List<PRODUTOS> BuscarProdutosNFPeriodo(int _emp_id, DateTime _dtini, DateTime _dtfim)
        {
            DateTime d = new DateTime();
            d = (DateTime)_dtfim;
            _dtfim = d.AddDays(1);

            List<PRODUTOS> _produtos  = (from n in db.NOTA_FISCAL_ITEM
                                         where (n.NOTA_FISCAL.EMP_ID == _emp_id) &&
                                               (((n.NOTA_FISCAL.NF_DATA_ENTRADA >= _dtini && n.NOTA_FISCAL.NF_DATA_ENTRADA < _dtfim) && (n.NOTA_FISCAL.NF_TIPO == 0)) //||
                                                //((n.NOTA_FISCAL.NF_DATA_EMISSAO >= _dtini && n.NOTA_FISCAL.NF_DATA_EMISSAO < _dtfim) && (n.NOTA_FISCAL.NF_TIPO == 1))
                                                )
                                       select n.PRODUTOS).Distinct().ToList();
           
            foreach (var _item in _produtos)
            {
                _item.PRO_NOME = (from n in db.NOTA_FISCAL_ITEM
                                  where (n.NOTA_FISCAL.EMP_ID == _emp_id) &&
                                        (n.PRO_ID == _item.PRO_ID) &&
                                        (((n.NOTA_FISCAL.NF_DATA_ENTRADA >= _dtini && n.NOTA_FISCAL.NF_DATA_ENTRADA < _dtfim) && (n.NOTA_FISCAL.NF_TIPO == 0)) ||
                                         ((n.NOTA_FISCAL.NF_DATA_EMISSAO >= _dtini && n.NOTA_FISCAL.NF_DATA_EMISSAO < _dtfim) && (n.NOTA_FISCAL.NF_TIPO == 1)))
                                  select n).FirstOrDefault().NFI_PRO_NOME;
            }

            return _produtos;

        }

        public byte[] XmlDocToBytes(XmlDocument _doc)
        {
            Encoding encoding = Encoding.UTF8;
            byte[] docAsBytes = encoding.GetBytes(_doc.OuterXml);
            return docAsBytes;
        }
        public XmlDocument BytesToXmlDoc(byte[] _linhas)
        {

            XmlDocument doc = new XmlDocument();
            string xml = Encoding.UTF8.GetString(_linhas);
            doc.LoadXml(xml);

            return doc;
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
        public DetalhesDaNotaFiscalDTO RetornarDetalhesDaNotaFiscal(int? nfID)
        {
            var query = (from 
                            nf in db.NOTA_FISCAL join
                            nfItm in db.NOTA_FISCAL_ITEM on nf.NF_ID equals nfItm.NF_ID join
                            pro in db.PRODUTOS on nfItm.PRO_ID equals pro.PRO_ID
                            
                        where nf.NF_ID == nfID
                        select new DetalhesDaNotaFiscalDTO()
                        {
                            ClienteNome = (nf.CLIENTES != null) ? nf.CLIENTES.CLI_NOME : null,
                            EmpID = nf.EMP_ID,
                            NotaDataEmissao = nf.NF_DATA_EMISSAO,
                            NotaFiscalNumero = nf.NF_NUMERO,
                            ValorBruto = nf.NF_VLR_PROD,
                            NotaValor = nf.NF_VLR_NOTA,
                            PathArquivoNota = nf.NF_PATH_ARQUIVO,
                            ProdutoNome = pro.PRO_NOME,
                            Discriminacao = nfItm.NFI_DISCRIMINACAO_SERVICO,
                            NotaChave = nf.NF_CHAVE,
                            CodVerificacao = nf.NF_COD_VERIFICACAO,
                            Serie = nf.NF_SERIE

                        }).FirstOrDefault();
            return query;
        }

        public ICollection<NotaFiscalDTO> RetornarNotasDoPedidoItem(int? ipeID)
        {
            var query = (from nf in
                             db.NOTA_FISCAL
                         where 
                            nf.IPE_ID == ipeID &&
                            (nf.NF_TIPO == 0 || nf.NF_TIPO == 1)
                         orderby nf.NF_DATA_EMISSAO descending
                         select nf);
            return ToDTO(query);
        }

        public ICollection<NotaFiscalDTO> RetornarNotasServicoDoPedidoItem(int? ipeID)
        {
            var query = (from nf in
                             db.NOTA_FISCAL
                         where 
                            nf.IPE_ID == ipeID &&
                            (nf.NF_TIPO == 2 || nf.NF_TIPO == 3)
                         orderby nf.NF_DATA_EMISSAO descending
                         select nf);
            return ToDTO(query);
        }

        public ICollection<NotaFiscalDTO> RetornarNotasDaPropostaItem(int? ppiID)
        {
            var query = (from nf in
                             db.NOTA_FISCAL
                         where 
                            nf.PPI_ID == ppiID &&
                            (nf.NF_TIPO == 0 || nf.NF_TIPO == 1)
                         orderby nf.NF_DATA_EMISSAO descending
                         select nf);
            return ToDTO(query);
        }

        public ICollection<NotaFiscalDTO> RetornarNotasServicoDaPropostaItem(int? ppiID)
        {
            var query = (from nf in
                             db.NOTA_FISCAL
                         where
                            nf.PPI_ID == ppiID &&
                            (nf.NF_TIPO == 2 || nf.NF_TIPO == 3)
                         orderby nf.NF_DATA_EMISSAO descending
                         select nf);
            return ToDTO(query);
        }

        public ICollection<NotaFiscalDTO> ListarNotasDeEntradaEnviada(int? ipeID, int? nfcId = null)
        {
            var query = (from 
                            nf in db.NOTA_FISCAL join
                            ltNf in db.NOTA_FISCAL_LOTE_ITEM on nf.NF_ID equals ltNf.NF_ID
                         where 
                            nf.IPE_ID == ipeID &&
                            nf.NF_TIPO == 1 &&
                            (nf.NST_ID == 1 || nf.NST_ID == 2) &&
                            (nfcId == null || ltNf.NFC_ID == nfcId)
                         orderby nf.NF_DATA_EMISSAO descending
                         select nf);
            return ToDTO(query);
        }

        public ICollection<NotaFiscalDTO> ListarNotasDeEntradaEnviadaProposta(int? ppiID)
        {
            var query = (from nf in
                             db.NOTA_FISCAL
                         where
                            nf.PPI_ID == ppiID &&
                            nf.NF_TIPO == 1 &&
                            (nf.NST_ID == 1 || nf.NST_ID == 2)
                         orderby nf.NF_DATA_EMISSAO descending
                         select nf);
            return ToDTO(query);
        }

        public NotaFiscalDTO BuscarPorChave(string chaveNota)
        {
            var query = (from nf in
                             db.NOTA_FISCAL
                         where
                            nf.NF_CHAVE == chaveNota
                         orderby nf.NF_DATA_EMISSAO descending
                         select nf);
            return ToDTO(query.FirstOrDefault());
        }

        public Pagina<NotaFiscalDTO> PesquisarNotaFiscal(PesquisaNotaFiscalDTO pesquisa)
        {
            DateTime? DataInicial = pesquisa.DataInicial;
            DateTime? DataFinal = pesquisa.DataFinal;
            int? NumeroNota = pesquisa.NumeroNota;
            string ChaveNota = pesquisa.ChaveNota;
            string CNPJ = pesquisa.CNPJ;
            int? EmpID = pesquisa.EmpID;
            int? Tipo = pesquisa.Tipo;
            int pagina = pesquisa.Pagina;
            int registrosPorPagina = pesquisa.RegistrosPorPagina;

            var query = (from
                            nf in db.NOTA_FISCAL
                         where
                            (NumeroNota == null || nf.NF_NUMERO == NumeroNota) &&
                            (ChaveNota == null || nf.NF_CHAVE == ChaveNota) &&
                            (EmpID == null || nf.EMP_ID == EmpID) &&
                            (Tipo == null || nf.NF_TIPO == Tipo) &&
                            (DataInicial == null || EntityFunctions.TruncateTime(nf.NF_DATA_EMISSAO) >= EntityFunctions.TruncateTime(DataInicial)) &&
                            (DataFinal == null || EntityFunctions.TruncateTime(nf.NF_DATA_EMISSAO) <= EntityFunctions.TruncateTime(DataFinal)) &&
                            (CNPJ == null ||
                            (   
                                nf.CLI_ID != null && 
                                (from
                                    cli in db.CLIENTES
                                 where
                                    cli.CLI_CPF_CNPJ == CNPJ
                                 select cli.CLI_ID).Contains((int)nf.CLI_ID)
                            ))
                         select nf);

            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        public ICollection<NotaFiscalDTO> ListarNotasDeServicoDeEntradaEnviada(int? ipeID, int? nfcId)
        {
            var query = (from
                            nf in db.NOTA_FISCAL join
                            ltNf in db.NOTA_FISCAL_LOTE_ITEM on nf.NF_ID equals ltNf.NF_ID
                         where
                            nf.IPE_ID == ipeID &&
                            nf.NF_TIPO == 3 &&
                            (nf.NST_ID == 1 || nf.NST_ID == 2) &&
                            ltNf.NFC_ID == nfcId

                         orderby nf.NF_DATA_EMISSAO descending
                         select nf);
            return ToDTO(query);
        }
    }
}