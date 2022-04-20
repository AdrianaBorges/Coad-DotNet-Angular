using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.SEGURANCA.Model.FiltersInfo;
using GenericCrud.Config.DataAttributes;

namespace COAD.SEGURANCA.DAO
{
    [DAOConfig("coadsys")]
    public class ContaDAO : DAOAdapter<CONTA, ContaDTO, int>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }

        public ContaDAO()
        {
            db = GetDb<COADSYSEntities>();
        }

        public List<int> ObterContas(List<int> ctaId, bool enviaBoleto)
        {
            var query = (from ct in db.CONTA
                         where ctaId.Contains(ct.CTA_ID) &&
                               ct.CTA_ENVIA_BOLETO == enviaBoleto
                         select ct.CTA_ID);

            return query.ToList();
        }

        public IList<ContaDTO> Listar(string banco_id)
        {
            IQueryable<CONTA> query = GetDbSet();

            if (!String.IsNullOrWhiteSpace(banco_id))
                query = query.Where(x => x.BAN_ID == banco_id );

            return ToDTO(query);
        }
        public IList<ContaDTO> ListarContasBanco(string bco, int empid)
        {
            var query = (from r in db.CONTA
                        where r.BAN_ID == bco
                           && r.EMP_ID == empid
                           && r.CTA_CEDENTE_EMITE_BOLETO == true
                       select r);

            return ToDTO(query);
        }
        public ContaDTO ListarEnviaBoletoAvulso(int empid, string banco_id)
        {
            IQueryable<CONTA> query = GetDbSet();

            var _retorno = query.Where(x => x.EMP_ID == empid &&
                                            x.BAN_ID == banco_id &&
                                            x.CTA_CARTEIRA_BOLETO == "04").FirstOrDefault();
            

            return ToDTO(_retorno);
        }
        public IList<ContaDTO> Listar(string banco_id, bool cta_emite_boleto)
        {
            IQueryable<CONTA> query = GetDbSet();
     
            if (!String.IsNullOrWhiteSpace(banco_id))
                query = query.Where(x => x.BAN_ID == banco_id &&
                                         x.CTA_CEDENTE_EMITE_BOLETO == cta_emite_boleto);
            else
                query = query.Where(x => x.CTA_CEDENTE_EMITE_BOLETO == cta_emite_boleto);


            return ToDTO(query);
        }
        public IList<ContaDTO> ListarPorEmpresa(int? empresa_id = null)
        {
            IQueryable<CONTA> query = GetDbSet();

            if (empresa_id != null)
                query = query.Where(x => x.EMP_ID == empresa_id);

            return ToDTO(query);
        }
        public Pagina<ContaDTO> BuscarContas(ContaFiltrosDTO filtros)
        {
            string queryStr = filtros.query;

            var query = (from r in db.CONTA
                         where (queryStr == null ||
                                    (r.CTA_AGENCIA.Contains(queryStr) ||
                                     r.CTA_CONTA.Contains(queryStr)))
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


        public IList<ContaDTO> Listar(int? empresa_id = null, string banco_id = null, string agencia = null, string tipo = null)
        {
            var query = (from c in db.CONTA
                         where (empresa_id == null || (empresa_id != null && c.EMP_ID == empresa_id))
                            && (banco_id == null || (banco_id != null && c.BAN_ID == banco_id))
                            && (agencia == null || (agencia != null && c.CTA_AGENCIA == agencia))
                            && (tipo == null || (tipo != null && c.CTA_TIPO == tipo))
                        select c);

            return ToDTO(query);
        }

    
        public ContaDTO ListarContaDePrimeiraParcela(int? empId)
        {
            var _contaativa = (from p in db.PARAMETROS
                               where p.PAR_KEY == "CTAPRIMEIRAPARCELA"
                               select p).FirstOrDefault();
            var _cta_id = 0;

            if (_contaativa != null)
                _cta_id = System.Convert.ToInt32(_contaativa.PAR_VALOR);

            var query = (from ct in db.CONTA 
                         where ct.CTA_ID == _cta_id
                         select ct).FirstOrDefault();

            return ToDTO(query);
        }
        public ContaDTO BuscarContaBoletoAvuso()
        {
            var _contaativa = (from p in db.PARAMETROS
                               where p.PAR_KEY == "CTABOLETOAVULSO"
                               select p).FirstOrDefault();
            var _cta_id = 0;

            if (_contaativa != null)
                _cta_id = System.Convert.ToInt32(_contaativa.PAR_VALOR);

            var query = (from ct in db.CONTA
                         where ct.CTA_ID == _cta_id
                         select ct).FirstOrDefault();

            return ToDTO(query);
        }

        public IList<ContaDTO> ListarContasEnviaBoleto()
        {
            var query = (from 
                            c in db.CONTA
                         where 
                            c.CTA_ENVIA_BOLETO == true
                         select c);

            return ToDTO(query);
        }

        public List<int> ListarIDContasEnviaBoleto()
        {
            var query = (from
                            c in db.CONTA
                         where
                            c.CTA_ENVIA_BOLETO == true
                         select c.CTA_ID);

            return query.ToList();
        }

        public ContaDTO ListarParaRemessa(int empid, string banco_id, bool cta_cedente_emite_boleto)
        {
            IQueryable<CONTA> query = GetDbSet();

            var _retorno = query.Where(x => x.EMP_ID == empid &&
                                            x.BAN_ID == banco_id &&
                                            x.CTA_CEDENTE_EMITE_BOLETO == cta_cedente_emite_boleto).FirstOrDefault();


            return ToDTO(_retorno);
        }

        public IList<ContaDTO> ListarPorEmpresa(int emp_id)
        {
            IQueryable<CONTA> query = GetDbSet();
    
            query = query.Where(x => x.EMP_ID == emp_id);

            return ToDTO(query);
        }

        public ContaDTO RetornarContaPorAgenciaEConta(int CTA_AGENCIA, int CTA_CONTA)
        {

            string agencia = CTA_AGENCIA.ToString();
            string conta = CTA_CONTA.ToString();

            var query = (from r in db.CONTA
                         where r.CTA_AGENCIA == agencia
                            && r.CTA_CONTA == conta
                            && r.CTA_CEDENTE_EMITE_BOLETO == true
                         select r);

            return ToDTO(query.FirstOrDefault());

        }

    }
}
