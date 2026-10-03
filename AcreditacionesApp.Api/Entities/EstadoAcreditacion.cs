namespace AcreditacionesApp.Api.Entities
{
    // enum = lista cerrada de valores. Evita "strings m[agicos" como "vigente".
    public enum EstadoAcreditacion
    {
        Pendiente = 1,
        EnRevision = 2,
        Vigente = 3,
        Vencida = 4,
        Rechazada = 5
    }
}
