using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.ClienteProspect;
using COAD.CORPORATIVO.Service;
using GenericCrud.Service;
using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenericCrud.Validations
{
    public class EnderecoMunicipioValidatorAttribute : ValidationAttribute
    { 
        
        public EnderecoMunicipioValidatorAttribute()
        {
            
        }

        public override bool IsValid(object value)
        {
            if (value != null && value is ClienteEnderecoDto)
            {
                var end = value as ClienteEnderecoDto;

                if(end != null)
                {
                    if (!string.IsNullOrWhiteSpace(end.END_UF) && (end.MUN_ID != null || end.MUNICIPIO != null))
                    {
                        if(end.MUN_ID != null && end.MUNICIPIO == null)
                        {
                            end.MUNICIPIO = ServiceFactory.RetornarServico<MunicipioSRV>().FindById(end.MUN_ID);
                        }

                        if(end.MUNICIPIO != null)
                        {
                            return (end.MUNICIPIO.UF == end.END_UF);
                        }
                    }
                }
            }
            else if (value != null && value is ClienteProspectEnderecoDTO)
            {
                var end = value as ClienteProspectEnderecoDTO;

                if (end != null)
                {
                    if (!string.IsNullOrWhiteSpace(end.UF) && (end.MunId != null || end.Munic != null))
                    {
                        if (end.MunId != null && end.Munic == null)
                        {
                            end.Munic = ServiceFactory.RetornarServico<MunicipioSRV>().FindById(end.MunId);
                        }

                        if (end.Munic != null)
                        {
                            return (end.Munic.UF == end.UF);
                        }
                    }
                }
            }
            return true;

        }
    }
}
