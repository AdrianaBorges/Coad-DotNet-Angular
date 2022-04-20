using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Dao.Base.Pagination;
using System.Transactions;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Service;
using GenericCrud.Util;

namespace COAD.CORPORATIVO.DAO
{/// <summary>
/// 
/// </summary>
    public partial class AcessosDTO
    {
        public string TDC_ID { get; set; }
        public string TAB_DESCRICAO { get; set; }
        public Nullable<int> QTDE { get; set; }
    }
    public partial class ClienteAcessosDTO
    {
        public ClienteAcessosDTO()
        {
            this.ACESSOS = new HashSet<AcessosDTO>();
        }

        public string ASN_NUM_ASSINATURA { get; set; }
        public string CLI_NOME { get; set; }
        public Nullable<int> QTDE { get; set; }
        public Nullable<int> QTDE_TOTAL { get; set; }

        public virtual ICollection<AcessosDTO> ACESSOS { get; set; }
    }



    public class AssinaturaDAO : DAOAdapter<ASSINATURA, AssinaturaDTO, string>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public AssinaturaDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

   
        public Pagina<AssinaturaDTO> BuscarAssinaturasProtocoladas(int _pagina = 1, int _registroPorPagina = 7)
        {
            var _query = (from x in db.ASSINATURA
                         where x.ASN_PROTOCOLADA == true
                        select x);

            return ToDTOPage(_query, _pagina, _registroPorPagina);
        }

        public void GerarSenha(string _assinatura)
        {
            db.GERAR_SENHA_ASSINATURA(_assinatura, "COADSYS");
        }
        public IList<AssinaturaTransferenciaDTO> BuscarTrasferenciaPorPeriodo(int _mes, int _ano, string _assinatura)
        {
            return new AssinaturaTransferenciaSRV().BuscarTrasferenciaPorPeriodo(_mes, _ano, _assinatura);
        }

        public void TrasferirVigencia(TransfAssinaturaCustomDTO _transf)
        {

        }


