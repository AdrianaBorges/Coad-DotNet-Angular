using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Extensions;
using System.Transactions;
using COAD.CORPORATIVO.Model.Dto.Custons;
using System.Data.Objects;
using GenericCrud.Models.Comparators;
using COAD.CORPORATIVO.Model.Dto.Custons.Relatorios;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Text.RegularExpressions;
using COAD.SEGURANCA.Service;
using System.Reflection;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using System.Data.Objects.SqlClient;

namespace COAD.CORPORATIVO.DAO
{
    public class ClienteDAO : DAOAdapter<CLIENTES, ClienteDto>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ClienteDAO()
        {
            this.db = GetDb<COADCORPEntities>(false);
        }

        public List<ClienteCustomDTO> ClientesAtivosLista(int? _grupo_id
                                                        , int _vigencia = 0
                                                        , int _atraso = 0
                                                        , bool _quitado = false
                                                        , int? _qtdecontratos = null
                                                        , string _anocoad = null
                                                        , string _uf = null)
        {

            var query = (from b in db.ASSINATURA
                         join s in db.ASSINATURA_SENHA on b.ASN_NUM_ASSINATURA equals s.ASN_NUM_ASSINATURA
                         join t in db.CONTRATOS on b.ASN_NUM_ASSINATURA equals t.ASN_NUM_ASSINATURA
                         join p in db.PRODUTOS on b.PRO_ID equals p.PRO_ID
                         join l in db.CLIENTES on b.CLI_ID equals l.CLI_ID
                         where (l.CLA_CLI_ID == 3) &&
                               ((_vigencia==0) || (_vigencia > 0 && t.CTR_DATA_INI_VIGENCIA.Value.Month == _vigencia)) &&
                               ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id)) &&
                               ((t.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                 ((t.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                  (t.CTR_DATA_FIM_VIGENCIA < DateTime.Now && t.CTR_PRORROGADO == 1))) ||
                                   t.CTR_CORTESIA == 1) && t.CTR_DATA_CANC == null
                                 && db.PARCELAS.Where(x => x.DATA_EXCLUSAO == null && 
                                                           x.CTR_NUM_CONTRATO == t.CTR_NUM_CONTRATO &&
                                                           x.PAR_DATA_PAGTO == null &&
                                                           SqlFunctions.DateDiff("day", DateTime.Now, x.PAR_DATA_VENCTO) > _atraso).Count() == 0
                               && ((!_quitado) || (_quitado && (db.PARCELAS.Where(x => x.DATA_EXCLUSAO == null &&
                                                                                  x.CTR_NUM_CONTRATO == t.CTR_NUM_CONTRATO &&
                                                                                  x.PAR_DATA_PAGTO == null).Count() == 0)))
                         select new ClienteCustomDTO
                         {
                             CLI_ID = l.CLI_ID,
                             ASN_NUM_ASSINATURA = b.ASN_NUM_ASSINATURA,
                             CLI_NOME = l.CLI_NOME,
                             CLI_CPF_CNPJ = l.CLI_CPF_CNPJ,
                             CTR_ANO_VIGENCIA = "",
                             CTR_DATA_FIM_VIGENCIA = null,
                             CTR_DATA_FAT = null,
                             PAR_QTDE_ABERTO = db.PARCELAS.Where(x => x.DATA_EXCLUSAO == null &&
                                                                      x.CTR_NUM_CONTRATO == t.CTR_NUM_CONTRATO &&
                                                                      x.PAR_DATA_PAGTO == null).Count(),
                             QTDE_CONTRATOS = db.CONTRATOS.Where(x => x.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                                                      x.ASN_NUM_ASSINATURA == t.ASN_NUM_ASSINATURA).Count(),
                             QTDE_RENOVACAO = db.CONTRATOS.Where(x => x.ASN_NUM_ASSINATURA == t.ASN_NUM_ASSINATURA).Count(),
                             ASN_ANO_COAD = b.ASN_ANO_COAD,
                             END_UF = db.CLIENTES_ENDERECO.Where(x => x.CLI_ID == l.CLI_ID).Max(x => x.END_UF),
                             CTR_VLR_CONTRATO = t.CTR_VLR_BRUTO,
                             AEM_EMAIL = db.ASSINATURA_EMAIL.FirstOrDefault(x => x.ASN_NUM_ASSINATURA == b.ASN_NUM_ASSINATURA).AEM_EMAIL

                         });
            

