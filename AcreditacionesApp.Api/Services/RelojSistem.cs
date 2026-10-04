namespace AcreditacionesApp.Api.Services;

// Implementaci[on Real: usa la fecha del sistema.
public class RelojSistem: IReloj
{
    // "=>" : cada vez que se lee Hoy, se calcula la fecha actual en UTC.
    public DateOnly Hoy => DateOnly.FromDateTime(DateTime.UtcNow);
}
