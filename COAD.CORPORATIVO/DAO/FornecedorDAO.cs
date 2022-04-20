
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model;
using COAD.SEGURANCA.Repositorios.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;

namespace COAD.CORPORATIVO.DAO
{
    public class FornecedorDAO : DAOAdapter<FORNECEDOR, FornecedorDTO>
    {
public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public FornecedorDAO()
        {
             this.db = GetDb<COADCORPEntities>();
        }
        public FornecedorDTO FindByIDFull(int _for_id)
        {
            var _for = (from f in db.FORNECEDOR
                        where f.FOR_ID == _for_id
                        select f).FirstOrDefault();

            var _dto = ToDTO(_for);

            _dto.MUNICIPIO = Convert<MUNICIPIO, MunicipioDTO>(_for.MUNICIPIO);

            return _dto;
        }
        public FornecedorDTO BuscarPorCNPJ(string _for_cnpj)
        {
            var _for =  (from f in db.FORNECEDOR
                        where f.FOR_CNPJ == _for_cnpj
                       select f).FirstOrDefault();
                         
            return ToDTO(_for);
        }
        public IList<FornecedorDTO> BuscarPorTipo(int _tipo_for_id)
        {
            var _for = (from f in db.FORNECEDOR
                        where f.TIPO_FOR_ID == _tipo_for_id
                        select f);
   

            return ToDTO(_for);
        }
        public FornecedorDTO VerficarIncluir(FornecedorDTO _forn)
        {
            var f = this.BuscarPorCNPJ(_forn.FOR_CNPJ);

            if (f == null || f.FOR_ID == 0)
            {
                _forn.FOR_NOME_FANTASIA = _forn.FOR_RAZAO_SOCIAL;
                _forn.FOR_TIPESSOA = "J";
                _forn.DATA_CADASTRO = DateTime.Now;
                _forn.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                _forn.TIPO_FOR_ID = 3;
                _forn.FOR_COD_PAIS = "1058";

                this.Incluir(ToModel(_forn));

                _forn = this.BuscarPorCNPJ(_forn.FOR_CNPJ);
            }
            else
            {
                _forn.FOR_ID = f.FOR_ID;
                _forn.FOR_RAZAO_SOCIAL = f.FOR_RAZAO_SOCIAL;
                _forn.FOR_NOME_FANTASIA = f.FOR_NOME_FANTASIA;
                _forn.FOR_TIPESSOA = f.FOR_TIPESSOA;
                _forn.FOR_CNPJ = f.FOR_CNPJ;
                _forn.FOR_ENDERECO = f.FOR_ENDERECO;
                _forn.FOR_END_COMPLEMENTO = f.FOR_END_COMPLEMENTO;
                _forn.FOR_BAIRRO = f.FOR_BAIRRO;
                _forn.MUN_ID = f.MUN_ID;
                _forn.FOR_CEP = f.FOR_CEP;
                _forn.FOR_TEL = f.FOR_TEL;
                _forn.FOR_CEL = f.FOR_CEL;
                _forn.FOR_FAX = f.FOR_FAX;
                _forn.FOR_INSCRICAO = f.FOR_INSCRICAO;
                _forn.FOR_CONTATO = f.FOR_CONTATO;
                _forn.FOR_DTMOV = f.FOR_DTMOV;
                _forn.FOR_DTCAD = f.FOR_DTCAD;
                _forn.FOR_DTNASC = f.FOR_DTNASC;
                _forn.FOR_ATIVO = f.FOR_ATIVO;
                _forn.FOR_EMAIL = f.FOR_EMAIL;
                _forn.FOR_INSCMUNIP = f.FOR_INSCMUNIP;
                _forn.FOR_INSCSUFRAMA = f.FOR_INSCSUFRAMA;
                _forn.DATA_CADASTRO = f.DATA_CADASTRO;
                _forn.DATA_ALTERA = f.DATA_ALTERA;
                _forn.USU_LOGIN = f.USU_LOGIN;
                _forn.TIPO_FOR_ID = f.TIPO_FOR_ID;
                _forn.FOR_END_NUMERO = f.FOR_END_NUMERO;
            }

            return _forn;
        }
        public FORNECEDOR Verficar(FORNECEDOR _forn)
        {
            var f = this.BuscarPorCNPJ(_forn.FOR_CNPJ);

            if (f == null || f.FOR_ID == 0)
            {
                _forn.FOR_NOME_FANTASIA = _forn.FOR_RAZAO_SOCIAL;
                _forn.FOR_TIPESSOA = "J";
                _forn.DATA_CADASTRO = DateTime.Now;

            }
            else
            {
                _forn.FOR_ID = f.FOR_ID;
                _forn.FOR_RAZAO_SOCIAL = f.FOR_RAZAO_SOCIAL;
                _forn.FOR_NOME_FANTASIA = f.FOR_NOME_FANTASIA;
                _forn.FOR_TIPESSOA = f.FOR_TIPESSOA;
                _forn.FOR_CNPJ = f.FOR_CNPJ;
                _forn.FOR_ENDERECO = f.FOR_ENDERECO;
                _forn.FOR_END_COMPLEMENTO = f.FOR_END_COMPLEMENTO;
                _forn.FOR_BAIRRO = f.FOR_BAIRRO;
                _forn.MUN_ID = f.MUN_ID;
                _forn.FOR_CEP = f.FOR_CEP;
                _forn.FOR_TEL = f.FOR_TEL;
                _forn.FOR_CEL = f.FOR_CEL;
                _forn.FOR_FAX = f.FOR_FAX;
                _forn.FOR_INSCRICAO = f.FOR_INSCRICAO;
                _forn.FOR_CONTATO = f.FOR_CONTATO;
                _forn.FOR_DTMOV = f.FOR_DTMOV;
                _forn.FOR_DTCAD = f.FOR_DTCAD;
                _forn.FOR_DTNASC = f.FOR_DTNASC;
                _forn.FOR_ATIVO = f.FOR_ATIVO;
                _forn.FOR_EMAIL = f.FOR_EMAIL;
                _forn.FOR_INSCMUNIP = f.FOR_INSCMUNIP;
                _forn.FOR_INSCSUFRAMA = f.FOR_INSCSUFRAMA;
                _forn.DATA_CADASTRO = f.DATA_CADASTRO;
                _forn.DATA_ALTERA = f.DATA_ALTERA;
                _forn.USU_LOGIN = f.USU_LOGIN;
                _forn.TIPO_FOR_ID = f.TIPO_FOR_ID;
                _forn.FOR_END_NUMERO = f.FOR_END_NUMERO;
            }

            return _forn;
        }
        public Pagina<FornecedorDTO> BuscarPorRazaoSocial(string _razaosocial, int numpagina = 1, int linhas = 10)
        {
            var _lista = (from f in db.FORNECEDOR
                          where f.FOR_RAZAO_SOCIAL.Contains(_razaosocial)
                          orderby f.FOR_RAZAO_SOCIAL descending
                          select f);



            return ToDTOPage(_lista, numpagina, linhas);
        }
    }
}
