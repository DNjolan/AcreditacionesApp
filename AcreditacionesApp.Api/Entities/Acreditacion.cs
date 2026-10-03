namespace AcreditacionesApp.Api.Entities
{
    public class Acreditacion
    {
        public int Id { get; set; }
        public int TipoAcreditacionId { get; set; }             // clave for[anea (EF la reconoce por el nombre)
        public TipoAcreditacion Tipo { get; set; } = null!;     // propiedad de navegaci[on: EF la llena con include
        public string Nombre { get; set; } = string.Empty;
        public string EntidadAcreditadora { get; set; } = string.Empty;
        public DateOnly FechaEmision { get; set; }              // DateOnly = solo fecha, sin hora
        public DateOnly FechaVencimiento { get; set; }
        public EstadoAcreditacion Estado { get; set; }
        public List<RequisitoAcreditacion> Requisitos { get; set; } = new();    // relaci[on 1 a muchos
    }
}
