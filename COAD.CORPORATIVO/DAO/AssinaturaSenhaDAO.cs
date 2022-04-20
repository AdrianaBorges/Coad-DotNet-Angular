using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace COAD.CORPORATIVO.DAO
{

    public class AssinaturaSenhaDAO : DAOAdapter<ASSINATURA_SENHA, AssinaturaSenhaDTO, string>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public AssinaturaSenhaDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        public AssinaturaSenhaDTO BuscarSenhaAtiva(string _assinatura)
        {
            var dto = GetDbSet().Where(x => x.ASN_NUM_ASSINATURA == _assinatura && x.ASN_ATIVO == true).FirstOrDefault();

            return ToDTO(dto);

        }
        public void GerarSenha(string _assinatura)
        {
            string _lastPass = GetDbSet().Max(x => x.ASN_SENHA);

            int _newPass = 0;
         
           
            ASSINATURA_SENHA _dto = new ASSINATURA_SENHA();

            _newPass = 1;// _newPass.Parse(_lastPass) + 1;
                  
            //---------- Cria Nova Senha
            
            _dto.ASN_NUM_ASSINATURA = _lastPass; 
            _dto.ASN_ATIVO = true;
            _dto.ASN_DATA_CADASTRO = DateTime.Now;
            _dto.ASN_SENHA = _lastPass; 

            this.Salvar(_dto);
            ///---------------
        }
        public void BloqueiaSenhaAtiva(string _assinatura)
        {
            AssinaturaSenhaDTO _dto = this.BuscarSenhaAtiva(_assinatura);

            //---------- Cria Nova Senha

            _dto.ASN_ATIVO = false;

            this.Update(ToModel(_dto));

            //---------------
        }

        public AssinaturaSenhaDTO BuscarPorAssinatura(string _assinatura)
        {
            ASSINATURA_SENHA query = db.ASSINATURA_SENHA.Where(x => x.ASN_NUM_ASSINATURA == _assinatura).FirstOrDefault();

            return ToDTO(query);
        }

        public string pegarUltimasenha()
        {
            string query = db.ASSINATURA_SENHA.OrderByDescending(x=>x.ASN_SENHA).Select(x=>x.ASN_SENHA).FirstOrDefault();

            return query;
        }

        public bool TestarSenhaDaAssinatura(string assinatura, string senha)
        {
            var query = (from assSenha in db.ASSINATURA_SENHA 
                         where 
                         assSenha.ASN_NUM_ASSINATURA == assinatura && 
                             assSenha.ASN_ATIVO == true &&
                             assSenha.ASN_SENHA == senha
                            select assSenha)
                            .Count();

            return (query > 0);
        }
    }
}