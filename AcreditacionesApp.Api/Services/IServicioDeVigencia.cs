namespace AcreditacionesApp.Api.Services;

public enum EstadoVigencia
{
    NoIniciada,
    Vigente,
    PorVencer,
    Vencida
}

public interface IServicioDeVigencia
{
    EstadoVigencia Calcular(DateOnly fechaEmision, DateOnly fechaVencimiento);
}
