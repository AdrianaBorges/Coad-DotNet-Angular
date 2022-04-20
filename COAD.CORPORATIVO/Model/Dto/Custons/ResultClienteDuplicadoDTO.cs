using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ResultClienteDuplicadoDTO
   {
        public ResultClienteDuplicadoDTO()
        {
            this.ListDuplicationCPF_CNPJ = new List<int?>();
            this.ListDuplicationEmail = new List<AssinaturaEmailDTO>();
            this.ListDuplicationNome = new List<int?>();
            this.ListDuplicationTelefone = new List<AssinaturaTelefoneDTO>();
        }

        public bool Force { get; set; }
        public ValidacaoTypeResultDTO HasDuplicationNome {get; set;}    
        public IList<int?> ListDuplicationNome { get; set; }
        public ValidacaoTypeResultDTO HasDuplicationCPF_CNPJ { get; set; }
        public IList<int?> ListDuplicationCPF_CNPJ { get; set; }
        public ValidacaoTypeResultDTO HasDuplicationTelefone { get; set; }
        public IList<AssinaturaTelefoneDTO> ListDuplicationTelefone { get; set; }
        public ValidacaoTypeResultDTO HasDuplicationEmail { get; set; }
        public IList<AssinaturaEmailDTO> ListDuplicationEmail { get; set; }
        
        /// <summary>
        /// Inicializa as listas que estiverem nullas.
        /// </summary>
        public void CriarListasQuandoNulla()
        {
            if (ListDuplicationNome == null)
                ListDuplicationNome = new List<int?>();
            if (ListDuplicationCPF_CNPJ == null)
                ListDuplicationCPF_CNPJ = new List<int?>();
            if (ListDuplicationTelefone == null)
                ListDuplicationTelefone = new List<AssinaturaTelefoneDTO>();
            if (ListDuplicationEmail == null)
                ListDuplicationEmail = new List<AssinaturaEmailDTO>();
        }
        public bool HasDuplicationErrors { get
            {
                bool duplicidade = false;
                if(HasDuplicationNome != null)
                    duplicidade = duplicidade || (HasDuplicationNome.Falhou && HasDuplicationNome.Tipo == TipoValidacao.ERRO);
                if (HasDuplicationCPF_CNPJ != null)
                    duplicidade = duplicidade || (HasDuplicationCPF_CNPJ.Falhou && HasDuplicationCPF_CNPJ.Tipo == TipoValidacao.ERRO);
                if (HasDuplicationEmail != null)
                    duplicidade = duplicidade || (HasDuplicationEmail.Falhou && HasDuplicationEmail.Tipo == TipoValidacao.ERRO);
                if (HasDuplicationTelefone != null)
                    duplicidade = duplicidade || (HasDuplicationTelefone.Falhou && HasDuplicationTelefone.Tipo == TipoValidacao.ERRO);

                return duplicidade;
            }
            set { } }
        public bool HasDuplicationWarnings {
            get {

                bool duplicidade = false;
                if (HasDuplicationNome != null)
                    duplicidade = duplicidade || (HasDuplicationNome.Falhou && HasDuplicationNome.Tipo == TipoValidacao.WARNING);
                if (HasDuplicationCPF_CNPJ != null)
                    duplicidade = duplicidade || (HasDuplicationCPF_CNPJ.Falhou && HasDuplicationCPF_CNPJ.Tipo == TipoValidacao.WARNING);
                if (HasDuplicationEmail != null)
                    duplicidade = duplicidade || (HasDuplicationEmail.Falhou && HasDuplicationEmail.Tipo == TipoValidacao.WARNING);
                if (HasDuplicationTelefone != null)
                    duplicidade = duplicidade || (HasDuplicationTelefone.Falhou && HasDuplicationTelefone.Tipo == TipoValidacao.WARNING);

                return duplicidade;
            }
            set { }
        }

        public bool HasDuplication
        {
            get
            {

                if (HasDuplicationErrors)
                    return true;
                else
                if (HasDuplicationWarnings && !Force)
                    return true;
                else
                if (HasDuplicationWarnings && Force)
                    return false;

                return false;
            }
            set { }
        }
        public string ErrorMessage { get; set; }

        // Implementar ToString();
    }
}
