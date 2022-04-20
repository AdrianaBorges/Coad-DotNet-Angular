using System;
using System.Collections.Generic;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg;
using COAD.CORPORATIVO.Service.Mundipagg;
using COAD.CORPORATIVO.Settings.Mundipagg;
using COADRESTSERVICE.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MundiAPI.PCL.Models;

namespace COADRESTSERVICE.Controllers.Mundipagg
{
    [Route("api/mundipagg/[controller]")]
    public class ClienteMundipaggController : MundipaggBaseController
    {
        public override MundipaggStt MundipaggStt { get; set; }
        private ClienteMundipaggSRV ClientMundipaggSRV { get; set; }

        public ClienteMundipaggController(IConfiguration configuration, ClienteMundipaggSRV clientMundipaggSRV) : base(configuration)
        {
            ClientMundipaggSRV = clientMundipaggSRV;
            ClientMundipaggSRV.MundipaggSTT = MundipaggStt;
        }

        public JSONResponse CriarCliente()
        {
            return null;
        }

        [HttpPost("cadastrar-cliente-mundipagg")]
        public JSONResponse CadastrarClienteMundipagg(CriarClienteMundipaggRequestDTO criarClienteRequestMundipaggDTO)
        {
            var result = new JSONResponse();
            try
            {
                var clienteMundipaggResponse = ClientMundipaggSRV.CadastrarClienteMundipagg(criarClienteRequestMundipaggDTO);

                var mock = new BuscarClienteMundipaggResponseDTO()
                {
                    GetCustomerResponse = new GetCustomerResponse()
                    {
                        Id = "cus_QA5V47r9c0Im3dzN",
                        Name = "Tony",
                        Email = "tonystarkk@avengers.com",
                        Code = "MY_CUSTOMER_001",
                        Document = "123456789",
                        Type = "individual",
                        Delinquent = false,
                        Address = new GetAddressResponse()
                        {
                            Id = "addr_KewjagEfrCbY1doZ",
                            Line1 = "375, Av. General Justo, Centro",
                            Line2 = "8º andar",
                            ZipCode = "20021130",
                            City = "Rio de Janeiro",
                            State = "RJ",
                            Country = "BR",
                            Status = "active",
                            CreatedAt = new DateTime(2017, 09, 22, 15, 36, 46, DateTimeKind.Utc),
                            UpdatedAt = new DateTime(2018, 04, 22, 15, 36, 46, DateTimeKind.Utc)
                        },
                        CreatedAt = new DateTime(2017, 09, 22, 15, 36, 46, DateTimeKind.Utc),
                        UpdatedAt = new DateTime(2018, 04, 22, 15, 36, 46, DateTimeKind.Utc),
                        Phones = new GetPhonesResponse()
                        {
                            HomePhone = new GetPhoneResponse()
                            {
                                CountryCode = "55",
                                Number = "000000000",
                                AreaCode = "21"
                            },
                            MobilePhone = new GetPhoneResponse()
                            {
                                CountryCode = "55",
                                Number = "000000000",
                                AreaCode = "21"
                            }
                        },
                        Metadata = new Dictionary<string, string>()
                        {
                            ["Id"] = "my_customer_id",
                            ["Company"] = "Avengers"
                        }
                    }
                };
                result.success = true;
                result.Add("data", mock);
            }
            catch (Exception e)
            {
                result.success = false;
                result.message = Message.Fail(e);
            }
            return result;
        }

        [HttpGet("buscar-cliente-mundipagg")]
        public JSONResponse BuscarClienteMundipagg(int? coadCliId)
        {
            var result = new JSONResponse();
            try
            {
                var clienteMundipaggResponse = ClientMundipaggSRV.BuscarClienteMundipagg(coadCliId);
                result.success = true;
            }
            catch (Exception e)
            {
                result.success = false;
                result.message = Message.Fail(e);
            }
            return result;
        }

    }
}