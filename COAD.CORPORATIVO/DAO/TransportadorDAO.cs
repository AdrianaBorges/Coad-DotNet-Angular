using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;

namespace COAD.CORPORATIVO.DAO
{
    public class TransportadorDAO : DAOAdapter<TRANSPORTADOR, TransportadorDTO>
    {
public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public TransportadorDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }
        public TransportadorDTO FindByIDFull(int _tra_id)
        {
            var _tra = (from f in db.TRANSPORTADOR
                        where f.TRA_ID == _tra_id
                        select f).FirstOrDefault();

            var _dto = ToDTO(_tra);

            _dto.MUNICIPIO = Convert<MUNICIPIO, MunicipioDTO>(_tra.MUNICIPIO);

            return _dto;
        }
        public TransportadorDTO BuscarPorCNPJ(string _tra_cnpj)
        {
            var _tra = (from t in db.TRANSPORTADOR
                       where t.TRA_CNPJ == _tra_cnpj
                      select t).FirstOrDefault();

            return ToDTO(_tra);

        }
        public TransportadorDTO VerficarIncluir(TransportadorDTO _tran)
        {
            var t = this.BuscarPorCNPJ(_tran.TRA_CNPJ);

            if (t == null || t.TRA_ID == 0)
            {
                _tran.TRA_NOME_FANTASIA = _tran.TRA_RAZAO_SOCIAL;
                _tran.TRA_TIPESSOA = "J";
                _tran.DATA_CADASTRO = DateTime.Now;
                _tran.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                this.Incluir(ToModel(_tran));

                _tran = this.BuscarPorCNPJ(_tran.TRA_CNPJ);
            }
            else
            {
                _tran.TRA_ID = t.TRA_ID;
                _tran.TRA_RAZAO_SOCIAL = t.TRA_RAZAO_SOCIAL;
                _tran.TRA_NOME_FANTASIA = t.TRA_NOME_FANTASIA;
                _tran.TRA_TIPESSOA = t.TRA_TIPESSOA;
                _tran.TRA_CNPJ = t.TRA_CNPJ;
                _tran.TRA_ENDERECO = t.TRA_ENDERECO;
                _tran.TRA_END_COMPLEMENTO = t.TRA_END_COMPLEMENTO;
                _tran.TRA_BAIRRO = t.TRA_BAIRRO;
                _tran.MUN_ID = t.MUN_ID;
                _tran.TRA_CEP = t.TRA_CEP;
                _tran.TRA_TEL = t.TRA_TEL;
                _tran.TRA_CEL = t.TRA_CEL;
                _tran.TRA_FAX = t.TRA_FAX;
                _tran.TRA_INSCRICAO = t.TRA_INSCRICAO;
                _tran.TRA_CONTATO = t.TRA_CONTATO;
                _tran.TRA_DTMOV = t.TRA_DTMOV;
                _tran.TRA_DTCAD = t.TRA_DTCAD;
                _tran.TRA_DTNASC = t.TRA_DTNASC;
                _tran.TRA_ATIVO = t.TRA_ATIVO;
                _tran.TRA_EMAIL = t.TRA_EMAIL;
                _tran.TRA_INSCMUNIP = t.TRA_INSCMUNIP;
                _tran.TRA_INSCSUFRAMA = t.TRA_INSCSUFRAMA;
                _tran.DATA_CADASTRO = t.DATA_CADASTRO;
                _tran.DATA_ALTERA = t.DATA_ALTERA;
                _tran.USU_LOGIN = t.USU_LOGIN;
                _tran.TRA_END_NUMERO = t.TRA_END_NUMERO;
            }

            return _tran;
        }
        public TRANSPORTADOR Verficar(TRANSPORTADOR _tran)
        {
            var t = this.BuscarPorCNPJ(_tran.TRA_CNPJ);

            if (t == null || t.TRA_ID == 0)
            {
                _tran.TRA_NOME_FANTASIA = _tran.TRA_RAZAO_SOCIAL;
                _tran.TRA_TIPESSOA = "J";
                _tran.DATA_CADASTRO = DateTime.Now;
            }
            else
            {
                _tran.TRA_ID = t.TRA_ID;
                _tran.TRA_RAZAO_SOCIAL = t.TRA_RAZAO_SOCIAL;
                _tran.TRA_NOME_FANTASIA = t.TRA_NOME_FANTASIA;
                _tran.TRA_TIPESSOA = t.TRA_TIPESSOA;
                _tran.TRA_CNPJ = t.TRA_CNPJ;
                _tran.TRA_ENDERECO = t.TRA_ENDERECO;
                _tran.TRA_END_COMPLEMENTO = t.TRA_END_COMPLEMENTO;
                _tran.TRA_BAIRRO = t.TRA_BAIRRO;
                _tran.MUN_ID = t.MUN_ID;
                _tran.TRA_CEP = t.TRA_CEP;
                _tran.TRA_TEL = t.TRA_TEL;
                _tran.TRA_CEL = t.TRA_CEL;
                _tran.TRA_FAX = t.TRA_FAX;
                _tran.TRA_INSCRICAO = t.TRA_INSCRICAO;
                _tran.TRA_CONTATO = t.TRA_CONTATO;
                _tran.TRA_DTMOV = t.TRA_DTMOV;
                _tran.TRA_DTCAD = t.TRA_DTCAD;
                _tran.TRA_DTNASC = t.TRA_DTNASC;
                _tran.TRA_ATIVO = t.TRA_ATIVO;
                _tran.TRA_EMAIL = t.TRA_EMAIL;
                _tran.TRA_INSCMUNIP = t.TRA_INSCMUNIP;
                _tran.TRA_INSCSUFRAMA = t.TRA_INSCSUFRAMA;
                _tran.DATA_CADASTRO = t.DATA_CADASTRO;
                _tran.DATA_ALTERA = t.DATA_ALTERA;
                _tran.USU_LOGIN = t.USU_LOGIN;
                _tran.TRA_END_NUMERO = t.TRA_END_NUMERO;
            }

            return _tran;
        }
        public Pagina<TransportadorDTO> BuscarPorRazaoSocial(string _razaosocial, int numpagina = 1, int linhas = 10)
        {
            var _lista = (from t in db.TRANSPORTADOR
                          where t.TRA_RAZAO_SOCIAL.Contains(_razaosocial)
                          orderby t.TRA_RAZAO_SOCIAL descending
                          select t);

            return ToDTOPage(_lista, numpagina, linhas);
        }
    }
}
