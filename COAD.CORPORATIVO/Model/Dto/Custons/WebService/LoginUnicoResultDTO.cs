using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.WebService
{
    [DataContract]
    public class LoginUnicoResultDTO
    {

        [DataMember]
        public bool Sucesso { get; set; }
        
        [DataMember]
        public LoginUnicoMensagemDTO Mensagem { get; set; }

        [DataMember]
        public string Perfil { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int? Empresa { get; set; }
        [DataMember]
        public string Usuario { get; set; }
        [DataMember]
        public string Senha { get; set; }

        [DataMember]
        public LoginUnicoClienteDTO Cliente { get; set; }

        [DataMember]
        public string Permisao { get; set; }
        [DataMember]
        public string Conhecimento { get; set; }
        [DataMember]
        public string Public { get; set; }
        [DataMember]
        public DateTime? Vigencia { get; set; }
        [DataMember]
        public bool ReceberNovidade { get; set; }
        [DataMember]
        public bool Cadastrado { get; set; }
        [DataMember]
        public DateTime? Data1 { get; set; }
        [DataMember]
        public DateTime? Expiracao { get; set; }
        [DataMember]
        public string TipoUsuario { get; set; }
        [DataMember]
        public int? Contador { get; set; }
        [DataMember]
        public DateTime? DataCadastro { get; set; }
        [DataMember]
        public DateTime? DataAtualizacao { get; set; }
        [DataMember]
        public int? IdStacio { get; set; }
        [DataMember]
        public bool OabFlag { get; set; }
        [DataMember]
        public string OabNrInscricao { get; set; }
        [DataMember]
        public string OabStatus { get; set; }
        [DataMember]
        public DateTime? DataUltimoLogin { get; set; }
        [DataMember]
        public int? QtdSessoes { get; set; }
        [DataMember]
        public string OabNrFicha { get; set; }
        [DataMember]
        public DateTime? DataRepositorio { get; set; }
        [DataMember]
        public bool Pesquisa { get; set; }
        [DataMember]
        public bool LoginUnico { get; set; }

        [DataMember]
        public ICollection<LoginUnicoInfoAssinaturaDTO> Assinaturas { get; set; }

        public LoginUnicoResultDTO()
        {

        }
    }
}
