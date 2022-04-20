namespace COAD.COADGED.Model.DTO
{
    public class ConsultaEmailPerfilColecDTO
    {
        public string PC_ID { get; set; }
        public int COLEC_ID { get; set; }

        public virtual ConsultaEmailColecionadorDTO CONSULTA_EMAIL_COLECIONADOR { get; set; }
    }
}