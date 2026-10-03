namespace AcreditacionesApp.Api.Entities
{
    public class RequisitoAcreditacion
    {
        public int Id { get; set; }
        public int AcreditacionId { get; set; }                 // FK hacia la acreditaci[on
        public string Descripcion { get; set; } = string.Empty;
        public bool Obligatorio { get; set; }
        public bool Cumplido { get; set; }
        public DateOnly? FechaCumplimiento { get; set; }         // DateOnly? = fecha opcional
    }
}
