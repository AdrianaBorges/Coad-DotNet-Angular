using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Model.Dto.Custons.Ecommerce;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using COADRESTSERVICE.Extensions;
using COAD.CORPORATIVO.Model.Dto.Custons.Atc;

namespace COADRESTSERVICE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public LoginController(ClienteUsuarioSRV ClienteUsuarioSRV, AssinaturaSenhaSRV AssinaturaSenhaSRV)
        {
            this.ClienteUsuarioSRV = ClienteUsuarioSRV;
            this.AssinaturaSenhaSRV = AssinaturaSenhaSRV;
        }
        
        private ClienteUsuarioSRV ClienteUsuarioSRV { get; set; }
        private AssinaturaSenhaSRV AssinaturaSenhaSRV { get; set;}

        [HttpPost("realizar-login-atc")]
        public JSONResponse RealizarLoginAtc([FromBody] UsuarioAtc usuario)
        {
            JSONResponse result = new JSONResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    result.success = AssinaturaSenhaSRV.LogarCliente(usuario);
                }
            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e, false, true);
                return result;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
                return result;
            }

            return result;
        }

        public JSONResponse RealizarLoginAt([FromBody] UsuarioAtc usuario)
        {
            JSONResponse result = new JSONResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    result.success = AssinaturaSenhaSRV.LogarCliente(usuario);
                }
            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e, false, true);
                return result;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
                return result;
            }

            return null;
        }


        [HttpGet("realizar-login-atc-get")]
        public JSONResponse RealizarLoginAtcGet([FromBody] UsuarioAtc usuario)
        {
            JSONResponse result = new JSONResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    result.success = AssinaturaSenhaSRV.LogarCliente(usuario);
                }
            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e, false, true);
                return result;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
                return result;
            }

            return null;
        }


        // POST: api/Vitrine
        [HttpPost("realizar-login")]
        public JSONResponse RealizarLogin([FromBody] UsuarioEcommerce usuario)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    result.success = ClienteUsuarioSRV.LogarClienteEcommerce(usuario);

                    if (result.success)
                    {
                        result.Add("autenticado", usuario);
                    }                    
                    result.message = Message.Info("Usuário logado com sucesso");

                    return result;

                }
                else
                {
                    result.success = false;
                    return result;
                }
            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e, false, true);
                return result;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
                return result;
            }
        }

		// POST: api/Vitrine
        [HttpPost("adicionar-usuario")]
        public JSONResponse AdicionarUsuario([FromBody] UsuarioEcommerce usuario)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    result.success = ClienteUsuarioSRV.AdicionarUsuarioEcommerce(usuario);

                    if (result.success)
                    {
                        result.Add("autenticado", usuario);
                    }
                    result.message = Message.Info("Cliente adicionado com sucesso");

                    return result;

                }
                else
                {
                    result.success = false;
                    return result;
                }
            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e, false, true);
                return result;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
                return result;
            }
        }


        // PUT: api/Vitrine/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public JSONResponse AdicionarUsuario(string login, string senha, int? cli_id)
        {

            JSONResponse result = new JSONResponse();
            UsuarioEcommerce usuario = new UsuarioEcommerce();

            try
            {

                usuario.login = login;
                usuario.senha = senha;
                usuario.cli_id = cli_id;


                if (ModelState.IsValid)
                {
                    result.success = ClienteUsuarioSRV.AdicionarUsuarioEcommerce(usuario);

                    if (result.success)
                    {
                        result.Add("autenticado", usuario);
                    }
                    result.message = Message.Info("Cliente adicionado com sucesso");

                    return result;

                }
                else
                {
                    result.success = false;
                    return result;
                }
            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e, false, true);
                return result;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
                return result;
            }
        }
    }
}