        public IList<ClienteAcessosDTO> BuscarAcessoClientesPorPeriodo(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, string _asn_num_assinatura = null, int? _pro_id = null)
        {

            if (_asn_num_assinatura == "")
                _asn_num_assinatura = null;

            if (_pro_id == null)
                _pro_id = 0;

            DateTime _dtinicial;
            DateTime _dtfinal;

            if (_dtini == null)
            {
                _dtinicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                _dtfinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            else
            {
                _dtinicial = new DateTime(_dtini.Value.Year, _dtini.Value.Month, _dtini.Value.Day);
                _dtfinal = new DateTime(_dtfim.Value.Year, _dtfim.Value.Month, _dtfim.Value.Day);
            }

            _dtfinal = _dtfinal.AddDays(1);


            var _listatmp = db.QTDE_ACESSOS_POR_CLIENTE(_dtinicial, _dtfinal, _asn_num_assinatura, _pro_id);
            var _asn_id = "";
            var _ListaClienteAcessosDTO = new List<ClienteAcessosDTO>();
            ClienteAcessosDTO _ClienteAcesosDTO = null;

            var _lista = _listatmp.OrderBy(x => x.ASN_NUM_ASSINATURA);

            foreach(var item in _lista){

                if (item.ASN_NUM_ASSINATURA != _asn_id)
                {
                    if (_ClienteAcesosDTO != null)
                    {
                        _ClienteAcesosDTO.QTDE = _ClienteAcesosDTO.ACESSOS.Count();
                        _ListaClienteAcessosDTO.Add(_ClienteAcesosDTO);
                    }

                    _ClienteAcesosDTO = new ClienteAcessosDTO();
                    _ClienteAcesosDTO.ASN_NUM_ASSINATURA = item.ASN_NUM_ASSINATURA;
                    _ClienteAcesosDTO.CLI_NOME = item.CLI_NOME;
                    _ClienteAcesosDTO.QTDE_TOTAL = 0;

                    _asn_id = item.ASN_NUM_ASSINATURA;
                }

                var _AcessosDTO = new AcessosDTO();
                _AcessosDTO.TDC_ID = item.TDC_ID;
                _AcessosDTO.TAB_DESCRICAO = item.TAB_DESCRICAO;
                _AcessosDTO.QTDE = item.QTDE;
                _ClienteAcesosDTO.QTDE_TOTAL += item.QTDE;

                _ClienteAcesosDTO.ACESSOS.Add(_AcessosDTO);

            }

            if (_ClienteAcesosDTO != null)
            {
                _ClienteAcesosDTO.QTDE = _ClienteAcesosDTO.ACESSOS.Count();
                _ListaClienteAcessosDTO.Add(_ClienteAcesosDTO);
            }

            return _ListaClienteAcessosDTO.OrderByDescending(x => x.QTDE).ToList();
        }
        public ASSINATURA RetornarAssinaturaDeCliente(int id)
        {
            var assinatura = (from x in db.ASSINATURA where x.CLI_ID == id select x).FirstOrDefault();
            return assinatura;
        }

        public Pagina<AssinaturaDTO> BuscarClientesPorAssinatura(string asn_id = null, string nome = null, int pagina = 1, int registroPorPagina = 7)
        {
            IQueryable<ASSINATURA> query = db.ASSINATURA.Where(x => x.CLIENTES.CLI_NOME.Contains(nome));

            if (!String.IsNullOrWhiteSpace(asn_id))
            {
                query = query.Where(x => x.ASN_NUM_ASSINATURA == asn_id);
            }

            return ToDTOPage(query, pagina, registroPorPagina);

        }
        public Pagina<AssinaturaDTO> BuscarPorCliente(int _cliid, int pagina = 1, int registroPorPagina = 7)
        {
            IQueryable<ASSINATURA> query = db.ASSINATURA.Where(x => x.CLI_ID == _cliid);
            return ToDTOPage(query, pagina, registroPorPagina);

        }

        public IList<AssinaturaDTO> FindAssinaturaFranquiaPorCliente(int CLI_ID, bool loadTelefones = false, bool loadEmails = false)
        {
            var query = GetDbSet().Where(x => x.CLI_ID == CLI_ID && x.UEN_ID == 1);
            IList<AssinaturaDTO> lstAssinatura = new List<AssinaturaDTO>();

            if (loadTelefones || loadEmails)
            {
                foreach (var ass in query)
                {
                    AssinaturaDTO assinaturaNova = ToDTO(ass);

                    if (loadTelefones)
                    {
                        assinaturaNova.ASSINATURA_TELEFONE = Convert<IEnumerable<ASSINATURA_TELEFONE>, List<AssinaturaTelefoneDTO>>(ass.ASSINATURA_TELEFONE);
                    }

                    if (loadEmails)
                    {
                        assinaturaNova.ASSINATURA_EMAIL = Convert<IEnumerable<ASSINATURA_EMAIL>, List<AssinaturaEmailDTO>>(ass.ASSINATURA_EMAIL);
                    }                    
                    lstAssinatura.Add(assinaturaNova);
                }
                return lstAssinatura;
            }
            else
            {
                return ToDTO(query);
            }
        }

        public IList<AssinaturaDTO> FindAssinaturaPorCliente(int CLI_ID, int? PROD_ID = null, bool loadTelefones = false, bool loadEmails = false)
        {
            var query = GetDbSet().Where(x => x.CLI_ID == CLI_ID && (PROD_ID == null || x.PRO_ID == PROD_ID));
            IList<AssinaturaDTO> lstAssinatura = new List<AssinaturaDTO>();

            if (loadTelefones || loadEmails)
            {
                foreach (var ass in query)
                {
                    AssinaturaDTO assinaturaNova = ToDTO(ass);

                    if (loadTelefones)
                    {
                        assinaturaNova.ASSINATURA_TELEFONE = Convert<IEnumerable<ASSINATURA_TELEFONE>, List<AssinaturaTelefoneDTO>>(ass.ASSINATURA_TELEFONE);
                    }

                    if (loadEmails)
                    {
                        assinaturaNova.ASSINATURA_EMAIL = Convert<IEnumerable<ASSINATURA_EMAIL>, List<AssinaturaEmailDTO>>(ass.ASSINATURA_EMAIL);
                    }
                    lstAssinatura.Add(assinaturaNova);
                }
                return lstAssinatura;
            }
            else
            {
                return ToDTO(query);
            }
        }
         
        public AssinaturaDTO BuscarAssinaturaPorCLIID(int? _cliid)
        {
            ASSINATURA query = db.ASSINATURA.Where(x => x.CLI_ID == _cliid).FirstOrDefault();

            return ToDTO(query);

        }
        public AssinaturaDTO BuscarAssinaturaPorContrato(string _numcontrato)
        {
            var query = (from p in db.PARCELAS
                         join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                         join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                         where (p.CTR_NUM_CONTRATO == _numcontrato)
                         select a).FirstOrDefault();


            return ToDTO(query);
        }

        public LoginUnicoAssinaturaDTO BuscarResumoAssinatura(string assinatura)
        {
            var dataAtual = DateTime.Now;

            var query = (from ass in db.ASSINATURA
                         where ass.ASN_NUM_ASSINATURA == assinatura
                         select new LoginUnicoAssinaturaDTO()
                         {
                             CodAssinatura = ass.ASN_NUM_ASSINATURA,
                             ProId = ass.PRO_ID,
                             ProNome = ass.PRODUTOS.PRO_NOME,
                             SenhaAssinatura = (
                                from assSenha in db.ASSINATURA_SENHA 
                                    where assSenha.ASN_NUM_ASSINATURA == ass.ASN_NUM_ASSINATURA
                                    && assSenha.ASN_ATIVO == true
                                    select assSenha.ASN_SENHA).FirstOrDefault(),
                             AssinaturaNativaCliente = false,
                             DataVigencia =
                                (from con in db.CONTRATOS
                                 where
                                 con.ASN_NUM_ASSINATURA == ass.ASN_NUM_ASSINATURA
                                 orderby con.CTR_DATA_INI_VIGENCIA descending
                                 select con.CTR_DATA_FIM_VIGENCIA)
                                     .FirstOrDefault()
                         }).FirstOrDefault();

            return query;
        }

        public IList<LoginUnicoAssinaturaDTO> BuscarResumosDeAssinaturasDoCliente(int? cliId, string excetoAssinatura = null, bool marcarAssinaturasComoNativa = false)
        {
            var dataAtual = DateTime.Now;

            var query = (from ass in db.ASSINATURA
                         where
                         ass.CLI_ID == cliId &&
                         (excetoAssinatura == null || ass.ASN_NUM_ASSINATURA != excetoAssinatura)
                         select new LoginUnicoAssinaturaDTO()
                         {
                             CodAssinatura = ass.ASN_NUM_ASSINATURA,
                             ProId = ass.PRO_ID,
                             ProNome = ass.PRODUTOS.PRO_NOME,
                             SenhaAssinatura = (
                                from assSenha in db.ASSINATURA_SENHA 
                                    where assSenha.ASN_NUM_ASSINATURA == ass.ASN_NUM_ASSINATURA
                                    && assSenha.ASN_ATIVO == true
                                    select assSenha.ASN_SENHA).FirstOrDefault(),
                             AssinaturaNativaCliente = marcarAssinaturasComoNativa,
                             DataVigencia =
                                (from con in db.CONTRATOS
                                 where
                                 con.ASN_NUM_ASSINATURA == ass.ASN_NUM_ASSINATURA
                                 orderby con.CTR_DATA_INI_VIGENCIA descending
                                 select con.CTR_DATA_FIM_VIGENCIA)
                                     .FirstOrDefault()
                         }).ToList();
            return query;
        }

        public IList<AssinaturaDTO> FindByIdLst(IList<string> lstCodAssinatura = null)
        {
            if (lstCodAssinatura != null)
            {
                var query = (from assi in db.ASSINATURA
                             where
                             lstCodAssinatura.Contains(assi.ASN_NUM_ASSINATURA)
                             select assi);
                return ToDTO(query);
            }

            return new List<AssinaturaDTO>();
        }

        public int? RetornarIdClienteDaAssinatura(string codAssinatura)
        {
            var query = (from assi in db.ASSINATURA
                         where assi.ASN_NUM_ASSINATURA == codAssinatura
                         select assi.CLI_ID).FirstOrDefault();

            return query;
        }

        public AssinaturaDTO RetornarAssinaturaDoPedido(int? ipeId)
        {
            var query = (from itm in
                             db.ITEM_PEDIDO
                         where itm.IPE_ID == ipeId
                         select itm.ASSINATURA)
                         .FirstOrDefault();
            return ToDTO(query);
        }
        public Pagina<AssinaturaDTO> ListarMateriaAdicional(string _asn_num_assinatura, int pagina = 1, int registroPorPagina = 7)
        {

            var query = (from a in db.ASSINATURA
                        where (_asn_num_assinatura == null || (_asn_num_assinatura != null && a.ASN_NUM_ASSINATURA == _asn_num_assinatura))
                           && (a.ASN_MATERIA_ADICIONAL != null && a.ASN_MATERIA_ADICIONAL != "")
                       select a);

            return ToDTOPage(query, pagina, registroPorPagina);
            
        }

    }
}