            var contratos = (from b in db.ASSINATURA
                             join s in db.ASSINATURA_SENHA on b.ASN_NUM_ASSINATURA equals s.ASN_NUM_ASSINATURA
                             join t in db.CONTRATOS on b.ASN_NUM_ASSINATURA equals t.ASN_NUM_ASSINATURA
                             join p in db.PRODUTOS on b.PRO_ID equals p.PRO_ID
                             join l in db.CLIENTES on b.CLI_ID equals l.CLI_ID
                             where (l.CLA_CLI_ID == 3) &&
                                       ((_vigencia == 0) || (_vigencia > 0 && t.CTR_DATA_INI_VIGENCIA.Value.Month == _vigencia)) &&
                                       ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id)) &&
                                       ((t.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                         ((t.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                          (t.CTR_DATA_FIM_VIGENCIA < DateTime.Now && t.CTR_PRORROGADO == 1))) ||
                                           t.CTR_CORTESIA == 1) && t.CTR_DATA_CANC == null
                                         && db.PARCELAS.Where(x => x.DATA_EXCLUSAO == null && x.CTR_NUM_CONTRATO == t.CTR_NUM_CONTRATO &&
                                                              SqlFunctions.DateDiff("day", DateTime.Now, x.PAR_DATA_VENCTO) > _atraso).Count() == 0
                                 select t);

            if (_qtdecontratos != null)
                query = query.Where(x => x.QTDE_CONTRATOS == _qtdecontratos);

            if (_anocoad != null)
                query = query.Where(x => x.ASN_ANO_COAD == _anocoad);

            if (_uf != null)
                query = query.Where(x => x.END_UF == _uf);

            query.OrderBy(x => x.CTR_DATA_INI_VIGENCIA).ThenBy(x => x.END_UF).ThenBy(x => x.CLI_NOME);

            var _lista = new List<ClienteCustomDTO>();

            foreach (var item in query)
            {
                var ctr = contratos.Where(x => x.ASN_NUM_ASSINATURA == item.ASN_NUM_ASSINATURA).OrderByDescending(x => x.CTR_DATA_FAT).FirstOrDefault();


                item.SITUACAO = "SEM CONTRATO";

                if (ctr != null)
                {

                    item.CTR_ANO_VIGENCIA = ctr.CTR_ANO_VIGENCIA;
                    item.CTR_DATA_FAT = ctr.CTR_DATA_FAT;
                    item.CTR_DATA_INI_VIGENCIA = ctr.CTR_DATA_INI_VIGENCIA;
                    item.CTR_DATA_FIM_VIGENCIA = ctr.CTR_DATA_FIM_VIGENCIA;

                    if (DateTime.Now >= ctr.CTR_DATA_INI_VIGENCIA && DateTime.Now <= ctr.CTR_DATA_FIM_VIGENCIA)
                        item.SITUACAO = "VIGENTE";
                    else if (DateTime.Now > ctr.CTR_DATA_FIM_VIGENCIA)
                        item.SITUACAO = "ENCERRADO";
                    else if (DateTime.Now < ctr.CTR_DATA_INI_VIGENCIA)
                        item.SITUACAO = "FUTURO";

                    if (ctr.CTR_PRORROGADO == 1 && DateTime.Now > ctr.CTR_DATA_FIM_VIGENCIA && ctr.CTR_DATA_CANC == null)
                        item.SITUACAO = "PRORROGADO";

                    if (ctr.CTR_DATA_CANC != null)
                        item.SITUACAO = "CANCELADO";

                    if (ctr.CTR_DATA_CANC < ctr.CTR_DATA_FIM_VIGENCIA)
                        item.SITUACAO = "CANC.DESISTÊNCIA";


                }

                _lista.Add(item);

            }

            return _lista;
        }

        public ClienteDto BuscarClienteUltimoContratoValidoPorCpfCnpj(string cpfCnpj)
        {
            var query = (from asn in db.ASSINATURA
                         join ctr in db.CONTRATOS on asn.ASN_NUM_ASSINATURA equals ctr.ASN_NUM_ASSINATURA
                         join cli in db.CLIENTES on asn.CLI_ID equals cli.CLI_ID
                         where cli.CLI_CPF_CNPJ == cpfCnpj &&
                         DateTime.Now <= ctr.CTR_DATA_FIM_VIGENCIA &&
                         ctr.CTR_DATA_CANC == null
                         orderby ctr.CTR_DATA_INI_VIGENCIA descending
                         select cli
                         ).FirstOrDefault();

            var cliente = ToDTO(query);

            return cliente;
        }

        public ClienteDto BuscarClienteUltimoContratoPorCpfCnpj(string cpfCnpj)
        {
            var query = (from asn in db.ASSINATURA
                         join ctr in db.CONTRATOS on asn.ASN_NUM_ASSINATURA equals ctr.ASN_NUM_ASSINATURA
                         join cli in db.CLIENTES on asn.CLI_ID equals cli.CLI_ID
                         where cli.CLI_CPF_CNPJ == cpfCnpj
                         orderby ctr.CTR_DATA_INI_VIGENCIA descending
                         select cli
                         ).FirstOrDefault();

            var cliente = ToDTO(query);

            return cliente;
        }

        public Pagina<ClienteCustomDTO> ClientesAtivos( int? _grupo_id
                                                , int _vigencia = 0
                                                , int _atraso = 0
                                                , bool _quitado = false
                                                , int? _qtdecontratos = null
                                                , string _anocoad = null
                                                , string _uf = null
                                                , int pagina = 1
                                                , int registroPorPagina = 10)
        {

            var query = (from b in db.ASSINATURA
                         join s in db.ASSINATURA_SENHA on b.ASN_NUM_ASSINATURA equals s.ASN_NUM_ASSINATURA
                         join t in db.CONTRATOS on b.ASN_NUM_ASSINATURA equals t.ASN_NUM_ASSINATURA
                         join p in db.PRODUTOS on b.PRO_ID equals p.PRO_ID
                         join l in db.CLIENTES on b.CLI_ID equals l.CLI_ID
                         where (l.CLA_CLI_ID == 3) &&
                               ((_vigencia == 0) || (_vigencia > 0 && t.CTR_DATA_INI_VIGENCIA.Value.Month == _vigencia)) &&
                               ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id)) &&
                               ((t.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                 ((t.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                  (t.CTR_DATA_FIM_VIGENCIA < DateTime.Now && t.CTR_PRORROGADO == 1))) ||
                                   t.CTR_CORTESIA == 1)
                                 && t.CTR_DATA_CANC == null
                                 && db.PARCELAS.Where(x => x.DATA_EXCLUSAO == null && x.CTR_NUM_CONTRATO == t.CTR_NUM_CONTRATO &&
                                                      SqlFunctions.DateDiff("day", DateTime.Now, x.PAR_DATA_VENCTO) > _atraso).Count() == 0
                                 && ((!_quitado) || (_quitado && (db.PARCELAS.Where(x => x.DATA_EXCLUSAO == null &&
                                                                                    x.CTR_NUM_CONTRATO == t.CTR_NUM_CONTRATO &&
                                                                                    x.PAR_DATA_PAGTO == null).Count() == 0)))
                         select new ClienteCustomDTO
                         {
                             CLI_ID = l.CLI_ID,
                             ASN_NUM_ASSINATURA = b.ASN_NUM_ASSINATURA,
                             CLI_NOME = l.CLI_NOME,
                             CLI_CPF_CNPJ = l.CLI_CPF_CNPJ,
                             CTR_ANO_VIGENCIA = "",
                             CTR_DATA_FIM_VIGENCIA = null,
                             CTR_DATA_FAT = null,
                             PAR_QTDE_ABERTO = db.PARCELAS.Where(x => x.DATA_EXCLUSAO == null &&
                                                                      x.CTR_NUM_CONTRATO == t.CTR_NUM_CONTRATO &&
                                                                      x.PAR_DATA_PAGTO == null).Count(),
                             QTDE_CONTRATOS = db.CONTRATOS.Where(x => x.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                                                      x.ASN_NUM_ASSINATURA == t.ASN_NUM_ASSINATURA).Count(),
                             QTDE_RENOVACAO = db.CONTRATOS.Where(x => x.ASN_NUM_ASSINATURA == t.ASN_NUM_ASSINATURA).Count(),
                             ASN_ANO_COAD = b.ASN_ANO_COAD,
                             END_UF = db.CLIENTES_ENDERECO.Where(x => x.CLI_ID == l.CLI_ID).Max(x => x.END_UF),
                             CTR_VLR_CONTRATO = t.CTR_VLR_BRUTO,
                             AEM_EMAIL = db.ASSINATURA_EMAIL.FirstOrDefault(x => x.ASN_NUM_ASSINATURA == b.ASN_NUM_ASSINATURA).AEM_EMAIL

                         });

            var contratos = (from b in db.ASSINATURA
                             join s in db.ASSINATURA_SENHA on b.ASN_NUM_ASSINATURA equals s.ASN_NUM_ASSINATURA
                             join t in db.CONTRATOS on b.ASN_NUM_ASSINATURA equals t.ASN_NUM_ASSINATURA
                             join p in db.PRODUTOS on b.PRO_ID equals p.PRO_ID
                             join l in db.CLIENTES on b.CLI_ID equals l.CLI_ID
                             where (l.CLA_CLI_ID == 3) &&
                                       ((_vigencia == 0) || (_vigencia > 0 && t.CTR_DATA_INI_VIGENCIA.Value.Month == _vigencia)) &&
                                       ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id)) &&
                                       ((t.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                         ((t.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                          (t.CTR_DATA_FIM_VIGENCIA < DateTime.Now && t.CTR_PRORROGADO == 1))) ||
                                           t.CTR_CORTESIA == 1) && t.CTR_DATA_CANC == null
                                         && db.PARCELAS.Where(x => x.DATA_EXCLUSAO == null && x.CTR_NUM_CONTRATO == t.CTR_NUM_CONTRATO &&
                                                              SqlFunctions.DateDiff("day", DateTime.Now, x.PAR_DATA_VENCTO) > _atraso).Count() == 0
                             select t);


            if (_qtdecontratos != null)
                query = query.Where(x => x.QTDE_CONTRATOS == _qtdecontratos);

            if (_anocoad != null)
                query = query.Where(x => x.ASN_ANO_COAD == _anocoad);

            if (_uf != null)
                query = query.Where(x => x.END_UF == _uf);

            query.OrderBy(x => x.CTR_DATA_INI_VIGENCIA).ThenBy(x => x.END_UF).ThenBy(x => x.CLI_NOME);

            var _retorno = query.Paginar(pagina, registroPorPagina);

            var _lista = new List<ClienteCustomDTO>();

            foreach (var item in _retorno.lista)
            {
                var ctr = contratos.Where(x => x.ASN_NUM_ASSINATURA == item.ASN_NUM_ASSINATURA).OrderByDescending(x => x.CTR_DATA_FAT).FirstOrDefault();

                item.SITUACAO = "SEM CONTRATO";

                if (ctr != null)
                {

                    item.CTR_ANO_VIGENCIA = ctr.CTR_ANO_VIGENCIA;
                    item.CTR_DATA_FAT = ctr.CTR_DATA_FAT;
                    item.CTR_DATA_INI_VIGENCIA = ctr.CTR_DATA_INI_VIGENCIA;
                    item.CTR_DATA_FIM_VIGENCIA = ctr.CTR_DATA_FIM_VIGENCIA;

                    if (DateTime.Now >= ctr.CTR_DATA_INI_VIGENCIA && DateTime.Now <= ctr.CTR_DATA_FIM_VIGENCIA)
                        item.SITUACAO = "VIGENTE";
                    else if (DateTime.Now > ctr.CTR_DATA_FIM_VIGENCIA)
                        item.SITUACAO = "ENCERRADO";
                    else if (DateTime.Now < ctr.CTR_DATA_INI_VIGENCIA)
                        item.SITUACAO = "FUTURO";

                    if (ctr.CTR_PRORROGADO == 1 && DateTime.Now > ctr.CTR_DATA_FIM_VIGENCIA && ctr.CTR_DATA_CANC == null)
                        item.SITUACAO = "PRORROGADO";

                    if (ctr.CTR_DATA_CANC != null)
                        item.SITUACAO = "CANCELADO";

                    if (ctr.CTR_DATA_CANC < ctr.CTR_DATA_FIM_VIGENCIA)
                        item.SITUACAO = "CANC.DESISTÊNCIA";


                }

                _lista.Add(item);

            }

            _retorno.lista = _lista.OrderBy(x => x.CLI_NOME);

            return _retorno;
        }

        
        public IList<RelContratosClienteAreaDTO> ListarClientesProduto(int _mes, int _ano)
        {

            DateTime _data = new DateTime(_ano, _mes, DateTime.DaysInMonth(_ano, _mes));

            var _lista = new List<RelContratosClienteDTO>();

            var _listaitens = db.RPT_CONTRATOS_CLIENTE_REGIAO(_data);

            RelContratosClienteAreaDTO _area = null;
            RelContratosClienteDTO _novoitem = null;
            RelContratosClienteDTO _totaltem = new RelContratosClienteDTO();
            List<RelContratosClienteAreaDTO> _listaArea = new List<RelContratosClienteAreaDTO>();
            _totaltem.PRO_NOME = "T O T A L:";

            var _pro_id_atu = 0;
            int? _area_id_atu = null;

            foreach (var _itens in _listaitens)
            {

                if (_itens.PRO_ID != _pro_id_atu)
                {
                    if (_novoitem != null)
                    {
                        _area.LISTAUF.Add(_novoitem);
                    }

                    _pro_id_atu = _itens.PRO_ID;
                    _novoitem = new RelContratosClienteDTO();
                    _novoitem.PRO_ID = _itens.PRO_ID;
                    _novoitem.PRO_NOME = _itens.PRO_NOME;

                }

                if (_itens.AREA_ID != _area_id_atu)
                {
                    if (_area_id_atu != null)
                    {
                        _area.LISTAUF.Add(_totaltem);
                        _listaArea.Add(_area);
                        _totaltem = new RelContratosClienteDTO();
                        _totaltem.PRO_NOME = "T O T A L:";

                    }
                    _area = new RelContratosClienteAreaDTO();
                    _area.AREA_ID = _itens.AREA_ID;
                    _area.AREA_NOME = _itens.AREA_NOME;

                    _pro_id_atu = _itens.PRO_ID;
                    _area_id_atu = _itens.AREA_ID;

                }


                var _camponome = _itens.UF;
                var _campovalor = _itens.QTDE;
                PropertyInfo propertyInfo = _novoitem.GetType().GetProperty(_camponome);
                propertyInfo.SetValue(_novoitem, _campovalor, null);


                PropertyInfo propertyInfo2 = _totaltem.GetType().GetProperty(_camponome);
                var _valoratu = propertyInfo2.GetValue(_totaltem);
                if (_valoratu != null)
                {
                    _campovalor += (int)_valoratu;
                    propertyInfo2.SetValue(_totaltem, _campovalor, null);
                }

                //---------------
                

            }

            if (_novoitem != null)
            {
                _area.LISTAUF.Add(_novoitem);
                _area.LISTAUF.Add(_totaltem);
            }

            _listaArea.Add(_area);

            return _listaArea;
        }
      
        public ClienteDto VerificarCPFCNPJ(string _cpfcnpj)
        {
            try
            {
                var _cliente = (from c in db.CLIENTES
                                where c.CLI_CPF_CNPJ == _cpfcnpj
                                select c).FirstOrDefault();

                return ToDTO(_cliente);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ClienteDto ValidarLoginEmail(string _email)
        {
            try
            {
                CLIENTES _usuario = null;

                var qtde = (from a in db.ASSINATURA_EMAIL
                            where a.AEM_EMAIL == _email
                            select a).Count();

                if (qtde > 0)
                    _usuario = (from c in db.ASSINATURA_EMAIL
                                where c.AEM_EMAIL == _email
                                select c.CLIENTES).First();

                return ToDTO(_usuario);

            }
            catch (Exception ex)
            {

                SysException.RegistrarLog("Erro ao Validar login (" + SysException.Show(ex) + ")", SysException.ShowIdException(ex), SessionContext.autenticado);

                throw new Exception(SysException.Show(ex));
            }
        }
        public string BuscarEmailSistema(int _cli_id)
        {
            try
            {
                string _email ="";

                var _assinatura = (from a in db.ASSINATURA_EMAIL
                                   join b in db.ASSINATURA on a.ASN_NUM_ASSINATURA equals b.ASN_NUM_ASSINATURA
                                   where b.CLI_ID == _cli_id && a.OPC_ID == 6
                                   select a).FirstOrDefault();

                if (_assinatura != null)
                    _email = _assinatura.AEM_EMAIL;

                return _email;

            }
            catch (Exception ex)
            {

                SysException.RegistrarLog("Erro ao Validar login (" + SysException.Show(ex) + ")", SysException.ShowIdException(ex), SessionContext.autenticado);

                throw new Exception(SysException.Show(ex));
            }
        }
        public void AlterarSenha(ClienteDto _cliente)
        {
            try
            {
                var _usuario = this.FindById(_cliente.CLI_ID);

                string _nsenha = SessionContext.HashMD5(_usuario.CLI_SENHA);

                _usuario.CLI_SENHA = _nsenha;

                this.Salvar(_usuario);

                SysException.RegistrarLog("Senha alterada com sucesso!!", "", SessionContext.autenticado);
            }
            catch (Exception ex)
            {
                SysException.RegistrarLog("Erro ao alterar senhado usuário (" + SysException.Show(ex) + ")", SysException.ShowIdException(ex), SessionContext.autenticado);
                throw;
            }
        }
        public Autenticado ValidarLogin(string _login, string _senha, int _origem)
        {
            try
            {

                ORIGEM_ACESSO _oacesso = db.ORIGEM_ACESSO.Where(x => x.OAC_ID == _origem).FirstOrDefault();

                Autenticado a = null;
                //     _senha = SessionContext.HashMD5(_senha);


                var _usulogado = (from b in db.ASSINATURA
                                  join s in db.ASSINATURA_SENHA on b.ASN_NUM_ASSINATURA equals s.ASN_NUM_ASSINATURA
                                  join c in db.PRODUTO_COMPOSICAO on b.CMP_ID equals c.CMP_ID
                                  join t in db.CONTRATOS on b.ASN_NUM_ASSINATURA equals t.ASN_NUM_ASSINATURA
                                  join l in db.CLIENTES on b.CLI_ID equals l.CLI_ID
                                  where (s.ASN_NUM_ASSINATURA == _login) &&
                                        (s.ASN_SENHA == _senha) &&
                                        (c.LIN_PRO_ID == _oacesso.LIN_PRO_ID) &&
                                        ((t.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                          ((t.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                           (t.CTR_DATA_FIM_VIGENCIA < DateTime.Now && t.CTR_PRORROGADO == 1))) ||
                                            t.CTR_CORTESIA == 1) && t.CTR_DATA_CANC == null
                                  select b).FirstOrDefault();


                if (_usulogado != null)
                {
                    a = new Autenticado();
                    a.USU_LOGIN = _usulogado.CLIENTES.CLI_LOGIN;
                    a.CLI_ID = (int)_usulogado.CLI_ID; 
                    a.ADMIN = true;
                    a.PER_ID = "CLIENTE";
                    a.USU_NOME = _usulogado.CLIENTES.CLI_NOME;
                    a.SIS_ID = _oacesso.OAC_DESCRICAO;
                    a.USU_CPF = _usulogado.CLIENTES.CLI_CPF_CNPJ;
                    a.USU_NOVA_SENHA = 0;
                    a.EMAIL = _usulogado.CLIENTES.CLI_EMAIL;
                    a.EMAIL_SENHA = _usulogado.CLIENTES.CLI_SENHA;
                    a.DATA_LOGIN = DateTime.Now;
                    a.MEIO_ACESSO = _oacesso.OAC_DESCRICAO;
                    a.LIN_PRO_ID = _oacesso.LIN_PRO_ID;
                }
                else
                   throw new Exception(" Caro cliente, seu login ou senha estão incorretos, Tente novamente! Caso o problema persista, entre em contato com o SAC para maiores informações!! ");


                return a;

            }
            catch (Exception ex)
            {

                SysException.RegistrarLog("Erro ao Validar login (" + SysException.Show(ex) + ")", SysException.ShowIdException(ex), SessionContext.autenticado, true);

                throw new Exception(SysException.Show(ex));

            }
        }

        public Boolean VerificarAssinaturaAtiva(string _assinatura)
        {
            try
            {


                var qtde = (from b in db.ASSINATURA
                            join s in db.ASSINATURA_SENHA on b.ASN_NUM_ASSINATURA equals s.ASN_NUM_ASSINATURA
                            join t in db.CONTRATOS on b.ASN_NUM_ASSINATURA equals t.ASN_NUM_ASSINATURA
                            join l in db.CLIENTES on b.CLI_ID equals l.CLI_ID
                            where (s.ASN_NUM_ASSINATURA == _assinatura) &&
                                  ((t.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                    ((t.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                     (t.CTR_DATA_FIM_VIGENCIA < DateTime.Now && t.CTR_PRORROGADO == 1))) ||
                                      t.CTR_CORTESIA == 1) && t.CTR_DATA_CANC == null
                            select b).Count();


                return (qtde > 0);

            }
            catch (Exception ex)
            {

                SysException.RegistrarLog("Erro ao Validar login (" + SysException.Show(ex) + ")", SysException.ShowIdException(ex), SessionContext.autenticado, true);

                throw new Exception(SysException.Show(ex));

            }
        }

        public ClienteDto BuscarPorCNPJ(string _cli_cnpj)
        {
            var _cli = (from c in db.CLIENTES
                        where c.CLI_CPF_CNPJ == _cli_cnpj && c.DATA_EXCLUSAO == null
                        select c).FirstOrDefault();

            return ToDTO(_cli);
        }
        public IList<ClienteDto> BuscarUltimoContratoPorCpfCnpj(string cpfCnpj)
        {
            var cliente = (from con in db.CONTRATOS
                           join asn in db.ASSINATURA on con.ASN_NUM_ASSINATURA equals asn.ASN_NUM_ASSINATURA
                           join cli in db.CLIENTES on asn.CLI_ID equals cli.CLI_ID
                           where cli.CLI_CPF_CNPJ == cpfCnpj
                           orderby con.CTR_DATA_INI_VIGENCIA descending
                           select cli);
            return ToDTO(cliente);
        }
        public override IQueryable<CLIENTES> SetTemplateQuery(System.Data.Entity.DbSet<CLIENTES> dbSet)
        {
            return dbSet.Where(op => op.CLA_CLI_ID == 3 && op.DATA_EXCLUSAO == null);
        }

        public Pagina<ClienteDto> ListaClientes(int cliente = 0,
                                                string contrato = null,
                                                string pedido = null,
                                                string assinatura = null,
                                                string logradouro = null,
                                                string cpf_cnpj = null,
                                                string nome = null, int pagina = 1, int registroPorPagina = 7)
        {
            IQueryable<CLIENTES> query = null;

            if (cliente > 0)
            {
                query = db.CLIENTES.Where(x => x.CLI_ID == cliente);
            }
            else
            {
                query = (from n in db.CLIENTES
                         where ((assinatura == null || assinatura == "") || n.ASSINATURA.Any(x => x.ASN_NUM_ASSINATURA == assinatura)) &&
                               ((logradouro == null || logradouro == "") || n.CLIENTES_ENDERECO.Any(e => e.END_LOGRADOURO.Contains(logradouro)))
                         orderby n.CLI_NOME
                         select n);


                if (!String.IsNullOrWhiteSpace(cpf_cnpj))
                {
                    query = query.Where(x => x.CLI_CPF_CNPJ == cpf_cnpj);
                }

                if (!String.IsNullOrWhiteSpace(nome))
                {
                    query = query.Where(x => x.CLI_NOME.Contains(nome));
                }

                if (!String.IsNullOrWhiteSpace(contrato))
                {
                    query = query.Where(x => x.ASSINATURA.Any(a => a.CONTRATOS.Any(b => b.CTR_NUM_CONTRATO == contrato)));
                }

                if (!String.IsNullOrWhiteSpace(pedido))
                {
                    query = query.Where(x => x.ASSINATURA.Any(a => a.CONTRATOS.Any(b => b.PED_NUM_PEDIDO == pedido)));
                }
            }

            query = query.Where(op => op.CLA_CLI_ID == 3);

            return ToDTOPage(query, pagina, registroPorPagina);
        }
        public ContratoSituacaoDTO BuscarSituacaoCliente(string _asn_id)
        {

            ContratoSituacaoDTO _situacao = new ContratoSituacaoDTO();

            var _linprod = (from c in db.CLIENTES
                            join a in db.ASSINATURA on c.CLI_ID equals a.CLI_ID
                            join t in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals t.ASN_NUM_ASSINATURA
                            join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                            join l in db.LINHA_PRODUTO on p.LIN_PRO_ID equals l.LIN_PRO_ID
                            where a.ASN_NUM_ASSINATURA == _asn_id
                           select l).FirstOrDefault();


            var _assinatura = (from a in db.ASSINATURA
                              where a.ASN_NUM_ASSINATURA == _asn_id
                             select a).FirstOrDefault();

            var _contrato = (from c in db.CLIENTES
                             join a in db.ASSINATURA on c.CLI_ID equals a.CLI_ID
                             join t in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals t.ASN_NUM_ASSINATURA
                             where (
                                    (t.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                      (
                                       (t.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                       (t.CTR_DATA_FIM_VIGENCIA < DateTime.Now && t.CTR_PRORROGADO == 1)
                                      )
                                     ) ||
                                     t.CTR_CORTESIA == 1) && 
                                     t.CTR_DATA_CANC == null && 
                                     a.ASN_NUM_ASSINATURA == _asn_id
                             select t).FirstOrDefault();

            //--Contrato Vigente -- não considera o campo prorrogado 
            var _contratovigente = (from c in db.CLIENTES
                                    join a in db.ASSINATURA on c.CLI_ID equals a.CLI_ID
                                    join t in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals t.ASN_NUM_ASSINATURA
                                    where (
                                            (t.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                                (
                                                 (t.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) 
                                                )
                                               ) ||
                                               t.CTR_CORTESIA == 1) &&
                                               t.CTR_DATA_CANC == null &&
                                               a.ASN_NUM_ASSINATURA == _asn_id
                                    select t).FirstOrDefault();


            var _senhas = (from c in db.CLIENTES
                           join a in db.ASSINATURA on c.CLI_ID equals a.CLI_ID
                           join s in db.ASSINATURA_SENHA on a.ASN_NUM_ASSINATURA equals s.ASN_NUM_ASSINATURA
                           join t in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals t.ASN_NUM_ASSINATURA
                           where ((t.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                 ((t.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                  (t.CTR_DATA_FIM_VIGENCIA < DateTime.Now && t.CTR_PRORROGADO == 1))) ||
                                   t.CTR_CORTESIA == 1) && t.CTR_DATA_CANC == null && a.ASN_NUM_ASSINATURA == _asn_id
                           select s).Count();

            var _telefones = (from c in db.CLIENTES
                              join a in db.ASSINATURA on c.CLI_ID equals a.CLI_ID
                              join e in db.ASSINATURA_TELEFONE on a.ASN_NUM_ASSINATURA equals e.ASN_NUM_ASSINATURA
                              join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                              join o in db.OPCAO_ATENDIMENTO on e.OPC_ID equals o.OPC_ID
                              join t in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals t.ASN_NUM_ASSINATURA
                              where ((t.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                    ((t.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                     (t.CTR_DATA_FIM_VIGENCIA < DateTime.Now && t.CTR_PRORROGADO == 1))) ||
                                      t.CTR_CORTESIA == 1)
                                      && t.CTR_DATA_CANC == null
                                      && p.PRO_URA == true
                                      && o.OPC_URA == true
                                      && a.ASN_NUM_ASSINATURA == _asn_id
                              select e).Count();

            var _titulosVencidos = (from p in db.PARCELAS
                                    join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                                    join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                                    where p.PAR_DATA_PAGTO == null
                                       && p.PAR_SITUACAO != "LIB"
                                       && EntityFunctions.DiffDays(p.PAR_DATA_VENCTO, DateTime.Now) > 21
                                       && a.ASN_NUM_ASSINATURA == _asn_id
                                       && c.CTR_DATA_INI_VIGENCIA <= DateTime.Now
						               && c.CTR_DATA_CANC == null
                                    select p).Count();

                   

            _situacao.CLI_ID = (int)_assinatura.CLI_ID;
            _situacao.ASN_NUM_ASSINATURA = _asn_id;
            _situacao.QTDE_CONS_CONTRATO = (_assinatura.ASN_QTDE_CONS_CONTRATO * 2);
            _situacao.QTDE_CONS_UTILIZADA = _assinatura.ASN_QTDE_CONS_UTILIZADA;

            if (_contrato != null)
            {
                _situacao.CONTRATO_VIGENTE = _contrato.CTR_NUM_CONTRATO;
                _situacao.CTR_VIGENCIA = ((DateTime)_contrato.CTR_DATA_INI_VIGENCIA).ToString("dd/MM/yyyy") + " a " +
                                         ((DateTime)_contrato.CTR_DATA_FIM_VIGENCIA).ToString("dd/MM/yyyy");
                _situacao.CTR_PRORROGADO = (_contratovigente != null) ? 0 : _contrato.CTR_PRORROGADO;
            }

            _situacao.SENHAS_CADASTRAS = _senhas;
            _situacao.TELEFONES_URA = _telefones;
            _situacao.TITULOS_VENCIDOS = _titulosVencidos;
            

            if (_linprod != null)
                _situacao.LINHA_PRODUTO = _linprod.LIN_PRO_DESCRICAO;

            if (_titulosVencidos == 0 && _senhas > 0 && _contrato != null)
            {
                if (_telefones > 0 && _situacao.QTDE_CONS_CONTRATO > _situacao.QTDE_CONS_UTILIZADA )
                    _situacao.ACESSA_URA = true;

                if (_assinatura.PRO_ID == 32)
                    _situacao.ACESSA_PORTAL_ST = true;
                else
                    _situacao.ACESSA_PORTAL = true;

            }



            return _situacao;
        }

        public Pagina<ClienteDto> ListaClientesContrato(int cliente = 0,
                                                string contrato = null,
                                                string pedido = null,
                                                string assinatura = null,
                                                string cpf_cnpj = null,
                                                string nome = null, int pagina = 1, int registroPorPagina = 7, Boolean somenteativos = false)
        {



            var query = (from c in db.CLIENTES
                         join a in db.ASSINATURA on c.CLI_ID equals a.CLI_ID into leftjoin
                         from x in leftjoin.DefaultIfEmpty()
                         where ((assinatura == null || assinatura == "") || (x.ASN_NUM_ASSINATURA == assinatura)) &&
                               ((cliente == 0) || (c.CLI_ID == cliente)) &&
                               ((contrato == null || contrato == "") || (db.CONTRATOS.Count(b1 => b1.CTR_NUM_CONTRATO == contrato && x.ASN_NUM_ASSINATURA == b1.ASN_NUM_ASSINATURA) > 0)) &&
                               ((pedido == null || pedido == "") || (db.CONTRATOS.Count(b2 => b2.PED_NUM_PEDIDO == pedido && x.ASN_NUM_ASSINATURA == b2.ASN_NUM_ASSINATURA) > 0))
                         group c by new
                         {
                             x.ASN_NUM_ASSINATURA,
                             c.CLI_ID,
                             c.CLI_NOME,
                             c.CLI_CPF_CNPJ,
                             c.CLA_CLI_ID
                         } into f
                         select new ClienteDto
                         {
                             CLI_ID = f.Key.CLI_ID,
                             ASN_NUM_ASSINATURA = f.Key.ASN_NUM_ASSINATURA,
                             CLI_NOME = f.Key.CLI_NOME,
                             CLI_CPF_CNPJ = f.Key.CLI_CPF_CNPJ,
                             CLA_CLI_ID = f.Key.CLA_CLI_ID,
                             CLI_CONTRATO_ATIVO = (db.CONTRATOS.Count(x => x.ASN_NUM_ASSINATURA == f.Key.ASN_NUM_ASSINATURA &&
                                                                           x.CTR_DATA_CANC == null &&
                                                                           ((x.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                                                           ((x.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                                                            (x.CTR_DATA_FIM_VIGENCIA < DateTime.Now && x.CTR_PRORROGADO == 1))) ||
                                                                             x.CTR_CORTESIA == 1)) > 0),

                             CLI_PARCELAS_ATRASO = 0


                         });

            var contratos = (from c in db.CLIENTES
                             join a in db.ASSINATURA on c.CLI_ID equals a.CLI_ID into leftjoina
                             from x in leftjoina.DefaultIfEmpty()
                             join t in db.CONTRATOS on x.ASN_NUM_ASSINATURA equals t.ASN_NUM_ASSINATURA into leftjoint
                             from t in leftjoint.DefaultIfEmpty()
                             where (c.CLA_CLI_ID == 3) &&
                                   ((assinatura == null || assinatura == "") || (x.ASN_NUM_ASSINATURA == assinatura)) &&
                                   ((cliente == 0) || (c.CLI_ID == cliente)) &&
                                   ((cpf_cnpj == null || cpf_cnpj == "") || (cpf_cnpj == c.CLI_CPF_CNPJ)) &&
                                   ((nome == null || nome == "") || ((nome != null && nome != "") && c.CLI_NOME.Contains(nome))) &&
                                   ((contrato == null || contrato == "") || (db.CONTRATOS.Count(b1 => b1.CTR_NUM_CONTRATO == contrato && x.ASN_NUM_ASSINATURA == b1.ASN_NUM_ASSINATURA) > 0)) &&
                                   ((pedido == null || pedido == "") || (db.CONTRATOS.Count(b2 => b2.PED_NUM_PEDIDO == pedido && x.ASN_NUM_ASSINATURA == b2.ASN_NUM_ASSINATURA) > 0)) 
                                   //(t.CTR_DATA_CANC == null &&
                                   // ((t.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                   // ((t.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                   //  (t.CTR_DATA_FIM_VIGENCIA < DateTime.Now && t.CTR_PRORROGADO == 1))) ||
                                   //                                             t.CTR_CORTESIA == 1))
                             select t);



            if (!String.IsNullOrWhiteSpace(cpf_cnpj))
            {
                
                query = query.Where(x => x.CLI_CPF_CNPJ == cpf_cnpj);
            }

            if (!String.IsNullOrWhiteSpace(nome))
            {
                
                query = query.Where(x => x.CLI_NOME.Contains(nome));
            }

            if (somenteativos)
                query = query.Where(op => op.CLI_CONTRATO_ATIVO == true);

            query = query.Where(op => op.CLA_CLI_ID == 3);

            var _retorno = query.Paginar(pagina, registroPorPagina);

            var _lista = new List<ClienteDto>();


            foreach (var item in _retorno.lista)
            {
                var ctr = contratos.Where(x => x.ASN_NUM_ASSINATURA == item.ASN_NUM_ASSINATURA).OrderByDescending(x => x.CTR_DATA_FAT).FirstOrDefault();


                item.SITUACAO = "ENCERRADO";

                if (ctr != null)
                {

                    item.CTR_NUM_CONTRATO = ctr.CTR_NUM_CONTRATO;
                    item.CTR_DATA_CANC = ctr.CTR_DATA_CANC;
                    item.CTR_DATA_FAT = ctr.CTR_DATA_FAT;
                    item.CTR_DATA_INI_VIGENCIA = ctr.CTR_DATA_INI_VIGENCIA;
                    item.CTR_DATA_FIM_VIGENCIA = ctr.CTR_DATA_FIM_VIGENCIA;

                    if (DateTime.Now >= ctr.CTR_DATA_INI_VIGENCIA && DateTime.Now <= ctr.CTR_DATA_FIM_VIGENCIA)
                        item.SITUACAO = "VIGENTE";
                    else if (DateTime.Now > ctr.CTR_DATA_FIM_VIGENCIA)
                        item.SITUACAO = "ENCERRADO";
                    else if (DateTime.Now < ctr.CTR_DATA_INI_VIGENCIA)
                        item.SITUACAO = "FUTURO";

                    if (ctr.CTR_PRORROGADO == 1 && DateTime.Now > ctr.CTR_DATA_FIM_VIGENCIA && ctr.CTR_DATA_CANC == null)
                        item.SITUACAO = "PRORROGADO";

                    if (ctr.CTR_DATA_CANC != null)
                        item.SITUACAO = "CANCELADO";

                    if (ctr.CTR_DATA_CANC < ctr.CTR_DATA_FIM_VIGENCIA)
                        item.SITUACAO = "CANC.DESISTÊNCIA";


                }

                _lista.Add(item);

            }


            _retorno.lista = _lista;

            return _retorno;

        }


        public Pagina<ClienteDto> Clientes(
            string cpf_cnpj = null,
            string nome = null,
            int pagina = 1,
            int registroPorPagina = 7,
            int? representanteId = null,
            int? uen_id = null,
            int? classificacaoClienteId = null,
            string CAR_ID = null,
            int? RG_ID = null,
            string email = null,
            string representante = null)
        {
            IQueryable<CLIENTES> query = null;

            if (representanteId != null)
            {
                query = TemplateQueryClientesPorRepresentante(cpf_cnpj, nome, representanteId, uen_id, classificacaoClienteId, CAR_ID, RG_ID, email, representante);
            }
            else
            {
                query = TemplateQueryClientes(cpf_cnpj, nome);
            }
            return ToDTOPage(query, pagina, registroPorPagina);
        }

        public Pagina<BuscarClienteDTO> BuscarClientes(
            PesquisaClienteDTO pesquisaDTO)
        {
            IQueryable<CLIENTES> query = null;
            
            query = TemplateBuscarClientes(pesquisaDTO);            

            Pagina<BuscarClienteDTO> respPagina = query.Paginar<CLIENTES, BuscarClienteDTO>(pesquisaDTO.pagina, pesquisaDTO.registroPorPagina, profileName);

            if (respPagina != null && respPagina.lista != null)
            {
                respPagina.lista = respPagina.lista.Distinct().OrderByDescending(or => or.DATA_ULTIMO_HISTORICO);
            }

            return respPagina;
        }

        private IQueryable<CLIENTES> TemplateQueryClientes(string cpf_cnpj = null, string nome = null)
        {
            IQueryable<CLIENTES> query = GetDbSetWithTemplate();

            if (!String.IsNullOrWhiteSpace(cpf_cnpj))
            {
                query = query.Where(x => x.CLI_CPF_CNPJ == cpf_cnpj);
            }

            if (!String.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(x => x.CLI_NOME.Contains(nome));
            }

            query = query.Where(op => op.CLA_CLI_ID == 3);

            return query;

        }

        private ClienteDto BuscarClientePorCPFCNPJ(string cpf_cnpj = null)
        {
            //CLIENTES query = GetDbSetWithTemplate();
            CLIENTES query = GetDbSet().Where(x => x.CLI_CPF_CNPJ == cpf_cnpj && x.CLA_CLI_ID == 3).FirstOrDefault();



            //query = query.Where(op => op.CLA_CLI_ID == 3);

            return ToDTO(query);

        }

        private IQueryable<CLIENTES> TemplateQueryClientesPorRepresentante(
            string cpf_cnpj = null,
            string nome = null,
            int? representanteId = null,
            int? uenId = null,
            int? classificacaoClienteId = null,
            string CAR_ID = null,
            int? RG_ID = null,
            string email = null,
            string representante = null,
            string dddTelefone = null,
            string telefone = null)
        {
            var db = GetDb<COADCORPEntities>(false);

            var respCarCli = (from car_rep in db.CARTEIRA_REPRESENTANTE
                              join car_cli in db.CARTEIRA_CLIENTE
                                 on car_rep.CAR_ID equals car_cli.CAR_ID
                              where car_rep.REPRESENTANTE.REP_ATIVO == 1
                              && car_cli.CARTEIRA.DATA_EXCLUSAO == null
                              && (representante == null || car_rep.REPRESENTANTE.REP_NOME.Contains(representante))
                              && (representanteId == null || car_rep.REP_ID == representanteId)
                              && car_cli.CLIENTES.DATA_EXCLUSAO == null
                              select car_cli);

            IQueryable<CLIENTES> resp = null;

            if (!String.IsNullOrWhiteSpace(CAR_ID))
            {
                respCarCli = respCarCli.Where(x => x.CAR_ID == CAR_ID);
            }

            if (RG_ID != null)
            {
                respCarCli = respCarCli.Where(x => x.CARTEIRA.RG_ID == RG_ID);
            }

            if (uenId != null)
            {
                resp = respCarCli.Where(x => x.CARTEIRA.UEN_ID == uenId).Select(x => x.CLIENTES);
            }
            else
            {
                resp = respCarCli.Select(x => x.CLIENTES);
            }

            if (!String.IsNullOrWhiteSpace(cpf_cnpj))
            {
                resp = resp.Where(x => x.CLI_CPF_CNPJ.Contains(cpf_cnpj));
            }
            if (!String.IsNullOrWhiteSpace(nome))
            {
                resp = resp.Where(x => x.CLI_NOME.Contains(nome));
            }

            if (classificacaoClienteId != null)
            {
                resp = resp.Where(x => x.CLA_CLI_ID == (int)classificacaoClienteId);
            }

            return resp.Distinct().OrderBy(or => or.CLI_NOME);
        }

        /// <summary>
        ///  Query otimizada para busca de clientes
        /// </summary>
        /// <param name="cpf_cnpj"></param>
        /// <param name="nome"></param>
        /// <param name="uenId"></param>
        /// <param name="classificacaoClienteId"></param>
        /// <param name="email"></param>
        /// <param name="REP_ID"></param>
        /// <param name="dddTelefone"></param>
        /// <param name="telefone"></param>
        /// <param name="AREA_ID"></param>
        /// <param name="CMP_ID"></param>
        /// <param name="CLI_ID"></param>
        /// <param name="pesquisaCpfCnpjPorIqualdade"></param>
        /// <returns></returns>
        private IQueryable<CLIENTES> TemplateBuscarClientes(
            PesquisaClienteDTO pesquisaDTO)
        {
            string cpf_cnpj = pesquisaDTO.cpf_cnpj;
            string codigoAssinatura = pesquisaDTO.codigoAssinatura;
            string nome = pesquisaDTO.nome;
            string email = pesquisaDTO.email;
            string dddTelefone = pesquisaDTO.dddTelefone;
            string telefone = pesquisaDTO.telefone;
            bool pesquisaCpfCnpjPorIqualdade = pesquisaDTO.pesquisaCpfCnpjPorIqualdade;
            int pagina = pesquisaDTO.pagina;
            int registroPorPagina = pesquisaDTO.registroPorPagina;
            int? codigoCliente = pesquisaDTO.codigoCliente;
            int? repIdCart = pesquisaDTO.repId;
            IList<int?> lstClaCliId = pesquisaDTO.lstClaCliId;
            var excluidosDaValidacao = pesquisaDTO.excluidosDaValidacao;
            var REP_ID = pesquisaDTO.REP_ID;
            var uenId = pesquisaDTO.uen_id;
            var classificacaoClienteId = pesquisaDTO.classificacaoClienteId;
            var AREA_ID = pesquisaDTO.AREA_ID;
            var CMP_ID = pesquisaDTO.CMP_ID;
            var origemId = pesquisaDTO.origemId;

            var db = GetDb<COADCORPEntities>(false);

            IQueryable<CLIENTES> clientes = null;

            if (excluidosDaValidacao == false)
                excluidosDaValidacao = null;

            if (classificacaoClienteId != null)
            {
                if (lstClaCliId == null)
                    lstClaCliId = new List<int?>();
                lstClaCliId.Add(classificacaoClienteId);
            }

            // Se existir filtro de representante a base da consulta, ou seja, o ponto de partida da query
            // passa pela carteira_representante x carteira_cliente x representante. Depois projeta o cliente
            // atravez da cliente.
            if (REP_ID != null)
            {
                clientes = (from car_rep in db.CARTEIRA_REPRESENTANTE
                            join car_cli in db.CARTEIRA_CLIENTE
                               on car_rep.CAR_ID equals car_cli.CAR_ID
                            where car_rep.REPRESENTANTE.REP_ATIVO == 1
                            && car_cli.CARTEIRA.DATA_EXCLUSAO == null
                            && (excluidosDaValidacao == null || car_cli.CLIENTES.CLI_EXCLUIDO_VALIDACAO == excluidosDaValidacao)
                            && (uenId == null || (car_rep.CARTEIRA.UEN_ID == uenId))
                            && (car_rep.REPRESENTANTE.REP_ID == REP_ID)
                            select car_cli.CLIENTES);

                if (pesquisaDTO.buscarCarteiraAssinatara)
                {
                    var queryAss = (from 
                                    cli in db.CLIENTES join
                                    ass in db.ASSINATURA
                                            on cli.CLI_ID equals ass.CLI_ID join
                                    car_ass in db.CARTEIRA_ASSINATURA 
                                            on ass.ASN_NUM_ASSINATURA equals car_ass.ASN_NUM_ASSINATURA join 
                                    car_rep in db.CARTEIRA_REPRESENTANTE
                                            on car_ass.CAR_ID equals car_rep.CAR_ID join
                                    rep in db.REPRESENTANTE 
                                            on car_rep.REP_ID equals rep.REP_ID join
                                    car in db.CARTEIRA 
                                            on car_rep.CAR_ID equals car.CAR_ID
                                where 
                                    rep.REP_ATIVO == 1
                                    && car.DATA_EXCLUSAO == null
                                    && (uenId == null || (car.UEN_ID == uenId))
                                    && (rep.REP_ID == REP_ID)
                                select cli);
                    clientes = clientes.Union(queryAss).Distinct();
                }

                // Se o filtro de telefone for preenchido 
                //faço um join da tabela assinatura_telefone atravez da clientes projetada na consulta base para pesquisar o telefone do cliente.
                if (!string.IsNullOrWhiteSpace(telefone))
                {
                    clientes = (from cli in clientes
                                join ass_tel in db.ASSINATURA_TELEFONE
                                on cli.CLI_ID equals ass_tel.CLI_ID into lstTelefone
                                from subSetTel in lstTelefone.DefaultIfEmpty()
                                where (subSetTel.ATE_TELEFONE == telefone) &&
                                    (dddTelefone == null || dddTelefone == "" || subSetTel.ATE_DDD == dddTelefone)
                                select cli);
                }


                // Se o filtro de email for preenchido 
                //faço um join da tabela assinatura_email atravez da clientes projetada na consulta base para pesquisar o email do cliente.
                if (!string.IsNullOrWhiteSpace(email))
                {
                    clientes = (from cli in clientes
                                join ass_email in db.ASSINATURA_EMAIL
                                on cli.CLI_ID equals ass_email.CLI_ID into lstEmail
                                from subSetEmail in lstEmail.DefaultIfEmpty()
                                where (subSetEmail.AEM_EMAIL.Contains(email))
                                select cli);
                }
            }
            else
            {
                if (pesquisaDTO.BuscarTodos)
                {
                    clientes = (from cli in db.CLIENTES
                                where
                                (excluidosDaValidacao == null || cli.CLI_EXCLUIDO_VALIDACAO == excluidosDaValidacao)
                                select cli);
                }
                else
                {
                    clientes = (from car_cli in db.CARTEIRA_CLIENTE
                                where car_cli.CARTEIRA.UEN_ID == uenId &&
                                (excluidosDaValidacao == null || car_cli.CLIENTES.CLI_EXCLUIDO_VALIDACAO == excluidosDaValidacao)
                                select car_cli.CLIENTES); // só retorna clientes encarteirados
                }
            }

            //Faço o filtro dos clientes
            clientes = (from cli in clientes
                        where
                         (codigoCliente == null || cli.CLI_ID == codigoCliente) &&
                         (cli.DATA_EXCLUSAO == null)
                        select cli);

            if (lstClaCliId != null)
            {
                clientes = (from cli in clientes
                            where
                             (from claCli in lstClaCliId select claCli).Contains(cli.CLA_CLI_ID)
                            select cli);
            }
            if (!string.IsNullOrWhiteSpace(cpf_cnpj))
            {
                if (pesquisaCpfCnpjPorIqualdade)
                {
                    clientes = clientes.Where(x => x.CLI_CPF_CNPJ == cpf_cnpj);
                }
                else
                {
                    clientes = clientes.Where(x => x.CLI_CPF_CNPJ.Contains(cpf_cnpj));
                }

            }

            if (!string.IsNullOrWhiteSpace(nome))
            {

                clientes = clientes.Where(x => x.CLI_NOME.Contains(nome));
            }

            if (!string.IsNullOrWhiteSpace(codigoAssinatura))
            {
                clientes = (from ass in db.ASSINATURA 
                                join cli in clientes on ass.CLI_ID equals cli.CLI_ID
                         where
                            ass.ASN_NUM_ASSINATURA.Contains(codigoAssinatura)
                         select cli);
            }
        

            // Verifico se o representante não está preenchido e se o telefone está. Se não for preenchido, será executada essa query para 
            // filtrar o telefone por subquery direto no cliente.
            if (REP_ID == null && !string.IsNullOrWhiteSpace(telefone))
            {
                clientes = (from cli in clientes
                            join ass_tel in db.ASSINATURA_TELEFONE
                            on cli.CLI_ID equals ass_tel.CLI_ID
                            where (ass_tel.ATE_TELEFONE == telefone) &&
                            (dddTelefone == null || dddTelefone == "" || ass_tel.ATE_DDD == dddTelefone)
                            select cli);
            }

            // Verifico se o representante não está preenchido e se o email está. Se não for preenchido, será executada essa query para 
            // filtrar o email por subquery direto no cliente.
            if (REP_ID == null && !string.IsNullOrWhiteSpace(email))
            {
                clientes = (from cli in clientes
                            join ass_email in db.ASSINATURA_EMAIL
                            on cli.CLI_ID equals ass_email.CLI_ID into lstEmail
                            from subSetEmail in lstEmail.DefaultIfEmpty()
                            where (subSetEmail.AEM_EMAIL.Contains(email))
                            select cli);
            }

            if (AREA_ID != null)
            {
                clientes = clientes.Where(x => x.INFO_MARKETING.AREA_INFO_MARKETING.Select(s => s.AREA_ID).Contains((int)AREA_ID));
            }

            if (CMP_ID != null)
            {
                clientes = clientes.Where(x => x.INFO_MARKETING.PRODUTO_COMPOSICAO_INFO_MARKETING.Select(s => s.CMP_ID).Contains((int)CMP_ID));
            }

            if (origemId != null)
            {
                clientes = clientes.Where(x => x.INFO_MARKETING.O_CAD_ID == origemId);
            }

            //IQueryable<CLIENTES> resp = clientes.Select(sel => sel.CLIENTES);
            return clientes;//.Distinct();//.OrderByDescending(or => or.DATA_ULTIMO_HISTORICO);
        }
        
        public IQueryable<QuantidadeClassificacaoClienteDTO> QtdClientesRepresentante(int? REP_ID, int? UEN_ID = 1)
        {
            var query = TemplateQueryClientesPorRepresentante(null, null, REP_ID, UEN_ID, null);
            query = query.Where(x => x.CLA_CLI_ID != null);

            var queryGroup =
                query.GroupBy(group => new
                {
                    group.CLASSIFICACAO_CLIENTE.CLA_CLI_ID,
                    group.CLASSIFICACAO_CLIENTE.CLA_CLI_DESCRICAO
                });


            var resp = queryGroup.Select(s =>
                      new QuantidadeClassificacaoClienteDTO()
                      {
                          CLA_CLI_DESCRICAO = s.Key.CLA_CLI_DESCRICAO,
                          CLA_CLI_ID = s.Key.CLA_CLI_ID,
                          QUANTIDADE = s.Count()
                      });

            return resp;

        }

        public bool RepresentantePodeEditarCliente(int? CLI_ID, int? representanteId, int? uenId = 1)
        {
            var query = (from car_rep in db.CARTEIRA_REPRESENTANTE
                         join car_cli in db.CARTEIRA_CLIENTE
                            on car_rep.CAR_ID equals car_cli.CAR_ID
                         where car_rep.REPRESENTANTE.REP_ATIVO == 1
                         && car_rep.CARTEIRA.DATA_EXCLUSAO == null
                         && car_cli.CLIENTES.DATA_EXCLUSAO == null
                         && car_cli.CLIENTES.CLI_ID == CLI_ID
                         && car_rep.REPRESENTANTE.REP_ID == representanteId
                         && car_rep.CARTEIRA.UEN_ID == uenId
                         && car_cli.CARTEIRA.UEN_ID == uenId
                         select car_cli);

            var count = query.Count();

            return (count > 0);
        }

        /// <summary>
        /// Faz querys nos campos para verificar se o cliente existe
        /// </summary>
        /// <param name="CLI_ID">Id do cliente caso necessite excluir um cliente da query. Caso de edição do mesmo</param>
        /// <param name="nome"></param>
        /// <param name="cpf_cnpj"></param>
        /// <param name="assinaturaTelefones"></param>
        /// <param name="assinaturaEmails"></param>
        /// <returns></returns>
        public ResultClienteDuplicadoDTO ClienteJaExiste(int? CLI_ID, string nome, string cpf_cnpj, 
            IEnumerable<AssinaturaTelefoneDTO> assinaturaTelefones, 
            IEnumerable<AssinaturaEmailDTO> assinaturaEmails)
        {
            //Veriáveis de resposta
            bool nomejaExiste = false;
            bool cpfJaExiste = false;
            bool telefoneJaExiste = false;
            bool emailJaExiste = false;

            ClienteDto clienteBanco = null;
            IList<int?> ListaCpjJaExiste = null;
            IList<AssinaturaTelefoneDTO> ListaTelefoneJaExiste = null;
            IList<AssinaturaEmailDTO> ListaEmailJaExiste = null;
            

            // query do cpf_cnpj
            if (!string.IsNullOrWhiteSpace(cpf_cnpj))
            {
                var queryCpf = db.CARTEIRA_CLIENTE.Where(x =>
                    (x.CLIENTES.CLI_EXCLUIDO_VALIDACAO == null || x.CLIENTES.CLI_EXCLUIDO_VALIDACAO == false) && 
                    (x.CLIENTES.CLI_CPF_CNPJ == cpf_cnpj.Trim()) && 
                    (CLI_ID == null || CLI_ID != x.CLIENTES.CLI_ID) && 
                    (x.CLIENTES.DATA_EXCLUSAO == null));

                ListaCpjJaExiste = queryCpf.Select(x => (int?) x.CLI_ID).Distinct().ToList();
                cpfJaExiste = ListaCpjJaExiste.Count() > 0;

                if (cpfJaExiste)
                {
                    var lstCPF = queryCpf
                        .Where(x => x.CLIENTES.CLI_CPF_CNPJ == null)
                        .Select(x => x.CLIENTES.CLI_CPF_CNPJ)
                        .Distinct();

                }
            }

            // query to telefone
            if (assinaturaTelefones != null && assinaturaTelefones.Count() > 0)
            {
                var lstAssinaturaTelefones = assinaturaTelefones.Select(x => x.ATE_TELEFONE);

                IQueryable<ASSINATURA_TELEFONE> queryTelefone = (from ass_telefone in db.ASSINATURA_TELEFONE
                                                                 join car_cli in db.CARTEIRA_CLIENTE on ass_telefone.CLI_ID equals car_cli.CLI_ID
                                                                 where
                                                                 (car_cli.CLIENTES.CLI_EXCLUIDO_VALIDACAO == null || car_cli.CLIENTES.CLI_EXCLUIDO_VALIDACAO == false) &&
                                                                 (CLI_ID == null || ass_telefone.CLI_ID != CLI_ID) &&
                                                                 (car_cli.CLIENTES.TIPO_CLI_ID == 2 && car_cli.CLIENTES.TIPO_CLI_ID == 2 || 
                                                                 car_cli.CLIENTES.TIPO_CLI_ID != 2 && car_cli.CLIENTES.TIPO_CLI_ID != 2) &&
                                                                 (ass_telefone.CLIENTES.DATA_EXCLUSAO == null) &&
                                                                     lstAssinaturaTelefones.Contains(ass_telefone.ATE_TELEFONE)
                                                                 select ass_telefone);

                // Não é possível fazer query em uma lista em memória com a do banco.
                // Por isso primeiro faço uma query no banco filtrando pelos números.
                // Se existir ddd na lista, então  pego e resultado em memória e 
                // faço outra consulta levando em conta o ddd
                var lstComDDD = assinaturaTelefones.Where(x => x.ATE_DDD != null);

                if (lstComDDD.Count() > 0)
                {
                    queryTelefone = queryTelefone.ToList().AsQueryable();
                    queryTelefone = (from telefone_ass in queryTelefone
                                     join telefone_ass_2 in lstComDDD
                                           on telefone_ass.ATE_TELEFONE equals telefone_ass_2.ATE_TELEFONE
                                     where telefone_ass.ATE_DDD == telefone_ass_2.ATE_DDD
                                     select telefone_ass);

                }

                ListaTelefoneJaExiste = Convert<IQueryable<ASSINATURA_TELEFONE>, List<AssinaturaTelefoneDTO>>(queryTelefone.Select(x => x));
                telefoneJaExiste = ListaTelefoneJaExiste.Count() > 0;
            }


            // query to email
            if (assinaturaEmails != null && assinaturaEmails.Count() > 0)
            {
                var lstEmail = assinaturaEmails.Select(x => x.AEM_EMAIL);

                var queryEmail = (from ass_email in db.ASSINATURA_EMAIL
                                  join car_cli in db.CARTEIRA_CLIENTE on ass_email.CLI_ID equals car_cli.CLI_ID
                                  where
                                  (car_cli.CLIENTES.CLI_EXCLUIDO_VALIDACAO == null || car_cli.CLIENTES.CLI_EXCLUIDO_VALIDACAO == false) &&
                                  (CLI_ID == null || ass_email.CLI_ID != CLI_ID) &&
                                  (car_cli.CLIENTES.TIPO_CLI_ID == 2 && car_cli.CLIENTES.TIPO_CLI_ID == 2 ||
                                    car_cli.CLIENTES.TIPO_CLI_ID != 2 && car_cli.CLIENTES.TIPO_CLI_ID != 2) &&
                                  (ass_email.CLIENTES.DATA_EXCLUSAO == null) &&
                                     lstEmail.Contains(ass_email.AEM_EMAIL)
                                  select ass_email);

                ListaEmailJaExiste = Convert<IQueryable<ASSINATURA_EMAIL>, List<AssinaturaEmailDTO>>(queryEmail.Select(x => x).Distinct());

                emailJaExiste = ListaEmailJaExiste.Count() > 0;
            }

            ResultClienteDuplicadoDTO _result = new ResultClienteDuplicadoDTO()
            {
                HasDuplicationNome = new ValidacaoTypeResultDTO() {Falhou = nomejaExiste },
                HasDuplicationCPF_CNPJ = new ValidacaoTypeResultDTO() { Falhou = cpfJaExiste },
                HasDuplicationTelefone = new ValidacaoTypeResultDTO() { Falhou = telefoneJaExiste },
                HasDuplicationEmail = new ValidacaoTypeResultDTO() { Falhou = emailJaExiste },
                ListDuplicationCPF_CNPJ = ListaCpjJaExiste,
                ListDuplicationEmail = ListaEmailJaExiste,
               // ListDuplicationNome = ListaNomejaExiste, não implementado
                ListDuplicationTelefone = ListaTelefoneJaExiste
            };

            return _result;
        }

        public Pagina<RelatorioClientesRecebidosDTO> SuspectsCadastradosNoDia(DateTime data, int? UEN_ID, int? RG_ID, int pagina = 1, int registrosPorPagina = 100)
        {
            var query = (from car_rep in db.CARTEIRA_REPRESENTANTE
                         join car_cli in db.CARTEIRA_CLIENTE
                          on car_rep.CAR_ID equals car_cli.CAR_ID
                         let prioridade = (from pri_ate in db.PRIORIDADE_ATENDIMENTO where pri_ate.CLI_ID == car_cli.CLI_ID && pri_ate.TP_PRI_ID == 1 select pri_ate).FirstOrDefault()
                         where EntityFunctions.TruncateTime(car_cli.CLIENTES.DATA_CADASTRO) == EntityFunctions.TruncateTime(data)
                         && car_rep.REPRESENTANTE.REP_ATIVO == 1
                         && car_cli.CARTEIRA.DATA_EXCLUSAO == null
                         && (RG_ID == null || (car_rep.CARTEIRA.RG_ID == RG_ID && car_rep.REPRESENTANTE.RG_ID == RG_ID))
                         && (UEN_ID == null || (car_rep.REPRESENTANTE.UEN_ID == UEN_ID && car_rep.CARTEIRA.UEN_ID == UEN_ID))

                         select new RelatorioClientesRecebidosDTO()
                         {
                             CLI_ID = (int)car_cli.CLI_ID,
                             CLI_NOME = car_cli.CLIENTES.CLI_NOME,
                             REP_NOME = prioridade.REPRESENTANTE.REP_NOME,
                             REP_NOME_DEMANDANTE = prioridade.REPRESENTANTE1.REP_NOME,
                             DATA = car_cli.CLIENTES.DATA_CADASTRO,
                             RG_ID = prioridade.REPRESENTANTE.REGIAO.RG_ID,
                             RG_DESCRICAO = prioridade.REPRESENTANTE.REGIAO.RG_DESCRICAO,
                             RG_ID_REP_DEMANDANTE = prioridade.REPRESENTANTE1.REGIAO.RG_ID,
                             RG_DESCRICAO_DEMANDANTE = prioridade.REPRESENTANTE1.REGIAO.RG_DESCRICAO
                         });

            return query.Paginar(pagina, registrosPorPagina);


        }

        private IQueryable<CLIENTES> TemplateGeralBuscarClientes(
            string cpf_cnpj = null,
            string nome = null,
            int? classificacaoClienteId = null,
            string email = null,
            string dddTelefone = null,
            string telefone = null,
            int? AREA_ID = null,
            int? CMP_ID = null,
            int? CLI_ID = null,
            bool pesquisaCpfCnpjPorIqualdade = true,
            bool? excluidosDaValidacao = null)
        {
            var db = GetDb<COADCORPEntities>(false);

            IQueryable<CLIENTES> clientes = null;
            clientes = db.CLIENTES;

            if (excluidosDaValidacao == false)
                excluidosDaValidacao = null;

            //Faço o filtro dos clientes
            clientes = (from cli in clientes
                        where
                         (excluidosDaValidacao == null || cli.CLI_EXCLUIDO_VALIDACAO == excluidosDaValidacao) && 
                         (classificacaoClienteId == null || cli.CLA_CLI_ID == classificacaoClienteId) &&
                         (CLI_ID == null || cli.CLI_ID == CLI_ID) &&
                         (cli.DATA_EXCLUSAO == null)
                        select cli);

            if (!string.IsNullOrWhiteSpace(cpf_cnpj))
            {
                if (pesquisaCpfCnpjPorIqualdade)
                {
                    clientes = clientes.Where(x => x.CLI_CPF_CNPJ == cpf_cnpj);
                }
                else
                {
                    clientes = clientes.Where(x => x.CLI_CPF_CNPJ.Contains(cpf_cnpj));
                }
            }

            if (!string.IsNullOrWhiteSpace(nome))
            {

                clientes = clientes.Where(x => x.CLI_NOME.Contains(nome));
            }

            // Verifico se o representante não está preenchido e se o telefone está. Se não for preenchido, será executada essa query para 
            // filtrar o telefone por subquery direto no cliente.
            if (!string.IsNullOrWhiteSpace(telefone))
            {
                var tels = (from ass_tel in db.ASSINATURA_TELEFONE
                            where (ass_tel.ATE_TELEFONE == telefone) &&
                                (dddTelefone == null || dddTelefone == "" || ass_tel.ATE_DDD == dddTelefone)
                            select ass_tel);

                clientes = (from cli in clientes join tel in tels on cli.CLI_ID equals tel.CLI_ID
                            where (excluidosDaValidacao == null || cli.CLI_EXCLUIDO_VALIDACAO == excluidosDaValidacao)
                            select cli);
            }

            // Verifico se o representante não está preenchido e se o email está. Se não for preenchido, será executada essa query para 
            // filtrar o email por subquery direto no cliente.
            if (!string.IsNullOrWhiteSpace(email))
            {
                var emailsIds =
                        (from ass_email in db.ASSINATURA_EMAIL
                         where (ass_email.AEM_EMAIL.Contains(email))
                         select
                             (ass_email.ASSINATURA != null)
                                 ? ass_email.ASSINATURA.CLI_ID : ass_email.CLI_ID);

                clientes = (from cli in clientes where emailsIds.Contains(cli.CLI_ID) select cli);
            }

            if (AREA_ID != null)
            {
                clientes = clientes.Where(x => x.INFO_MARKETING.AREA_INFO_MARKETING.Select(s => s.AREA_ID).Contains((int)AREA_ID));
            }

            if (CMP_ID != null)
            {
                clientes = clientes.Where(x => x.INFO_MARKETING.PRODUTO_COMPOSICAO_INFO_MARKETING.Select(s => s.CMP_ID).Contains((int)CMP_ID));
            }

            //IQueryable<CLIENTES> resp = clientes.Select(sel => sel.CLIENTES);
            return clientes; //.Select(sel => sel).Distinct().OrderByDescending(or => or.DATA_ULTIMO_HISTORICO);
        }

        public Pagina<BuscarClienteDTO> BuscarClientesGeral(
            string cpf_cnpj = null,
            string nome = null,
            int? classificacaoClienteId = null,
            string email = null,
            string dddTelefone = null,
            string telefone = null,
            int? AREA_ID = null,
            int? CMP_ID = null,
            int? CLI_ID = null,
            bool pesquisaCpfCnpjPorIqualdade = true,
            int pagina = 1,
            int registroPorPagina = 7,
            bool? excluidosDaValidacao = null)
        {
            IQueryable<CLIENTES> query = null;
            query = TemplateGeralBuscarClientes(cpf_cnpj, nome, classificacaoClienteId, email, dddTelefone, telefone, AREA_ID, CMP_ID, CLI_ID, pesquisaCpfCnpjPorIqualdade, excluidosDaValidacao);
            Pagina<BuscarClienteDTO> respPagina = query.Paginar<CLIENTES, BuscarClienteDTO>(pagina, registroPorPagina, profileName);

            if (respPagina != null && respPagina.lista != null)
            {
                respPagina.lista.OrderByDescending(or => or.DATA_ULTIMO_HISTORICO);
            }

            return respPagina;
        }

        public bool ChecaClienteExisteDentroDaAgenda(int? CLI_ID)
        {
            var count = (from car_cli in db.CARTEIRA_CLIENTE
                         where car_cli.CLI_ID == CLI_ID
                         select car_cli.CLIENTES.CLI_ID).Count();

            return (count > 0);
        }

        public ClienteDto BuscarClientesJaExistentes(IList<string> listaCNPJ_CPF, IList<string> listaTelefones, IList<string> listaEmails)
        {
            if(listaCNPJ_CPF != null){

                int count = listaCNPJ_CPF.Count();
                for(var index = 0; index <= count - 1; index++)
                {
                    listaCNPJ_CPF[index] = listaCNPJ_CPF[index].Trim();
                }
            }
            
            if(listaTelefones != null){

                int count = listaTelefones.Count();
                for(var index = 0; index <= count - 1; index++)
                {
                    listaTelefones[index] = listaTelefones[index].Trim();
                    Regex rgx = new Regex(@"\D");
                    listaTelefones[index] = rgx.Replace(listaTelefones[index], ""); 
                }
            }

            if(listaEmails != null){

                int count = listaEmails.Count();
                for(var index = 0; index <= count - 1; index++)
                {
                    listaEmails[index] = listaEmails[index].Trim();
                }
            }

            var query = (from cli in db.CLIENTES join 
                             tel in db.ASSINATURA_TELEFONE on cli.CLI_ID equals tel.CLI_ID into lstTelefone
                             from subSet in lstTelefone.DefaultIfEmpty()
                             join email in db.ASSINATURA_EMAIL on cli.CLI_ID equals email.CLI_ID into lstEmail
                                 from subSetEmail in lstEmail.DefaultIfEmpty()
                         where 
                             listaCNPJ_CPF.Contains(cli.CLI_CPF_CNPJ.Trim()) ||
                             listaTelefones.Contains(subSet.ATE_DDD + subSet.ATE_TELEFONE) ||
                             listaEmails.Contains(subSetEmail.AEM_EMAIL) 
                         select cli).FirstOrDefault();

            return ToDTO(query);
        }

        public Pagina<BuscarClienteDTO> BuscarProspects(
            string cpf_cnpj = null,
            string nome = null,
            string email = null,
            string dddTelefone = null,
            string telefone = null,
            bool pesquisaCpfCnpjPorIqualdade = true,
            int pagina = 1,
            int registroPorPagina = 30,
            int? codigoCliente = null)
        {
            
            if(string.IsNullOrWhiteSpace(cpf_cnpj))
                cpf_cnpj = null;

            if(string.IsNullOrWhiteSpace(nome))
                nome = null;

            if (string.IsNullOrWhiteSpace(dddTelefone))
                dddTelefone = null;

            if(string.IsNullOrWhiteSpace(telefone))
                telefone = null;

            if (string.IsNullOrWhiteSpace(email))
                email = null;

            IQueryable<CLIENTES> query = (from cli in db.CLIENTES 
                         where 
                             //(cli.CLA_CLI_ID != 3) && // descomentar depois
                             (codigoCliente == null || cli.CLI_ID == codigoCliente) &&
                             (
                                (cpf_cnpj == null) || 
                                (
                                    (pesquisaCpfCnpjPorIqualdade && cli.CLI_CPF_CNPJ == cpf_cnpj) || 
                                    (pesquisaCpfCnpjPorIqualdade == false && cli.CLI_CPF_CNPJ.Contains(cpf_cnpj))
                                )
                             ) 
                             &&
                             (nome == null || cli.CLI_NOME.Contains(nome))
                        select cli);


            if (dddTelefone != null || telefone != null)
            {
                query = (from cli in query
                         join tel in db.ASSINATURA_TELEFONE on cli.CLI_ID equals tel.CLI_ID
                         where
                         (dddTelefone == null || tel.ATE_DDD == dddTelefone) &&
                         (telefone == null || tel.ATE_TELEFONE == telefone)
                         select cli);
            }

            if (email != null)
            {
                query = (from cli in query
                         join objMail in db.ASSINATURA_EMAIL on cli.CLI_ID equals objMail.CLI_ID
                         where
                            objMail.AEM_EMAIL == email
                         select cli);
            }

            Pagina<BuscarClienteDTO> respPagina = query.Paginar<CLIENTES, BuscarClienteDTO>(pagina, registroPorPagina, profileName);

            if (respPagina != null && respPagina.lista != null)
            {
                respPagina.lista = respPagina.lista.Distinct().OrderByDescending(or => or.DATA_ULTIMO_HISTORICO);
            }

            return respPagina;
        }

        /// <summary>
        /// Busca todos os clientes do sistema, independente de carteira ou UEN.
        /// </summary>
        /// <param name="cpf_cnpj"></param>
        /// <param name="nome"></param>
        /// <param name="email"></param>
        /// <param name="dddTelefone"></param>
        /// <param name="telefone"></param>
        /// <param name="pesquisaCpfCnpjPorIqualdade"></param>
        /// <param name="pagina"></param>
        /// <param name="registroPorPagina"></param>
        /// <param name="codigoCliente"></param>
        /// <returns></returns>
        //public Pagina<BuscarClienteDTO> BuscarClienteGlobal(
        //    PesquisaClienteDTO pesquisaDTO
        //    )
        //{
        //    string cpf_cnpj = pesquisaDTO.cpf_cnpj;
        //    string codigoAssinatura = pesquisaDTO.codigoAssinatura;
        //    string nome = pesquisaDTO.nome;
        //    string email = pesquisaDTO.email;
        //    string dddTelefone = pesquisaDTO.dddTelefone;
        //    string telefone = pesquisaDTO.telefone;
        //    bool pesquisaCpfCnpjPorIqualdade = pesquisaDTO.pesquisaCpfCnpjPorIqualdade;
        //    int pagina = pesquisaDTO.pagina;
        //    int registroPorPagina = pesquisaDTO.registroPorPagina;
        //    int? codigoCliente = pesquisaDTO.codigoCliente;
        //    int? repIdCart = pesquisaDTO.repId;
        //    IList<int?> lstClaCliId = pesquisaDTO.lstClaCliId;       

        //    if (string.IsNullOrWhiteSpace(cpf_cnpj))
        //        cpf_cnpj = null;

        //    if (string.IsNullOrWhiteSpace(nome))
        //        nome = null;

        //    if (string.IsNullOrWhiteSpace(dddTelefone))
        //        dddTelefone = null;

        //    if (string.IsNullOrWhiteSpace(telefone))
        //        telefone = null;

        //    if (string.IsNullOrWhiteSpace(email))
        //        email = null;

        //    IQueryable<CLIENTES> query = null;

        //    if (!string.IsNullOrWhiteSpace(codigoAssinatura))
        //        query = (from ass in db.ASSINATURA
        //                 where
        //                    ass.ASN_NUM_ASSINATURA.Contains(codigoAssinatura)
        //                 select ass.CLIENTES);
        //    else
        //        query = (from cli in db.CLIENTES select cli);

        //    query = (from cli in query
        //                                  where
        //                                      (codigoCliente == null || cli.CLI_ID == codigoCliente) &&
        //                                      (
        //                                         (cpf_cnpj == null) ||
        //                                         (
        //                                             (pesquisaCpfCnpjPorIqualdade && cli.CLI_CPF_CNPJ == cpf_cnpj) ||
        //                                             (pesquisaCpfCnpjPorIqualdade == false && cli.CLI_CPF_CNPJ.Contains(cpf_cnpj))
        //                                         )
        //                                      )
        //                                      &&
        //                                      (nome == null || cli.CLI_NOME.Contains(nome))                                              
        //                                  select cli);


            

        //    if (dddTelefone != null || telefone != null)
        //    {
        //        query = (from cli in query
        //                 join tel in db.ASSINATURA_TELEFONE on cli.CLI_ID equals tel.CLI_ID
        //                 where
        //                 (dddTelefone == null || tel.ATE_DDD == dddTelefone) &&
        //                 (telefone == null || tel.ATE_TELEFONE == telefone)
        //                 select cli);
        //    }

        //    if (email != null)
        //    {
        //        query = (from cli in query
        //                 join objMail in db.ASSINATURA_EMAIL on cli.CLI_ID equals objMail.CLI_ID
        //                 where
        //                    objMail.AEM_EMAIL == email
        //                 select cli);
        //    }

        //    if (repIdCart != null)
        //    {
        //        //query = (from cli in query
        //        //         where
        //        //            (from rep in db.REPRESENTANTE where rep.REP_ID == repIdCart
        //        //                 select rep.CAR_ID)
        //        //                 .Contains(cli.CLI_CAR_ID_PROSPECT)
        //        //         select cli);
        //    }

        //    if (lstClaCliId != null)
        //    {
        //        query = (from cli in query
        //                 where
        //                 (from claCli in lstClaCliId select claCli).Contains(cli.CLA_CLI_ID)
        //                 select cli);
        //    }

        //    Pagina<BuscarClienteDTO> respPagina = query.Paginar<CLIENTES, BuscarClienteDTO>(pagina, registroPorPagina, profileName);

        //    if (respPagina != null && respPagina.lista != null)
        //    {
        //        respPagina.lista = respPagina.lista.Distinct().OrderByDescending(or => or.DATA_ULTIMO_HISTORICO);
        //    }

        //    return respPagina;
        //}


        /// <summary>
        /// Verifica se o representante está associado ao prospect (associação do corporativo antigo)
        /// </summary>
        /// <param name="cliId"></param>
        /// <param name="carId"></param>
        /// <returns></returns>
        public bool ChecarProspectDoRepresentante(int? cliId, int? repId)
        {
            var count = (from 
                                car_cli in db.CARTEIRA_CLIENTE join
                                car in db.CARTEIRA on car_cli.CAR_ID equals car.CAR_ID join
                                car_rep in db.CARTEIRA_REPRESENTANTE on car.CAR_ID equals car_rep.CAR_ID
                            where 
                                car.DATA_EXCLUSAO == null &&
                                car_rep.REPRESENTANTE.REP_ATIVO == 1 &&
                                car_cli.CLI_ID == cliId &&
                                car_rep.REP_ID == repId
                            select car_cli.CLI_ID
                         )
                         .Count();
            var count2 = (from 
                                cli in db.CLIENTES join
                                ass in db.ASSINATURA on cli.CLI_ID equals ass.CLI_ID join
                                car_ass in db.CARTEIRA_ASSINATURA on ass.ASN_NUM_ASSINATURA equals car_ass.ASN_NUM_ASSINATURA join
                                car in db.CARTEIRA on car_ass.CAR_ID equals car.CAR_ID join
                                car_rep in db.CARTEIRA_REPRESENTANTE on car.CAR_ID equals car_rep.CAR_ID
                            where 
                                car.DATA_EXCLUSAO == null &&
                                car_rep.REPRESENTANTE.REP_ATIVO == 1 &&
                                cli.CLI_ID == cliId &&
                                car_rep.REP_ID == repId
                            select cli.CLI_ID
                         )
                         .Count();

            var countFinal = count + count2;
            return ((countFinal > 0));
        }

        public IList<ClienteDto> ListarClientesPorIds(ICollection<int?> lstIds)
        {
            if (lstIds == null)
                lstIds = new HashSet<int?>();

            var query = (from cli in db.CLIENTES
                         where lstIds.Contains(cli.CLI_ID)
                         select cli);

            return ToDTO(query);            
        }

        public IList<ClienteDto> BuscarClientePorBoleto(string _parcela)
        {
            var query = (from e in db.CLIENTES
                         join a in db.ASSINATURA on e.CLI_ID equals a.CLI_ID
                         join c in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA
                         join p in db.PARCELAS on c.CTR_NUM_CONTRATO equals p.CTR_NUM_CONTRATO
                         where p.PAR_NUM_PARCELA == _parcela
                         select e);

            var query2 = (from p in db.PARCELAS
                          join i in db.ITEM_PEDIDO on p.IPE_ID equals i.IPE_ID
                          join e in db.PEDIDO_CRM on i.PED_CRM_ID equals e.PED_CRM_ID
                          join c in db.CLIENTES on e.CLI_ID equals c.CLI_ID
                          where p.PAR_NUM_PARCELA == _parcela
                          select c);

            var query3 = (from p in db.PARCELAS
                          join i in db.PROPOSTA_ITEM on p.PPI_ID equals i.PPI_ID
                          join a in db.PROPOSTA on i.PRT_ID equals a.PRT_ID
                          join c in db.CLIENTES on a.CLI_ID equals c.CLI_ID
                          where p.PAR_NUM_PARCELA == _parcela
                          select c);
            
            var query4 = query.Union(query2).Union(query3).Distinct();


            return ToDTO(query4);

        }

        public IList<ClienteDto> ListarClientesSimilares(int? cliIdExcluir, string cpf_cnpj = null, ICollection<AssinaturaTelefoneDTO> lstTelefones = null, ICollection<AssinaturaEmailDTO> lstEmails = null) 
        {
                if(string.IsNullOrWhiteSpace(cpf_cnpj) &&
                    lstTelefones == null &&
                    lstEmails == null)
                {
                    return new List<ClienteDto>();
                }

                IList<string> lstTelefonesStr = null;
                IList<string> lstEmailStr = null;

                IQueryable<CLIENTES> query = null;
                IQueryable<CLIENTES> clienteCNPJ_CPF = null;
                IQueryable<CLIENTES> clienteTel = null;
                IQueryable<CLIENTES> clienteEmail = null;

                if (lstTelefones != null)
                {
                    lstTelefonesStr = lstTelefones.Select(x => x.ATE_DDD + x.ATE_TELEFONE).ToList();
                }

                if (lstEmails != null)
                {
                    lstEmailStr = lstEmails.Select(x => x.AEM_EMAIL).ToList();
                }

                if (!string.IsNullOrWhiteSpace(cpf_cnpj)){
                    
                    clienteCNPJ_CPF =
                        (from cli in
                                 db.CLIENTES
                         where 
                            cli.CLI_CPF_CNPJ == cpf_cnpj &&
                            cli.CLI_ID != cliIdExcluir
                         select cli);
                }

                if (lstTelefonesStr != null)
                {
                    var queryTel1 = (from 
                                assTel in db.ASSINATURA_TELEFONE join
                                cli in db.CLIENTES on assTel.CLI_ID equals cli.CLI_ID
                             where lstTelefonesStr.Contains(assTel.ATE_DDD + assTel.ATE_TELEFONE) &&
                                cli.CLI_ID != cliIdExcluir
                                     select cli);

                    var queryTel2 = (from 
                                assTel in db.ASSINATURA_TELEFONE join
                                ass in db.ASSINATURA on assTel.ASN_NUM_ASSINATURA equals ass.ASN_NUM_ASSINATURA join
                                cli in db.CLIENTES on ass.CLI_ID equals cli.CLI_ID
                             where lstTelefonesStr.Contains(assTel.ATE_DDD + assTel.ATE_TELEFONE) &&
                                cli.CLI_ID != cliIdExcluir
                                     select cli);

                    clienteTel = queryTel1.Union(queryTel2);

                }

                if (lstEmailStr != null)
                {
                    var queryEmail1 = (from 
                                assEmail in db.ASSINATURA_EMAIL join
                                cli in db.CLIENTES on assEmail.CLI_ID equals cli.CLI_ID
                             where lstEmailStr.Contains(assEmail.AEM_EMAIL) &&
                                    cli.CLI_ID != cliIdExcluir
                                       select cli);

                    var queryEmail2 = (from 
                                assEmail in db.ASSINATURA_EMAIL join
                                ass in db.ASSINATURA on assEmail.ASN_NUM_ASSINATURA equals ass.ASN_NUM_ASSINATURA join
                                cli in db.CLIENTES on ass.CLI_ID equals cli.CLI_ID
                             where lstEmailStr.Contains(assEmail.AEM_EMAIL) &&
                                    cli.CLI_ID != cliIdExcluir
                                       select cli);

                    clienteEmail = queryEmail1.Union(queryEmail2);
                }
                
                if(clienteCNPJ_CPF != null)
                    query = (query != null) ? query.Union(clienteCNPJ_CPF) : clienteCNPJ_CPF;
                if (clienteTel != null)
                    query = (query != null) ? query = query.Union(clienteTel) : clienteTel;                    
                if (clienteEmail != null)
                    query = (query != null) ? query.Union(clienteEmail) : clienteEmail;
                
                return ToDTO(query.Distinct());
        }

        public ClienteDto FindByIDImportacaoSuspect(int? ipsID)
        {
            var query = (from cli in 
                             db.CLIENTES
                         where 
                            cli.IPS_ID == ipsID
                         select cli);
            return ToDTO(query.FirstOrDefault());
        }

        public ClienteDto RetornarClienteDaParcela(string codParcela)
        {
            var query = (from 
                            par in db.PARCELAS join
                            con in db.CONTRATOS on par.CTR_NUM_CONTRATO equals con.CTR_NUM_CONTRATO join
                            assi in db.ASSINATURA on con.ASN_NUM_ASSINATURA equals assi.ASN_NUM_ASSINATURA
                        where 
                            par.PAR_NUM_PARCELA == codParcela
                        select assi.CLIENTES).FirstOrDefault();
            return ToDTO(query);
        }


        public ClienteDto ObterClientePorCLIID(int cLI_ID)
        {
            var result = (from cli in db.CLIENTES
                          where cli.DATA_EXCLUSAO == null && cli.CLI_ID == cLI_ID
                          select cli).FirstOrDefault();

            return ToDTO(result);
        }

        public int? ObterTipoPorCLIID(int cLI_ID)
        {
            var result = (from cli in db.CLIENTES
                          where cli.DATA_EXCLUSAO == null && cli.CLI_ID == cLI_ID
                          select cli.TIPO_CLI_ID).FirstOrDefault();

            return result;
        }
    }
}