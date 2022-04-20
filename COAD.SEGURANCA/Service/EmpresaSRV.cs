using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.DAO;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using Coad.GenericCrud.Service.Base;
using System.Web.Mvc;
using GenericCrud.Config.DataAttributes;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.SEGURANCA.Model.FiltersInfo;

namespace COAD.SEGURANCA.Service
{
    [ServiceConfig("EMP_ID")]
    public class EmpresaSRV : GenericService<EMPRESA, EmpresaModel, int>
    {

        private EmpresaDAO _dao = new EmpresaDAO();

        public EmpresaSRV()
        {
            this.Dao = _dao;
        }

        public IList<SelectListItem> RetornarEmpresas()
        {
            IList<EmpresaModel> listaDeEmpresas = this.FindAll();

            var empresas = new List<SelectListItem>();
            empresas.Add(new SelectListItem() { Value = "", Text = "Selecione" });

            foreach (EmpresaModel tipo in listaDeEmpresas)
            {
                empresas.Add(new SelectListItem() { Value = tipo.EMP_ID.ToString(), Text = tipo.EMP_NOME_FANTASIA });
            }

            return empresas;
        }

        public EmpresaModel Buscar(int _emp_id)
        {
            return new EmpresaDAO().Buscar(_emp_id);
        }
        public List<EmpresaModel> Listar(string _razao_social)
        {
            return new EmpresaDAO().Listar(_razao_social);
        }
        public List<EmpresaModel> Listar()
        {
            var listaDeEmpresas = this.FindAll().ToList();

            return listaDeEmpresas;
        }
        public Pagina<EmpresaModel> BuscarEmpresas(EmpresaFiltrosDTO filtros)
        { 
            return _dao.BuscarEmpresas(filtros);
        }

