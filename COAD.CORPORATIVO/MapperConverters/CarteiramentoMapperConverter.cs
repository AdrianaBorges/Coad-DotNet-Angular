using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.DTOConversion;
using GenericCrud.DTOConversion.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.MapperConverters
{
    public class CarteiramentoMapperConverter : DTOConverter<CARTEIRA, CarteiraDTO>
    {
        
        public override void Convert(CARTEIRA objOrigem, CarteiraDTO objDestino, ConversionService<CARTEIRA, CarteiraDTO> conversionService)
        {
            //var representanteAtivo = objOrigem.CARTEIRA_REPRESENTANTE.Where(p => p.REPRESENTANTE.REP_ATIVO == 1).Select(op => op.REPRESENTANTE).FirstOrDefault();
            //var representanteNovo = objDestino.CARTEIRA_REPRESENTANTE.Where(op => op.REPRESENTANTE.REP_ATIVO == 1).Select(op => op.REPRESENTANTE).FirstOrDefault();


        }
    }
}
