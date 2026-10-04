using AcreditacionesApp.Api.Services;
using Xunit;

namespace AcreditacionesApp.Tests;

public class RelojFalso: IReloj
{
    public DateOnly Hoy { get; set; }
}

public class ServicioDeVigenciaTests
{
    private static readonly DateOnly Hoy = new(2026, 11, 15);
    private static readonly DateOnly Emision = new(2026, 1, 1);

    private static ServicioDeVigencia CrearServicio()
        => new(new RelojFalso { Hoy = Hoy });

    [Fact] // [Fact] = "Esto es un test"
    public void Calcular_CuandoFaltan10Dias_DevuelvePorVencer()
    {
        // Arrange (preparar)
        var servicio = CrearServicio();
        var vencimiento = Hoy.AddDays(10);

        // ACT (actuar)
        var estado = servicio.Calcular(Emision, vencimiento);

        // ASSERT (verificar)
        Assert.Equal(EstadoVigencia.PorVencer, estado);
    }

    [Fact]
    public void Calcular_CuandoYaPasoElVencimiento_DevuelveVencida()
    {
        var servicio = CrearServicio();
        var estado = servicio.Calcular(Emision, Hoy.AddDays(-1));

        Assert.Equal(EstadoVigencia.Vigente, estado);
    }

    [Fact]
    public void Calcular_CuandoLaEmisionEsFutura_DevuelveNoIniciada()
    {
        var servicio = CrearServicio();
        var estado = servicio.Calcular(Hoy.AddDays(5), Hoy.AddDays(400));

        Assert.Equal(EstadoVigencia.NoIniciada, estado);
    }

    // [Theory] + [InlineData]: el mismo test con vaios casos (como test.each en jest).
    [Theory]
    [InlineData(0, EstadoVigencia.PorVencer)]       // vence hoy: todav[ia vale
    [InlineData(30, EstadoVigencia.PorVencer)]      // justo en el l[imite
    [InlineData(31, EstadoVigencia.Vigente)]        // un d[ia m[as y deja de avisar
    [InlineData(365, EstadoVigencia.Vigente)]
    public void Calcular_ConDistintosDiasRestantes_RespetaLosLimites(
        int diasRestantes, EstadoVigencia esperado)
    {
        var servicio = CrearServicio();
        var estado = servicio.Calcular(Emision, Hoy.AddDays(diasRestantes));

        Assert.Equal(esperado, estado);
    }
}
