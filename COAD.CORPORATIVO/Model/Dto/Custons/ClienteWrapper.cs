using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ClienteWrapper
    {
        public ClienteWrapper()
        {

        }

        public ClienteWrapper(ICliente cliente)
        {
            if (cliente is ClienteDto)
            {
                Cliente = cliente as ClienteDto;
            }

            if (cliente is ImportacaoSuspectDTO)
            {
                ImportacaoSuspect = cliente as ImportacaoSuspectDTO;
            }
        }

        public enum Tipo
        {
            CLIENTE = 0,
            SUSPECT_NAO_IMPORTADO = 1
        }

        public Tipo TipoObj { get; private set; }
        private ClienteDto cli;
        private ImportacaoSuspectDTO imporSus;
        
        public ClienteDto Cliente{ 
            get 
            { 
                return cli;
            }
            set 
            {
                this.TipoObj = Tipo.CLIENTE;
                cli = value;
            }
        }

        public ImportacaoSuspectDTO ImportacaoSuspect
        {
            get
            {
                return imporSus;
            }
            set
            {
                this.TipoObj = Tipo.SUSPECT_NAO_IMPORTADO;
                imporSus = value;
            }
        }

        public ICliente Value
        {
            get
            {
                if (TipoObj.Equals(Tipo.SUSPECT_NAO_IMPORTADO))
                    return (ICliente)ImportacaoSuspect;
                
                if (TipoObj.Equals(Tipo.CLIENTE))
                    return (ICliente)Cliente;
                return null;
            }
        }
    }
}
