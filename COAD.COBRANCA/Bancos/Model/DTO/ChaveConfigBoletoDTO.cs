using COAD.COBRANCA.Bancos.Model.DTO.Interfaces;


namespace COAD.COBRANCA.Bancos.Model.DTO
{
    public class ChaveConfigBoletoDTO
    {
        public IBanco Banco { get; set; }
        public string CodigoCarteira { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is ChaveConfigBoletoDTO == false)
                return false;
            var other = obj as ChaveConfigBoletoDTO;

            if (other == null)
                return false;

            if (this.Banco != null && other.Banco == null)
                return false;
            if (this.Banco == null && other.Banco != null)
                return false;
            if (this.Banco.CodigoBanco != other.Banco.CodigoBanco)
                return false;
            if (this.CodigoCarteira != other.CodigoCarteira)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            if (this.Banco != null && this.Banco.CodigoBanco != null)
                hash = this.Banco.CodigoBanco.GetHashCode();
            if (this.CodigoCarteira != null)
                hash += this.CodigoCarteira.GetHashCode();

            return hash;

        }
    }
}
