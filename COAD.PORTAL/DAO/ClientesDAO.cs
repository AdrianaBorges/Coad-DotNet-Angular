using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.PORTAL.Model.DTO;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.DAO
{
    public class ClientesDAO : AbstractGenericDao<CLIENTES_PORTAL_PROC_Result, ClientesDTO, string>
    {
        private COADIARIOEntities db { get; set; }
        
        public ClientesDAO()
        {
            SetProfileName("portal");

            db = GetDb<COADIARIOEntities>(false);
        }


        public ClientesDTO BuscarCliente(string login, string senha, string email = "", string cpf = "", string tipoBusca = "0")
        {
            CLIENTES_PORTAL_PROC_Result query = db.CLIENTES_PORTAL_PROC(login,senha,email,cpf,tipoBusca).FirstOrDefault();                
            //GetDbSet().Where(x=> x.usuario.Equals(login) && x.senha.Equals(senha)).FirstOrDefault();
            return ToDTO(query);
        }

        public int AtualizarClienteExpirado(ClientesDTO cliente, string dtaExpiracao)
        {
            int query = db.ATUALIZAR_CLIENTES_EXPIRADO_PORTAL_PROC(cliente.id.ToString(), cliente.senha, cliente.email, dtaExpiracao, cliente.data_atualizacao.ToString());
            //GetDbSet().Where(x=> x.usuario.Equals(login) && x.senha.Equals(senha)).FirstOrDefault();
            return query;
        }

        public int CadastrarCliente(ClientesDTO cliente, string dtaExpiracao)
        {
            cliente.numero = cliente.numero == null ? "" : cliente.numero;
            cliente.complemento = cliente.complemento == null ? "" : cliente.complemento;
            cliente.bairro = cliente.bairro == null ? "" : cliente.bairro;
            cliente.cep = cliente.cep == null ? "" : cliente.cep;
            cliente.cidade = cliente.cidade == null ? "" : cliente.cidade;
            cliente.conhecimento = cliente.conhecimento == null ? "" : cliente.conhecimento;
            cliente.publico = cliente.publico == null ? "" : cliente.publico;
            cliente.vigencia = cliente.vigencia == null ? "" : cliente.vigencia;
            cliente.trab = cliente.trab == null ? "" : cliente.trab;
            cliente.profissao = cliente.profissao == null ? "" : cliente.profissao;
            cliente.sexo = cliente.sexo == null ? "" : cliente.sexo;
            cliente.data_nascimento = cliente.data_nascimento == null ? "" : cliente.data_nascimento;
            cliente.area_atuacao = cliente.area_atuacao == null ? "" : cliente.area_atuacao;
            cliente.receber_novidades = cliente.receber_novidades == null ? "" : cliente.receber_novidades;
            cliente.cadastrado = cliente.cadastrado == null ? 0 : cliente.cadastrado;
            cliente.data1 = cliente.data1 == null ? "" : cliente.data1;
            cliente.contador = cliente.contador == null ? 0 : cliente.contador;
            cliente.dataCadastro = cliente.dataCadastro == null ? DateTime.Now : cliente.dataCadastro;
            cliente.data_atualizacao = cliente.data_atualizacao == null ? DateTime.Now : cliente.data_atualizacao;
            cliente.cnpj = cliente.cnpj == null ? "" : cliente.cnpj;
            cliente.id_estacio = cliente.id_estacio == null ? 0 : cliente.id_estacio;
            cliente.oab_flag = cliente.oab_flag == null ? 0 : cliente.oab_flag;
            cliente.oab_nr_inscricao = cliente.oab_nr_inscricao == null ? "" : cliente.oab_nr_inscricao;
            cliente.oab_status = cliente.oab_status == null ? "" : cliente.oab_status;
            cliente.dt_ultimo_login = cliente.dt_ultimo_login == null ? DateTime.Now : cliente.dt_ultimo_login;
            cliente.qtd_sessoes = cliente.qtd_sessoes == null ? 1 : cliente.qtd_sessoes;
            cliente.oab_nr_ficha = cliente.oab_nr_ficha == null ? "" : cliente.oab_nr_ficha;
            cliente.data_repositorio = cliente.data_repositorio == null ? DateTime.Now : cliente.data_repositorio;
            cliente.pesquisa = cliente.pesquisa == null ? 0 : cliente.pesquisa;            

            int query = db.GRAVAR_CLIENTES_PORTAL_PROC(cliente.perfil,cliente.usuario, cliente.senha, cliente.permissao,
                cliente.status, cliente.cpf, cliente.nome, cliente.sobrenome, cliente.empresa, cliente.email, cliente.endereco, cliente.numero, cliente.complemento,
                cliente.bairro, cliente.cep, cliente.cidade, cliente.estado, cliente.telefone, cliente.conhecimento, cliente.publico,
                cliente.vigencia, cliente.trab, cliente.profissao, cliente.sexo, cliente.data_nascimento, cliente.area_atuacao,
                cliente.receber_novidades, cliente.cadastrado.ToString(), cliente.data1, dtaExpiracao, cliente.tipo_usuario, cliente.contador.ToString(),
                cliente.dataCadastro.ToString(), cliente.data_atualizacao.ToString(), cliente.cnpj, cliente.id_estacio.ToString(), cliente.oab_flag.ToString(), cliente.oab_nr_inscricao,
                cliente.oab_status, cliente.dt_ultimo_login.ToString(), cliente.qtd_sessoes.ToString(), cliente.oab_nr_ficha, cliente.data_repositorio.ToString(), cliente.pesquisa.ToString());

            return query;
        }
    }
}