        public void IncluirReg(EmpresaModel _empresa)
        {
            EMPRESA emp = new EMPRESA();
            emp.EMP_ID =  _empresa.EMP_ID; 
            emp.EMP_RAZAO_SOCIAL = _empresa.EMP_RAZAO_SOCIAL;
            emp.EMP_NOME_FANTASIA = _empresa.EMP_NOME_FANTASIA; 
            emp.EMP_CNPJ = _empresa.EMP_CNPJ; 
            emp.EMP_IE = _empresa.EMP_IE;
            emp.EMP_IM = _empresa.EMP_IM;
            emp.EMP_SUFRAMA = _empresa.EMP_SUFRAMA;
            emp.EMP_LOGRADOURO = _empresa.EMP_LOGRADOURO;
            emp.EMP_NUMERO = _empresa.EMP_NUMERO;
            emp.EMP_COMPLEMENTO = _empresa.EMP_COMPLEMENTO;
            emp.EMP_BAIRRO = _empresa.EMP_BAIRRO;
            emp.CID_ID = _empresa.CID_ID ;
            emp.EMP_CEP = _empresa.EMP_CEP; 
            emp.EMP_TEL1 = _empresa.EMP_TEL1;
            emp.EMP_TEL2 = _empresa.EMP_TEL2;
            emp.EMP_TEL3 = _empresa.EMP_TEL3;
            emp.EMP_EMAIL = _empresa.EMP_EMAIL; 
            emp.EMP_SITE = _empresa.EMP_SITE; 
            emp.EMP_ULTIMA_NFE = _empresa.EMP_ULTIMA_NFE;
            emp.EMP_CNR_AGCEDENTE = _empresa.EMP_CNR_AGCEDENTE;
            emp.EMP_AREA = _empresa.EMP_AREA;
            emp.EMP_TIPO = _empresa.EMP_TIPO;

            new EmpresaDAO().Incluir(emp);
        }
        public void SalvarReg(EmpresaModel _empresa)
        {
            EMPRESA emp = new EMPRESA();
            emp.EMP_ID = _empresa.EMP_ID;
            emp.EMP_RAZAO_SOCIAL = _empresa.EMP_RAZAO_SOCIAL;
            emp.EMP_NOME_FANTASIA = _empresa.EMP_NOME_FANTASIA;
            emp.EMP_CNPJ = _empresa.EMP_CNPJ;
            emp.EMP_IE = _empresa.EMP_IE;
            emp.EMP_IM = _empresa.EMP_IM;
            emp.EMP_SUFRAMA = _empresa.EMP_SUFRAMA;
            emp.EMP_LOGRADOURO = _empresa.EMP_LOGRADOURO;
            emp.EMP_NUMERO = _empresa.EMP_NUMERO;
            emp.EMP_COMPLEMENTO = _empresa.EMP_COMPLEMENTO;
            emp.EMP_BAIRRO = _empresa.EMP_BAIRRO;
            emp.CID_ID = _empresa.CID_ID;
            emp.EMP_CEP = _empresa.EMP_CEP;
            emp.EMP_TEL1 = _empresa.EMP_TEL1;
            emp.EMP_TEL2 = _empresa.EMP_TEL2;
            emp.EMP_TEL3 = _empresa.EMP_TEL3;
            emp.EMP_EMAIL = _empresa.EMP_EMAIL;
            emp.EMP_SITE = _empresa.EMP_SITE;
            emp.EMP_ULTIMA_NFE = _empresa.EMP_ULTIMA_NFE;
            emp.EMP_CNR_AGCEDENTE = _empresa.EMP_CNR_AGCEDENTE;
            emp.EMP_AREA = _empresa.EMP_AREA;
            emp.EMP_TIPO = _empresa.EMP_TIPO;

            new EmpresaDAO().Salvar(emp);
        }
        public void ExcluirReg(EmpresaModel _empresa)
        {
            EMPRESA emp = new EMPRESA();
            emp.EMP_ID = _empresa.EMP_ID;
            emp.EMP_RAZAO_SOCIAL = _empresa.EMP_RAZAO_SOCIAL;
            emp.EMP_NOME_FANTASIA = _empresa.EMP_NOME_FANTASIA;
            emp.EMP_CNPJ = _empresa.EMP_CNPJ;
            emp.EMP_IE = _empresa.EMP_IE;
            emp.EMP_IM = _empresa.EMP_IM;
            emp.EMP_SUFRAMA = _empresa.EMP_SUFRAMA;
            emp.EMP_LOGRADOURO = _empresa.EMP_LOGRADOURO;
            emp.EMP_NUMERO = _empresa.EMP_NUMERO;
            emp.EMP_COMPLEMENTO = _empresa.EMP_COMPLEMENTO;
            emp.EMP_BAIRRO = _empresa.EMP_BAIRRO;
            emp.CID_ID = _empresa.CID_ID;
            emp.EMP_CEP = _empresa.EMP_CEP;
            emp.EMP_TEL1 = _empresa.EMP_TEL1;
            emp.EMP_TEL2 = _empresa.EMP_TEL2;
            emp.EMP_TEL3 = _empresa.EMP_TEL3;
            emp.EMP_EMAIL = _empresa.EMP_EMAIL;
            emp.EMP_SITE = _empresa.EMP_SITE;
            emp.EMP_ULTIMA_NFE = _empresa.EMP_ULTIMA_NFE;
            emp.EMP_CNR_AGCEDENTE = _empresa.EMP_CNR_AGCEDENTE;
            emp.EMP_AREA = _empresa.EMP_AREA;
            emp.EMP_TIPO = _empresa.EMP_TIPO;

            new EmpresaDAO().Excluir(emp);
        }
        public EmpresaModel BuscarPorCNPJ(string _cnpj)
        {
            return new EmpresaDAO().BuscarPorCNPJ(_cnpj);
        }

        public IList<EmpresaModel> ListarEmpresasParaPrePedido()
        {
            return _dao.ListarEmpresasParaPrePedido();
        }

        /// <summary>
        /// Retorna os ids das empresas pesquisadas por seu nome
        /// </summary>
        /// <param name="empRazaoSocial"></param>
        /// <returns></returns>
        public IList<int> ListEmpIds(string empRazaoSocial)
        {
            return _dao.ListEmpIds(empRazaoSocial);
        }

        public Pagina<EmpresaModel> ListarEmpresa(string razaoSocial = null, string nomeFantasia = null, int pagina = 1, int registrosPorPagina = 15)
        {
            return _dao.ListarEmpresa(razaoSocial, nomeFantasia, pagina, registrosPorPagina);
        }

    }
}
