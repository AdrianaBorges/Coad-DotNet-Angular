﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Model.DTOPortal
{
    public class ClienteDTO
    {
        public int id { get; set; }
        public string perfil { get; set; }
        public string usuario { get; set; }
        public string senha { get; set; }
        public string permissao { get; set; }
        public string status { get; set; }
        public string cpf { get; set; }
        public string nome { get; set; }
        public string sobrenome { get; set; }
        public string empresa { get; set; }
        public string email { get; set; }
        public string endereco { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cep { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string telefone { get; set; }
        public string conhecimento { get; set; }
        public string @public { get; set; }
        public string vigencia { get; set; }
        public string trab { get; set; }
        public string profissao { get; set; }
        public string sexo { get; set; }
        public string data_nascimento { get; set; }
        public string area_atuacao { get; set; }
        public string receber_novidades { get; set; }
        public Nullable<int> cadastrado { get; set; }
        public string data1 { get; set; }
        public Nullable<System.DateTime> expiracao { get; set; }
        public string tipo_usuario { get; set; }
        public int contador { get; set; }
        public System.DateTime dataCadastro { get; set; }
        public Nullable<System.DateTime> data_atualizacao { get; set; }
        public string cnpj { get; set; }
        public int id_estacio { get; set; }
        public Nullable<bool> oab_flag { get; set; }
        public string oab_nr_inscricao { get; set; }
        public string oab_status { get; set; }
        public Nullable<System.DateTime> dt_ultimo_login { get; set; }
        public int qtd_sessoes { get; set; }
        public string oab_nr_ficha { get; set; }
        public Nullable<System.DateTime> data_repositorio { get; set; }
        public Nullable<int> pesquisa { get; set; }
    }
}
