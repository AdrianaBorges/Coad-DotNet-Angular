using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(CARTEIRA))]
    public class CarteiraDTO
    { 
        public CarteiraDTO()
        {
            this.AGENDAMENTO = new HashSet<AgendamentoDTO>();
            this.CARTEIRA_ASSINATURA = new HashSet<CarteiraAssinaturaDTO>();
            this.CARTEIRA_CLIENTE = new HashSet<CarteiraClienteDTO>();
            //this.PRE_PEDIDO = new HashSet<PRE_PEDIDO>();
            this.CARTEIRA_REPRESENTANTE = new HashSet<CarteiraRepresentanteDTO>();

        }

        public string CAR_ID { get; set; }
        public string REGIAO_ID { get; set; }
        public string SEQ_REG { get; set; }
        public Nullable<int> AREA_ID { get; set; }
        public string REGIAO_UF { get; set; }
        public int CAR_VARIOS_REPRESENTANTES { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public Nullable<int> UEN_ID { get; set; }
        public Nullable<int> REP_REF { get; set; }
        public Nullable<int> RG_ID { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }

        /// <summary>
        ///  Esse atributo é uma representação do objeto no banco configsys
        /// Ou seja, é um atributo derivado de consultas suplementares
        /// </summary>
        public EmpresaModel EMPRESA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<AgendamentoDTO> AGENDAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CarteiraAssinaturaDTO> CARTEIRA_ASSINATURA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CarteiraClienteDTO> CARTEIRA_CLIENTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual EmpresaRefDTO EMPRESA_REF { get; set; }
        //public virtual ICollection<PRE_PEDIDO> PRE_PEDIDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RegiaoDTO REGIAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CarteiraRepresentanteDTO> CARTEIRA_REPRESENTANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual UENDTO UEN { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual UFDTO UF { get; set; }

    }
}
