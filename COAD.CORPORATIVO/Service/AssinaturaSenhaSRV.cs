using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.Atc;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Service;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("ASN_NUM_ASSINATURA")]
    public class AssinaturaSenhaSRV : ServiceAdapter<ASSINATURA_SENHA, AssinaturaSenhaDTO, string>
    {
        private AssinaturaSenhaDAO _dao = new AssinaturaSenhaDAO();
        public ContratoSRV ContratoSRV { get; set; }
        public SenhaSRV SenhaSRV { get; set; }

        public AssinaturaSenhaSRV()
        {
            SetDao(_dao);
        }
        //public void GerarSenha(string _assinatura)
        //{
        //    _dao.GerarSenha(_assinatura);
        //}
        public void BloqueiaSenhaAtiva(string _assinatura)
        {
            _dao.BloqueiaSenhaAtiva(_assinatura);
        }

        public AssinaturaSenhaDTO BuscarSenhaAtiva(string _assinatura)
        {
            return _dao.BuscarSenhaAtiva(_assinatura);
        }

        public AssinaturaSenhaDTO BuscarPorAssinatura(string _assinatura)
        {
            return _dao.BuscarPorAssinatura(_assinatura);
        }

        public string pegarUltimasenha()
        {
            return _dao.pegarUltimasenha();
        }

        public bool TestarSenhaDaAssinatura(string assinatura, string senha)
        {
            if (!string.IsNullOrWhiteSpace(assinatura) && !string.IsNullOrWhiteSpace(senha))
                return _dao.TestarSenhaDaAssinatura(assinatura, senha);
            else
                return false;
        }

        /// <summary>
        /// Deleta a senha da assinatura
        /// </summary>
        /// <param name="assinatura"></param>
        public void DeletarAssinaturaSenha(string assinatura)
        {
            var senha = BuscarPorAssinatura(assinatura);

            if(senha != null)
                Delete(senha);
        }

        public bool LogarCliente(UsuarioAtc usuario)
        {

            if (usuario != null && !string.IsNullOrWhiteSpace(usuario.senha))
            {
                var contrato = ContratoSRV.BuscarUltimoContratoValido(usuario.Assinatura);

                if (contrato != null)
                {
                    var UsuarioSenha = _dao.BuscarPorAssinatura(usuario.Assinatura);

                    //var senhaMd5 = SenhaSRV.HashMD5(UsuarioSenha.ASN_SENHA);

                    if(UsuarioSenha.ASN_SENHA != usuario.senha)
                    {
                        throw new Exception("Usuario ou senha inválido!");
                    }

                    return true;
                }
                else
                {
                    throw new Exception("Usuario ou senha inválido!");
                }
            }
            throw new NotImplementedException();
        }
    }

}
