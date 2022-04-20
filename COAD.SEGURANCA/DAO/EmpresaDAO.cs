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
    public class EmpresaDAO : DAOAdapter<EMPRESA, EmpresaModel, int>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }

        public EmpresaDAO()
        {
            db = GetDb<COADSYSEntities>();
        }
        public Pagina<EmpresaModel> BuscarEmpresas(EmpresaFiltrosDTO filtros)
        {
            string queryStr = filtros.query;

            var query = (from r in db.EMPRESA
                        where (queryStr == null ||
                                   (r.EMP_RAZAO_SOCIAL.Contains(queryStr) ||
                                    r.EMP_NOME_FANTASIA.Contains(queryStr)))
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


        public EmpresaModel Buscar(int _emp_id)
        {
            EmpresaModel _empresa = (from e in db.EMPRESA
                                     where e.EMP_ID == _emp_id
                                     select new EmpresaModel()
                                     {
                                         EMP_ID = e.EMP_ID,
                                         EMP_RAZAO_SOCIAL = e.EMP_RAZAO_SOCIAL,
                                         EMP_NOME_FANTASIA = e.EMP_NOME_FANTASIA,
                                         EMP_CNPJ = e.EMP_CNPJ,
                                         EMP_IE = e.EMP_IE,
                                         EMP_IM = e.EMP_IM,
                                         EMP_SUFRAMA = e.EMP_SUFRAMA,
                                         EMP_LOGRADOURO = e.EMP_LOGRADOURO,
                                         EMP_NUMERO = e.EMP_NUMERO,
                                         EMP_COMPLEMENTO = e.EMP_COMPLEMENTO,
                                         EMP_BAIRRO = e.EMP_BAIRRO,
                                         CID_ID = e.CID_ID,
                                         EMP_CEP = e.EMP_CEP,
                                         EMP_TEL1 = e.EMP_TEL1,
                                         EMP_TEL2 = e.EMP_TEL2,
                                         EMP_TEL3 = e.EMP_TEL3,
                                         EMP_EMAIL = e.EMP_EMAIL,
                                         EMP_SITE = e.EMP_SITE,
                                         EMP_ULTIMA_NFE = e.EMP_ULTIMA_NFE,
                                         EMP_CNR_AGCEDENTE = e.EMP_CNR_AGCEDENTE,
                                         EMP_AREA = e.EMP_AREA,
                                         EMP_TIPO = e.EMP_TIPO
                                     }).First();

            return _empresa;
        }
        public EMPRESA BuscarPorId(int _emp_id)
        {

            EMPRESA _empresa = (from e in db.EMPRESA
                                     where e.EMP_ID == _emp_id
                                     select e).First();

            return _empresa;
        }
        public List<EmpresaModel> Listar(string _razao_social)
        {
            if (_razao_social == null)
                _razao_social = "";

            List<EmpresaModel> _empresa = (from e in db.EMPRESA
                                           where e.EMP_RAZAO_SOCIAL.StartsWith(_razao_social)
                                           select new EmpresaModel()
                                           {
                                               EMP_ID = e.EMP_ID,
                                               EMP_RAZAO_SOCIAL = e.EMP_RAZAO_SOCIAL,
                                               EMP_NOME_FANTASIA = e.EMP_NOME_FANTASIA,
                                               EMP_CNPJ = e.EMP_CNPJ,
                                               EMP_IE = e.EMP_IE,
                                               EMP_IM = e.EMP_IM,
                                               EMP_SUFRAMA = e.EMP_SUFRAMA,
                                               EMP_LOGRADOURO = e.EMP_LOGRADOURO,
                                               EMP_NUMERO = e.EMP_NUMERO,
                                               EMP_COMPLEMENTO = e.EMP_COMPLEMENTO,
                                               EMP_BAIRRO = e.EMP_BAIRRO,
                                               CID_ID = e.CID_ID,
                                               EMP_CEP = e.EMP_CEP,
                                               EMP_TEL1 = e.EMP_TEL1,
                                               EMP_TEL2 = e.EMP_TEL2,
                                               EMP_TEL3 = e.EMP_TEL3,
                                               EMP_EMAIL = e.EMP_EMAIL,
                                               EMP_SITE = e.EMP_SITE,
                                               EMP_ULTIMA_NFE = e.EMP_ULTIMA_NFE,
                                               EMP_CNR_AGCEDENTE = e.EMP_CNR_AGCEDENTE,
                                               EMP_AREA = e.EMP_AREA,
                                               EMP_TIPO = e.EMP_TIPO
                                           }).ToList();

            return _empresa;
        }
        public EmpresaModel BuscarPorCNPJ(string _cnpj)
        {
            EmpresaModel _empresa = (from e in db.EMPRESA
                                     where e.EMP_CNPJ == _cnpj
                                     select new EmpresaModel()
                                     {
                                         EMP_ID = e.EMP_ID,
                                         EMP_RAZAO_SOCIAL = e.EMP_RAZAO_SOCIAL,
                                         EMP_NOME_FANTASIA = e.EMP_NOME_FANTASIA,
                                         EMP_CNPJ = e.EMP_CNPJ,
                                         EMP_IE = e.EMP_IE,
                                         EMP_IM = e.EMP_IM,
                                         EMP_SUFRAMA = e.EMP_SUFRAMA,
                                         EMP_LOGRADOURO = e.EMP_LOGRADOURO,
                                         EMP_NUMERO = e.EMP_NUMERO,
                                         EMP_COMPLEMENTO = e.EMP_COMPLEMENTO,
                                         EMP_BAIRRO = e.EMP_BAIRRO,
                                         CID_ID = e.CID_ID,
                                         EMP_CEP = e.EMP_CEP,
                                         EMP_TEL1 = e.EMP_TEL1,
                                         EMP_TEL2 = e.EMP_TEL2,
                                         EMP_TEL3 = e.EMP_TEL3,
                                         EMP_EMAIL = e.EMP_EMAIL,
                                         EMP_SITE = e.EMP_SITE,
                                         EMP_ULTIMA_NFE = e.EMP_ULTIMA_NFE,
                                         EMP_CNR_AGCEDENTE = e.EMP_CNR_AGCEDENTE,
                                         EMP_AREA = e.EMP_AREA,
                                         EMP_TIPO = e.EMP_TIPO
                                     }).FirstOrDefault();

            return _empresa;

        }

        public IList<EmpresaModel> ListarEmpresasParaPrePedido()
        {
            var query = db.EMPRESA.Where(op => op.EMP_APARECE_PRE_PEDIDO);
            return ToDTO(query);
        }

        public IList<int> ListEmpIds(string empRazaoSocial)
        {
            var listIds =
               GetDbSet().
                Where(x => x.EMP_RAZAO_SOCIAL.Contains(empRazaoSocial))
                .Select(sel => sel.EMP_ID)
                .ToList();

            return listIds;
        }

        public Pagina<EmpresaModel> ListarEmpresa(string razaoSocial = null, string nomeFantasia = null, int pagina = 1, int registrosPorPagina = 15)
        {
            IQueryable<EMPRESA> query = GetDbSet();

            if (!string.IsNullOrWhiteSpace(razaoSocial))
            {
                query = query.Where(x => x.EMP_RAZAO_SOCIAL.Contains(razaoSocial));
            }

            if (!string.IsNullOrWhiteSpace(nomeFantasia))
            {
                query = query.Where(x => x.EMP_NOME_FANTASIA.Contains(nomeFantasia));
            }

            return ToDTOPage(query, pagina, registrosPorPagina);
        }
    }
}
