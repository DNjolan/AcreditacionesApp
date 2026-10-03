namespace AcreditacionesApp.Api.Dtos
{
    public record CrearAcreditacionDto(
        int TipoAcreditacionId,
        string Nombre,
        string EntidadAcreditadora,
        DateOnly FechaEmision);
}
