namespace AcreditacionesApp.Api.Entities
{
    public class TipoAcreditacion
    {
        public int Id { get; set; }                         // EF detecta la clave primaria por llamarse "Id"
        public string Nombre { get; set; } = string.Empty;  // "= string.Empty" evita que nazca null
        public string? Descripcion { get; set; }            // "?" = la columna admite NULL
        public int VigenciaMeses { get; set; }
    }
}
