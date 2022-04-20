using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using System.Web;
using System.IO;
using GenericCrud.Service;
using GenericCrud.Validations;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("BAN_ID", "EMP_ID")]
    public class SequencialNossoNumeroSRV : GenericService<SEQUENCIAL_NOSSO_NUMERO, SequencialNossoNumeroDTO, object>
    {

        public SequencialNossoNumeroDAO _dao { get; set; }

        public SequencialNossoNumeroSRV(SequencialNossoNumeroDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public SequencialNossoNumeroDTO RetornarObjetoSequencialDoNossoNumero(string banId, int? empId)
        {
            var seq = FindById(banId, empId);
            return seq;
        }

        public int? RetornarSequencialDoNossoNumero(string banId, int? empId)
        {
            var seq = RetornarObjetoSequencialDoNossoNumero(banId, empId);

            if(seq == null)
            {
                var seqNovo = new SequencialNossoNumeroDTO()
                {
                    BAN_ID = banId,
                    EMP_ID = empId,
                    SQN_SEQUENCIAL = 0
                };
                seq = Save(seqNovo);
            }

            if(seq != null && seq.SQN_SEQUENCIAL != null)
            {
                var seqInt = seq.SQN_SEQUENCIAL;
                seqInt++;
                seq.SQN_SEQUENCIAL = seqInt;

                if (seq.SQN_LIMITE_FAIXA != null &&
                    seq.SQN_LIMITE_FAIXA > 0 &&
                    seq.SQN_SEQUENCIAL > seqInt)
                {
                    throw new Exception($"O nosso número tentou gerar o sequencial {seqInt}. Esse sequêncial é maior do que a faixa limite: {seq.SQN_SEQUENCIAL}. Contante o banco para solicitar uma faixa nova. Depois informe TI.");
                }
                Merge(seq);
                return seqInt;
            }

            return null;
        }
    }
}
