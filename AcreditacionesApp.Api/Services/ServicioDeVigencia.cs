namespace AcreditacionesApp.Api.Services;

public class ServicioDeVigencia: IServicioDeVigencia
{
    private const int DiasAvisoPorVencer = 30;  // const = valor fijo conocido al compilar
    private readonly IReloj _reloj;
    public ServicioDeVigencia(IReloj reloj) => _reloj = reloj;

    public EstadoVigencia Calcular(DateOnly fechaEmision, DateOnly fechaVencimiento)
    {
        var hoy = _reloj.Hoy;                                       // 1. ¿qué día es?

        if (hoy < fechaEmision) return EstadoVigencia.NoIniciada;   // 2. todav[ia no empieza
        if (hoy > fechaVencimiento) return EstadoVigencia.Vencida;  // 3. ya termin[o

        // DayNumber = d[ias transcurridos desde el a;o 1: restarlos da la diferencia en d[ias.
        var diasRestantes = fechaVencimiento.DayNumber - hoy.DayNumber;

        return diasRestantes <= DiasAvisoPorVencer
            ? EstadoVigencia.PorVencer                              // 4. faltan 30 d[ias o menos
            : EstadoVigencia.Vigente;                               // 5. con margen
    }
}
